using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VotingApp.Contracts.Entities;

namespace VotingApp.Data.Db.Configuration;

public class BlockChainConfiguration : IEntityTypeConfiguration<BlockChain>
{
    public void Configure(EntityTypeBuilder<BlockChain> builder)
    {
        builder.ToTable("BlockChains");

        builder.HasKey(p => p.Id);
    }
}
