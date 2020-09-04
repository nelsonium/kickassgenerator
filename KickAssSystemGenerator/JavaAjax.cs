using System;
using System.Collections.Generic;
using System.IO;

namespace KickAssSystemGenerator
{
	public class JavaAjax
	{
		private IEnumerable<DataTableObject> globalList;
		private string site_root = "utility/DATA/";

		public JavaAjax(IEnumerable<DataTableObject> list)
		{
			globalList = list;
		}

		public void GenerateObjects(string path)
		{
			string str = "<?php\n";
			foreach (var item in globalList)
			{
				str += "\nClass " + item.TableName + "()\n{\n";
				foreach (var col in item.RowList)
				{
					str += "public $" + col.ColumnName + ";\n";
				}
				str += "}\n";
			}
			str += "\n?>";
			CreateSaveFile(path, str);//saving .php object file
		}

		public void GenerateByTable(string path)
		{
			string str = " function successFunc(data, status) { return data;   } function completed(data){ return data; } ";
			str += "function errorFunc(xhr, textStatus, errorThrown) {alert(xhr.responseText); }";
			foreach (var item in globalList)
			{
				//str += "\n\n/*GET*/\n" + GenerateFetchQuery(item);

				str += "\n\n/*INSERT " + item.TableName.ToUpper() + "*/\n" + GenerateInsert(item);

				str += "\n\n/*UPDATE " + item.TableName.ToUpper() + "*/\n" + GenerateUpdate(item);

				//str += "\n\n/*GET " + item.TableName.ToUpper() + "*/\n" + GenerateGetAll(item);

				//str += "\n\n/*DELETE*/\n" + GenerateDelete(item);

				//foreach (var col in item.RowList)
				//{
				//    str += "\n\n" + GenerateOneQueryFile(item, col.ColumnName);
				//}
			}

			CreateSaveFile(path + "\\Core.js", str);
		}

		private string GenerateOneQueryFile(DataTableObject item, string attribute)
		{
			//===========get multiple===========
			string vars = "";
			foreach (var v in item.RowList)
			{
				vars += "@" + v.ColumnName + ",";
			}
			vars = vars.Remove(vars.Length - 1);

			string str = "function Get" + item.TableName + "By" + attribute + "($" + attribute + ")\n{\n";
			str += "\t$query = \"CALL sp_" + item.TableName + "_BY_" + attribute + "(\" . $" + attribute + " . \")\";\n";
			str += "\tif ($result = connect()->query($query)) {\n";
			str += "\t $emparray = array();\n";
			str += "\t while ($row = mysqli_fetch_assoc($result))\n\t{\n";
			str += "\t $emparray[] = $row;\n";
			str += "\t}\n";
			str += "\t$result->free();\n";
			str += "\tconnect()->close();\n";
			str += "\treturn json_encode($emparray);}\n}";

			return str;
		}

		private string GenerateFetchQuery(DataTableObject item)
		{
			//Arrays Results
			string str = "function GetAll" + item.TableName + "()\n{";
			str += "\n\t$query='CALL sp_" + item.TableName + "_Select();';\n";
			str += "\t if ($result = connect()->query($query)) {\n";
			str += "\t   $emparray = array();\n";
			str += "\t while ($row = mysqli_fetch_assoc($result))\n{\n";
			str += "\t $emparray[] = $row;\n";
			str += "\t }\n";
			str += "\t $result->free();\n";
			str += "\t connect()->close();\n";
			str += "\t return json_encode($emparray);\n}\n}";

			return str;
		}

