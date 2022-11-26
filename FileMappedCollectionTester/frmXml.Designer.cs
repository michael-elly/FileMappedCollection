namespace FileMappedCollectionTester {
	partial class frmXml {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.txtXml = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// txtXml
			// 
			this.txtXml.Location = new System.Drawing.Point(81, 36);
			this.txtXml.Name = "txtXml";
			this.txtXml.Size = new System.Drawing.Size(251, 174);
			this.txtXml.TabIndex = 0;
			this.txtXml.Text = "";
			this.txtXml.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtXml_KeyDown);
			// 
			// frmXml
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.txtXml);
			this.Name = "frmXml";
			this.Text = "frmXml";
			this.Load += new System.EventHandler(this.frmXml_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private RichTextBox txtXml;
	}
}