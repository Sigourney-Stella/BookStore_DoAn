using BookStoreTM.Models;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly AppDbContext _db;

        public CustomerController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string name, int page = 1)
        {
            int limit = 2;
            var item = _db.Customers.ToPagedList(page, limit);
            if (!string.IsNullOrEmpty(name))
            {
                //tìm kiếm theo tên sách hoặc tên nhà xuất bản
                item = _db.Customers.Where(x => x.Fullname.Contains(name)).ToPagedList(page, limit);
            }
            ViewBag.keyword = name;
            return View(item);
        }
    }
}
