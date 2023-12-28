using OpenQA.Selenium;
using PuppeteerSharp;
using Pushification.Manager;
using Pushification.Models;
using Pushification.PuppeteerDriver;
using Pushification.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pushification.Services
{
    public class SubscribeService : IServiceWorker
    {
        private SubscriptionModeSettings _subscribeSettings = null;
        private IWebDriver _driver = null;

        public SubscribeService()
        {
            _subscribeSettings = SubscriptionModeSettings.LoadSubscriptionSettingsFromJson();
        }

        public async Task Run()
        {
            int workingTime = _subscribeSettings.TimeOptionOne * 60 * 1000; // Преобразуем минуты в миллисекунды                       
            string url = _subscribeSettings.URL;

            EventPublisherManager.RaiseUpdateUIMessage("Запускаю режим подписки на уведомления");

            // Цикл будет выполняться указанное в настройках время
            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalMilliseconds < workingTime)
            {
                EventPublisherManager.RaiseUpdateUIMessage($"Время выполнения данного режима будет: {workingTime}");
                ClearBlackList(); // Проверяю пороговое значение IP и удаляю если нужно

                // Получаю прокси 
                string proxyFilePath = _subscribeSettings.ProxyList;
                EventPublisherManager.RaiseUpdateUIMessage($"Путь к файлу с прокси: {proxyFilePath}");
                ProxyInfo proxy = await ProxyInfo.GetProxy(proxyFilePath, _subscribeSettings.MaxTimeGettingOutIP);

                if (proxy == null || string.IsNullOrEmpty(proxy.ExternalIP))
                {
                    EventPublisherManager.RaiseUpdateUIMessage("Не удалось получить прокси, пробую ещё раз"); 
                    continue;
                }
                    

                EventPublisherManager.RaiseUpdateUIMessage($"Получил IP {proxy.ExternalIP}");

                string profilePath = ProfilesManager.CreateProfileFolderPath(); // Создаю папку профиля

                string userAgent = UserAgetManager.GetRandomUserAgent();

               
                try
                {
                    _driver =  DriverManager.CreateDriver(profilePath, proxy, userAgent);                   
                }
                catch (Exception) { continue; }

                try
                {
                    // Устанавливаю время ожидания загрузки страницы
                    int timeOutMillisecond = _subscribeSettings.MaxTimePageLoading * 1000;                  

                    //await _page.GoToAsync("https://www.whatismyip.com/");
                    //await _page.ScreenshotAsync("whatismyip.png");

                    try
                    {
                        EventPublisherManager.RaiseUpdateUIMessage($"Перехожу по адресу {url}");
                        _driver.Navigate().GoToUrl(url);
                    }
                    catch (Exception ex)
                    {
                        EventPublisherManager.RaiseUpdateUIMessage($"Не удалось перейти по адресу : {ex.Message}");
                    }

                    // Извлекаем хост (домен) для передачи в виде простой строки без схемы
                    Uri uri = new Uri(_subscribeSettings?.URL);
                    string siteName = uri.Host;

                    // Подписываюсь на уведомление
                    bool IsSuccess = AutoItHandler.SubscribeToWindow(siteName, _subscribeSettings.BeforeAllowTimeout);

                    if (IsSuccess)
                    {
                        // Если успешно, то убираю прокси в блеклист
                        ProxyInfo.AddProxyToBlacklist(proxy.ExternalIP);
                        // Время ожидания после подписки
                        int afterAllowTimeoutMillisecond = _subscribeSettings.AfterAllowTimeout * 1000;
                        await Task.Delay(afterAllowTimeoutMillisecond);
                    }
                    else
                    {
                         CloseBrowser();
                        EventPublisherManager.RaiseUpdateUIMessage($"Не удалось подписаться на уведомление");
                        ProfilesManager.RemoveProfile(profilePath);
                    }
                }
                catch (Exception)
                {
                     CloseBrowser();
                    ProfilesManager.RemoveProfile(profilePath);
                }
                 CloseBrowser();
            }
        }

        // Закрываю браузер
        public void CloseBrowser()
        {
            try
            {
                _driver.Quit();
                _driver.Close();
                _driver.Dispose();
            }
            catch (Exception)
            {

            }

            // Удаляю лишние папки и файлы из профиля
           Thread.Sleep(1000);
            ProfilesManager.RemoveCash();
        }

        // Метод удаления IP
        private void ClearBlackList()
        {
            string blacklistFilePath = "blacklistproxy.txt";
            string[] blacklist = null;

            try
            {
                blacklist = File.ReadAllLines(blacklistFilePath);

                // Проверяем, если количество записей в блеклисте больше чем заданное _subscribeSettings.CountIP
                if (blacklist.Length > _subscribeSettings.CountIP)
                {
                    // Количество записей, которые нужно удалить
                    int numberOfDeletions = _subscribeSettings.CountIPDeletion;

                    // Удаляем указанное количество записей из начала блеклиста
                    List<string> updatedBlacklist = blacklist.Skip(numberOfDeletions).ToList();

                    // Перезаписываем обновленный блеклист
                    File.WriteAllLines(blacklistFilePath, updatedBlacklist);

                    EventPublisherManager.RaiseUpdateUIMessage($"Достигнуто пороговое значение IP {_subscribeSettings.CountIP},  удалено {numberOfDeletions} IP");
                }
            }
            catch (Exception ex)
            {
                EventPublisherManager.RaiseUpdateUIMessage($"Не удалось удалить IP, причина: {ex.Message}");
            }
        }
    }
}
