using DummyAuthorizationProvider.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DummyAuthorizationProvider.Data.Db.Context;

public class ApplicationDbContext : DbContext
{
    public DbSet<Voter> Voters { get; set; } = default!;

    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
