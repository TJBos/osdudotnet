using Newtonsoft.Json;
using OsduLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OsduLib.Client;

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
    private string? RefreshUrl;
    private string? ClientID;
    private string? ClientSecret;

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
    }
    public async void UpdateToken()
    {
        if (RefreshToken == null || RefreshUrl == null)
        {
            throw new Exception("Expired or invalid access token. Both refresh url and refresh token must be set to auto refresh");
        }
        // implement refresh token http request
        var data = new Dictionary<string, string> {
            {"grant_type", "refresh_token"},
            {"client_id", ClientID},
            {"client_secret", ClientSecret},
            {"refresh_token", RefreshToken},
            {"scope", "openid email"}
        };


        var request = new HttpRequestMessage(HttpMethod.Post, RefreshUrl);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        request.Content = new StringContent(JsonConvert.SerializeObject(data));
        var httpclient = new HttpClient();
        var response = await httpclient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(content);
        AccessToken = tokenResponse.AccessToken;
        TokenExpiration = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn);
    }
}
