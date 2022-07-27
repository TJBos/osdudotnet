using OsduLib.Models.Search;
using Newtonsoft.Json;
using OsduLib.Client;

namespace OsduLib.Services;

public class SearchService : BaseOsduService
{
    private readonly int _defaultPagingLimit = 100;

    private readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore
    };

    private readonly string[] _quickReturnFields = {"id", "tags", "version", "type", "kind", "version"};

    public SearchService(BaseOsduClient client) : base(client, "search", "2")
    {
    }

    public async Task<SearchResult> FindById(string kind, string id)
    {
        var request = new SearchRequest
        {
            Kind = kind,
            Query = $"id: \"{id}\"",
            Limit = 1
        };
        var resultSet = await Query(request);
        return resultSet.Results.First();
    }

    public IAsyncEnumerable<SearchResultSet> QuickQuery(SearchRequest request)
    {
        request.ReturnedFields = request.ReturnedFields != null
            ? request.ReturnedFields.Concat(_quickReturnFields).Distinct().ToArray()
            : _quickReturnFields;

        return QueryWithPaging(request);
    }

    public Task<SearchResultSet> Query(SearchRequest request)
    {
        return _client.PostJson<SearchResultSet>(
            $"{_servicePath}/query",
            _defaultHeaders,
            SerializeRequest(request)
        );
    }

    public async IAsyncEnumerable<SearchResultSet> QueryWithPaging(SearchRequest request)
    {
        if (request.Limit is null or 0) request.Limit = _defaultPagingLimit;
        
        SearchResultSet results = null;
        while (results == null || !string.IsNullOrEmpty(results.Cursor))
        {
            var serializedRequest = SerializeRequest(request);
            results = await _client.PostJson<SearchResultSet>(
                $"{_servicePath}/query_with_cursor",
                _defaultHeaders,
                serializedRequest
            );
            yield return results;
            request.Cursor = results.Cursor;
        }
    }

    private string SerializeRequest(SearchRequest request)
    {
        return JsonConvert.SerializeObject(request, Formatting.None, _jsonSerializerSettings);
    }
}