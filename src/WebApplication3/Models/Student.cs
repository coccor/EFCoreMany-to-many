using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SchoolId { get; set; }

        public virtual School School { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
