using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        #region Fields and properties

        private readonly SignInService _signInService;
        private readonly ILogger<Main> _logger;

        #endregion

        #region Constructors

        public Main(ILogger<Main> logger, SignInService signInService)
        {
            InitializeComponent();

            _logger = logger;
            _signInService = signInService;
        }

        #endregion

        #region Overridden Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RefreshMenuItems();
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
            _logger.LogDebug("test");
            try
            {
                var result = await _signInService.SignInAsync();
                if (result.IsError)
                {
                    MessageBox.Show(this, result.Error, "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UserAuthorized(false);
                }
                else {
                    UserAuthorized(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.LogError(ex.ToString());
                UserAuthorized(false);
            }
            RefreshMenuItems();
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
                _logger.LogError(ex.ToString());
            }
            RefreshMenuItems();
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

        private void RefreshMenuItems() {
            menuItemLogin.Enabled = !_signInService.IsAuthorized;
            menuItemLogout.Enabled = _signInService.IsAuthorized;
            panelInfo.Visible = _signInService.IsAuthorized;
        }

        private void UserAuthorized(bool authorized)
        {
            if (authorized)
            {
                lblUserValue.Text = _signInService.UserDisplayName;
            }
            else {
                lblUserValue.Text = string.Empty;
            }
        }

        #endregion

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            menuItemLogin_Click(sender, new EventArgs());
        }
    }
}