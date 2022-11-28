namespace FileMappedCollectionViewer {
	partial class frmNewFile {
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
			this.txtPath = new System.Windows.Forms.TextBox();
			this.chkRegenerateFile = new System.Windows.Forms.CheckBox();
			this.chkAutoShrink = new System.Windows.Forms.CheckBox();
			this.nudMaxExtensions = new System.Windows.Forms.NumericUpDown();
			this.nudExtensionsizeMB = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.nudInitialSizeMB = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnStoreToFields = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxExtensions)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudExtensionsizeMB)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudInitialSizeMB)).BeginInit();
			this.SuspendLayout();
			// 
			// txtPath
			// 
			this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPath.Location = new System.Drawing.Point(74, 12);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(404, 23);
			this.txtPath.TabIndex = 17;
			this.txtPath.Text = "c:\\temp\\a.amc";
			// 
			// chkRegenerateFile
			// 
			this.chkRegenerateFile.AutoSize = true;
			this.chkRegenerateFile.Location = new System.Drawing.Point(370, 74);
			this.chkRegenerateFile.Name = "chkRegenerateFile";
			this.chkRegenerateFile.Size = new System.Drawing.Size(160, 19);
			this.chkRegenerateFile.TabIndex = 21;
			this.chkRegenerateFile.Text = "Regenerate File If Corrupt";
			this.chkRegenerateFile.UseVisualStyleBackColor = true;
			// 
			// chkAutoShrink
			// 
			this.chkAutoShrink.AutoSize = true;
			this.chkAutoShrink.Checked = true;
			this.chkAutoShrink.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAutoShrink.Location = new System.Drawing.Point(215, 44);
			this.chkAutoShrink.Name = "chkAutoShrink";
			this.chkAutoShrink.Size = new System.Drawing.Size(133, 19);
			this.chkAutoShrink.TabIndex = 22;
			this.chkAutoShrink.Text = "Auto Shrink Enabled";
			this.chkAutoShrink.UseVisualStyleBackColor = true;
			// 
			// nudMaxExtensions
			// 
			this.nudMaxExtensions.Location = new System.Drawing.Point(108, 71);
			this.nudMaxExtensions.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nudMaxExtensions.Name = "nudMaxExtensions";
			this.nudMaxExtensions.Size = new System.Drawing.Size(70, 23);
			this.nudMaxExtensions.TabIndex = 18;
			this.nudMaxExtensions.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
			// 
			// nudExtensionsizeMB
			// 
			this.nudExtensionsizeMB.Location = new System.Drawing.Point(271, 71);
			this.nudExtensionsizeMB.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
			this.nudExtensionsizeMB.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudExtensionsizeMB.Name = "nudExtensionsizeMB";
			this.nudExtensionsizeMB.Size = new System.Drawing.Size(50, 23);
			this.nudExtensionsizeMB.TabIndex = 19;
			this.nudExtensionsizeMB.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(52, 15);
			this.label1.TabIndex = 11;
			this.label1.Text = "File Path";
			// 
			// nudInitialSizeMB
			// 
			this.nudInitialSizeMB.Location = new System.Drawing.Point(108, 42);
			this.nudInitialSizeMB.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
			this.nudInitialSizeMB.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudInitialSizeMB.Name = "nudInitialSizeMB";
			this.nudInitialSizeMB.Size = new System.Drawing.Size(70, 23);
			this.nudInitialSizeMB.TabIndex = 20;
			this.nudInitialSizeMB.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(13, 46);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 15);
			this.label5.TabIndex = 12;
			this.label5.Text = "Initial File Size";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 75);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(89, 15);
			this.label2.TabIndex = 13;
			this.label2.Text = "Max Extensions";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(184, 46);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(25, 15);
			this.label9.TabIndex = 14;
			this.label9.Text = "MB";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(327, 75);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(25, 15);
			this.label6.TabIndex = 15;
			this.label6.Text = "MB";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(184, 75);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(81, 15);
			this.label7.TabIndex = 16;
			this.label7.Text = "Extension Size";
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(484, 7);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(63, 30);
			this.btnDelete.TabIndex = 23;
			this.btnDelete.Text = "Del";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnStoreToFields
			// 
			this.btnStoreToFields.Location = new System.Drawing.Point(13, 103);
			this.btnStoreToFields.Name = "btnStoreToFields";
			this.btnStoreToFields.Size = new System.Drawing.Size(121, 42);
			this.btnStoreToFields.TabIndex = 24;
			this.btnStoreToFields.Text = "Update Fields";
			this.btnStoreToFields.UseVisualStyleBackColor = true;
			this.btnStoreToFields.Click += new System.EventHandler(this.btnStoreToFields_Click);
			// 
			// frmNewFile
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(559, 157);
			this.Controls.Add(this.btnStoreToFields);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.txtPath);
			this.Controls.Add(this.chkRegenerateFile);
			this.Controls.Add(this.chkAutoShrink);
			this.Controls.Add(this.nudMaxExtensions);
			this.Controls.Add(this.nudExtensionsizeMB);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nudInitialSizeMB);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label7);
			this.Name = "frmNewFile";
			this.Text = "FMC Properties";
			this.Load += new System.EventHandler(this.frmNewFile_Load);
			((System.ComponentModel.ISupportInitialize)(this.nudMaxExtensions)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudExtensionsizeMB)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudInitialSizeMB)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private TextBox txtPath;
		private CheckBox chkRegenerateFile;
		private CheckBox chkAutoShrink;
		private NumericUpDown nudMaxExtensions;
		private NumericUpDown nudExtensionsizeMB;
		private Label label1;
		private NumericUpDown nudInitialSizeMB;
		private Label label5;
		private Label label2;
		private Label label9;
		private Label label6;
		private Label label7;
		private Button btnDelete;
		private Button btnStoreToFields;
	}
}