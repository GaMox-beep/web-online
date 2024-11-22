using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using selling_online.Models;
using selling_online.Models.ViewModel;

namespace selling_online.Controllers
{
    public class CartController : Controller
    {
		private Database1Entities db = new Database1Entities();

        private CartService GetCartService()
        {
            return new CartService(Session);
        }
		// GET: Cart
		public ActionResult Index()
        {
             var cart=GetCartService().GetCart();
            return View(cart);
        }

        public ActionResult AddToCart(int id,int quantity =1)
        {
            var product=db.Products.Find(id);
            if (product != null)
            {
                var cartService = GetCartService();
                cartService.GetCart().AddItem(product.ProductID, product.Image,
                    product.ProductName, product.Price, quantity, product.Category.CategoryName);
            }
			Debug.WriteLine("User.Identity.Name: " + User.Identity.Name);

			return RedirectToAction("Index");
        }
        public ActionResult RemoveFromCart(int id)
        {
            var cartService=GetCartService();
                cartService.GetCart().RemoveItem(id);
                return RedirectToAction("Index");
   
        }
        public ActionResult ClearCart()
        {
            GetCartService().ClearCart();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            var cartService = GetCartService();
            cartService.GetCart().UpdateQuantity(id, quantity);
            return RedirectToAction("Index");
        }
    }
}