using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddEmergencyContactsAndPaymentPlanRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactName",
                table: "Gymnasts",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactPhone",
                table: "Gymnasts",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactRelationship",
                table: "Gymnasts",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalNotes",
                table: "Gymnasts",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentPlanRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentUserId = table.Column<string>(type: "TEXT", nullable: false),
                    AthleteId = table.Column<int>(type: "INTEGER", nullable: false),
                    RequestedMonths = table.Column<int>(type: "INTEGER", nullable: false),
                    RequestedStartMonth = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    AdminResponse = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentPlanRequests", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentPlanRequests");

            migrationBuilder.DropColumn(
                name: "EmergencyContactName",
                table: "Gymnasts");

            migrationBuilder.DropColumn(
                name: "EmergencyContactPhone",
                table: "Gymnasts");

            migrationBuilder.DropColumn(
                name: "EmergencyContactRelationship",
                table: "Gymnasts");

            migrationBuilder.DropColumn(
                name: "MedicalNotes",
                table: "Gymnasts");
        }
    }
}
