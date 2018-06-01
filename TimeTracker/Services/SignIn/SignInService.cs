using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Properties;
using TimeTracker.WebView;

namespace TimeTracker.Services.SignIn
{
    public class SignInService : ISignInService
    {
        #region Fields and Properties

        private OidcClient _oidcClient;

        public bool IsAuthorized
        {
            get
            {
                return !string.IsNullOrWhiteSpace(AccessToken);
            }
        }

        public string UserDisplayName { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

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
            ResetInfo();

            var result = new SignInResult(await _oidcClient.LoginAsync(new LoginRequest { BrowserDisplayMode = DisplayMode.Visible }));

            if (result.IsError)
            {
                //MessageBox.Show(this, result.Error, "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                AccessToken = result.AccessToken;
                RefreshToken = result.RefreshToken;
                // set user display name as identity name
                UserDisplayName = result.User?.Identity?.Name;
                if (string.IsNullOrWhiteSpace(UserDisplayName))
                {
                    var displayNameClaim = result.User?.Claims?.FirstOrDefault(t => t.Type == Resources.DisplayNameClaimType);
                    if (displayNameClaim != null)
                        UserDisplayName = displayNameClaim.Value;
                }

                var sb = new StringBuilder(128);
                foreach (var claim in result.User.Claims)
                {
                    sb.AppendLine($"{claim.Type}: {claim.Value}");
                }
                if (!string.IsNullOrWhiteSpace(result.RefreshToken))
                {
                    sb.AppendLine($"refresh token: {result.RefreshToken}");
                    
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
            ResetInfo();
        }

        private void ResetInfo() {
            AccessToken = string.Empty;
            RefreshToken = string.Empty;
            UserDisplayName = string.Empty;
        }

        #endregion

    }
}
