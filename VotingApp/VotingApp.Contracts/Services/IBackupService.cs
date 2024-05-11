namespace VotingApp.Contracts.Services;

public interface IBackupService
{
    Task CreateAsync(string candidate);
}
