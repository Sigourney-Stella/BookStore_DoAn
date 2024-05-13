using BookStoreTM.Models;
using BookStoreTM.Models.EF;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace BookStoreTM.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;
        public CustomerController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Login(string url)
        {
            if (HttpContext.Session.GetString("Member") != null)
            {
                var dataLogin = JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("Member"));
                ViewBag.Customer = dataLogin;
            }
            ViewBag.UrlAction = url;
            return View();
        }

        [HttpPost]
        public IActionResult Login(Customer model, string urlAction)
        {
            var pass = Utilitties.Utils.GetSHA26Hash(model.Password);
            var data = _context.Customers.Where(x => x.IsActive == true).Where(x => x.Fullname.Equals(model.Fullname) ||
                x.Email.Equals(model.Email)).FirstOrDefault(x => x.Password.Equals(pass));
            var dataLogin = data.ToJson();
            if (data != null)
            {
                //lưu session khi đăng nhập thành công
                HttpContext.Session.SetString("Member", dataLogin);
                if (!string.IsNullOrEmpty(urlAction))
                {
                    return RedirectToAction(urlAction);
                }
                return RedirectToAction("Index", "Shopcart");
            }
            TempData["errorLogin"] = "Lỗi đăng nhập";
            return RedirectToAction("Index");
        }
        public IActionResult Registy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registy(Customer model)
        {
            try
            {
                var pass = Utilitties.Utils.GetSHA26Hash(model.Password);
                model.Password = pass;
                model.CreateDate = DateTime.Now;
                model.IsActive = true;
                _context.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Login", "Customer");
            }
            catch (Exception ex)
            {
                TempData["errorRegisty"] = "Lỗi đăng ký" + ex.Message;
                return RedirectToAction("Index");
            }
        }

        //xem thông tin đơn tài khoản
        public IActionResult Detail()
        {
            var customer = JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("Member"));

            if (customer != null)
            {
                var orderBooks = _context.OrderBooks.Where(x => x.CustomerID == customer.CustomerID).Include(p => p.TransactStatus).Include(p=>p.Payment).ToList();
                //var orderDetails = _context.OrderDetails.Where(x => x.OrderId == orderBooks.Id).Include(p => p.Product).ToListAsync();
                //var customer = _context.Customers.SingleOrDefault(c => c.CustomerID == customerId);
                var orderBooksViews = new List<OrderBooksView>();
                foreach(var item in orderBooks) {
                    var orderBooksView = new OrderBooksView()
                    {
                        OrderId = item.OrderId,
                        CodeOrder = item.CodeOrder,
                        OrderDate = item.OrderDate,
                        TotalMoney = item.TotalMoney,
                        ReceiveName = item.ReceiveName,
                        ReceiveAddress = item.ReceiveAddress,
                        ReceivePhone = item.ReceivePhone,
                        Notes = item.Notes,
                        CustomerName = customer.Fullname,
                        TransactStatus = item.TransactStatus.Status,
                        PaymentName = item.Payment.PaymentName,
                    };
                    orderBooksViews.Add(orderBooksView);
                };
                
                ViewBag.orderBooksViews = orderBooksViews;
                return View(customer);
            }

            return View("Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Member");
            return RedirectToAction("Index","Home");
        }


    }
}
