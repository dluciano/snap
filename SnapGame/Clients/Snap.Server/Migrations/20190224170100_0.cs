using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Snap.Server.Migrations
{
    public partial class _0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameRooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CanJoin = table.Column<bool>(nullable: false),
                    GameIdentifier = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StackNodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Card = table.Column<byte>(nullable: false),
                    PreviousId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StackNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StackNodes_StackNodes_PreviousId",
                        column: x => x.PreviousId,
                        principalTable: "StackNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameRoomPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<int>(nullable: true),
                    IsViewer = table.Column<bool>(nullable: false),
                    GameRoomId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRoomPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameRoomPlayers_GameRooms_GameRoomId",
                        column: x => x.GameRoomId,
                        principalTable: "GameRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameRoomPlayers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerTurns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<int>(nullable: true),
                    NextId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTurns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerTurns_PlayerTurns_NextId",
                        column: x => x.NextId,
                        principalTable: "PlayerTurns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerTurns_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameDatas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstPlayerId = table.Column<int>(nullable: true),
                    CurrentTurnId = table.Column<int>(nullable: true),
                    CurrentState = table.Column<byte>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    GameRoomId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameDatas_PlayerTurns_CurrentTurnId",
                        column: x => x.CurrentTurnId,
                        principalTable: "PlayerTurns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameDatas_PlayerTurns_FirstPlayerId",
                        column: x => x.FirstPlayerId,
                        principalTable: "PlayerTurns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameDatas_GameRooms_GameRoomId",
                        column: x => x.GameRoomId,
                        principalTable: "GameRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SnapGames",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameDataId = table.Column<int>(nullable: true),
                    CentralPile_LastId = table.Column<int>(nullable: true),
                    StackNodeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SnapGames_GameDatas_GameDataId",
                        column: x => x.GameDataId,
                        principalTable: "GameDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SnapGames_StackNodes_StackNodeId",
                        column: x => x.StackNodeId,
                        principalTable: "StackNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SnapGames_StackNodes_CentralPile_LastId",
                        column: x => x.CentralPile_LastId,
                        principalTable: "StackNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayersData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StackEntity_LastId = table.Column<int>(nullable: true),
                    PlayerTurnId = table.Column<int>(nullable: true),
                    SnapGameId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayersData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayersData_PlayerTurns_PlayerTurnId",
                        column: x => x.PlayerTurnId,
                        principalTable: "PlayerTurns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayersData_SnapGames_SnapGameId",
                        column: x => x.SnapGameId,
                        principalTable: "SnapGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayersData_StackNodes_StackEntity_LastId",
                        column: x => x.StackEntity_LastId,
                        principalTable: "StackNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerGamePlays",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Card = table.Column<byte>(nullable: false),
                    PlayerTurnId = table.Column<int>(nullable: true),
                    GameDataId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerGamePlays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerGamePlays_GameDatas_GameDataId",
                        column: x => x.GameDataId,
                        principalTable: "GameDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerGamePlays_PlayersData_PlayerTurnId",
                        column: x => x.PlayerTurnId,
                        principalTable: "PlayersData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameDatas_CurrentTurnId",
                table: "GameDatas",
                column: "CurrentTurnId");

            migrationBuilder.CreateIndex(
                name: "IX_GameDatas_FirstPlayerId",
                table: "GameDatas",
                column: "FirstPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GameDatas_GameRoomId",
                table: "GameDatas",
                column: "GameRoomId",
                unique: true,
                filter: "[GameRoomId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GameRoomPlayers_GameRoomId",
                table: "GameRoomPlayers",
                column: "GameRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRoomPlayers_PlayerId",
                table: "GameRoomPlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGamePlays_GameDataId",
                table: "PlayerGamePlays",
                column: "GameDataId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGamePlays_PlayerTurnId",
                table: "PlayerGamePlays",
                column: "PlayerTurnId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersData_PlayerTurnId",
                table: "PlayersData",
                column: "PlayerTurnId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersData_SnapGameId",
                table: "PlayersData",
                column: "SnapGameId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersData_StackEntity_LastId",
                table: "PlayersData",
                column: "StackEntity_LastId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTurns_NextId",
                table: "PlayerTurns",
                column: "NextId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTurns_PlayerId",
                table: "PlayerTurns",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapGames_GameDataId",
                table: "SnapGames",
                column: "GameDataId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapGames_StackNodeId",
                table: "SnapGames",
                column: "StackNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapGames_CentralPile_LastId",
                table: "SnapGames",
                column: "CentralPile_LastId");

            migrationBuilder.CreateIndex(
                name: "IX_StackNodes_PreviousId",
                table: "StackNodes",
                column: "PreviousId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameRoomPlayers");

            migrationBuilder.DropTable(
                name: "PlayerGamePlays");

            migrationBuilder.DropTable(
                name: "PlayersData");

            migrationBuilder.DropTable(
                name: "SnapGames");

            migrationBuilder.DropTable(
                name: "GameDatas");

            migrationBuilder.DropTable(
                name: "StackNodes");

            migrationBuilder.DropTable(
                name: "PlayerTurns");

            migrationBuilder.DropTable(
                name: "GameRooms");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
