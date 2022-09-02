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

    public Task<SearchResultSet> Query(SearchRequest request)
    {
        // Executes a query against the OSDU search service.
        // Takes a type of SearchRequest as a parameter,  with at least a kind property, and optionally a query and other fields. 
        // Must adhere to the Lucene syntax suported by OSDU. For more details, see:
        // https://community.opengroup.org/osdu/documentation/-/wikis/Releases/R2.0/OSDU-Query-Syntax
        // Returns 3 items; aggregations, list of search results and totalCount

        return _client.PostJson<SearchResultSet>(
            $"{_servicePath}/query",
            _defaultHeaders,
            SerializeRequest(request)
        );
    }
    public IAsyncEnumerable<SearchResultSet> QuickQuery(SearchRequest request)
    {
    // Query with a fixed number of returned fields (id, tags, version, type, kind, version)

        request.ReturnedFields = request.ReturnedFields != null
            ? request.ReturnedFields.Concat(_quickReturnFields).Distinct().ToArray()
            : _quickReturnFields;

        return QueryWithPaging(request);
    }


    public async IAsyncEnumerable<SearchResultSet> QueryWithPaging(SearchRequest request)
    {

     // Executes a query with cursor against the OSDU search service. Returns a generator, which can than be
     // iterated over to retrieve each page in the result set without having to deal with any cursor
        
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