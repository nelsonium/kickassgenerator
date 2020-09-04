namespace UI.WIndows
{
    partial class HTMLDesign
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txt = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.listToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadHTMLPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createHTMLPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadHTMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createHTMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadHTMLToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.createHTMLToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt
            // 
            this.txt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt.Location = new System.Drawing.Point(0, 24);
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(910, 466);
            this.txt.TabIndex = 0;
            this.txt.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.listToolStripMenuItem,
            this.createToolStripMenuItem,
            this.updateToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(910, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // listToolStripMenuItem
            // 
            this.listToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadHTMLPageToolStripMenuItem,
            this.createHTMLPageToolStripMenuItem});
            this.listToolStripMenuItem.Name = "listToolStripMenuItem";
            this.listToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.listToolStripMenuItem.Text = "List";
            // 
            // loadHTMLPageToolStripMenuItem
            // 
            this.loadHTMLPageToolStripMenuItem.Name = "loadHTMLPageToolStripMenuItem";
            this.loadHTMLPageToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadHTMLPageToolStripMenuItem.Text = "Load HTML";
            this.loadHTMLPageToolStripMenuItem.Click += new System.EventHandler(this.loadHTMLPageToolStripMenuItem_Click);
            // 
            // createHTMLPageToolStripMenuItem
            // 
            this.createHTMLPageToolStripMenuItem.Name = "createHTMLPageToolStripMenuItem";
            this.createHTMLPageToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.createHTMLPageToolStripMenuItem.Text = "Create HTML";
            this.createHTMLPageToolStripMenuItem.Click += new System.EventHandler(this.createHTMLPageToolStripMenuItem_Click);
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadHTMLToolStripMenuItem,
            this.createHTMLToolStripMenuItem});
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.createToolStripMenuItem.Text = "Create";
            // 
            // loadHTMLToolStripMenuItem
            // 
            this.loadHTMLToolStripMenuItem.Name = "loadHTMLToolStripMenuItem";
            this.loadHTMLToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadHTMLToolStripMenuItem.Text = "Load HTML ";
            // 
            // createHTMLToolStripMenuItem
            // 
            this.createHTMLToolStripMenuItem.Name = "createHTMLToolStripMenuItem";
            this.createHTMLToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.createHTMLToolStripMenuItem.Text = "Create HTML";
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.createToolStripMenuItem1});
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.updateToolStripMenuItem.Text = "Update";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadToolStripMenuItem.Text = "Load HTML";                                                 
            // 
            // createToolStripMenuItem1
            // 
            this.createToolStripMenuItem1.Name = "createToolStripMenuItem1";
            this.createToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.createToolStripMenuItem1.Text = "Create HTML";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadHTMLToolStripMenuItem1,
            this.createHTMLToolStripMenuItem1});
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // loadHTMLToolStripMenuItem1
            // 
            this.loadHTMLToolStripMenuItem1.Name = "loadHTMLToolStripMenuItem1";
            this.loadHTMLToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.loadHTMLToolStripMenuItem1.Text = "Load HTML";
            // 
            // createHTMLToolStripMenuItem1
            // 
            this.createHTMLToolStripMenuItem1.Name = "createHTMLToolStripMenuItem1";
            this.createHTMLToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.createHTMLToolStripMenuItem1.Text = "Create HTML";
            // 
            // HTMLDesign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 490);
            this.Controls.Add(this.txt);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "HTMLDesign";
            this.Text = "HTMLDesign";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txt;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem listToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadHTMLPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createHTMLPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadHTMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createHTMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadHTMLToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem createHTMLToolStripMenuItem1;
    }
}