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
            this.systemTrayIconContextMenu.SuspendLayout();
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
            this.systemTrayIconContextMenu.Size = new System.Drawing.Size(181, 76);
            // 
            // systemTrayMenuOpen
            // 
            this.systemTrayMenuOpen.Name = "systemTrayMenuOpen";
            this.systemTrayMenuOpen.Size = new System.Drawing.Size(180, 22);
            this.systemTrayMenuOpen.Text = "Open";
            this.systemTrayMenuOpen.Click += new System.EventHandler(this.SystemTrayMenuOpen_Click);
            // 
            // systemTrayMenuSeparator
            // 
            this.systemTrayMenuSeparator.Name = "systemTrayMenuSeparator";
            this.systemTrayMenuSeparator.Size = new System.Drawing.Size(177, 6);
            // 
            // systemTrayMenuClose
            // 
            this.systemTrayMenuClose.Name = "systemTrayMenuClose";
            this.systemTrayMenuClose.Size = new System.Drawing.Size(180, 22);
            this.systemTrayMenuClose.Text = "Exit";
            this.systemTrayMenuClose.Click += new System.EventHandler(this.SystemTrayMenuClose_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Time Tracker";
            this.systemTrayIconContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon systemTrayIcon;
        private System.Windows.Forms.ContextMenuStrip systemTrayIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem systemTrayMenuClose;
        private System.Windows.Forms.ToolStripMenuItem systemTrayMenuOpen;
        private System.Windows.Forms.ToolStripSeparator systemTrayMenuSeparator;
    }
}