		private string GenerateInsert(DataTableObject item)
		{
			string variables = "", pass = "", passVariables = "";
			foreach (var col in item.RowList)
			{
				if (col.DataType != null && col.ColumnName != "AddDate" && item.PrimaryKey != col.ColumnName)
				{
					variables += " '" + col.ColumnName + "' : " + "$('#" + col.ColumnName + "').val() ,";
					pass += col.ColumnName.ToLower() + " ,";
					passVariables += " '" + col.ColumnName + "' : " + col.ColumnName.ToLower() + " ,";
				}
			}

			if (!String.IsNullOrEmpty(variables))
			{
				variables = variables + "***";
				variables = variables.Replace(",***", "");

				pass = pass + "***";
				pass = pass.Replace(",***", "");
				passVariables = passVariables + "***";
				passVariables = passVariables.Replace(",***", "");
			}

			string str = "function create" + item.TableName.ToLower() + "()\n{";
			str += "\n\tvar d = { " + variables + " };";
			str += "\n\tvar data = JSON.stringify(d);";
			str += "\n\t$.ajax({";
			str += "\n\ttype: \"GET\",";
			str += "\n\turl: '" + site_root + "Create" + item.TableName + ".php',";
			str += "\n\tdata: { 'data':data},";
			str += "\n\tcontentType: \"application/json; charset=utf-8\",";
			str += "\n\tdataType: \"json\",";
			str += "\n\tsuccess: successFunc,";
			str += "\n\terror: errorFunc";
			str += "\n\t});\n}\n\n";

			str += "function create" + item.TableName.ToLower() + "v(" + pass + ")\n{";
			str += "\n\tvar d = { " + passVariables + " };";
			str += "\n\tvar data = JSON.stringify(d);";
			str += "\n\t$.ajax({";
			str += "\n\ttype: \"GET\",";
			str += "\n\turl: '" + site_root + "Create" + item.TableName + ".php',";
			str += "\n\tdata: { 'data':data},";
			str += "\n\tcontentType: \"application/json; charset=utf-8\",";
			str += "\n\tdataType: \"json\",";
			str += "\n\tsuccess: successFunc,";
			str += "\n\terror: errorFunc";
			str += "\n\t});\n}";

			if (item.TableName == "Identity")
			{
				str = GenerateIdentity(item);
			}

			return str;
		}

		private string GenerateUpdate(DataTableObject item)
		{
			string variables = "", pass = "", passVariables = "";
			foreach (var col in item.RowList)
			{
				if (col.DataType != null && col.ColumnName != "AddDate" && item.PrimaryKey != col.ColumnName)
				{
					variables += " '" + col.ColumnName + "' : " + "$('#" + col.ColumnName + "').val() ,";
					pass += col.ColumnName.ToLower() + " ,";
					passVariables += " '" + col.ColumnName + "' : " + col.ColumnName.ToLower() + " ,";
				}
			}

			if (!String.IsNullOrEmpty(variables))
			{
				variables = variables + "***";
				variables = variables.Replace(",***", "");

				pass = pass + "***";
				pass = pass.Replace(",***", "");

				passVariables = passVariables + "***";
				passVariables = passVariables.Replace(",***", "");
			}

			string str = "function update" + item.TableName.ToLower() + "()\n{";
			str += "\n\tvar d = { " + variables + " };";
			str += "\n\tvar data = JSON.stringify(d);";
			str += "\n\t$.ajax({";
			str += "\n\ttype: \"GET\",";
			str += "\n\turl: '" + site_root + "Update" + item.TableName + ".php',";
			str += "\n\tdata: { 'data':data},";
			str += "\n\tcontentType: \"application/json; charset=utf-8\",";
			str += "\n\tdataType: \"json\",";
			str += "\n\tsuccess: successFunc,";
			str += "\n\terror: errorFunc";
			str += "\n\t});\n}\n\n";

			str += "function update" + item.TableName.ToLower() + "v(" + pass + ")\n{";
			str += "\n\tvar d = { " + passVariables + " };";
			str += "\n\tvar data = JSON.stringify(d);";
			str += "\n\t$.ajax({";
			str += "\n\ttype: \"GET\",";
			str += "\n\turl: '" + site_root + "Update" + item.TableName + ".php',";
			str += "\n\tdata: { 'data':data},";
			str += "\n\tcontentType: \"application/json; charset=utf-8\",";
			str += "\n\tdataType: \"json\",";
			str += "\n\tsuccess: successFunc,";
			str += "\n\terror: errorFunc";
			str += "\n\t});\n}";
			return str;
		}

		//private string GenerateGetAll(DataTableObject item)
		//{
		//    string str = "";
		//    //"function getall" + item.TableName.ToLower() + "()\n{";
		//    //str += "\n\t$.ajax({";
		//    //str += "\n\ttype: \"GET\",";
		//    //str += "\n\turl: '" + site_root + "GetAll" + item.TableName + ".php',";
		//    //str += "\n\tcontentType: \"application/json; charset=utf-8\",";
		//    //str += "\n\tdataType: \"json\",";
		//    //str += "\n\tasync: true,";
		//    //str += "\n\tcomplete: completed,";
		//    //str += "\n\tsuccess: successFunc,";
		//    //str += "\n\terror: errorFunc";
		//    //str += "\n\t});\n}\n\n";

