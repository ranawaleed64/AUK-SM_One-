using SM_One.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;   

namespace SM_One.Repositories
{
    public class CourseRepository : ICoursesRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public CourseRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }
        public IEnumerable<Courses> GetAllCourses()
        {
            using (var connection = _databaseConfig.GetConnection())
            {
                connection.Open();
                string sql = "SELECT ID, CourseCode, DescriptionEn,CreditHours,PassMark, CASE WHEN ScholarShipDiscount = 1 THEN 'Y' ELSE 'N' END AS Scholarship FROM EdCourses";
                return connection.Query<Courses>(sql);
            }
        }
    }
}
