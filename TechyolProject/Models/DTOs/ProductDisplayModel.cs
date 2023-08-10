namespace TechyolProject.Models.DTOs
{
    public class ProductDisplayModel
    {
        public IEnumerable<Product> Products { get; set;}

        public IEnumerable<Category> Categories { get; set;}

        public string sTerm { get; set;}

        public int CategoryId { get; set;}
    }
}
