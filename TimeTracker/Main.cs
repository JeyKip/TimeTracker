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
using TimeTracker.Services;
using TimeTracker.Services.SignIn;
using TimeTracker.WebView;

namespace TimeTracker
{
    public partial class Main : Form
    {
        #region Fields and properties

        private readonly ISignInService _signInService;
        private readonly ITaskRunner _taskRunner;
        private readonly ILogger<Main> _logger;

        #endregion

        #region Constructors

        public Main(ILogger<Main> logger, ISignInService signInService, ITaskRunner taskRunner)
        {
            InitializeComponent();

            _logger = logger;
            _signInService = signInService;
            _taskRunner = taskRunner;
        }

        #endregion

        #region Overridden Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _signInService.RefreshTokenAsync().GetAwaiter().GetResult();
            UserAuthorized(_signInService.IsAuthorized);
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
            Exit();
        }

        private void SystemTrayMenuOpen_Click(object sender, EventArgs e)
        {
            // just restore window to normal size if user clicked "Open" button from system tray context menu
            OpenFromTray();
        }

        private async void MenuItemLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var result = await _signInService.SignInAsync();
                if (result.IsError)
                {
                    MessageBox.Show(this, result.Error, "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UserAuthorized(false);
                }
                else
                {
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

        private async void MenuItemLogout_Click(object sender, EventArgs e)
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

        private void MenuItemExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void LinkLabelLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MenuItemLogin_Click(sender, e);
        }

        private void BtnTrack_Click(object sender, EventArgs e)
        {
            try
            {
                _taskRunner.Start();

                ToggleTaskRunnerButtons(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Start Tracking", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.LogError(ex, "Error occurred during starting task runner.");
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                _taskRunner.Stop();

                ToggleTaskRunnerButtons(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Stop Tracking", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.LogError(ex, "Error occurred during stopping task runner.");
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

        private void RefreshMenuItems()
        {
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
            else
            {
                lblUserValue.Text = string.Empty;
            }
        }

        private void ToggleTaskRunnerButtons(bool started)
        {
            btnStop.Enabled = started;
            btnTrack.Enabled = !started;
        }

        private void Exit()
        {
            Application.Exit();
        }

        #endregion
    }
}