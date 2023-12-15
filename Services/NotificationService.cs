using Pushification.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace Pushification.Services
{
    public class NotificationService : IServiceWorker
    {
        public void Run()
        {
            IntPtr handle = FindChromeWindowWithoutNameAndAutomationId();

            if (handle != IntPtr.Zero)
            {
                // Активируем окно
                SetForegroundWindow(handle);

                // Ожидаем некоторое время для активации окна
                Thread.Sleep(1000);

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

        public async Task StopAsync(string profilePath)
        {
            throw new System.NotImplementedException();
        }

        public void ClickByPush()
        {
            // Ваш код для выполнения клика
        }

        private IntPtr FindChromeWindowWithoutNameAndAutomationId()
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

        private List<AutomationElement> FindWindowsByClassName(string className)
        {
            Condition condition = new PropertyCondition(AutomationElement.ClassNameProperty, className);
            AutomationElementCollection elementCollection = AutomationElement.RootElement.FindAll(TreeScope.Children, condition);
            return new List<AutomationElement>(elementCollection.Cast<AutomationElement>());
        }

        // Добавленные методы для выполнения клика
        private void MouseClick(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }

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

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
    }
}
