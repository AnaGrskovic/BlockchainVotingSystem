using VotingApp.Contracts.Entities;
using VotingApp.Contracts.Repositories;
using VotingApp.Contracts.UoW;
using VotingApp.Data.Db.Context;
using VotingApp.Data.Db.Repositories;

namespace VotingApp.Data.Db.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly IRepository<Vote>? _votes;
    private readonly IRepository<BlockChain>? _blockChains;
    private readonly IRepository<Peer>? _peers;

    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public IRepository<Vote> Votes
        => _votes ?? new Repository<Vote>(_dbContext);
    public IRepository<BlockChain> BlockChains
        => _blockChains ?? new Repository<BlockChain>(_dbContext);
    public IRepository<Peer> Peers
        => _peers ?? new Repository<Peer>(_dbContext);

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
