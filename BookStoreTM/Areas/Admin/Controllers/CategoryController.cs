using BookStoreTM.Models.Entities;
using BookStoreTM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using X.PagedList;

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

        //public IActionResult Index(int page = 1)
        //{
        //    int limit = 2; // số lượng bản ghi trên trang
        //    var items = _db.Categories.ToPagedList(page, limit);
        //    return View(items);
        //}
        public IActionResult Index(string name, int page = 1)
        {
            int limit = 2;
            var items = _db.Categories.OrderByDescending(x => x.CategoryId).ToPagedList(page, limit);
            if (!string.IsNullOrEmpty(name))
            {
                items = _db.Categories.Where(x => x.Title.Contains(name)).ToPagedList(page, limit);
            }
            ViewBag.keyword = name;
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
                loai.CreatedDate = DateTime.Now;
                _db.Update(loai);
                //_db.Entry(loai).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(loai);
        }

        //xoá
        public IActionResult Delete(int id)
        {
            var item = _db.Categories.Find(id);
            if (item != null)
            {
                _db.Categories.Remove(item);
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
                        var obj = _db.Categories.Find(Convert.ToInt32(item));
                        _db.Categories.Remove(obj);
                        _db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        ////xoá
        //public IActionResult XoaTinTuc(int maLoai)
        //{
        //    var item = _db.Categories.Find(maLoai);
        //    if (item != null)
        //    {
        //        _db.Categories.Remove(item);
        //        _db.SaveChanges();
        //        return RedirectToAction("index");
        //    }
        //    return View(maLoai);
        //}
    }
}
