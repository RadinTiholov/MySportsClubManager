using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySportsClubManager.Data.Migrations
{
    public partial class AddOwnedClubForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clubs_OwnerId",
                table: "Clubs");

            migrationBuilder.AddColumn<int>(
                name: "OwnedClubId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_OwnerId",
                table: "Clubs",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OwnedClubId",
                table: "AspNetUsers",
                column: "OwnedClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Clubs_OwnedClubId",
                table: "AspNetUsers",
                column: "OwnedClubId",
                principalTable: "Clubs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Clubs_OwnedClubId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_OwnerId",
                table: "Clubs");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_OwnedClubId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OwnedClubId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_OwnerId",
                table: "Clubs",
                column: "OwnerId",
                unique: true);
        }
    }
}
