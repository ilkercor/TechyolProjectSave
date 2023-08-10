using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TechyolProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController (ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
              _context.Product.Add(product);
              _context.SaveChanges();
              return RedirectToAction("Index", "Home"); // Örneğin ana sayfaya yönlendirme
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Category.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(category);
        }

    }
}
