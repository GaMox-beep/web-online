using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace selling_online.Controllers
{
    public class AccountController : Controller
    {
		// GET: Account
		public ActionResult Login()
		{
			return View();
		}

		// Hiển thị trang Register
		public ActionResult Register()
		{
			return View();
		}
	}
}