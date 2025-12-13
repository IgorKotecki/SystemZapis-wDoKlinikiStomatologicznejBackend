using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemZapisowDoKlinikiApi.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceCategoryManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Services");

            migrationBuilder.CreateTable(
                name: "ServiceCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name_pl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name_en = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ServiceCategory_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Service_Category_Assignment",
                columns: table => new
                {
                    service_id = table.Column<int>(type: "int", nullable: false),
                    service_category_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Service_Category_Assignment_pk", x => new { x.service_id, x.service_category_id });
                    table.ForeignKey(
                        name: "FK_ServiceCategoryAssignment_Service",
                        column: x => x.service_id,
                        principalTable: "Services",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ServiceCategoryAssignment_ServiceCategory",
                        column: x => x.service_category_id,
                        principalTable: "ServiceCategory",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Service_Category_Assignment_service_category_id",
                table: "Service_Category_Assignment",
                column: "service_category_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Service_Category_Assignment");

            migrationBuilder.DropTable(
                name: "ServiceCategory");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
