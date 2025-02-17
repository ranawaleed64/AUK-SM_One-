using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM_One.Models;
using SM_One.Repositories;

namespace SM_One.Services
{
    public class CollegeService
    {
        private readonly ICollegeRepository _collegeRepository;

        public CollegeService(ICollegeRepository collegeRepository)
        {
            _collegeRepository = collegeRepository;
        }
        public IEnumerable<Colleges> GetAllColleges()
        {
            return _collegeRepository.GetAllColleges();
        }
    }
}
