namespace applogger
{
    partial class applogger
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(applogger));
            this.button_toggle_start = new System.Windows.Forms.Button();
            this.button_open_log = new System.Windows.Forms.Button();
            this.timer_logger = new System.Windows.Forms.Timer(this.components);
            this.label_status = new System.Windows.Forms.Label();
            this.timer_saver = new System.Windows.Forms.Timer(this.components);
            this.button_open_folder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_toggle_start
            // 
            this.button_toggle_start.Location = new System.Drawing.Point(15, 67);
            this.button_toggle_start.Name = "button_toggle_start";
            this.button_toggle_start.Size = new System.Drawing.Size(115, 75);
            this.button_toggle_start.TabIndex = 0;
            this.button_toggle_start.Text = "Start logging";
            this.button_toggle_start.UseVisualStyleBackColor = true;
            this.button_toggle_start.Click += new System.EventHandler(this.button_toggle_start_Click);
            // 
            // button_open_log
            // 
            this.button_open_log.Location = new System.Drawing.Point(164, 67);
            this.button_open_log.Name = "button_open_log";
            this.button_open_log.Size = new System.Drawing.Size(115, 75);
            this.button_open_log.TabIndex = 0;
            this.button_open_log.Text = "Open log";
            this.button_open_log.UseVisualStyleBackColor = true;
            this.button_open_log.Click += new System.EventHandler(this.button_open_log_Click);
            // 
            // timer_logger
            // 
            this.timer_logger.Tick += new System.EventHandler(this.timer_logger_Tick);
            // 
            // label_status
            // 
            this.label_status.AutoSize = true;
            this.label_status.Location = new System.Drawing.Point(12, 27);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(59, 13);
            this.label_status.TabIndex = 1;
            this.label_status.Text = "Status: idle";
            // 
            // timer_saver
            // 
            this.timer_saver.Tick += new System.EventHandler(this.timer_saver_Tick);
            // 
            // button_open_folder
            // 
            this.button_open_folder.Location = new System.Drawing.Point(314, 67);
            this.button_open_folder.Name = "button_open_folder";
            this.button_open_folder.Size = new System.Drawing.Size(115, 75);
            this.button_open_folder.TabIndex = 2;
            this.button_open_folder.Text = "Open folder";
            this.button_open_folder.UseVisualStyleBackColor = true;
            this.button_open_folder.Click += new System.EventHandler(this.button_open_folder_Click);
            // 
            // applogger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 154);
            this.Controls.Add(this.button_open_folder);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.button_open_log);
            this.Controls.Add(this.button_toggle_start);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "applogger";
            this.Text = "Window Logger";
            this.Resize += new System.EventHandler(this.applogger_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_toggle_start;
        private System.Windows.Forms.Button button_open_log;
        private System.Windows.Forms.Timer timer_logger;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Timer timer_saver;
        private System.Windows.Forms.Button button_open_folder;
    }
}

