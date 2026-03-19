using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBudgetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAthleteProfileFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Athletes",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Athletes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactName",
                table: "Athletes",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactPhone",
                table: "Athletes",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactRelationship",
                table: "Athletes",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalNotes",
                table: "Athletes",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentPhone",
                table: "Athletes",
                type: "TEXT",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "EmergencyContactName",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "EmergencyContactPhone",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "EmergencyContactRelationship",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "MedicalNotes",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "ParentPhone",
                table: "Athletes");
        }
    }
}
