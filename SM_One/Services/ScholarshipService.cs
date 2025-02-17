using SM_One.Models;
using SM_One.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM_One.Services
{
    class ScholarshipService
    {
        private readonly IScholarshipRepository _scholarshipRepository;

        public ScholarshipService(IScholarshipRepository scholarshipRepository)
        {
            _scholarshipRepository = scholarshipRepository;
        }
        public IEnumerable<Scholarships> GetAllScholarships()
        {
            return _scholarshipRepository.GetAllScholarships();
        }
    }
}
