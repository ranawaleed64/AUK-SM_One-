using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM_One.Models
{
    public class Courses
    {
        public int ID { get; set; }
        public string CourseCode { get; set;  }
        public string DescriptionEn { get; set; }
        public double PassMark{ get; set; }
        public int CreditHours { get; set; }
        public string Scholarship { get; set; }
    }
}
