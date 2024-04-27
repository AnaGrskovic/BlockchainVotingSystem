using VotingApp.Contracts.Services;

namespace VotingApp.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IHttpClientService _httpClientService;
    public AuthorizationService(IHttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }

    public async Task<bool> CheckTokenAsync(string token)
    {
        try
        {
            await _httpClientService.GetAsync("https://localhost:44378/api/voters/check-token", token);
        }
        catch (HttpRequestException)
        {
            return false;
        }
        return true;
    }
}
