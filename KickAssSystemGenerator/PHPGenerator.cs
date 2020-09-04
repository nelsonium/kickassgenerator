using System;
using System.Collections.Generic;
using System.IO;

namespace KickAssSystemGenerator
{
	public class PHPGenerator
	{
		private IEnumerable<DataTableObject> globalList;

		public PHPGenerator(IEnumerable<DataTableObject> list)
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
			foreach (var item in globalList)
			{
				string str = "";

				str = GenerateGetAll(item);
				CreateSaveFile(Path.Combine(path, "GetAll" + item.TableName.ToLower() + ".php"), str);
				str = GenerateInsert(item);
				CreateSaveFile(Path.Combine(path, "Create" + item.TableName.ToLower() + ".php"), str);
				str = GenerateUpdate(item);
				CreateSaveFile(Path.Combine(path, "Update" + item.TableName.ToLower() + ".php"), str);
				str = GenerateDelete(item);
				CreateSaveFile(Path.Combine(path, "Delete" + item.TableName.ToLower() + ".php"), str);
				str = GenerateOneQueryFile(item, item.PrimaryKey);
				CreateSaveFile(Path.Combine(path, "Get" + item.TableName + "ByID" + ".php"), str);
				foreach (var row in item.RowList)
				{
					if (!string.IsNullOrEmpty(row.TableRefID))
					{
						str = GenerateOneQueryFile(item, row.TableRefID);
						CreateSaveFile(Path.Combine(path, "Get" + item.TableName + "By" + row.TableRefID + ".php"), str);
					}
				}
				//string str = "<?PHP\n//INCLUDE 'Utility/connection.php';";
				//str += "\n\n/*GET*/\n" + GenerateFetchQuery(item);

				//str += "\n\n/*INSERT*/\n" + GenerateInsert(item);

				//str += "\n\n/*UPDATE*/\n" + GenerateUpdate(item);

				//str += "\n\n/*DELETE*/\n" + GenerateDelete(item);

				//foreach (var col in item.RowList)
				//{
				//   str += "\n\n" + GenerateOneQueryFile(item, col.ColumnName);
				//}
				//if (item.TableName == "Identity")
				//{
				//    str += "\n\nfunction GetIdentityLogin($Identity , $Password)\n" +
				//        "{ $query = \"CALL sp_Identity_Login('\" . $Identity . \"','\" . $Password . \"' )\";\n" +
				//        "if ($result = connect()->query($query)) {\n" +
				//        "$emparray = array();\n" +
				//        "    while ($row = mysqli_fetch_assoc($result)) {\n" +
				//        "        $emparray[] = $row;\n }\n" +
				//        "            $result->free();\n" +
				//        "            connect()->close();\n" +
				//        "    return json_encode($emparray); } } ";
				//}
				//str +=  "\n\n?>";
				//CreateSaveFile(path + "\\" + item.TableName + ".php", str);
			}
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

			string str = "<?php \n\n include_once 'global.php';\n\n $ID = $_GET['" + attribute + "'];\n\n "; ;// "function Get"+item.TableName+"By"+ attribute+"($"+attribute+")\n{\n";
			str += "\t$query = \"CALL sp_" + item.TableName + "_BY_" + attribute + "(\" . $" + attribute + " . \")\";\n";
			str += "\tif ($result = connect()->query($query)) {\n";
			str += "\t $emparray = array();\n";
			str += "\t while ($row = mysqli_fetch_assoc($result))\n\t{\n";
			str += "\t $emparray[] = $row;\n";
			str += "\t}\n";
			str += "\t$result->free();\n";
			str += "\tconnect()->close();\n";
			str += "\t echo json_encode($emparray);\n}\n\n?>";

			return str;
		}

		private string GenerateGetAll(DataTableObject item)
		{
			//Arrays Results
			string str = "<?php \n\n include_once 'global.php';\n\n ";
			str += "\n\t$query='CALL sp_" + item.TableName + "_Select();';\n";
			str += "\t if ($result = connect()->query($query)) {\n";
			str += "\t   $emparray = array();\n";
			str += "\t while ($row = mysqli_fetch_assoc($result))\n{\n";
			str += "\t $emparray[] = $row;\n";
			str += "\t }\n";
			str += "\t $result->free();\n";
			str += "\t connect()->close();\n";
			str += "\t echo json_encode($emparray);\n}\n\n?>";

			return str;
		}

