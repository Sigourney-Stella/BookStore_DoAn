using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreTM.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public ProductViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var product = _context.ProductCategories.ToList();
            var product = await _context.Products.Include(c => c.ProductCategory).Where(x=>x.IsHome && x.IsActicve).Take(12).ToListAsync();
            return View(product);
        }
    }
}
