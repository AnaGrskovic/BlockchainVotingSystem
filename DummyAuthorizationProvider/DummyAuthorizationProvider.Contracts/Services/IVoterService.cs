namespace DummyAuthorizationProvider.Contracts.Services;

public interface IVoterService
{
    Task<string> GetToken(string oib);
    Task<bool> IsTokenValid(string token);
}