		private string GenerateInsert(DataTableObject item)
		{
			string variables = "\n", variables2 = "\n", pass = "", varDeclare = "\n";
			foreach (var col in item.RowList)
			{
				if (col.DataType != null && col.ColumnName != "AddDate" && item.PrimaryKey != col.ColumnName)
				{
					if (col.DataType.ToString() == "float" || col.DataType.ToString() == "int" || col.DataType.ToString() == "decimal" || col.DataType.ToString() == "bit" || col.DataType.ToString() == "double")
					{
						varDeclare += "\t$" + col.ColumnName + " = null;\n";
						variables += "\t$" + col.ColumnName + " = " + "$data->" + col.ColumnName + ";\n";
						variables2 += "\t$" + col.ColumnName + " = " + "$data[0]->" + col.ColumnName + ";\n";
						pass += "\t$" + col.ColumnName + " . \",\" . ";
					}
					else if ((col.DataType.ToString() == "mediumblob" || col.DataType.ToString() == "varchar" || col.DataType.ToString() == "date" || col.DataType.ToString() == "datetime") || col.DataType.ToString() == "datetime")
					{
						varDeclare += "\t$" + col.ColumnName + " = null;\n";
						variables += "\t$" + col.ColumnName + " = " + "$data->" + col.ColumnName + ";\n";
						variables2 += "\t$" + col.ColumnName + " = " + "$data[0]->" + col.ColumnName + ";\n";
						pass += "\t\"'\" . $" + col.ColumnName + " . \"'\" . \",\" . ";
					}
				}
			}

			if (!String.IsNullOrEmpty(variables))
			{
				variables = variables.Remove(variables.Length - 1);
			}
			if (!String.IsNullOrEmpty(pass))
			{
				pass = pass + "***";
				pass = pass.Replace(" . \",\" . ***", "");
			}
			string baseCode = "$data1 = $_POST['data'];//website";
			baseCode += "$data = null;";
			baseCode += varDeclare;

			baseCode += "if ($data1 != null) \n";
			baseCode += "{\n";
			baseCode += "\t$data2 = json_encode($data1);  \n";
			baseCode += "\t$data = json_decode($data2);\n";
			baseCode += variables2;
			baseCode += "} \n";
			baseCode += "else\n";
			baseCode += "{\n";
			baseCode += "\t$data1 = file_get_contents('php://input');\n";
			baseCode += "\t$data = json_decode($data1);\n";

			baseCode += variables;
			baseCode += "\n}\n\n";

			string str = "<?php\n\n";
			str += "include_once 'global.php';\n";
			str += baseCode;
			// str += "\n\t$d = file_get_contents('php://input');\n";
			//str += "\n\t$data = json_decode($d);\n";
			// str +=  variables +"\n\n";
			str += "\t$query=\"CALL sp_Insert_" + item.TableName + "(\"." + pass + " . \")\";\n";
			str += "\t $result = mysqli_query(connect(), $query) or die('Query fail');// . mysqli_error());\n";
			str += "\t$last_id = -1;\n";
			str += "\t while ($row = mysqli_fetch_array($result))\n";
			str += "\t { $last_id = $row[0]; }\n";
			str += "\t echo json_encode(array('ID' => $last_id), JSON_FORCE_OBJECT);\n";
			str += "\t mysqli_close(connect());\n\n?>";

			if (item.TableName == "Identity")
			{
				str = GenerateIdentity(item);
			}

			return str;
		}

		private string GenerateUpdate(DataTableObject item)
		{
			string variables = "", pass = "";
			foreach (var col in item.RowList)
			{
				if (col.DataType != null && col.ColumnName != "AddDate" && item.PrimaryKey != col.ColumnName)
				{
					if (col.DataType.ToString() == "int" || col.DataType.ToString() == "decimal")
					{
						variables += "\t$" + col.ColumnName + " = " + "$data->" + col.ColumnName + ";\n";
						pass += "\t$" + col.ColumnName + " . \",\" . ";
					}
					else if ((col.DataType.ToString() == "varchar" || col.DataType.ToString() == "date") || col.DataType.ToString() == "datetime")
					{
						variables += "\t$" + col.ColumnName + " = " + "$data->" + col.ColumnName + ";\n";
						pass += "\t\"'\" . $" + col.ColumnName + " . \"'\" . \",\" . ";
					}
				}
			}

			if (!String.IsNullOrEmpty(pass))
			{
				pass = pass + "***";
				pass = pass.Replace(" . \",\" . ***", "");
			}

			string str = "<?php\n\n";
			str += "include_once 'global.php';\n";
			str += "\n\t $data = $_GET['data'];\n";
			str += "\n" + variables;
			str += "\n\t $query=\"CALL sp_Update_" + item.TableName + "(\" . " + pass + "\")\";\n";
			str += "\t if (mysqli_query(connect(), $query)){\n";
			str += "\t $last_id = mysqli_insert_id(connect());\n";
			str += "\t echo $last_id;\n" +
				"\tmysqli_close(connect());}\n\n?>";
			return str;
		}

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

