using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddMeetGroupAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeetGroupAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MeetId = table.Column<int>(type: "INTEGER", nullable: false),
                    SeasonGroupId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetGroupAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeetGroupAssignments_Meets_MeetId",
                        column: x => x.MeetId,
                        principalTable: "Meets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetGroupAssignments_SeasonGroups_SeasonGroupId",
                        column: x => x.SeasonGroupId,
                        principalTable: "SeasonGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeetGroupAssignments_MeetId_SeasonGroupId",
                table: "MeetGroupAssignments",
                columns: new[] { "MeetId", "SeasonGroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeetGroupAssignments_SeasonGroupId",
                table: "MeetGroupAssignments",
                column: "SeasonGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeetGroupAssignments");
        }
    }
}
