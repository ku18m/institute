using Institute.Models;
using Institute.ViewModels;

namespace Institute.Repository
{
    public class CourseRepository(InstituteContext context) : ICourseRepo
    {
        public List<Course> GetPage(int page)
        {
            int pageSize = 5;

            return context.Courses.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public int GetTotalPages(int pageSize)
        {
            return (int)Math.Ceiling((double)context.Courses.Count() / pageSize);
        }

        public List<Course> Search(string searchString, string searchProperty)
        {

            switch (searchProperty)
            {
                case "hours":
                    int hours;
                    int.TryParse(searchString, out hours);
                    return context.Courses.Where(crs => crs.Hours == hours).ToList();
                case "degree":
                    int degree;
                    int.TryParse(searchString, out degree);
                    return context.Courses.Where(crs => crs.Degree == degree).ToList();
                case "minDegree":
                    int minDegree;
                    int.TryParse(searchString, out minDegree);
                    return context.Courses.Where(crs => crs.MinDegree == minDegree).ToList();
                default: // Otherwise, search by name.
                    return context.Courses.Where(tr => tr.Name.Contains(searchString)).ToList();
            }
        }

        public void Insert(Course entity)
        {
            context.Courses.Add(entity);
        }
        public void Update(Course entity)
        {
            context.Courses.Update(entity);
        }

        public void Delete(int id)
        {
            context.Courses.Remove(context.Courses.Find(id));
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public List<Course> GetAll()
        {
            throw new NotImplementedException();
        }

        public Course GetById(int id)
        {
            return context.Courses.Find(id);
        }

        public List<Course> GetByDepartment(int departmentId)
        {
            throw new NotImplementedException();
        }

        public List<Course> GetByInstructor(int instructorId)
        {
            throw new NotImplementedException();
        }

        public List<Course> GetByTrainee(int traineeId)
        {
            throw new NotImplementedException();
        }

        public List<CourseResult> courseResults(int courseId)
        {
            throw new NotImplementedException();
        }

        public CourseDetailsVM? BindGetCourseDetails(int id)
        {
            return context.Courses.Where(crs => crs.Id == id)
                .Select(crs => new CourseDetailsVM
                {
                    CourseId = crs.Id,
                    CourseName = crs.Name,
                    CourseDegree = crs.Degree,
                    CourseHours = crs.Hours,
                    CourseMinDegree = crs.MinDegree,
                    DepartmentId = crs.DepartmentId,
                    DepartmentName = crs.Department.Name,
                    Instructors = crs.Instructors.ToList(),
                    CourseResults = context.CourseResults
                        .Where(crslt => crslt.CourseId == crs.Id)
                        .Select(crslt => new StudentWithCourseDegreeVM
                        {
                            TraineeName = crslt.Trainee.Name,
                            Degree = crslt.Degree,
                            IsPassed = crslt.Degree >= crslt.Course.MinDegree
                        }).ToList()
                }
                ).FirstOrDefault();
        }
    }
}
