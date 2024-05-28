namespace VotingApp.Contracts.Services;

public interface ITimeService
{
    bool IsBlockChainCalculationTime();
    bool CanResultsBeShown();
}
