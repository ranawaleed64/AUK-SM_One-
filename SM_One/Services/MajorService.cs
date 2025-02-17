using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM_One.Models;
using SM_One.Repositories;

namespace SM_One.Services
{
    public class MajorService
    {
        private readonly IMajorRepository _MajorRepository;
        public MajorService(IMajorRepository MajorRepository)
        {
            _MajorRepository = MajorRepository;
        }
        public IEnumerable<Majors> GetAllMajors()
        {
            return _MajorRepository.GetAllMajors();
        }
    }
}
