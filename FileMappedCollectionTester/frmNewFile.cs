using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FileMappedCollection;

namespace FileMappedCollectionViewer {
	public partial class frmNewFile : Form {
		public frmNewFile() {
			InitializeComponent();
		}

		internal static string path;
		internal static int size_bytes;
		internal static short init_size_mb;
		internal static short size_extension_mb;
		internal static byte max_extensions;
		internal static bool regerenate_file_on_error;
		internal static bool enable_auto_shrink;

		private void frmNewFile_Load(object sender, EventArgs e) {

		}

		public void PopulateValuesFromControlsToPublicFields() {
			path = txtPath.Text;
			if (File.Exists(path)) {
				size_bytes = (int)(new FileInfo(path)).Length;
			} else {
				size_bytes = (int)nudInitialSizeMB.Value * 10124 * 1024;
			}

			init_size_mb = (short)nudInitialSizeMB.Value;
			size_extension_mb = (short)nudExtensionsizeMB.Value;
			max_extensions = (byte)nudMaxExtensions.Value;
			regerenate_file_on_error = chkRegenerateFile.Checked;
			enable_auto_shrink = chkAutoShrink.Checked;

			SetFileBackground();
		}

		public void PopulateValuesFromFmcObjectToPublicFields(FileMappedCollection mRecords) {
			path = mRecords.FilePath;
			init_size_mb = (short)(mRecords.FileSizeInitialBytes / 1024 / 1024);
			size_extension_mb = mRecords.ExtensionSizeMB;
			max_extensions = mRecords.MaxAllowedExtensions;
			regerenate_file_on_error = mRecords.RegenerateFileIffoundCorrupt;
			enable_auto_shrink = mRecords.EnableAutoShrink;

			// now update the forms controls
			txtPath.Text = path;
			nudInitialSizeMB.Value = init_size_mb;
			nudExtensionsizeMB.Value = size_extension_mb;
			nudMaxExtensions.Value = max_extensions;
			chkRegenerateFile.Checked = regerenate_file_on_error;
			chkAutoShrink.Checked = enable_auto_shrink;

			SetFileBackground();
		}

		private void SetFileBackground() {
			txtPath.BackColor = File.Exists(path) ? Color.LightGreen : Color.LightCoral;
		}

		private void btnDelete_Click(object sender, EventArgs e) {
			try {
				File.Delete(path);
			} catch (Exception ex) {
				MessageBox.Show("Error: " + ex.Message);
			}

			SetFileBackground();
		}

		private void btnStoreToFields_Click(object sender, EventArgs e) {
			PopulateValuesFromControlsToPublicFields();
			this.Close();
		}
	}
}
