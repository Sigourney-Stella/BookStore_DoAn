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
            var categories = _db.ProductCategories.ToList();

            ViewBag.Categories = categories;

            if (id != null)
            {
                products = _db.Products.Where(b => b.ProductCategoryId == id.Value).ToList();
            }
            return View(products);
        }
        public IActionResult Detail(int? id)
        {
            var products = _db.Products.Include(p => p.ProductCategory).Include(b=>b.Publisher).ToList();
            var item = _db.Products.Find(id);
            return View(item);
        }

        public ActionResult ProductCategory(string alias, int id)
        {
            var items = _db.Products.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = _db.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Name;
            }

            ViewBag.CateId = id;
            return View(items);
        }
    }
}
