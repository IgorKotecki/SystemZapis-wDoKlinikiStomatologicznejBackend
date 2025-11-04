using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemZapisowDoKlinikiApi.Migrations
{
    /// <inheritdoc />
    public partial class AddToothStatusCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.RenameColumn(
                                                             //     name: "specialization",
                                                             //     table: "Doctor",
                                                             //     newName: "specialization_pl");

            migrationBuilder.AddColumn<int>(
                name: "category_id",
                table: "Tooth_status",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "required_service_id",
                table: "ServiceDependencies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            // migrationBuilder.AddColumn<string>(
            //     name: "specialization_en",
            //     table: "Doctor",
            //     type: "nvarchar(100)",
            //     maxLength: 100,
            //     nullable: false,
            //     defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Tooth_status_category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tooth_status_category_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Tooth_status_category_translation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_id = table.Column<int>(type: "int", nullable: false),
                    language_code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tooth_status_category_translation_pk", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tooth_status_category_translation",
                        column: x => x.category_id,
                        principalTable: "Tooth_status_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tooth_status_category_id",
                table: "Tooth_status",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tooth_status_category_translation_category_id",
                table: "Tooth_status_category_translation",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tooth_status_category",
                table: "Tooth_status",
                column: "category_id",
                principalTable: "Tooth_status_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tooth_status_category",
                table: "Tooth_status");

            migrationBuilder.DropTable(
                name: "Tooth_status_category_translation");

            migrationBuilder.DropTable(
                name: "Tooth_status_category");

            migrationBuilder.DropIndex(
                name: "IX_Tooth_status_category_id",
                table: "Tooth_status");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "Tooth_status");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "specialization_en",
                table: "Doctor");

            migrationBuilder.RenameColumn(
                name: "specialization_pl",
                table: "Doctor",
                newName: "specialization");

            migrationBuilder.AlterColumn<int>(
                name: "required_service_id",
                table: "ServiceDependencies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
