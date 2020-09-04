using System.Collections.Generic;

namespace KickAssSystemGenerator
{
	public class DataTableObject
	{
		public string TableName { get; set; }
		public string PrimaryKey { get; set; }
		public string PrimaryKeyDataType { get; set; }
		public List<DataRow> RowList { get; set; }
		public List<string> ForeignKeys { get; set; }

		public DataTableObject()
		{
		}
	}
}