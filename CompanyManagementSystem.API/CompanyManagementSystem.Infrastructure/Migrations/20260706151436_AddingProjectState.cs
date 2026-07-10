using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingProjectState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectStatus",
                table: "Projects",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectStatus",
                table: "Projects");
        }
    }
}
