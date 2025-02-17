using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM_One.Models
{
    public class Semesters
    {
        public int ID { get; set; }
        public int Sequence { get; set; }
        public string SemesterType { get; set; }
        public DateTime SemesterStartDate { get; set; }
        public DateTime SemesterEndDate { get; set; }
        public string DescriptionEn { get; set; }
    }
}
