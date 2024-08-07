using Institute.Models;

namespace Institute.ViewModels
{
    public class DepartmentDetailsVM
    {
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? DepartmentManager { get; set; }
        public List<Instructor>? Instructors { get; set; }
        public List<Course>? Courses { get; set; }
        
    }
}
