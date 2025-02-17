using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM_One.Models
{
   public class StudentCourses
    {
        public string CourseCode { get; set; }
        public string CourseDescriptionEn { get; set; }
        public int Hours { get; set; }
        public int CurrentScholarshipID { get; set; }
        public string CurrentScholarshipCode { get; set; }
        public string CurrentScholarshipDesc { get; set; }
        public string Repeat { get; set; }
        public string RepeatCourse { get; set; }
    }
}
