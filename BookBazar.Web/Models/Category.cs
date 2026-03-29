using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookBazar.Web.Models
{
    public class Category
    {
        public int Id { get; set; }

        [DisplayName("Category Name")]
        [MaxLength(30)]
        public required string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1 and 100")]
        [Required(ErrorMessage = "Display Order is required")]
        public int? DisplayOrder { get; set; }
    }
}
