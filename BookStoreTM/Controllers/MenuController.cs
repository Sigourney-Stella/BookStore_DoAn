using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreTM.Controllers
{
    public class MenuController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<MenuController> _logger;

        public MenuController(AppDbContext db, ILogger<MenuController> logger)
        {
            _db = db;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MenuTop()
        {
            var categories = _db.Categories.OrderBy(x => x.Position).ToList();
            return PartialView("_MenuTop", categories);
        }
    }
}
