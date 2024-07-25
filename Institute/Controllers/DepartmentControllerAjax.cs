using Microsoft.AspNetCore.Mvc;

namespace Institute.Controllers
{
    public partial class DepartmentController : Controller
    {
        public IActionResult GetCoursesForInput(int id)
        {
            var courses = context.Courses.Where(c => c.DepartmentId == id)
                .Select(crs => new {id = crs.Id, name = crs.Name}) // Return only the id and name of the course.
                .ToList();

            return Json(courses);
        }
    }
}
