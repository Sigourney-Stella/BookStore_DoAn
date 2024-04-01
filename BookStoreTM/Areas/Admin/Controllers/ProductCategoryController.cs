using BookStoreTM.Models.Entities;
using BookStoreTM.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductCategoryController : Controller
    {
        private readonly AppDbContext _db;

        public ProductCategoryController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var items = _db.ProductCategories.ToList();
            return View(items);
        }

        //thêm danh mục sản phẩm
        [HttpGet]
        public IActionResult ThemDMSanPham()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemDMSanPham(ProductCategory SanPham)
        {
            if (ModelState.IsValid)
            {
                SanPham.CreatedDate = DateTime.Now;
                _db.ProductCategories.Add(SanPham);
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(SanPham);
        }

        //sửa 
        [HttpGet]
        public IActionResult SuaDMSanPham(int MaSanPham)
        {
            var sanPham = _db.ProductCategories.Find(MaSanPham);
            return View(sanPham);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaDMSanPham(ProductCategory MaLoai)
        {
            if (ModelState.IsValid)
            {
                MaLoai.CreatedDate = DateTime.Now;
                _db.Update(MaLoai);
                //_db.Entry(loai).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(MaLoai);
        }

        //xoá
        //[Route("XoaDanhMuc")]
        public IActionResult XoaDMSanPham(int maLoai)
        {
            var item = _db.ProductCategories.Find(maLoai);
            if (item != null)
            {
                _db.ProductCategories.Remove(item);
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(maLoai);
        }
    }
}
