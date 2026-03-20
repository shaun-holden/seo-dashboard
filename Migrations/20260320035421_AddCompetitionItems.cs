using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCompetitionItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompetitionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    AppliesTo = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitionItems_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GymnastCompetitionSelections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GymnastId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IsSelected = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymnastCompetitionSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GymnastCompetitionSelections_Gymnasts_GymnastId",
                        column: x => x.GymnastId,
                        principalTable: "Gymnasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionItems_SeasonId",
                table: "CompetitionItems",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_GymnastCompetitionSelections_GymnastId",
                table: "GymnastCompetitionSelections",
                column: "GymnastId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitionItems");

            migrationBuilder.DropTable(
                name: "GymnastCompetitionSelections");
        }
    }
}
