namespace VotingApp.Contracts.Services;

public interface ITimeService
{
    bool IsBeforeVotingTime();
    bool IsDuringVotingTime();
    bool IsAfterVotingTime();
}
