using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSizesAndPaymentMonths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMonths",
                table: "Seasons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LeotardSize",
                table: "Gymnasts",
                type: "TEXT",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShirtSize",
                table: "Gymnasts",
                type: "TEXT",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMonths",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "LeotardSize",
                table: "Gymnasts");

            migrationBuilder.DropColumn(
                name: "ShirtSize",
                table: "Gymnasts");
        }
    }
}
