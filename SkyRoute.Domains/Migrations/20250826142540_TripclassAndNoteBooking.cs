using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyRoute.Domains.Migrations
{
    /// <inheritdoc />
    public partial class TripclassAndNoteBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBusiness",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBusiness",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Bookings");
        }
    }
}
