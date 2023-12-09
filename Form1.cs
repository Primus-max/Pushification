using System;
using System.Windows.Automation;
using System.Windows.Forms;
using AutoItX3Lib;
using AutoIt;


namespace Pushification
{
    public partial class Form1 : Form
    {
         private AutomationElement lastFocusedElement;
        public Form1()
        {
            InitializeComponent();

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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
