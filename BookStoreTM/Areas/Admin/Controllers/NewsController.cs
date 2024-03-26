using BookStoreTM.Models.Entities;
using BookStoreTM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly AppDbContext _db;

        public object DataLocal { get; private set; }

        public NewsController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var items = _db.News.OrderByDescending(x => x.NewsId).ToList();
            return View(items);
        }
        [HttpGet]
        public IActionResult ThemTinTuc()
        {
            ViewBag.CategoryId = new SelectList(_db.Categories.ToList(), "CategoryId", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemTinTuc(News TinTuc)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0].Length > 0) // kiểm tra xem tập có đc gửi từ file lên không 
                {
                    var file = files[0];
                    var FileName = file.FileName;
                    // upload ảnh vào thư mục wwwroot\\images\\category
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\LayoutAdmin\\images\\tintuc", FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        TinTuc.Image = "/LayoutAdmin/images/tintuc/" + FileName; // gán tên ảnh cho thuộc tinh Image
                    }

                }
                TinTuc.CreatedDate = DateTime.Now;

                _db.Add(TinTuc);
                _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(TinTuc);
        }

        //[HttpGet]
        //public IActionResult SuaTinTuc(int TinTuc)
        //{
        //    //ViewBag.CategoryId = new SelectList(_db.Categories.ToList(), "CategoryId", "Title",TinTuc);
        //    var tinTuc = _db.News.Find(TinTuc);
        //    ViewBag.CategoryId = new SelectList(_db.Categories.ToList(), "CategoryId", "Title", tinTuc.CategoryId);
        //    return View(tinTuc);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult SuaDanhMuc(int id, Category tinTuc)
        //{
        //    if (id != tinTuc.NewId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var file = files[0];
        //            var FileName = file.FileName;
        //            // upload ảnh vào thư mục wwwroot\\images\\category
        //            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\LayoutAdmin\\images\\tintuc", FileName);
        //            using (var stream = new FileStream(path, FileMode.Create))
        //            {
        //                file.CopyTo(stream);
        //                tinTuc.Image = "/LayoutAdmin/images/tintuc/" + FileName; // gán tên ảnh cho thuộc tinh Image
        //            }
        //            _db.Update(tinTuc);
        //            _db.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!NewExists(tinTuc.NewId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(tinTuc);
        //}

        //xoá
        //[HttpGet]
        //public IActionResult XoaTintuc(int maTin)
        //{
        //    var item = _db.News.Where(x=>x.NewId==maTin).ToList();
        //    if (item != null)
        //    {
        //        _db.News.Remove(item);
        //        _db.SaveChanges();
        //        return RedirectToAction("index");
        //    }
        //    return View(maTin);
        //}
    }
}
