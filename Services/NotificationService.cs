using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Pushification.Manager;
using Pushification.Models;
using Pushification.PuppeteerDriver;
using Pushification.Services.Interfaces;

namespace Pushification.Services
{
    public class NotificationService : IServiceWorker
    {
        private SubscriptionModeSettings _subscribeSettings = null;
        private readonly PushNotificationModeSettings _notificationModeSettings;

        private IWebDriver _driver;

        private Timer timer;
        private bool stopTimer;
        private bool _isRunning = true;
        public event EventHandler<string> UpdateUIMessage;

        private readonly object lockObject = new object();
        private TaskCompletionSource<bool> runWithAppModeCompletionSource = null;
        public NotificationService()
        {
            _subscribeSettings = SubscriptionModeSettings.LoadSubscriptionSettingsFromJson();
            _notificationModeSettings = PushNotificationModeSettings.LoadFromJson();
        }

        // Точка входа
        public async Task Run()
        {
            StartTimer();
            await RunWithAppModeAsync();
        }


        public async Task RunWithAppModeAsync()
        {
            EventPublisherManager.RaiseUpdateUIMessage($"Приступаю к принятию уведомлений");

            Random random = new Random();

            List<string> profiles = null;

            do
            {
                profiles = ProfilesManager.GetAllProfiles();

                if (profiles == null || profiles.Count == 0)
                {
                    EventPublisherManager.RaiseUpdateUIMessage("Не удалось получить список профилей, " +
                                                                "возможно он пуст, перехожу в режим подписки на уведомления");

                    CloseBrowser();

                    SubscribeService subscribeService = new SubscribeService();
                    await subscribeService.Run();

                    profiles = ProfilesManager.GetAllProfiles();
                }

                profiles?.Sort((p1, p2) => File.GetCreationTime(p1).CompareTo(File.GetCreationTime(p2)));

                foreach (string profilePath in profiles)
                {
                    string userAgent = UserAgetManager.GetRandomUserAgent();

                    if (!_isRunning)
                        return;

                    // Определяем режим для профиля и выполняем соответствующий метод
                    if (random.NextDouble() * 100 < _notificationModeSettings.PercentToDelete)
                    {
                        EventPublisherManager.RaiseUpdateUIMessage("Режим Delete -------------------------------------------");
                        await RunDeleteModeAsync(profilePath, userAgent);
                    }
                    else
                    {

                        if (random.NextDouble() * 100 < _notificationModeSettings.PercentToIgnore)
                        {
                            EventPublisherManager.RaiseUpdateUIMessage("Режим Ignore -------------------------------------------");
                            await RunIgnoreModeAsync(profilePath, userAgent);
                        }
                        else
                        {
                            EventPublisherManager.RaiseUpdateUIMessage("Режим Click -------------------------------------------");
                            await RunClickModeAsync(profilePath, userAgent);
                        }
                    }
                }

            } while (_isRunning);

            // Сигнализируем о завершении RunWithAppModeAsync
            runWithAppModeCompletionSource?.TrySetResult(true);
        }

        private void StartTimer()
        {
            TimeSpan startTime = TimeSpan.Parse(_subscribeSettings.StartOptionOne);
            DateTime currentDate = DateTime.Now;
            DateTime targetTime = currentDate.Date.Add(startTime);

            if (currentDate.TimeOfDay > startTime)
            {
                targetTime = targetTime.AddDays(1);
            }

            TimeSpan timeUntilStart = targetTime - DateTime.Now;

            if (timeUntilStart.TotalMilliseconds > 0)
            {
                timer = new Timer(TimerCallback, null, (int)timeUntilStart.TotalMilliseconds, Timeout.Infinite);
            }
        }

        private async void TimerCallback(object state)
        {
            CloseBrowser();
            timer.Dispose();

            _isRunning = false;

            // Создаем новый TaskCompletionSource для следующей итерации RunWithAppModeAsync
            runWithAppModeCompletionSource = new TaskCompletionSource<bool>();

            try
            {
                // Ждем завершения RunWithAppModeAsync перед запуском subscribeService.Run()
                await RunWithAppModeAsync();

                SubscribeService subscribeService = new SubscribeService();

                // Запускаем subscribeService.Run(), дожидаемся его завершения
                await subscribeService.Run();

                // Завершение RunWithAppModeAsync
                runWithAppModeCompletionSource.TrySetResult(true);
            }
            finally
            {
                _isRunning = true;

                // Дожидаемся завершения RunWithAppModeAsync перед стартом таймера
                await runWithAppModeCompletionSource.Task;

                // Запускаем новую итерацию RunWithAppModeAsync
                await RunWithAppModeAsync();
                StartTimer();
            }
        }

