namespace Pushification
{
    partial class MainWindow
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.TabMainPanel = new System.Windows.Forms.TabControl();
            this.LogsTab = new System.Windows.Forms.TabPage();
            this.Mode1Tab = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Mode2Tab = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.MaxTimePageLoadingTextBox = new System.Windows.Forms.TextBox();
            this.BeforeAllowTimeoutTextBox = new System.Windows.Forms.TextBox();
            this.AfterAllowTimeoutTextBox = new System.Windows.Forms.TextBox();
            this.ProxyWaitingTimeoutTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.IP = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.CountIPTextBlock = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.CountIPDeletionTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.StartOptionOneTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.TimeOptionOneTextBox = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.TabMainPanel.SuspendLayout();
            this.Mode1Tab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.IP.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabMainPanel
            // 
            this.TabMainPanel.Controls.Add(this.LogsTab);
            this.TabMainPanel.Controls.Add(this.Mode1Tab);
            this.TabMainPanel.Controls.Add(this.Mode2Tab);
            this.TabMainPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TabMainPanel.Location = new System.Drawing.Point(12, 12);
            this.TabMainPanel.Name = "TabMainPanel";
            this.TabMainPanel.Padding = new System.Drawing.Point(8, 8);
            this.TabMainPanel.SelectedIndex = 0;
            this.TabMainPanel.Size = new System.Drawing.Size(940, 809);
            this.TabMainPanel.TabIndex = 0;
            // 
            // LogsTab
            // 
            this.LogsTab.Location = new System.Drawing.Point(4, 44);
            this.LogsTab.Name = "LogsTab";
            this.LogsTab.Padding = new System.Windows.Forms.Padding(3);
            this.LogsTab.Size = new System.Drawing.Size(932, 761);
            this.LogsTab.TabIndex = 0;
            this.LogsTab.Text = "Logs";
            this.LogsTab.UseVisualStyleBackColor = true;
            // 
            // Mode1Tab
            // 
            this.Mode1Tab.Controls.Add(this.groupBox2);
            this.Mode1Tab.Controls.Add(this.IP);
            this.Mode1Tab.Controls.Add(this.groupBox1);
            this.Mode1Tab.Controls.Add(this.button1);
            this.Mode1Tab.Controls.Add(this.textBox1);
            this.Mode1Tab.Controls.Add(this.label3);
            this.Mode1Tab.Controls.Add(this.label2);
            this.Mode1Tab.Controls.Add(this.label1);
            this.Mode1Tab.Location = new System.Drawing.Point(4, 44);
            this.Mode1Tab.Name = "Mode1Tab";
            this.Mode1Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Mode1Tab.Size = new System.Drawing.Size(932, 761);
            this.Mode1Tab.TabIndex = 1;
            this.Mode1Tab.Text = "Mode-1";
            this.Mode1Tab.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(316, 152);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(280, 47);
            this.button1.TabIndex = 4;
            this.button1.Text = "Выбрать файл с прокси\r\n";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OpenFileButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.Location = new System.Drawing.Point(316, 63);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(538, 41);
            this.textBox1.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(627, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Всего:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 174);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Загрузить прокси";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Ссылка для перехода";
            // 
            // Mode2Tab
            // 
            this.Mode2Tab.Location = new System.Drawing.Point(4, 44);
            this.Mode2Tab.Name = "Mode2Tab";
            this.Mode2Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Mode2Tab.Size = new System.Drawing.Size(932, 761);
            this.Mode2Tab.TabIndex = 2;
            this.Mode2Tab.Text = "Mode-2";
            this.Mode2Tab.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(450, 93);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(203, 20);
            this.label11.TabIndex = 0;
            this.label11.Text = "Ожидание внешнего IP";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(13, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(260, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Ожидание загрузки страницы";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(13, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(267, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "Время перед под-ской на push";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(450, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(260, 20);
            this.label8.TabIndex = 0;
            this.label8.Text = "Ожидание после \"разрешить\"";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(383, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 20);
            this.label6.TabIndex = 0;
            this.label6.Text = "сек.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(336, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 20);
            this.label7.TabIndex = 0;
            this.label7.Text = "шт. max";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(805, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 20);
            this.label9.TabIndex = 0;
            this.label9.Text = "сек.";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.Location = new System.Drawing.Point(805, 93);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 20);
            this.label12.TabIndex = 0;
            this.label12.Text = "сек.";
            // 
            // MaxTimePageLoadingTextBox
            // 
            this.MaxTimePageLoadingTextBox.Location = new System.Drawing.Point(294, 34);
            this.MaxTimePageLoadingTextBox.Name = "MaxTimePageLoadingTextBox";
            this.MaxTimePageLoadingTextBox.Size = new System.Drawing.Size(83, 30);
            this.MaxTimePageLoadingTextBox.TabIndex = 2;
            // 
            // BeforeAllowTimeoutTextBox
            // 
            this.BeforeAllowTimeoutTextBox.Location = new System.Drawing.Point(294, 83);
            this.BeforeAllowTimeoutTextBox.Name = "BeforeAllowTimeoutTextBox";
            this.BeforeAllowTimeoutTextBox.Size = new System.Drawing.Size(83, 30);
            this.BeforeAllowTimeoutTextBox.TabIndex = 2;
            // 
            // AfterAllowTimeoutTextBox
            // 
            this.AfterAllowTimeoutTextBox.Location = new System.Drawing.Point(716, 34);
            this.AfterAllowTimeoutTextBox.Name = "AfterAllowTimeoutTextBox";
            this.AfterAllowTimeoutTextBox.Size = new System.Drawing.Size(83, 30);
            this.AfterAllowTimeoutTextBox.TabIndex = 2;
            // 
            // ProxyWaitingTimeoutTextBox
            // 
            this.ProxyWaitingTimeoutTextBox.Location = new System.Drawing.Point(716, 83);
            this.ProxyWaitingTimeoutTextBox.Name = "ProxyWaitingTimeoutTextBox";
            this.ProxyWaitingTimeoutTextBox.Size = new System.Drawing.Size(83, 30);
            this.ProxyWaitingTimeoutTextBox.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ProxyWaitingTimeoutTextBox);
            this.groupBox1.Controls.Add(this.AfterAllowTimeoutTextBox);
            this.groupBox1.Controls.Add(this.BeforeAllowTimeoutTextBox);
            this.groupBox1.Controls.Add(this.MaxTimePageLoadingTextBox);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(22, 241);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(886, 156);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Таймауты";
            // 
            // IP
            // 
            this.IP.Controls.Add(this.label13);
            this.IP.Controls.Add(this.label10);
            this.IP.Controls.Add(this.CountIPDeletionTextBox);
            this.IP.Controls.Add(this.CountIPTextBlock);
            this.IP.Controls.Add(this.label14);
            this.IP.Controls.Add(this.label7);
            this.IP.Location = new System.Drawing.Point(22, 421);
            this.IP.Name = "IP";
            this.IP.Size = new System.Drawing.Size(886, 85);
            this.IP.TabIndex = 6;
            this.IP.TabStop = false;
            this.IP.Text = "IP";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(13, 44);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(208, 20);
            this.label10.TabIndex = 0;
            this.label10.Text = "Кол-во IP для удаления";
            // 
            // CountIPTextBlock
            // 
            this.CountIPTextBlock.Location = new System.Drawing.Point(247, 34);
            this.CountIPTextBlock.Name = "CountIPTextBlock";
            this.CountIPTextBlock.Size = new System.Drawing.Size(83, 30);
            this.CountIPTextBlock.TabIndex = 2;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label13.Location = new System.Drawing.Point(450, 41);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(268, 20);
            this.label13.TabIndex = 0;
            this.label13.Text = "Кол-во IP для ужадения за раз";
            // 
            // CountIPDeletionTextBox
            // 
            this.CountIPDeletionTextBox.Location = new System.Drawing.Point(716, 34);
            this.CountIPDeletionTextBox.Name = "CountIPDeletionTextBox";
            this.CountIPDeletionTextBox.Size = new System.Drawing.Size(83, 30);
            this.CountIPDeletionTextBox.TabIndex = 2;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label14.Location = new System.Drawing.Point(805, 44);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(37, 20);
            this.label14.TabIndex = 0;
            this.label14.Text = "шт.";
            this.label14.Click += new System.EventHandler(this.label7_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.StartOptionOneTimePicker);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.TimeOptionOneTextBox);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Location = new System.Drawing.Point(22, 543);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(886, 122);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Время работы режима";
            // 
            // StartOptionOneTimePicker
            // 
            this.StartOptionOneTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.StartOptionOneTimePicker.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.StartOptionOneTimePicker.Location = new System.Drawing.Point(247, 46);
            this.StartOptionOneTimePicker.Name = "StartOptionOneTimePicker";
            this.StartOptionOneTimePicker.Size = new System.Drawing.Size(136, 30);
            this.StartOptionOneTimePicker.TabIndex = 1;
            this.StartOptionOneTimePicker.Value = new System.DateTime(2023, 12, 10, 17, 14, 0, 0);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label16.Location = new System.Drawing.Point(13, 56);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(160, 20);
            this.label16.TabIndex = 0;
            this.label16.Text = "Начать работу в :";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label15.Location = new System.Drawing.Point(450, 55);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(164, 20);
            this.label15.TabIndex = 0;
            this.label15.Text = "Закончить через :";
            // 
            // TimeOptionOneTextBox
            // 
            this.TimeOptionOneTextBox.Location = new System.Drawing.Point(627, 45);
            this.TimeOptionOneTextBox.Name = "TimeOptionOneTextBox";
            this.TimeOptionOneTextBox.Size = new System.Drawing.Size(83, 30);
            this.TimeOptionOneTextBox.TabIndex = 2;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label17.Location = new System.Drawing.Point(716, 56);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(45, 20);
            this.label17.TabIndex = 0;
            this.label17.Text = "мин.";
            this.label17.Click += new System.EventHandler(this.label7_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 833);
            this.Controls.Add(this.TabMainPanel);
            this.Name = "MainWindow";
            this.Text = "ПУШИФИКАТОР";
            this.TabMainPanel.ResumeLayout(false);
            this.Mode1Tab.ResumeLayout(false);
            this.Mode1Tab.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.IP.ResumeLayout(false);
            this.IP.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabMainPanel;
        private System.Windows.Forms.TabPage LogsTab;
        private System.Windows.Forms.TabPage Mode1Tab;
        private System.Windows.Forms.TabPage Mode2Tab;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox ProxyWaitingTimeoutTextBox;
        private System.Windows.Forms.TextBox AfterAllowTimeoutTextBox;
        private System.Windows.Forms.TextBox BeforeAllowTimeoutTextBox;
        private System.Windows.Forms.TextBox MaxTimePageLoadingTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox IP;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox CountIPTextBlock;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox CountIPDeletionTextBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker StartOptionOneTimePicker;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox TimeOptionOneTextBox;
        private System.Windows.Forms.Label label17;
    }
}

