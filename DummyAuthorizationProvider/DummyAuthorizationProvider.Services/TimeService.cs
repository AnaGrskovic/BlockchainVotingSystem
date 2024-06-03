using DummyAuthorizationProvider.Contracts.Exceptions;
using DummyAuthorizationProvider.Contracts.Services;
using DummyAuthorizationProvider.Contracts.Settings;
using Microsoft.Extensions.Options;

namespace DummyAuthorizationProvider.Services;

public class TimeService : ITimeService
{
    private readonly TimeSettings _settings;

    public TimeService(IOptions<TimeSettings> settings)
    {
        _settings = settings.Value;
    }

    public bool IsBeforeVotingTime()
    {
        CheckTimes();
        var currentDateTime = DateTime.Now;
        return currentDateTime < _settings.BlockChainCalculationStartTime && currentDateTime < _settings.BlockChainCalculationEndTime;
    }

    public bool IsDuringVotingTime()
    {
        CheckTimes();
        var currentDateTime = DateTime.Now;
        return _settings.BlockChainCalculationStartTime < currentDateTime && currentDateTime < _settings.BlockChainCalculationEndTime;
    }

    public bool IsAfterVotingTime()
    {
        CheckTimes();
        var currentDateTime = DateTime.Now;
        return _settings.BlockChainCalculationStartTime < currentDateTime && _settings.BlockChainCalculationEndTime < currentDateTime;
    }

    private void CheckTimes()
    {
        if (_settings.BlockChainCalculationStartTime >= _settings.BlockChainCalculationEndTime)
        {
            throw new TimeSettingsInvalidException("Time stamps in the settings are invalid.");
        }
    }
}
