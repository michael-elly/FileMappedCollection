namespace FileMappedCollectionTester {
	partial class frmMain {
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnLoad = new System.Windows.Forms.Button();
			this.lblNumRecords = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtHeader = new System.Windows.Forms.TextBox();
			this.txtUtil = new System.Windows.Forms.TextBox();
			this.txtSize = new System.Windows.Forms.TextBox();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.picFile = new System.Windows.Forms.PictureBox();
			this.grdRecords = new System.Windows.Forms.DataGridView();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripDropDownButton6 = new System.Windows.Forms.ToolStripDropDownButton();
			this.mnuRefreshMap = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuVerify = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShrink = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuExtend = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDefrag = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuExpertMode = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPeekSelectedRecord = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDequeueSelectedRecord = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripDropDownButton5 = new System.Windows.Forms.ToolStripDropDownButton();
			this.mnuCopyCellText = new System.Windows.Forms.ToolStripMenuItem();
			this.btnRefresh = new System.Windows.Forms.ToolStripButton();
			this.btnAutoRefresh = new System.Windows.Forms.ToolStripButton();
			this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
			this.mnuFillTableWith10LastRecords = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFillTableWithAllRecords = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.lblNumRecordsStatusBar = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblVerifyState = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblLastUpdated = new System.Windows.Forms.ToolStripStatusLabel();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picFile)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.grdRecords)).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnLoad);
			this.panel1.Controls.Add(this.lblNumRecords);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.txtHeader);
			this.panel1.Controls.Add(this.txtUtil);
			this.panel1.Controls.Add(this.txtSize);
			this.panel1.Controls.Add(this.txtPath);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 25);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(639, 131);
			this.panel1.TabIndex = 0;
			// 
			// btnLoad
			// 
			this.btnLoad.Image = global::FileMappedCollectionViewer.Properties.Resources.Refresh;
			this.btnLoad.Location = new System.Drawing.Point(536, 7);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.btnLoad.Size = new System.Drawing.Size(95, 64);
			this.btnLoad.TabIndex = 5;
			this.btnLoad.Text = "Reload";
			this.btnLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// lblNumRecords
			// 
			this.lblNumRecords.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.lblNumRecords.Location = new System.Drawing.Point(536, 90);
			this.lblNumRecords.Name = "lblNumRecords";
			this.lblNumRecords.Size = new System.Drawing.Size(95, 23);
			this.lblNumRecords.TabIndex = 6;
			this.lblNumRecords.Text = "0";
			this.lblNumRecords.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 96);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(45, 15);
			this.label4.TabIndex = 6;
			this.label4.Text = "Header";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 67);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(25, 15);
			this.label3.TabIndex = 6;
			this.label3.Text = "Util";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 38);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(27, 15);
			this.label2.TabIndex = 6;
			this.label2.Text = "Size";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(31, 15);
			this.label1.TabIndex = 6;
			this.label1.Text = "Path";
			// 
			// txtHeader
			// 
			this.txtHeader.Location = new System.Drawing.Point(72, 93);
			this.txtHeader.Name = "txtHeader";
			this.txtHeader.Size = new System.Drawing.Size(458, 23);
			this.txtHeader.TabIndex = 7;
			this.txtHeader.Text = "First Record Start Address: 0, Last Record Start Address: 0";
			// 
			// txtUtil
			// 
			this.txtUtil.Location = new System.Drawing.Point(72, 64);
			this.txtUtil.Name = "txtUtil";
			this.txtUtil.Size = new System.Drawing.Size(458, 23);
			this.txtUtil.TabIndex = 7;
			this.txtUtil.Text = "Records Count: 0, Percent Free: 90%, Percent Free Immidiate: 30%";
			// 
			// txtSize
			// 
			this.txtSize.Location = new System.Drawing.Point(72, 35);
			this.txtSize.Name = "txtSize";
			this.txtSize.Size = new System.Drawing.Size(458, 23);
			this.txtSize.TabIndex = 7;
			this.txtSize.Text = "File Size: 500KB, Initial Size: 500KB, Max Size: 12000KB, Increment Size: 100KB";
			// 
			// txtPath
			// 
			this.txtPath.Location = new System.Drawing.Point(72, 7);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(458, 23);
			this.txtPath.TabIndex = 7;
			this.txtPath.Text = "c:\\temp\\a.amc";
			// 
			// picFile
			// 
			this.picFile.Dock = System.Windows.Forms.DockStyle.Top;
			this.picFile.Location = new System.Drawing.Point(0, 156);
			this.picFile.Name = "picFile";
			this.picFile.Size = new System.Drawing.Size(639, 27);
			this.picFile.TabIndex = 1;
			this.picFile.TabStop = false;
			// 
			// grdRecords
			// 
			this.grdRecords.AllowUserToAddRows = false;
			this.grdRecords.AllowUserToDeleteRows = false;
			this.grdRecords.BackgroundColor = System.Drawing.SystemColors.Control;
			this.grdRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grdRecords.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column4,
            this.Column3,
            this.Column5});
			this.grdRecords.Location = new System.Drawing.Point(12, 204);
			this.grdRecords.Name = "grdRecords";
			this.grdRecords.ReadOnly = true;
			this.grdRecords.RowTemplate.Height = 25;
			this.grdRecords.Size = new System.Drawing.Size(573, 96);
			this.grdRecords.TabIndex = 2;
			this.grdRecords.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdRecords_CellDoubleClick);
			// 
			// Column1
			// 
			this.Column1.HeaderText = "#";
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			// 
			// Column2
			// 
			this.Column2.HeaderText = "Start";
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			// 
			// Column4
			// 
			this.Column4.HeaderText = "Next";
			this.Column4.Name = "Column4";
			this.Column4.ReadOnly = true;
			// 
			// Column3
			// 
			this.Column3.HeaderText = "Length";
			this.Column3.Name = "Column3";
			this.Column3.ReadOnly = true;
			// 
			// Column5
			// 
			this.Column5.HeaderText = "Message";
			this.Column5.Name = "Column5";
			this.Column5.ReadOnly = true;
			// 
			// toolStrip1
			// 
			this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton6,
            this.toolStripDropDownButton5,
            this.btnRefresh,
            this.btnAutoRefresh,
            this.toolStripDropDownButton2});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(639, 25);
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripDropDownButton6
			// 
			this.toolStripDropDownButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButton6.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRefreshMap,
            this.toolStripMenuItem1,
            this.mnuVerify,
            this.mnuShrink,
            this.mnuExtend,
            this.mnuDefrag,
            this.toolStripMenuItem2,
            this.mnuExpertMode,
            this.toolStripMenuItem3,
            this.mnuClose});
			this.toolStripDropDownButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton6.Image")));
			this.toolStripDropDownButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton6.Name = "toolStripDropDownButton6";
			this.toolStripDropDownButton6.Size = new System.Drawing.Size(38, 22);
			this.toolStripDropDownButton6.Text = "&File";
			// 
			// mnuRefreshMap
			// 
			this.mnuRefreshMap.Image = global::FileMappedCollectionViewer.Properties.Resources.Refresh;
			this.mnuRefreshMap.Name = "mnuRefreshMap";
			this.mnuRefreshMap.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.mnuRefreshMap.Size = new System.Drawing.Size(159, 22);
			this.mnuRefreshMap.Text = "Refresh Map";
			this.mnuRefreshMap.Click += new System.EventHandler(this.mnuRefreshMap_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(156, 6);
			// 
			// mnuVerify
			// 
			this.mnuVerify.Name = "mnuVerify";
			this.mnuVerify.Size = new System.Drawing.Size(159, 22);
			this.mnuVerify.Text = "Verify";
			this.mnuVerify.Click += new System.EventHandler(this.mnuVerify_Click);
			// 
			// mnuShrink
			// 
			this.mnuShrink.Name = "mnuShrink";
			this.mnuShrink.Size = new System.Drawing.Size(159, 22);
			this.mnuShrink.Text = "Shrink";
			this.mnuShrink.Click += new System.EventHandler(this.mnuShrink_Click);
			// 
			// mnuExtend
			// 
			this.mnuExtend.Name = "mnuExtend";
			this.mnuExtend.Size = new System.Drawing.Size(159, 22);
			this.mnuExtend.Text = "Extend";
			this.mnuExtend.Click += new System.EventHandler(this.mnuExtend_Click);
			// 
			// mnuDefrag
			// 
			this.mnuDefrag.Name = "mnuDefrag";
			this.mnuDefrag.Size = new System.Drawing.Size(159, 22);
			this.mnuDefrag.Text = "Defrag";
			this.mnuDefrag.Click += new System.EventHandler(this.mnuDefrag_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(156, 6);
			// 
			// mnuExpertMode
			// 
			this.mnuExpertMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPeekSelectedRecord,
            this.mnuDequeueSelectedRecord});
			this.mnuExpertMode.Name = "mnuExpertMode";
			this.mnuExpertMode.Size = new System.Drawing.Size(159, 22);
			this.mnuExpertMode.Text = "Expert Mode";
			this.mnuExpertMode.DropDownOpening += new System.EventHandler(this.mnuExpertMode_DropDownOpening);
			// 
			// mnuPeekSelectedRecord
			// 
			this.mnuPeekSelectedRecord.Name = "mnuPeekSelectedRecord";
			this.mnuPeekSelectedRecord.Size = new System.Drawing.Size(208, 22);
			this.mnuPeekSelectedRecord.Text = "Peek Selected Record";
			this.mnuPeekSelectedRecord.Click += new System.EventHandler(this.mnuPeekSelectedRecord_Click);
			// 
			// mnuDequeueSelectedRecord
			// 
			this.mnuDequeueSelectedRecord.Name = "mnuDequeueSelectedRecord";
			this.mnuDequeueSelectedRecord.Size = new System.Drawing.Size(208, 22);
			this.mnuDequeueSelectedRecord.Text = "Dequeue Selected Record";
			this.mnuDequeueSelectedRecord.Click += new System.EventHandler(this.mnuDequeueSelectedRecord_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(156, 6);
			// 
			// mnuClose
			// 
			this.mnuClose.Name = "mnuClose";
			this.mnuClose.Size = new System.Drawing.Size(159, 22);
			this.mnuClose.Text = "Close";
			this.mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
			// 
			// toolStripDropDownButton5
			// 
			this.toolStripDropDownButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButton5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCopyCellText});
			this.toolStripDropDownButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton5.Image")));
			this.toolStripDropDownButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton5.Name = "toolStripDropDownButton5";
			this.toolStripDropDownButton5.Size = new System.Drawing.Size(40, 22);
			this.toolStripDropDownButton5.Text = "&Edit";
			// 
			// mnuCopyCellText
			// 
			this.mnuCopyCellText.Name = "mnuCopyCellText";
			this.mnuCopyCellText.Size = new System.Drawing.Size(149, 22);
			this.mnuCopyCellText.Text = "Copy Cell Text";
			this.mnuCopyCellText.Click += new System.EventHandler(this.mnuCopyCellText_Click);
			// 
			// btnRefresh
			// 
			this.btnRefresh.Image = global::FileMappedCollectionViewer.Properties.Resources.Refresh;
			this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(66, 22);
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// btnAutoRefresh
			// 
			this.btnAutoRefresh.CheckOnClick = true;
			this.btnAutoRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnAutoRefresh.Name = "btnAutoRefresh";
			this.btnAutoRefresh.Size = new System.Drawing.Size(79, 22);
			this.btnAutoRefresh.Text = "Auto Refresh";
			this.btnAutoRefresh.Click += new System.EventHandler(this.btnAutoRefresh_Click);
			// 
			// toolStripDropDownButton2
			// 
			this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFillTableWith10LastRecords,
            this.mnuFillTableWithAllRecords});
			this.toolStripDropDownButton2.Image = global::FileMappedCollectionViewer.Properties.Resources.table;
			this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
			this.toolStripDropDownButton2.Size = new System.Drawing.Size(81, 22);
			this.toolStripDropDownButton2.Text = "Fill Table";
			this.toolStripDropDownButton2.Click += new System.EventHandler(this.toolStripDropDownButton2_Click);
			// 
			// mnuFillTableWith10LastRecords
			// 
			this.mnuFillTableWith10LastRecords.Name = "mnuFillTableWith10LastRecords";
			this.mnuFillTableWith10LastRecords.Size = new System.Drawing.Size(229, 22);
			this.mnuFillTableWith10LastRecords.Text = "Fill Table with 10 Last Records";
			this.mnuFillTableWith10LastRecords.Click += new System.EventHandler(this.mnuFillTableWith10LastRecords_Click);
			// 
			// mnuFillTableWithAllRecords
			// 
			this.mnuFillTableWithAllRecords.Name = "mnuFillTableWithAllRecords";
			this.mnuFillTableWithAllRecords.Size = new System.Drawing.Size(229, 22);
			this.mnuFillTableWithAllRecords.Text = "Fill Table with All Records";
			this.mnuFillTableWithAllRecords.Click += new System.EventHandler(this.mnuFillTableWithAllRecords_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblNumRecordsStatusBar,
            this.lblVerifyState,
            this.lblLastUpdated});
			this.statusStrip1.Location = new System.Drawing.Point(0, 328);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(639, 25);
			this.statusStrip1.TabIndex = 4;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// lblNumRecordsStatusBar
			// 
			this.lblNumRecordsStatusBar.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.lblNumRecordsStatusBar.Name = "lblNumRecordsStatusBar";
			this.lblNumRecordsStatusBar.Size = new System.Drawing.Size(101, 20);
			this.lblNumRecordsStatusBar.Text = "Records Count: 0";
			// 
			// lblVerifyState
			// 
			this.lblVerifyState.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.lblVerifyState.Image = global::FileMappedCollectionViewer.Properties.Resources.check_ok;
			this.lblVerifyState.Name = "lblVerifyState";
			this.lblVerifyState.Size = new System.Drawing.Size(68, 20);
			this.lblVerifyState.Text = "Healthy";
			// 
			// lblLastUpdated
			// 
			this.lblLastUpdated.Name = "lblLastUpdated";
			this.lblLastUpdated.Size = new System.Drawing.Size(107, 20);
			this.lblLastUpdated.Text = "Last Updated: Now";
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(639, 353);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.grdRecords);
			this.Controls.Add(this.picFile);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "frmMain";
			this.Text = "File Mapped Collection Manager";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picFile)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.grdRecords)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Panel panel1;
		private Button btnLoad;
		private Label lblNumRecords;
		private Label label4;
		private Label label3;
		private Label label2;
		private Label label1;
		private TextBox txtHeader;
		private TextBox txtUtil;
		private TextBox txtSize;
		private TextBox txtPath;
		private PictureBox picFile;
		private DataGridView grdRecords;
		private ToolStrip toolStrip1;
		private ToolStripDropDownButton toolStripDropDownButton6;
		private ToolStripMenuItem mnuRefreshMap;
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripMenuItem mnuVerify;
		private ToolStripMenuItem mnuShrink;
		private ToolStripMenuItem mnuExtend;
		private ToolStripMenuItem mnuDefrag;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripMenuItem mnuExpertMode;
		private ToolStripMenuItem mnuPeekSelectedRecord;
		private ToolStripMenuItem mnuDequeueSelectedRecord;
		private ToolStripSeparator toolStripMenuItem3;
		private ToolStripMenuItem mnuClose;
		private ToolStripDropDownButton toolStripDropDownButton5;
		private ToolStripDropDownButton toolStripDropDownButton2;
		private StatusStrip statusStrip1;
		private ToolStripMenuItem mnuCopyCellText;
		private ToolStripButton btnRefresh;
		private ToolStripButton btnAutoRefresh;
		private ToolStripMenuItem mnuFillTableWith10LastRecords;
		private ToolStripMenuItem mnuFillTableWithAllRecords;
		private DataGridViewTextBoxColumn Column1;
		private DataGridViewTextBoxColumn Column2;
		private DataGridViewTextBoxColumn Column4;
		private DataGridViewTextBoxColumn Column3;
		private DataGridViewTextBoxColumn Column5;
		private ToolStripStatusLabel lblNumRecordsStatusBar;
		private ToolStripStatusLabel lblVerifyState;
		private ToolStripStatusLabel lblLastUpdated;
	}
}