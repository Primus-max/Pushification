using Newtonsoft.Json;
using System.Windows.Forms;
using System;
using System.IO;

namespace Pushification.Models
{
    public class SubscriptionModeSettings
    {
        public static string FilePath { get; } = "subscriptionModeSettings.json";

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

        public static SubscriptionModeSettings LoadSubscriptionSettingsFromJson()
        {
            try
            {
                string json = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<SubscriptionModeSettings>(json);
            }
            catch (Exception ex)
            {
                // Обработка ошибок при загрузке
                MessageBox.Show($"Ошибка при загрузке настроек: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void SaveSubscriptionSettingsToJson()
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
