using Newtonsoft.Json;
using OsduLib.Models;
using OsduLib.Services;
using System.Net.Http.Headers;

namespace OsduLib.Client
{

    public class SimpleOsduClient : BaseOsduClient
    {
        /* 
         BYOT: Bring your own token.

    This client assumes you are obtaining a token yourself (e.g. via your application's
    login form or otheer mechanism. With this SimpleOsduClient, you simply provide that token.
    With this simplicity, you are also then respnsible for refreshing the token as needed either by manually
    re-instantiating the client with the new token or by providing the authentication client id, secret, refresh token, and refresh url
    and allowing the client to attempt the refresh automatically. 

         */

        public string RefreshToken;
        protected string? RefreshUrl;
        protected string? ClientID;
        protected string? ClientSecret;

        public SimpleOsduClient(string dataPartitionId, string apiBaseUrl, string accessToken) : base(dataPartitionId, apiBaseUrl)
        {
            AccessToken = accessToken;
        }

        public SimpleOsduClient(string dataPartitionId, string apiBaseUrl, string refreshToken, string refreshUrl, string clientId, string clientSecret) : base(dataPartitionId, apiBaseUrl)
        {
            RefreshToken = refreshToken;
            RefreshUrl = refreshUrl;
            ClientID = clientId;
            ClientSecret = clientSecret;
            Task.Run(async () => await UpdateToken()).Wait();
        }

        protected override async Task UpdateToken()
        {
            if (RefreshToken == null || RefreshUrl == null)
            {
                throw new Exception("Expired or invalid access token. Both refresh url and refresh token must be set to auto refresh");
            }

            var tokenUrl = $"{RefreshUrl}?grant_type=refresh_token&client_id={ClientID}&refresh_token={RefreshToken}";
            var authorizationHeader = "Basic " + Utilities.Base64Encode($"{ClientID}:{ClientSecret}");
            var req = new HttpRequestMessage(HttpMethod.Post, new Uri(tokenUrl));
            req.Headers.Add("Authorization", authorizationHeader);
            req.Content = new StringContent("");
            req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = await SendAsync(req);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(content);
            AccessToken = tokenResponse.AccessToken;
            TokenExpiration = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn);
        }
    }
}