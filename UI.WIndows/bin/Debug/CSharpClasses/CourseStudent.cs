using System;
namespace Web_Core.Models
{
	public class CourseStudent
	{
		public Int32 Id { get; set;}
		public Int32 StudentId { get; set;}
		public Int32 CourseId { get; set;}
		public DateTime StartDate { get; set;}
		public DateTime EndDate { get; set;}
		public Int32 Status { get; set;}
		public DateTime AddDate { get; set;}
	}
}