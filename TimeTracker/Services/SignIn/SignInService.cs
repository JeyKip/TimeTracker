using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Properties;
using TimeTracker.Services.Storage;
using TimeTracker.Services.Storage.Entities.SignIn;
using TimeTracker.WebView;

namespace TimeTracker.Services.SignIn
{
    public class SignInService : ISignInService
    {
        #region Fields and Properties

        private readonly IStorageService _storageService;

        public bool IsAuthorized
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Data?.AccessToken);
            }
        }

        public string UserDisplayName {
            get {
                return Data?.UserDisplayName ?? string.Empty;
            }
        }

        private SigninStore Data { get; set; }

        #endregion

        #region Contructors

        public SignInService(IStorageService storageService) {
            _storageService = storageService;
        }

        #endregion

        #region Methods

        public async Task<SignInResult> SignInAsync() {
            ResetInfo();

            var client = new OidcClient(GetIdentityOptions());
            var result = new SignInResult(await client.LoginAsync(new LoginRequest { BrowserDisplayMode = DisplayMode.Visible }));

            if (result.IsError)
            {
                //MessageBox.Show(this, result.Error, "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                var info = new SigninStore
                {
                    AccessToken = result.AccessToken,
                    RefreshToken = result.RefreshToken,
                    UserDisplayName = result.User?.Identity?.Name,
                    //RefreshHandler = result.RefreshTokenHandler
                };
                if (string.IsNullOrWhiteSpace(info.UserDisplayName))
                {
                    var displayNameClaim = result.User?.Claims?.FirstOrDefault(t => t.Type == Resources.DisplayNameClaimType);
                    if (displayNameClaim != null)
                        info.UserDisplayName = displayNameClaim.Value;
                }
                //var sb = new StringBuilder(128);
                //foreach (var claim in result.User.Claims)
                //{
                //    sb.AppendLine($"{claim.Type}: {claim.Value}");
                //}
                //if (!string.IsNullOrWhiteSpace(result.RefreshToken))
                //{
                //    sb.AppendLine($"refresh token: {result.RefreshToken}");
                    
                //}

                Data = _storageService.SaveSignInInfo(info);

                //_apiClient = new HttpClient(result.RefreshTokenHandler);
                //_apiClient.BaseAddress = new Uri("https://demo.identityserver.io/api/");
            }

            return result;
        }

        public async Task SignOutAsync()
        {
            var client = new OidcClient(GetIdentityOptions());
            await client.LogoutAsync();
            ResetInfo();
        }

        public async Task RefreshTokenAsync()
        {
            try
            {
                var data = _storageService.LoadSignInInfo();
                Data = data;
                return;

                if (data == null || string.IsNullOrWhiteSpace(data.RefreshToken))
                    return;
                var client = new OidcClient(GetIdentityOptions());
                var result = await client.GetUserInfoAsync(data.AccessToken);
                if (result.IsError)
                    return;
                var displayNameClaim = result.Claims?.FirstOrDefault(t => t.Type == Resources.DisplayNameClaimType);
                if (displayNameClaim != null)
                    data.UserDisplayName = displayNameClaim.Value;
                Data = _storageService.SaveSignInInfo(data);
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
                throw;
            }
        }

        public SigninStore GetStatus()
        {
            return Data;
        }

        private void ResetInfo()
        {
            Data = _storageService.SaveSignInInfo(new SigninStore());
        }

        private OidcClientOptions GetIdentityOptions() {
            //return new OidcClientOptions
            //{
            //    Authority = "https://demo.identityserver.io",
            //    ClientId = "native.hybrid",
            //    Scope = "openid email api offline_access",
            //    RedirectUri = "http://localhost/winforms.client",

            //    Browser = new WinFormsEmbeddedBrowser()
            //};
            return new OidcClientOptions
            {
                Authority = Settings.Default.IdentityUrl,
                ClientId = Settings.Default.IdentityClientId,
                Scope = Settings.Default.IdentityScope,
                RedirectUri = Settings.Default.IdentityRedirectUri,

                Browser = new WinFormsEmbeddedBrowser()
            };
        }
        #endregion

    }
}
