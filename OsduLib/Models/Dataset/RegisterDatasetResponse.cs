using OsduLib.Models.Record;
using Newtonsoft.Json;

namespace OsduLib.Models.Dataset;

public class RegisterDatasetFileResponse
{
    [JsonProperty("datasetRegistries")] public List<DatasetRegistry> DatasetRegistries;
}