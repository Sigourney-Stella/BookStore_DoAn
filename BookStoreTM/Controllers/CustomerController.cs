using BookStoreTM.Models;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Member");
            return RedirectToAction("Index","Home");
        }
    }
}
