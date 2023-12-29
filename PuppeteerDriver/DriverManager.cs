using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chrome.ChromeDriverExtensions;
using Pushification.Manager;
using System;

namespace Pushification.PuppeteerDriver
{
    public class DriverManager
    {
        public static IWebDriver CreateDriver(string profilePath, ProxyInfo proxyInfo = null, string userAgent = null, bool useHeadlessMode = false, bool disableNotifivation = false, bool enableNotifications = false)
        {
            // Проверка наличия пути к папке профиля
            if (string.IsNullOrEmpty(profilePath))
            {
                Console.WriteLine("Не удалось создать путь к профилю");
                return null;
            }

            // Создание опций для браузера
            ChromeOptions options = new ChromeOptions();

            // Добавление аргумента для максимизации окна
            options.AddArgument("--start-maximized");

            // Добавление аргумента для указания папки профиля
            if (!string.IsNullOrEmpty(profilePath))
                options.AddArgument($"--user-data-dir={profilePath}");

            // Установка юзер-агента
            if (proxyInfo != null)
                options.AddArgument($"--user-agent={userAgent}");

            // Добавление аргумента для безголового режима
            if (useHeadlessMode)
                options.AddArgument("--headless");

            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();

            if (proxyInfo != null)
            {
                options.AddHttpProxy(proxyInfo.IP, proxyInfo.Port, proxyInfo.Username, proxyInfo.Password);
            }
            else
            {
                ProfilesManager.ClearDefaultDirectory(profilePath);
            }
                

            // Отключаю уведомления
            if (disableNotifivation)
                DisableNotifications(options);

            // Включаю уведомления
            //if(enableNotifications)
            //    EnableNotifications(options);

            //string chromeDriverPath = @"C:\Users\FedoTT\source\repos\Pushification\bin\Debug\chromedriver.exe";

            try
            {
                // Создание экземпляра ChromeDriver с указанными опциями
                IWebDriver driver = new ChromeDriver( options);

                // Возвращение объекта драйвера
                return driver;
            }
            catch (Exception ex)
            {
                EventPublisherManager.RaiseUpdateUIMessage($"Не удалось создать драйвер: {ex.Message}");
                // TODO: логирование
                return null;
            }
        }

        public static void DisableNotifications(ChromeOptions options)
        {
            options.AddUserProfilePreference("profile.default_content_settings.notifications", 2);
            options.AddUserProfilePreference("profile.default_content_setting_values.notifications", 2);
            options.AddUserProfilePreference("profile.contentSettings.notifications", 2);
            options.AddUserProfilePreference("profile.content_settings.exceptions.notifications", 2);
            options.AddUserProfilePreference("profile.managed_default_content_settings.notifications", 2);
        }

        public static void EnableNotifications(ChromeOptions options)
        {
            options.AddUserProfilePreference("profile.default_content_settings.notifications", 1);
            options.AddUserProfilePreference("profile.default_content_setting_values.notifications", 1);
            options.AddUserProfilePreference("profile.contentSettings.notifications", 1);
            options.AddUserProfilePreference("profile.content_settings.exceptions.notifications", 1);
            options.AddUserProfilePreference("profile.managed_default_content_settings.notifications", 1);
        }
    }
}
