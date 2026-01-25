using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemZapisowDoKlinikiApi.Migrations
{
    /// <inheritdoc />
    public partial class CascadeOnUserOnCompletedAppointemnts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletedAppointments_User_UserId",
                table: "CompletedAppointments");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedAppointments_User_UserId",
                table: "CompletedAppointments",
                column: "UserId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletedAppointments_User_UserId",
                table: "CompletedAppointments");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedAppointments_User_UserId",
                table: "CompletedAppointments",
                column: "UserId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
