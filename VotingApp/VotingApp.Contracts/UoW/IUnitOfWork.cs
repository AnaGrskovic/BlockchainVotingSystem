using VotingApp.Contracts.Entities;
using VotingApp.Contracts.Repositories;

namespace VotingApp.Contracts.UoW;

public interface IUnitOfWork
{
    IRepository<Vote> Votes { get; }
    IRepository<BlockChain> BlockChains { get; }
    IRepository<Peer> Peers { get; }

    Task SaveChangesAsync();
}
