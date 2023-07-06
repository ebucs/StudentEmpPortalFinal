using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEmploymentPortal.Areas.Identity;
using StudentEmploymentPortal.Areas.jobpostA.Models;
using StudentEmploymentPortal.Areas.recruiterj.Models;
using StudentEmploymentPortal.Areas.studentj.Models;
using StudentEmploymentPortal.Data;
using System;

namespace StudentEmploymentPortal.DataSeeding
{
    public class Fakers
    {
        private readonly ApplicationDbContext _context;

        public Fakers(ApplicationDbContext context)
        {
            _context = context;
        }

        private static async Task SeedJobPostsAsync(ApplicationDbContext dbContext)
        {
            if (!dbContext.JobPost.Any())
            {
                var faker = new Faker<JobPost>()
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
                    .RuleFor(j => j.Approved, f => f.Random.Bool())
                    .RuleFor(j => j.ApproversNote, f => f.Random.Bool() ? f.Lorem.Sentence() : null)
                    .RuleFor(j => j.RecruiterType, f => f.PickRandom<JobPost.EnumRecruiterType>())
                    .RuleFor(j => j.Faculty, f => f.PickRandom<JobPost.EnumFaculty>())
                    .RuleFor(j => j.Department, f => f.Random.Word());

                var jobPosts = faker.Generate(10); // Generate 10 fake JobPost instances

                await dbContext.JobPost.AddRangeAsync(jobPosts);
                await dbContext.SaveChangesAsync();
            }
        }

    }
}
