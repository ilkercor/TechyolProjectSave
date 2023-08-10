using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace TechyolProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        [Obsolete]
        private readonly IWebHostEnvironment _hostEnvironment;

        [Obsolete]
        public AdminController (ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
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
              return RedirectToAction("Index", "Home");
        }

        public IActionResult Create()
        {
            var categories = _context.Category.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View();
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
