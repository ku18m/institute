using Institute.Models;
using Institute.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Institute.Controllers
{
    public class InstructorController : Controller
    {
        private readonly InstituteContext context = new();
        private readonly IWebHostEnvironment webHostEnv;

        public InstructorController(IWebHostEnvironment webHostEnvironment)
        {
            webHostEnv = webHostEnvironment;
        }

        public IActionResult Index(int page = 1)
        {
            int pageSize = 5;
            var instructorVM = new InstructorPaginationVM()
            {
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)context.Instructors.Count() / pageSize),
                Instructors = context.Instructors.Skip((page - 1) * pageSize).Take(pageSize).ToList()
            };

            return View("IndexPagination", instructorVM);
        }

        [HttpPost]
        public IActionResult Search(string searchString, string propertyToSearch)
        {
            List<Instructor> filteredInstructors;
            InstructorSearchVM instructorSearchVM;

            switch (propertyToSearch)
            {
                case "address":
                    filteredInstructors = context.Instructors.Where(ins => ins.Address.Contains(searchString)).ToList();
                    break;
                case "salary":
                    decimal salary;
                    Decimal.TryParse(searchString, out salary);
                    filteredInstructors = context.Instructors.Where(ins => ins.Salary == salary).ToList();
                    break;
                default: // Otherwise, search by name.
                    filteredInstructors = context.Instructors.Where(ins => ins.Name.Contains(searchString)).ToList();
                    break;
            }

            instructorSearchVM = new InstructorSearchVM()
            {
                SearchString = searchString,
                SearchProperty = propertyToSearch,
                Instructors = filteredInstructors
            };

            return View("Search", instructorSearchVM);
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
        public async Task<IActionResult> Add(InstructorWithDepartmentsAndCoursesVM instructorVM, IFormFile imgFile)
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
                Salary = instructorVM.Salary,
                Address = instructorVM.Address,
                DepartmentId = instructorVM.DepartmentId,
                CourseId = instructorVM.CourseId,
                ImageUrl = await UploadImage(imgFile) // Upload the img and get its path, if error ocurred returns Empty string.
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
        public async Task<IActionResult> Edit(InstructorWithDepartmentsAndCoursesVM editInstructorVM, IFormFile imgFile)
        {
            if (!ModelState.IsValid)
            {
                editInstructorVM.Departments = context.Departments.ToList();
                editInstructorVM.Courses = context.Courses.ToList();
                return View("Edit", editInstructorVM);
            }


            var instructor = context.Instructors.FirstOrDefault(ins => ins.Id == editInstructorVM.Id);

            if (imgFile != null) // If the user uploaded a new image, remove the old one.
            {
                RemoveImage(instructor.ImageUrl); 
            }

            instructor.Name = editInstructorVM.Name;
            instructor.Salary = editInstructorVM.Salary;
            instructor.Address = editInstructorVM.Address;
            instructor.DepartmentId = editInstructorVM.DepartmentId;
            instructor.CourseId = editInstructorVM.CourseId;
            instructor.ImageUrl = await UploadImage(imgFile); // Upload the img and get its path, if error ocurred returns Empty string.

            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var instructor = context.Instructors.FirstOrDefault(ins => ins.Id == id);
                if (instructor == null)
                {
                    return StatusCode(404, new {message = "Instructor not found."});
                }
                RemoveImage(instructor.ImageUrl);
                context.Instructors.Remove(instructor);
                context.SaveChanges();
                return StatusCode(201, new {message = "Instructor successfully removed."});
            }
            catch
            {
                return StatusCode(500, $"An error occured while removing the instructor.");
            }

        }

        private async Task<string> UploadImage(IFormFile imgFile)
        {
            string imgPath;

            if (imgFile != null)
            {
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var fileName = timestamp + "-" + imgFile.FileName;
                imgPath = Path.Combine(webHostEnv.WebRootPath, "Images", fileName);

                try
                {
                    using (var fileStream = new FileStream(imgPath, FileMode.Create))
                    {
                        await imgFile.CopyToAsync(fileStream);
                    }
                    return $"/Images/{fileName}";
                }
                catch
                {
                    return "";
                }
            }
            return "";
        }

        private bool RemoveImage(string imgPath)
        {
            bool status = false;

            if (imgPath != null)
            {
                try
                {
                    var fullPath = Path.Combine(webHostEnv.WebRootPath, "Images", imgPath.Split("/")[^1]);
                    System.IO.File.Delete(fullPath);
                    status = true;
                }
                catch
                {
                    status = false;
                }
            }

            return status;
        }

    }
}
