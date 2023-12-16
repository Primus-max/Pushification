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


        private bool _isRunning = true;

        public NotificationService()
        {
            _subscribeSettings = SubscriptionModeSettings.LoadSubscriptionSettingsFromJson();
            _notificationModeSettings = PushNotificationModeSettings.LoadFromJson();
        }

        // Точка входа
        public async Task Run()
        {

            await RunWithAppModeAsync();
        }


        public async Task RunWithAppModeAsync()
        {            
            Random random = new Random();

            while (_isRunning)
            {
                List<string> profiles = ProfilesManager.GetAllProfiles();
                profiles.Sort((p1, p2) => File.GetCreationTime(p1).CompareTo(File.GetCreationTime(p2)));

                foreach (string profilePath in profiles)
                {
                    // Определяем режим для профиля и выполняем соответствующий метод
                    if (random.NextDouble() * 100 < _notificationModeSettings.PercentToDelete)
                    {
                        await RunDeleteModeAsync(profilePath);
                    }
                    else
                    {                       

                        if (random.NextDouble() * 100 < _notificationModeSettings.PercentToIgnore)
                        {
                            await RunIgnoreModeAsync(profilePath);
                        }
                        else
                        {
                            await RunClickModeAsync(profilePath);
                        }
                    }

                    // Удаляем обработанный профиль из списка
                    profiles.Remove(profilePath);
                }

            }
        }

        // Метод выбора режима и запуска методов
        private async Task RunIgnoreModeAsync(string profilePath)
        {
            // Использовать прокси или нет
            bool isUseProxy = _notificationModeSettings.ProxyForIgnore;

            // Получаю прокси
            string proxyFilePath = _subscribeSettings.ProxyList;
            ProxyInfo proxyInfo = await ProxyInfo.GetProxy(proxyFilePath, 10, true);

            // Получаю драйвер, открываю страницу
            _browser = await DriverManager.CreateDriver(profilePath, isUseProxy ? proxyInfo : null);
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

            if(_notificationModeSettings.NotificationCloseByButton)
            {
                while (handle != IntPtr.Zero || handle != null)
                {
                    handle = FindNotificationToast();
                    CloseNotificationToast(handle);

                    await Task.Delay(500);
                }
            }   

          await  StopAsync();
        }

        private async Task RunClickModeAsync(string profilePath)
        {
            // Получаю прокси
            string proxyFilePath = _subscribeSettings.ProxyList;
            ProxyInfo proxyInfo = await ProxyInfo.GetProxy(proxyFilePath, 10, true);

            _browser = await DriverManager.CreateDriver(profilePath, proxyInfo);
            _page = await _browser.NewPageAsync();

            // Получаю рандомное число для закрытия по крестику
            Random random = new Random();
            int minClickCount = _notificationModeSettings.MinNumberOfClicks;
            int maxClickCount = _notificationModeSettings.MaxNumberOfClicks;
            int randomClickByPush = random.Next(minClickCount, maxClickCount);

            IntPtr handle = FindNotificationToast(); // Получаю окно toast
            if (handle == null)
            {
                // TODO здесь будет длогирование
                return;
            }

            ClickByPush(handle);
        }

        // Метод удаления профиля
        private async Task RunDeleteModeAsync(string profilePath)
        {
            await Task.Delay(_notificationModeSettings.SleepBeforeProfileDeletion);
            ProfilesManager.RemoveProfile(profilePath);
        }

        // Метод опредения режима работы
        //private WorkMode DetermineWorkMode()
        //{
        //    Random random = new Random();
        //    double randomValue = random.NextDouble() * 100; // Умножаем на 100, чтобы получить значение в диапазоне от 0 до 100.

        //    if (randomValue < _notificationModeSettings.PercentToDelete)
        //    {
        //        return WorkMode.Delete;
        //    }
        //    else if (randomValue < _notificationModeSettings.PercentToDelete + _notificationModeSettings.PercentToIgnore)
        //    {
        //        return WorkMode.Ignore;
        //    }
        //    else
        //    {
        //        return WorkMode.Click;
        //    }
        //}


        // Остановка работы
        public async Task StopAsync()
        {
            // Закрыть браузер после прошествия времени
            await _browser.CloseAsync();
            await _page.DisposeAsync();

            // Удаляю лишние папки и файлы из профиля
            await Task.Delay(500);
            ProfilesManager.RemoveCash();

            _isRunning = !_isRunning;
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

                Console.WriteLine("Действие выполнено успешно.");
            }
            else
            {
                Console.WriteLine("Окно не найдено.");
            }

        }

        // Метод получения окна toast
        private IntPtr FindNotificationToast()
        {
            List<AutomationElement> chromeWindows = FindWindowsByClassName("Chrome_WidgetWin_1");

            foreach (var chromeWindow in chromeWindows)
            {
                // Фильтруем окна без Name и AutomationId
                if (string.IsNullOrEmpty(chromeWindow.Current.Name) && string.IsNullOrEmpty(chromeWindow.Current.AutomationId))
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
        }

        // Закрываю окно
        public void CloseNotificationToast(IntPtr handle)
        {
            //IntPtr handle = FindNotificationToast();

            if (handle != IntPtr.Zero)
            {
                // Отправляем сообщение WM_CLOSE для попытки закрытия окна
                SendMessage(handle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

                Console.WriteLine("Попытка закрытия окна выполнена.");
            }
            else
            {
                Console.WriteLine("Окно не найдено.");
            }
        }

        private enum WorkMode
        {
            Ignore,
            Click,
            Delete
        }

        // Импорт зависимостей из библиотеки
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
