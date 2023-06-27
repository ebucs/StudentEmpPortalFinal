using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentEmploymentPortal.Migrations
{
    public partial class qualificationmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Qualification",
                columns: table => new
                {
                    QualificationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Institution = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StuQualification = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Subjects = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Majors = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Submajors = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Research = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualification", x => x.QualificationId);
                    table.ForeignKey(
                        name: "FK_Qualification_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Qualification_StudentId",
                table: "Qualification",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Qualification");
        }
    }
}
