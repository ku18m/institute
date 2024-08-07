using System.ComponentModel.DataAnnotations;

namespace Institute.Models
{
    public class Department
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(50)]
        public required string Manager { get; set; }


        public virtual ICollection<Course>? Courses { get; set; }
        public virtual ICollection<Instructor>? Instructors { get; set; }
        public virtual ICollection<Trainee>? Trainees { get; set; }
    }
}
