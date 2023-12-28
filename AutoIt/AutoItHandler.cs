using AutoIt;
using Pushification.Manager;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

public class AutoItHandler
{
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, IntPtr dwExtraInfo);

    private const int MOUSEEVENTF_LEFTDOWN = 0x02;
    private const int MOUSEEVENTF_LEFTUP = 0x04;

    private const int SW_RESTORE = 9;

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
    public static bool SubscribeToWindow(int waitingBeforeClick)
    {
        string targetClass = "SysShadow";

        IntPtr targetWindowHandle = IntPtr.Zero;

        // Указанное в настройках время ожидания перед кликом
        int waitingBeforeClickMilliseconds = waitingBeforeClick * 1000;
        Thread.Sleep(waitingBeforeClickMilliseconds);

        EnumWindows((hWnd, lParam) =>
        {
            StringBuilder className = new StringBuilder(256);
            GetClassName(hWnd, className, className.Capacity);

            if (className.ToString() == targetClass)
            {
                targetWindowHandle = hWnd;
                return false; // Остановка перечисления окон
            }

            return true;
        }, IntPtr.Zero);

        if (targetWindowHandle != IntPtr.Zero)
        {
            // Показываем и активируем окно
            ShowWindow(targetWindowHandle, SW_RESTORE);
            SetForegroundWindow(targetWindowHandle);         

            // Координаты, где нужно кликнуть
            int x = 247;
            int y = 172;

            // Установка позиции курсора
            SetCursorPos(x, y);

            Thread.Sleep(1000);
            // Выполняем клик
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, IntPtr.Zero);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, IntPtr.Zero);

            return true;
        }
        else
        {
            EventPublisherManager.RaiseUpdateUIMessage("Не удалось получить окно или подписаться на уведомления");
            return false;
        }

        return false;
    }

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
}
