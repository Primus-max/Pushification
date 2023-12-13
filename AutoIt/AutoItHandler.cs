using AutoIt;
using System;
using System.Threading.Tasks;

public class AutoItHandler
{
    public static async Task<bool> SubscribeToWindow(string title, int waitingWindow, int waitingBeforeClick)
    {
        int x = 247;
        int y = 172;

        string windowTitle = $"{title} запрашивает разрешение на:";

        try
        {
            // Ожидаем окно с подпиской
            AutoItX.WinWait(title: windowTitle, timeout: waitingWindow);
            // На всякий случай делаем активным. На случай если перекрыло другое окно
            AutoItX.WinActivate(title: windowTitle);

            // Указаное в настройках время ожидания перед кликом
            await Task.Delay(waitingBeforeClick);

            // КЛикаем на Разрешить получать уведомления
            AutoItX.MouseMove(x, y, speed: 50);
            AutoItX.MouseClick(button: "LEFT", x: x, y: y, numClicks: 1, speed: 10);

            // Отсутствие этого окна будет означать успешный клик
            int isSucsess = AutoItX.WinWait(title: windowTitle, timeout: 5); 
            return !Convert.ToBoolean(isSucsess);
        }
        catch (Exception)
        {
            return false;
        }
    }
}
