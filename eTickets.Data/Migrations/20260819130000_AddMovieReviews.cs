using eTickets.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eTickets.Data.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260819130000_AddMovieReviews")]
public partial class AddMovieReviews : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "MovieReviews",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                MovieId = table.Column<int>(type: "int", nullable: false),
                ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Rating = table.Column<byte>(type: "tinyint", nullable: false),
                Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MovieReviews", x => x.Id);
                table.ForeignKey(
                    name: "FK_MovieReviews_AspNetUsers_ApplicationUserId",
                    column: x => x.ApplicationUserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_MovieReviews_Movies_MovieId",
                    column: x => x.MovieId,
                    principalTable: "Movies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_MovieReviews_ApplicationUserId",
            table: "MovieReviews",
            column: "ApplicationUserId");

        migrationBuilder.CreateIndex(
            name: "IX_MovieReviews_MovieId_ApplicationUserId",
            table: "MovieReviews",
            columns: new[] { "MovieId", "ApplicationUserId" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "MovieReviews");
    }
}
