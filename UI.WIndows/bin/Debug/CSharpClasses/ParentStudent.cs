using System;
namespace Web_Core.Models
{
	public class ParentStudent
	{
		public Int32 Id { get; set;}
		public Int32 StudentId { get; set;}
		public Int32 ParentId { get; set;}
		public String Relationship { get; set;}
		public DateTime AddDate { get; set;}
		public Int32 Status { get; set;}
	}
}