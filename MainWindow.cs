using Pushification.Models;
using System;
using System.Globalization;
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


            LoadSubscriptonSettingsData();
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

                    _subscriptionSettings.ProxyList = selectedFilePath;
                    _subscriptionSettings.SaveSubscriptionSettingsToJson();
                    // Далее можно использовать выбранный файл (selectedFilePath) для ваших нужд
                }
            }
        }

        // Ссылка для перехода
        private void URL_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSetting<string>((settings, value) => settings.URL = value, URLTextBox);
        }

        // Максимальное время загрузки страницы
        private void MaxTimePageLoading_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSetting<int>((settings, value) => settings.MaxTimePageLoading = value, MaxTimePageLoadingTextBox);
        }

        // Время перед подкиской
        private void BeforeAllowTimeout_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSetting<int>((settings, value) => settings.BeforeAllowTimeout = value, BeforeAllowTimeoutTextBox);
        }

        // Ожидание после подписки
        private void AfterAllowTimeout_TextChenged(object sender, EventArgs e)
        {
            UpdateAndSaveSetting<int>((settings, value) => settings.AfterAllowTimeout = value, AfterAllowTimeoutTextBox);
        }

        // Время ожидания внешнего IP
        private void MaxTimeGettingOutI_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSetting<int>((settings, value) => settings.MaxTimeGettingOutIP = value, MaxTimeGettingOutITextBlock);
        }

        // Время ожидания получения прокси
        private void ProxyWaitingTimeout_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSetting<int>((settings, value) => settings.ProxyWaitingTimeout = value, ProxyWaitingTimeoutTextBox);
        }

        // Всего количество удаление IP (максимальное)
        private void CountIPToDelete_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSetting<int>((settings, value) => settings.CountIP = value, CountIPToDeleteTextBlock);
        }

        // Количество IP для удаления за раз
        private void CountIPDeletionPerTime_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSetting<int>((settings, value) => settings.CountIPDeletion = value, CountIPDeletionPerTimeTextBox);
        }

        private void TimeOptionOne_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSetting<int>((settings, value) => settings.TimeOptionOne = value, TimeOptionOneTextBox);
        }

        // Время запуска первогорежима
        private void StartOptionOneTimePicker_ValueChanged(object sender, EventArgs e)
        {
            string value = StartOptionOneTimePicker.Value.ToString("hh:mm tt");
            _subscriptionSettings.StartOptionOne = value;
            _subscriptionSettings.SaveSubscriptionSettingsToJson();
        }

        // Общий метод записи данных из полей в моде подписки на уведомления
        private void UpdateAndSaveSetting<T>(Action<SubscriptionModeSettings, T> updateAction, TextBox textBox)
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
            _subscriptionSettings = SubscriptionModeSettings.LoadSubscriptionSettingsFromJson();
            URLTextBox.Text = _subscriptionSettings.URL;
            MaxTimePageLoadingTextBox.Text = _subscriptionSettings.MaxTimePageLoading.ToString();
            AfterAllowTimeoutTextBox.Text = _subscriptionSettings.AfterAllowTimeout.ToString();
            BeforeAllowTimeoutTextBox.Text = _subscriptionSettings.BeforeAllowTimeout.ToString();
            ProxyWaitingTimeoutTextBox.Text = _subscriptionSettings.ProxyWaitingTimeout.ToString();
            MaxTimeGettingOutITextBlock.Text = _subscriptionSettings.MaxTimeGettingOutIP.ToString();
            CountIPToDeleteTextBlock.Text  = _subscriptionSettings.CountIP.ToString();
            CountIPDeletionPerTimeTextBox.Text = _subscriptionSettings.CountIPDeletion.ToString();
            if (DateTime.TryParseExact(_subscriptionSettings.StartOptionOne, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime))
            {
                StartOptionOneTimePicker.Value = startTime;
            }
            TimeOptionOneTextBox.Text = _subscriptionSettings.TimeOptionOne.ToString();
        }







        #endregion

        #region РЕЖИМ-2


        #endregion


    }
}
