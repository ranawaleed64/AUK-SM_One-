using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM_One.Models
{
   public class StudentInfo
    {
        public int RowNum { get; set; }
        public string Select { get; set; }
        public int StudentID { get; set; }
        public string StudentCode { get; set; }
        public string StudentNameEn { get; set; }
        public string StudentStatusID { get; set; }
        public DateTime StudentDate { get; set; }
        public DateTime SemesterStartDate { get; set; }
        public DateTime SemesterEndDate { get; set; }
        public string StudentGroupID { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Email { get; set; }
        public int CollegeID { get; set; }
        public string CollegeCode { get; set; }
        public string CollegeDescriptionEn { get; set; }
        public int SemesterID { get; set; }
        public string SemesterDescriptionEn { get; set; }
        public string AcademicYearDescriptionEn { get; set; }
        public string SemesterStartMonth { get; set; }
        public string SemesterEndMonth { get; set; }
        public int MajorID { get; set; }
        public string MajorCode { get; set; }
        public string MajorDescriptionEn { get; set; }
        public int AdmissionScholarshipID { get; set; }
        public string AdmissionScholarshipCode { get; set; }
        public string AdmissionScholarshipDesc { get; set; }
        public int CurrentScholarshipID { get; set; }
        public string CurrentScholarshipCode { get; set; }
        public string CurrentScholarshipDesc { get; set; }
        public double CGPA { get; set; }
        public double TotalScholarshipHours { get; set; }
        public double AttemptedCredits { get; set; }
    }
}
