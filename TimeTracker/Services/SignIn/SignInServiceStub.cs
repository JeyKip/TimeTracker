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
    public class SignInServiceStub: ISignInService
    {
        #region Fields and Properties

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

        public SignInServiceStub() {
        }


        #endregion

        #region Methods

        public async Task<SignInResult> SignInAsync() {
            ResetInfo();

            var result = new SignInResult(new LoginResult());
            await Task.Run(() =>
            {
                result.AccessToken = AccessToken = "123";
                result.RefreshToken = RefreshToken = "456";
                UserDisplayName = UserDisplayName = "Stub user Display Name";
            });

            return result;
        }

        public async Task SignOutAsync()
        {
            await Task.Run(() =>
            {
                ResetInfo();
            });
        }

        private void ResetInfo() {
            AccessToken = string.Empty;
            RefreshToken = string.Empty;
            UserDisplayName = string.Empty;
        }

        #endregion

    }
}
