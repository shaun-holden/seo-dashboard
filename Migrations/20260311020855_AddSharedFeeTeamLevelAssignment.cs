using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSharedFeeTeamLevelAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SharedFeeTeamLevelAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SharedFeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamLevelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedFeeTeamLevelAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedFeeTeamLevelAssignments_SharedFees_SharedFeeId",
                        column: x => x.SharedFeeId,
                        principalTable: "SharedFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedFeeTeamLevelAssignments_TeamLevels_TeamLevelId",
                        column: x => x.TeamLevelId,
                        principalTable: "TeamLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharedFeeTeamLevelAssignments_SharedFeeId_TeamLevelId",
                table: "SharedFeeTeamLevelAssignments",
                columns: new[] { "SharedFeeId", "TeamLevelId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SharedFeeTeamLevelAssignments_TeamLevelId",
                table: "SharedFeeTeamLevelAssignments",
                column: "TeamLevelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharedFeeTeamLevelAssignments");
        }
    }
}
