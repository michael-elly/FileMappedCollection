using System.Data;
using System.Windows.Forms;
using FileMappedCollectionViewer;

namespace FileMappedCollectionTester;


public partial class frmMain : Form {
	public frmMain() {
		InitializeComponent();
	}

	private void Form1_Load(object sender, EventArgs e) {
		grdRecords.Dock = DockStyle.Fill;
		txtProperties.Dock = DockStyle.Fill;
		splitContainer1.Dock = DockStyle.Fill;
		grdRecords.BorderStyle = BorderStyle.None;
		txtProperties.BorderStyle = BorderStyle.None;
		
		// setup the timer which should be disabled initially
		aTimer = new System.Timers.Timer(2000);
		//aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
		aTimer.Elapsed += this.ATimer_Elapsed;
		aTimer.Interval = 800;
		aTimer.Enabled = false;

		string[] args = Environment.GetCommandLineArgs();
		if (args.Length > 1) {
			if (File.Exists(args[1])) {
				if (Path.GetExtension(args[1]).ToLower() == ".amc") {
					txtPath.Text = args[1];
					LoadFile();
				}
			}
		} else {
			LoadFile();
		}
	}

	public delegate void UpdateRefresh();
	FileMappedCollection mRecords;
	string path;
	short init_size_mb; // file size when first created or after shrinked
	int size_bytes; // current file size
	short size_extension_mb;
	byte max_extensions;
	int max_size_bytes;
	int size_increment_bytes;
	bool regerenate_file_on_error;
	bool enable_auto_shrink;

	private static System.Timers.Timer aTimer;

