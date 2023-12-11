namespace Pushification.Models
{
    public class SubscriptionModeSettings
    {
        public string URL { get; set; }
        public string ProxyList { get; set; }
        public int MaxTimePageLoading { get; set; }
        public int BeforeAllowTimeout { get; set; }
        public int AfterAllowTimeout { get; set; }
        public int MaxTimeGettingOutIP { get; set; }
        public int ProxyWaitingTimeout { get; set; }
        public int CountIP { get; set; }
        public int CountIPDeletion { get; set; }
        public string StartOptionOne { get; set; }
        public int TimeOptionOne { get; set; }
    }

}
