using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyRoute.Domains.Migrations
{
    /// <inheritdoc />
    public partial class ModifyMeals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocalMeal",
                table: "MealOptions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocalMeal",
                table: "MealOptions");
        }
    }
}
