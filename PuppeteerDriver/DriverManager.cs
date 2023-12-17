using PuppeteerSharp;
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
                string log = ex.ToString();
                return null;
                // TODO логирование
            }
        }


    }
}
