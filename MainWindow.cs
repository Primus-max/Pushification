using Pushification.Models;
using System;
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

        private void BeforeAllowTimeout_TextChanged(object sender, EventArgs e)
        {
            UpdateAndSaveSetting<int>((settings, value) => settings.BeforeAllowTimeout = value, BeforeAllowTimeoutTextBox);
        }

        // Общий метод записи данных из полей в моде подписки на уведомления
        private void UpdateAndSaveSetting<T>(Action<SubscriptionModeSettings, T> updateAction, TextBox textBox)
        {
            if (textBox == null || string.IsNullOrEmpty(textBox.Text)) return;

            T value = (T)Convert.ChangeType(textBox.Text, typeof(T));
            updateAction(_subscriptionSettings, value);
            _subscriptionSettings.SaveSubscriptionSettingsToJson();
        }

        #endregion

        #region РЕЖИМ-2


        #endregion


    }
}
