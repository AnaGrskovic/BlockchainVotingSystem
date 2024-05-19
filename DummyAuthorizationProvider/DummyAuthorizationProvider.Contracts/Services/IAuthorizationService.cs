namespace DummyAuthorizationProvider.Contracts.Services;

public interface IAuthorizationService
{
    Task<string> GetTokenAsync(string? oib);
    Task CheckTokenAsync(string? token);
    Task SetVotedAsync(string? token);
}
