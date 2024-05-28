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
    public bool IsBlockChainCalculationTime()
    {
        var currentDateTime = DateTime.UtcNow;
        return _settings.BlockChainCaluldationStartTime < currentDateTime && currentDateTime < _settings.BlockChainCaluldationEndTime;
    }
}
