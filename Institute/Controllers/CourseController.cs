using Institute.Models;
using Institute.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Institute.Controllers
{
    public class CourseController : Controller
    {
        private readonly InstituteContext context = new();
        public IActionResult ShowCourseResult(int id)
        {
            var courseResults = context.CourseResults
                .Where(crs => crs.CourseId == id)
                .Select
                (
                    crsresult => new StudentWithCourseDegreeVM()
                    {
                        TraineeName = crsresult.Trainee.Name,
                        Degree = crsresult.Degree,
                        IsPassed = crsresult.Degree >= crsresult.Course.MinDegree
                    }
                ).ToList();

            if (courseResults.Count == 0)
            {
                return NotFound();
            }

            var courseResultsVM = new CourseResultsVM()
            {
                CourseName = context.Courses.Find(id).Name,
                CourseResults = courseResults
            };

            return View("ShowCourseResult", courseResultsVM);
        }
    }
}
