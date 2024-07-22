using Institute.Models;

namespace Institute.ViewModels
{
    public class InstructorSearchVM
    {
        public string? SearchString { get; set; }
        public string? SearchProperty { get; set; }
        public List<Instructor> Instructors { get; set; }
    }
}
