using System.ComponentModel.DataAnnotations;

namespace Institute.ViewModels
{
    public class DepartmenModifytVM
    {
        public int? DepartmentId { get; set; }

        [Required(ErrorMessage = "Department Name is required")]
        public string? DepartmentName { get; set; }

        [Required(ErrorMessage = "Department Manager Name is required")]
        public string? DepartmentManager { get; set; }
    }
}
