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
            this.Mode2Tab = new System.Windows.Forms.TabPage();
            this.TabMainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabMainPanel
            // 
            this.TabMainPanel.Controls.Add(this.LogsTab);
            this.TabMainPanel.Controls.Add(this.Mode1Tab);
            this.TabMainPanel.Controls.Add(this.Mode2Tab);
            this.TabMainPanel.Location = new System.Drawing.Point(12, 12);
            this.TabMainPanel.Name = "TabMainPanel";
            this.TabMainPanel.SelectedIndex = 0;
            this.TabMainPanel.Size = new System.Drawing.Size(555, 759);
            this.TabMainPanel.TabIndex = 0;
            // 
            // LogsTab
            // 
            this.LogsTab.Location = new System.Drawing.Point(4, 25);
            this.LogsTab.Name = "LogsTab";
            this.LogsTab.Padding = new System.Windows.Forms.Padding(3);
            this.LogsTab.Size = new System.Drawing.Size(547, 730);
            this.LogsTab.TabIndex = 0;
            this.LogsTab.Text = "Logs";
            this.LogsTab.UseVisualStyleBackColor = true;
            this.LogsTab.Click += new System.EventHandler(this.LogsTab_Click);
            // 
            // Mode1Tab
            // 
            this.Mode1Tab.Location = new System.Drawing.Point(4, 25);
            this.Mode1Tab.Name = "Mode1Tab";
            this.Mode1Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Mode1Tab.Size = new System.Drawing.Size(547, 730);
            this.Mode1Tab.TabIndex = 1;
            this.Mode1Tab.Text = "Mode-1";
            this.Mode1Tab.UseVisualStyleBackColor = true;
            // 
            // Mode2Tab
            // 
            this.Mode2Tab.Location = new System.Drawing.Point(4, 25);
            this.Mode2Tab.Name = "Mode2Tab";
            this.Mode2Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Mode2Tab.Size = new System.Drawing.Size(547, 730);
            this.Mode2Tab.TabIndex = 2;
            this.Mode2Tab.Text = "Mode-2";
            this.Mode2Tab.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 783);
            this.Controls.Add(this.TabMainPanel);
            this.Name = "MainWindow";
            this.Text = "ПУШИФИКАТОР";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.TabMainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabMainPanel;
        private System.Windows.Forms.TabPage LogsTab;
        private System.Windows.Forms.TabPage Mode1Tab;
        private System.Windows.Forms.TabPage Mode2Tab;
    }
}

