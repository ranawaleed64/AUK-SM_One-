using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM_One.Repositories;
using SM_One.Models;

namespace SM_One.Services
{
    class StudentInfoService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentInfoService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public IEnumerable<StudentInfo> GetStudentInfos(string SemesterID, string CollegeID = null, string MajorID = null)
        {
            return _studentRepository.GetStudentsBySemester(SemesterID,CollegeID,MajorID);
        }
        public IEnumerable<StudentCourses> GetStudentCourses(string SemesterID, string StudentCode,string College,string Major)
        {
            return _studentRepository.GetStudentCourses(SemesterID,StudentCode, College,Major);
        }
    }
}
