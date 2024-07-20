using System.ComponentModel.DataAnnotations.Schema;

namespace Institute.Models
{
    public class Course
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Degree { get; set; }
        public int MinDegree { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        public virtual ICollection<Instructor> Instructors { get; set; }

        public virtual ICollection<CourseResult> CourseResults { get; set; }
    }
}
