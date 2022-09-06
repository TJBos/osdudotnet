using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Newtonsoft.Json;
using OsduLib.Models;
using System.Net.Http.Headers;

namespace OsduLib.Services.Authentication
{
    internal class ServicePrincipal : BaseHttpClient
    {
        private readonly IAmazonSecretsManager _secretsManager;
        private readonly IAmazonSimpleSystemsManagement _ssm;

        public string? AwsOauthCustomScope;
        public string? BaseTokenUrl;
        public string? ClientId;
        public string ResourcePrefix;
        public string Region;
        public string Profile;
        public string ApiUrl { get; private set; }
        public string? ClientSecret { get; set; }
        private string TOKEN_URL_SSM_PATH => $"/osdu/{ResourcePrefix}/oauth-token-uri";
        private string AWS_OAUTH_CUSTOM_SCOPE_SSM_PATH => $"/osdu/{ResourcePrefix}/oauth-custom-scope";
        private string CLIENT_ID_SSM_PATH => $"/osdu/{ResourcePrefix}/client-credentials-client-id";
        private string CLIENT_SECRET_NAME => $"/osdu/{ResourcePrefix}/client_credentials_secret";
        private string CLIENT_SECRET_DICT_KEY => "client_credentials_client_secret";
        private string API_URL_PATH => $"/osdu/{ResourcePrefix}/api/url";

        public ServicePrincipal(string resourcePrefix, string region, string profile)
        {
            ResourcePrefix = resourcePrefix;
            Region = region;
            ConfigureAmazon(region, profile);
            _secretsManager = new AmazonSecretsManagerClient();
            _ssm = new AmazonSimpleSystemsManagementClient();
        }

        public ServicePrincipal(string clientId, string clientSecret, string authTokenUrl, string awsOAuthCustomScope)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            BaseTokenUrl = authTokenUrl;
            AwsOauthCustomScope = awsOAuthCustomScope;
        }
        public async Task<TokenResponse> GetToken()
        {
            await LoadConfigurationData();
            var tokenUrl = $"{BaseTokenUrl}?grant_type=client_credentials&client_id={ClientId}&scope={AwsOauthCustomScope}";
            var contentTypeHeader = "application/x-www-form-urlencoded";
            var authorizationHeader = "Basic " + Utilities.Base64Encode($"{ClientId}:{ClientSecret}");
            var req = new HttpRequestMessage();
            req.Method = HttpMethod.Post;
            req.RequestUri = new Uri(tokenUrl);
            req.Headers.Add("Authorization", authorizationHeader);
            req.Content = new StringContent("");
            req.Content.Headers.ContentType = new MediaTypeHeaderValue(contentTypeHeader);
            var response = await SendAsync(req);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokenResponse>(content);
        }

        private async Task LoadConfigurationData()
        {
            if (ClientSecret == null)
            {
                await Task.WhenAll(
                    Task.Run(async () => { ApiUrl = await GetParameterStoreValueAsync(API_URL_PATH); }),
                    Task.Run(async () => { BaseTokenUrl = await GetParameterStoreValueAsync(TOKEN_URL_SSM_PATH); }),
                    Task.Run(async () => { ClientId = await GetParameterStoreValueAsync(CLIENT_ID_SSM_PATH); }),
                    Task.Run(async () =>
                    {
                        ClientSecret = await GetSecretValueAsync<string>(CLIENT_SECRET_NAME, CLIENT_SECRET_DICT_KEY);
                    }),
                    Task.Run(async () =>
                    {
                        AwsOauthCustomScope = await GetParameterStoreValueAsync(AWS_OAUTH_CUSTOM_SCOPE_SSM_PATH);
                    })
                );
            }
        }
        private async Task<string> GetSecretValueAsync(string secretName)
        {
            var value = await _secretsManager.GetSecretValueAsync(new GetSecretValueRequest
            {
                SecretId = secretName
            });
            return value.SecretString;
        }

        private async Task<T> GetSecretValueAsync<T>(string secretName, string dictKey)
        {
            var value = await GetSecretValueAsync(secretName);
            var jsonValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(value);
            return (T)jsonValue[dictKey];
        }

        private async Task<string> GetParameterStoreValueAsync(string parameter)
        {
            var response = await _ssm.GetParameterAsync(new GetParameterRequest
            {
                Name = parameter,
                WithDecryption = true
            });
            return response.Parameter.Value;
        }
        private static void ConfigureAmazon(string region, string? profile, bool updateEnvironment = false)
        {
            if (!string.IsNullOrEmpty(region))
            {
                AWSConfigs.AWSRegion = region;
                if (updateEnvironment)
                    Environment.SetEnvironmentVariable("AWS_DEFAULT_REGION", region);
            }

            if (!string.IsNullOrEmpty(profile))
            {
                AWSConfigs.AWSProfileName = profile;
                if (updateEnvironment)
                    Environment.SetEnvironmentVariable("AWS_PROFILE", profile);
            }
        }
    }
   }
