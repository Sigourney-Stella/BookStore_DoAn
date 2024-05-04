using BookStoreTM.Models.Entities;
using BookStoreTM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using CKFinder.Settings;
using BookStoreTM.Common;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public IActionResult Index(string name, int? page)
        {
            var products = _db.Products.Include(p => p.ProductCategory).ToList();

            var pageSize = 3;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var item = _db.Products.OrderByDescending(x => x.ProductId).ToPagedList(pageIndex, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(item);
        }

        //ckeditor
        public IActionResult UploadImage(List<IFormFile> files)
        {
            var filepath = "";
            foreach (IFormFile photo in Request.Form.Files)
            {
                string serverMapPath = Path.Combine(_env.WebRootPath, "product", photo.FileName);
                using (var stream = new FileStream(serverMapPath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }
                filepath = "https://localhost:44388/" + "product/" + photo.FileName;
            }
            return Json(new { url = filepath });
        }


        //thêm mới
        [HttpGet]
        public IActionResult ThemSanPham()
        {
            ViewBag.Publisher = new SelectList(_db.Publishers.ToList(), "PublisherId", "PublisherName");
            ViewBag.ProductCategory = new SelectList(_db.ProductCategories.ToList(), "ProductCategoryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemSanPham(Product model)
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
                        var nameImg = "SanPham" + ConvertVietNamToEnglish.LocDau(model.ProductName) + "." + tokens[tokens.Length - 1];
                        string result = nameImg.Replace(" ", "");
                        // upload ảnh vào thư mục wwwroot\\images\\category
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\LayoutAdmin\\images\\products", result);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            model.Images = "/LayoutAdmin/images/products/" + result; // gán tên ảnh cho thuộc tinh Image
                        }
                    }
                    model.CreatedDate = DateTime.Now;
                    if (string.IsNullOrEmpty(model.Alias))
                    model.Alias = BookStoreTM.Common.Filter.FilterChar(model.ProductName);
                    _db.Add(model);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message.ToString(); 
                ViewBag.Publisher = new SelectList(_db.Publishers.ToList(), "PublisherId", "PublisherName");
                ViewBag.ProductCategory = new SelectList(_db.ProductCategories.ToList(), "ProductCategoryId", "Name");
                return View(model);
            }
            ViewBag.Publisher = new SelectList(_db.Publishers.ToList(), "PublisherId", "PublisherName");
            ViewBag.ProductCategory = new SelectList(_db.ProductCategories.ToList(), "ProductCategoryId", "Name");
            return View(model);
        }

        [HttpGet]
        public IActionResult SuaSanPham(int model)
        {
            var sanpham = _db.Products.Find(model);
            ViewBag.Publisher = new SelectList(_db.Publishers.ToList(), "PublisherId", "PublisherName", sanpham.ProductId);
            ViewBag.ProductCategory = new SelectList(_db.ProductCategories.ToList(), "ProductCategoryId", "Name", sanpham.ProductCategoryId);
            return View(sanpham);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSanPham(int id, Product model)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0].Length > 0) // kiểm tra xem tập có đc gửi từ file lên không 
                {
                    var file = files[0];
                    var FileName = file.FileName;
                    string[] tokens = FileName.Split('.');
                    var nameImg = "SanPham" + ConvertVietNamToEnglish.LocDau(model.ProductName) + "." + tokens[tokens.Length - 1];
                    string result = nameImg.Replace(" ", "");
                    // upload ảnh vào thư mục wwwroot\\images\\category
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\LayoutAdmin\\images\\products", result);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        model.Images = "/LayoutAdmin/images/products/" + result; // gán tên ảnh cho thuộc tinh Image
                    }
                }
                model.UpdatedDate = DateTime.Now;
                model.Alias = BookStoreTM.Common.Filter.FilterChar(model.ProductName);

                _db.Update(model);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        //xoá
        public IActionResult Delete(int id)
        {
            var item = _db.Products.Find(id);
            if (item != null)
            {
                _db.Products.Remove(item);
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
                        var obj = _db.Products.Find(Convert.ToInt32(item));
                        _db.Products.Remove(obj);
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
            var item = _db.Products.Find(id);
            if (item != null)
            {
                item.IsActicve = !item.IsActicve;
                var entry = _db.Entry<Product>(item);
                entry.State = EntityState.Modified;
                _db.SaveChanges();
                return Json(new { success = true, isActicve = item.IsActicve });
            }
            return Json(new { success = false });
        }
        public IActionResult IsHome(int id)
        {
            var item = _db.Products.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                var entry = _db.Entry<Product>(item);
                entry.State = EntityState.Modified;
                _db.SaveChanges();
                return Json(new { success = true, IsHome = item.IsHome });
            }
            return Json(new { success = false });
        }
    }
}
