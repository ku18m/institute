using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Institute.ViewModels
{
    public class CourseWithDepartmentsVM
    {
        public int? CourseId { get; set; }

        [Required(ErrorMessage = "The course Name is required.")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "The course degree is required.")]
        public int CourseDegree { get; set; }

        [Required(ErrorMessage = "The course minimum degree is required.")]
        public int CourseMinDegree { get; set; }

        [Required(ErrorMessage = "The department is required.")]
        public int DepartmentId { get; set; }

        public List<SelectListItem>? Departments { get; set; }
    }
}
