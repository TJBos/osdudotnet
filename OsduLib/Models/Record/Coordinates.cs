using Newtonsoft.Json;

namespace OsduLib.Models.Record;

public class Coordinates
{
    [JsonProperty("latitude")] public double Latitude;

    [JsonProperty("longitude")] public double Longitude;
}