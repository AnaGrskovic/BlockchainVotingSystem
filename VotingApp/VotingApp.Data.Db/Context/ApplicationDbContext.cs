using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VotingApp.Contracts.Entities;

namespace VotingApp.Data.Db.Context;

public class ApplicationDbContext : DbContext
{
    public DbSet<Vote> Votes { get; set; } = default!;

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
