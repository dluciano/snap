﻿using GameSharp.DataAccess;
using Microsoft.EntityFrameworkCore;
using Snap.Entities;

namespace Snap.DataAccess
{
    public sealed class SnapDbContext : GameSharpContext
    {
        public DbSet<PlayerGameplay> PlayerGamePlays { get; set; }
        public DbSet<PlayerData> PlayersData { get; set; }
        public DbSet<SnapGame> SnapGames { get; set; }
        public DbSet<StackNode> StackNodes { get; set; }

        public SnapDbContext()
        {
        }
        public SnapDbContext(DbContextOptions options) :
            base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (optionsBuilder.IsConfigured)
                return;
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=Snap.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<SnapGame>()
                .OwnsOne(p => p.CentralPile);
            modelBuilder
                .Entity<SnapGame>()
                .Ignore(p => p.CurrentTurn);
            modelBuilder
                .Entity<SnapGame>()
                .HasOne(p => p.GameData)
                .WithMany(p => p.SnapGames);
            modelBuilder
                .Entity<PlayerData>()
                .OwnsOne(p => p.StackEntity);
            modelBuilder
                .Entity<PlayerData>()
                .HasOne(p => p.SnapGame)
                .WithMany(r => r.PlayersData);
        }
    }
}
