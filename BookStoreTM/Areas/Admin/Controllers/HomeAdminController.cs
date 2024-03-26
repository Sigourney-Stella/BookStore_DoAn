using Microsoft.AspNetCore.Mvc;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("admin")]
    public class HomeAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
