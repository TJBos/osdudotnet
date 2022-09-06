using Newtonsoft.Json;
using OsduLib.Exceptions;
using OsduLib.Models;
using OsduLib.Services;
using System.Net.Http.Headers;
using HttpHeaders = OsduLib.Models.HttpHeaders;

namespace OsduLib.Client
{
    public abstract class BaseOsduClient : BaseHttpClient
    {
        private string _accessToken;

        public DateTime TokenExpiration;
        public string AccessToken
        {
            get
            {
                if (DateTime.Compare(TokenExpiration, DateTime.Now) < 0)
                {
                    Task.Run(async () => await UpdateToken()).Wait();
                }
                return _accessToken;
            }
            set { _accessToken = value; }
        }
        internal string DataPartitionId { get; private set; }
        protected string ApiBaseUrl { get; private set; }

        public SearchService Search => new SearchService(this);
        public DatasetService Dataset => new DatasetService(this);
        public SchemaService Schema => new SchemaService(this);
        public StorageService Storage => new StorageService(this);

        public IngestionService ManifestIngestion => new IngestionService(this);

        public BaseOsduClient(string dataPartitionId, string? apiBaseUrl = null)
        {
            DataPartitionId = dataPartitionId;
            ApiBaseUrl = apiBaseUrl ?? Environment.GetEnvironmentVariable("OSDU_API_URL");
        }
        protected virtual async Task UpdateToken()
        {
            // implemented by parent classes.
        }

        internal Task<TResponse> PutJson<TResponse>(string path, HttpHeaders headers, string body)
        {
            return SendJson<TResponse>(HttpMethod.Put, path, headers, body);
        }
        internal Task<TResponse> PostJson<TResponse>(string path, HttpHeaders headers, string body)
        {
            return SendJson<TResponse>(HttpMethod.Post, path, headers, body);
        }
        internal Task<TResponse> GetJson<TResponse>(string path, HttpHeaders headers, string body)
        {
            return SendJson<TResponse>(HttpMethod.Get, path, headers, body);
        }

        internal async Task<HttpResponseMessage> Delete(string path, HttpHeaders headers, string body)
        {
            var requestUrl = ApiBaseUrl + "/" + path;
            var request = MakeRequest(requestUrl, headers);
            request.Method = HttpMethod.Delete;
            return await SendAsync(request);
        }

        internal async Task<TResponse> SendJson<TResponse>(HttpMethod method, string path, HttpHeaders headers, string body)
        {
            var requestUrl = ApiBaseUrl + "/" + path;
            var request = MakeRequest(requestUrl, headers);
            request.Method = method;
            var requestContent = new StringContent(body);
            request.Content = requestContent;
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await SendAsync(request);
            try
            {
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(content);
            }
            catch (HttpRequestException ex)
            {
                var content = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(content);
                if (string.IsNullOrEmpty(errorResponse?.Message)) throw;

                throw new OsduHttpException(errorResponse, ex);
            }
        }

        internal void SetToken(TokenResponse token)
        {
            var authenticationHeaderValue = new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
            _baseClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await base.SendAsync(request);
        }
    }
}
