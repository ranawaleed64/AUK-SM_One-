using SM_One.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace SM_One.Repositories
{
    public class ScholarshipRepository : IScholarshipRepository
    {
        private readonly DatabaseConfig _databaseConfig;
        public ScholarshipRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }
        public IEnumerable<Scholarships> GetAllScholarships()
        {
            using (var connection = _databaseConfig.GetConnection())
            {
                connection.Open();
                string sql = "Select ID,Code as Code,DescriptionEn,MinCGPA from EdScholarshipTypes";
                return connection.Query<Scholarships>(sql);
            }
        }
    }
}
