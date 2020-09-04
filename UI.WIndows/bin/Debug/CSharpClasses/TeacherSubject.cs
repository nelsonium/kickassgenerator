using System;
namespace Web_Core.Models
{
	public class TeacherSubject
	{
		public Int32 Id { get; set;}
		public Int32 TeacherId { get; set;}
		public Int32 SubjectId { get; set;}
		public DateTime StartDate { get; set;}
		public DateTime EndDate { get; set;}
		public Int32 Status { get; set;}
		public DateTime AddDate { get; set;}
	}
}