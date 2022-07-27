using System.Net.Http.Headers;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using OsduLib.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OsduLib.Services.Authentication;

public class ServicePrincipalToken : BaseHttpClient, ITokenProvider, IDisposable
{
    private readonly ILogger<ServicePrincipalToken> _logger;
    private readonly IAmazonSecretsManager _secretsManager;
    private readonly IAmazonSimpleSystemsManagement _ssm;

    public string? AwsOauthCustomScope;
    public string? BaseTokenUrl;
    public string? ClientId;

    public ServicePrincipalToken(ILogger<ServicePrincipalToken> logger, IHttpClientFactory httpClientFactory,
        OsduAWSEnvironment osduEnv, IAmazonSecretsManager secretsManager,
        IAmazonSimpleSystemsManagement ssm) :
        base(httpClientFactory.CreateClient())
    {
        _logger = logger;
        _secretsManager = secretsManager;
        _ssm = ssm;
        ResourcePrefix = osduEnv.ResourcePrefix;
    }

    public string ApiUrl { get; private set; }
    public string? ClientSecret { get; set; }
    public string ResourcePrefix { get; }
    private string TOKEN_URL_SSM_PATH => $"/osdu/{ResourcePrefix}/oauth-token-uri";
    private string AWS_OAUTH_CUSTOM_SCOPE_SSM_PATH => $"/osdu/{ResourcePrefix}/oauth-custom-scope";
    private string CLIENT_ID_SSM_PATH => $"/osdu/{ResourcePrefix}/client-credentials-client-id";
    private string CLIENT_SECRET_NAME => $"/osdu/{ResourcePrefix}/client_credentials_secret";
    private string CLIENT_SECRET_DICT_KEY => "client_credentials_client_secret";
    private string API_URL_PATH => $"/osdu/{ResourcePrefix}/api/url";

    public void Dispose()
    {
        _secretsManager.Dispose();
        _ssm.Dispose();
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

    public async Task<bool> IsValid()
    {
        try
        {
            await LoadConfigurationData();
            return !string.IsNullOrEmpty(ClientSecret);
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ServicePrincipalToken is invalid - {ex.Message}");
            return false;
        }
    }

    public async Task LoadConfigurationData()
    {
        if (ClientSecret == null)
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
        return (T) jsonValue[dictKey];
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
}