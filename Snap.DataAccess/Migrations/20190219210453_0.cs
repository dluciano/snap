using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Snap.DataAccess.Migrations
{
    public partial class _0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardPileNodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Card = table.Column<byte>(nullable: false),
                    PreviousId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardPileNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardPileNodes_CardPileNodes_PreviousId",
                        column: x => x.PreviousId,
                        principalTable: "CardPileNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "GameRooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    State = table.Column<int>(nullable: false),
                    FirstPlayerId = table.Column<int>(nullable: true),
                    CurrentTurnId = table.Column<int>(nullable: true),
                    CentralPileLastId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameRooms_CardPileNodes_CentralPileLastId",
                        column: x => x.CentralPileLastId,
                        principalTable: "CardPileNodes",
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
                    NextId = table.Column<int>(nullable: true),
                    GameRoomId = table.Column<int>(nullable: true),
                    LastId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTurns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerTurns_GameRooms_GameRoomId",
                        column: x => x.GameRoomId,
                        principalTable: "GameRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerTurns_CardPileNodes_LastId",
                        column: x => x.LastId,
                        principalTable: "CardPileNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "PlayerGamePlays",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Card = table.Column<byte>(nullable: false),
                    PlayerTurnId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerGamePlays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerGamePlays_PlayerTurns_PlayerTurnId",
                        column: x => x.PlayerTurnId,
                        principalTable: "PlayerTurns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardPileNodes_PreviousId",
                table: "CardPileNodes",
                column: "PreviousId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRoomPlayers_GameRoomId",
                table: "GameRoomPlayers",
                column: "GameRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRoomPlayers_PlayerId",
                table: "GameRoomPlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRooms_CentralPileLastId",
                table: "GameRooms",
                column: "CentralPileLastId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRooms_CurrentTurnId",
                table: "GameRooms",
                column: "CurrentTurnId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRooms_FirstPlayerId",
                table: "GameRooms",
                column: "FirstPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGamePlays_PlayerTurnId",
                table: "PlayerGamePlays",
                column: "PlayerTurnId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTurns_GameRoomId",
                table: "PlayerTurns",
                column: "GameRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTurns_LastId",
                table: "PlayerTurns",
                column: "LastId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTurns_NextId",
                table: "PlayerTurns",
                column: "NextId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTurns_PlayerId",
                table: "PlayerTurns",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameRooms_PlayerTurns_CurrentTurnId",
                table: "GameRooms",
                column: "CurrentTurnId",
                principalTable: "PlayerTurns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameRooms_PlayerTurns_FirstPlayerId",
                table: "GameRooms",
                column: "FirstPlayerId",
                principalTable: "PlayerTurns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTurns_GameRooms_GameRoomId",
                table: "PlayerTurns");

            migrationBuilder.DropTable(
                name: "GameRoomPlayers");

            migrationBuilder.DropTable(
                name: "PlayerGamePlays");

            migrationBuilder.DropTable(
                name: "GameRooms");

            migrationBuilder.DropTable(
                name: "PlayerTurns");

            migrationBuilder.DropTable(
                name: "CardPileNodes");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
