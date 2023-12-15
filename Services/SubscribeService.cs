using PuppeteerSharp;
using Pushification.Manager;
using Pushification.Models;
using Pushification.PuppeteerDriver;
using Pushification.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pushification.Services
{
    public class SubscribeService : IServiceWorker
    {
        private SubscriptionModeSettings _subscribeSettings = null;
        private IBrowser _browser = null;
        private IPage _page = null;

        public SubscribeService()
        {
            // _driverManager = driverManager;
            _subscribeSettings = SubscriptionModeSettings.LoadSubscriptionSettingsFromJson();
        }

        public async Task Run()
        {
            int workingTime = _subscribeSettings.TimeOptionOne * 60 * 1000; // Преобразуем минуты в миллисекунды                       
            string url = _subscribeSettings.URL;

            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalMilliseconds < workingTime)
            {
                string profilePath = ProfilesManager.CreateProfileFolderPath();
                string proxyInfoString = ProxyInfo.GetProxy(_subscribeSettings.ProxyList);
                ProxyInfo proxyInfo = ProxyInfo.Parse(proxyInfoString);

                string userAgent = GetRandomUserAgent();

                _browser = await DriverManager.CreateDriver(profilePath, proxyInfo, userAgent);

                if (_browser == null)
                {
                    // TODO здесь будет логирование
                    return;
                }

                _page = await _browser.NewPageAsync();

                // Авторизую прокси
                await _page.AuthenticateAsync(new Credentials() { Password = proxyInfo.Password, Username = proxyInfo.Username });
                try
                {
                    // Устанавливаю время ожидания загрузки страницы
                    int timeOutMillisecond = _subscribeSettings.MaxTimePageLoading * 1000;
                    await _page.WaitForTimeoutAsync(timeOutMillisecond);
                    await _page.GoToAsync(url);

                    // Принимаю подписку Push уведомлений
                    // Парсим URL
                    Uri uri = new Uri(_subscribeSettings?.URL);

                    // Извлекаем хост (домен)
                    string siteName = uri.Host;

                    bool IsSuccess = AutoItHandler.SubscribeToWindow(siteName, 10, _subscribeSettings.BeforeAllowTimeout);

                    if (IsSuccess)
                    {
                        // Если успешно, то убираем прокси в блеклист
                        //ProxyInfo.AddProxyToBlacklist(proxyInfoString);

                        // Время ожидания после подписки
                        int afterAllowTimeoutMillisecond = _subscribeSettings.AfterAllowTimeout * 1000;
                        await Task.Delay(afterAllowTimeoutMillisecond);
                    }
                }
                catch (Exception)
                {
                    await StopAsync(profilePath);
                }
                NotificationService notificationService = new NotificationService();

                notificationService.CloseNotificationToast();

                 await StopAsync(profilePath);
            }
        }

        // Закрываю браузер
        public async Task StopAsync(string profilePath)
        {
            // Закрыть браузер после прошествия времени
            await _browser.CloseAsync();
            await _page.DisposeAsync();

            // Удаляю лишние папки и файлы из профиля
            await Task.Delay(1000);
            RemoveCashFolders(profilePath);
        }

        // Удаление папки кэша, занимает место
        private void RemoveCashFolders(string profilePath)
        {
            string[] foldersToDelete = {
            "GPUCache",
            "Cache",
            "Code Cache",
            "DawnCache",
            "Extension Rules",
            "Extension Scripts",
            "Extension State",
            "Extensions",
            "Local Extension Settings",
            "GrShaderCache",
            "ShaderCache",
            "Crashpad",
            "GraphiteDawnCache"
        };

            try
            {
                foreach (var folderName in foldersToDelete)
                {
                    string folderPath = Path.Combine(profilePath, folderName);

                    // Проверяем, существует ли папка в корне профиля
                    if (Directory.Exists(folderPath))
                    {
                        Directory.Delete(folderPath, true);
                        Thread.Sleep(100);
                    }
                    else
                    {
                        // Папка не найдена в корне профиля, поэтому пробуем внутри "Default"
                        folderPath = Path.Combine(profilePath, "Default", folderName);
                        if (Directory.Exists(folderPath))
                        {
                            Directory.Delete(folderPath, true);
                            Thread.Sleep(100);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to remove folders: {ex.Message}");
            }
        }

        //// Метод получения пути профиля
        //private string CreateProfileFolderPath()
        //{
        //    // Получаем текущее время в формате Unix timestamp
        //    long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        //    // Получаем текущую дату в формате dd-MM-yyyy
        //    string currentDate = DateTime.Now.ToString("dd-MM-yyyy");

        //    // Формируем название папки профиля
        //    string profileFolderName = $"{unixTimestamp}_{currentDate}";

        //    // Сформируйте полный путь к папке профиля в папке "profiles" в корне проекта
        //    string profilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "profiles", profileFolderName);

        //    // Возвращаем полученный путь
        //    return profilePath;
        //}

        // Метод получения рандомного юзер агента
        private string GetRandomUserAgent()
        {
            try
            {
                // Загрузка списка User-Agent'ов из файла
                string[] userAgents = LoadUserAgentsFromFile("useragents.txt");

                if (userAgents is null) return null;

                // Получение случайного User-Agent'а из списка
                Random random = new Random();
                int randomIndex = random.Next(userAgents.Length);
                return userAgents[randomIndex];
            }
            catch (Exception ex)
            {
                // Обработка ошибок при загрузке или выборе User-Agent'а
                MessageBox.Show($"Ошибка при получении User-Agent'а: {ex.Message}");
                return string.Empty;
            }
        }

        // Вспомогательный метод получения юзер агента
        private string[] LoadUserAgentsFromFile(string filePath)
        {
            try
            {
                // Чтение всех строк из файла
                string[] lines = File.ReadAllLines(filePath);

                // Фильтрация и удаление пустых строк
                string[] nonEmptyLines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

                return nonEmptyLines;
            }
            catch (Exception ex)
            {
                // Обработка ошибок при чтении файла
                MessageBox.Show($"Ошибка при загрузке User-Agent'ов: {ex.Message}");
                return new string[0];
            }
        }
    }
}
