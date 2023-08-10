using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechyolProject.Models
{
    [Table("Category")]
    public class Category
    {

        public int Id { get; set; }


        [Required]
        [MaxLength(100)]
        public string? CategoryName { get; set; }


        public List<Product>? Products { get; set; }
    }
}
