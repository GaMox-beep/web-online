using selling_online.Models;
using selling_online.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace selling_online.Controllers
{
	public class AccountController : Controller
	{
		private Database1Entities db = new Database1Entities();
		// GET: Account
		public ActionResult Login()
		{
			return View(); // Trả về view đăng nhập
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginVM model)
		{
			if (ModelState.IsValid) // Kiểm tra dữ liệu nhập vào có hợp lệ không
			{
				// Tìm người dùng trong cơ sở dữ liệu
				var user = db.Users.SingleOrDefault(u => u.UserName == model.Username && u.Password == model.Password && u.UserRole == "Customer");

				if (user != null) // Nếu tìm thấy người dùng
				{
					// Lưu thông tin người dùng vào session
					Session["Username"] = user.UserName;
					Session["UserRole"] = user.UserRole;

					// Lưu thông tin xác thực người dùng vào cookie
					FormsAuthentication.SetAuthCookie(user.UserName, false);
					Debug.WriteLine("Authentication Cookie Set: " + Request.Cookies[FormsAuthentication.FormsCookieName]?.Value);


					// Chuyển hướng đến trang chủ
					return RedirectToAction("Index", "Home");
				}
				else
				{
					// Hiển thị thông báo lỗi nếu không tìm thấy người dùng
					ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
				}
			}

			return View(model); // Trả về lại view đăng nhập nếu có lỗi
		}
		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			Session.Clear();
			return RedirectToAction("Login", "Account");
		}


		// Hiển thị trang Register
		public ActionResult Register()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterVM model)
		{
			if (ModelState.IsValid)
			{
				// Kiểm tra xem tên đăng nhập đã tồn tại chưa
				var existingUser = db.Users.SingleOrDefault(u => u.UserName == model.Username);
				if (existingUser != null)
				{
					ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại!");
					return View(model);
				}

				// Nếu chưa tồn tại thì tạo mới tài khoản
				var user = new User
				{
					UserName = model.Username,
					Password = model.Password, // Lưu ý: Nên mã hóa mật khẩu trước khi lưu
					UserRole = "Customer"
				};
				db.Users.Add(user);

				// Tạo thông tin khách hàng
				var customer = new Customer
				{
					CustomerName = model.CustomerName,
					CustomerEmail = model.CustomerEmail,
					CustomerPhone = model.CustomerPhone,
					Address = model.CustomerAddress,
					UserName = model.Username
				};
				db.Customers.Add(customer);

				// Lưu thay đổi vào CSDL
				db.SaveChanges();

				return RedirectToAction("Index", "Home");
			}

			return View(model);
		}
		public ActionResult Profile()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Account");
			}

			string username = User.Identity.Name;

			{
				var user = db.Users.SingleOrDefault(u => u.UserName == username);
				if (user == null)
				{
					return HttpNotFound();
				}
				var customer=db.Customers.SingleOrDefault(u => u.UserName == username);
				if (customer == null)
				{
					return HttpNotFound();
				}
				var ProfileVM = new ProfileVM
				{
					UserName = user.UserName,
					Name=customer.CustomerName,
					Email=customer.CustomerEmail,
					Phone=customer.CustomerPhone,
					City=customer.City,
					Address=customer.Address,
					Country=customer.Country,
				};
				return View(ProfileVM);
			}

		}
		[HttpPost]
		public ActionResult Profile(ProfileVM model)
		{
			if (ModelState.IsValid)
			{
				{
					// Lấy thông tin từ bảng users
					var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
					if (user == null)
					{
						return HttpNotFound();
					}

					// Lấy thông tin từ bảng customers
					var customer = db.Customers.FirstOrDefault(c => c.UserName == user.UserName);
					if (customer != null)
					{
						// Cập nhật thông tin từ ViewModel
						customer.CustomerName = model.Name;
						customer.CustomerPhone = model.Phone;
						customer.Address = model.Address;
						customer.City = model.City;
						customer.Country = model.Country;

						db.SaveChanges();
					}
				}
				return RedirectToAction("Profile");
			}
			return View(model);
		}
	}
}