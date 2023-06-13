using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentEmploymentPortal.Migrations
{
    public partial class YearsOfStudyToAcceptingMultiple : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "YearsOfStudy",
                table: "JobPost",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "YearsOfStudy",
                table: "JobPost",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");
        }
    }
}
