using Institute.Models;

namespace Institute.ViewModels
{
    public class TraineeDetailsVM
    {
        public int? TraineeId { get; set; }
        public string? TraineeName { get; set; }
        public string? TraineeAddress { get; set; }
        public int? TraineeGrade { get; set; }
        public string? TraineeImageUrl { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public List<StudentWithCourseDegreeVM>? CourseResults { get; set; }
    }
}
