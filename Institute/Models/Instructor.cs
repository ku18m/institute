using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institute.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }

        [Column(TypeName = "money"), Range(4000, double.MaxValue)]
        public decimal Salary { get; set; }
        public string? Address { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        [ForeignKey("Course")]
        public int? CourseId { get; set; }
        public virtual Course? Course { get; set; }
    }
}
