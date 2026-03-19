using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    CanViewBudget = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanEditBudget = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanViewAthletes = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanEditAthletes = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanViewCoaches = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanViewMeets = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanViewReports = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanViewPayments = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanManageRoster = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanViewNotes = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePermissions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeePermissions");
        }
    }
}
