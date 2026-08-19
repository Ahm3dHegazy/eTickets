using eTickets.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eTickets.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260819120000_AddCinemaLocation")]
    public partial class AddCinemaLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Cinemas",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Cinemas");
        }
    }
}
