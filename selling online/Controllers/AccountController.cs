using selling_online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace selling_online.Controllers
{
	public class AccountController : Controller
	{
		private Database1Entities db = new Database1Entities();
		// GET: Account
		public ActionResult Login()
		{
			return View();
		}
		public ActionResult AuthenLogin(Customer _user)
		{
			try
			{
				//var check_ID=db.Customers.Where(s=>s.CustomerID==_user.CustomerID).FirstOrDefault();
				var check_Name = db.Customers.Where(s => s.UserName == _user.UserName).FirstOrDefault();
				var check_Pass = db.Customers.Where(s => s.Password == _user.Password).FirstOrDefault();
				if (check_Pass == null || check_Name == null)
				{
					//if (check_ID == null)
					//ViewBag.ErrorID = "Id not match";
					if (check_Name == null)
						ViewBag.ErrorName = "Name not match";
					if (check_Pass == null)
						ViewBag.ErrorPass = "Password not match";
					return View("Login");
				}

				else
				{
					return RedirectToAction("Index", "Home");
				}
			}

			catch
			{
				return View("Login");

			}
		}

		// Hiển thị trang Register
		public ActionResult Register()
		{
			return View();
		}
		public ActionResult AuthenRegister()
		{
			return View();

		}
	}
}