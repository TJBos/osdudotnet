using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace OsduLib.Models.Dataset;

public class GetRetrievalInstructionsResponse
{
    [JsonProperty("signedUrl")]
    public string SignedUrl;

    [JsonProperty("delivery")]
    public RetrievalInstruction[] Delivery;
}