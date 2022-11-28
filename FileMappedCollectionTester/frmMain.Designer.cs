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
			this.txtProperties = new System.Windows.Forms.TextBox();
			this.picFile = new System.Windows.Forms.PictureBox();
			this.grdRecords = new System.Windows.Forms.DataGridView();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripDropDownButton6 = new System.Windows.Forms.ToolStripDropDownButton();
			this.mnuNewFMC = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRefreshMap = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuVerify = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShrink = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuExtend = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDefrag = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTruncate = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuExpertMode = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPeekSelectedRecord = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDequeueSelectedRecord = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripDropDownButton5 = new System.Windows.Forms.ToolStripDropDownButton();
			this.mnuCopyCellText = new System.Windows.Forms.ToolStripMenuItem();
			this.btnAutoRefresh = new System.Windows.Forms.ToolStripButton();
			this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
			this.mnuFillTableWith10LastRecords = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFillTableWithAllRecords = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.txtPath = new System.Windows.Forms.ToolStripLabel();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.lblNumRecordsStatusBar = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblVerifyState = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblLastUpdated = new System.Windows.Forms.ToolStripStatusLabel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			((System.ComponentModel.ISupportInitialize)(this.picFile)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.grdRecords)).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtProperties
			// 
			this.txtProperties.Location = new System.Drawing.Point(9, 9);
			this.txtProperties.Margin = new System.Windows.Forms.Padding(9);
			this.txtProperties.Multiline = true;
			this.txtProperties.Name = "txtProperties";
			this.txtProperties.ReadOnly = true;
			this.txtProperties.Size = new System.Drawing.Size(416, 49);
			this.txtProperties.TabIndex = 0;
			// 
			// picFile
			// 
			this.picFile.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.picFile.Location = new System.Drawing.Point(0, 70);
			this.picFile.Name = "picFile";
			this.picFile.Size = new System.Drawing.Size(617, 20);
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
			this.grdRecords.Location = new System.Drawing.Point(9, 49);
			this.grdRecords.Name = "grdRecords";
			this.grdRecords.ReadOnly = true;
			this.grdRecords.RowTemplate.Height = 25;
			this.grdRecords.Size = new System.Drawing.Size(573, 66);
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
            this.btnAutoRefresh,
            this.toolStripDropDownButton2,
            this.toolStripSeparator1,
            this.txtPath});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(767, 25);
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripDropDownButton6
			// 
			this.toolStripDropDownButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButton6.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewFMC,
            this.toolStripMenuItem4,
            this.mnuRefreshMap,
            this.toolStripMenuItem1,
            this.mnuVerify,
            this.mnuShrink,
            this.mnuExtend,
            this.mnuDefrag,
            this.mnuTruncate,
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
			// mnuNewFMC
			// 
			this.mnuNewFMC.Image = global::FileMappedCollectionViewer.Properties.Resources.New_16x16;
			this.mnuNewFMC.Name = "mnuNewFMC";
			this.mnuNewFMC.Size = new System.Drawing.Size(159, 22);
			this.mnuNewFMC.Text = "New FMC...";
			this.mnuNewFMC.Click += new System.EventHandler(this.mnuNewFMC_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(156, 6);
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
			// mnuTruncate
			// 
			this.mnuTruncate.Name = "mnuTruncate";
			this.mnuTruncate.Size = new System.Drawing.Size(159, 22);
			this.mnuTruncate.Text = "Truncate";
			this.mnuTruncate.Click += new System.EventHandler(this.mnuTruncate_Click);
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
			// btnAutoRefresh
			// 
			this.btnAutoRefresh.CheckOnClick = true;
			this.btnAutoRefresh.Image = global::FileMappedCollectionViewer.Properties.Resources.Refresh;
			this.btnAutoRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnAutoRefresh.Name = "btnAutoRefresh";
			this.btnAutoRefresh.Size = new System.Drawing.Size(95, 22);
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
			this.toolStripDropDownButton2.Size = new System.Drawing.Size(61, 22);
			this.toolStripDropDownButton2.Text = "View";
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
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// txtPath
			// 
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(86, 22);
			this.txtPath.Text = "c:\\temp\\a.amc";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblNumRecordsStatusBar,
            this.lblVerifyState,
            this.lblLastUpdated});
			this.statusStrip1.Location = new System.Drawing.Point(0, 332);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(767, 25);
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
			// splitContainer1
			// 
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(12, 45);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.txtProperties);
			this.splitContainer1.Panel1.Controls.Add(this.picFile);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.grdRecords);
			this.splitContainer1.Size = new System.Drawing.Size(617, 219);
			this.splitContainer1.SplitterDistance = 90;
			this.splitContainer1.TabIndex = 6;
			this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(767, 357);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "frmMain";
			this.Text = "File Mapped Collection Manager";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.picFile)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.grdRecords)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
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
		private ToolStripMenuItem mnuTruncate;
		private TextBox txtProperties;
		private SplitContainer splitContainer1;
		private ToolStripMenuItem mnuNewFMC;
		private ToolStripSeparator toolStripMenuItem4;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripLabel txtPath;
	}
}