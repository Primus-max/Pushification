using PuppeteerSharp;
using Pushification.Models;
using Pushification.PuppeteerDriver;
using Pushification.Services.Interfaces;
using System.IO;
using System;

namespace Pushification.Services
{
    public class SubscribeService : IServiceWorker
    {
        private readonly DriverManager _driverManager = null;
        private SubscriptionModeSettings _subscribeSettings = null; 
        private IBrowser _browser = null;

        public SubscribeService(DriverManager driverManager)
        {
            _driverManager = driverManager;
            _subscribeSettings = SubscriptionModeSettings.LoadSubscriptionSettingsFromJson();
        }

        public async void Run()
        {
            string profilePath = GetProfileFolderPath();
            string proxyInfoString = "ip:port:логин:пароль";
            string userAgent = "ваш_user_agent";

            ProxyInfo proxyInfo = ProxyInfo.Parse(proxyInfoString);

            IBrowser browser = await _driverManager.CreateDriver(profilePath, proxyInfo, userAgent);
        }

        public void Stop()
        {
            _browser.Dispose();
        }

        public string GetProfileFolderPath()
        {
            // Получаем текущее время в формате Unix timestamp
            long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            // Получаем текущую дату в формате dd-MM-yyyy
            string currentDate = DateTime.Now.ToString("dd-MM-yyyy");

            // Формируем название папки профиля
            string profileFolderName = $"{unixTimestamp}_{currentDate}";

            // Сформируйте полный путь к папке профиля в папке "profiles" в корне проекта
            string profilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "profiles", profileFolderName);

            // Возвращаем полученный путь
            return profilePath;
        }

    }
}
