using System;
using System.Collections.Generic;
using System.IO;

namespace KickAssSystemGenerator
{
	/// <summary>
	/// This will read a SQL file and convert to usable object
	/// </summary>
	public class DBInterpreter
	{
		private DataTableObject objTable { get; set; }

		/// <summary>
		/// Read sql file and convert it to DataTableObject
		/// </summary>
		/// <param name="sqlLocation">Full file location</param>
		/// <returns></returns>
		public List<DataTableObject> Get(string sqlLocation)
		{
			if (!File.Exists(sqlLocation))
			{
				throw new Exception("Specified MySql file does not exists");
			}
			var list = new List<DataTableObject>();
			var fileLines = File.ReadAllLines(sqlLocation);
			var dtCount = File.ReadAllText(sqlLocation).Split(new string[] { "create table" }, StringSplitOptions.None).Length;
			Boolean canStartCollecting = false;

			foreach (string line in fileLines)
			{
				if (!String.IsNullOrEmpty(line))
				{
					if (line.ToLower().Contains(("create table").ToLower()))
					{
						objTable = new DataTableObject();
						objTable.RowList = new List<DataRow>();
						var temp = line.TrimStart().Split(' ');
						objTable.TableName = temp[2];

						canStartCollecting = true;
					}
					if (canStartCollecting)
					{
						var str = line.TrimStart().Split(' ');
						if (!line.ToLower().Contains(("create table").ToLowerInvariant()) && !line.Contains(");") && line.Trim() != ")" && line.Trim() != "(")
						{
							if (line.ToLower().Contains(("AUTO_INCREMENT").ToLower()))
							{
								objTable.PrimaryKey = str[0];
								objTable.PrimaryKeyDataType = str[1];
							}
							if (str[0].ToLower() != "constraint" && str[0].Length != 0)
							{
								var row = new DataRow();
								row.ColumnName = str[0];
								row.WholeDataType = str[1];
								if (str[1].ToLower().Contains(("varchar").ToLowerInvariant()))
								{
									row.DataType = "varchar";
									row.Length = str[1].Split('(', ')')[1];
								}
								else if (str[1].ToLower().Contains(("decimal").ToLowerInvariant()))
								{
									row.DataType = "varchar";
									row.Length = str[1].Split('(', ')')[1];
								}
								else { row.DataType = str[1]; }
								if (str.Length > 2) { row.IsNull = false; } else { row.IsNull = true; }
								objTable.RowList.Add(row);
							}
							else
							{
								if (str.Length > 5)
								{
									var row = new DataRow();
									row.ColumnName = str[1];
									foreach (var i in str[6])
									{
										if (i != '(') { row.TableRef += i; } else { break; }
									}
									row.WholeDataType = str[1];
									row.TableRefID = str[4].Split('(', ')')[1];
									objTable.RowList.Add(row);
								}
							}
						}
						else
						{
							if (line.Contains(");")) { list.Add(objTable); canStartCollecting = false; objTable = null; }
						}
					}
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				var obj = list[i];
				for (int f = 0; f < list.Count; f++)
				{
					var trr = list[f].TableName;
					for (int e = 0; e < list[f].RowList.Count; e++)
					{
						if ((list[i].TableName + list[i].PrimaryKey == list[f].RowList[e].ColumnName) || ("Parent" + list[i].TableName + list[i].PrimaryKey == list[f].RowList[e].ColumnName))
						{
							list[f].RowList[e].TableRef = list[i].TableName;
						}
					}
				}
			}
			return list;
		}
	}
}