﻿namespace TimeTracker
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemLogin = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.systemTrayIconContextMenu.SuspendLayout();
            this.mainPanel.SuspendLayout();
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
            this.mainPanel.Controls.Add(this.menuStrip1);
            this.mainPanel.Location = new System.Drawing.Point(-3, -1);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(466, 187);
            this.mainPanel.TabIndex = 1;
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
            this.menuItemLogin.Size = new System.Drawing.Size(180, 22);
            this.menuItemLogin.Text = "Login";
            this.menuItemLogin.Click += new System.EventHandler(this.menuItemLogin_Click);
            // 
            // menuItemLogout
            // 
            this.menuItemLogout.Name = "menuItemLogout";
            this.menuItemLogout.Size = new System.Drawing.Size(180, 22);
            this.menuItemLogout.Text = "Logout";
            this.menuItemLogout.Click += new System.EventHandler(this.menuItemLogout_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.Size = new System.Drawing.Size(180, 22);
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
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
    }
}

