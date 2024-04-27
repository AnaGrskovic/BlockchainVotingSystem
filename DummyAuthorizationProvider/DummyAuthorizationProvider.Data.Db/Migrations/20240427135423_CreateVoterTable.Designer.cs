﻿// <auto-generated />
using System;
using DummyAuthorizationProvider.Data.Db.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DummyAuthorizationProvider.Data.Db.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240427135423_CreateVoterTable")]
    partial class CreateVoterTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DummyAuthorizationProvider.Contracts.Entities.Voter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Oib")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Oib")
                        .IsUnique();

                    b.ToTable("Voters", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}