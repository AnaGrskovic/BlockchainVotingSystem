using DummyAuthorizationProvider.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DummyAuthorizationProvider.Data.Db.Configuration;

public class VoterConfiguration : IEntityTypeConfiguration<Voter>
{
    public void Configure(EntityTypeBuilder<Voter> builder)
    {
        builder.ToTable("Voters");

        builder.HasKey(v => v.Id);

        builder.HasIndex(v => new { v.Oib })
            .IsUnique();
    }
}
