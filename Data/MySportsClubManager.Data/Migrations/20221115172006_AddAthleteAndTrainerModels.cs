using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySportsClubManager.Data.Migrations
{
    public partial class AddAthleteAndTrainerModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Clubs_EnrolledClubId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Clubs_OwnedClubId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Clubs_TrainedClubId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_AspNetUsers_OwnerId",
                table: "Clubs");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_OwnerId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Wins_AspNetUsers_UserId",
                table: "Wins");

            migrationBuilder.DropTable(
                name: "ApplicationUserContest");

            migrationBuilder.DropTable(
                name: "ApplicationUserTraining");

            migrationBuilder.DropIndex(
                name: "IX_Wins_UserId",
                table: "Wins");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_OwnerId",
                table: "Clubs");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EnrolledClubId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Wins");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "EnrolledClubId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TrainedClubId",
                table: "AspNetUsers",
                newName: "TrainerId");

            migrationBuilder.RenameColumn(
                name: "OwnedClubId",
                table: "AspNetUsers",
                newName: "AthleteId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_TrainedClubId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_TrainerId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_OwnedClubId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_AthleteId");

            migrationBuilder.AddColumn<int>(
                name: "AthleteId",
                table: "Wins",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "Reviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "TrainerId",
                table: "Clubs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Athletes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EnrolledClubId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Athletes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Athletes_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Athletes_Clubs_EnrolledClubId",
                        column: x => x.EnrolledClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OwnedClubId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trainers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trainers_Clubs_OwnedClubId",
                        column: x => x.OwnedClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AthleteContest",
                columns: table => new
                {
                    ContestsId = table.Column<int>(type: "int", nullable: false),
                    ParticipantsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AthleteContest", x => new { x.ContestsId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_AthleteContest_Athletes_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "Athletes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AthleteContest_Contests_ContestsId",
                        column: x => x.ContestsId,
                        principalTable: "Contests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AthleteTraining",
                columns: table => new
                {
                    EnrolledAthletesId = table.Column<int>(type: "int", nullable: false),
                    TrainingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AthleteTraining", x => new { x.EnrolledAthletesId, x.TrainingsId });
                    table.ForeignKey(
                        name: "FK_AthleteTraining_Athletes_EnrolledAthletesId",
                        column: x => x.EnrolledAthletesId,
                        principalTable: "Athletes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AthleteTraining_Trainings_TrainingsId",
                        column: x => x.TrainingsId,
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wins_AthleteId",
                table: "Wins",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_TrainerId",
                table: "Clubs",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_AthleteContest_ParticipantsId",
                table: "AthleteContest",
                column: "ParticipantsId");

            migrationBuilder.CreateIndex(
                name: "IX_Athletes_ApplicationUserId",
                table: "Athletes",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Athletes_EnrolledClubId",
                table: "Athletes",
                column: "EnrolledClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Athletes_IsDeleted",
                table: "Athletes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AthleteTraining_TrainingsId",
                table: "AthleteTraining",
                column: "TrainingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_ApplicationUserId",
                table: "Trainers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_IsDeleted",
                table: "Trainers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_OwnedClubId",
                table: "Trainers",
                column: "OwnedClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Athletes_AthleteId",
                table: "AspNetUsers",
                column: "AthleteId",
                principalTable: "Athletes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Trainers_TrainerId",
                table: "AspNetUsers",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_Trainers_TrainerId",
                table: "Clubs",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Athletes_OwnerId",
                table: "Reviews",
                column: "OwnerId",
                principalTable: "Athletes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wins_Athletes_AthleteId",
                table: "Wins",
                column: "AthleteId",
                principalTable: "Athletes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Athletes_AthleteId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Trainers_TrainerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_Trainers_TrainerId",
                table: "Clubs");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Athletes_OwnerId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Wins_Athletes_AthleteId",
                table: "Wins");

            migrationBuilder.DropTable(
                name: "AthleteContest");

            migrationBuilder.DropTable(
                name: "AthleteTraining");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropTable(
                name: "Athletes");

            migrationBuilder.DropIndex(
                name: "IX_Wins_AthleteId",
                table: "Wins");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_TrainerId",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "AthleteId",
                table: "Wins");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "Clubs");

            migrationBuilder.RenameColumn(
                name: "TrainerId",
                table: "AspNetUsers",
                newName: "TrainedClubId");

            migrationBuilder.RenameColumn(
                name: "AthleteId",
                table: "AspNetUsers",
                newName: "OwnedClubId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_TrainerId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_TrainedClubId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_AthleteId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_OwnedClubId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Wins",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Clubs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EnrolledClubId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationUserContest",
                columns: table => new
                {
                    ContestsId = table.Column<int>(type: "int", nullable: false),
                    ParticipantsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserContest", x => new { x.ContestsId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserContest_AspNetUsers_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUserContest_Contests_ContestsId",
                        column: x => x.ContestsId,
                        principalTable: "Contests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserTraining",
                columns: table => new
                {
                    EnrolledUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrainingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserTraining", x => new { x.EnrolledUsersId, x.TrainingsId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserTraining_AspNetUsers_EnrolledUsersId",
                        column: x => x.EnrolledUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUserTraining_Trainings_TrainingsId",
                        column: x => x.TrainingsId,
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wins_UserId",
                table: "Wins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_OwnerId",
                table: "Clubs",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EnrolledClubId",
                table: "AspNetUsers",
                column: "EnrolledClubId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserContest_ParticipantsId",
                table: "ApplicationUserContest",
                column: "ParticipantsId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserTraining_TrainingsId",
                table: "ApplicationUserTraining",
                column: "TrainingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Clubs_EnrolledClubId",
                table: "AspNetUsers",
                column: "EnrolledClubId",
                principalTable: "Clubs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Clubs_OwnedClubId",
                table: "AspNetUsers",
                column: "OwnedClubId",
                principalTable: "Clubs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Clubs_TrainedClubId",
                table: "AspNetUsers",
                column: "TrainedClubId",
                principalTable: "Clubs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_AspNetUsers_OwnerId",
                table: "Clubs",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_OwnerId",
                table: "Reviews",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wins_AspNetUsers_UserId",
                table: "Wins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
