using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace selling_online.Models.ViewModel
{
	public class CartItem
	{
		public int ProductID {  get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public string Image { get; set; }

		public decimal TotalPrice =>Quantity * Price;

	}
}