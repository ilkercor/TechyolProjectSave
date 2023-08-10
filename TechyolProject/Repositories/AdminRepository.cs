using Microsoft.AspNetCore.Mvc;

namespace TechyolProject.Repositories
{
    public class AdminRepository : IAdminRepository
    {

        private ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddItem(Product product)
        {
            _context.Product.Add(product);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void AddCategory(Category category)
        {
            _context.Category.Add(category);
        }
    }
}
