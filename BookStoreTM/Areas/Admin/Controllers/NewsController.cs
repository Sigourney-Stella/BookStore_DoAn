using BookStoreTM.Models.Entities;
using BookStoreTM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

using Microsoft.AspNetCore.Hosting;


namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public NewsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public IActionResult Index(string name, int page =1)
        {
            int limit = 2;
            var items = _db.News.OrderByDescending(x => x.NewsId).ToPagedList(page, limit);
            if (!string.IsNullOrEmpty(name))
            {
                items = _db.News.Where(x => x.Title.Contains(name)).ToPagedList(page, limit);
            }
            ViewBag.keyword = name;
            return View(items);
        }
        //ckeditor
        public IActionResult UploadImage(List<IFormFile> files)
        {
            var filepath = "";
            foreach (IFormFile photo in Request.Form.Files)
            {
                string serverMapPath = Path.Combine(_env.WebRootPath, "Images", photo.FileName);
                using (var stream = new FileStream(serverMapPath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }
                filepath = "https://localhost:44388/" + "Images/" + photo.FileName;
            }
            return Json(new { url = filepath });
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
            try
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
                    _db.SaveChanges(); 
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex )
            {
                ViewBag.error = ex.Message.ToString();
                return View(TinTuc);
            }
            return View(TinTuc);
        }

        [HttpGet]
        public IActionResult SuaTinTuc(int TinTuc)
        {
            //ViewBag.CategoryId = new SelectList(_db.Categories.ToList(), "CategoryId", "Title", TinTuc);
            var tinTuc = _db.News.Find(TinTuc);
            ViewBag.CategoryId = new SelectList(_db.Categories.ToList(), "CategoryId", "Title", tinTuc.CategoryId);
            return View(tinTuc);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaTinTuc(int id, News TinTuc)
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
                _db.Update(TinTuc);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(TinTuc);
        }

        //xoá
        public IActionResult Delete(int id)
        {
            var item = _db.News.Find(id);
            if (item != null)
            {
                _db.News.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        
        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = _db.News.Find(Convert.ToInt32(item));
                        _db.News.Remove(obj);
                        _db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        //bật tắt tin tức
        public IActionResult IsActicve(int id)
        {
            var item = _db.News.Find(id);
            if (item != null)
            {
                item.IsActicve = !item.IsActicve;
                _db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.SaveChanges();
                return Json(new { success = true, isActicve = item.IsActicve });
            }
            return Json(new { success = false });
        }
        //[HttpPost]
        //public IActionResult XoaTintuc(int maTin)
        //{
        //    var news =  _db.News.Find(maTin);
        //    if (news == null)
        //    {
        //        return NotFound();
        //    }

        //    _db.News.Remove(news);
        //    _db.SaveChanges();
        //    return RedirectToAction(nameof(Index));
    }
}
