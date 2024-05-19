namespace DummyAuthorizationProvider.Contracts.Services;

public interface IAuthorizationService
{
    Task<string> GetTokenAsync(string? oib);
    Task CheckTokenAsync(string? token);
    Task SetVoteRequestedAsync(string? token);
    Task SetVoteCreatedAsync(string? token);
}
