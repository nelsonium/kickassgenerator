using System;
namespace Web_Core.Models
{
	public class SystemUser
	{
		public Int32 Id { get; set;}
		public String Username { get; set;}
		public DateTime LastActive { get; set;}
		public String UserPassword { get; set;}
		public DateTime AddDate { get; set;}
	}
}