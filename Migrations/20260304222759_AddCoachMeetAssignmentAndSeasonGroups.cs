using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCoachMeetAssignmentAndSeasonGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeasonGroupId",
                table: "Meets",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeasonGroupId",
                table: "Coaches",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeasonGroupId",
                table: "BudgetLineItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CoachMeetAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CoachId = table.Column<int>(type: "INTEGER", nullable: false),
                    MeetId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachMeetAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoachMeetAssignments_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoachMeetAssignments_Meets_MeetId",
                        column: x => x.MeetId,
                        principalTable: "Meets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeasonGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeasonGroups_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meets_SeasonGroupId",
                table: "Meets",
                column: "SeasonGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_SeasonGroupId",
                table: "Coaches",
                column: "SeasonGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLineItems_SeasonGroupId",
                table: "BudgetLineItems",
                column: "SeasonGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CoachMeetAssignments_CoachId_MeetId",
                table: "CoachMeetAssignments",
                columns: new[] { "CoachId", "MeetId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoachMeetAssignments_MeetId",
                table: "CoachMeetAssignments",
                column: "MeetId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonGroups_SeasonId",
                table: "SeasonGroups",
                column: "SeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetLineItems_SeasonGroups_SeasonGroupId",
                table: "BudgetLineItems",
                column: "SeasonGroupId",
                principalTable: "SeasonGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Coaches_SeasonGroups_SeasonGroupId",
                table: "Coaches",
                column: "SeasonGroupId",
                principalTable: "SeasonGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Meets_SeasonGroups_SeasonGroupId",
                table: "Meets",
                column: "SeasonGroupId",
                principalTable: "SeasonGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetLineItems_SeasonGroups_SeasonGroupId",
                table: "BudgetLineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Coaches_SeasonGroups_SeasonGroupId",
                table: "Coaches");

            migrationBuilder.DropForeignKey(
                name: "FK_Meets_SeasonGroups_SeasonGroupId",
                table: "Meets");

            migrationBuilder.DropTable(
                name: "CoachMeetAssignments");

            migrationBuilder.DropTable(
                name: "SeasonGroups");

            migrationBuilder.DropIndex(
                name: "IX_Meets_SeasonGroupId",
                table: "Meets");

            migrationBuilder.DropIndex(
                name: "IX_Coaches_SeasonGroupId",
                table: "Coaches");

            migrationBuilder.DropIndex(
                name: "IX_BudgetLineItems_SeasonGroupId",
                table: "BudgetLineItems");

            migrationBuilder.DropColumn(
                name: "SeasonGroupId",
                table: "Meets");

            migrationBuilder.DropColumn(
                name: "SeasonGroupId",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "SeasonGroupId",
                table: "BudgetLineItems");
        }
    }
}
