using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Services.SignIn
{
    public class SignInResult
    {
        //todo: remove useless fields
        public SignInResult(LoginResult loginResult) {
            AccessToken = loginResult.AccessToken;
            AccessTokenExpiration = loginResult.AccessTokenExpiration;
            AuthenticationTime = loginResult.AuthenticationTime;
            Error = loginResult.Error;
            IdentityToken = loginResult.IdentityToken;
            IsError = loginResult.IsError;
            RefreshToken = loginResult.RefreshToken;
            RefreshTokenHandler = loginResult.RefreshTokenHandler;
            User = loginResult.User;
        }

        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public DateTime AuthenticationTime { get; set; }
        public string Error { get; set; }
        public string IdentityToken { get; set; }
        public bool IsError { get; set; }
        public string RefreshToken { get; set; }
        public HttpMessageHandler RefreshTokenHandler { get; set; }
        public ClaimsPrincipal User { get; set; }
    }
}
