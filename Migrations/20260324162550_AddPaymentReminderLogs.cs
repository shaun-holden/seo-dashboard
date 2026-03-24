using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentReminderLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentReminderLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AthleteId = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentUserId = table.Column<string>(type: "TEXT", nullable: false),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: false),
                    InstallmentNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    ReminderType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SentAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentReminderLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentReminderLogs");
        }
    }
}
