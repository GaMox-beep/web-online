using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace selling_online.Models.ViewModel
{
	public class Cart
	{
		private List<CartItem> items=new List<CartItem>();
		public IEnumerable<CartItem> Items => items;

		public void AddItem(int productId,string image,string productName,decimal price,int quantity,string category)
		{
			var existingItem=items.FirstOrDefault(i=>i.ProductID == productId);
			if (existingItem == null)
			{
				items.Add(new CartItem { ProductID = productId, Image = image, ProductName = productName, Price = price, Quantity = quantity });
			}
			else
			{
				existingItem.Quantity += quantity;
			}
			Debug.WriteLine($"Giỏ hàng hiện có {items.Count} món.");

		}

		public void RemoveItem(int productId)
		{
			items.RemoveAll(i=>i.ProductID == productId);	
		}
		public decimal TotalValue()
		{
			return items.Sum(i=>i.TotalPrice);
		}

		public void Clear()
		{
			items.Clear();
		}

		public void UpdateQuantity(int productId, int quantity)
		{
			var item=items.FirstOrDefault(i=>i.ProductID == productId);
			if (item != null)
			{
				item.Quantity = quantity;
			}
		}
	}
}