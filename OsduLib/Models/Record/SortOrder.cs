using Newtonsoft.Json;

namespace OsduLib.Models.Record;

public class SortOrder
{
    public static readonly string DescOrder = "DESC";
    public static readonly string AscOrder = "ASC";

    [JsonProperty("field")] public string[] Field;

    [JsonProperty("order")] public string[] Order;
}