using Institute.Models;
using Institute.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Institute.Controllers
{
    public class TraineeController : Controller
    {
        private readonly InstituteContext context = new();
        private readonly IWebHostEnvironment webHostEnv;

        public TraineeController(IWebHostEnvironment webHostEnvironment)
        {
            webHostEnv = webHostEnvironment;
        }

        public IActionResult ShowResult(int id, int crsid)
        {
            StudentWithCourseDegreeVM stdCrsVM;
            var traineeWithCourse = context.CourseResults
                .Where(crs => crs.TraineeId == id && crs.CourseId == crsid)
                .Select
                (
                    crsresult => new
                    {
                        CrsName = crsresult.Course.Name,
                        TraineeName = crsresult.Trainee.Name,
                        crsresult.Degree,
                        IsPassed = crsresult.Degree >= crsresult.Course.MinDegree
                    }
                ).FirstOrDefault();

            if (traineeWithCourse == null)
            {
                return NotFound();
            }

            stdCrsVM = new StudentWithCourseDegreeVM()
            {
                CrsName = traineeWithCourse.CrsName,
                TraineeName = traineeWithCourse.TraineeName,
                Degree = traineeWithCourse.Degree,
                IsPassed = traineeWithCourse.IsPassed
            };

            return View("ShowResult", stdCrsVM);
        }

        public IActionResult ShowTraineeResult(int id)
        {
            var courseResults = context.CourseResults
                .Where(crs => crs.TraineeId == id)
                .Select
                (
                    crsresult => new StudentWithCourseDegreeVM()
                    {
                        CrsName = crsresult.Course.Name,
                        Degree = crsresult.Degree,
                        IsPassed = crsresult.Degree >= crsresult.Course.MinDegree
                    }
                ).ToList();

            if (courseResults.Count == 0)
            {
                return NotFound();
            }

            var traineeResultsVM = new TraineeResults()
            {
                TraineeName = context.Trainees.Find(id).Name,
                CourseResults = courseResults
            };

            return View("ShowTraineeResult", traineeResultsVM);
        }

        public IActionResult Login(int id)
        {
            var cookieOptions = new CookieOptions()
            {
                Expires = System.DateTime.Now.AddDays(2)
            };

            Response.Cookies.Append("TraineeId", id.ToString(), cookieOptions);

            HttpContext.Session.SetString("TraineeId", id.ToString());


            return Content("Logged in successfully");
        }

        public IActionResult CheckLogin()
        {
            string traineeId = Request.Cookies["TraineeId"];

            if (string.IsNullOrEmpty(traineeId))
            {
                return Content("Not logged in");
            }

            var trainee = context.Trainees.Find(int.Parse(traineeId));
            if (trainee == null)
            {
                return Content("Not logged in");
            }

            string sessionTraineeId = HttpContext.Session.GetString("TraineeId");

            if (string.IsNullOrEmpty(sessionTraineeId))
            {
                return Content("Not logged in");
            }

            var traineeUsingSession = context.Trainees.Find(int.Parse(sessionTraineeId));

            return Content($"Trainee: {trainee.Name} Logged in successfully\n" +
                $"Trainee Name using session {traineeUsingSession.Name}");
        }

        public IActionResult Index(int page = 1)
        {
            int pageSize = 5;
            var traineeVM = new PaginationVM<Trainee>()
            {
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)context.Trainees.Count() / pageSize),
                Items = context.Trainees.Skip((page - 1) * pageSize).Take(pageSize).ToList()
            };

            return View("Index", traineeVM);
        }

        [HttpPost]
        public IActionResult Search(string searchString, string propertyToSearch)
        {
            List<Trainee> filteredTrainees;
            SearchVM<Trainee> traineeSearchVM;

            switch (propertyToSearch)
            {
                case "address":
                    filteredTrainees = context.Trainees.Where(tr => tr.Address.Contains(searchString)).ToList();
                    break;
                case "grade":
                    decimal grade;
                    Decimal.TryParse(searchString, out grade);
                    filteredTrainees = context.Trainees.Where(tr => tr.Grade == grade).ToList();
                    break;
                default: // Otherwise, search by name.
                    filteredTrainees = context.Trainees.Where(tr => tr.Name.Contains(searchString)).ToList();
                    break;
            }

            traineeSearchVM = new SearchVM<Trainee>()
            {
                SearchString = searchString,
                SearchProperty = propertyToSearch,
                Items = filteredTrainees
            };

            return View("Search", traineeSearchVM);
        }

        public IActionResult Details(int id)
        {
            var traineeVM = context.Trainees.Where(tr => tr.Id == id)
                .Select(tr => new TraineeDetailsVM
                {
                    TraineeId = tr.Id,
                    TraineeName = tr.Name,
                    TraineeAddress = tr.Address,
                    TraineeGrade = tr.Grade,
                    TraineeImageUrl = tr.ImageUrl,
                    DepartmentId = tr.DepartmentId,
                    DepartmentName = tr.Department.Name,
                    CourseResults = context.CourseResults
                        .Where(crs => crs.TraineeId == tr.Id)
                        .Select(crs => new StudentWithCourseDegreeVM
                        {
                            CrsName = crs.Course.Name,
                            Degree = crs.Degree,
                            IsPassed = crs.Degree >= crs.Course.MinDegree
                        }).ToList()
                }
                ).FirstOrDefault();

            return View("Details", traineeVM);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var addTraineeVM = new TraineeWithDepartmentsVM()
            {
                Departments = context.Departments.Select(dept => new SelectListItem
                {
                    Value = dept.Id.ToString(),
                    Text = dept.Name
                }).ToList()
            };
            return View("Add", addTraineeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TraineeWithDepartmentsVM traineeVM)
        {
            if (!ModelState.IsValid)
            {
                traineeVM.Departments = context.Departments.Select(dept => new SelectListItem
                {
                    Value = dept.Id.ToString(),
                    Text = dept.Name
                }).ToList();

                return View("Add", traineeVM);
            }

            var trainee = new Trainee()
            {
                Name = traineeVM.TraineeName,
                Address = traineeVM.TraineeAddress,
                Grade = traineeVM.TraineeGrade,
                DepartmentId = traineeVM.DepartmentId,
                ImageUrl = await UploadImage(traineeVM.imgFile) // Upload the img and get its path, if error ocurred returns Empty string.
            };

            context.Trainees.Add(trainee);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var trainee = context.Trainees.FirstOrDefault(ins => ins.Id == id);

            var editTraineeVM = new TraineeWithDepartmentsVM()
            {
                TraineeId = trainee.Id,
                TraineeName = trainee.Name,
                TraineeAddress = trainee.Address,
                TraineeGrade = trainee.Grade,
                TraineeImageUrl = trainee.ImageUrl,
                DepartmentId = trainee.DepartmentId,
                Departments = context.Departments.Select(dept => new SelectListItem
                {
                    Value = dept.Id.ToString(),
                    Text = dept.Name
                }).ToList()
            };

            return View("Edit", editTraineeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TraineeWithDepartmentsVM editTraineeVM)
        {
            if (!ModelState.IsValid)
            {
                editTraineeVM.Departments = context.Departments.Select(dept => new SelectListItem
                {
                    Value = dept.Id.ToString(),
                    Text = dept.Name
                }).ToList();

                return View("Edit", editTraineeVM);
            }


            var trainee = context.Trainees.FirstOrDefault(ins => ins.Id == editTraineeVM.TraineeId);

            if (editTraineeVM.imgFile != null) // If the user uploaded a new image, remove the old one.
            {
                RemoveImage(trainee.ImageUrl);
            }

            trainee.Name = editTraineeVM.TraineeName;
            trainee.Address = editTraineeVM.TraineeAddress;
            trainee.Grade = editTraineeVM.TraineeGrade ?? trainee.Grade;
            trainee.DepartmentId = editTraineeVM.DepartmentId;
            trainee.ImageUrl = await UploadImage(editTraineeVM.imgFile); // Upload the img and get its path, if error ocurred returns Empty string.

            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var trainee = context.Trainees.FirstOrDefault(ins => ins.Id == id);
                if (trainee == null)
                {
                    return StatusCode(404, new { message = "Trainee not found." });
                }
                RemoveImage(trainee.ImageUrl);
                context.Trainees.Remove(trainee);
                context.SaveChanges();
                return StatusCode(201, new { message = "Trainee successfully removed." });
            }
            catch
            {
                return StatusCode(500, $"An error occured while removing the Trainee.");
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

