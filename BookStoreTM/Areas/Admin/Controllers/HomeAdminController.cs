using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("admin")]
    public class HomeAdminController : Controller
    {
        private readonly AppDbContext _db;

        public HomeAdminController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetThongKe(string fromDate, string toDate)
        {
            var query = from o in _db.OrderBooks
                        join od in _db.OrderDetails
                        on o.OrderId equals od.OrderId
                        join p in _db.Products
                        on od.ProductId equals p.ProductId
                        select new
                        {
                            CreatedDate = o.OrderDate,
                            Quantity = od.Quatity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate < endDate);
            }

            var result = query.GroupBy(x => x.CreatedDate.Date).Select(x => new
            {
                Date = x.Key,
                TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
                TotalSell = x.Sum(y => y.Quantity * y.Price),
            }).Select(x => new
            {
                Date = x.Date,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy
            })
            .ToList();
            return Json(new { Data = result });
        }
        //[HttpGet]
        //public ActionResult GetStatistical(string fromDate, string toDate)
        //{
        //    var query = from o in _db.OrderBooks
        //                join od in _db.OrderDetails
        //                on o.OrderId equals od.OrderId
        //                join p in _db.Products
        //                on od.ProductId equals p.ProductId
        //                select new
        //                {
        //                    CreatedDate = o.OrderDate,
        //                    Quantity = od.Quatity,
        //                    Price = od.Price,
        //                    OriginalPrice = p.OriginalPrice
        //                };
        //    if (!string.IsNullOrEmpty(fromDate))
        //    {
        //        DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
        //        query = query.Where(x => x.CreatedDate >= startDate);
        //    }
        //    if (!string.IsNullOrEmpty(toDate))
        //    {
        //        DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
        //        query = query.Where(x => x.CreatedDate < endDate);
        //    }

        //    var result = query.GroupBy(x => AppDbContext.TruncateTime(x.CreatedDate)).Select(x => new
        //    {
        //        Date = x.Key.Value,
        //        TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
        //        TotalSell = x.Sum(y => y.Quantity * y.Price),
        //    }).Select(x => new
        //    {
        //        Date = x.Date,
        //        DoanhThu = x.TotalSell,
        //        LoiNhuan = x.TotalSell - x.TotalBuy
        //    });
        //    return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        //}
    }
}
