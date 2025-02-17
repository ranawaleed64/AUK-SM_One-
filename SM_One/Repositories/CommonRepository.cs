using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM_One.Models;

namespace SM_One.Repositories
{
    public interface IStudentRepository
    {
        IEnumerable<StudentInfo> GetStudentsBySemester(string SemesterID,string CollegeID = null, string MajorID = null);
        IEnumerable<StudentCourses> GetStudentCourses(string SemesterID, string StudentCode,string College,string Major);

    }
    public interface ICollegeRepository
    {
        IEnumerable<Colleges> GetAllColleges();
    }

    public interface ISemesterRepository
    {
        IEnumerable<Semesters> GetAllSemesters();
    }
    public interface IMajorRepository
    {
        IEnumerable<Majors> GetAllMajors();
    }
    public interface IScholarshipRepository
    {
        IEnumerable<Scholarships> GetAllScholarships();
    }

    public interface ICoursesRepository
    {
        IEnumerable<Courses> GetAllCourses();
    }
}
