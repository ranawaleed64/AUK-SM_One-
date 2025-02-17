using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM_One.Models;
using Dapper;

namespace SM_One.Repositories
{
    class StudentRepository : IStudentRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public StudentRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }
        public IEnumerable<StudentInfo> GetStudentsBySemester(string SemesterID,string CollegeID = null,string MajorID = null)
        {
            using (var connection = _databaseConfig.GetConnection())
            {
                connection.Open();
                string sql = @"
                SELECT
                ROW_NUMBER() over (order by a.StudentID) as [RowNum],
                'Y' as [Select],
                a.StudentID,
                a.StudentCode, 
                a.StudentNameEn, 
                e.StudentStatusID, 
                e.StudentDate, 
                e.StudentGroupID, 
                e.TotalScholarshipHours,
                e.AttemptedCredits,
                e.CGPA,
                ISNULL(e.Telephone1,'') as Telephone1,
                ISNULL(e.Telephone2,'') as Telephone2, 
                e.Email,
                a.CollegeID, 
                LEFT(b.Code,8) as CollegeCode, 
                a.CollegeDescriptionEn,
                a.SemesterID, 
                a.SemesterDescriptionEn, 
                a.AcademicYearDescriptionEn,
                DATENAME(MM, c.SemesterStartDate) AS 'SemesterStartMonth', 
                DATENAME(MM, c.SemesterEndDate) AS 'SemesterEndMonth',
                a.MajorID, 
                LEFT(d.Code,8) as MajorCode, 
                a.MajorDescriptionEn, 
                e.ScholarshipAdmissionTypeID AS 'AdmissionScholarshipID', 
                y.Code AS 'AdmissionScholarshipCode', 
                y.DescriptionEn AS 'AdmissionScholarshipDesc',
                e.ScholarshipTypeID AS 'CurrentScholarshipID', 
                z.Code AS 'CurrentScholarshipCode', 
                z.DescriptionEn AS 'CurrentScholarshipDesc',
                c.SemesterStartDate as 'SemesterStartDate',
                c.SemesterEndDate as 'SemesterEndDate'

                FROM EdTimeTables_View a
                LEFT JOIN EdColleges b ON a.CollegeID = b.ID
                LEFT JOIN EdSemesters c ON a.SemesterID = c.ID
                LEFT JOIN EdMajors d ON a.MajorID = d.ID
                LEFT JOIN EdStudentsTable e ON a.StudentCode = e.Code
                INNER JOIN EdScholarshipTypes z ON e.ScholarshipTypeID = z.ID
                INNER JOIN EdScholarshipTypes y ON e.ScholarshipAdmissionTypeID = y.ID

                WHERE a.SemesterID = @Semester AND (@College IS NULL OR a.CollegeID = @College or @College = '') AND (@Major IS NULL OR a.MajorID = @Major or @Major = '') 

                GROUP BY a.StudentID,a.studentcode, a.StudentNameEn, e.StudentStatusID, e.StudentDate, e.StudentGroupID, e.Telephone1, e.Telephone2, e.Email,
                a.CollegeID, b.Code, a.CollegeDescriptionEn, e.TotalScholarshipHours, e.AttemptedCredits,e.CGPA,
                 a.SemesterID, a.SemesterDescriptionEn, a.AcademicYearDescriptionEn,
                DATENAME(MM, c.SemesterStartDate), DATENAME(MM, c.SemesterEndDate),
                a.MajorID, d.Code, a.MajorDescriptionEn, 
                e.ScholarshipAdmissionTypeID, y.Code, y.DescriptionEn,
                e.ScholarshipTypeID, z.Code, z.DescriptionEn,c.SemesterStartDate, c.SemesterEndDate;";
                    
                return connection.Query<StudentInfo>(sql, new { Semester = SemesterID, College = CollegeID,Major = MajorID });
            }
        }

        public IEnumerable<StudentCourses> GetStudentCourses(string SemesterID, string StudentCode,string College,string Major)
        {
            using (var connection = _databaseConfig.GetConnection())
            {
                connection.Open();
                string sql = @"
                SELECT 
                a.CourseCode,
                a.CourseDescriptionEn,
                a.Hours,
                z.ID as 'CurrentScholarshipID',
                z.Code as 'CurrentScholarshipCode',
                z.DescriptionEn as 'CurrentScholarshipDescription',
                coalesce(Zz.CourseCode,'') as 'RepeatCourse',
                CASE WHEN Zz.Notation = 'RP' THEN 'Y' ELSE 'N' END as 'Repeat'

                FROM EdTimeTables_View a

                LEFT JOIN EdColleges b ON a.CollegeID = b.ID
                LEFT JOIN EdSemesters c ON a.SemesterID = c.ID
                LEFT JOIN EdMajors d ON a.MajorID = d.ID
                LEFT JOIN EdStudentsTable e ON a.StudentCode = e.Code
                INNER JOIN EdScholarshipTypes z ON e.ScholarshipTypeID = z.ID
                INNER JOIN EdScholarshipTypes y ON e.ScholarshipAdmissionTypeID = y.ID
                INNER JOIN EdCourses x on x.ID = a.CourseID
                LEFT OUTER JOIN 
                (
	                SELECT DISTINCT StudentNumber, StudentFullNameEn, SemesterDescriptionEn, Notation, RepeatedCourse, CourseCode, CourseDescriptionEn
	                FROM EdStudentTimetableTemp
	                where 
	                Notation = 'RP'
                ) Zz ON a.studentcode = Zz.StudentNumber AND  a.SemesterDescriptionEn = Zz.SemesterDescriptionEn AND  a.CourseCode = Zz.CourseCode

                WHERE a.SemesterID = @Semester AND a.StudentCode = @StudentCode and a.CollegeID = @College and a.MajorID = @Major ;";

                return connection.Query<StudentCourses>(sql, new { Semester = SemesterID, StudentCode = StudentCode, College = College,Major = Major });
            }
        }
    }
}
