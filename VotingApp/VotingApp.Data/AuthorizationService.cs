using Microsoft.Extensions.Options;
using VotingApp.Contracts.Services;
using VotingApp.Contracts.Settings;

namespace VotingApp.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly AuthorizationSettings _settings;
    private readonly IHttpClientService _httpClientService;

    public AuthorizationService(
        IOptions<AuthorizationSettings> settings,
        IHttpClientService httpClientService)
    {
        _settings = settings.Value;
        _httpClientService = httpClientService;
    }

    public async Task<bool> CheckTokenAsync(string token)
    {
        string authorizationUrl = $"{_settings.BaseUrl}{_settings.CheckTokenEndpoint}";
        try
        {
            await _httpClientService.GetAsync(authorizationUrl, token);
        }
        catch (HttpRequestException)
        {
            return false;
        }
        return true;
    }
}
