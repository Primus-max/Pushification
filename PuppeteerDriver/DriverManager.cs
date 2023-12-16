using PuppeteerSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pushification.PuppeteerDriver
{
    public class DriverManager
    {
        public static async Task<IBrowser> CreateDriver(string profilePath, ProxyInfo proxyInfo = null, string userAgent = null)
        {
            if (string.IsNullOrEmpty(profilePath))
            {
                MessageBox.Show("Не удалось создать путь к профилю");
                return null;
            }

            await new BrowserFetcher().DownloadAsync();

            var launchOptions = new LaunchOptions
            {
                Headless = false,
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

            return await Puppeteer.LaunchAsync(launchOptions);
        }


    }
}