	private void ATimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
		this.Invoke(new UpdateRefresh(this.RefreshControlsNow));
	}

	private void LoadFile() {
		frmNewFile f = new frmNewFile();
		f.PopulateValuesFromControlsToPublicFields();
		ReadFileSettings();

		mRecords = new FileMappedCollection(path, regerenate_file_on_error, init_size_mb, size_extension_mb, max_extensions, enable_auto_shrink);
		RefreshControlsNow();

		// Empty the Data Grid
		DataTable d = GetEmptyGridDateTable();
		grdRecords.DataSource = d;
	}

	private void RefreshControlsNow() {
		string err_msg;
		bool is_ok = mRecords.VerifyConsistency(out err_msg);
		List<FileMappedCollection.Record> l;
		bool have_read_all_available_records_on_file;

		init_size_mb = (short) (mRecords.FileSizeInitialBytes / 1024/1024);
		size_bytes = mRecords.FileSizeBytes;
		max_size_bytes = mRecords.FileSizeMaxBytes;
		size_increment_bytes = mRecords.FileSizeIncrementBytes;

		lblVerifyState.ToolTipText = err_msg;
		lblVerifyState.Image = (is_ok ? FileMappedCollectionViewer.Properties.Resources.StateOK : FileMappedCollectionViewer.Properties.Resources.StateFail);
		lblVerifyState.Text = (is_ok ? "Healthy" : "Failed");

		if (is_ok) {
			if (mRecords.TryPeek(out l, out have_read_all_available_records_on_file)) {
				int records_count = l.Count;
				int total_used_bytes = 0;
				int total_unused_bytes;
				double percent_free;
				double percent_free_immidiate;
				int immidiate_unused_bytes;

				if (records_count > 0) {
					foreach (FileMappedCollection.Record r in l) {
						total_used_bytes += r.RecordSize;
					}

					total_unused_bytes = size_bytes - total_used_bytes - 16;
					percent_free = ((100 * (double)total_unused_bytes)) / size_bytes;
					immidiate_unused_bytes = size_bytes - (l[l.Count - 1].StartAddress + l[l.Count - 1].RecordSize);
					percent_free_immidiate = ((100 * (double)immidiate_unused_bytes)) / size_bytes;

					RecolorFileImage(l);
				} else {
					total_unused_bytes = 0;
					percent_free = 100;
					percent_free_immidiate = 100;

					ClearPictureImage();
				}

				txtProperties.Text = @$"File Size: {(size_bytes / 1024 / 1024):N0}MB, Initial Size: {init_size_mb:N0}MB, Max Size: {max_size_bytes / 1024 / 1024:N0}MB, Increment Size: {size_increment_bytes / 1024 / 1024}MB
Records Count: {records_count:N0}, Percent Free: {percent_free:N1}%, Percent Free Immidiate: {percent_free_immidiate:N1}%
";
				lblNumRecordsStatusBar.Text = string.Format("Records Count: {0:#,0}", records_count);				
				lblLastUpdated.Text = string.Format("Last Updated: {0}", DateTime.Now.ToString("HH:mm:ss"));
			}

			// read the header file
			int first_rec_start_address;
			int last_rec_start_address;
			string h;
			if (mRecords.ReadHeader(out first_rec_start_address, out last_rec_start_address)) {
				h = $"First Record Start Address: {first_rec_start_address:N0}, Last Record Start Address: {last_rec_start_address:N0}";
			} else {
				h = "Error reading header";
			}
			txtProperties.AppendText($"\r\n{h}");

		}
	}

	private Color mImgaeBackgroundColor = Color.LightBlue;
	private Brush mImgaeUtilizedBrush = new SolidBrush(Color.DarkBlue);

	private void ClearPictureImage() {
		Bitmap b = new Bitmap(picFile.Width, picFile.Height);
		Graphics g = Graphics.FromImage(b);
		g.Clear(mImgaeBackgroundColor);
		picFile.Image = b;
	}

	private void RecolorFileImage(List<FileMappedCollection.Record> l) {
		List<Rectangle> boxes = new List<Rectangle>();
		double f = ((double)picFile.Width) / size_bytes; //factor
		int H = picFile.Height;
		int w; //width

		Bitmap b = new Bitmap(picFile.Width, picFile.Height);
		Graphics g = Graphics.FromImage(b);
		g.Clear(mImgaeBackgroundColor);

		foreach (FileMappedCollection.Record r in l) {
			w = (int)Math.Ceiling((r.RecordSize + 16) * f);
			if (w == 0) w = 1;
			boxes.Add(new Rectangle((int)Math.Floor(r.StartAddress * f), 0, w, H));
		}

		foreach (Rectangle r in boxes) {
			g.FillRectangle(mImgaeUtilizedBrush, r);
		}

		//g.FillRectangle(Brushes.Blue, new RectangleF(100, 0, 30, H));
		picFile.Image = b;
	}

	private void btnRefresh_Click(object sender, EventArgs e) {
		RefreshControlsNow();
	}

	private void mnuVerify_Click(object sender, EventArgs e) {
		RefreshControlsNow();
	}

	private void mnuShrink_Click(object sender, EventArgs e) {
		if (MessageBox.Show("Shrink File to Initial Size?", "Confirm Resize", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes) {
			if (mRecords.ShrinkFile()) {
				RefreshControlsNow();
			}
		}
	}

	private void mnuExtend_Click(object sender, EventArgs e) {
		if (mRecords.ExtendFile()) {
			RefreshControlsNow();
		}

	}

	private void mnuDefrag_Click(object sender, EventArgs e) {
		int bytes_released;
		if (mRecords.Defrag(out bytes_released)) {
			RefreshControlsNow();
		}

	}

	private void mnuClose_Click(object sender, EventArgs e) {
		this.Close();

	}

	private void btnAutoRefresh_Click(object sender, EventArgs e) {
		aTimer.Enabled = btnAutoRefresh.Checked;

	}

	private void mnuRefreshMap_Click(object sender, EventArgs e) {
		RefreshControlsNow();

	}

	private void mnuCopyCellText_Click(object sender, EventArgs e) {
		if (grdRecords.Rows.Count > 0) {
			if (grdRecords.CurrentCell != null) {
				try {
					Clipboard.SetText(grdRecords.CurrentCell.Value.ToString());
				} catch { }
			}
		}

	}

	private void mnuExpertMode_DropDownOpening(object sender, EventArgs e) {
		bool enabled = (grdRecords.Rows.Count > 0 && grdRecords.CurrentCell != null); ;
		mnuPeekSelectedRecord.Enabled = enabled;
		mnuPeekSelectedRecord.Enabled = enabled;

	}

	private void mnuPeekSelectedRecord_Click(object sender, EventArgs e) {
		if (grdRecords.CurrentCell != null) {
			byte[] b;
			int rec_start_address = int.Parse(grdRecords.Rows[grdRecords.CurrentCell.RowIndex].Cells[1].Value.ToString());
			FileMappedCollection.RecordRetreivalStatus s;
			s = mRecords.TryPeekAt(rec_start_address, out b);
			MessageBox.Show(string.Format("Peek status for the record in position {0} is {1} with {2} bytes in the message peeked.", rec_start_address, s.ToString(), (b == null ? 0 : b.Length)),
				"Peek", MessageBoxButtons.OK, (s == FileMappedCollection.RecordRetreivalStatus.Success ? MessageBoxIcon.Information : MessageBoxIcon.Exclamation));
		}

	}

	private void mnuDequeueSelectedRecord_Click(object sender, EventArgs e) {
		if (grdRecords.CurrentCell != null) {
			if (MessageBox.Show("Are you sure you want to remove a record from the collection?", "Confirm",
				MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3) == DialogResult.Yes) {
				int rec_start_address = int.Parse(grdRecords.Rows[grdRecords.CurrentCell.RowIndex].Cells[1].Value.ToString());
				FileMappedCollection.RecordRetreivalStatus s = mRecords.TryRemoveAt(rec_start_address);
				MessageBox.Show(string.Format("Dequeue status for the record in position {0} is {1}", rec_start_address, s.ToString()),
					"Dequeue", MessageBoxButtons.OK, (s == FileMappedCollection.RecordRetreivalStatus.Success ? MessageBoxIcon.Information : MessageBoxIcon.Exclamation));
			}
		}

	}

	private void PopulateRecords(int count, bool reverse) {
		int i;
		List<FileMappedCollection.Record> l;
		bool have_read_all_available_records_on_file;

		this.Cursor = Cursors.WaitCursor;

		if (mRecords.TryPeek(out l, out have_read_all_available_records_on_file)) {
			if (l.Count > 0) {
				if (reverse) l.Reverse();
				i = 0;
				//foreach (FileMappedCollection.Record r in l) {
				//	Console.Write("{0} ", i); if (i % 30 == 0) Console.WriteLine();
				//	try {
				//		grdRecords.Rows.Add(new object[] { ++i, r.StartAddress, r.NextRecordStartAddress, r.RecordSize, FileMappedCollection.ByteArrayToString(r.RecordBody) });
				//	} catch (Exception ex) {
				//		Console.WriteLine(ex.Message);
				//	}
				//}

				DataTable d = GetEmptyGridDateTable();
				// fill in the datatable
				DataRow relation;
				foreach (FileMappedCollection.Record r in l) {
					object[] rowArray = new object[] { ++i, r.StartAddress, r.NextRecordStartAddress, r.RecordSize, FileMappedCollection.ByteArrayToString(r.RecordBody) };
					relation = d.NewRow();
					relation.ItemArray = rowArray;
					d.Rows.Add(relation);
					if (count > 0 && i >= count) break;
				}
				// make the datagridview to display the populated datatble
				grdRecords.DataSource = d;
			}
		}

		this.Cursor = Cursors.Default;

	}

	private DataTable GetEmptyGridDateTable() {
		//create a datatable with the same scheme as the datagridview					
		DataTable d = new DataTable();
		foreach (DataGridViewColumn col in grdRecords.Columns) {
			d.Columns.Add(col.Name);
			// make sure property matches so we can later copy the datatable directly to the datagridview
			col.DataPropertyName = col.Name;
		}
		return d;
	}

	private void mnuFillTableWith10LastRecords_Click(object sender, EventArgs e) {
		PopulateRecords(10, true);

	}

	private void mnuFillTableWithAllRecords_Click(object sender, EventArgs e) {
		PopulateRecords(-1, false);

	}

	private void grdRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
		if (e != null && e.RowIndex >= 0 && e.ColumnIndex == 4 && grdRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null) {
			frmXml f = new frmXml();
			f.mXmlText = grdRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
			f.ShowDialog(this);
		}
	}

	private void mnuTruncate_Click(object sender, EventArgs e) {		
		if (mRecords.TryClear()) {
			RefreshControlsNow();
		}
	}

	private void mnuNewFMC_Click(object sender, EventArgs e) {
		frmNewFile f = new frmNewFile();
		if (mRecords != null) { f.PopulateValuesFromFmcObjectToPublicFields(mRecords); }
		f.ShowDialog(this);

		ReadFileSettings();			
		mRecords = new FileMappedCollection(path, regerenate_file_on_error, init_size_mb, size_extension_mb, max_extensions, enable_auto_shrink);
		RefreshControlsNow();

		// Empty the Data Grid
		DataTable d = GetEmptyGridDateTable();
		grdRecords.DataSource = d;

	}

	private void ReadFileSettings() {
		path = frmNewFile.path;
		init_size_mb = frmNewFile.init_size_mb;
		size_extension_mb = frmNewFile.size_extension_mb;
		max_extensions = frmNewFile.max_extensions;
		size_bytes = frmNewFile.size_bytes;
		regerenate_file_on_error = frmNewFile.regerenate_file_on_error;
		enable_auto_shrink = frmNewFile.enable_auto_shrink;
		
		// also update the UI only for the Path
		txtPath.Text = path;
	}

	private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e) {
			
	}
}
