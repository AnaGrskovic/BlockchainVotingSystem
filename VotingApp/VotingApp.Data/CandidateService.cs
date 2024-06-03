using Microsoft.Extensions.Options;
using VotingApp.Contracts.Services;
using VotingApp.Contracts.Settings;

namespace VotingApp.Services;

public class CandidateService : ICandidateService
{
    private readonly CandidatesSettings _settings;

    public CandidateService(IOptions<CandidatesSettings> settings)
    {
        _settings = settings.Value;
    }

    public IEnumerable<string> GetAll()
    {
        return _settings.Candidates;
    }

    public bool Check(string candidate)
    {
        return GetAll().Contains(candidate);
    }
}
