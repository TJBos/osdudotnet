using OsduLib.Models;
using Microsoft.Extensions.Configuration;

namespace OsduLib.Services.Authentication;

public class DetectedUserToken : ITokenProvider
{
    private readonly IConfiguration _configuration;
    private readonly UserCredentialService _credentialService;

    public DetectedUserToken(
        UserCredentialService credentialService,
        IConfiguration configuration
    )
    {
        _credentialService = credentialService;
        _configuration = configuration;
    }

    public Task<bool> IsValid()
    {
        var userPassword = GetUserPassword();
        var username = GetUsername();
        var isInvalid = string.IsNullOrEmpty(userPassword) || string.IsNullOrEmpty(username);
        return Task.FromResult(!isInvalid);
    }


    public async Task<TokenResponse> GetToken()
    {
        var userCredentials = await _credentialService.GetUserCredentials(GetUsername(), GetUserPassword());
        return await userCredentials.GetToken();
    }

    private string GetUsername()
    {
        return _configuration.GetValue<string>("OSDU_USERNAME");
    }

    private string GetUserPassword()
    {
        return _configuration.GetValue<string>("OSDU_PASSWORD");
    }
}