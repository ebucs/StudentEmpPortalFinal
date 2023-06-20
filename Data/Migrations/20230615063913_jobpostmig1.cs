using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentEmploymentPortal.Migrations
{
    public partial class jobpostmig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobPost",
                columns: table => new
                {
                    JobPostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecruiterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    YearsOfStudyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecruiterType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Faculty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeyResponsibilities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartTimeNumberOfHours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "Date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "Date", nullable: false),
                    HourlyRate = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinRequirements = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationInstruction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClosingDate = table.Column<DateTime>(type: "Date", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    ApproversNote = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPost", x => x.JobPostId);
                    table.ForeignKey(
                        name: "FK_JobPost_Recruiter_RecruiterId",
                        column: x => x.RecruiterId,
                        principalTable: "Recruiter",
                        principalColumn: "RecruiterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YearsOfStudy",
                columns: table => new
                {
                    YearsOfStudyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JobPostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsFirstYear = table.Column<bool>(type: "bit", nullable: false),
                    IsSecondYear = table.Column<bool>(type: "bit", nullable: false),
                    IsThirdYear = table.Column<bool>(type: "bit", nullable: false),
                    IsHonours = table.Column<bool>(type: "bit", nullable: false),
                    IsGraduates = table.Column<bool>(type: "bit", nullable: false),
                    IsMasters = table.Column<bool>(type: "bit", nullable: false),
                    IsPhD = table.Column<bool>(type: "bit", nullable: false),
                    IsPostdoc = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearsOfStudy", x => x.YearsOfStudyId);
                    table.ForeignKey(
                        name: "FK_YearsOfStudy_JobPost_JobPostId",
                        column: x => x.JobPostId,
                        principalTable: "JobPost",
                        principalColumn: "JobPostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobPost_RecruiterId",
                table: "JobPost",
                column: "RecruiterId");

            migrationBuilder.CreateIndex(
                name: "IX_YearsOfStudy_JobPostId",
                table: "YearsOfStudy",
                column: "JobPostId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YearsOfStudy");

            migrationBuilder.DropTable(
                name: "JobPost");
        }
    }
}
