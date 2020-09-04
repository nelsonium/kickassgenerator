using System;
using System.Collections.Generic;
using System.IO;

namespace KickAssSystemGenerator
{
	/// <summary>
	/// This will generate stored procedures for MySql for each table in sql file
	/// </summary>
	public class MySQLGenerator
	{
		public MySQLGenerator(List<DataTableObject> lst, string path)
		{
			string final = "";
			foreach (var item in lst)
			{
				string str = "";
				final += "\n\n/*-------------------*/\n\n";
				final += str = GenerateFetchQuery(item);

				final += "\n\n/*-------------------*/\n\n";
				final += str = GenerateInsert(item);

				final += "\n\n/*-------------------*/\n\n";
				final += str = GenerateUpdate(item);

				final += "\n\n/*-------------------*/\n\n";
				final += str = GenerateDelete(item);

				final += "\n\n/*-------------------*/\n\n";
				final += str = GenerateFetchOne(item, item.PrimaryKey, "int");

				foreach (var row in item.RowList)
				{
					if (!string.IsNullOrEmpty(row.TableRefID))
					{
						final += str = GenerateFetchOne(item, row.TableRefID, "int");
					}
				}

				CreateSaveFile(path + "\\StoredAll.sql", final);
			}
		}

		/// <summary>
		/// Select all the content from a table
		/// </summary>
		/// <param name="dataTable"></param>
		/// <returns></returns>
		private string GenerateFetchQuery(DataTableObject dataTable)
		{
			string str = "";
			str += "DELIMITER $$\n";
			str += "DROP PROCEDURE IF EXISTS sp_" + dataTable.TableName + "_Select$$\n";
			str += "CREATE PROCEDURE sp_" + dataTable.TableName + "_Select()\n";
			str += "LANGUAGE SQL\n";
			str += "DETERMINISTIC\n";
			str += "SQL SECURITY DEFINER\n";
			str += "COMMENT 'A procedure to select all'\n";
			str += "BEGIN\n";
			str += "SELECT * FROM " + dataTable.TableName + ";\n";
			str += "END$$\n\n";

			return str;
		}

		/// <summary>
		/// Get one result stored procedure
		/// </summary>
		/// <param name="dataTable"></param>
		/// <param name="attribute"></param>
		/// <param name="datatype"></param>
		/// <returns></returns>
		private string GenerateFetchOne(DataTableObject dataTable, string attribute, string datatype)
		{
			string str = "";
			str += "DELIMITER $$\n";
			str += "DROP PROCEDURE IF EXISTS sp_" + dataTable.TableName + "_BY_" + attribute + "$$\n";
			str += "CREATE PROCEDURE sp_" + dataTable.TableName + "_BY_" + attribute + "(\n";
			str += "IN p" + attribute + " " + datatype + "\n)\n";
			str += "LANGUAGE SQL\n";
			str += "DETERMINISTIC\n";
			str += "SQL SECURITY DEFINER\n";
			str += "COMMENT 'A procedure to select all'\n";
			str += "BEGIN\n";
			str += "SELECT * FROM " + dataTable.TableName + " WHERE " + attribute + " = p" + attribute + ";\n";
			str += "END$$\n\n";
			return str;
		}

		/// <summary>
		/// Generate insert into a table stored procedure
		/// </summary>
		/// <param name="dataTable"></param>
		/// <returns></returns>
		private string GenerateInsert(DataTableObject dataTable)
		{
			string str = "";
			str += "DELIMITER $$\n";
			str += "DROP PROCEDURE IF EXISTS sp_Insert_" + dataTable.TableName + "$$\n";
			str += "CREATE PROCEDURE sp_Insert_" + dataTable.TableName + "(\n";
			str += DeclareNoAuto(dataTable);
			str += ")\n";
			str += "BEGIN\n";

			str += "INSERT INTO " + dataTable.TableName + "(\n";
			str += ColumnsNoAuto(dataTable, "");
			str += ")\n";
			str += "VALUES(\n";
			str += ColumnsNoAuto(dataTable, "p");
			str += ");\nSELECT LAST_INSERT_ID();\n";
			str += "END$$";

			return str;
		}

