using Institute.Models;
using System.ComponentModel.DataAnnotations;

namespace Institute.ViewModels
{
    public class InstructorWithDepartmentsAndCoursesVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? ImageUrl { get; set; }
        public IFormFile? imgFile { get; set; }

        [Required, Range(6000, double.MaxValue)]
        public decimal Salary { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int CourseId { get; set; }


        public List<Course>? Courses { get; set; }
        public List<Department>? Departments { get; set; }
    }
}
