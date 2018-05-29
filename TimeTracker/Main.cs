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
using TimeTracker.WebView;

namespace TimeTracker
{
    public partial class Main : Form
    {
        #region Fields and Properties

        private OidcClient _oidcClient;
        private string _accessTokenDisplay;
        private string _refreshToken;

        #endregion

        #region Constructors

        public Main()
        {
            InitializeComponent();

            var options = new OidcClientOptions
            {
                Authority = "https://demo.identityserver.io",
                ClientId = "native.hybrid",
                Scope = "openid email api offline_access",
                RedirectUri = "http://localhost/winforms.client",

                Browser = new WinFormsEmbeddedBrowser()
            };

            _oidcClient = new OidcClient(options);
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var result = await _oidcClient.LoginAsync(new LoginRequest { BrowserDisplayMode = DisplayMode.Visible });

            if (result.IsError)
            {
                MessageBox.Show(this, result.Error, "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _accessTokenDisplay = result.AccessToken;

                var sb = new StringBuilder(128);
                foreach (var claim in result.User.Claims)
                {
                    sb.AppendLine($"{claim.Type}: {claim.Value}");
                }

                if (!string.IsNullOrWhiteSpace(result.RefreshToken))
                {
                    sb.AppendLine($"refresh token: {result.RefreshToken}");
                    _refreshToken = result.RefreshToken;
                }

                System.Diagnostics.Debug.WriteLine(sb.ToString());

                //_apiClient = new HttpClient(result.RefreshTokenHandler);
                //_apiClient.BaseAddress = new Uri("https://demo.identityserver.io/api/");
            }
        }

        private async void btnLogout_Click(object sender, EventArgs e)
        {
            await _oidcClient.LogoutAsync();
            _accessTokenDisplay = string.Empty;
        }
    }
}