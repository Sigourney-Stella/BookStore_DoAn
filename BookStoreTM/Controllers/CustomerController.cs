using BookStoreTM.Models;
using BookStoreTM.Models.EF;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Http;
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
            var data = _context.Customers.Where(x => x.IsActive == true).Where(x => x.Email.Equals(model.Email))
                .FirstOrDefault(x => x.Password.Equals(pass));
            var dataLogin = data.ToJson();
            if (data != null)
            {
                //lưu session khi đăng nhập thành công
                HttpContext.Session.SetString("Member", dataLogin);
                if (!string.IsNullOrEmpty(urlAction))
                {
                    return RedirectToAction(urlAction);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu khôg đúng! Vui lòng nhập lại");
                return View(model);
            }
            //ViewData["errorLogin"] = "Lỗi đăng nhập";
            return RedirectToAction("Login");
        }
        public IActionResult Registy()
        {
            Customer model = new Customer();
            return View(model);
        }
        [HttpPost]
        public IActionResult Registy(Customer model)
        {
            try
            {
                var taiKhoan = _context.Customers.Where(x => x.Email == model.Email).FirstOrDefault();
                if(taiKhoan == null)
                {
                    var pass = Utilitties.Utils.GetSHA26Hash(model.Password);
                    model.Password = pass;
                    model.CreateDate = DateTime.Now;
                    model.IsActive = true;
                    _context.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction("Login", "Customer");
                }
                else
                {
                    //TempData["errorRegisty"] = "Email đã được sử dụng! Vui lòng sử dụng email khác";
                    //return View();
                    ModelState.AddModelError("Email", "Email đã được sử dụng! Vui lòng sử dụng email khác");
                    return View(model);
                }
                
            }
            catch (Exception ex)
            {
                return RedirectToAction("Registy");
            }
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Member");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Customer model)
        {
            if (ModelState.IsValid)
            {
                // Lấy đối tượng Customer hiện tại từ cơ sở dữ liệu
                var customer = _context.Customers.Find(model.CustomerID);
                if (customer == null)
                {
                    return NotFound();
                }
                // Cập nhật thông tin từ form vào đối tượng Customer
                customer.Fullname = model.Fullname;
                customer.Email = model.Email;
                customer.Address = model.Address;
                customer.Phone = model.Phone;
                customer.Brithday = model.Brithday;
                customer.Avatar = model.Avatar;

                _context.SaveChanges();

                //Lưu lại thông tin vào session
                var updatedCustomer = JsonConvert.SerializeObject(customer);
                HttpContext.Session.SetString("Member", updatedCustomer);
               
                return RedirectToAction("Detail");
            }
            return View("Detail", model);
        }

        //xem thông tin đơn tài khoản

        public IActionResult Detail()
        {
            var customer = JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("Member"));

            if (customer != null)
            {
                var orderBooks = _context.OrderBooks
                                         .Where(x => x.CustomerID == customer.CustomerID)
                                         .Include(p => p.TransactStatus)
                                         .Include(p => p.Payment)
                                         .ToList();

                var orderBooksViews = new List<OrderBooksView>();
                foreach (var item in orderBooks)
                {
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
                }

                var orderDetails = _context.OrderDetails
                                           .Where(od => orderBooks.Select(ob => ob.OrderId).Contains(od.OrderId))
                                           .Include(od => od.Product)
                                           .ToList();

                var orderDetailsViews = orderDetails.Select(od => new ProductDetailViewModel
                {
                    OrderDetailsId = od.OrderDetailsId,
                    ProductName = od.Product.ProductName,
                    Quantity = od.Quatity,
                    Price = od.Price,
                    TotalMoney = od.TotalMoney,
                    OrderId = od.OrderId,
                    Description = od.Product.Description,
                    Images = od.Product.Images,
                }).ToList();

                ViewBag.orderBooksViews = orderBooksViews;
                ViewBag.orderDetailsViews = orderDetailsViews;
                return View(customer);
            }

            return View("Home");
        }
    }
}
