﻿using CentralPeerCoordinator.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CentralPeerCoordinator.Data.Db.Configuration;

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

