using KickAssSystemGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace UI.WIndows
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			Initializer();
		}

		private readonly string location = Path.Combine(@"C:\Users\testdb.sql");
		private List<DataTableObject> list = new List<DataTableObject>();
		private readonly string path = Path.Combine("Files", "Output\\");
		private Tuple<string, string, string> imports { get; set; }

		public readonly string CsharpFilesLocation = @"\DbModels";

		private void Form1_Load(object sender, EventArgs e)
		{
		}

		private void Initializer()
		{
			btnGet.Click += BtnGet_Click;
			btnGenPHP.Click += BtnGenPHP_Click;
			btnGenStored.Click += BtnGenStored_Click;
			btnJava.Click += BtnJava_Click;
			btnHTML.Click += BtnHTML_Click;
			btnTypeScript.Click += BtnTypeScript_Click;
			btnComponents.Click += BtnComponents_Click;
			btnCopyImport.Click += BtnCopyImport_Click;
			btnCopyDeclaration.Click += BtnCopyDeclaration_Click;
			btnGetMethods.Click += BtnGetMethods_Click;
			btnGenCSharpClass.Click += btnGenCSharpClass_Click;
		}

		private void btnGenCSharpClass_Click(object sender, EventArgs e)
		{
			var c = new CSharpClassGenerator(list, "Web_Core.Models");
			c.MakeClasses(@"C:\Users\nelso\Source\Workspaces\KickAssProject\KickAssSystemGenerator\UI.WIndows\bin\Debug\");
			MessageBox.Show("Done Generating C# Classes");
		}

		private void BtnGetMethods_Click(object sender, EventArgs e)
		{
			var files = Directory.GetFiles(CsharpFilesLocation, "*.cs");
			string final = "";
			foreach (var file in files)
			{
				if (!file.ToLower().EndsWith("context.cs"))
				{
					var fullName = Path.GetFileName(file);
					var c = fullName.Split('.')[0];
					var v = fullName.EndsWith("r.cs") ? fullName.Replace("r.cs", "rs.cs") : fullName.Replace(".cs", "s.cs");
					final += $"get{ v.Split('.')[0] }(): Observable<any> {{ return this.httpclient.get<{c}[]>('/api/{c}/'); }} \n";
				}
			}

			Clipboard.SetText(final);
		}

		private void BtnCopyDeclaration_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(imports.Item2);
		}

		private void BtnCopyImport_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(imports.Item1);
		}

		private void BtnComponents_Click(object sender, EventArgs e)
		{
			var comp = new GenerateComponentsFromHTML(@"C:\Users\Web\ClientApp\src\app\admin", path + "Components\\");
			imports = comp.GenerateComponents();

			btnCopyDeclaration.Enabled = true;
			btnCopyImport.Enabled = true;
			btnGetMethods.Enabled = true;

			MessageBox.Show("Done creating components");
		}

		private void BtnTypeScript_Click(object sender, EventArgs e)
		{
			var tS = new GenerateTypeScriptObjectsFromCS(CsharpFilesLocation, path + "TS\\");
			tS.GenerateTypeScript();
			MessageBox.Show("Done creating TS files:\nNOW IMPORT THEMM BEFORE GENERATING OTHER FILES");
		}

		private void BtnHTML_Click(object sender, EventArgs e)
		{
			HTMLDesign h = new HTMLDesign(list);
			h.ShowDialog();
		}

		private void BtnJava_Click(object sender, EventArgs e)
		{
			var jsObject = new JavaAjax(list);
			jsObject.GenerateByTable(path + "JS\\");
			MessageBox.Show("Done Creating JS Files");
		}

		private void BtnGenStored_Click(object sender, EventArgs e)
		{
			var generateStoreProc = new MySQLGenerator(list, path + "MySQL\\");
			MessageBox.Show("Stored Procedures Created");
		}

		private void BtnGenPHP_Click(object sender, EventArgs e)
		{
			var phpObject = new PHPGenerator(list);
			phpObject.GenerateByTable(path + "PHP\\");
			MessageBox.Show("Done Creating PHP Files");
		}

		private void BtnGet_Click(object sender, EventArgs e)
		{
			try
			{
				var lst = new DBInterpreter().Get(location);
				list = lst;
				foreach (var table in lst)
				{
					var node = new TreeNode(table.TableName);
					foreach (var attr in table.RowList)
					{
						node.Nodes.Add(attr.ColumnName);
					}
					treeV.Nodes.Add(node);
				}
				panel1.Enabled = true; ;
			}
			catch (Exception x) { MessageBox.Show(x.Message, "Could read MySql file", MessageBoxButtons.OK, MessageBoxIcon.Error); }
		}
	}
}