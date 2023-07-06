using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using StudentEmploymentPortal.Data;
using StudentEmploymentPortal.Utility;
using StudentEmploymentPortal.Areas.Identity;
using Bogus;
using Microsoft.EntityFrameworkCore;
using StudentEmploymentPortal.Areas.jobpostA.Models;

namespace StudentEmploymentPortal.DataSeeding
{
    public static class SeedData
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

                await SeedRolesAsync(roleManager);
                await SeedApproverAsync(userManager);
                await SeedAdminAsync(userManager);
                await SeedJobPostsAsync(dbContext);

                // Perform other data seeding if needed
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(SD.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.Admin));
            }

            if (!await roleManager.RoleExistsAsync(SD.Approver))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.Approver));
            }

            // Add other roles if needed
        }

        private static async Task SeedApproverAsync(UserManager<ApplicationUser> userManager)
        {
            var email = "approver0@gmail.com";
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = "approver",
                    Surname = "approver",
                    Telephone = "0116789876",
                    PhoneNumber = "0116789876",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Approver01#");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, SD.Approver);
                }
            }
        }

        private static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            var email = "admin0@gmail.com";
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = "admin",
                    Surname = "admin",
                    Telephone = "0116789876",
                    PhoneNumber = "0116789876",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin01#");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, SD.Admin);
                }
            }
        }

        private static async Task SeedJobPostsAsync(ApplicationDbContext dbContext)
        {
            var recruiters = await dbContext.Recruiter.ToListAsync();

            var faker = new Faker();

            var jobPostFaker = new Faker<JobPost>()
                .RuleFor(j => j.JobTitle, f => f.Name.JobTitle())
                .RuleFor(j => j.Location, f => f.Address.City())
                .RuleFor(j => j.JobDescription, f => f.Lorem.Paragraph())
                .RuleFor(j => j.KeyResponsibilities, f => f.Lorem.Paragraph())
                .RuleFor(j => j.JobType, f => f.PickRandom<JobPost.EnumJobType>())
                .RuleFor(j => j.StartDate, f => f.Date.Future())
                .RuleFor(j => j.EndDate, (f, j) => f.Date.Between(j.StartDate, j.StartDate.AddDays(30)))
                .RuleFor(j => j.HourlyRate, f => f.Random.Number(10, 50))
                .RuleFor(j => j.MinRequirements, f => f.Lorem.Paragraph())
                .RuleFor(j => j.ApplicationInstruction, f => f.Lorem.Paragraph())
                .RuleFor(j => j.ClosingDate, (f, j) => f.Date.Between(j.StartDate.AddDays(1), j.StartDate.AddDays(10)))
                .RuleFor(j => j.ContactPerson, f => f.Person.FullName)
                .RuleFor(j => j.Email, (f, j) => f.Internet.Email(j.ContactPerson))
                .RuleFor(j => j.ContactNo, f => f.Phone.PhoneNumber())
                .RuleFor(j => j.ApprovalStatus, JobPost.EnumApprovalStatus.Pending)
                .RuleFor(j => j.Approved, false)
                .RuleFor(j => j.RecruiterType, f => f.PickRandom<JobPost.EnumRecruiterType>())
                .RuleFor(j => j.Faculty, f => f.PickRandom<JobPost.EnumFaculty>())
                .RuleFor(j => j.Department, f => f.Random.Word())
                .RuleFor(j => j.PartTimeNumberOfHours, (f, j) =>
                {
                    if (j.JobType == JobPost.EnumJobType.PartTime)
                    {
                        return f.PickRandom<JobPost.EnumWeekHours>();
                    }
                    else
                    {
                        return JobPost.EnumWeekHours.None;
                    }
                });

            foreach (var recruiter in recruiters)
            {
                var jobPostsExist = dbContext.JobPost.Any(j => j.RecruiterId == recruiter.RecruiterId);

                if (!jobPostsExist)
                {
                    var jobPosts = jobPostFaker.Generate(10); // Generate 10 fake JobPost instances

                    foreach (var jobPost in jobPosts)
                    {
                        jobPost.RecruiterId = recruiter.RecruiterId;
                        jobPost.YearsOfStudy = new YearsOfStudy
                        {
                            IsFirstYear = faker.Random.Bool(),
                            IsSecondYear = faker.Random.Bool(),
                            IsThirdYear = faker.Random.Bool(),
                            IsHonours = faker.Random.Bool(),
                            IsGraduates = faker.Random.Bool(),
                            IsMasters = faker.Random.Bool(),
                            IsPhD = faker.Random.Bool(),
                            IsPostdoc = faker.Random.Bool()
                        };
                    }

                    await dbContext.JobPost.AddRangeAsync(jobPosts);
                }
            }

            await dbContext.SaveChangesAsync();
        }


    }
}
