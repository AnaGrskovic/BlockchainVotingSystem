using Microsoft.Extensions.Options;
using VotingApp.Contracts.Exceptions;
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
        CheckTimes();
        var currentDateTime = DateTime.Now;
        return currentDateTime < _settings.BlockChainCalculationStartTime && currentDateTime < _settings.BlockChainCalculationEndTime && currentDateTime < _settings.BlockChainStabilizationEndTime;
    }

    public bool IsDuringVotingTime()
    {
        CheckTimes();
        var currentDateTime = DateTime.Now;
        return _settings.BlockChainCalculationStartTime < currentDateTime && currentDateTime < _settings.BlockChainCalculationEndTime && currentDateTime < _settings.BlockChainStabilizationEndTime;
    }

    public bool IsAfterVotingTime()
    {
        CheckTimes();
        var currentDateTime = DateTime.Now;
        return _settings.BlockChainCalculationStartTime < currentDateTime && _settings.BlockChainCalculationEndTime < currentDateTime;
    }

    public bool IsAfterStabilizationTime()
    {
        CheckTimes();
        var currentDateTime = DateTime.Now;
        return _settings.BlockChainCalculationStartTime < currentDateTime && _settings.BlockChainCalculationEndTime < currentDateTime && _settings.BlockChainStabilizationEndTime < currentDateTime;
    }

    private void CheckTimes()
    {
        if (_settings.BlockChainCalculationStartTime >= _settings.BlockChainCalculationEndTime || _settings.BlockChainCalculationEndTime >= _settings.BlockChainStabilizationEndTime)
        {
            throw new TimeSettingsInvalidException("Time stamps in the settings are invalid.");
        }
    }
}
