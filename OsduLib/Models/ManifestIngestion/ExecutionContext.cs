using OsduLib.Models.Record;
using Newtonsoft.Json;

namespace OsduLib.Models.ManifestIngestion;

public class ExecutionContext : RecordBase
{

    [JsonProperty("manifest")] public Manifest Manifest;

    [JsonProperty("Payload")] public GenericData Payload;

    public ExecutionContext() : base()
    {
        Payload = new GenericData() { { "AppKey", "test-app" }, { "data-partition-id", "osdu" } };
        Manifest = new Manifest();
    }
}