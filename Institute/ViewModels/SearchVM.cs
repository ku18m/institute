using Institute.Models;
using System.ComponentModel.DataAnnotations;

namespace Institute.ViewModels
{
    public class SearchVM<Type>
    {
        [Required(ErrorMessage = "Search string is required")]
        public string SearchString { get; set; }

        public string? SearchProperty { get; set; }

        public List<Type> Items { get; set; }
    }
}
