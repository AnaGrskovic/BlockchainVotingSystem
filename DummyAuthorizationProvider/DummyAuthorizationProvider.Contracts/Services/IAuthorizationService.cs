namespace DummyAuthorizationProvider.Contracts.Services;

public interface IAuthorizationService
{
    Task<string> GetTokenAsync(string? oib);
    Task CheckTokenNotVotedAsync(string? token);
    Task CheckTokenVotedAsync(string? token);
    Task SetVotedAsync(string? token);
}