		/// <summary>
		/// Generate update table by primary stored procedure
		/// </summary>
		/// <param name="dataTable"></param>
		/// <returns></returns>
		private string GenerateUpdate(DataTableObject dataTable)
		{
			string str = "DELIMITER $$\n";
			str += "DROP PROCEDURE IF EXISTS sp_Update_" + dataTable.TableName + "$$\n";
			str += "CREATE PROCEDURE sp_Update_" + dataTable.TableName + "(";
			str += DeclareNoAuto(dataTable);
			str += ")\n";
			str += "BEGIN\n";

			str += "UPDATE " + dataTable.TableName + "\nSET\n";
			str += Set(dataTable, "p");
			str += "\nWHERE " + dataTable.PrimaryKey + " = p" + dataTable.PrimaryKey;
			str += ";\nEND$$";

			return str;
		}

		/// <summary>
		/// Generate delete record by primary id stored procedure
		/// </summary>
		/// <param name="dataTable"></param>
		/// <returns></returns>
		private string GenerateDelete(DataTableObject dataTable)
		{
			string declare = "", set = "";
			declare += "IN pRecordStatus int,\n";
			set = "RecordStatus = pRecordStatus\n";

			string str = "DELIMITER $$\n";
			str += "DROP PROCEDURE IF EXISTS sp_Delete_" + dataTable.TableName + "$$\n";
			str += "CREATE PROCEDURE sp_Delete_" + dataTable.TableName + "(\n";
			str += "IN pRecordStatus int";
			str += ")\n";
			str += "BEGIN\n";

			str += "UPDATE " + dataTable.TableName + "\nSET\n";
			str += set;
			str += "\nWHERE " + dataTable.PrimaryKey + " = p" + dataTable.PrimaryKey + ";";
			str += "\nEND$$";

			return str;
		}

		/// <summary>
		/// Set column value parameter
		/// </summary>
		/// <param name="dataTable"></param>
		/// <param name="prefix"></param>
		/// <returns></returns>
		private string Set(DataTableObject dataTable, string prefix)
		{
			string str = "";
			foreach (var row in dataTable.RowList)
			{
				str += row.ColumnName + " = " + prefix + row.ColumnName + ",\n";
			}
			str = str.Substring(0, str.Length - 2);
			return str;
		}

		/// <summary>
		/// Resolve default values: Some values can not be updated, such as record create date
		/// </summary>
		/// <param name="dataTable"></param>
		/// <param name="prefix"></param>
		/// <returns></returns>
		private string ColumnsNoAuto(DataTableObject dataTable, string prefix)
		{
			string str = "";
			foreach (var row in dataTable.RowList)
			{
				var isForeign = row.ColumnName.StartsWith("FK_", StringComparison.InvariantCulture);
				var isPrimary = row.ColumnName.EndsWith("_PK", StringComparison.InvariantCulture);
				var datebool = row.ColumnName.ToLower().Contains("adddate");
				if (!isPrimary && !isForeign && !datebool && row.DataType != null && row.DataType.ToString() != "timestamp" && dataTable.PrimaryKey != row.ColumnName)
				{
					str += prefix + "" + row.ColumnName + ",\n";
				}
			}
			if (!String.IsNullOrEmpty(str))
			{
				str = str.Substring(0, str.Length - 2);
			}
			return str;
		}

		/// <summary>
		/// Resolve default values: Some values can not be updated, such as record create date
		/// </summary>
		/// <param name="dataTable"></param>
		/// <returns></returns>
		private string DeclareNoAuto(DataTableObject dataTable)
		{
			string str = "";

			foreach (var row in dataTable.RowList)
			{
				var isForeign = row.ColumnName.StartsWith("FK_", StringComparison.InvariantCulture);
				var isPrimary = row.ColumnName.EndsWith("_PK", StringComparison.InvariantCulture);
				var datebool = row.ColumnName.ToLower().Contains("adddate");

				if (!isPrimary && !isForeign && !datebool)
				{
					if (row.DataType != null && row.DataType.ToString() != "timestamp" && dataTable.PrimaryKey != row.ColumnName)
					{
						str += "\nIN p" + row.ColumnName + " " + row.WholeDataType + ",";
					}
				}
			}
			if (!String.IsNullOrEmpty(str))
			{
				str = str.Substring(0, str.Length - 1);
			}
			return str;
		}

		private bool CreateSaveFile(string path, string saveString)
		{
			if (!File.Exists(path))
			{
				// Create a file to write to.
				System.IO.FileInfo file = new System.IO.FileInfo(path);
				file.Directory.Create();
				using (StreamWriter sw = File.CreateText(path))
				{
					sw.WriteLine(saveString);
				}
			}
			else
			{
				using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
				{
					file.Write(saveString);
				}
			}

			return false;
		}
	}
}