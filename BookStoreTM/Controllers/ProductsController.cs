using BookStoreTM.Models;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreTM.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(AppDbContext db, ILogger<ProductsController> logger)
        {
            _db = db;
            _logger = logger;
        }
        public IActionResult Index(int? id)
        {
            var products = _db.Products.Include(p => p.ProductCategory).ToList();
            if (id != null)
            {
                products = _db.Products.Where(b => b.ProductCategoryId == id.Value).ToList();
            }
            //var item = _db.ProductCategories.Find(id);
            //ViewBag.CateName = item.Name;
            return View(products);
        }
        public IActionResult Detail(int? id)
        {
            var item = _db.Products.Find(id);
            return View(item);
        }
    }
}
