using System;
namespace Web_Core.Models
{
	public class Book
	{
		public Int32 Id { get; set;}
		public String Name { get; set;}
		public String ISBN { get; set;}
		public String Description { get; set;}
		public String Author { get; set;}
		public String Price { get; set;}
		public DateTime AddDate { get; set;}
		public Int32 Status { get; set;}
	}
}