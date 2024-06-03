using Microsoft.Extensions.Options;
using VotingApp.Contracts.Services;
using VotingApp.Contracts.Settings;

namespace VotingApp.Services;

public class TimeService : ITimeService
{
    private readonly TimeSettings _settings;

    public TimeService(IOptions<TimeSettings> settings)
    {
        _settings = settings.Value;
    }

    public bool IsBeforeVotingTime()
    {
        var currentDateTime = DateTime.Now;
        return currentDateTime < _settings.BlockChainCalculationStartTime && currentDateTime < _settings.BlockChainCalculationEndTime;
    }

    public bool IsDuringVotingTime()
    {
        var currentDateTime = DateTime.Now;
        return _settings.BlockChainCalculationStartTime < currentDateTime && currentDateTime < _settings.BlockChainCalculationEndTime;
    }

    public bool IsAfterVotingTime()
    {
        var currentDateTime = DateTime.Now;
        return _settings.BlockChainCalculationStartTime < currentDateTime && _settings.BlockChainCalculationEndTime < currentDateTime;
    }
}
