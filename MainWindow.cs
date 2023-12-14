using Pushification.Models;
using Pushification.Services;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Automation;
using System.Windows.Forms;


namespace Pushification
{
    public partial class MainWindow : Form
    {
        private AutomationElement lastFocusedElement;
        private PushNotificationModeSettings _pushSettings;
        private SubscriptionModeSettings _subscriptionSettings;
        public MainWindow()
        {
            InitializeComponent();


            _pushSettings = new PushNotificationModeSettings();
            _subscriptionSettings = new SubscriptionModeSettings();

            // Инициализация полей форм из json
            LoadSubscriptonSettingsData();
            LoadPushNotificationSettingsData();

            
            // Метод для подписки на разные нативные события винды
            //Automation.AddAutomationFocusChangedEventHandler((sender, e) =>
            //{
            //    AutomationElement focusedElement = sender as AutomationElement;

            //    // Проверяем, изменился ли фокус на новый элемент
            //    if (focusedElement != null && focusedElement != lastFocusedElement)
            //    {
            //        lastFocusedElement = focusedElement;

            //        // Проверяем тип элемента (если окно)
            //        if (focusedElement.Current.ControlType == ControlType.Window)
            //        {
            //            var adfg = focusedElement.Current.HelpText;
            //            // Проверяем класс элемента
            //            if (focusedElement.Current.HelpText == "Toast") // Здесь нужно использовать реальное имя класса уведомления
            //            {
            //                // Это уведомление, которое вас интересует
            //                MessageBox.Show("Обнаружено уведомление");
            //            }
            //        }
            //    }
            //});

            //// Активируем окно
            //AutoItX.WinActivate(windowTitle);
            //AutoItX.
            //// Выполняем клик в определенных местах
            //AutoItX.MouseClick("left", x, y, 1, 0);
        }

        private bool IsWindowPatternSupported(AutomationElement element)
        {
            // Проверяем поддержку WindowPattern
            object patternObj;
            if (element.TryGetCurrentPattern(WindowPattern.Pattern, out patternObj))
            {
                WindowPattern windowPattern = patternObj as WindowPattern;
                return windowPattern != null;
            }

            return false;
        }


        #region РЕЖИМ-1 Подписка на уведомления
        // Соханяю файл прокси
        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Выберите файл с прокси";
                openFileDialog.Filter = "Текстовые файлы|*.txt|Все файлы|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    string destinationPath = Path.Combine(Application.StartupPath, Path.GetFileName(selectedFilePath));

                    try
                    {
                        // Копирование файла
                        File.Copy(selectedFilePath, destinationPath, overwrite: true);

                        // Установка пути к скопированному файлу
                        _subscriptionSettings.ProxyList = destinationPath;
                        _subscriptionSettings.SaveSubscriptionSettingsToJson();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при копировании файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        // Ссылка для перехода
        private void URL_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSubscribeSetting<string>((settings, value) => settings.URL = value, URLTextBox);
        }

        // Максимальное время загрузки страницы
        private void MaxTimePageLoading_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSubscribeSetting<int>((settings, value) => settings.MaxTimePageLoading = value, MaxTimePageLoadingTextBox);
        }

