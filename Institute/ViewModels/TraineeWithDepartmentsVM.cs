using Institute.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Institute.ViewModels
{
    public class TraineeWithDepartmentsVM
    {
        public int? TraineeId { get; set; }

        [Required(ErrorMessage = "The trainee Name is required.")]
        public string TraineeName { get; set; }

        [Required(ErrorMessage = "The trainee Address is required.")]
        public string TraineeAddress { get; set; }

        public int? TraineeGrade { get; set; }

        public string? TraineeImageUrl { get; set; }

        public IFormFile? imgFile { get; set; }

        [Required(ErrorMessage = "The department is required.")]
        public int DepartmentId { get; set; }

        public List<SelectListItem>? Departments { get; set; }
    }
}
