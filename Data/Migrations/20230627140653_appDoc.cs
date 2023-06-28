using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentEmploymentPortal.Migrations
{
    public partial class appDoc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Documet",
                table: "ApplicationDocument");

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
                name: "FilePath",
                table: "ApplicationDocument");

            migrationBuilder.AddColumn<byte[]>(
                name: "Documet",
                table: "ApplicationDocument",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
