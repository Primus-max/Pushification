using System;
using System.Windows.Automation;
using System.Windows.Forms;


namespace Pushification
{
    public partial class MainWindow : Form
    {
        private AutomationElement lastFocusedElement;
        public MainWindow()
        {
            InitializeComponent();

            // Метод для подписки на разные нативные события винды
            Automation.AddAutomationFocusChangedEventHandler((sender, e) =>
            {
                AutomationElement focusedElement = sender as AutomationElement;

                // Проверяем, изменился ли фокус на новый элемент
                if (focusedElement != null && focusedElement != lastFocusedElement)
                {
                    lastFocusedElement = focusedElement;

                    // Проверяем тип элемента (если окно)
                    if (focusedElement.Current.ControlType == ControlType.Window)
                    {
                        var adfg = focusedElement.Current.HelpText;
                        // Проверяем класс элемента
                        if (focusedElement.Current.HelpText == "Toast") // Здесь нужно использовать реальное имя класса уведомления
                        {
                            // Это уведомление, которое вас интересует
                            MessageBox.Show("Обнаружено уведомление");
                        }
                    }
                }
            });

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










        #region РЕЖИМ-1

        #endregion

        #region РЕЖИМ-2


        #endregion

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Выберите файл с прокси";
                openFileDialog.Filter = "Текстовые файлы|*.txt|Все файлы|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;

                    // Далее можно использовать выбранный файл (selectedFilePath) для ваших нужд
                }
            }
        }
               
    }
}
