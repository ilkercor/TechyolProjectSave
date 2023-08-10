
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TechyolProject.Models;
using TechyolProject.Models.DTOs;
using static System.Reflection.Metadata.BlobBuilder;

namespace TechyolProject.Repositories
{
    public class HomeRepository:IHomeRepository
    {

        private readonly ApplicationDbContext _context;

        public HomeRepository(ApplicationDbContext _context) { 
            this._context = _context;
        }


        public async Task<IEnumerable<Category>> Categories()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts(string sTerm = "", int categoryId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await (from product in _context.Product
                            join category in _context.Category
                            on product.CategoryID equals category.Id
                            where string.IsNullOrWhiteSpace(sTerm) || (product !=null && product.Name.ToLower().StartsWith(sTerm))
                            select new Product
                            {
                                Id = product.Id,
                                Image = product.Image,
                                Name = product.Name,
                                Price = product.Price,
                                CategoryID = product.CategoryID,
                                CategoryName = category.CategoryName,
                            }
                           ).ToListAsync();
            if (categoryId > 0)
            {

                products = products.Where(a => a.CategoryID == categoryId).ToList();
            }
            return products;
        }
    }
}
