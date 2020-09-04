using System;
using System.IO;
using System.Linq;

namespace KickAssSystemGenerator
{
	/// <summary>
	/// This will generate TypeScript components for Angular, from provided html files
	/// </summary>
	public class GenerateComponentsFromHTML
	{
		public string Source, Destination;

		public GenerateComponentsFromHTML(string source, string desination)
		{
			this.Source = source;
			this.Destination = desination;
		}

		public Tuple<string, string, string> GenerateComponents()
		{
			try
			{
				var files = Directory.GetFiles(Source, "*.html");
				string compFile;

				string item1 = string.Empty, item2 = string.Empty, item3 = string.Empty;
				foreach (var file in files)
				{
					var c = Path.GetFileName(file).Split('.')[0];
					compFile = MakeComponent(c);
					var loc = $"{Destination}\\{c}.component.ts";
					CreateSaveFile($"{Destination}\\{c}.component.ts", compFile);
					item1 += ($"import {{ {c}Component }} from '../app/admin/{c}.component';\n");
					item2 += ($"{c}Component,\n");
				}
				Tuple<string, string, string> imports = new Tuple<string, string, string>(item1, item2, item3);

				return imports;
			}
			catch (Exception x)
			{
				throw x;
			}
		}

		private string MakeComponent(string c)
		{
			var classes = Directory.GetFiles(@"C:\Users\Nelson\Source\repos\SchoolSystem\Web\ClientApp\src\app\class", "*.ts");
			string className = classes.Where(x => x.ToLower().EndsWith(c + ".ts") || ("new" + Path.GetFileName(x).Split('.')[0] == c))?.FirstOrDefault();
			if (string.IsNullOrEmpty(className))
			{
				throw new Exception("TS class that corresponds with the HTML page not found");
			}
			var finalClass = File.ReadAllLines(className)?.FirstOrDefault().Split(' ')[2];

			var word = "import { Component } from '@angular/core';\n" +
"import { Observable } from 'rxjs';\n" +
"import { HttpClient, HttpParams } from '@angular/common/http';\n" +
"import { " + finalClass + " } from '../class/" + c + "';\n\n" +

"@Component({\n" +
"  selector: '" + c + "',\n" +
"  templateUrl: './" + c + ".component.html',\n" +
"})\n" +

"  export class " + finalClass + "Component" +
"  {\n" +
"       constructor(private httpclient: HttpClient) { this." + c + "Item = new " + finalClass + "();" +
"  }\n" +
"  //BASIC FUNCTIONS\n" +
"  get" + finalClass + "() : Observable<any> { return this.httpclient.get<" + finalClass + "[]>('/api/" + finalClass + "/'); }\n" +
"  get" + finalClass + "ById(id: number) : Observable<any> { return this.httpclient.get<" + finalClass + ">('/api/" + finalClass + "/' + id); }\n" +
"  new" + finalClass + "(" + c + ": " + finalClass + ") : Observable<any> { return this.httpclient.post('/api/Book', book); }\n\n" +

"  //LOCAL VARIABLES\n" +
"  lst" + finalClass + ": " + finalClass + "[];\n" +
"  " + c + "Item: " + finalClass + ";\n\n" +

"  //EVENTS\n" +
 "  onSaveNew" + finalClass + "()" +
"  {\n" +
"  \tthis.new" + finalClass + "(this." + c + "Item).subscribe(data => { this.lst" + finalClass + " = data; })\n" +
"  }\n" +
"  onGet" + finalClass + "ById(id: number)" +
 "  {\n" +
"  \tthis.get" + finalClass + "ById(id).subscribe(data => { this." + c + "Item = data; })\n" +
"  }\n" +

 "  ngOnInit()" +
"  {\n" +
"  \tthis.get" + finalClass + "().subscribe(data => { this.lst" + finalClass + " = data; })\n" +
"  }\n" +
"}";

			return word;
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