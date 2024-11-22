using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace selling_online.Models.ViewModel
{
	public class SearchProductVM
	{
	public string SearchTerm { get; set; }

        public string SortOrder { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

  

		public List<Product> Products { get; set; }
	}
}