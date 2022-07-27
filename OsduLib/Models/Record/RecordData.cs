using Newtonsoft.Json;

namespace OsduLib.Models.Record;

public class RecordData : RecordBase
{
   
    [JsonProperty("data")] public GenericData Data;

}