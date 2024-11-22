using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using selling_online.Models;
using selling_online.Models.ViewModel;

namespace selling_online.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private Database1Entities db = new Database1Entities();

        // GET: Admin/Products
        public ActionResult Index(string searchTerm, decimal? minPrice, decimal? maxPrice, string sortOrder)
        {
            var model = new SearchProductVM();
            var products = db.Products.Include(p => p.Category);

            if(!string.IsNullOrEmpty(searchTerm) )
            {
                products = products.Where(p =>
                p.ProductName.Contains(searchTerm) ||
                p.Description.Contains(searchTerm) ||
                p.Category.CategoryName.Contains(searchTerm));             
            }
			if (minPrice.HasValue)
			{
				products = products.Where(p => p.Price >= minPrice.Value);
			}
			if (maxPrice.HasValue)
			{
				products = products.Where(p => p.Price <= maxPrice.Value);
			}
			switch (sortOrder)
			{
				case "name_asc":
					products = products.OrderBy(p => p.ProductName);
					break;
				case "name_desc":
					products = products.OrderByDescending(p => p.ProductName);
					break;
				case "price_asc":
					products = products.OrderBy(p => p.Price);
					break;
				case "price_desc":
					products = products.OrderByDescending(p => p.Price);
					break;
				default:
					products = products.OrderBy(p => p.ProductName);
					break;
			}
			model.Products=products.ToList();
            return View(model);
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,Description,Price,StockQuantity,CategoryID,Image")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,Description,Price,StockQuantity,CategoryID,Image")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
