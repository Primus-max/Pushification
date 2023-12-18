using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;

namespace Pushification.PuppeteerDriver
{

    public class DriverManager
    {
        public static IWebDriver CreateDriver(string profilePath, ProxyInfo proxyInfo = null, string userAgent = null, bool useHeadlessMode = false)
        {
            var chromeOptions = new ChromeOptions();

            if (useHeadlessMode)
            {
                chromeOptions.AddArgument("--headless=new");
            }

            if (!string.IsNullOrEmpty(profilePath))
            {
                chromeOptions.AddArgument($"--user-data-dir={profilePath}");
            }

            if (proxyInfo != null)
            {
                // Если требуется аутентификация на прокси, укажите логин и пароль в URL
                if (!string.IsNullOrEmpty(proxyInfo.Username) && !string.IsNullOrEmpty(proxyInfo.Password))
                {
                    var proxyUrl = $"http://{proxyInfo.Username}:{proxyInfo.Password}@{proxyInfo.IP}:{proxyInfo.Port}";
                    chromeOptions.AddArgument($"--proxy-server={proxyUrl}");
                }
            }

            //chromeService.HideCommandPromptWindow = true;

            if (!string.IsNullOrEmpty(userAgent))
            {
                chromeOptions.AddArgument($"--user-agent={userAgent}");
            }

            var chromeDriverPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chromedriver(109).exe");
            var service = ChromeDriverService.CreateDefaultService(AppDomain.CurrentDomain.BaseDirectory, "chromedriver(109).exe");

            return new ChromeDriver(service, chromeOptions);
        }
    }


    //public class DriverManager
    //{
    //    public static async Task<IBrowser> CreateDriver(string profilePath, ProxyInfo proxyInfo = null, string userAgent = null, bool useHeadlessMode = false)
    //    {

    //        if (string.IsNullOrEmpty(profilePath))
    //        {
    //            MessageBox.Show("Не удалось создать путь к профилю");
    //            return null;
    //        }

    //        await new BrowserFetcher().DownloadAsync();

    //        var launchOptions = new LaunchOptions
    //        {
    //            Headless = useHeadlessMode,
    //            Args = new List<string>
    //            {
    //                "--start-maximized"
    //            }.ToArray(),
    //            UserDataDir = profilePath,
    //        };

    //        if (proxyInfo != null)
    //        {
    //            launchOptions.Args.Append($"--proxy-server=http://{proxyInfo.IP}:{proxyInfo.Port}");
    //        }

    //        if (!string.IsNullOrEmpty(userAgent))
    //        {
    //            launchOptions.Args.Append($"--user-agent={userAgent}");
    //        }


    //        try
    //        {
    //            return await Puppeteer.LaunchAsync(launchOptions);
    //        }
    //        catch (System.Exception ex)
    //        {
    //            EventPublisherManager.RaiseUpdateUIMessage($"Не удалось создать драйвер: {ex.Message}");
    //            return null;
    //            // TODO логирование
    //        }
    //    }

    //}

    //public class DriverManager
    //{
    //    public static async Task<IBrowser> CreateDriver(string profilePath, ProxyInfo proxyInfo = null, string userAgent = null, bool useHeadlessMode = false)
    //    {

    //        var browserFetcher = new BrowserFetcher();
    //        await browserFetcher.DownloadAsync("109.0.5414.25");

    //        EventPublisherManager.RaiseUpdateUIMessage($"Получаю драйвер");
    //        if (string.IsNullOrEmpty(profilePath))
    //        {
    //            MessageBox.Show("Не удалось создать путь к профилю");
    //            return null;
    //        }

    //        var chromedriverPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chromedriver(109).exe");

    //        var launchOptions = new LaunchOptions
    //        {
    //            Headless = useHeadlessMode,
    //            Args = new List<string> { "--start-maximized" }.ToArray(),
    //            UserDataDir = profilePath,
    //            ExecutablePath = chromedriverPath
    //        };

    //        if (proxyInfo != null)
    //        {
    //            launchOptions.Args = launchOptions.Args.Concat($"--proxy-server=http://{proxyInfo.IP}:{proxyInfo.Port}".Split(' ')).ToArray();
    //        }

    //        if (!string.IsNullOrEmpty(userAgent))
    //        {
    //            launchOptions.Args = launchOptions.Args.Concat($"--user-agent={userAgent}".Split(' ')).ToArray();
    //        }

    //        try
    //        {
    //            EventPublisherManager.RaiseUpdateUIMessage($"Получаю драйвер");
    //            return await Puppeteer.LaunchAsync(launchOptions);
    //        }
    //        catch (System.Exception ex)
    //        {
    //            EventPublisherManager.RaiseUpdateUIMessage($"Не удалось создать драйвер: {ex.Message}");
    //            return null;
    //            // TODO логирование
    //        }
    //    }
    //}

}
