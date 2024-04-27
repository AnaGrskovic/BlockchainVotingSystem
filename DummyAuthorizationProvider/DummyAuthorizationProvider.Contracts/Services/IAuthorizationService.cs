namespace DummyAuthorizationProvider.Contracts.Services;

public interface IAuthorizationService
{
    Task<string?> GetTokenAsync(string oib);
    Task<bool> IsTokenValidAsync(string token);
}
