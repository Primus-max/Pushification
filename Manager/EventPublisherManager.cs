using System;

namespace Pushification.Manager
{
    public class EventPublisherManager
    {
        // Определение события
        public static event EventHandler<string> UpdateUIMessage;

        // Метод, вызывающий событие
        public static void RaiseUpdateUIMessage(string message)
        {
            UpdateUIMessage?.Invoke(null, message);
        }
    }
}
