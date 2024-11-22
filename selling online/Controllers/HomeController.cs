
using selling_online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using selling_online.Models.ViewModel;
using System.Web.UI;
using System.Diagnostics;

namespace selling_online.Controllers
{
	public class HomeController : Controller
	{
		private Database1Entities db = new Database1Entities();
		//Get: Home/ProductDetails/5
		public ActionResult ProductDetails(int? id, int? quantity, int? page)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Product pro = db.Products.Find(id);
			if (pro == null)
			{
				return HttpNotFound();
			}

			// lấy tất cả các sản phẩm cùng danh mục
			var products = db.Products.Where(p => p.CategoryID == pro.CategoryID && p.ProductID != pro.ProductID).AsQueryable();

			ProductDetailsVM model = new ProductDetailsVM();
			// Đoạn code liên quan tới phân trang
			// Lấy số trang hiện tại (mặc định là trang 1 nếu không có giá trị)
			int pageNumber = page ?? 1;
			int pageSize = model.PageSize;  // Số sản phẩm mỗi trang
			model.product = pro;
			model.RelatedProducts = products.OrderBy(p => p.ProductID).Take(8).ToPagedList(pageNumber, pageSize);
			model.TopProducts = products.OrderByDescending(p => p.OrderDetails.Count()).Take(8).ToPagedList(pageNumber, pageSize);

			if (quantity.HasValue)
			{
				model.quantity = quantity.Value;
			}

			return View(model);
		}


		public PartialViewResult _PVCollection()
		{
			var collectionProducts = db.Products
				.Where(p => p.ProductID >= 1 && p.ProductID <= 3)
				.ToList();

			return PartialView("_PVCollection", collectionProducts);
		}
		public ActionResult Index2(string searchTerm, int? page)
		{
			var model = new HomeProductVM();
			var products = db.Products.AsQueryable();
			if (!string.IsNullOrEmpty(searchTerm))
			{
				model.SearchTerm = searchTerm;
				products = products.Where(p => p.ProductName.Contains(searchTerm) ||
										p.Description.Contains(searchTerm) ||
										p.Category.CategoryName.Contains(searchTerm));
			}
			int pageNumber = page ?? 1;
			int pageSize = 6;
			model.FeaturedProducts = products.OrderByDescending(p => p.OrderDetails.Count()).Take(10).ToList();

			model.NewProducts = products.OrderBy(p => p.OrderDetails.Count()).Take(20).ToPagedList(pageNumber, pageSize);

			return View(model);
		}

		public ActionResult Index(int? categoryId, int page = 1, int pageSize =6 )
		{

			IQueryable<Product> items;

			if (categoryId.HasValue)
			{
				items = db.Products.Where(p => p.CategoryID == categoryId.Value);
			}
			else
			{
				items = db.Products;
			}

			items = items.OrderBy(p => p.ProductID); // Sắp xếp sản phẩm

			var pagedItems = items.ToPagedList(page, pageSize); // Chuyển đổi sang trang
			ViewBag.Items = pagedItems; // Gán vào ViewBag
			ViewBag.SelectedCategoryId = categoryId;
			Debug.WriteLine("Current User: " + User.Identity.Name); // Kiểm tra giá trị User.Identity.Name

			return View();
		}





		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";
			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
		public ActionResult Login()
		{
			return View();
		}
		public ActionResult Register()
		{
			return View();
		}
	}
}