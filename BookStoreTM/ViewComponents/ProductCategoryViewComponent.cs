using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return View(product);
        }
    }
}