        // Режим Ignore
        private async Task RunIgnoreModeAsync(string profilePath, string userAgent)
        {
            // Использовать прокси или нет
            bool isUseProxy = _notificationModeSettings.ProxyForIgnore;

            // Получаю прокси
            string proxyFilePath = _subscribeSettings.ProxyList;
            ProxyInfo proxyInfo = ProxyInfo.GetRandomProxy(proxyFilePath); //  ProxyInfo.GetRandomProxy(proxyFilePath)
            string url = _subscribeSettings.URL;

            try
            {
                _driver = DriverManager.CreateDriver(profilePath, isUseProxy ? proxyInfo : null, userAgent: userAgent, useHeadlessMode: _notificationModeSettings.HeadlessMode);
            }
            catch (Exception ex)
            {
                EventPublisherManager.RaiseUpdateUIMessage($"Не удалось перейти по адресу : {ex.Message}");
                return;
            }

            IntPtr handle = IntPtr.Zero;

            // Время ожидания уведомлений
            int maxTimeToWaitNotificationIgnoreInSeconds = _notificationModeSettings.MaxTimeToWaitNotificationIgnore;
            DateTime startTime = DateTime.Now;

            int sleepBeforeProcessKillIgnore = _notificationModeSettings.SleepBeforeProcessKillIgnore * 1000;

            // Ожидаю уведомления
            handle = GetNotificationWindow(maxTimeToWaitNotificationIgnoreInSeconds);
            if (handle == IntPtr.Zero)
            {
                CloseBrowser();
                return;
            }

            if (_notificationModeSettings.NotificationCloseByButton)
            {
                while (handle != IntPtr.Zero || handle != null)
                {
                    handle = FindNotificationToast();
                    CloseNotificationToast(handle);
                    EventPublisherManager.RaiseUpdateUIMessage($"Закрыл окно push");
                    await Task.Delay(500);
                }
            }

            await Task.Delay(sleepBeforeProcessKillIgnore);
            CloseBrowser();
        }

        // Режим кликов по уведомлениям
        private async Task RunClickModeAsync(string profilePath, string userAgent)
        {
            // Получаю прокси
            string proxyFilePath = _subscribeSettings.ProxyList;
            ProxyInfo proxyInfo = ProxyInfo.GetRandomProxy(proxyFilePath); //ProxyInfo.GetRandomProxy(proxyFilePath)

            if (proxyInfo == null)
                return;

            // Получаю драйвер, открываю страницу
            try
            {
                _driver = DriverManager.CreateDriver(profilePath, proxyInfo: proxyInfo, userAgent: userAgent, useHeadlessMode: _notificationModeSettings.HeadlessMode);
            }
            catch (Exception ex)
            {
                EventPublisherManager.RaiseUpdateUIMessage($"Не удалось перейти по адресу : {ex.Message}");
                return;
            }


            // Получаю рандомное число для закрытия по крестику
            Random random = new Random();
            int minClickCount = _notificationModeSettings.MinNumberOfClicks;
            int maxClickCount = _notificationModeSettings.MaxNumberOfClicks;
            int randomClickByPush = random.Next(minClickCount, maxClickCount);

            EventPublisherManager.RaiseUpdateUIMessage($"Будет сделано кликов : {randomClickByPush}");

            // Время ожидания между кликами
            int sleepBetweenClick = _notificationModeSettings.SleepBetweenClick * 1000;
            for (int i = 0; i < randomClickByPush; i++)
            {
                IntPtr handle = GetNotificationWindow(_notificationModeSettings.TimeToWaitNotificationClick);
                if (handle == IntPtr.Zero)
                {
                    CloseBrowser();
                    return;
                }

                ClickByPush(handle);
                EventPublisherManager.RaiseUpdateUIMessage($"Выполнил click по push  : {i + 1} раз");
                await Task.Delay(sleepBetweenClick);
            }

            // Задержка после кликов по уведомлениям
            int sleepAfterAllNotificationsClickMs = _notificationModeSettings.SleepAfterAllNotificationsClick * 1000;
            await Task.Delay(sleepAfterAllNotificationsClickMs);

            CloseBrowser();
        }