		//old
		//private string GenerateOneQueryFile(DataTableObject item, string attribute)
		//{
		//    //===========get multiple===========
		//    string vars = "";
		//    foreach (var v in item.RowList)
		//    {
		//        vars += "@" + v.ColumnName + ",";
		//    }
		//    vars = vars.Remove(vars.Length - 1);

		//    string str = "function Get" + item.TableName + "By" + attribute + "($" + attribute + ")\n{\n";
		//    str += "\t$query = \"CALL sp_" + item.TableName + "_BY_" + attribute + "(\" . $" + attribute + " . \")\";\n";
		//    str += "\tif ($result = connect()->query($query)) {\n";
		//    str += "\t $emparray = array();\n";
		//    str += "\t while ($row = mysqli_fetch_assoc($result))\n\t{\n";
		//    str += "\t $emparray[] = $row;\n";
		//    str += "\t}\n";
		//    str += "\t$result->free();\n";
		//    str += "\tconnect()->close();\n";
		//    str += "\treturn json_encode($emparray);}\n}";

		//    return str;
		//}
		//private string GenerateFetchQuery(DataTableObject item)
		//{
		//    //Arrays Results
		//    string str = "function GetAll" + item.TableName + "()\n{";
		//    str += "\n\t$query='CALL sp_" + item.TableName + "_Select();';\n";
		//    str += "\t if ($result = connect()->query($query)) {\n";
		//    str += "\t   $emparray = array();\n";
		//    str += "\t while ($row = mysqli_fetch_assoc($result))\n{\n";
		//    str += "\t $emparray[] = $row;\n";
		//    str += "\t }\n";
		//    str += "\t $result->free();\n";
		//    str += "\t connect()->close();\n";
		//    str += "\t return json_encode($emparray);\n}\n}";

		//    return str;
		//}
		//private string GenerateInsert(DataTableObject item)
		//{
		//    string variables = "\n", pass = "";
		//    foreach (var col in item.RowList)
		//    {
		//        if (col.DataType != null && col.ColumnName != "AddDate" && item.PrimaryKey != col.ColumnName)
		//        {
		//            if (col.DataType.ToString() == "int" || col.DataType.ToString() == "decimal")
		//            {
		//                variables += "\t$" + col.ColumnName + " = " + "$data['" + col.ColumnName + "'];\n";
		//                pass += "\t$" + col.ColumnName + " . \",\" . ";
		//            }
		//            else if ((col.DataType.ToString() == "varchar" || col.DataType.ToString() == "date") || col.DataType.ToString() == "datetime")
		//            {
		//                variables += "\t$" + col.ColumnName + " = " + "$data['" + col.ColumnName + "'];\n";
		//                pass += "\t\"'\" . $" + col.ColumnName + " \"'\" . \",\" . ";
		//            }
		//        }
		//    }
		//    if (!String.IsNullOrEmpty(variables))
		//    {
		//        variables = variables.Remove(variables.Length - 1);
		//    }
		//    if (!String.IsNullOrEmpty(pass))
		//    {
		//        pass = pass.Remove(pass.Length - 6);
		//    }

		//    string str = "function Create" + item.TableName + "()\n{";
		//    str += "\n\t$data = $_POST['data'];\n";
		//    str += variables + "\n\n";
		//    str += "\t$query=\"CALL sp_Insert_" + item.TableName + "(\" . " + pass + " \")\";\n";
		//    str += "\t $result = mysqli_query(connect(), $query) or die('Query fail');// . mysqli_error());\n";
		//    str += "\t$last_id = -1;\n";
		//    str += "\t while ($row = mysqli_fetch_array($result))\n";
		//    str += "\t { $last_id = $row[0]; }\n";
		//    str += "\t return $last_id;\n";
		//    str += "\t mysqli_close(connect());\n}";

		//    if (item.TableName == "Identity")
		//    {
		//        str = GenerateIdentity(item);
		//    }

		//    return str;
		//}

