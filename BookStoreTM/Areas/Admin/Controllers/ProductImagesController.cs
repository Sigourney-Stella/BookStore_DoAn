using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStoreTM.Models;
using BookStoreTM.Models.Entities;
using BookStoreTM.Models.EF;
using Microsoft.AspNetCore.Authorization;
using BookStoreTM.Common;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductImagesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductImagesController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _db = context;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var appDbContext = _db.ProductImages.Include(p => p.Product).Where(x=>x.ProductId == id).ToList();
            ViewBag.Id = id;
            return View(appDbContext);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int productId, IFormFileCollection productImages)
        {
            if (productImages != null && productImages.Count > 0)
            {
                foreach (var formFile in productImages)
                {
                    if (formFile.Length > 0)
                    {
                        var FileName = formFile.FileName;
                        string[] tokens = FileName.Split('.');
                        var nameImg = "Sach" + ConvertVietNamToEnglish.LocDau(productId.ToString()) + "." + tokens[tokens.Length - 1];
                        string result = nameImg.Replace(" ", "");
                        var filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", result);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        var productImage = new ProductImage
                        {
                            ProductImgName = result,
                            Url = "/images/" + formFile.FileName,
                            ProductId = productId,
                            IsDefault = false
                        };
                        _db.ProductImages.Add(productImage);
                    }
                }
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Create", "ProductImages");
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
                        var obj = _db.ProductImages.Find(Convert.ToInt32(item));
                        _db.ProductImages.Remove(obj);
                        _db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
