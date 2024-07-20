using Institute.Models;
using Microsoft.AspNetCore.Mvc;

namespace Institute.Controllers
{
    public class InstructorController : Controller
    {
        private readonly InstituteContext context = new();

        public IActionResult Index()
        {
            var allInstructors = context.Instructors.ToList();

            return View("Index", allInstructors);
        }

        public IActionResult Details(int id)
        {
            var instructor = context.Instructors.FirstOrDefault(ins => ins.Id == id);

            return View("Details", instructor);
        }
    }
}
