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

        public string AccessToken { get; }
        public DateTime AccessTokenExpiration { get; }
        public DateTime AuthenticationTime { get; }
        public string Error { get; }
        public string IdentityToken { get; }
        public bool IsError { get; }
        public string RefreshToken { get; }
        public HttpMessageHandler RefreshTokenHandler { get; }
        public ClaimsPrincipal User { get; }
    }
}
