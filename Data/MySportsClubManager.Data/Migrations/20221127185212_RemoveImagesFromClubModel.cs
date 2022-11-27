using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySportsClubManager.Data.Migrations
{
    public partial class RemoveImagesFromClubModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Clubs_ClubId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ClubId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "Images");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Clubs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_ImageId",
                table: "Clubs",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_Images_ImageId",
                table: "Clubs",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_Images_ImageId",
                table: "Clubs");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_ImageId",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Clubs");

            migrationBuilder.AddColumn<int>(
                name: "ClubId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_ClubId",
                table: "Images",
                column: "ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Clubs_ClubId",
                table: "Images",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id");
        }
    }
}
