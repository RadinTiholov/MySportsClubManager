using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySportsClubManager.Data.Migrations
{
    public partial class FixNaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Athletes_OwnerId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Messages",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_OwnerId",
                table: "Messages",
                newName: "IX_Messages_SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Athletes_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "Athletes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Athletes_SenderId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Messages",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                newName: "IX_Messages_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Athletes_OwnerId",
                table: "Messages",
                column: "OwnerId",
                principalTable: "Athletes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