		//private string GenerateUpdate(DataTableObject item)
		//{
		//    string variables = "", pass = "";
		//    foreach (var col in item.RowList)
		//    {
		//        if (col.DataType != null && col.ColumnName != "AddDate")
		//        {
		//            if (col.DataType.ToString() == "int" || col.DataType.ToString() == "decimal")
		//            {
		//                variables += "\t$" + col.ColumnName + " = " + "$data['" + col.ColumnName + "'];\n";
		//                pass += "\t$" + col.ColumnName + " . \",\" . ";
		//            }
		//            else if ((col.DataType.ToString() == "varchar" || col.DataType.ToString() == "date") || col.DataType.ToString() == "datetime")
		//            {
		//                variables += "\t$" + col.ColumnName + " = " + "$data['" + col.ColumnName + "'];\n";
		//                pass += "\t\"'\" . $" + col.ColumnName + " \"'\" . \",\" . ";
		//            }
		//        }

		//    }
		//    if (!String.IsNullOrEmpty(pass))
		//    {
		//        pass = pass.Remove(pass.Length - 6);
		//    }

		//    string str = "function Update" + item.TableName + "()\n{";
		//    str += "\n\t $data = $_POST['data'];\n";
		//    str += "\n" + variables;
		//    str += "\n\t $query=\"CALL sp_Update_" + item.TableName + "(\" . " + pass + "\")\";\n";
		//    str += "\t if (mysqli_query(connect(), $query)){\n";
		//    str += "\t $last_id = mysqli_insert_id(connect());\n";
		//    str += "\t return json_encode($last_id);\n" +
		//        "\tmysqli_close(connect());}\n}";
		//    return str;
		//}
		//private string GenerateDelete(DataTableObject item)
		//{
		//    string str = "function Delete" + item.TableName + "($id, $status){\n";
		//    str += "\t $query=\"CALL sp_Delete_" + item.TableName + "($id, $status)\";\n";
		//    str += "\t if (mysqli_query(connect(), $query)) \n{\n";
		//    str += "\t $last_id = mysqli_insert_id(connect());\n";
		//    str += "\t return json_encode($last_id);\n" +
		//        "\t mysqli_close(connect());}\n}";

		//    return str;
		//}
		//private string GenerateIdentity(DataTableObject item)
		//{
		//    string columns = "", pass = "";
		//    foreach (var col in item.RowList)
		//    {
		//        if (col.DataType != null)
		//        {
		//            if ((col.DataType.ToString() == "varchar" || col.DataType.ToString() == "date") || col.DataType.ToString() == "datetime")
		//            {
		//                columns += "'\" . $" + col.ColumnName + " . \"',";
		//                pass += "$" + col.ColumnName + ",";
		//            }
		//            else
		//            {
		//                if (item.PrimaryKey != col.ColumnName)
		//                {
		//                    if (col.DataType == null)
		//                    {
		//                        columns += "\" . $" + col.ColumnName + " . \",";
		//                        pass += "$" + col.ColumnName + ",";
		//                    }
		//                    else
		//                    {
		//                        if (col.DataType.ToString().ToLower() != "timestamp")
		//                        {
		//                            if (col.ColumnName == "RecordStatus")
		//                            {
		//                                columns += "\" . 1 . \",";
		//                            }
		//                            else
		//                            {
		//                                columns += "\" . $" + col.ColumnName + " . \",";
		//                                pass += "$" + col.ColumnName + ",";
		//                            }
		//                        }
		//                    }
		//                }
		//            }
		//        }
		//    }
		//    if (!String.IsNullOrEmpty(columns))
		//    {
		//        columns = columns.Trim().Remove(columns.Length - 1);
		//    }
		//    if (!String.IsNullOrEmpty(pass))
		//    {
		//        pass = pass.Trim().Remove(pass.Length - 1);
		//    }

		//    string str = "function Create" + item.TableName + "(" + pass + ")\n{";
		//    str += "\t //$data = json_decode($data);\n";
		//    str += "\n\t $query=\"CALL sp_Insert_" + item.TableName + "(" + columns + ")\";\n";
		//    str += "\t $result = mysqli_query(connect(), $query) or die('Query fail');// . mysqli_error());\n";
		//    str += "\t$last_id = -1;\n";
		//    str += "\t while ($row = mysqli_fetch_array($result))\n";
		//    str += "\t { $last_id = $row[0]; }\n";
		//    str += "\t return $last_id;\n";
		//    str += "\t mysqli_close(connect());\n}";

		//    return str;
		//}
	}
}