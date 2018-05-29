namespace TimeTracker
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.systemTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.systemTrayIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.systemTrayMenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.systemTrayMenuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.systemTrayMenuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.loginPanel = new System.Windows.Forms.Panel();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.systemTrayIconContextMenu.SuspendLayout();
            this.loginPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // systemTrayIcon
            // 
            this.systemTrayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.systemTrayIcon.BalloonTipText = "The application is working in the background";
            this.systemTrayIcon.BalloonTipTitle = "Time Tracker";
            this.systemTrayIcon.ContextMenuStrip = this.systemTrayIconContextMenu;
            this.systemTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("systemTrayIcon.Icon")));
            this.systemTrayIcon.Text = "Time Tracker";
            this.systemTrayIcon.Visible = true;
            this.systemTrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SystemTrayIcon_MouseDoubleClick);
            // 
            // systemTrayIconContextMenu
            // 
            this.systemTrayIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemTrayMenuOpen,
            this.systemTrayMenuSeparator,
            this.systemTrayMenuClose});
            this.systemTrayIconContextMenu.Name = "systemTrayIconContextMenu";
            this.systemTrayIconContextMenu.Size = new System.Drawing.Size(104, 54);
            // 
            // systemTrayMenuOpen
            // 
            this.systemTrayMenuOpen.Name = "systemTrayMenuOpen";
            this.systemTrayMenuOpen.Size = new System.Drawing.Size(103, 22);
            this.systemTrayMenuOpen.Text = "Open";
            this.systemTrayMenuOpen.Click += new System.EventHandler(this.SystemTrayMenuOpen_Click);
            // 
            // systemTrayMenuSeparator
            // 
            this.systemTrayMenuSeparator.Name = "systemTrayMenuSeparator";
            this.systemTrayMenuSeparator.Size = new System.Drawing.Size(100, 6);
            // 
            // systemTrayMenuClose
            // 
            this.systemTrayMenuClose.Name = "systemTrayMenuClose";
            this.systemTrayMenuClose.Size = new System.Drawing.Size(103, 22);
            this.systemTrayMenuClose.Text = "Exit";
            this.systemTrayMenuClose.Click += new System.EventHandler(this.SystemTrayMenuClose_Click);
            // 
            // loginPanel
            // 
            this.loginPanel.Controls.Add(this.btnLogout);
            this.loginPanel.Controls.Add(this.btnLogin);
            this.loginPanel.Location = new System.Drawing.Point(-3, -1);
            this.loginPanel.Name = "loginPanel";
            this.loginPanel.Size = new System.Drawing.Size(466, 187);
            this.loginPanel.TabIndex = 1;
            this.loginPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(335, 147);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(128, 37);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(201, 147);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(128, 37);
            this.btnLogout.TabIndex = 0;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Visible = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 186);
            this.Controls.Add(this.loginPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Time Tracker";
            this.systemTrayIconContextMenu.ResumeLayout(false);
            this.loginPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon systemTrayIcon;
        private System.Windows.Forms.ContextMenuStrip systemTrayIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem systemTrayMenuClose;
        private System.Windows.Forms.ToolStripMenuItem systemTrayMenuOpen;
        private System.Windows.Forms.ToolStripSeparator systemTrayMenuSeparator;
        private System.Windows.Forms.Panel loginPanel;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnLogout;
    }
}

