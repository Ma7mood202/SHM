using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SHM_Smart_Hospital_Management_.Migrations
{
    public partial class AddRequestsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Request_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Request_Type = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Request_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Request_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Accept = table.Column<bool>(type: "bit", nullable: false),
                    Patient_Id = table.Column<int>(type: "int", nullable: true),
                    Doctor_Id = table.Column<int>(type: "int", nullable: true),
                    Employee_Id = table.Column<int>(type: "int", nullable: true),
                    Request_Data = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Request_Id);
                    table.ForeignKey(
                        name: "FK_Requests_Doctors_Doctor_Id",
                        column: x => x.Doctor_Id,
                        principalTable: "Doctors",
                        principalColumn: "Doctor_Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Requests_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalTable: "Employees",
                        principalColumn: "Employee_Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Requests_Patients_Patient_Id",
                        column: x => x.Patient_Id,
                        principalTable: "Patients",
                        principalColumn: "Patient_Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_Doctor_Id",
                table: "Requests",
                column: "Doctor_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_Employee_Id",
                table: "Requests",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_Patient_Id",
                table: "Requests",
                column: "Patient_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");
        }
    }
}
