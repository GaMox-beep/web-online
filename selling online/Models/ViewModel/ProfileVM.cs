using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace selling_online.Models.ViewModel
{
	public class ProfileVM
	{
		public string UserName { get; set; }
		public string Name {  get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string Email {  get; set; }
		public string Phone { get; set; }
	}
}