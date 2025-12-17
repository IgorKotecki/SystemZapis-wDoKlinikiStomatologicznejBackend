using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemZapisowDoKlinikiApi.Migrations
{
    /// <inheritdoc />
    public partial class photoUrlAdding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "photo_url",
                table: "User",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "photo_url",
                table: "Services",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "photo_url",
                table: "User");

            migrationBuilder.DropColumn(
                name: "photo_url",
                table: "Services");
        }
    }
}
