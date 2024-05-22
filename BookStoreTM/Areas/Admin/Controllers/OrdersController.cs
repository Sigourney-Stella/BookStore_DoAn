using BookStoreTM.Models;
using BookStoreTM.Models.EF;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _db;

        public OrdersController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string name, int page = 1)
        {
            int limit = 5;
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
            var item = _db.OrderBooks.Find(id);

            var productDetail = _db.OrderDetails.Where(x => x.OrderId == id).Include(p => p.Product).ToList();
            ViewBag.productDetail = productDetail;

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, int IdStatus)
        {
            try
            {
                var order = await _db.OrderBooks.FindAsync(orderId);
                if (order != null)
                {
                    if(IdStatus == 3) // nếu là trạng thái 
                    {
                        var orderDetail = _db.OrderDetails.Where(x => x.OrderId == orderId).ToList();
                        foreach(var detail in orderDetail)
                        {
                            var product = _db.Products.Find(detail.ProductId);
                            if (product != null)
                            {
                                product.Quatity += detail.Quatity;
                            }
                        }
                    }
                    order.TransactStatusID = IdStatus;
                    _db.Update(order);
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Cập nhật trạng thái đơn hàng thành công.";
                }
                else
                {
                    TempData["error"] = "Không tìm thấy đơn hàng.";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Có lỗi xảy ra: " + ex.Message;
            }

            return RedirectToAction("Index");
        }   
    }
}
