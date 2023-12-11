namespace Pushification.Models
{
    public class PushNotificationModeSettings
    {
        public int PercentToDelete { get; set; }
        public int PercentToClick { get; set; }
        public int SleepBeforeProcessKillIgnore { get; set; }
        public int MaxTimeToWaitNotificationIgnore { get; set; }
        public int TimeToWaitNotificationClick { get; set; }
        public int MinNumberOfClicks { get; set; }
        public int MaxNumberOfClicks { get; set; }
        public int SleepBetweenClick { get; set; }
        public int SleepAfterAllNotificationsClick { get; set; }
        public int SleepBeforeUnsubscribe { get; set; }
        public int SleepAfterUnsubscribe { get; set; }
        public int SleepBeforeProfileDeletion { get; set; }
        public bool HeadlessMode { get; set; }
        public bool ProxyForIgnore { get; set; }
        public bool NotificationCloseByButton { get; set; }
    }

}
