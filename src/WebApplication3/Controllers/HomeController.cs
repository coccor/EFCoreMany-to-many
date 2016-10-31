using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using WebApplication3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Create()
        {
            School school = new School { Name = "Great School", Description = "sdadasd", Students = new List<Student>() };
            Student student = new Student { Name = "Jack", School = school, StudentCourses = new List<StudentCourse>() };

            Course course1 = new Course { Name = "Course 1" };
            Course course2 = new Course { Name = "Course 2" };
            Course course3 = new Course { Name = "Course 3" };

            StudentCourse sc1 = new StudentCourse { Student = student, Course = course1 };
            StudentCourse sc2 = new StudentCourse { Student = student, Course = course2 };
            StudentCourse sc3 = new StudentCourse { Student = student, Course = course3 };

            student.StudentCourses.Add(sc1);
            student.StudentCourses.Add(sc2);
            student.StudentCourses.Add(sc3);

            school.Students.Add(student);

            _context.Add(school);

            _context.SaveChanges();

            return View("Index");

        }

        public IActionResult Read()
        {
            var schools = _context.Schools.
                Include(s => s.Students)
                    .ThenInclude(s => s.StudentCourses)
                        .ThenInclude(s => s.Course);

            _logger.LogInformation($"\n******************************************\n{Flatten(schools)}\n******************************************\n");

            return View("Index");
        }

        //TODO: Use StringBuilder
        string Flatten(IEnumerable<School> schools)
        {
            string s = "";
            foreach (var school in schools)
            {
                s += "School: " + school.Name + "\nStudents:\n";
                foreach (var item in school.Students)
                {
                    s += $"\tName: {item.Name}\nCourses:\n";
                    foreach (var sc in item.StudentCourses)
                    {
                        s += $"\t\t{sc.Course.Name}\n";
                    }
                }
            }
            return s;
        }

        //TODO: update from the school downward
        public IActionResult Update()
        {
            // update student, courses and studentCourses all at once
            var student = _context.Students
                .Include(s => s.StudentCourses)
                    .ThenInclude(s => s.Course)
                .FirstOrDefault(s => s.Name.Equals("Jack"));

            //edit student name
            student.Name = "Jack Sparrow";

            //remove first and third
            var first = student.StudentCourses.FirstOrDefault(sc => sc.Course.Name.Contains("1"));
            var third = student.StudentCourses.FirstOrDefault(sc => sc.Course.Name.Contains("3"));
            student.StudentCourses.Remove(first);
            student.StudentCourses.Remove(third);

            //edit course (append 'x' to the current course name)
            student.StudentCourses.ToList().ForEach(sc => sc.Course.Name = sc.Course.Name + "x");

            _context.Students.Update(student);
            _context.SaveChanges();

            return View("Index");
        }

        public IActionResult Delete()
        {
            //By removing the schools we also remove the students and the record needed to join students with courses
            _context.RemoveRange(_context.Schools);
            //The courses aren't removed with the schools
            _context.RemoveRange(_context.Courses);
            _context.SaveChanges();
            return View("Index");
        }
    }
}
