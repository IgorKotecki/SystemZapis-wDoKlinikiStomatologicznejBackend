using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemZapisowDoKlinikiApi.Migrations
{
    /// <inheritdoc />
    public partial class DelteCascadeToServiceAndRelatedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_Service_Permissions_Role",
                table: "Role_Service_Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Service_Permissions_Service",
                table: "Role_Service_Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCategoryAssignment_Service",
                table: "Service_Category_Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCategoryAssignment_ServiceCategory",
                table: "Service_Category_Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceDependencies_RequiredService",
                table: "ServiceDependencies");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Service_Permissions_Role",
                table: "Role_Service_Permissions",
                column: "role_id",
                principalTable: "Roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Service_Permissions_Service",
                table: "Role_Service_Permissions",
                column: "service_id",
                principalTable: "Services",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCategoryAssignment_Service",
                table: "Service_Category_Assignment",
                column: "service_id",
                principalTable: "Services",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCategoryAssignment_ServiceCategory",
                table: "Service_Category_Assignment",
                column: "service_category_id",
                principalTable: "ServiceCategory",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceDependencies_RequiredService",
                table: "ServiceDependencies",
                column: "required_service_id",
                principalTable: "Services",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_Service_Permissions_Role",
                table: "Role_Service_Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Service_Permissions_Service",
                table: "Role_Service_Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCategoryAssignment_Service",
                table: "Service_Category_Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCategoryAssignment_ServiceCategory",
                table: "Service_Category_Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceDependencies_RequiredService",
                table: "ServiceDependencies");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Service_Permissions_Role",
                table: "Role_Service_Permissions",
                column: "role_id",
                principalTable: "Roles",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Service_Permissions_Service",
                table: "Role_Service_Permissions",
                column: "service_id",
                principalTable: "Services",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCategoryAssignment_Service",
                table: "Service_Category_Assignment",
                column: "service_id",
                principalTable: "Services",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCategoryAssignment_ServiceCategory",
                table: "Service_Category_Assignment",
                column: "service_category_id",
                principalTable: "ServiceCategory",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceDependencies_RequiredService",
                table: "ServiceDependencies",
                column: "required_service_id",
                principalTable: "Services",
                principalColumn: "id");
        }
    }
}
