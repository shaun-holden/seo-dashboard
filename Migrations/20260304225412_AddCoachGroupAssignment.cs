using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCoachGroupAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoachGroupAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CoachId = table.Column<int>(type: "INTEGER", nullable: false),
                    SeasonGroupId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachGroupAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoachGroupAssignments_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoachGroupAssignments_SeasonGroups_SeasonGroupId",
                        column: x => x.SeasonGroupId,
                        principalTable: "SeasonGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoachGroupAssignments_CoachId_SeasonGroupId",
                table: "CoachGroupAssignments",
                columns: new[] { "CoachId", "SeasonGroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoachGroupAssignments_SeasonGroupId",
                table: "CoachGroupAssignments",
                column: "SeasonGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoachGroupAssignments");
        }
    }
}
