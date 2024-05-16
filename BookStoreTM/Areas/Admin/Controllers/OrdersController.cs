using BookStoreTM.Models;
using BookStoreTM.Models.EF;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using X.PagedList;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("admin")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _db;

        public OrdersController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string name, int page = 1)
        {
            int limit = 10;
            IPagedList<OrderBook> orders = _db.OrderBooks.Include(p => p.Customer).Include(p => p.TransactStatus).OrderByDescending(p => p.OrderId).ToPagedList(page, limit);
            if (!string.IsNullOrEmpty(name))
            {
                orders = _db.OrderBooks.Where(x => x.CodeOrder.Contains(name)).ToPagedList(page, limit);
            }
            ViewBag.PageSize = limit;
            ViewBag.Page = page;
            //phương thức thanh toán
            var dataStatus = _db.TransactStatus.ToList();
            ViewData["IdStatus"] = new SelectList(dataStatus, "TransactStatusID", "Status");
            ViewBag.keyword = name;
            return View(orders);
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var orderBooksViews = new List<OrderBooksView>();
            ViewBag.orderBooksViews = orderBooksViews;
            var item = _db.OrderBooks.Find(id);
            return View(item);
        }
    }
}
