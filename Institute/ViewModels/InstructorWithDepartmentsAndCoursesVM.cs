using Institute.Models;
using System.ComponentModel.DataAnnotations;

namespace Institute.ViewModels
{
    public class InstructorWithDepartmentsAndCoursesVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required.")]
        public string Name { get; set; }

        public string? ImageUrl { get; set; }
        public IFormFile? imgFile { get; set; }

        [Required, Range(6000, double.MaxValue, ErrorMessage = "The salary must be more than 6000")]
        public decimal Salary { get; set; }

        [Required]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please select a department")]  // Needs to validate the department using remote ASAP.
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please select a course")]  // Needs to validate the course using remote ASAP.
        public int CourseId { get; set; }


        public List<Course>? Courses { get; set; }
        public List<Department>? Departments { get; set; }
    }
}
