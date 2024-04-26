using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            var item = _db.Products.ToList();
            return View(item);
        }
    }
}
