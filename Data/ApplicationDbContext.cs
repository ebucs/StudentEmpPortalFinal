﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentEmploymentPortal.Areas.Identity;
using StudentEmploymentPortal.Areas.recruiterj.Models;
using StudentEmploymentPortal.Areas.studentj.Models;
using StudentEmploymentPortal.ViewModels;

namespace StudentEmploymentPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Recruiter> Recruiter { get; set; }
        public DbSet<Student> Student { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure enums as strings
             modelBuilder.Entity<Student>()
                .Property(s => s.Race)
                .HasConversion<string>();

            modelBuilder.Entity<Student>()
                .Property(s => s.Gender)
                .HasConversion<string>();

            modelBuilder.Entity<Student>()
                .Property(s => s.Nationality)
                .HasConversion<string>();

            modelBuilder.Entity<Student>()
                .Property(s => s.CurrentYearOfStudy)
                .HasConversion<string>();

            modelBuilder.Entity<Student>()
                .Property(s => s.Faculty)
                .HasConversion<string>();

            modelBuilder.Entity<Student>()
                .Property(s => s.Department)
                .HasConversion<string>();

            modelBuilder.Entity<Student>()
                .Property(s => s.DriversLicense)
                .HasConversion<string>();


            // Add additional enums here...
        }
    }
}
