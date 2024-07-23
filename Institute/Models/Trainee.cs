using System.ComponentModel.DataAnnotations.Schema;

namespace Institute.Models
{
    public class Trainee
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Address { get; set; }
        public int? Grade { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        public virtual ICollection<CourseResult> CourseResults { get; set; }
    }
}
