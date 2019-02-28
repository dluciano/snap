﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Snap.DataAccess;

namespace Snap.Server.Migrations
{
    [DbContext(typeof(SnapDbContext))]
    partial class SnapDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GameSharp.Entities.GameData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("CurrentState");

                    b.Property<int?>("CurrentTurnId");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int?>("FirstPlayerId");

                    b.HasKey("Id");

                    b.HasIndex("CurrentTurnId");

                    b.HasIndex("FirstPlayerId");

                    b.ToTable("GameDatas");

                    b.HasDiscriminator<string>("Discriminator").HasValue("GameData");
                });

            modelBuilder.Entity("GameSharp.Entities.GameRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("CanJoin");

                    b.Property<int?>("CreatedById");

                    b.Property<Guid>("GameIdentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("GameRooms");
                });

            modelBuilder.Entity("GameSharp.Entities.GameRoomPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsViewer");

                    b.Property<int>("PlayerId");

                    b.Property<int>("RoomId");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("PlayerId", "RoomId")
                        .IsUnique();

                    b.ToTable("GameRoomPlayers");
                });

            modelBuilder.Entity("GameSharp.Entities.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("GameSharp.Entities.PlayerTurn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("NextId");

                    b.Property<int?>("PlayerId");

                    b.HasKey("Id");

                    b.HasIndex("NextId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerTurns");
                });

            modelBuilder.Entity("Snap.Entities.PlayerData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("PlayerTurnId");

                    b.Property<int?>("SnapGameId");

                    b.HasKey("Id");

                    b.HasIndex("PlayerTurnId");

                    b.HasIndex("SnapGameId");

                    b.ToTable("PlayersData");
                });

            modelBuilder.Entity("Snap.Entities.PlayerGameplay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("Card");

                    b.Property<int?>("GameDataId");

                    b.Property<int?>("PlayerTurnId");

                    b.HasKey("Id");

                    b.HasIndex("GameDataId");

                    b.HasIndex("PlayerTurnId");

                    b.ToTable("PlayerGamePlays");
                });

            modelBuilder.Entity("Snap.Entities.SnapGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GameDataId");

                    b.Property<int?>("StackNodeId");

                    b.HasKey("Id");

                    b.HasIndex("GameDataId");

                    b.HasIndex("StackNodeId");

                    b.ToTable("SnapGames");
                });

            modelBuilder.Entity("Snap.Entities.StackNode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("Card");

                    b.Property<int?>("PreviousId");

                    b.HasKey("Id");

                    b.HasIndex("PreviousId");

                    b.ToTable("StackNodes");
                });

            modelBuilder.Entity("Snap.Entities.SnapGameData", b =>
                {
                    b.HasBaseType("GameSharp.Entities.GameData");

                    b.Property<int>("GameRoomId");

                    b.HasIndex("GameRoomId")
                        .IsUnique()
                        .HasFilter("[GameRoomId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("SnapGameData");
                });

            modelBuilder.Entity("GameSharp.Entities.GameData", b =>
                {
                    b.HasOne("GameSharp.Entities.PlayerTurn", "CurrentTurn")
                        .WithMany("CurrentTurns")
                        .HasForeignKey("CurrentTurnId");

                    b.HasOne("GameSharp.Entities.PlayerTurn", "FirstPlayer")
                        .WithMany("FirstPlayers")
                        .HasForeignKey("FirstPlayerId");
                });

            modelBuilder.Entity("GameSharp.Entities.GameRoom", b =>
                {
                    b.HasOne("GameSharp.Entities.Player", "CreatedBy")
                        .WithMany("CreatedRooms")
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("GameSharp.Entities.GameRoomPlayer", b =>
                {
                    b.HasOne("GameSharp.Entities.Player", "Player")
                        .WithMany("GameRoomPlayers")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GameSharp.Entities.GameRoom", "GameRoom")
                        .WithMany("RoomPlayers")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GameSharp.Entities.PlayerTurn", b =>
                {
                    b.HasOne("GameSharp.Entities.PlayerTurn", "Next")
                        .WithMany()
                        .HasForeignKey("NextId");

                    b.HasOne("GameSharp.Entities.Player", "Player")
                        .WithMany("PlayerTurns")
                        .HasForeignKey("PlayerId");
                });

            modelBuilder.Entity("Snap.Entities.PlayerData", b =>
                {
                    b.HasOne("GameSharp.Entities.PlayerTurn", "PlayerTurn")
                        .WithMany()
                        .HasForeignKey("PlayerTurnId");

                    b.HasOne("Snap.Entities.SnapGame", "SnapGame")
                        .WithMany("PlayersData")
                        .HasForeignKey("SnapGameId");

                    b.OwnsOne("Snap.Entities.StackEntity", "StackEntity", b1 =>
                        {
                            b1.Property<int>("PlayerDataId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<int?>("LastId");

                            b1.HasKey("PlayerDataId");

                            b1.HasIndex("LastId");

                            b1.ToTable("PlayersData");

                            b1.HasOne("Snap.Entities.StackNode", "Last")
                                .WithMany()
                                .HasForeignKey("LastId");

                            b1.HasOne("Snap.Entities.PlayerData")
                                .WithOne("StackEntity")
                                .HasForeignKey("Snap.Entities.StackEntity", "PlayerDataId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Snap.Entities.PlayerGameplay", b =>
                {
                    b.HasOne("Snap.Entities.SnapGameData", "GameData")
                        .WithMany("PlayerGamePlays")
                        .HasForeignKey("GameDataId");

                    b.HasOne("Snap.Entities.PlayerData", "PlayerTurn")
                        .WithMany("PlayerGameplay")
                        .HasForeignKey("PlayerTurnId");
                });

            modelBuilder.Entity("Snap.Entities.SnapGame", b =>
                {
                    b.HasOne("Snap.Entities.SnapGameData", "GameData")
                        .WithMany("SnapGames")
                        .HasForeignKey("GameDataId");

                    b.HasOne("Snap.Entities.StackNode")
                        .WithMany("SnapGames")
                        .HasForeignKey("StackNodeId");

                    b.OwnsOne("Snap.Entities.StackEntity", "CentralPile", b1 =>
                        {
                            b1.Property<int>("SnapGameId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<int?>("LastId");

                            b1.HasKey("SnapGameId");

                            b1.HasIndex("LastId");

                            b1.ToTable("SnapGames");

                            b1.HasOne("Snap.Entities.StackNode", "Last")
                                .WithMany()
                                .HasForeignKey("LastId");

                            b1.HasOne("Snap.Entities.SnapGame")
                                .WithOne("CentralPile")
                                .HasForeignKey("Snap.Entities.StackEntity", "SnapGameId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Snap.Entities.StackNode", b =>
                {
                    b.HasOne("Snap.Entities.StackNode", "Previous")
                        .WithMany()
                        .HasForeignKey("PreviousId");
                });

            modelBuilder.Entity("Snap.Entities.SnapGameData", b =>
                {
                    b.HasOne("GameSharp.Entities.GameRoom", "Room")
                        .WithOne("GamesData")
                        .HasForeignKey("Snap.Entities.SnapGameData", "GameRoomId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
