using System;
using System.IO;

namespace KickAssSystemGenerator
{
	/// <summary>
	/// This will create TypeScript files from C# files
	/// </summary>
	public class GenerateTypeScriptObjectsFromCS
	{
		private string classLocation, outPutLocation;

		public GenerateTypeScriptObjectsFromCS(string location, string outputlocation)
		{
			this.outPutLocation = outputlocation;
			this.classLocation = location;
		}

		public bool GenerateTypeScript()
		{
			try
			{
				var files = Directory.GetFiles(classLocation, "*.cs");
				Tuple<string, string> typeScriptFile = new Tuple<string, string>(string.Empty, string.Empty);

				foreach (var file in files)
				{
					if (!file.ToLower().EndsWith("schoolcontext.cs"))
					{
						var c = File.ReadAllLines(file);
						typeScriptFile = MakeTypeScript(c);
						CreateSaveFile($"{outPutLocation}\\{typeScriptFile.Item2.ToLower()}.ts", $"{typeScriptFile.Item1}");
					}
				}
				return true;
			}
			catch (Exception x)
			{
				throw x;
			}
		}

		private Tuple<string, string> MakeTypeScript(string[] text)
		{
			string oName = string.Empty, variables = "\n";
			foreach (var t in text)
			{
				if (!string.IsNullOrEmpty(oName) && t.Trim().StartsWith("public"))
				{
					var words = t.ToLower().Trim().Split(' ');
					variables += MakeVar(words[1], words[2]);
				}
				else if (!string.IsNullOrEmpty(oName) && t.Trim() == "}")
				{
					var cls = $"export class {oName} \n{{{variables}}}";
					return new Tuple<string, string>(cls, oName);
				}
				var lines = t.Contains("public partial class") ? t.Split(new string[] { "public partial class" }, StringSplitOptions.None) : null;
				oName = t.Contains("public partial class") ? lines[1].Trim() : oName;
			}
			return null;
		}

		private string MakeVar(string v, string v_name)
		{
			if (v == "int" || v == "decimal")
			{
				return $"\t{v_name}: number;\n";
			}
			else if (v == "bool")
			{
				return $"\t{v_name}: boolean;\n";
			}
			else if (v == "DateTime" || v == "DateTime?" | v == "datetime" || v == "datetime?")
			{
				return $"\t{v_name}: any;\n";
			}
			else
			{
				return $"\t{v_name}: {v};\n";
			}
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