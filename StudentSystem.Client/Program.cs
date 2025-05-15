using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ToQuyenBTT8.Data;
using ToQuyenBTT8.Models;

namespace ToQuyenBTT8.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // Cấu hình đọc file appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            // Cấu hình DbContext với SQLite
            var optionsBuilder = new DbContextOptionsBuilder<ToQuyenBTT8Context>();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

            using var context = new ToQuyenBTT8Context(optionsBuilder.Options);

            // Task 1: List all students and their homework submissions
            Console.WriteLine("Task 1: Students and their homework submissions");
            var students = context.Students
                .Include(s => s.Homeworks)
                .ToList();
            foreach (var student in students)
            {
                Console.WriteLine($"Student: {student.Name}");
                foreach (var homework in student.Homeworks)
                {
                    Console.WriteLine($"-- Homework: {homework.Content}, Type: {homework.ContentType}");
                }
            }

            // Task 2: List all courses with their resources
            Console.WriteLine("\nTask 2: Courses with their resources");
            var courses = context.Courses
                .Include(c => c.Resources)
                .OrderBy(c => c.StartDate)
                .ThenByDescending(c => c.EndDate)
                .ToList();
            foreach (var course in courses)
            {
                Console.WriteLine($"Course: {course.Name}, Description: {course.Description ?? "N/A"}");
                foreach (var resource in course.Resources)
                {
                    Console.WriteLine($"-- Resource: {resource.Name}, Type: {resource.Type}, URL: {resource.Url}");
                }
            }

            // Task 3: List courses with more than 5 resources
            Console.WriteLine("\nTask 3: Courses with more than 5 resources");
            var coursesWithManyResources = context.Courses
                .Include(c => c.Resources)
                .Where(c => c.Resources.Count > 5)
                .OrderByDescending(c => c.Resources.Count)
                .ThenByDescending(c => c.StartDate)
                .Select(c => new { c.Name, ResourceCount = c.Resources.Count })
                .ToList();
            foreach (var course in coursesWithManyResources)
            {
                Console.WriteLine($"Course: {course.Name}, Resources: {course.ResourceCount}");
            }

            // Task 4: List active courses on a given date
            Console.WriteLine("\nTask 4: Active courses on 2025-05-09");
            var givenDate = new DateTime(2025, 5, 9);
            var activeCourses = context.Courses
                .Include(c => c.StudentCourses)
                .Where(c => c.StartDate <= givenDate && c.EndDate >= givenDate)
                .Select(c => new
                {
                    c.Name,
                    c.StartDate,
                    c.EndDate,
                    Duration = (c.EndDate - c.StartDate).Days,
                    StudentCount = c.StudentCourses.Count
                })
                .OrderByDescending(c => c.StudentCount)
                .ThenByDescending(c => c.Duration)
                .ToList();
            foreach (var course in activeCourses)
            {
                Console.WriteLine($"Course: {course.Name}, Start: {course.StartDate}, End: {course.EndDate}, Duration: {course.Duration} days, Students: {course.StudentCount}");
            }

            // Task 5: Calculate student course stats
            Console.WriteLine("\nTask 5: Student course statistics");
            var studentStats = context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .Select(s => new
                {
                    s.Name,
                    CourseCount = s.StudentCourses.Count,
                    TotalPrice = s.StudentCourses.Sum(sc => sc.Course.Price),
                    AveragePrice = s.StudentCourses.Any() ? s.StudentCourses.Average(sc => sc.Course.Price) : 0
                })
                .OrderByDescending(s => s.TotalPrice)
                .ThenByDescending(s => s.CourseCount)
                .ThenBy(s => s.Name)
                .ToList();
            foreach (var stat in studentStats)
            {
                Console.WriteLine($"Student: {stat.Name}, Courses: {stat.CourseCount}, Total Price: {stat.TotalPrice}, Avg Price: {stat.AveragePrice}");
            }
        }
    }
}