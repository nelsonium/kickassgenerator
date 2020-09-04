using System;

namespace KickAssSystemGenerator
{
	/// <summary>
	/// This will contain data table row definition information
	/// </summary>
	public class DataRow
	{
		public string ColumnName { get; set; }
		public object DataType { get; set; }
		public string WholeDataType { get; set; }
		public Boolean IsNull { get; set; }
		public object Length { get; set; }
		public string TableRef { get; set; }
		public string TableRefID { get; set; }
		public string TablePrimaryKey { get; set; }
	}
}