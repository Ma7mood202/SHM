using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SHM_Smart_Hospital_Management_.Migrations
{
    public partial class AddBaseTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Doctor_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "111111, 1"),
                    Doctor_First_Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Doctor_Middle_Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Doctor_Last_Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Doctor_National_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Doctor_Gender = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Doctor_Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Doctor_Password = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Doctor_Family_Members = table.Column<int>(type: "int", nullable: true),
                    Doctor_Qualifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Doctor_Social_Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Doctor_Birth_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Doctor_Hire_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Doctor_Birth_Place = table.Column<int>(type: "int", nullable: false),
                    Area_Id = table.Column<int>(type: "int", nullable: false),
                    Department_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Doctor_Id);
                });

            migrationBuilder.CreateTable(
                name: "Doctor_Phone_Numbers",
                columns: table => new
                {
                    Doctor_Id = table.Column<int>(type: "int", nullable: false),
                    Doctor_Phone_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor_Phone_Numbers", x => new { x.Doctor_Id, x.Doctor_Phone_Number });
                    table.ForeignKey(
                        name: "FK_Doctor_Phone_Numbers_Doctors_Doctor_Id",
                        column: x => x.Doctor_Id,
                        principalTable: "Doctors",
                        principalColumn: "Doctor_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Department_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Department_Name = table.Column<int>(type: "int", nullable: false),
                    Ho_Id = table.Column<int>(type: "int", nullable: false),
                    Dept_Mgr_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Department_Id);
                    table.ForeignKey(
                        name: "FK_Departments_Doctors_Dept_Mgr_Id",
                        column: x => x.Dept_Mgr_Id,
                        principalTable: "Doctors",
                        principalColumn: "Doctor_Id",
                        onDelete: ReferentialAction.NoAction);
                    table.UniqueConstraint("Department_Mgr_Id_Same_Hospital_Unique", x =>new { x.Dept_Mgr_Id, x.Ho_Id });

                });

            migrationBuilder.CreateTable(
                name: "Employee_Phone_Numbers",
                columns: table => new
                {
                    Employee_Id = table.Column<int>(type: "int", nullable: false),
                    Employee_Phone_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee_Phone_Numbers", x => new { x.Employee_Id, x.Employee_Phone_Number });
                });

            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    Ho_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ho_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ho_Subscribtion_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Area_Id = table.Column<int>(type: "int", nullable: false),
                    Mgr_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.Ho_Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Employee_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "111111, 1"),
                    Employee_First_Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Employee_Middle_Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Employee_Last_Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Employee_Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Employee_Password = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Employee_National_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Employee_Gender = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Employee_X_Y = table.Column<string>(type: "nvarchar(61)", maxLength: 61, nullable: false),
                    Employee_Family_Members = table.Column<int>(type: "int", nullable: true),
                    Employee_Job = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Employee_Social_Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Employee_Birth_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Employee_Hire_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Area_Id = table.Column<int>(type: "int", nullable: false),
                    Employee_Birth_Place = table.Column<int>(type: "int", nullable: false),
                    Ho_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Employee_Id);
                    table.ForeignKey(
                        name: "FK_Employees_Hospitals_Ho_Id",
                        column: x => x.Ho_Id,
                        principalTable: "Hospitals",
                        principalColumn: "Ho_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hospital_Phone_Numbers",
                columns: table => new
                {
                    Ho_Id = table.Column<int>(type: "int", nullable: false),
                    Hospital_Phone_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospital_Phone_Numbers", x => new { x.Ho_Id, x.Hospital_Phone_Number });
                    table.ForeignKey(
                        name: "FK_Hospital_Phone_Numbers_Hospitals_Ho_Id",
                        column: x => x.Ho_Id,
                        principalTable: "Hospitals",
                        principalColumn: "Ho_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Patient_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "111111, 1"),
                    Patient_First_Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Patient_Middle_Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Patient_Last_Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Patient_Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Patient_Password = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Patient_X_Y = table.Column<string>(type: "nvarchar(61)", maxLength: 61, nullable: false),
                    Patient_National_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Patient_Gender = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Patient_Social_Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Patient_Birth_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Canceled = table.Column<bool>(type: "bit", nullable: false),
                    Sent = table.Column<bool>(type: "bit", nullable: false),
                    PreviewCount = table.Column<int>(type: "int", nullable: true),
                    Area_Id = table.Column<int>(type: "int", nullable: false),
                    Patient_Birth_Place = table.Column<int>(type: "int", nullable: false),
                    Ho_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Patient_Id);
                    table.ForeignKey(
                        name: "FK_Patients_Hospitals_Ho_Id",
                        column: x => x.Ho_Id,
                        principalTable: "Hospitals",
                        principalColumn: "Ho_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Phone_Numbers",
                columns: table => new
                {
                    Patient_Id = table.Column<int>(type: "int", nullable: false),
                    Patient_Phone_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Phone_Numbers", x => new { x.Patient_Id, x.Patient_Phone_Number });
                    table.ForeignKey(
                        name: "FK_Patient_Phone_Numbers_Patients_Patient_Id",
                        column: x => x.Patient_Id,
                        principalTable: "Patients",
                        principalColumn: "Patient_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Dept_Mgr_Id",
                table: "Departments",
                column: "Dept_Mgr_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Ho_Id",
                table: "Departments",
                column: "Ho_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_Department_Id",
                table: "Doctors",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Ho_Id",
                table: "Employees",
                column: "Ho_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_Mgr_Id",
                table: "Hospitals",
                column: "Mgr_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Ho_Id",
                table: "Patients",
                column: "Ho_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Departments_Department_Id",
                table: "Doctors",
                column: "Department_Id",
                principalTable: "Departments",
                principalColumn: "Department_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Hospitals_Ho_Id",
                table: "Departments",
                column: "Ho_Id",
                principalTable: "Hospitals",
                principalColumn: "Ho_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Phone_Numbers_Employees_Employee_Id",
                table: "Employee_Phone_Numbers",
                column: "Employee_Id",
                principalTable: "Employees",
                principalColumn: "Employee_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hospitals_Employees_Mgr_Id",
                table: "Hospitals",
                column: "Mgr_Id",
                principalTable: "Employees",
                principalColumn: "Employee_Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Doctors_Dept_Mgr_Id",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Hospitals_Ho_Id",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Doctor_Phone_Numbers");

            migrationBuilder.DropTable(
                name: "Employee_Phone_Numbers");

            migrationBuilder.DropTable(
                name: "Hospital_Phone_Numbers");

            migrationBuilder.DropTable(
                name: "Patient_Phone_Numbers");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Hospitals");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
