using AutoIt;
using PuppeteerSharp;
using Pushification.Manager;
using Pushification.Models;
using Pushification.PuppeteerDriver;
using Pushification.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace Pushification.Services
{
    public class NotificationService : IServiceWorker
    {
        private SubscriptionModeSettings _subscribeSettings = null;
        private readonly PushNotificationModeSettings _notificationModeSettings;
        private IBrowser _browser = null;
        private IPage _page = null;
        private Timer timer;
        private bool stopTimer;
        private bool _isRunning = true;
        public event EventHandler<string> UpdateUIMessage;

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
          
            while (_isRunning)
            {
                List<string> profiles = ProfilesManager.GetAllProfiles();
                profiles.Sort((p1, p2) => File.GetCreationTime(p1).CompareTo(File.GetCreationTime(p2)));
                string userAgent = UserAgetManager.GetRandomUserAgent();

                foreach (string profilePath in profiles)
                {
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

            }
        }

        // Запускаю таймер
        private void StartTimer()
        {
            TimeSpan startTime = TimeSpan.Parse(_subscribeSettings.StartOptionOne);
            DateTime currentDate = DateTime.Now;
            DateTime targetTime = currentDate.Date.Add(startTime);

            // Если указанное время уже прошло для текущего дня, добавляем 1 день
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

        // Если срабатывает таймер
        private async void TimerCallback(object state)
        {
            await StopAsync();
            // Остановить таймер после выполнения кода
            timer.Dispose();

            _isRunning = false;
            SubscribeService subscribeService = new SubscribeService();
            await subscribeService.Run();

            await RunWithAppModeAsync();
            StartTimer();                      
        }


        // Режим Ignore
        private async Task RunIgnoreModeAsync(string profilePath, string userAgent)
        {
            // Использовать прокси или нет
            bool isUseProxy = _notificationModeSettings.ProxyForIgnore;

            // Получаю прокси
            string proxyFilePath = _subscribeSettings.ProxyList;
            ProxyInfo proxyInfo = await ProxyInfo.GetProxy(proxyFilePath, 10, true);

            // Получаю драйвер, открываю страницу
            _browser = await DriverManager.CreateDriver(profilePath, isUseProxy ? proxyInfo : null, userAgent: userAgent, useHeadlessMode: _notificationModeSettings.HeadlessMode);
            _page = await _browser.NewPageAsync();

            IntPtr handle = IntPtr.Zero;

            // Время ожиданий уведомлений
            int maxTimeToWaitNotificationIgnoreInSeconds = _notificationModeSettings.MaxTimeToWaitNotificationIgnore;
            DateTime startTime = DateTime.Now;

            // Ожидаю уведомления
            while (handle == IntPtr.Zero || (DateTime.Now - startTime).TotalSeconds < maxTimeToWaitNotificationIgnoreInSeconds)
            {
                handle = FindNotificationToast();
                await Task.Delay(1000);
            }
          

            // Ожидаю перед закрытием
            int sleepBeforeProcessKillIgnore = _notificationModeSettings.SleepBeforeProcessKillIgnore * 1000;
            await Task.Delay(sleepBeforeProcessKillIgnore);

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

            await StopAsync();
        }

        // Режим кликов по уведомлениям
        private async Task RunClickModeAsync(string profilePath, string userAgent)
        {
            EventPublisherManager.RaiseUpdateUIMessage($"Получаю прокси");
            // Получаю прокси
            string proxyFilePath = _subscribeSettings.ProxyList;
            ProxyInfo proxyInfo = await ProxyInfo.GetProxy(proxyFilePath, 10, true);
            EventPublisherManager.RaiseUpdateUIMessage($"Получил прокси");
            // Получаю драйвер, открываю страницу
            _browser = await DriverManager.CreateDriver(profilePath, proxyInfo, userAgent: userAgent, useHeadlessMode: _notificationModeSettings.HeadlessMode);
            _page = await _browser.NewPageAsync();

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
                IntPtr handle = GetNotificationWindow();
                if (handle == IntPtr.Zero)
                {                    
                    await StopAsync();
                    break;
                }      

                ClickByPush(handle);
                EventPublisherManager.RaiseUpdateUIMessage($"Выполнил click по push  : {i + 1} раз");
                await Task.Delay(sleepBetweenClick);
            }

            // Задержка после кликов по уведомлениям
            int sleepAfterAllNotificationsClickMs = _notificationModeSettings.SleepAfterAllNotificationsClick * 1000;
            await Task.Delay(sleepAfterAllNotificationsClickMs);

            await StopAsync();
        }

        // Метод удаления профиля
        private async Task RunDeleteModeAsync(string profilePath, string userAgent)
        {
            int sleepBeforeUnsubscribeMS = _notificationModeSettings.SleepBeforeUnsubscribe * 1000;
            await Task.Delay(sleepBeforeUnsubscribeMS);
            EventPublisherManager.RaiseUpdateUIMessage($"Удаляю профиль : {profilePath}");
            ProfilesManager.RemoveProfile(profilePath);
        }

        // Ождаю окно уведомлений
        private IntPtr GetNotificationWindow()
        {
            IntPtr handle = IntPtr.Zero;

            // Время ожиданий уведомлений
            int timeToWaitNotificationClick = _notificationModeSettings.TimeToWaitNotificationClick;
            DateTime startTime = DateTime.Now;
                        
            // Ожидаю уведомления
            while (handle == IntPtr.Zero )
            {
                if (!((DateTime.Now - startTime).TotalSeconds < timeToWaitNotificationClick)) 
                {
                    EventPublisherManager.RaiseUpdateUIMessage("Не удалось получить окно push, истекло время ожидания"); 
                    break;
                }               

                handle = FindNotificationToast();
                Thread.Sleep(1000);
            }

            return handle;
        }

        // Остановка работы
        public async Task StopAsync()
        {
            // Закрыть браузер после прошествия времени
            await _browser.CloseAsync();
            await _page.DisposeAsync();

            // Удаляю лишние папки и файлы из профиля
            await Task.Delay(500);
            ProfilesManager.RemoveCash();
        }

        // Ищу и кликаю на уведомлении
        public async void ClickByPush(IntPtr handle)
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
                await StopAsync();
            }

        }

        // Метод получения окна toast
        private IntPtr FindNotificationToast()
        {
            List<AutomationElement> chromeWindows = FindWindowsByClassName("Chrome_WidgetWin_1");

            foreach (var chromeWindow in chromeWindows)
            {
                // Фильтруем окна без Name и AutomationId
                if (string.IsNullOrEmpty(chromeWindow?.Current.Name) && string.IsNullOrEmpty(chromeWindow?.Current.AutomationId))
                {
                    return new IntPtr(chromeWindow.Current.NativeWindowHandle);
                }
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
                Console.WriteLine("Окно не найдено.");
            }
        }

        // Метод, вызывающий событие
        private void RaiseUpdateUIMessage(string message)
        {
            UpdateUIMessage?.Invoke(this, message);
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
