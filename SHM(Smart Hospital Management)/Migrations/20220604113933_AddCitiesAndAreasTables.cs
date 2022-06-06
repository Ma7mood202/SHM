using Microsoft.EntityFrameworkCore.Migrations;

namespace SHM_Smart_Hospital_Management_.Migrations
{
    public partial class AddCitiesAndAreasTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Departments_Department_Id",
                table: "Doctors");

            migrationBuilder.AlterColumn<int>(
                name: "Patient_Birth_Place",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Employee_Gender",
                table: "Employees",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Employee_Birth_Place",
                table: "Employees",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Doctor_Gender",
                table: "Doctors",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Doctor_Birth_Place",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Department_Id",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Area_Id",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    City_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.City_Id);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Area_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Area_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    City_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Area_Id);
                    table.ForeignKey(
                        name: "FK_Areas_Cities_City_Id",
                        column: x => x.City_Id,
                        principalTable: "Cities",
                        principalColumn: "City_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Area_Id",
                table: "Patients",
                column: "Area_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Patient_Birth_Place",
                table: "Patients",
                column: "Patient_Birth_Place");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_Area_Id",
                table: "Hospitals",
                column: "Area_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Area_Id",
                table: "Employees",
                column: "Area_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Employee_Birth_Place",
                table: "Employees",
                column: "Employee_Birth_Place");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_Area_Id",
                table: "Doctors",
                column: "Area_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_Doctor_Birth_Place",
                table: "Doctors",
                column: "Doctor_Birth_Place");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_City_Id",
                table: "Areas",
                column: "City_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Areas_Area_Id",
                table: "Doctors",
                column: "Area_Id",
                principalTable: "Areas",
                principalColumn: "Area_Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Cities_Doctor_Birth_Place",
                table: "Doctors",
                column: "Doctor_Birth_Place",
                principalTable: "Cities",
                principalColumn: "City_Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Departments_Department_Id",
                table: "Doctors",
                column: "Department_Id",
                principalTable: "Departments",
                principalColumn: "Department_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Areas_Area_Id",
                table: "Employees",
                column: "Area_Id",
                principalTable: "Areas",
                principalColumn: "Area_Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Cities_Employee_Birth_Place",
                table: "Employees",
                column: "Employee_Birth_Place",
                principalTable: "Cities",
                principalColumn: "City_Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Hospitals_Areas_Area_Id",
                table: "Hospitals",
                column: "Area_Id",
                principalTable: "Areas",
                principalColumn: "Area_Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Areas_Area_Id",
                table: "Patients",
                column: "Area_Id",
                principalTable: "Areas",
                principalColumn: "Area_Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Cities_Patient_Birth_Place",
                table: "Patients",
                column: "Patient_Birth_Place",
                principalTable: "Cities",
                principalColumn: "City_Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Areas_Area_Id",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Cities_Doctor_Birth_Place",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Departments_Department_Id",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Areas_Area_Id",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Cities_Employee_Birth_Place",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Hospitals_Areas_Area_Id",
                table: "Hospitals");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Areas_Area_Id",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Cities_Patient_Birth_Place",
                table: "Patients");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Patients_Area_Id",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_Patient_Birth_Place",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Hospitals_Area_Id",
                table: "Hospitals");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Area_Id",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Employee_Birth_Place",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_Area_Id",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_Doctor_Birth_Place",
                table: "Doctors");

            migrationBuilder.AlterColumn<int>(
                name: "Patient_Birth_Place",
                table: "Patients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Employee_Gender",
                table: "Employees",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);

            migrationBuilder.AlterColumn<int>(
                name: "Employee_Birth_Place",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Doctor_Gender",
                table: "Doctors",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);

            migrationBuilder.AlterColumn<int>(
                name: "Doctor_Birth_Place",
                table: "Doctors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Department_Id",
                table: "Doctors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Area_Id",
                table: "Doctors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Departments_Department_Id",
                table: "Doctors",
                column: "Department_Id",
                principalTable: "Departments",
                principalColumn: "Department_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
