using Newtonsoft.Json;
using OsduLib.Models.Record;

namespace OsduLib.Models.ManifestIngestion;

public class WorkProduct: RecordBase

{
   [JsonProperty("data")] public WorkProductData WorkProductData;

    public WorkProduct(Guid id) : base()
    {
        WorkProductData = new WorkProductData();
        Kind = "osdu:wks:work-product--WorkProduct:1.0.0";
        Id = $"osdu:work-product--WorkProduct:{id}";
    }

}