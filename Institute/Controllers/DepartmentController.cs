using Institute.Models;
using Institute.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Institute.Controllers
{
    public partial class DepartmentController : Controller
    {
        private readonly InstituteContext context = new();
        public IActionResult Index(int page = 1)
        {
            int pageSize = 5;
            var departmentVM = new PaginationVM<Department>()
            {
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)context.Departments.Count() / pageSize),
                Items = context.Departments.Skip((page - 1) * pageSize).Take(pageSize).ToList()
            };

            return View("Index", departmentVM);
        }

        [HttpPost]
        public IActionResult Search(string searchString, string propertyToSearch)
        {
            List<Department> filteredDepartments;
            SearchVM<Department> departmentSearchVM;

            switch (propertyToSearch)
            {
                case "manager":
                    filteredDepartments = context.Departments.Where(dept => dept.Manager.Contains(searchString)).ToList();
                    break;
                default: // Otherwise, search by name.
                    filteredDepartments = context.Departments.Where(dept => dept.Name.Contains(searchString)).ToList();
                    break;
            }

            departmentSearchVM = new SearchVM<Department>()
            {
                SearchString = searchString,
                SearchProperty = propertyToSearch,
                Items = filteredDepartments
            };

            return View("Search", departmentSearchVM);
        }

        public IActionResult Details(int id)
        {
            var departmentVM = context.Departments.Where(tr => tr.Id == id)
                .Select(dept => new DepartmentDetailsVM
                {
                    DepartmentId = dept.Id,
                    DepartmentName = dept.Name,
                    DepartmentManager = dept.Manager,
                    Instructors = dept.Instructors.ToList(),
                    Courses = dept.Courses.ToList()
                }
                ).FirstOrDefault();

            return View("Details", departmentVM);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var addDepartmentVM = new DepartmenModifytVM();

            return View("Add", addDepartmentVM);
        }

        [HttpPost]
        public IActionResult Add(DepartmenModifytVM departmentVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", departmentVM);
            }

            var department = new Department()
            {
                Name = departmentVM.DepartmentName,
                Manager = departmentVM.DepartmentManager
            };

            context.Departments.Add(department);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var department = context.Departments.Find(id);

            var editDepartmentVM = new DepartmenModifytVM()
            {
                DepartmentId = department.Id,
                DepartmentName = department.Name,
                DepartmentManager = department.Manager
            };

            return View("Edit", editDepartmentVM);
        }

        [HttpPost]
        public IActionResult Edit(DepartmenModifytVM editDepartmentVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", editDepartmentVM);
            }

            var department = context.Departments.Find(editDepartmentVM.DepartmentId);

            department.Name = editDepartmentVM.DepartmentName;
            department.Manager = editDepartmentVM.DepartmentManager;

            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var department = context.Departments.Find(id);
                if (department == null)
                {
                    return StatusCode(404, new { message = "Department not found." });
                }
                context.Departments.Remove(department);
                context.SaveChanges();
                return StatusCode(201, new { message = "Department successfully removed." });
            }
            catch
            {
                return StatusCode(500, $"An error occured while removing the Department.");
            }
        }
    }
}
