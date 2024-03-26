using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;

        public ProductController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(int? page)
        {

            return View();
        }
    }
}
