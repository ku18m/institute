using Institute.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Institute.ViewModels
{
    public class CourseWithDepartmentsVM
    {
        public int? CourseId { get; set; }

        [Required(ErrorMessage = "The course Name is required.")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "The course name must be between 3 and 25 characters.")]
        [UniquePerDept(ErrorMessage = "A course with the same name already exist in the selected department.")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "The course hours is required.")]
        [Remote("ValidateHours", "Course", ErrorMessage = "The course hours must be divisible by 3.")]
        public int CourseHours { get; set; }

        [Required(ErrorMessage = "The course degree is required.")]
        [Range(20, 100, ErrorMessage = "The course degree must be between 20 and 100.")]
        public int CourseDegree { get; set; }

        [Required(ErrorMessage = "The course minimum degree is required.")]
        [Remote("ValidateMinDegree", "Course", AdditionalFields = "CourseDegree", ErrorMessage = "Course Degree is lower than the minimum degree.")]
        public int CourseMinDegree { get; set; }

        [Required(ErrorMessage = "The department is required.")]
        public int DepartmentId { get; set; }

        public List<SelectListItem>? Departments { get; set; }
    }
}
