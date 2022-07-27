using Newtonsoft.Json;

namespace OsduLib.Models.Record;

public class BoundingBox
{
    [JsonProperty("bottomRight")] public Coordinates BottomRight;

    [JsonProperty("longitude")] public Coordinates TopLeft;
}