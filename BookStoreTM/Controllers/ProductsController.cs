﻿using BookStoreTM.Models;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BookStoreTM.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(AppDbContext db, ILogger<ProductsController> logger)
        {
            _db = db;
            _logger = logger;
        }
        public IActionResult Index(int? id, string name, int page = 1)
        {
            int limit = 10;
            var products = _db.Products.Include(p => p.ProductCategory).ToPagedList(page, limit);
            if (!string.IsNullOrEmpty(name))
            {
                //tìm kiếm theo tên sách hoặc tên nhà xuất bản
                products = _db.Products.Where(x => x.ProductName.Contains(name) || x.Author.Contains(name)).ToPagedList(page, limit);
            }
            ViewBag.keyword = name;
            return View(products);
        }
        public IActionResult Detail(int? id)
        {
            var products = _db.Products.Include(p => p.ProductCategory).Include(b=>b.Publisher).ToList();
            var item = _db.Products.Find(id);
            return View(item);
        }

        public ActionResult ProductCategory(string alias, int id)
        {
            var items = _db.Products.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = _db.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Name;
            }

            ViewBag.CateId = id;
            return View(items);
        }
    }
}
