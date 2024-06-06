using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VotingApp.Contracts.Entities;

namespace VotingApp.Data.Db.Configuration;

public class PeerConfiguration : IEntityTypeConfiguration<Peer>
{
    public void Configure(EntityTypeBuilder<Peer> builder)
    {
        builder.ToTable("Peers");

        builder.HasKey(p => p.Id);

        builder.HasIndex(p => new { p.IpAddress, p.Port })
            .IsUnique();
    }
}
