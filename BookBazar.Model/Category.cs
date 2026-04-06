using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookBazar.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1 and 100")]
        [Required(ErrorMessage = "Display Order is required")]
        public int? DisplayOrder { get; set; }
    }
}
