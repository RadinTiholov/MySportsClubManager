using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySportsClubManager.Data.Migrations
{
    public partial class addSportToContest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SportId",
                table: "Contests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contests_SportId",
                table: "Contests",
                column: "SportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contests_Sports_SportId",
                table: "Contests",
                column: "SportId",
                principalTable: "Sports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contests_Sports_SportId",
                table: "Contests");

            migrationBuilder.DropIndex(
                name: "IX_Contests_SportId",
                table: "Contests");

            migrationBuilder.DropColumn(
                name: "SportId",
                table: "Contests");
        }
    }
}
