using Institute.Models;
using Institute.ViewModels;
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

        [HttpGet]
        public IActionResult Add()
        {
            var addInstructorVM = new InstructorWithDepartmentsAndCoursesVM()
            {
                Departments = context.Departments.ToList(),
                Courses = context.Courses.ToList()
            };
            return View("Add", addInstructorVM);
        }

        [HttpPost]
        public IActionResult Add(InstructorWithDepartmentsAndCoursesVM instructorVM)
        {
            if (!ModelState.IsValid)
            {
                instructorVM.Departments = context.Departments.ToList();
                instructorVM.Courses = context.Courses.ToList();
                return View("Add", instructorVM);
            }

            var instructor = new Instructor()
            {
                Name = instructorVM.Name,
                ImageUrl = instructorVM.ImageUrl,
                Salary = instructorVM.Salary,
                Address = instructorVM.Address,
                DepartmentId = instructorVM.DepartmentId,
                CourseId = instructorVM.CourseId
            };

            context.Instructors.Add(instructor);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var instructor = context.Instructors.FirstOrDefault(ins => ins.Id == id);

            var editInstructorVM = new InstructorWithDepartmentsAndCoursesVM()
            {
                Id = instructor.Id,
                Name = instructor.Name,
                ImageUrl = instructor.ImageUrl,
                Salary = instructor.Salary,
                Address = instructor.Address,
                DepartmentId = instructor.DepartmentId ?? 0,
                CourseId = instructor.CourseId ?? 0,
                Departments = context.Departments.ToList(),
                Courses = context.Courses.ToList()
            };

            return View("Edit", editInstructorVM);
        }

        [HttpPost]
        public IActionResult Edit(InstructorWithDepartmentsAndCoursesVM instructorVM)
        {
            if (!ModelState.IsValid)
            {
                instructorVM.Departments = context.Departments.ToList();
                instructorVM.Courses = context.Courses.ToList();
                return View("Edit", instructorVM);
            }

            var instructor = context.Instructors.FirstOrDefault(ins => ins.Id == instructorVM.Id);

            instructor.Name = instructorVM.Name;
            instructor.ImageUrl = instructorVM.ImageUrl;
            instructor.Salary = instructorVM.Salary;
            instructor.Address = instructorVM.Address;
            instructor.DepartmentId = instructorVM.DepartmentId;
            instructor.CourseId = instructorVM.CourseId;

            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
