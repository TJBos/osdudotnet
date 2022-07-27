using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace OsduLib.Models.Search;

public class SearchResultSet
{
    [JsonProperty("aggregations")] public object? Aggregations;

    [JsonProperty("Cursor")] public string? Cursor;

    [JsonProperty("results")] public Collection<SearchResult> Results;

    [JsonProperty("totalCount")] public int TotalCount;
}