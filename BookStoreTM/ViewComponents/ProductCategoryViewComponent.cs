using BookStoreTM.Models;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookStoreTM.ViewComponents
{
    public class ProductCategoryViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public ProductCategoryViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var product = await _context.ProductCategories.ToListAsync();
            var cartTotalQuantity = HttpContext.Session.GetInt32("CartTotalQuantity") ?? 0;
            ViewBag.CartTotalQuantity = cartTotalQuantity;

            var dataLogin = _context.Customers.ToListAsync();
            ViewBag.Customer = dataLogin;

            return View(product);
        }
    }
}
