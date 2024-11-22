using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace selling_online.Models.ViewModel
{
	public class OrderHistoryVM
	{
		public int OrderId { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal TotalAmount { get; set; }
		public string Status { get; set; }
		public List<OrderDetail> OrderDetails { get; set; }
	}
}