using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SHM_Smart_Hospital_Management_.Migrations
{
    public partial class AddMedicalDetailsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Allergies",
                columns: table => new
                {
                    Allergy_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Allergy_Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergies", x => x.Allergy_Id);
                });

            migrationBuilder.CreateTable(
                name: "Diseases_Types",
                columns: table => new
                {
                    Disease_Type_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Disease_Type_Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diseases_Types", x => x.Disease_Type_Id);
                });

            migrationBuilder.CreateTable(
                name: "Medical_Details",
                columns: table => new
                {
                    Medical_Details_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MD_Patient_Blood_Type = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    MD_Patient_Treatment_Plans_And_Daily_Supplements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MD_Patient_Special_Needs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Patient_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medical_Details", x => x.Medical_Details_Id);
                    table.ForeignKey(
                        name: "FK_Medical_Details_Patients_Patient_Id",
                        column: x => x.Patient_Id,
                        principalTable: "Patients",
                        principalColumn: "Patient_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ray_Types",
                columns: table => new
                {
                    Ray_Type_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ray_Type_Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ray_Types", x => x.Ray_Type_Id);
                });

            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    Specialization_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Specialization_Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.Specialization_Id);
                });

            migrationBuilder.CreateTable(
                name: "Test_Types",
                columns: table => new
                {
                    Test_Type_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Test_Type_Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Types", x => x.Test_Type_Id);
                });

            migrationBuilder.CreateTable(
                name: "Diseases",
                columns: table => new
                {
                    Disease_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Disease_Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Disease_Type_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diseases", x => x.Disease_Id);
                    table.ForeignKey(
                        name: "FK_Diseases_Diseases_Types_Disease_Type_Id",
                        column: x => x.Disease_Type_Id,
                        principalTable: "Diseases_Types",
                        principalColumn: "Disease_Type_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "External_Records",
                columns: table => new
                {
                    External_Records_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Details = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Medical_Detail_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_External_Records", x => x.External_Records_Id);
                    table.ForeignKey(
                        name: "FK_External_Records_Medical_Details_Medical_Detail_Id",
                        column: x => x.Medical_Detail_Id,
                        principalTable: "Medical_Details",
                        principalColumn: "Medical_Details_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medical_Allergies",
                columns: table => new
                {
                    Allergy_Id = table.Column<int>(type: "int", nullable: false),
                    Medical_Detail_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medical_Allergies", x => new { x.Allergy_Id, x.Medical_Detail_Id });
                    table.ForeignKey(
                        name: "FK_Medical_Allergies_Allergies_Allergy_Id",
                        column: x => x.Allergy_Id,
                        principalTable: "Allergies",
                        principalColumn: "Allergy_Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Medical_Allergies_Medical_Details_Medical_Detail_Id",
                        column: x => x.Medical_Detail_Id,
                        principalTable: "Medical_Details",
                        principalColumn: "Medical_Details_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medical_Rays",
                columns: table => new
                {
                    Ray_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ray_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ray_Result = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Ray_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Medical_Detail_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medical_Rays", x => x.Ray_Id);
                    table.ForeignKey(
                        name: "FK_Medical_Rays_Medical_Details_Medical_Detail_Id",
                        column: x => x.Medical_Detail_Id,
                        principalTable: "Medical_Details",
                        principalColumn: "Medical_Details_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medical_Rays_Ray_Types_Ray_Type_Id",
                        column: x => x.Ray_Type_Id,
                        principalTable: "Ray_Types",
                        principalColumn: "Ray_Type_Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Test_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Test_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Test_Type_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Test_Id);
                    table.ForeignKey(
                        name: "FK_Tests_Test_Types_Test_Type_Id",
                        column: x => x.Test_Type_Id,
                        principalTable: "Test_Types",
                        principalColumn: "Test_Type_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medical_Diseases",
                columns: table => new
                {
                    Disease_Id = table.Column<int>(type: "int", nullable: false),
                    Medical_Detail_Id = table.Column<int>(type: "int", nullable: false),
                    Family_Health_History = table.Column<bool>(type: "bit", nullable: false),
                    Chronic_Diseases = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medical_Diseases", x => new { x.Disease_Id, x.Medical_Detail_Id });
                    table.ForeignKey(
                        name: "FK_Medical_Diseases_Diseases_Disease_Id",
                        column: x => x.Disease_Id,
                        principalTable: "Diseases",
                        principalColumn: "Disease_Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Medical_Diseases_Medical_Details_Medical_Detail_Id",
                        column: x => x.Medical_Detail_Id,
                        principalTable: "Medical_Details",
                        principalColumn: "Medical_Details_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medical_Tests",
                columns: table => new
                {
                    Medical_Test_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Test_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Test_Result = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Test_Id = table.Column<int>(type: "int", nullable: false),
                    Medical_Detail_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medical_Tests", x => x.Medical_Test_Id);
                    table.ForeignKey(
                        name: "FK_Medical_Tests_Medical_Details_Medical_Detail_Id",
                        column: x => x.Medical_Detail_Id,
                        principalTable: "Medical_Details",
                        principalColumn: "Medical_Details_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medical_Tests_Tests_Test_Id",
                        column: x => x.Test_Id,
                        principalTable: "Tests",
                        principalColumn: "Test_Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Department_Name",
                table: "Departments",
                column: "Department_Name");

            migrationBuilder.CreateIndex(
                name: "IX_Diseases_Disease_Type_Id",
                table: "Diseases",
                column: "Disease_Type_Id");

            migrationBuilder.CreateIndex(
                name: "IX_External_Records_Medical_Detail_Id",
                table: "External_Records",
                column: "Medical_Detail_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Medical_Allergies_Medical_Detail_Id",
                table: "Medical_Allergies",
                column: "Medical_Detail_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Medical_Details_Patient_Id",
                table: "Medical_Details",
                column: "Patient_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Medical_Diseases_Medical_Detail_Id",
                table: "Medical_Diseases",
                column: "Medical_Detail_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Medical_Rays_Medical_Detail_Id",
                table: "Medical_Rays",
                column: "Medical_Detail_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Medical_Rays_Ray_Type_Id",
                table: "Medical_Rays",
                column: "Ray_Type_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Medical_Tests_Medical_Detail_Id",
                table: "Medical_Tests",
                column: "Medical_Detail_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Medical_Tests_Test_Id",
                table: "Medical_Tests",
                column: "Test_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_Test_Type_Id",
                table: "Tests",
                column: "Test_Type_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Specializations_Department_Name",
                table: "Departments",
                column: "Department_Name",
                principalTable: "Specializations",
                principalColumn: "Specialization_Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Specializations_Department_Name",
                table: "Departments");

            migrationBuilder.DropTable(
                name: "External_Records");

            migrationBuilder.DropTable(
                name: "Medical_Allergies");

            migrationBuilder.DropTable(
                name: "Medical_Diseases");

            migrationBuilder.DropTable(
                name: "Medical_Rays");

            migrationBuilder.DropTable(
                name: "Medical_Tests");

            migrationBuilder.DropTable(
                name: "Specializations");

            migrationBuilder.DropTable(
                name: "Allergies");

            migrationBuilder.DropTable(
                name: "Diseases");

            migrationBuilder.DropTable(
                name: "Ray_Types");

            migrationBuilder.DropTable(
                name: "Medical_Details");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Diseases_Types");

            migrationBuilder.DropTable(
                name: "Test_Types");

            migrationBuilder.DropIndex(
                name: "IX_Departments_Department_Name",
                table: "Departments");
        }
    }
}
