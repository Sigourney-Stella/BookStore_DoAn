using BookStoreTM.Models.Entities;
using BookStoreTM.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("admin")]
    //[Route("/Admin/Category")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;

        public CategoryController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var items = _db.Categories.ToList();
            return View(items);
        }

        //thêm mới danh mục
        [HttpGet]
        public IActionResult ThemMoiDanhMuc()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemMoiDanhMuc(Category DanhMuc)
        {
            if (ModelState.IsValid)
            {
                DanhMuc.CreatedDate = DateTime.Now;
                //DanhMuc.UpdatedDate = DateTime.Now;
                _db.Categories.Add(DanhMuc);
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(DanhMuc);
        }

        //sửa 
        [HttpGet]
        public IActionResult SuaDanhMuc(int maLoai)
        {
            var loai = _db.Categories.Find(maLoai);
            return View(loai);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaDanhMuc(Category loai)
        {
            if (ModelState.IsValid)
            {
                _db.Update(loai);
                //_db.Entry(loai).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(loai);
        }

        //xoá
        public IActionResult XoaDanhMuc(int maLoai)
        {
            var item = _db.Categories.Find(maLoai);
            if (item != null)
            {
                _db.Categories.Remove(item);
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(maLoai);
        }
    }
}
