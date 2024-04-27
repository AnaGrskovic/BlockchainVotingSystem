using DummyAuthorizationProvider.Contracts.Entities;
using DummyAuthorizationProvider.Contracts.Repositories;

namespace DummyAuthorizationProvider.Contracts.UoW;

public interface IUnitOfWork
{
    IRepository<Voter> Voters { get; }

    Task SaveChangesAsync();
}
