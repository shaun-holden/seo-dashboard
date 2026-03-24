using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSeasonGymnast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeasonGymnasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: false),
                    GymnastId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonGymnasts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeasonGymnasts_Gymnasts_GymnastId",
                        column: x => x.GymnastId,
                        principalTable: "Gymnasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeasonGymnasts_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeasonGymnasts_GymnastId",
                table: "SeasonGymnasts",
                column: "GymnastId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonGymnasts_SeasonId_GymnastId",
                table: "SeasonGymnasts",
                columns: new[] { "SeasonId", "GymnastId" },
                unique: true);

            // Pre-populate from existing Athlete -> TeamLevel -> Season relationships
            migrationBuilder.Sql(@"
                INSERT OR IGNORE INTO SeasonGymnasts (SeasonId, GymnastId)
                SELECT DISTINCT tl.SeasonId, a.Id
                FROM Athletes a
                INNER JOIN TeamLevels tl ON a.TeamLevelId = tl.Id
                INNER JOIN Gymnasts g ON g.Id = a.Id
                WHERE g.IsArchived = 0
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeasonGymnasts");
        }
    }
}
