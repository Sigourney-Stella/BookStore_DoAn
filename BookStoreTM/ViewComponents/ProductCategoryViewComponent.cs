using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;

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
            var product = _context.ProductCategories.ToList();
            //var categories = await _context.Categories.Include(c => c.ProductCategories).ToListAsync();
            return View(product);
        }
    }
}
