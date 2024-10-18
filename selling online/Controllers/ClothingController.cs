using selling_online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace selling_online.Controllers
{
    public class ClothingController : Controller
    {
		// GET: Clothing
		private Database1Entities db = new Database1Entities();
		public ActionResult ClothingDetails(int ProductID)
        {
           var product=db.Products.FirstOrDefault(p=>p.ProductID == ProductID);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
    }
}