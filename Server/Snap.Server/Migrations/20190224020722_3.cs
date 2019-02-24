using Microsoft.EntityFrameworkCore.Migrations;

namespace Snap.Server.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameDatas_GameRooms_GameRoomId",
                table: "GameDatas");

            migrationBuilder.DropIndex(
                name: "IX_GameDatas_GameRoomId",
                table: "GameDatas");

            migrationBuilder.DropColumn(
                name: "GameRoomId",
                table: "GameDatas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameRoomId",
                table: "GameDatas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameDatas_GameRoomId",
                table: "GameDatas",
                column: "GameRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameDatas_GameRooms_GameRoomId",
                table: "GameDatas",
                column: "GameRoomId",
                principalTable: "GameRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
