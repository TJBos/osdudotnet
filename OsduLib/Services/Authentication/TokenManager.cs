using System.Collections.ObjectModel;
using OsduLib.Models;
using Microsoft.Extensions.Logging;

namespace OsduLib.Services.Authentication;

public class TokenManager : ITokenProvider
{
    private readonly ILogger<TokenManager> _logger;
    private readonly Collection<ITokenProvider> _tokenProviders;

    public TokenManager(ILogger<TokenManager> logger)
    {
        _logger = logger;
        _tokenProviders = new Collection<ITokenProvider>();
    }

    public async Task<TokenResponse> GetToken()
    {
        foreach (var provider in _tokenProviders)
        {
            var isValid = await provider.IsValid();
            if (isValid)
                try
                {
                    var token = await provider.GetToken();
                    _logger.LogDebug($"Picked provider: {provider.GetType().FullName}");
                    return token;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Could not retrieve {provider.GetType().FullName} token");
                    _logger.LogError(ex.Message);
                }
        }

        return new TokenResponse();
    }

    public async Task<bool> IsValid()
    {
        foreach (var provider in _tokenProviders)
        {
            var isValid = await provider.IsValid();
            if (isValid)
                return true;
        }

        return false;
    }

    public TokenManager AddProvider(ITokenProvider provider)
    {
        _tokenProviders.Add(provider);
        return this;
    }
}