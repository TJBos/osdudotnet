using OsduLib.Models.Record;
using Newtonsoft.Json;

namespace OsduLib.Models.ManifestIngestion;

public class ManifestData
{
    [JsonProperty("Datasets")] public List<Dataset> Datasets;

    [JsonProperty("WorkProduct")] public WorkProduct WorkProduct;

    [JsonProperty("WorkProductComponents")] public List<WorkProductComponent> WorkProductComponents;

    public ManifestData()
    {
        Datasets = new List<Dataset>() { new Dataset(new Guid()) };
        WorkProduct = new WorkProduct(new Guid());
        WorkProductComponents = new List<WorkProductComponent>() { new WorkProductComponent(new Guid()) };
    }
}