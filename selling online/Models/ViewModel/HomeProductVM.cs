using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace selling_online.Models.ViewModel
{
	public class HomeProductVM
	{
		public string SearchTerm { get; set; }
		public List<Product> FeaturedProducts { get; set; }

		public int PageNumber { get; set; }
		public int PageSize { get; set; } = 10;

		public PagedList.IPagedList<Product> NewProducts { get; set; }
	}
}