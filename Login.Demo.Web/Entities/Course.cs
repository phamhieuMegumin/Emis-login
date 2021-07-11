using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Demo.Web.Entities
{
    public class Course
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public Guid AccountId { get; set; }
    }
}
