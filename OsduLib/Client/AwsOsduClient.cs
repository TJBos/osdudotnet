using Newtonsoft.Json;
using OsduLib.Models;
using OsduLib.Services;
using OsduLib.Services.Authentication;
using System.Net.Http.Headers;
using static OsduLib.Services.Authentication.UserCredentialService;

namespace OsduLib.Client
{
    public class AwsOsduClient : BaseOsduClient
    {
        public UserCredentialService userCredentialService;
        public string RefreshToken;
        public string? RefreshUrl;
        public string? ClientID;
        public string? ClientSecret;
        public AwsOsduClient(OsduAWSEnvironment osduAWSEnvironment, string username, string password) : base(osduAWSEnvironment.DataPartitionId, osduAWSEnvironment.BaseApiUrl)
        {
            if (osduAWSEnvironment.UserPoolClientId == null || osduAWSEnvironment.UserPoolId == null || osduAWSEnvironment.UserPoolClientSecret == null)
            {
                throw new ArgumentException("Must provide the Cognito UserpoolID, client Id and client secret");
            } 
            userCredentialService = new UserCredentialService(osduAWSEnvironment);
            RefreshUrl = osduAWSEnvironment.TokenUrl ?? null;
            ClientID = osduAWSEnvironment.UserPoolClientId;
            ClientSecret = osduAWSEnvironment.UserPoolClientSecret;

            Task.Run(async () => await SetToken(username, password)).Wait();
        }
        private async Task SetToken(string username, string password)
        {
            UserCredentials userCredentials = await userCredentialService.GetUserCredentials(username, password);
            TokenResponse response = await userCredentials.GetToken();
            AccessToken = response.AccessToken;
            RefreshToken = response.RefreshToken;
            TokenExpiration = DateTime.Now.AddSeconds(response.ExpiresIn);
        }

        protected override async Task UpdateToken()
        {
            if (RefreshUrl == null)
            {
                throw new Exception("Expired or invalid access token. A token url must be set to auto refresh");
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
