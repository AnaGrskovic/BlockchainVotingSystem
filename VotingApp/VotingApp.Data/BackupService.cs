using VotingApp.Contracts.Entities;
using VotingApp.Contracts.Services;
using VotingApp.Contracts.UoW;

namespace VotingApp.Services;

public class BackupService : IBackupService
{
    private readonly IUnitOfWork _uow;

    public BackupService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task CreateAsync(string candidate)
    {
        Vote vote = new Vote(candidate);
        _uow.Votes.Add(vote);
        await _uow.SaveChangesAsync();
    }

    public async Task<int> GetCountAsync()
    {
        var votes = await _uow.Votes.GetAllAsync();
        return votes.Count();
    }
}
