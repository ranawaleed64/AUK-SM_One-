using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM_One.Models;
using Dapper;

namespace SM_One.Repositories
{
    public class CollegeRepository : ICollegeRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public CollegeRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public IEnumerable<Colleges> GetAllColleges()
        {
            using (var connection = _databaseConfig.GetConnection())
            {
                connection.Open();
                string sql = "SELECT ID,Code as 'Code', DescriptionEn FROM EdColleges";
                return connection.Query<Colleges>(sql);
            }
        }
    }
}
