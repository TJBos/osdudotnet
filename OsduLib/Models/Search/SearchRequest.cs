using OsduLib.Models.Record;
using Newtonsoft.Json;

namespace OsduLib.Models.Search;

public class SearchRequest
{
    [JsonProperty("aggregateBy")] public string? AggregateBy { get; set; }

    [JsonProperty("kind")] public string Kind { get; set; } = "*:*:*:*";

    [JsonProperty("limit")] public int? Limit { get; set; }

    [JsonProperty("offset")] public int? Offset { get; set; }

    [JsonProperty("query")] public string Query { get; set; } = "*";

    [JsonProperty("returnedFields")] public string[]? ReturnedFields { get; set; }

    [JsonProperty("sort")] public SortOrder? Sort { get; set; }

    [JsonProperty("spatialFilter")] public SpatialFilter? SpatialFilter{ get; set; }
    [JsonProperty("cursor")] public string? Cursor { get; set; }
}