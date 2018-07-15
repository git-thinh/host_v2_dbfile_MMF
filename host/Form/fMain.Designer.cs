namespace host.Forms
{
    partial class fMain
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
            this.panelBar = new System.Windows.Forms.Panel();
            this.label_URI_SITE = new System.Windows.Forms.Label();
            this.button_Config = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button_search = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.label_Domain = new System.Windows.Forms.Label();
            this.panelBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBar
            // 
            this.panelBar.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelBar.Controls.Add(this.label_Domain);
            this.panelBar.Controls.Add(this.label_URI_SITE);
            this.panelBar.Controls.Add(this.button_Config);
            this.panelBar.Controls.Add(this.textBox1);
            this.panelBar.Controls.Add(this.button_search);
            this.panelBar.Controls.Add(this.buttonClose);
            this.panelBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBar.Location = new System.Drawing.Point(0, 0);
            this.panelBar.Name = "panelBar";
            this.panelBar.Size = new System.Drawing.Size(700, 39);
            this.panelBar.TabIndex = 0;
            this.panelBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelBar_MouseMove);
            // 
            // label_URI_SITE
            // 
            this.label_URI_SITE.AutoSize = true;
            this.label_URI_SITE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_URI_SITE.Location = new System.Drawing.Point(12, 20);
            this.label_URI_SITE.Name = "label_URI_SITE";
            this.label_URI_SITE.Size = new System.Drawing.Size(103, 15);
            this.label_URI_SITE.TabIndex = 2;
            this.label_URI_SITE.Text = "http://127.0.0.1";
            this.label_URI_SITE.Click += new System.EventHandler(this.label_URI_SITE_Click);
            this.label_URI_SITE.MouseLeave += new System.EventHandler(this.label_URI_SITE_MouseLeave);
            this.label_URI_SITE.MouseHover += new System.EventHandler(this.label_URI_SITE_MouseHover);
            // 
            // button_Config
            // 
            this.button_Config.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Config.Location = new System.Drawing.Point(547, 8);
            this.button_Config.Name = "button_Config";
            this.button_Config.Size = new System.Drawing.Size(75, 23);
            this.button_Config.TabIndex = 1;
            this.button_Config.Text = "Cấu hình";
            this.button_Config.UseVisualStyleBackColor = true;
            this.button_Config.Click += new System.EventHandler(this.button_Config_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(278, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(182, 20);
            this.textBox1.TabIndex = 0;
            // 
            // button_search
            // 
            this.button_search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_search.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_search.Location = new System.Drawing.Point(466, 8);
            this.button_search.Name = "button_search";
            this.button_search.Size = new System.Drawing.Size(75, 24);
            this.button_search.TabIndex = 0;
            this.button_search.Text = "Tìm kiếm";
            this.button_search.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClose.Location = new System.Drawing.Point(628, 8);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(67, 23);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Thoát";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // panelMain
            // 
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 39);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(700, 369);
            this.panelMain.TabIndex = 1;
            // 
            // label_Domain
            // 
            this.label_Domain.AutoSize = true;
            this.label_Domain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Domain.Location = new System.Drawing.Point(12, 4);
            this.label_Domain.Name = "label_Domain";
            this.label_Domain.Size = new System.Drawing.Size(103, 15);
            this.label_Domain.TabIndex = 3;
            this.label_Domain.Text = "http://127.0.0.1";
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 408);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "fMain";
            this.Text = "DB Admin";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.fMain_Load);
            this.panelBar.ResumeLayout(false);
            this.panelBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelBar;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button_search;
        private System.Windows.Forms.Button button_Config;
        private System.Windows.Forms.Label label_URI_SITE;
        private System.Windows.Forms.Label label_Domain;
    }
}