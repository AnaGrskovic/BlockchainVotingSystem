namespace VotingApp.Contracts.Services;

public interface IAuthorizationService
{
    Task<bool> CheckTokenAsync(string token);
    Task SetVoteRequestedAsync(string token);
}
