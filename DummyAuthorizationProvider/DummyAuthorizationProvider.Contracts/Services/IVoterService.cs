namespace DummyAuthorizationProvider.Contracts.Services;

public interface IVoterService
{
    string GetToken(string oib);
    Task<bool> IsTokenValidAsync(string token);
}
