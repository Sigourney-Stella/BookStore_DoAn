﻿using BookStoreTM.Models.Entities;
using BookStoreTM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

using Microsoft.AspNetCore.Hosting;
using BookStoreTM.Common;
using Microsoft.AspNetCore.Authorization;


namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class NewsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public NewsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public IActionResult Index(string name, int? page)
        {
            //var products = _db.News.Include(p => p.Category).ToList();

            var pageSize = 3;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var item = _db.News.OrderByDescending(x => x.NewsId).ToPagedList(pageIndex, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(item);
        }
        
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
            //ViewBag.CategoryId = new SelectList(_db.Categories.ToList(), "CategoryId", "Title");
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
                        string[] tokens = FileName.Split('.');
                        var nameImg = "Tintuc" + ConvertVietNamToEnglish.LocDau(TinTuc.Title) +"."+ tokens[tokens.Length-1];
                        string result = nameImg.Replace(" ", "");
                        // upload ảnh vào thư mục wwwroot\\images\\category
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\LayoutAdmin\\images\\tintuc", result);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            TinTuc.Image = "/LayoutAdmin/images/tintuc/" + result; // gán tên ảnh cho thuộc tinh Image
                        }
                    }
                    TinTuc.CreatedDate = DateTime.Now;
                    TinTuc.Alias = BookStoreTM.Common.Filter.FilterChar(TinTuc.Title);
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
            var tinTuc = _db.News.Find(TinTuc);
            tinTuc.Image = "wwwroot\\LayoutAdmin\\images\\tintuc";
            //ViewBag.CategoryId = new SelectList(_db.Categories.ToList(), "CategoryId", "Title", tinTuc.CategoryId);
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
                TinTuc.Alias = BookStoreTM.Common.Filter.FilterChar(TinTuc.Title);
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
                var entry = _db.Entry<News>(item);
                entry.State = EntityState.Modified;
                //_db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.SaveChanges();
                return Json(new { success = true, isActicve = item.IsActicve });
            }
            return Json(new { success = false });
        }
    }
}
