using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeTracker.Services.SignIn;
using TimeTracker.WebView;

namespace TimeTracker
{
    public partial class Main : Form
    {
        private readonly SignInService _signInService;
        #region Constructors

        public Main()
        {
            InitializeComponent();

            _signInService = new SignInService();
        }

        #endregion

        #region Overridden Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // in case if user minimized window we need to put it in the system tray and hide it from taskbar
            if (WindowState == FormWindowState.Minimized)
                PutInTray();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // in case if user clicked "Close" button we need to minimize window and put it to the system tray
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                WindowState = FormWindowState.Minimized;
            }
        }

        #endregion

        #region Event Handlers

        private void SystemTrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // if application is in the system tray we need to restore it to normal size
            if (WindowState == FormWindowState.Minimized)
                OpenFromTray();
        }

        private void SystemTrayMenuClose_Click(object sender, EventArgs e)
        {
            // when user clicked "Exit" button from system tray we need to shutdown application
            Application.Exit();
        }

        private void SystemTrayMenuOpen_Click(object sender, EventArgs e)
        {
            // just restore window to normal size if user clicked "Open" button from system tray context menu
            OpenFromTray();
        }

        private async void menuItemLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var result = await _signInService.SignInAsync();
                if (result.IsError)
                {
                    MessageBox.Show(this, result.Error, "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Logger.Error(ex);
            }
        }

        private async void menuItemLogout_Click(object sender, EventArgs e)
        {
            try
            {
                await _signInService.SignOutAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Logout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Logger.Error(ex);
            }
        }

        #endregion

        #region Private Methods

        private void PutInTray()
        {
            ShowInTaskbar = false;

            systemTrayIcon.Visible = true;
            systemTrayIcon.ShowBalloonTip(3000);
        }

        private void OpenFromTray()
        {
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;

            systemTrayIcon.Visible = false;
        }

        #endregion

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}