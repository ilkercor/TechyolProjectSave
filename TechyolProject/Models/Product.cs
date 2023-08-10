using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechyolProject.Models
{

    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }


        [Required]
        public double Price { get; set; }

        public string? Image { get; set; }


        [Required]
        public int CategoryID { get; set; }

        public Category Category { get; set; }

        public List<OrderDetail> OrderDetail { get; set; }
        public List<CartDetail> CartDetail { get; set; }


        [NotMapped]
        public string CategoryName { get; set; }
    }
}
