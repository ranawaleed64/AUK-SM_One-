using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM_One.Models;
using Dapper;

namespace SM_One.Repositories
{
    public class MajorRepository : IMajorRepository
    {
        private readonly DatabaseConfig _databaseConfig;
        public MajorRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }
        public IEnumerable<Majors> GetAllMajors()
        {
            using (var connection = _databaseConfig.GetConnection())
            {
                connection.Open();
                string sql = "SELECT ID, LEFT(Code,8) as \"Code\", \"DescriptionEn\" FROM EdMajors";
                return connection.Query<Majors>(sql);
            }
        }
    }
}
