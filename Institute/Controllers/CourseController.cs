using Institute.Models;
using Institute.Repository;
using Institute.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Institute.Controllers
{
    public class CourseController(InstituteContext context, ICourseRepo CourseRepo, IDepartmentRepo DepartmentRepo) : Controller
    {
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
                TotalPages = CourseRepo.GetTotalPages(pageSize),
                Items = CourseRepo.GetPage(page)
            };

            return View("Index", coursesVM);
        }

        [HttpPost]
        public IActionResult Search(SearchVM<Course> searchVM)
        {
            SearchVM<Course> courseSearchVM;

            searchVM.Items = CourseRepo.Search(searchVM.SearchString, searchVM.SearchString);

            return View("Search", searchVM);
        }

        public IActionResult Details(int id)
        {
            CourseDetailsVM courseVM = CourseRepo.BindGetCourseDetails(id);

            return View("Details", courseVM);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var addCourseVM = new CourseWithDepartmentsVM()
            {
                Departments = DepartmentRepo.GetAll().Select(dept => new SelectListItem
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
                courseVM.Departments = DepartmentRepo.GetAll().Select(dept => new SelectListItem
                {
                    Value = dept.Id.ToString(),
                    Text = dept.Name
                }).ToList();

                return View("Add", courseVM);
            }

            var course = new Course()
            {
                Name = courseVM.CourseName,
                Hours = courseVM.CourseHours,
                Degree = courseVM.CourseDegree,
                MinDegree = courseVM.CourseMinDegree,
                DepartmentId = courseVM.DepartmentId,
            };

            CourseRepo.Insert(course);

            CourseRepo.Save();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = CourseRepo.GetById(id);

            var editCourseVM = new CourseWithDepartmentsVM()
            {
                CourseId = course.Id,
                CourseName = course.Name,
                CourseHours = course.Hours,
                CourseDegree = course.Degree,
                CourseMinDegree = course.MinDegree,
                DepartmentId = course.DepartmentId,
                Departments = DepartmentRepo.GetAll().Select(dept => new SelectListItem
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
                editCourseVM.Departments = DepartmentRepo.GetAll().Select(dept => new SelectListItem
                {
                    Value = dept.Id.ToString(),
                    Text = dept.Name
                }).ToList();

                return View("Edit", editCourseVM);
            }


            Course course = new() 
            { 
                Id = (int)editCourseVM.CourseId,
                Name = editCourseVM.CourseName,
                Hours = editCourseVM.CourseHours,
                Degree = editCourseVM.CourseDegree,
                MinDegree = editCourseVM.CourseMinDegree,
                DepartmentId = editCourseVM.DepartmentId
            };


            CourseRepo.Update(course);

            CourseRepo.Save();

            return RedirectToAction("Index");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var course = CourseRepo.GetById(id);
                if (course == null)
                {
                    return StatusCode(404, new { message = "Course not found." });
                }

                CourseRepo.Delete(id);

                CourseRepo.Save();

                return StatusCode(201, new { message = "Course successfully removed." });
            }
            catch
            {
                return StatusCode(500, $"An error occured while removing the Course.");
            }
        }

        public IActionResult ValidateMinDegree(int courseMinDegree, int courseDegree)
        {
            if (courseMinDegree > courseDegree)
            {
                return Json("Course Degree is lower than the minimum degree.");
            }
            return Json(true);
        }

        public IActionResult ValidateHours(int courseHours)
        {
            if (courseHours % 3 != 0)
            {
                return Json("The course hours must be divisible by 3.");
            }
            return Json(true);
        }
    }
}
