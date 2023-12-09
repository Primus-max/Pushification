using PuppeteerSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pushification.PuppeteerDriver
{
    public class DriverManager
    {
        static async Task<IBrowser> CreateDriver()
        {
            await new BrowserFetcher().DownloadAsync();

            var launchOptions = new LaunchOptions
            {
                Headless = false,
                Args = new List<string> { "--start-maximized" }.ToArray()
            };

            return await Puppeteer.LaunchAsync(launchOptions);
        }
    }
}
