using Newtonsoft.Json;

namespace OsduLib.Models.Record;

public class SpatialFilter
{
    [JsonProperty("byBoundingBox")] public BoundingBox? ByBoundingBox;

    [JsonProperty("field")] public string Field;
}