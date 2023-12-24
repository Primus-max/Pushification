using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace Pushification.PuppeteerDriver
{
    public class DriverManager
    {
        public static IWebDriver CreateDriver(string profilePath, ProxyInfo proxyInfo = null, string userAgent = null, bool useHeadlessMode = false)
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
            options.AddArgument($"--user-data-dir={profilePath}");

            // Установка юзер-агента
            options.AddArgument($"--user-agent={userAgent}");

            // Добавление аргумента для безголового режима
            if (useHeadlessMode)
            {
                options.AddArgument("--headless");
            }

            // Добавление опций для прокси, если они указаны
            if (proxyInfo != null)
            {
                options.AddArgument($"--proxy-server={proxyInfo.IP}:{proxyInfo.Port}");
            }

            try
            {
                // Создание экземпляра ChromeDriver с указанными опциями
                IWebDriver driver = new ChromeDriver(options);

                // Возвращение объекта драйвера
                return driver;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось создать драйвер: {ex.Message}");
                // TODO: логирование
                return null;
            }
        }
    }
}
