using PuppeteerSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pushification.PuppeteerDriver
{
    public class DriverManager
    {
        public static async Task<IBrowser> CreateDriver(string profilePath, ProxyInfo proxyInfo, string userAgent)
        {
            if(proxyInfo == null)
            {
                MessageBox.Show("Проверь лист с прокси, не удалось получить");
                return null; 
            }

            if(string.IsNullOrEmpty(profilePath))
            {
                MessageBox.Show("Не удалось создать путь к профилю");
                return null;
            }

            if (string.IsNullOrEmpty(userAgent))
            {
                MessageBox.Show("Не удалось получить юзер агента, проверь файл");
                return null;
            }

            await new BrowserFetcher().DownloadAsync();



            var launchOptions = new LaunchOptions
            {
                Headless = false,
                Args = new List<string>
                {
                    "--start-maximized",
                    $"--proxy-server=http://{proxyInfo.IP}:{proxyInfo.Port}"
                }.ToArray(),
                UserDataDir = profilePath,                
            };

            if (!string.IsNullOrEmpty(userAgent))
            {
                launchOptions.Args.Append($"--user-agent={userAgent}");
            }

            return await Puppeteer.LaunchAsync(launchOptions);
        }

    }
}
