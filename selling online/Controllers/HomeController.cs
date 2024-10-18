using selling_online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace selling_online.Controllers
{
	public class HomeController : Controller
	{
		private Database1Entities db = new Database1Entities();

		public ActionResult Index2()
		{
			var selectedIds = new List<int> { 1, 2, 3 };
			var product = db.Products.Where(p => selectedIds.Contains(p.ProductID)).ToList() ;
			return View(product);
		}

		public ActionResult Index(int? categoryId)
		{
			List<Product> items;

			if (categoryId.HasValue)
			{
				items = db.Products.Where(p => p.CategoryID == categoryId.Value).ToList(); // Lọc sản phẩm theo CategoryId
			}
			else
			{
				items = db.Products.ToList(); // Lấy tất cả sản phẩm nếu không có CategoryId
			}

			ViewBag.Items = items; // Chuyển danh sách sản phẩm đến View
			ViewBag.SelectedCategoryId = categoryId; // Lưu lại CategoryId để hiển thị trên view
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