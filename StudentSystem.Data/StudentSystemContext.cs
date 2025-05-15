using Microsoft.EntityFrameworkCore;
using ToQuyenBTT8.Models;
using StudentSystem.Models.models;

namespace ToQuyenBTT8.Data
{
    public class ToQuyenBTT8Context : DbContext
    {
        public ToQuyenBTT8Context(DbContextOptions<ToQuyenBTT8Context> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);

            modelBuilder.Entity<Resource>()
                .HasOne(r => r.Course)
                .WithMany(c => c.Resources)
                .HasForeignKey(r => r.CourseId);

            modelBuilder.Entity<Homework>()
                .HasOne(h => h.Student)
                .WithMany(s => s.Homeworks)
                .HasForeignKey(h => h.StudentId);

            modelBuilder.Entity<Homework>()
                .HasOne(h => h.Course)
                .WithMany(c => c.Homeworks)
                .HasForeignKey(h => h.CourseId);

            // Seed data
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, Name = "Nguyen Van A", RegistrationDate = new DateTime(2025, 1, 1) },
                new Student { Id = 2, Name = "Tran Thi B", RegistrationDate = new DateTime(2025, 1, 1) }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "C# Basics", StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 2, 1), Price = 1000 },
                new Course { Id = 2, Name = "Web Development", StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 3, 1), Price = 1500 }
            );

            modelBuilder.Entity<StudentCourse>().HasData(
                new { StudentId = 1, CourseId = 1 },
                new { StudentId = 2, CourseId = 1 },
                new { StudentId = 2, CourseId = 2 }
            );

            modelBuilder.Entity<Resource>().HasData(
                new Resource { Id = 1, Name = "C# Intro", Type = ResourceType.Video, Url = "http://example.com", CourseId = 1 },
                new Resource { Id = 2, Name = "Slides", Type = ResourceType.Presentation, Url = "http://example.com", CourseId = 1 },
                new Resource { Id = 3, Name = "Web Basics", Type = ResourceType.Document, Url = "http://example.com", CourseId = 2 }
            );

            modelBuilder.Entity<Homework>().HasData(
                new Homework { Id = 1, Content = "Assignment 1", ContentType = ContentType.Pdf, SubmissionDate = DateTime.Now, StudentId = 1, CourseId = 1 },
                new Homework { Id = 2, Content = "Assignment 2", ContentType = ContentType.Zip, SubmissionDate = DateTime.Now, StudentId = 2, CourseId = 1 }
            );
        }
    }
}