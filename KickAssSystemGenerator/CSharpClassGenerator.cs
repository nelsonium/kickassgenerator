using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace KickAssSystemGenerator
{
	/// <summary>
	/// This will generate C# files from sql file
	/// </summary>
	public class CSharpClassGenerator
	{
		private List<DataTableObject> ListOfObjects = new List<DataTableObject>();
		private string Namespace;

		public CSharpClassGenerator(List<DataTableObject> list, string NameSpace)
		{
			ListOfObjects = list;
			Namespace = NameSpace;
		}

		public void MakeClasses(string locationToSave)
		{
			foreach (var item in ListOfObjects)
			{
				try
				{
					string str = $"using System;\nnamespace {Namespace}\n{{\n";
					str += $"\tpublic class {item.TableName}\n\t{{";
					foreach (var r in item.RowList)
					{
						if (r.ColumnName != "FOREIGN")
						{
							var dType = r.DataType.ToString().ToLower() == "varchar" ? GetClrType(r.DataType.ToString()).Name : GetClrType(r.DataType.ToString()).GenericTypeArguments[0].Name;

							str += $"\n\t\tpublic { dType } {r.ColumnName} {{ get; set;}}";
						}
					}
					str += "\n\t}\n}";
					if (!Directory.Exists(Path.Combine(locationToSave, "CSharpClasses\\")))
					{
						Directory.CreateDirectory(Path.Combine(locationToSave, "CSharpClasses\\"));
					}
					File.WriteAllText(Path.Combine(locationToSave, "CSharpClasses\\") + item.TableName + ".cs", str);
				}
				catch (Exception x) { throw x; }
			}
		}

		private Type GetClrType(string sqlType)
		{
			SqlDbType myType = (SqlDbType)Enum.Parse(typeof(SqlDbType), GetRightTypeName(sqlType));

			switch (myType)
			{
				case SqlDbType.BigInt:
					return typeof(long?);

				case SqlDbType.Binary:
				case SqlDbType.Image:
				case SqlDbType.Timestamp:
				case SqlDbType.VarBinary:
					return typeof(byte[]);

				case SqlDbType.Bit:
					return typeof(bool?);

				case SqlDbType.Char:
				case SqlDbType.NChar:
				case SqlDbType.NText:
				case SqlDbType.NVarChar:
				case SqlDbType.Text:
				case SqlDbType.VarChar:
				case SqlDbType.Xml:
					return typeof(string);

				case SqlDbType.DateTime:
				case SqlDbType.SmallDateTime:
				case SqlDbType.Date:
				case SqlDbType.Time:
				case SqlDbType.DateTime2:
					return typeof(DateTime?);

				case SqlDbType.Decimal:
				case SqlDbType.Money:
				case SqlDbType.SmallMoney:
					return typeof(decimal?);

				case SqlDbType.Float:
					return typeof(double?);

				case SqlDbType.Int:
					return typeof(int?);

				case SqlDbType.Real:
					return typeof(float?);

				case SqlDbType.UniqueIdentifier:
					return typeof(Guid?);

				case SqlDbType.SmallInt:
					return typeof(short?);

				case SqlDbType.TinyInt:
					return typeof(byte?);

				case SqlDbType.Variant:
				case SqlDbType.Udt:
					return typeof(object);

				case SqlDbType.Structured:
					return typeof(DataTable);

				case SqlDbType.DateTimeOffset:
					return typeof(DateTimeOffset?);

				default:
					throw new ArgumentOutOfRangeException("sqlType");
			}
		}

		private string GetRightTypeName(string sqlType)
		{
			var values = Enum.GetValues(typeof(SqlDbType));
			foreach (var item in values)
			{
				if (sqlType.ToLower() == item.ToString().ToLower())
				{
					return item.ToString();
				}
			}
			throw new Exception(sqlType + "NOT FOUND");
		}
	}
}