using Microsoft.Extensions.Options;
using VotingApp.Contracts.Exceptions;
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
        string url = $"{_settings.BaseUrl}{_settings.CheckTokenEndpoint}";
        try
        {
            await _httpClientService.GetAsync(url, token);
        }
        catch (HttpRequestException)
        {
            return false;
        }
        return true;
    }

    public async Task SetVotedAsync(string token)
    {
        string url = $"{_settings.BaseUrl}{_settings.SetVotedEndpoint}";
        try
        {
            await _httpClientService.PutAsync(url, token);
        }
        catch (HttpRequestException)
        {
            throw new AuthProviderException("An error occurred while setting the voted status.");
        }
    }
}
