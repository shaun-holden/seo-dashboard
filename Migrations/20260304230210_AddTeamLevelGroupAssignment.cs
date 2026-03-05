using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamLevelGroupAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeamLevelGroupAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeamLevelId = table.Column<int>(type: "INTEGER", nullable: false),
                    SeasonGroupId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamLevelGroupAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamLevelGroupAssignments_SeasonGroups_SeasonGroupId",
                        column: x => x.SeasonGroupId,
                        principalTable: "SeasonGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamLevelGroupAssignments_TeamLevels_TeamLevelId",
                        column: x => x.TeamLevelId,
                        principalTable: "TeamLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamLevelGroupAssignments_SeasonGroupId",
                table: "TeamLevelGroupAssignments",
                column: "SeasonGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLevelGroupAssignments_TeamLevelId_SeasonGroupId",
                table: "TeamLevelGroupAssignments",
                columns: new[] { "TeamLevelId", "SeasonGroupId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamLevelGroupAssignments");
        }
    }
}
