using AutoIt;
using FlaUI.Core.AutomationElements;
using Pushification.Manager;
using System;
using System.Threading;


namespace Pushification.AutoIt
{
    public static class ProxyAuth
    {
        public static void Run(string username, string password)
        {
            // Ожидаем активации окна по заголовку
            
            
            AutoItX.WinWaitActive(text: "Chrome Legacy Window", timeout: 20);

            // Получаем ручку окна
            IntPtr windowHandle = AutoItX.WinGetHandle("Chrome Legacy Window", "");

            // Определяем контрол для ввода логина и вводим логин
            AutoItX.ControlFocus("[HANDLE:" + windowHandle + "]", "", "[CLASS:Chrome_RenderWidgetHostHWND; INSTANCE:1]");
            AutoItX.ControlSetText("[HANDLE:" + windowHandle + "]", "", "[CLASS:Chrome_RenderWidgetHostHWND; INSTANCE:1]", username);

            // Определяем контрол для ввода пароля и вводим пароль
            AutoItX.ControlFocus("[HANDLE:" + windowHandle + "]", "", "[CLASS:Chrome_RenderWidgetHostHWND; INSTANCE:2]");
            AutoItX.ControlSetText("[HANDLE:" + windowHandle + "]", "", "[CLASS:Chrome_RenderWidgetHostHWND; INSTANCE:2]", password);

            // Нажимаем клавишу Enter (пример)
            AutoItX.Send("{ENTER}");
        }
    }
}
