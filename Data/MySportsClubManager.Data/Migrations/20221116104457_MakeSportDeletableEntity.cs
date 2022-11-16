using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySportsClubManager.Data.Migrations
{
    public partial class MakeSportDeletableEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Sports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Sports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Sports_IsDeleted",
                table: "Sports",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sports_IsDeleted",
                table: "Sports");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Sports");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Sports");
        }
    }
}
