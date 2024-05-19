namespace DummyAuthorizationProvider.Contracts.Services;

public interface IAuthorizationService
{
    Task<string> GetTokenAsync(string? oib);
    Task CheckTokenNothingAsync(string? token);
    Task CheckTokenRequestedAsync(string? token);
    Task SetVoteRequestedAsync(string? token);
    Task SetVoteCreatedAsync(string? token);
}
