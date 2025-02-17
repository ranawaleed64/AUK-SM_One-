using SM_One.Models;
using SM_One.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM_One.Services
{
    public class CourseService
    {
        private readonly ICoursesRepository _coursesRepository;

        public CourseService(ICoursesRepository courseRepository)
        {
            _coursesRepository = courseRepository;
        }
        public IEnumerable<Courses> GetAllCourses()
        {
            return _coursesRepository.GetAllCourses();
        }
    }
}
