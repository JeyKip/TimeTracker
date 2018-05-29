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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnTrack = new System.Windows.Forms.Button();
            this.lblUserValue = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.linkLabelLogin = new System.Windows.Forms.LinkLabel();
            this.labelLogin = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemLogin = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.systemTrayIconContextMenu.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.menuStrip1.SuspendLayout();
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
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.panelInfo);
            this.mainPanel.Controls.Add(this.linkLabelLogin);
            this.mainPanel.Controls.Add(this.labelLogin);
            this.mainPanel.Controls.Add(this.menuStrip1);
            this.mainPanel.Location = new System.Drawing.Point(-3, -1);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(466, 187);
            this.mainPanel.TabIndex = 1;
            // 
            // panelInfo
            // 
            this.panelInfo.Controls.Add(this.btnStop);
            this.panelInfo.Controls.Add(this.btnTrack);
            this.panelInfo.Controls.Add(this.lblUserValue);
            this.panelInfo.Controls.Add(this.lblUser);
            this.panelInfo.Location = new System.Drawing.Point(3, 27);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(463, 160);
            this.panelInfo.TabIndex = 5;
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStop.Location = new System.Drawing.Point(352, 120);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 28);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // btnTrack
            // 
            this.btnTrack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnTrack.Location = new System.Drawing.Point(246, 120);
            this.btnTrack.Name = "btnTrack";
            this.btnTrack.Size = new System.Drawing.Size(100, 28);
            this.btnTrack.TabIndex = 2;
            this.btnTrack.Text = "Start";
            this.btnTrack.UseVisualStyleBackColor = true;
            this.btnTrack.Click += new System.EventHandler(this.BtnTrack_Click);
            // 
            // lblUserValue
            // 
            this.lblUserValue.AutoSize = true;
            this.lblUserValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUserValue.Location = new System.Drawing.Point(56, 10);
            this.lblUserValue.Name = "lblUserValue";
            this.lblUserValue.Size = new System.Drawing.Size(72, 20);
            this.lblUserValue.TabIndex = 1;
            this.lblUserValue.Text = "loading...";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUser.Location = new System.Drawing.Point(12, 10);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(51, 20);
            this.lblUser.TabIndex = 0;
            this.lblUser.Text = "User: ";
            // 
            // linkLabelLogin
            // 
            this.linkLabelLogin.AutoSize = true;
            this.linkLabelLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkLabelLogin.Location = new System.Drawing.Point(185, 74);
            this.linkLabelLogin.Name = "linkLabelLogin";
            this.linkLabelLogin.Size = new System.Drawing.Size(55, 25);
            this.linkLabelLogin.TabIndex = 3;
            this.linkLabelLogin.TabStop = true;
            this.linkLabelLogin.Text = "here";
            this.linkLabelLogin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelLogin_LinkClicked);
            // 
            // labelLogin
            // 
            this.labelLogin.AutoSize = true;
            this.labelLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLogin.Location = new System.Drawing.Point(133, 74);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new System.Drawing.Size(184, 25);
            this.labelLogin.TabIndex = 4;
            this.labelLogin.Text = "Click here to login";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(466, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mainToolStripMenuItem
            // 
            this.mainToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemLogin,
            this.menuItemLogout,
            this.toolStripSeparator1,
            this.menuItemExit});
            this.mainToolStripMenuItem.Name = "mainToolStripMenuItem";
            this.mainToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.mainToolStripMenuItem.Text = "Main";
            // 
            // menuItemLogin
            // 
            this.menuItemLogin.Name = "menuItemLogin";
            this.menuItemLogin.Size = new System.Drawing.Size(112, 22);
            this.menuItemLogin.Text = "Login";
            this.menuItemLogin.Click += new System.EventHandler(this.MenuItemLogin_Click);
            // 
            // menuItemLogout
            // 
            this.menuItemLogout.Name = "menuItemLogout";
            this.menuItemLogout.Size = new System.Drawing.Size(112, 22);
            this.menuItemLogout.Text = "Logout";
            this.menuItemLogout.Click += new System.EventHandler(this.MenuItemLogout_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(109, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.Size = new System.Drawing.Size(112, 22);
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += new System.EventHandler(this.MenuItemExit_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 186);
            this.Controls.Add(this.mainPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Time Tracker";
            this.systemTrayIconContextMenu.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon systemTrayIcon;
        private System.Windows.Forms.ContextMenuStrip systemTrayIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem systemTrayMenuClose;
        private System.Windows.Forms.ToolStripMenuItem systemTrayMenuOpen;
        private System.Windows.Forms.ToolStripSeparator systemTrayMenuSeparator;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemLogin;
        private System.Windows.Forms.ToolStripMenuItem menuItemLogout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.LinkLabel linkLabelLogin;
        private System.Windows.Forms.Label labelLogin;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblUserValue;
        private System.Windows.Forms.Button btnTrack;
        private System.Windows.Forms.Button btnStop;
    }
}

