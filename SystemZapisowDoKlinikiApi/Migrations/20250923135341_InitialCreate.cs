using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemZapisowDoKlinikiApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Additional_information",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    body_pl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    body_en = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Additional_information_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Roles_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    low_price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    high_price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    min_time = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Services_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Time_block",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    time_start = table.Column<DateTime>(type: "datetime", nullable: false),
                    time_end = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Time_block_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Tooth_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tooth_status_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    surname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Roles_id = table.Column<int>(type: "int", nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    salt = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    refreshToken = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    refreshTokenExpDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pk", x => x.id);
                    table.ForeignKey(
                        name: "FK_User_Roles",
                        column: x => x.Roles_id,
                        principalTable: "Roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Role_Service_Permissions",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false),
                    service_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role_Ser__95E9BE46E4746543", x => new { x.role_id, x.service_id });
                    table.ForeignKey(
                        name: "FK_Role_Service_Permissions_Role",
                        column: x => x.role_id,
                        principalTable: "Roles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Role_Service_Permissions_Service",
                        column: x => x.service_id,
                        principalTable: "Services",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceDependencies",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    service_id = table.Column<int>(type: "int", nullable: false),
                    required_service_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ServiceD__3213E83F4B64088B", x => x.id);
                    table.ForeignKey(
                        name: "FK_ServiceDependencies_RequiredService",
                        column: x => x.required_service_id,
                        principalTable: "Services",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ServiceDependencies_Service",
                        column: x => x.service_id,
                        principalTable: "Services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services_translation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    service_id = table.Column<int>(type: "int", nullable: false),
                    language_code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Services_translation_pk", x => x.id);
                    table.ForeignKey(
                        name: "FK_Services_translation_Service",
                        column: x => x.service_id,
                        principalTable: "Services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tooth_status_translation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tooth_status_id = table.Column<int>(type: "int", nullable: false),
                    language_code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tooth_status_translation_pk", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tooth_status_translation",
                        column: x => x.tooth_status_id,
                        principalTable: "Tooth_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    User_id = table.Column<int>(type: "int", nullable: false),
                    specialization = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    img_path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Doctor_pk", x => x.User_id);
                    table.ForeignKey(
                        name: "FK_Doctor_User",
                        column: x => x.User_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tooth",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tooth_status_id = table.Column<int>(type: "int", nullable: false),
                    tooth_number = table.Column<int>(type: "int", nullable: false),
                    User_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tooth_pk", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tooth_ToothStatus",
                        column: x => x.Tooth_status_id,
                        principalTable: "Tooth_status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Tooth_User",
                        column: x => x.User_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Day_scheme_time_block",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Doctor_User_id = table.Column<int>(type: "int", nullable: false),
                    week_day = table.Column<int>(type: "int", nullable: false),
                    job_start = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: false),
                    job_end = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Day_scheme_time_block_pk", x => x.id);
                    table.ForeignKey(
                        name: "FK_DaySchemeTimeBlock_Doctor",
                        column: x => x.Doctor_User_id,
                        principalTable: "Doctor",
                        principalColumn: "User_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doctor_block",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time_block_id = table.Column<int>(type: "int", nullable: false),
                    Doctor_User_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Doctor_block_pk", x => x.id);
                    table.ForeignKey(
                        name: "FK_DoctorBlock_Doctor",
                        column: x => x.Doctor_User_id,
                        principalTable: "Doctor",
                        principalColumn: "User_id");
                    table.ForeignKey(
                        name: "FK_DoctorBlock_TimeBlock",
                        column: x => x.Time_block_id,
                        principalTable: "Time_block",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_id = table.Column<int>(type: "int", nullable: false),
                    Doctor_block_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Appointment_pk", x => x.id);
                    table.ForeignKey(
                        name: "FK_Appointment_DoctorBlock",
                        column: x => x.Doctor_block_id,
                        principalTable: "Doctor_block",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointment_User",
                        column: x => x.User_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Apointment_informations",
                columns: table => new
                {
                    Appointment_id = table.Column<int>(type: "int", nullable: false),
                    Additional_information_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Apointment_informations_pk", x => new { x.Appointment_id, x.Additional_information_id });
                    table.ForeignKey(
                        name: "FK_AppInfo_AdditionalInformation",
                        column: x => x.Additional_information_id,
                        principalTable: "Additional_information",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_AppInfo_Appointment",
                        column: x => x.Appointment_id,
                        principalTable: "Appointment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments_services",
                columns: table => new
                {
                    Appointment_id = table.Column<int>(type: "int", nullable: false),
                    Services_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Appointments_services_pk", x => new { x.Appointment_id, x.Services_id });
                    table.ForeignKey(
                        name: "FK_AppointmentsServices_Appointment",
                        column: x => x.Appointment_id,
                        principalTable: "Appointment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentsServices_Services",
                        column: x => x.Services_id,
                        principalTable: "Services",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apointment_informations_Additional_information_id",
                table: "Apointment_informations",
                column: "Additional_information_id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_Doctor_block_id",
                table: "Appointment",
                column: "Doctor_block_id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_User_id",
                table: "Appointment",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_services_Services_id",
                table: "Appointments_services",
                column: "Services_id");

            migrationBuilder.CreateIndex(
                name: "IX_Day_scheme_time_block_Doctor_User_id",
                table: "Day_scheme_time_block",
                column: "Doctor_User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_block_Doctor_User_id",
                table: "Doctor_block",
                column: "Doctor_User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_block_Time_block_id",
                table: "Doctor_block",
                column: "Time_block_id");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Service_Permissions_service_id",
                table: "Role_Service_Permissions",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceDependencies_required_service_id",
                table: "ServiceDependencies",
                column: "required_service_id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceDependencies_service_id",
                table: "ServiceDependencies",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_Services_translation_service_id",
                table: "Services_translation",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tooth_Tooth_status_id",
                table: "Tooth",
                column: "Tooth_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tooth_User_id",
                table: "Tooth",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tooth_status_translation_tooth_status_id",
                table: "Tooth_status_translation",
                column: "tooth_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Roles_id",
                table: "User",
                column: "Roles_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apointment_informations");

            migrationBuilder.DropTable(
                name: "Appointments_services");

            migrationBuilder.DropTable(
                name: "Day_scheme_time_block");

            migrationBuilder.DropTable(
                name: "Role_Service_Permissions");

            migrationBuilder.DropTable(
                name: "ServiceDependencies");

            migrationBuilder.DropTable(
                name: "Services_translation");

            migrationBuilder.DropTable(
                name: "Tooth");

            migrationBuilder.DropTable(
                name: "Tooth_status_translation");

            migrationBuilder.DropTable(
                name: "Additional_information");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Tooth_status");

            migrationBuilder.DropTable(
                name: "Doctor_block");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropTable(
                name: "Time_block");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
