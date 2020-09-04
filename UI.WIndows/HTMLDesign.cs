using KickAssSystemGenerator;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UI.WIndows
{
	public partial class HTMLDesign : Form
	{
		private List<DataTableObject> list = new List<DataTableObject>();

		public HTMLDesign(List<DataTableObject> lst)
		{
			InitializeComponent();
			list = lst;
		}

		private void loadHTMLPageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog f = new OpenFileDialog();
			f.ShowDialog();
			if (f.FileName != null)
			{
				System.IO.StreamReader file = new System.IO.StreamReader(f.FileName);
				txt.Text = file.ReadToEnd();
				file.Close();
			}
		}

		private void createHTMLPageToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}
	}
}