using Institute.Models;

namespace Institute.ViewModels
{
    public class CourseDetailsVM
    {
        public int? CourseId { get; set; }

        public string CourseName { get; set; }

        public int CourseDegree { get; set; }

        public int CourseMinDegree { get; set; }

        public string? DepartmentName { get; set; }

        public int DepartmentId { get; set; }

        public List<Instructor> Instructors { get; set; }

        public List<StudentWithCourseDegreeVM> CourseResults { get; set; }
    }
}
