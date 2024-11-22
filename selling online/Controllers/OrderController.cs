using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using selling_online.Models;
using selling_online.Models.ViewModel;

namespace selling_online.Controllers
{
	public class OrderController : Controller
	{
		private Database1Entities db = new Database1Entities(); // GET: Order
		public ActionResult Index()
		{
			return View();
		}
		// GET: Order/Checkout
		public ActionResult OrderSuccess()
		{
			return View();
		}

		public ActionResult Checkout()
		{
			//Kiểm tra giỏ hàng trong session,
			//nếu giỏ hàng rỗng hoặc không có sản phẩm thì chuyển hướng về trang chủ
			var cartService =new CartService(Session);
			var cart = cartService.GetCart();
			
			if (cart == null || !cart.Items.Any())
			{
				Debug.WriteLine("User.Identity.Name: " + User.Identity.Name);

				return RedirectToAction("Index", "Home");
			}


			// Lấy tên người dùng từ User.Identity
			var user = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
			if (user == null)
			{
				Debug.WriteLine("User.Identity.Name: " + User.Identity.Name);

				return RedirectToAction("Login", "Account");
			}

			// Tìm khách hàng trong bảng Customers dựa vào CustomerID
			var customer = db.Customers.SingleOrDefault(c => c.UserName == user.UserName);
			if (customer == null)
			{
				return RedirectToAction("Register", "Account"); // Hoặc xử lý lỗi khác​14:04/-strong/-heart:>:o:-((:-h Xem trước khi gửiThả Files vào đây để xem lại trước khi gửi
			}
			var model = new CheckoutVM
			{
				CartItems = cart.Items.ToList(),
				TotalAmount = cart.Items.Sum(item => item.TotalPrice),
				OrderDate = DateTime.Now,
				ShippingAddress = (string)customer.Address,
				CustomerID = customer.CustomerID,
				Username = customer.UserName
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]

		public ActionResult Checkout(CheckoutVM model)
		{
			if (ModelState.IsValid)
			{
				var cartService = new CartService(Session);
				var cart = cartService.GetCart();

				if (cart == null || !cart.Items.Any())
				{
					Debug.WriteLine("Giỏ hàng trống hoặc không có sản phẩm.");

					return RedirectToAction("Index", "Home");
				}

				// Lấy tên người dùng từ User.Identity
				var user = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
				if (user == null)
				{
					return RedirectToAction("Login", "Account");
				}			
				// Tìm khách hàng trong bảng Customers dựa vào CustomerID
				var customer = db.Customers.SingleOrDefault(c => c.UserName == user.UserName);
				if (customer == null)
				{
					return RedirectToAction("Register", "Account"); // Hoặc xử lý lỗi khác​14:04/-strong/-heart:>:o:-((:-h Xem trước khi gửiThả Files vào đây để xem lại trước khi gửi
				}
				//Nếu người dùng chọn thanh toán bằng Paypal, sẽ điều hướng tới trang PaymentWithPaypal
				if (model.PaymentMethod == "Paypal")
				{

					return RedirectToAction("PaymentWithPaypal", "PayPal", model);
				}

				// Thiết Lập PaymentStatus dựa trên PaymentMethod
				string paymentStatus = "Chưa thanh toán";
				switch (model.PaymentMethod)
				{
					case "Tiền mặt": paymentStatus = "Thanh toán tiền mặt"; break;
					case "Paypal": paymentStatus = "Thanh toán paypal"; break;
					case "Mua trước trả sau": paymentStatus = "Trả góp"; break;
					default: paymentStatus = "Chua thanh toán"; break;
				}



				//Tạo đơn hàng và ti tiết đơn hàng liên quan
				var order = new Order
				{
					CustomerID = customer.CustomerID,
					OrderDate = DateTime.Now,
					TotalAmount = model.TotalAmount,
					OrderStatus = paymentStatus,
					PaymentMethod = model.PaymentMethod,
					DeliveryMethod = model.ShippingMethod,
					AddressDelivery = model.ShippingAddress,
					OrderDetails = cart.Items.Select(item => new OrderDetail
					{
						ProductID = item.ProductID,
						Quantity = item.Quantity,
						UnitPrice = item.Price,
						TotalPrice = item.TotalPrice
					}).ToList()
				};

				//Lưu đơn hàng vào CSDL
				db.Orders.Add(order);
				try
				{
					db.SaveChanges(); // Lưu thay đổi vào database
				}
				catch (DbEntityValidationException e)
				{
					foreach (var validationError in e.EntityValidationErrors)
					{
						foreach (var error in validationError.ValidationErrors)
						{
							// In ra thông tin chi tiết lỗi, như tên thuộc tính và thông báo lỗi
							Debug.WriteLine("Property: " + error.PropertyName);
							Debug.WriteLine("Error: " + error.ErrorMessage);
						}
					}
				}
				// Xóa giỏ hàng sau khi đặt hàng thành công
				Session["Cart"] = null;
				// Điều hướng tới trang Xác nhận đơn hàng
				//return RedirectToAction("Index", "Home");
				return RedirectToAction("OrderSuccess", new { id = order.OrderID });
			}
			return View(model);
		}
		public ActionResult OrderHistory()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Account");
			}

			string username = User.Identity.Name;

			{
				// Lấy danh sách đơn hàng và chi tiết
				var orders = db.Orders
					.Where(o => o.Customer.UserName == username)
					.OrderByDescending(o => o.OrderDate)
					.Select(o => new OrderHistoryVM
					{
						OrderId = o.OrderID,
						OrderDate = o.OrderDate,
						TotalAmount = o.TotalAmount,
						OrderDetails = db.OrderDetails
							.Where(od => od.OrderID == o.OrderID)
							.ToList()  // Lấy trực tiếp OrderDetail từ DB
					}).ToList();

				return View(orders);
			}
		}
	}
}


