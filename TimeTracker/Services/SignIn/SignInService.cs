using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.WebView;

namespace TimeTracker.Services.SignIn
{
    public class SignInService
    {
        #region Fields and Properties

        private OidcClient _oidcClient;
        private string _accessToken;
        private string _refreshToken;

        #endregion

        #region Contructors

        public SignInService() {
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

        #region Methods

        public async Task<SignInResult> SignInAsync() {
            _accessToken = string.Empty;
            _refreshToken = string.Empty;

            var result = new SignInResult(await _oidcClient.LoginAsync(new LoginRequest { BrowserDisplayMode = DisplayMode.Visible }));

            if (result.IsError)
            {
                //MessageBox.Show(this, result.Error, "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                _accessToken = result.AccessToken;

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

            return result;
        }

        public async Task SignOutAsync()
        {
            await _oidcClient.LogoutAsync();
        }

        #endregion

    }
}
