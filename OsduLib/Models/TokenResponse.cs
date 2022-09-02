using Newtonsoft.Json;

namespace OsduLib.Models;

public class TokenResponse
{
    [JsonProperty("access_token")] public string AccessToken = string.Empty;

    [JsonProperty("expires_in")] public int ExpiresIn;

    [JsonProperty("token_type")] public string TokenType = string.Empty;

    [JsonProperty("refresh_token")] public string RefreshToken = string.Empty;

    public string? Username { get; set; }
}