		//    str += "\n\tasync function getall" + item.TableName.ToLower() + "()\n{";
		//   str += "\n\tconst result = await $.ajax({";
		//      str += "\n\turl: '" + site_root + "GetAll" + item.TableName + ".php',";
		//    str += "\n\ttype: 'GET'";
		//    str += "\n\t});";
		//    str += "return result;";
		//    str += "\n\t}\n\n";
		//    return str;
		//}
		private string GenerateDelete(DataTableObject item)
		{
			string variables = "", pass = "";
			foreach (var col in item.RowList)
			{
				if (col.DataType != null && col.ColumnName != "AddDate")
				{
					if (col.DataType.ToString() == "int" || col.DataType.ToString() == "decimal")
					{
						variables += "\t$" + col.ColumnName + " = " + "$data['" + col.ColumnName + "'];\n";
						pass += "\t$" + col.ColumnName + " . \",\" . ";
					}
					else if ((col.DataType.ToString() == "varchar" || col.DataType.ToString() == "date") || col.DataType.ToString() == "datetime")
					{
						variables += "\t$" + col.ColumnName + " = " + "$data['" + col.ColumnName + "'];\n";
						pass += "\t\"'\" . $" + col.ColumnName + " \"'\" . \",\" . ";
					}
				}
			}
			if (!String.IsNullOrEmpty(pass))
			{
				pass = pass.Remove(pass.Length - 6);
			}

			string str = "<?php\n\n";
			str += "\n\t $data = $_POST['data'];\n";
			str += "\n" + variables;

			str += "\t $query=\"CALL sp_Delete_" + item.TableName + "(\" . " + pass + "\")\";\n";
			str += "\t if (mysqli_query(connect(), $query)) \n{\n";
			str += "\t $last_id = mysqli_insert_id(connect());\n";
			str += "\t return json_encode($last_id);\n" +
				"\t mysqli_close(connect());}\n\n?>";

			return str;
		}

		private string GenerateIdentity(DataTableObject item)
		{
			string columns = "", pass = "";
			foreach (var col in item.RowList)
			{
				if (col.DataType != null)
				{
					if ((col.DataType.ToString() == "varchar" || col.DataType.ToString() == "date") || col.DataType.ToString() == "datetime")
					{
						columns += "'\" . $" + col.ColumnName + " . \"',";
						pass += "$" + col.ColumnName + ",";
					}
					else
					{
						if (item.PrimaryKey != col.ColumnName)
						{
							if (col.DataType == null)
							{
								columns += "\" . $" + col.ColumnName + " . \",";
								pass += "$" + col.ColumnName + ",";
							}
							else
							{
								if (col.DataType.ToString().ToLower() != "timestamp")
								{
									if (col.ColumnName == "RecordStatus")
									{
										columns += "\" . 1 . \",";
									}
									else
									{
										columns += "\" . $" + col.ColumnName + " . \",";
										pass += "$" + col.ColumnName + ",";
									}
								}
							}
						}
					}
				}
			}
			if (!String.IsNullOrEmpty(columns))
			{
				columns = columns.Trim().Remove(columns.Length - 1);
			}
			if (!String.IsNullOrEmpty(pass))
			{
				pass = pass.Trim().Remove(pass.Length - 1);
			}

			string str = "function Create" + item.TableName + "(" + pass + ")\n{";
			str += "\t //$data = json_decode($data);\n";
			str += "\n\t $query=\"CALL sp_Insert_" + item.TableName + "(" + columns + ")\";\n";
			str += "\t $result = mysqli_query(connect(), $query) or die('Query fail');// . mysqli_error());\n";
			str += "\t$last_id = -1;\n";
			str += "\t while ($row = mysqli_fetch_array($result))\n";
			str += "\t { $last_id = $row[0]; }\n";
			str += "\t return $last_id;\n";
			str += "\t mysqli_close(connect());\n}";

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