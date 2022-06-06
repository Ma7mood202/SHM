using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SHM_Smart_Hospital_Management_.Migrations
{
    public partial class AddSurgeryRoomsAndSurgeriesTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Surgery_Rooms",
                columns: table => new
                {
                    Surgery_Room_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Su_Room_Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Su_Room_Floor = table.Column<int>(type: "int", nullable: false),
                    Surgery_Room_Ready = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Ho_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surgery_Rooms", x => x.Surgery_Room_Id);
                    table.ForeignKey(
                        name: "FK_Surgery_Rooms_Hospitals_Ho_Id",
                        column: x => x.Ho_Id,
                        principalTable: "Hospitals",
                        principalColumn: "Ho_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Surgeries",
                columns: table => new
                {
                    Surgery_Number = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Surgery_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surgery_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Surgery_Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Surgery_Room_Id = table.Column<int>(type: "int", nullable: false),
                    Doctor_Id = table.Column<int>(type: "int", nullable: false),
                    Patient_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surgeries", x => x.Surgery_Number);
                    table.ForeignKey(
                        name: "FK_Surgeries_Doctors_Doctor_Id",
                        column: x => x.Doctor_Id,
                        principalTable: "Doctors",
                        principalColumn: "Doctor_Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Surgeries_Patients_Patient_Id",
                        column: x => x.Patient_Id,
                        principalTable: "Patients",
                        principalColumn: "Patient_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Surgeries_Surgery_Rooms_Surgery_Room_Id",
                        column: x => x.Surgery_Room_Id,
                        principalTable: "Surgery_Rooms",
                        principalColumn: "Surgery_Room_Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Surgeries_Doctor_Id",
                table: "Surgeries",
                column: "Doctor_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Surgeries_Patient_Id",
                table: "Surgeries",
                column: "Patient_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Surgeries_Surgery_Room_Id",
                table: "Surgeries",
                column: "Surgery_Room_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Surgery_Rooms_Ho_Id",
                table: "Surgery_Rooms",
                column: "Ho_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Surgeries");

            migrationBuilder.DropTable(
                name: "Surgery_Rooms");
        }
    }
}
