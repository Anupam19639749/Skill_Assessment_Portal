using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skill_Assessment_Portal_Backend.Migrations
{
    /// <inheritdoc />
    public partial class addedConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OptionsJson",
                table: "Questions",
                newName: "Options");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Options",
                table: "Questions",
                newName: "OptionsJson");
        }
    }
}
