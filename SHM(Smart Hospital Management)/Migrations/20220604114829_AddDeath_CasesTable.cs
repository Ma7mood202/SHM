using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SHM_Smart_Hospital_Management_.Migrations
{
    public partial class AddDeath_CasesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Death_Cases",
                columns: table => new
                {
                    Death_Case_Number = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Death_Cause = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Death_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Patient_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Death_Cases", x => x.Death_Case_Number);
                    table.ForeignKey(
                        name: "FK_Death_Cases_Patients_Patient_Id",
                        column: x => x.Patient_Id,
                        principalTable: "Patients",
                        principalColumn: "Patient_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Death_Cases_Patient_Id",
                table: "Death_Cases",
                column: "Patient_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Death_Cases");
        }
    }
}
