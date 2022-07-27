//using System.Net.Http.Headers;
//using OsduLib.Exceptions;
//using OsduLib.Models;
//using OsduLib.Services.Authentication;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using HttpHeaders = OsduLib.Models.HttpHeaders;

//namespace OsduLib.Services;

//public class OsduClient : BaseHttpClient
//{
//    private readonly ILogger<OsduClient> _logger;
//    private readonly ITokenProvider? _tokenProvider;
//    private readonly FileReferenceManager _fileReferenceManager;
//    public SearchService Search => new SearchService(this);
//    public DatasetService Dataset => new DatasetService(this, _fileReferenceManager);
//    public SchemaService Schema => new SchemaService(this);
//    public IngestionService Ingestion => new IngestionService(this, _fileReferenceManager, _logger);

//    public OsduClient(ILogger<OsduClient> logger, IHttpClientFactory httpClientFactory, OsduAWSEnvironment osduEnv, FileReferenceManager fileReferenceManager, ITokenProvider? tokenProvider = null) : base(
//        httpClientFactory.CreateClient())
//    {
//        _logger = logger;
//        _tokenProvider = tokenProvider;
//        _fileReferenceManager = fileReferenceManager;
//        SetAwsEnvironment(osduEnv);
//    }

//    public void SetAwsEnvironment(OsduAWSEnvironment osduEnv)
//    {
//        if (!string.IsNullOrEmpty(osduEnv.BaseApiUrl))
//        {
//            var uriString = $"{osduEnv.BaseApiUrl}/api";
//            _baseClient.BaseAddress = new Uri(uriString);
//        }

//        if (!string.IsNullOrEmpty(osduEnv.DataPartitionId))
//            _baseClient.DefaultRequestHeaders.Add("data-partition-id", osduEnv.DataPartitionId);
//    }

//    public void SetToken(TokenResponse token)
//    {
//        var authenticationHeaderValue = new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
//        _baseClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
//    }

//public Task<TResponse> PutJson<TResponse>(string path, HttpHeaders headers, string body)
//{
//    return SendJson<TResponse>(HttpMethod.Put, path, headers, body);
//}
//public Task<TResponse> PostJson<TResponse>(string path, HttpHeaders headers, string body)
//{
//    return SendJson<TResponse>(HttpMethod.Post, path, headers, body);
//}
//public Task<TResponse> GetJson<TResponse>(string path, HttpHeaders headers, string body)
//{
//    return SendJson<TResponse>(HttpMethod.Get, path, headers, body);
//}

//    public async Task<TResponse> SendJson<TResponse>(HttpMethod method, string path, HttpHeaders headers, string body)
//    {
//        var request = MakeRequest(path, headers);
//        request.Method = method;
//        _logger.LogDebug($"[osdu-api/SendJson] {request.Method} {request.RequestUri} -- {body}");
//        var requestContent = new StringContent(body);
//        request.Content = requestContent;
//        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
//        var response = await SendAsync(request);
//        try
//        {
//            response.EnsureSuccessStatusCode();
//            var content = await response.Content.ReadAsStringAsync();
//            return JsonConvert.DeserializeObject<TResponse>(content);
//        }
//        catch (HttpRequestException ex)
//        {
//            var content = await response.Content.ReadAsStringAsync();
//            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(content);
//            if (string.IsNullOrEmpty(errorResponse?.Message)) throw;

//            throw new OsduHttpException(errorResponse, ex);
//        }
//    }

//    private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
//    {
//        // When token has not been set, but there is a token provider, use an available token
//        if (_baseClient.DefaultRequestHeaders.Authorization == null && _tokenProvider != null)
//        {
//            var tokenResponse = await _tokenProvider.GetToken();
//            if (string.IsNullOrEmpty(tokenResponse.TokenType) || string.IsNullOrEmpty(tokenResponse.AccessToken))
//                throw new InvalidTokenException(tokenResponse.GetType(),
//                    "Both a token type and access token must be provided.");
//            SetToken(tokenResponse);
//        }

//        return await base.SendAsync(request);
//    }
//}