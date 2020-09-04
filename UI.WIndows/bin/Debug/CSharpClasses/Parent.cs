using System;
namespace Web_Core.Models
{
	public class Parent
	{
		public Int32 Id { get; set;}
		public Int32 UserId { get; set;}
		public Int32 AddressId { get; set;}
		public String Name { get; set;}
		public String Email { get; set;}
		public String ContactNumber { get; set;}
		public String Address { get; set;}
		public String Profession { get; set;}
		public Int32 Status { get; set;}
	}
}