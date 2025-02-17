using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM_One.Models;
using SM_One.Repositories;

namespace SM_One.Services
{
    public class SemesterService
    {
        private readonly ISemesterRepository _semesterRepository;

        public SemesterService(ISemesterRepository semesterRepository)
        {
            _semesterRepository = semesterRepository;
        }
        public IEnumerable<Semesters> GetAllSemesters()
        {
            return _semesterRepository.GetAllSemesters();
        }
    }
}
