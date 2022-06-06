using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SHM_Smart_Hospital_Management_.Migrations
{
    public partial class AddRoomsAndReservationsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Room_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Room_Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Room_Floor = table.Column<int>(type: "int", nullable: false),
                    Room_Empty = table.Column<bool>(type: "bit", nullable: false),
                    Room_Beds_Count = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Ho_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Room_Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Hospitals_Ho_Id",
                        column: x => x.Ho_Id,
                        principalTable: "Hospitals",
                        principalColumn: "Ho_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Reservation_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Patient_Id = table.Column<int>(type: "int", nullable: false),
                    Room_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Reservation_Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Patients_Patient_Id",
                        column: x => x.Patient_Id,
                        principalTable: "Patients",
                        principalColumn: "Patient_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Rooms_Room_Id",
                        column: x => x.Room_Id,
                        principalTable: "Rooms",
                        principalColumn: "Room_Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Patient_Id",
                table: "Reservations",
                column: "Patient_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Room_Id",
                table: "Reservations",
                column: "Room_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_Ho_Id",
                table: "Rooms",
                column: "Ho_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
