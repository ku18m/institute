using Institute.Models;
using Institute.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Institute.Controllers
{
    public class TraineeController : Controller
    {
        private readonly InstituteContext context = new();

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


    }
}

