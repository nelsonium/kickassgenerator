using System;
namespace Web_Core.Models
{
	public class CourseSubject
	{
		public Int32 Id { get; set;}
		public Int32 CourseId { get; set;}
		public Int32 SubjectId { get; set;}
		public Int32 Status { get; set;}
		public DateTime AddDate { get; set;}
	}
}