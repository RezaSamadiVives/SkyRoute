using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyRoute.Domains.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToMealOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "MealOptions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "MealOptions");
        }
    }
}
