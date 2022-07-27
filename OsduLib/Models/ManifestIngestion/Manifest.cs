using OsduLib.Models.Record;
using Newtonsoft.Json;

namespace OsduLib.Models.ManifestIngestion;

public class Manifest
{
    [JsonProperty("kind")] public string Kind => "osdu:wks:Manifest:1.0.0";

    [JsonProperty("Data")] public ManifestData Data;

    [JsonProperty("MasterData")] public List<RecordData> MasterData;

    [JsonProperty("ReferenceData")] public List<RecordData> ReferenceData;

    public Manifest()
    {
        MasterData = new List<RecordData>();
        ReferenceData = new List<RecordData>();
        Data = new ManifestData();
    }
}

