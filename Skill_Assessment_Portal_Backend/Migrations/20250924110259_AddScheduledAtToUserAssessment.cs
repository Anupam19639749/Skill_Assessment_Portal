using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skill_Assessment_Portal_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduledAtToUserAssessment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduledAt",
                table: "Assessments");

            migrationBuilder.RenameColumn(
                name: "AssignedAt",
                table: "UserAssessments",
                newName: "ScheduledAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScheduledAt",
                table: "UserAssessments",
                newName: "AssignedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledAt",
                table: "Assessments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
