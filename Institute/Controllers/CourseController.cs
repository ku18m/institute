using Institute.Models;
using Institute.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Index(int page = 1)
        {
            int pageSize = 5;
            var coursesVM = new PaginationVM<Course>()
            {
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)context.Courses.Count() / pageSize),
                Items = context.Courses.Skip((page - 1) * pageSize).Take(pageSize).ToList()
            };

            return View("Index", coursesVM);
        }

        [HttpPost]
        public IActionResult Search(SearchVM<Course> searchVM)
        {
            List<Course> filteredCourses;
            SearchVM<Course> courseSearchVM;

            switch (searchVM.SearchProperty)
            {
                case "degree":
                    int degree;
                    int.TryParse(searchVM.SearchString, out degree);
                    filteredCourses = context.Courses.Where(crs => crs.Degree == degree).ToList();
                    break;
                case "minDegree":
                    int minDegree;
                    int.TryParse(searchVM.SearchString, out minDegree);
                    filteredCourses = context.Courses.Where(crs => crs.MinDegree == minDegree).ToList();
                    break;
                default: // Otherwise, search by name.
                    filteredCourses = context.Courses.Where(tr => tr.Name.Contains(searchVM.SearchString)).ToList();
                    break;
            }

            searchVM.Items = filteredCourses;

            return View("Search", searchVM);
        }

        public IActionResult Details(int id)
        {
            var courseVM = context.Courses.Where(crs => crs.Id == id)
                .Select(crs => new CourseDetailsVM
                {
                    CourseId = crs.Id,
                    CourseName = crs.Name,
                    CourseDegree = crs.Degree,
                    CourseMinDegree = crs.MinDegree,
                    DepartmentId = crs.DepartmentId,
                    DepartmentName = crs.Department.Name,
                    Instructors = crs.Instructors.ToList(),
                    CourseResults = context.CourseResults
                        .Where(crs => crs.CourseId == crs.Id)
                        .Select(crslt => new StudentWithCourseDegreeVM
                        {
                            TraineeName = crslt.Trainee.Name,
                            Degree = crslt.Degree,
                            IsPassed = crslt.Degree >= crslt.Course.MinDegree
                        }).ToList()
                }
                ).FirstOrDefault();

            return View("Details", courseVM);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var addCourseVM = new CourseWithDepartmentsVM()
            {
                Departments = context.Departments.Select(dept => new SelectListItem
                {
                    Value = dept.Id.ToString(),
                    Text = dept.Name
                }).ToList()
            };
            return View("Add", addCourseVM);
        }

        [HttpPost]
        public IActionResult Add(CourseWithDepartmentsVM courseVM)
        {
            if (!ModelState.IsValid)
            {
                courseVM.Departments = context.Departments.Select(dept => new SelectListItem
                {
                    Value = dept.Id.ToString(),
                    Text = dept.Name
                }).ToList();

                return View("Add", courseVM);
            }

            var course = new Course()
            {
                Name = courseVM.CourseName,
                Degree = courseVM.CourseDegree,
                MinDegree = courseVM.CourseMinDegree,
                DepartmentId = courseVM.DepartmentId,
            };

            context.Courses.Add(course);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = context.Courses.Find(id);

            var editCourseVM = new CourseWithDepartmentsVM()
            {
                CourseId = course.Id,
                CourseName = course.Name,
                CourseDegree = course.Degree,
                CourseMinDegree = course.MinDegree,
                DepartmentId = course.DepartmentId,
                Departments = context.Departments.Select(dept => new SelectListItem
                {
                    Value = dept.Id.ToString(),
                    Text = dept.Name
                }).ToList()
            };

            return View("Edit", editCourseVM);
        }

        [HttpPost]
        public IActionResult Edit(CourseWithDepartmentsVM editCourseVM)
        {
            if (!ModelState.IsValid)
            {
                editCourseVM.Departments = context.Departments.Select(dept => new SelectListItem
                {
                    Value = dept.Id.ToString(),
                    Text = dept.Name
                }).ToList();

                return View("Edit", editCourseVM);
            }

            var course = context.Courses.Find(editCourseVM.CourseId);

            course.Name = editCourseVM.CourseName;
            course.Degree = editCourseVM.CourseDegree;
            course.MinDegree = editCourseVM.CourseMinDegree;
            course.DepartmentId = editCourseVM.DepartmentId;

            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var course = context.Courses.Find(id);
                if (course == null)
                {
                    return StatusCode(404, new { message = "Course not found." });
                }
                context.Courses.Remove(course);
                context.SaveChanges();
                return StatusCode(201, new { message = "Course successfully removed." });
            }
            catch
            {
                return StatusCode(500, $"An error occured while removing the Course.");
            }

        }
    }
}
