namespace VotingApp.Contracts.Services;

public interface IVotingService
{
    Task VoteAsync(string? token, string vote);
}
