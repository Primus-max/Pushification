using PuppeteerSharp;
using Pushification.Manager;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pushification.PuppeteerDriver
{
    public class DriverManager
    {
        public static async Task<IBrowser> CreateDriver(string profilePath, ProxyInfo proxyInfo = null, string userAgent = null, bool useHeadlessMode = false)
        {

            if (string.IsNullOrEmpty(profilePath))
            {
                MessageBox.Show("Не удалось создать путь к профилю");
                return null;
            }

            await new BrowserFetcher().DownloadAsync();

            var launchOptions = new LaunchOptions
            {
                Headless = useHeadlessMode,
                Args = new List<string>
                {
                    "--start-maximized"
                }.ToArray(),
                UserDataDir = profilePath,
            };

            if (proxyInfo != null)
            {
                launchOptions.Args.Append($"--proxy-server=http://{proxyInfo.IP}:{proxyInfo.Port}");
            }

            if (!string.IsNullOrEmpty(userAgent))
            {
                launchOptions.Args.Append($"--user-agent={userAgent}");
            }


            try
            {
                return await Puppeteer.LaunchAsync(launchOptions);
            }
            catch (System.Exception ex)
            {
                EventPublisherManager.RaiseUpdateUIMessage($"Не удалось создать драйвер: {ex.Message}");
                return null;
                // TODO логирование
            }
        }

    }

    //    public class DriverManager
    //{
    //    public static async Task<IBrowser> CreateDriver(string profilePath, ProxyInfo proxyInfo = null, string userAgent = null, bool useHeadlessMode = false)
    //    {
    //        if (string.IsNullOrEmpty(profilePath))
    //        {
    //            MessageBox.Show("Не удалось создать путь к профилю");
    //            return null;
    //        }

    //        // Создаем объект BrowserFetcher
    //        var browserFetcher = new BrowserFetcher();

    //        // Получаем установленные браузеры
    //        var installedBrowsers = browserFetcher.GetInstalledBrowsers();
    //        foreach ( var browser in installedBrowsers )
    //        {
    //            string desiredChromeDriverVersion = browser.BuildId;
    //            // Скачиваем ChromeDriver нужной версии
    //            await browserFetcher.DownloadAsync(desiredChromeDriverVersion);
    //        }


    //        var launchOptions = new LaunchOptions
    //        {
    //            Headless = useHeadlessMode,
    //            Args = new List<string> { "--start-maximized" }.ToArray(),
    //            UserDataDir = profilePath,                
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
