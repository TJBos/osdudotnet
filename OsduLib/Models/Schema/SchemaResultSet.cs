using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace OsduLib.Models.Schema;

public class SchemaResultSet
{
    [JsonProperty("schemaInfos")] public Collection<SchemaInfo> SchemaInfos;

    [JsonProperty("offset")] public int Offset;

    [JsonProperty("count")] public int Count;

    [JsonProperty("totalCount")] public int TotalCount;
}