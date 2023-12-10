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
            this.TabMainPanel.SuspendLayout();
            this.Mode1Tab.SuspendLayout();
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
    }
}

