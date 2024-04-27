using DummyAuthorizationProvider.Contracts.Entities;
using DummyAuthorizationProvider.Contracts.Repositories;
using DummyAuthorizationProvider.Contracts.UoW;
using DummyAuthorizationProvider.Data.Db.Context;
using DummyAuthorizationProvider.Data.Db.Repositories;

namespace DummyAuthorizationProvider.Data.Db.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly IRepository<Voter>? _voters;

    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public IRepository<Voter> Voters
        => _voters ?? new Repository<Voter>(_dbContext);

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
