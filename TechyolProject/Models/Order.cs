using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechyolProject.Models
{

    [Table("Order")]
    public class Order
    {

        public int Id { get; set; }

        [Required]
        public int UserID { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;


        [Required]
        public int OrderStatusID { get; set; }

        public bool IsDeleted { get; set; }

        public OrderStatus OrderStatus { get; set; }
            
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
