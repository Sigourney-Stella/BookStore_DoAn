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
using BookStoreTM.Common;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            ViewBag.Id = id;
            return View();
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
            return RedirectToAction("Index", "Product");
        }
        //public IActionResult Index(int id)
        //{
        //    var productImage = _db.ProductImages.Where(x => x.ProductId == id).ToList();
        //    return View(productImage);
        //}


        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Create(int productId, IFormFileCollection productImages)
        //{

        //    if (productImages != null && productImages.Count > 0)
        //    {
        //        foreach (var formFile in productImages)
        //        {
        //            if (formFile.Length > 0)
        //            {
        //                var filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", formFile.FileName);
        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    formFile.CopyToAsync(stream);
        //                }

        //                var productImage = new ProductImage
        //                {
        //                    ProductImgName = formFile.FileName,
        //                    Url = "/images/" + formFile.FileName,
        //                    ProductId = productId
        //                };

        //                _db.ProductImages.Add(productImage);
        //            }
        //        }
        //        _db.SaveChanges();
        //    }
        //    return View();
        //}
    }
}
