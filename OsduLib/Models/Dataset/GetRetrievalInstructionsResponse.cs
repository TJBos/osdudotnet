using Newtonsoft.Json;

namespace OsduLib.Models.Dataset
{
    public class GetRetrievalInstructionsResponse
    {
        [JsonProperty("delivery")]
        public RetrievalInstruction[] Delivery;
    }
}