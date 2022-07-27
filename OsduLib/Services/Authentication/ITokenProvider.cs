using OsduLib.Models;

namespace OsduLib.Services.Authentication;

public interface ITokenProvider
{
    public Task<TokenResponse> GetToken();
    public Task<bool> IsValid();
}