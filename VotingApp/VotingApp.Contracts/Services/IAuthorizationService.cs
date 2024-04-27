namespace VotingApp.Contracts.Services;

public interface IAuthorizationService
{
    bool CheckTokenAsync(string token);
}
