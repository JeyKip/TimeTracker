using IdentityModel.Client;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeTracker.Properties;
using TimeTracker.Services.Storage;
using TimeTracker.Services.Storage.Entities.SignIn;
using TimeTracker.WebView;

namespace TimeTracker.Services.SignIn
{
    /// <summary>
    /// HTTP message handler that encapsulates token handling and refresh
    /// </summary>
    public class IdentityHttpHandler : DelegatingHandler
    {
        private static readonly TimeSpan _lockTimeout = TimeSpan.FromSeconds(2);

        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private readonly TokenClient _tokenClient;
        private readonly IStorageService _storageService;

        private string _accessToken;
        private string _refreshToken;
        private bool _disposed;

        /// <summary>
        /// Gets the current access token
        /// </summary>
        public string AccessToken
        {
            get
            {
                if (_lock.Wait(_lockTimeout))
                {
                    try
                    {
                        return _accessToken;
                    }
                    finally
                    {
                        _lock.Release();
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the current refresh token
        /// </summary>
        public string RefreshToken
        {
            get
            {
                if (_lock.Wait(_lockTimeout))
                {
                    try
                    {
                        return _refreshToken;
                    }
                    finally
                    {
                        _lock.Release();
                    }
                }

                return null;
            }
        }

        public IdentityHttpHandler(string tokenEndpoint, string clientId, string clientSecret, string refreshToken, string accessToken = null, HttpMessageHandler innerHandler = null)
            : this(new TokenClient(tokenEndpoint, clientId, clientSecret), refreshToken, accessToken, innerHandler)
        { }

        public IdentityHttpHandler(TokenClient client, string refreshToken, string accessToken = null, HttpMessageHandler innerHandler = null)
        {
            _tokenClient = client;
            _refreshToken = refreshToken;
            _accessToken = accessToken;

            InnerHandler = innerHandler ?? new HttpClientHandler();
            _storageService = new StorageService();
        }
        public IdentityHttpHandler(IStorageService storageService, string refreshToken, string accessToken = null)
        {
            _refreshToken = refreshToken;
            InnerHandler = new HttpClientHandler();
            _accessToken = accessToken;
            _storageService = storageService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = AccessToken;
            if (string.IsNullOrEmpty(accessToken))
            {
                if (await RefreshTokensAsync(cancellationToken) == false)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                return response;
            }

            if (await RefreshTokensAsync(cancellationToken) == false)
            {
                return response;
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                _lock.Dispose();
            }

            base.Dispose(disposing);
        }

        private async Task<bool> RefreshTokensAsync(CancellationToken cancellationToken)
        {
            var refreshToken = RefreshToken;
            if (string.IsNullOrEmpty(refreshToken))
            {
                return false;
            }

            if (await _lock.WaitAsync(_lockTimeout, cancellationToken).ConfigureAwait(false))
            {
                try
                {
                    // default IdentityModel way to refresh token
                    if (_tokenClient != null)
                    {
                        var response = await _tokenClient.RequestRefreshTokenAsync(refreshToken, cancellationToken: cancellationToken).ConfigureAwait(false);
                        if (!response.IsError)
                        {
                            _accessToken = response.AccessToken;
                            _refreshToken = response.RefreshToken;
                            SaveToken();
                            return true;
                        }
                    }
                    else
                    {
                        var client = new OidcClient(GetIdentityOptions());
                        var refreshResult = await client.RefreshTokenAsync(refreshToken).ConfigureAwait(false);
                        if (!refreshResult.IsError)
                        {
                            _accessToken = refreshResult.AccessToken;
                            _refreshToken = refreshResult.RefreshToken;
                            SaveToken();
                            return true;
                        }
                    }

                }
                finally
                {
                    _lock.Release();
                }
            }

            return false;
        }

        private OidcClientOptions GetIdentityOptions()
        {
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

        private void SaveToken() {
            var info = _storageService.LoadSignInInfo();
            info.AccessToken = _accessToken;
            info.RefreshToken = _refreshToken;
            _storageService.SaveSignInInfo(info);
        }
    }
}
