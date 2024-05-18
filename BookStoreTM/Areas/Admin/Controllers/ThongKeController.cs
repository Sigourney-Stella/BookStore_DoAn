using Microsoft.AspNetCore.Mvc;

namespace BookStoreTM.Areas.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
