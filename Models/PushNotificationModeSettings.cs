using Newtonsoft.Json;
using System.Windows.Forms;
using System;
using System.IO;

namespace Pushification.Models
{
    public class PushNotificationModeSettings
    {
        public static string FilePath { get; } = "pushNotificationModeSettings.json";

        public double PercentToIgnore { get; set; }
        public double PercentToDelete { get; set; }
        public double PercentToClick { get; set; }
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


        public static PushNotificationModeSettings LoadFromJson()
        {           
            try
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FilePath);
                string json = File.ReadAllText(fullPath);
                return JsonConvert.DeserializeObject<PushNotificationModeSettings>(json);
            }
            catch (Exception ex)
            {
                // Обработка ошибок при загрузке
                MessageBox.Show($"Ошибка при загрузке настроек: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void SaveToJson()
        {            
            try
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                // Обработка ошибок при сохранении
                MessageBox.Show($"Ошибка при сохранении настроек: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}
