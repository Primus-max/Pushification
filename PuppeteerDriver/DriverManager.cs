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
            string driverPath = "chromedriver(109).exe";

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

            // Проверяем, был ли предоставлен путь к драйверу
            if (!string.IsNullOrEmpty(driverPath))
            {
                launchOptions.Args.Append(driverPath);
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
}
