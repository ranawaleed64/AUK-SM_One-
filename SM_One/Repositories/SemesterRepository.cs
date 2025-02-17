using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM_One.Models;
using Dapper;

namespace SM_One.Repositories
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public SemesterRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public IEnumerable<Semesters> GetAllSemesters()
        {
            using (var connection = _databaseConfig.GetConnection())
            {
                connection.Open();
                string sql = "SELECT ID, DescriptionEn, FORMAT(SemesterStartDate,'yyyy-MM-dd 00:00:00.000') as [SemesterStartDate], FORMAT(SemesterEndDate,'yyyy-MM-dd 00:00:00.000')  as [SemesterEndDate],Sequence,SemesterTypeID as [SemesterType] FROM EdSemesters order by Sequence";
                return connection.Query<Semesters>(sql);
            }
        }
    }
}