        // Метод удаления профиля
        private async Task RunDeleteModeAsync(string profilePath, string userAgent)
        {
            try
            {
                _driver = DriverManager.CreateDriver(profilePath, userAgent: userAgent, disableNotifivation: true);

                int sleepBeforeUnsubscribeMS = _notificationModeSettings.SleepBeforeUnsubscribe * 1000;
                await Task.Delay(sleepBeforeUnsubscribeMS);
                _driver.Navigate().GoToUrl("chrome://settings/content/all/");

                IWebElement button;

                // Находим элемент с помощью JavaScript
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)_driver;
                Actions actions = new Actions(_driver);

                string deleteBtn = @"
                return document.querySelector('body settings-ui')
                .shadowRoot.querySelector('#main')
                .shadowRoot.querySelector('settings-basic-page')
                .shadowRoot.querySelector('#basicPage settings-section.expanded settings-privacy-page')
                .shadowRoot.querySelector('#pages settings-subpage all-sites')
                .shadowRoot.querySelector('#clearAllButton cr-button');
                ";

                button = (IWebElement)jsExecutor.ExecuteScript(deleteBtn);
                actions.Click(button).Build().Perform();


                string confirmBnt = @"
                return document.querySelector('body settings-ui')
                .shadowRoot.querySelector('#main')
                .shadowRoot.querySelector('settings-basic-page')
                .shadowRoot.querySelector('#basicPage settings-section.expanded settings-privacy-page')
                .shadowRoot.querySelector('#pages settings-subpage all-sites')
                .shadowRoot.querySelector('cr-dialog > div:nth-child(3) > cr-button.action-button');
                ";

                button = (IWebElement)jsExecutor.ExecuteScript(confirmBnt);
                actions.Click(button).Build().Perform();

                int sleepAfterUnsubscribe = _notificationModeSettings.SleepAfterUnsubscribe * 1000;
                await Task.Delay(sleepAfterUnsubscribe);

                CloseBrowser();

                int sleepBeforeProfileDeletion = _notificationModeSettings.SleepBeforeProfileDeletion * 1000;
                await Task.Delay(sleepBeforeProfileDeletion);

                EventPublisherManager.RaiseUpdateUIMessage($"Удаляю профиль : {profilePath}");
                ProfilesManager.RemoveProfile(profilePath);
            }
            catch (Exception ex)
            {
                EventPublisherManager.RaiseUpdateUIMessage($"Ошибка в режиме удаления : {ex.Message}");
            }
        }


        // Ожидаю окно уведомлений
        private IntPtr GetNotificationWindow(int timeout)
        {
            IntPtr handle = IntPtr.Zero;

            // Время ожиданий уведомлений
            int timeToWaitNotificationClick = timeout;
            DateTime startTime = DateTime.Now;

            // Ожидаю уведомления
            while (handle == IntPtr.Zero)
            {
                if (!((DateTime.Now - startTime).TotalSeconds < timeToWaitNotificationClick))
                {
                    break;
                }

                handle = FindNotificationToast();
                Thread.Sleep(500);
            }

            return handle;
        }



        // Остановка работы
        public void CloseBrowser()
        {
            // Закрыть браузер после прошествия времени
            try
            {
                _driver.Quit();
                _driver.Dispose();
            }
            catch (Exception) { }

            // Удаляю лишние папки и файлы из профиля
            Thread.Sleep(2000);
            ProfilesManager.RemoveCash();
        }

        // Ищу и кликаю на уведомлении
        public void ClickByPush(IntPtr handle)
        {

            if (handle != IntPtr.Zero)
            {
                // Активируем окно
                SetForegroundWindow(handle);

                // Ожидаем некоторое время для активации окна
                Thread.Sleep(500);

                // Получаем координаты окна
                GetWindowRect(handle, out RECT windowRect);

                // Выполняем клик в центре окна
                int centerX = (windowRect.Left + windowRect.Right) / 2;
                int centerY = (windowRect.Top + windowRect.Bottom) / 2;
                MouseClick(centerX, centerY);
            }
            else
            {
                EventPublisherManager.RaiseUpdateUIMessage("Не удалось кликнуть по push");
                // TODO логирование
                CloseBrowser();
            }

        }

        // Метод получения окна toast
        private IntPtr FindNotificationToast()
        {
            List<AutomationElement> chromeWindows = FindWindowsByClassName("Chrome_WidgetWin_1");

            foreach (var chromeWindow in chromeWindows)
            {
                if (chromeWindow == null) continue;

                // Фильтруем окна без Name и AutomationId
                try
                {
                    if (string.IsNullOrEmpty(chromeWindow?.Current.Name) && string.IsNullOrEmpty(chromeWindow?.Current.AutomationId))
                    {
                        EventPublisherManager.RaiseUpdateUIMessage($"Получил окно push");
                        return new IntPtr(chromeWindow.Current.NativeWindowHandle);
                    }
                }
                catch (Exception) { }
            }

            return IntPtr.Zero;
        }

        // Получаю все окна
        private List<AutomationElement> FindWindowsByClassName(string className)
        {
            Condition condition = new PropertyCondition(AutomationElement.ClassNameProperty, className);
            AutomationElementCollection elementCollection = AutomationElement.RootElement.FindAll(TreeScope.Children, condition);
            return new List<AutomationElement>(elementCollection.Cast<AutomationElement>());
        }

        // Наведение курсора и клик
        private void MouseClick(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
            Thread.Sleep(1000);
            //AutoItX.MouseClick(button: "LEFT", x: x, y: y, numClicks: 1, speed: 2);
        }

        // Закрываю окно
        public void CloseNotificationToast(IntPtr handle)
        {
            //IntPtr handle = FindNotificationToast();

            if (handle != IntPtr.Zero)
            {
                // Отправляем сообщение WM_CLOSE для попытки закрытия окна
                SendMessage(handle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

                EventPublisherManager.RaiseUpdateUIMessage($"Закрыл окно push");

            }
            else
            {
                EventPublisherManager.RaiseUpdateUIMessage("Окно не найдено.");
            }
        }


        // Импорт зависимостей из библиотеки

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // Коммманды
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const uint WM_CLOSE = 0x0010;
    }
}
