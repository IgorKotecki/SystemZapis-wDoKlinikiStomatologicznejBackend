using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemZapisowDoKlinikiApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateTablesCancellationAdnCompletedAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CancelledAppointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AppointmentGroupId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    ServicesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancelledAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CancelledAppointments_AppointmentStatuses_AppointmentStatusId",
                        column: x => x.AppointmentStatusId,
                        principalTable: "AppointmentStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CancelledAppointments_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompletedAppointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AppointmentGroupId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    ServicesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentStatusId = table.Column<int>(type: "int", nullable: false),
                    AdditionalInformationJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedAppointments_AppointmentStatuses_AppointmentStatusId",
                        column: x => x.AppointmentStatusId,
                        principalTable: "AppointmentStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompletedAppointments_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CancelledAppointments_AppointmentStatusId",
                table: "CancelledAppointments",
                column: "AppointmentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CancelledAppointments_UserId",
                table: "CancelledAppointments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedAppointments_AppointmentStatusId",
                table: "CompletedAppointments",
                column: "AppointmentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedAppointments_UserId",
                table: "CompletedAppointments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CancelledAppointments");

            migrationBuilder.DropTable(
                name: "CompletedAppointments");
        }
    }
}
