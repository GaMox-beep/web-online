using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace selling_online.Models.ViewModel
{
	// Lớp này lưu trữ thông tin của form đăng ký tài khoản khách hàng
	public class RegisterVM
	{
		// Tên đăng nhập
		[Required]
		[Display(Name = "Tên đăng nhập")]
		public string Username { get; set; }

		// Mật khẩu
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Mật khẩu")]
		public string Password { get; set; }

		// Xác nhận mật khẩu
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Xác nhận mật khẩu")]
		[Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
		public string ConfirmPassword { get; set; }

		// Họ tên
		[Required]
		[Display(Name = "Họ tên")]
		public string CustomerName { get; set; }

		// Số điện thoại
		[Required]
		[Display(Name = "Số điện thoại")]
		[DataType(DataType.PhoneNumber)]
		public string CustomerPhone { get; set; }

		// Email
		[Required]
		[Display(Name = "Email")]
		[DataType(DataType.EmailAddress)]
		public string CustomerEmail { get; set; }

		// Địa chỉ
		[Required]
		[Display(Name = "Địa chỉ")]
		public string CustomerAddress { get; set; }
	}
}