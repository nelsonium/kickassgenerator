using System;
namespace Web_Core.Models
{
	public class Mark
	{
		public Int32 StudentId { get; set;}
		public Int32 Id { get; set;}
		public Int32 SubjectId { get; set;}
		public Int32 AssessmentTypeId { get; set;}
		public DateTime AssessmentDate { get; set;}
		public String ObtainedMark { get; set;}
		public DateTime AddDate { get; set;}
		public Int32 Status { get; set;}
	}
}