using System;
namespace Web_Core.Models
{
	public class Student
	{
		public Int32 Id { get; set;}
		public Int32 UserId { get; set;}
		public String Name { get; set;}
		public String Surname { get; set;}
		public DateTime Birthday { get; set;}
		public Int32 Gender { get; set;}
		public String BloodGroup { get; set;}
		public Int32 AddressId { get; set;}
		public String ContactNumber { get; set;}
		public String Email { get; set;}
		public String StudentNumber { get; set;}
		public DateTime AddDate { get; set;}
		public Int32 Status { get; set;}
	}
}