        // Время перед подкиской
        private void BeforeAllowTimeout_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSubscribeSetting<int>((settings, value) => settings.BeforeAllowTimeout = value, BeforeAllowTimeoutTextBox);
        }

        // Ожидание после подписки
        private void AfterAllowTimeout_TextChenged(object sender, EventArgs e)
        {
            UpdateAndSaveSubscribeSetting<int>((settings, value) => settings.AfterAllowTimeout = value, AfterAllowTimeoutTextBox);
        }

        // Время ожидания внешнего IP
        private void MaxTimeGettingOutI_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSubscribeSetting<int>((settings, value) => settings.MaxTimeGettingOutIP = value, MaxTimeGettingOutITextBlock);
        }

        // Время ожидания получения прокси
        private void ProxyWaitingTimeout_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSubscribeSetting<int>((settings, value) => settings.ProxyWaitingTimeout = value, ProxyWaitingTimeoutTextBox);
        }

        // Всего количество удаление IP (максимальное)
        private void CountIPToDelete_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSubscribeSetting<int>((settings, value) => settings.CountIP = value, CountIPToDeleteTextBlock);
        }

        // Количество IP для удаления за раз
        private void CountIPDeletionPerTime_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSubscribeSetting<int>((settings, value) => settings.CountIPDeletion = value, CountIPDeletionPerTimeTextBox);
        }

        private void TimeOptionOne_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSubscribeSetting<int>((settings, value) => settings.TimeOptionOne = value, TimeOptionOneTextBox);
        }

        // Время запуска первогорежима
        private void StartOptionOneTimePicker_ValueChanged(object sender, EventArgs e)
        {
            string value = StartOptionOneTimePicker.Value.ToString("hh:mm tt");
            _subscriptionSettings.StartOptionOne = value;
            _subscriptionSettings.SaveSubscriptionSettingsToJson();
        }

        // Общий метод записи данных из полей в моде подписки на уведомления
        private void UpdateAndSaveSubscribeSetting<T>(Action<SubscriptionModeSettings, T> updateAction, TextBox textBox)
        {
            if (textBox == null || string.IsNullOrEmpty(textBox.Text)) return;

            T value = (T)Convert.ChangeType(textBox.Text, typeof(T));
            updateAction(_subscriptionSettings, value);
            _subscriptionSettings.SaveSubscriptionSettingsToJson();
        }

        // Инициализация полей данными из json

        //LoadSubscriptonSettingsData();
        private void LoadSubscriptonSettingsData()
        {
            try
            {
                _subscriptionSettings = SubscriptionModeSettings.LoadSubscriptionSettingsFromJson();
                URLTextBox.Text = _subscriptionSettings.URL;
                MaxTimePageLoadingTextBox.Text = _subscriptionSettings.MaxTimePageLoading.ToString();
                AfterAllowTimeoutTextBox.Text = _subscriptionSettings.AfterAllowTimeout.ToString();
                BeforeAllowTimeoutTextBox.Text = _subscriptionSettings.BeforeAllowTimeout.ToString();
                ProxyWaitingTimeoutTextBox.Text = _subscriptionSettings.ProxyWaitingTimeout.ToString();
                MaxTimeGettingOutITextBlock.Text = _subscriptionSettings.MaxTimeGettingOutIP.ToString();
                CountIPToDeleteTextBlock.Text = _subscriptionSettings.CountIP.ToString();
                CountIPDeletionPerTimeTextBox.Text = _subscriptionSettings.CountIPDeletion.ToString();
                if (DateTime.TryParseExact(_subscriptionSettings.StartOptionOne, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime))
                {
                    StartOptionOneTimePicker.Value = startTime;
                }
                TimeOptionOneTextBox.Text = _subscriptionSettings.TimeOptionOne.ToString();
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region РЕЖИМ-2 Принятие уведомлений

        // Задержка перед закрытием браузер после того как пришли уведомления
        private void SleepBeforeProcessKillIgnore_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSavePushNotificationSettings<int>((settings, value) => settings.SleepBeforeProcessKillIgnore = value, SleepBeforeProcessKillIgnoreTextBox);
        }

        //Макс. время ожидания уведомлений при click
        private void TimeToWaitNotificationClick_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSavePushNotificationSettings<int>((settings, value) => settings.TimeToWaitNotificationClick = value, TimeToWaitNotificationClickTextBox);
        }

        //  Макс. время ожидания уведомлений при ignore
        private void MaxTimeToWaitNotificationIgnore_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSavePushNotificationSettings<int>((settings, value) => settings.MaxTimeToWaitNotificationIgnore = value, MaxTimeToWaitNotificationIgnoreTextBox);
        }

        // Задержка между кликами по уведомлениям
        private void SleepBetweenClick_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSavePushNotificationSettings<int>((settings, value) => settings.SleepBetweenClick = value, SleepBetweenClickTextBox);
        }

        // Задержка после кликов по уведомлениям
        private void SleepAfterAllNotificationsClick_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSavePushNotificationSettings<int>((settings, value) => settings.SleepAfterAllNotificationsClick = value, SleepAfterAllNotificationsClickTextBox); ;
        }

        // Задержка после открытия браузера при удалении
        private void SleepBeforeUnsubscribe_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSavePushNotificationSettings<int>((settings, value) => settings.SleepBeforeUnsubscribe = value, SleepBeforeUnsubscribeTextBox); 
        }

        // Задержка после сброса разрешения
        private void SleepAfterUnsubscribe_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSavePushNotificationSettings<int>((settings, value) => settings.SleepAfterUnsubscribe = value, SleepAfterUnsubscribeTextBox);
        }

        // Задержка перед удалением профиля
        private void SleepBeforeProfileDeletion_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSavePushNotificationSettings<int>((settings, value) => settings.SleepBeforeProfileDeletion = value, SleepBeforeProfileDeletionTextBox);
        }

        // Шанс на режим delete
        private void PercentToDelete_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSavePushNotificationSettings<int>((settings, value) => settings.PercentToDelete = value, PercentToDeleteTextBox);
        }

        // Шанс на режим click
        private void PercentToClick_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSavePushNotificationSettings<int>((settings, value) => settings.PercentToClick = value, PercentToClickTextBox);
        }

        // Минимальное кол-во кликов по уведомлениям
        private void MinNumberOfClicks_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSavePushNotificationSettings<int>((settings, value) => settings.MinNumberOfClicks = value, MinNumberOfClicksTextBox);
        }

        // Максимальное кол-во кликов по уведомлениям
        private void MaxNumberOfClicks_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSavePushNotificationSettings<int>((settings, value) => settings.MaxNumberOfClicks = value, MaxNumberOfClicksTextBox);
        }

        //Режим ignore (с прокси или без)
        private void ProxyForIgnore_CheckedChange(object sender, EventArgs e)
        {
            _pushSettings.ProxyForIgnore = ProxyForIgnoreCheckBox.Checked;
            _pushSettings.SaveToJson();
        }

        // Закрывать пуш уведомления  (на крестик)
        private void NotificationCloseByButton_CheckedChange(object sender, EventArgs e)
        {
            _pushSettings.NotificationCloseByButton = NotificationCloseByButtonCheckBox.Checked;
            _pushSettings.SaveToJson();
        }

        // Безголовый режим
        private void HeadlessMode_CheckedChanged(object sender, EventArgs e)
        {
            _pushSettings.HeadlessMode = HeadlessModeCheckBox.Checked;
            _pushSettings.SaveToJson();
        }

        // Общий метод записи данных из полей в моде подписки на уведомления
        private void UpdateAndSavePushNotificationSettings<T>(Action<PushNotificationModeSettings, T> updateAction, TextBox textBox)
        {
            if (textBox == null || string.IsNullOrEmpty(textBox.Text)) return;

            T value = (T)Convert.ChangeType(textBox.Text, typeof(T));
            updateAction(_pushSettings, value);
            _pushSettings.SaveToJson();
        }

        // Инициализация данныз из Json во view
        private void LoadPushNotificationSettingsData()
        {
            try
            {
                _pushSettings = PushNotificationModeSettings.LoadFromJson();

                SleepBeforeProcessKillIgnoreTextBox.Text = _pushSettings.SleepBeforeProcessKillIgnore.ToString();
                TimeToWaitNotificationClickTextBox.Text = _pushSettings.TimeToWaitNotificationClick.ToString();
                MaxTimeToWaitNotificationIgnoreTextBox.Text = _pushSettings.MaxTimeToWaitNotificationIgnore.ToString();
                SleepBetweenClickTextBox.Text = _pushSettings.SleepBetweenClick.ToString();
                SleepAfterAllNotificationsClickTextBox.Text = _pushSettings.SleepAfterAllNotificationsClick.ToString();
                SleepBeforeUnsubscribeTextBox.Text = _pushSettings.SleepBeforeUnsubscribe.ToString();
                SleepAfterUnsubscribeTextBox.Text = _pushSettings.SleepAfterUnsubscribe.ToString();
                SleepBeforeProfileDeletionTextBox.Text = _pushSettings.SleepBeforeProfileDeletion.ToString();
                PercentToDeleteTextBox.Text = _pushSettings.PercentToDelete.ToString();
                PercentToClickTextBox.Text = _pushSettings.PercentToClick.ToString();
                MinNumberOfClicksTextBox.Text = _pushSettings.MinNumberOfClicks.ToString();
                MaxNumberOfClicksTextBox.Text = _pushSettings.MaxNumberOfClicks.ToString();
                ProxyForIgnoreCheckBox.Checked = _pushSettings.ProxyForIgnore;
                NotificationCloseByButtonCheckBox.Checked = _pushSettings.NotificationCloseByButton;
                HeadlessModeCheckBox.Checked = _pushSettings.HeadlessMode;

            }
            catch (Exception)
            {

            }
        }















        #endregion

        private void Start_Click(object sender, EventArgs e)
        {
            SubscribeService subscribeService = new SubscribeService();

            subscribeService.Run();
        }
    }
}
