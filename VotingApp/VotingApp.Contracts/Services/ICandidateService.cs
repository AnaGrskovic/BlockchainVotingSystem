namespace VotingApp.Contracts.Services;

public interface ICandidateService
{
    IEnumerable<string> GetAll();
    bool Check(string candidate);
}
