using PuppeteerSharp;
using Pushification.Manager;
using System;
using System.Collections.Generic;
using System.IO;
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
                EventPublisherManager.RaiseUpdateUIMessage("Не удалось создать путь к профилю");
                return null;
            }

            await new BrowserFetcher().DownloadAsync();

            var launchOptions = new LaunchOptions
            {
                Headless = useHeadlessMode,
                Args = new[]
                {
                    "--start-maximized",
                    $"--proxy-server={proxyInfo?.IP}:{proxyInfo?.Port}",       
                }
                .Where(arg => arg != null)
                .ToArray(),
                UserDataDir = profilePath,
            };

            
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

}
