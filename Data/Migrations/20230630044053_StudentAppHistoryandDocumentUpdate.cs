using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentEmploymentPortal.Migrations
{
    public partial class StudentAppHistoryandDocumentUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Documet",
                table: "ApplicationDocument");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "StudentApplication",
                newName: "StudentApplicationStatus");

            migrationBuilder.AddColumn<bool>(
                name: "IsWithdrawn",
                table: "StudentApplication",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "ApplicationDocument",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWithdrawn",
                table: "StudentApplication");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "ApplicationDocument");

            migrationBuilder.RenameColumn(
                name: "StudentApplicationStatus",
                table: "StudentApplication",
                newName: "Status");

            migrationBuilder.AddColumn<byte[]>(
                name: "Documet",
                table: "ApplicationDocument",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
