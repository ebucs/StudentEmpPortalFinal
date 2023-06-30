using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentEmploymentPortal.Migrations
{
    public partial class manyDocsMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationDocument_StudentApplication_StudentApplicationApplicationId",
                table: "ApplicationDocument");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationDocument_StudentApplicationApplicationId",
                table: "ApplicationDocument");

            migrationBuilder.DropColumn(
                name: "StudentApplicationApplicationId",
                table: "ApplicationDocument");

            migrationBuilder.AddColumn<string>(
                name: "StudentApplicationId",
                table: "ApplicationDocument",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocument_StudentApplicationId",
                table: "ApplicationDocument",
                column: "StudentApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationDocument_StudentApplication_StudentApplicationId",
                table: "ApplicationDocument",
                column: "StudentApplicationId",
                principalTable: "StudentApplication",
                principalColumn: "ApplicationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationDocument_StudentApplication_StudentApplicationId",
                table: "ApplicationDocument");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationDocument_StudentApplicationId",
                table: "ApplicationDocument");

            migrationBuilder.DropColumn(
                name: "StudentApplicationId",
                table: "ApplicationDocument");

            migrationBuilder.AddColumn<string>(
                name: "StudentApplicationApplicationId",
                table: "ApplicationDocument",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocument_StudentApplicationApplicationId",
                table: "ApplicationDocument",
                column: "StudentApplicationApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationDocument_StudentApplication_StudentApplicationApplicationId",
                table: "ApplicationDocument",
                column: "StudentApplicationApplicationId",
                principalTable: "StudentApplication",
                principalColumn: "ApplicationId");
        }
    }
}
