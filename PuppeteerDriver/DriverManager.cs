using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins;
using PuppeteerSharp;
using Pushification.Manager;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pushification.PuppeteerDriver
{
    public class DriverManager
    {
        public static ProxyInfo _proxy = null;

        public static async Task<IBrowser> CreateDriver(string profilePath, ProxyInfo proxyInfo = null, string userAgent = null, bool useHeadlessMode = false)
        {
            _proxy = proxyInfo;

            if (string.IsNullOrEmpty(profilePath))
            {
                EventPublisherManager.RaiseUpdateUIMessage("Не удалось создать путь к профилю");
                return null;
            }

            await new BrowserFetcher().DownloadAsync();

            var launchArguments = new List<string>
            {
                "--start-maximized",
            };

            if (proxyInfo != null)
            {
                launchArguments.Add($"--proxy-server={proxyInfo.IP}:{proxyInfo.Port}");
            }

            var launchOptions = new LaunchOptions
            {
                Headless = useHeadlessMode,
                Args = launchArguments.ToArray(),
                UserDataDir = profilePath,
            };

            UserAgentProvider.CustomUserAgent = userAgent;
            UserAgentProvider.Pass = proxyInfo?.Password;
            UserAgentProvider.Username = proxyInfo?.Username;

            try
            {
                var extra = new PuppeteerExtra();

                // var browser = await Puppeteer.LaunchAsync(launchOptions);
                // var wsEndPoint = browser.WebSocketEndpoint;
                //var uaPlugin = new PuppeteerExtraSharp.Plugins.AnonymizeUa.AnonymizeUaPlugin();
                //uaPlugin.CustomizeUa(customUserAgent => userAgent);
                extra.Use(new AnonymizeUaPlugin());
                // Возвращаем объект браузера
                return await extra.LaunchAsync(launchOptions);
            }
            catch (System.Exception ex)
            {
                EventPublisherManager.RaiseUpdateUIMessage($"Не удалось создать драйвер: {ex.Message}");
                return null;
                // TODO логирование
            }
        }


    }

    public static class UserAgentProvider
    {
        public static string CustomUserAgent { get; set; }
        public static string Pass { get; set; }
        public static string Username { get; set; }
    }

    public class AnonymizeUaPlugin : PuppeteerExtraPlugin
    {
        public AnonymizeUaPlugin()
            : base("anonymize-ua")
        {
        }

        public override async Task OnPageCreated(IPage page)
        {
            string input = (await page.Browser.GetUserAgentAsync()).Replace("HeadlessChrome", "Chrome");
            input = new Regex(@"/\(([^)]+)\)/").Replace(input, "(Windows NT 10.0; Win64; x64)");

            if (!string.IsNullOrEmpty(UserAgentProvider.CustomUserAgent))
            {
                input = UserAgentProvider.CustomUserAgent;
            }

            await page.SetUserAgentAsync(input);

            if (UserAgentProvider.Pass != null || UserAgentProvider.Username != null)
                await page.AuthenticateAsync(new Credentials() { Password = UserAgentProvider.Pass, Username = UserAgentProvider.Username });
        }
    }


}
