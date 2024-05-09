﻿using BookStoreTM.Models.EF;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Mvc.Filters;
using BookStoreTM.Models;


namespace BookStoreTM.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly AppDbContext _db;

        private List<ShopCart> carts = new List<ShopCart>();
        public ShopCartController(AppDbContext db)
        {
            _db = db;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cartInSession = HttpContext.Session.GetString("My-Cart");
            if (cartInSession != null)
            {
                // nếu cartInSession không null thì gán dữ liệu cho biến carts
                // Chuyển san dữ liệu json
                carts = JsonConvert.DeserializeObject<List<ShopCart>>(cartInSession);
            }
            base.OnActionExecuting(context);
        }
        
        public IActionResult Index()
        {
            decimal total = 0;
            var count = 0;
            foreach (var item in carts)
            {
                total += item.Quantity * item.PriceSale;
                count++;
            }
            ViewBag.Total = total;
            ViewBag.Count = count;
            return View(carts);
        }
        public IActionResult CheckOut()
        {
            decimal total = 0;
            var count = 0;
            foreach (var item in carts)
            {
                total += item.Quantity * item.PriceSale;
                count++;
            }
            ViewBag.Total = total;
            ViewBag.Count = count;
            return View(carts);
        }

        //thêm sản phẩm vào giỏ hàng
        public IActionResult Add(int id)
        {
            var items = carts.SingleOrDefault(p => p.ProductId == id);
            if (carts.Any(c => c.ProductId == id))
            {
                carts.Where(c => c.ProductId == id).First().Quantity += 1;
                items.SoLuong++;
            }
            else
            {
                var p = _db.Products.FirstOrDefault(x => x.ProductId == id);

                var item = new ShopCart()
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = (decimal)p.Price,
                    PriceSale = (decimal)p.PriceSale,
                    Quantity = 1,
                    ProductImg = p.Images,
                    TotalPrice = (decimal)p.PriceSale * 1
                };
                item.SoLuong++;
                carts.Add(item);
            }
            var totalQuantity = carts.Sum(c => c.Quantity);
            HttpContext.Session.SetInt32("CartTotalQuantity", totalQuantity);
            ViewBag.CartTotalQuantity = totalQuantity;

            HttpContext.Session.SetString("My-Cart", JsonConvert.SerializeObject(carts));
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            if (carts.Any(c => c.ProductId == id))
            {
                var item = carts.Where(c => c.ProductId == id).First();
                carts.Remove(item);
                HttpContext.Session.SetString("My-Cart", JsonConvert.SerializeObject(carts));
            }
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id, int quantity)
        {
            if (carts.Any(c => c.ProductId == id))
            {
                carts.Where(c => c.ProductId == id).First().Quantity = quantity;
                HttpContext.Session.SetString("My-Cart", JsonConvert.SerializeObject(carts));
            }
            return RedirectToAction("Index");
        }
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("My-Cart");
            return RedirectToAction("Index");
        }
        //public IActionResult Index()
        //{
        //    decimal total = 0;
        //    foreach (var item in carts)
        //    {
        //        total += item.Quantity * item.PriceSale;
        //    }
        //    ViewBag.Total = total;
        //    return View(carts);
        //}
        //[HttpPost]
        //public IActionResult Add(int id)
        //{
        //    if (carts.Any(c => c.ProductId == id))
        //    {
        //        carts.Where(c => c.ProductId == id).First().Quantity += 1;
        //    }
        //    else
        //    {
        //        var p = _db.Products.Find(id);
        //        var item = new ShopCart()
        //        {
        //            ProductId = p.ProductId,
        //            ProductName = p.ProductName,
        //            Price = (decimal)p.Price,
        //            PriceSale = (decimal)p.PriceSale,
        //            Quantity = 1,
        //            ProductImg = p.Images,
        //            TotalPrice = (decimal)p.Price * 1
        //        };
        //        //if (p.PriceSale > 0)
        //        //{
        //        //    item.Price = (decimal)p.PriceSale;
        //        //}
        //        //item.TotalPrice = item.Quantity * item.Price;
        //        carts.Add(item);
        //    }

        //    HttpContext.Session.SetString("My-Cart", JsonConvert.SerializeObject(carts));
        //    return RedirectToAction("Index");
        //}
    }
}
