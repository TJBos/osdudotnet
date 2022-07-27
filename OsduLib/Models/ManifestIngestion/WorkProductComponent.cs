using Newtonsoft.Json;
using OsduLib.Models.Record;

namespace OsduLib.Models.ManifestIngestion;

public class WorkProductComponent : RecordBase
{
    [JsonProperty("data")]
    public WorkProductComponentData WorkProductComponentData;
    [JsonProperty("tags")]
    public Dictionary<string, string>? Tags;

    public WorkProductComponent(Guid id) : base()
    {
        Kind = "osdu:wks:work-product-component--WellLog:1.1.0";
        WorkProductComponentData = new WorkProductComponentData();
        Id = $"osdu:work-product-component--WellLog:{id}";
    }
}