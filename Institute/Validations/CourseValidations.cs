using Institute.Models;
using Institute.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Institute.Validations
{
    public class UniquePerDeptAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("The course name is required.");
            }

            string courseName = value?.ToString();

            var context = new InstituteContext();

            var courseVM = (CourseWithDepartmentsVM)validationContext.ObjectInstance;

            var courseInDb = context.Courses.SingleOrDefault(crs => crs.Name == courseName && crs.DepartmentId == courseVM.DepartmentId);

            if (courseInDb == null || courseInDb.Id == courseVM.CourseId)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}
