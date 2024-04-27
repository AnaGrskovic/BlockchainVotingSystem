namespace DummyAuthorizationProvider.Contracts.Services;

public interface IVoterService
{
    Task<string?> GetTokenAsync(string oib);
    Task<bool> IsTokenValidAsync(string token);
}
