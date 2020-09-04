using System;
namespace Web_Core.Models
{
	public class Course
	{
		public Int32 Id { get; set;}
		public String Name { get; set;}
		public String Duration { get; set;}
		public String Cost { get; set;}
		public DateTime AddDate { get; set;}
		public Int32 Status { get; set;}
	}
}