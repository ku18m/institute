using Institute.Models;
using Institute.ViewModels;

namespace Institute.Repository
{
    public interface ICourseRepo: IRepository<Course>
    {
        public List<Course> GetByDepartment(int departmentId);
        public List<Course> GetByInstructor(int instructorId);
        public List<Course> GetByTrainee(int traineeId);
        public List<CourseResult> courseResults(int courseId);
        public CourseDetailsVM? BindGetCourseDetails(int courseId);
    }
}
