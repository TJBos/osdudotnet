using OsduLib.Models;

namespace OsduLib.Services;

public abstract class BaseHttpClient
{
    protected readonly HttpClient _baseClient;

    protected BaseHttpClient()
    {
        _baseClient = new HttpClient();
    }

    protected BaseHttpClient(HttpClient baseClient)
    {
        _baseClient = baseClient;
    }

    protected Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        return _baseClient.SendAsync(request);
    }

    protected HttpRequestMessage MakeRequest(HttpHeaders? headers = null)
    {
        var request = new HttpRequestMessage();
        if (headers != null)
            foreach (var header in headers)
                request.Headers.Add(header.Key, header.Value);

        return request;
    }

    protected HttpRequestMessage MakeRequest(string path, HttpHeaders headers)
    {
        var request = MakeRequest(headers);
        request.RequestUri = new Uri(path);
        return request;
    }
}