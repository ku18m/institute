namespace Institute.ViewModels
{
    public class StudentWithCourseDegreeVM
    {
        public string? CrsName { get; set; }
        public string? TraineeName { get; set; }
        public int Degree { get; set; }
        public bool IsPassed { get; set; }
        public string Color => IsPassed ? "success" : "danger";
    }
}
