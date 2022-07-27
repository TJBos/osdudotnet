using Newtonsoft.Json;

namespace OsduLib.Models;

public class ErrorResponse
{
    [JsonProperty("code")] public int Code;

    [JsonProperty("message")] public string Message;

    [JsonProperty("reason")] public string Reason;
}