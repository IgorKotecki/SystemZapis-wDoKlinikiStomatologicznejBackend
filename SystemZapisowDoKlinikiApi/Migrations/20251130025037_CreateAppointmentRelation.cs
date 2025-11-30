using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemZapisowDoKlinikiApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateAppointmentRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentStatusId",
                table: "Appointment",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_AppointmentStatusId",
                table: "Appointment",
                column: "AppointmentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_AppointmentStatuses_AppointmentStatusId",
                table: "Appointment",
                column: "AppointmentStatusId",
                principalTable: "AppointmentStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_AppointmentStatuses_AppointmentStatusId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_AppointmentStatusId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "AppointmentStatusId",
                table: "Appointment");
        }
    }
}
