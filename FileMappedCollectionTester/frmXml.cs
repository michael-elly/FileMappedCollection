using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;

namespace FileMappedCollectionTester {
	public partial class frmXml : Form {
		public frmXml() {
			InitializeComponent();
		}

		internal string mXmlText;

		private void frmXml_Load(object sender, EventArgs e) {
			txtXml.Dock = DockStyle.Fill;
			txtXml.Text = PrintFriendlyXML(mXmlText);

		}

		private void txtXml_KeyDown(object sender, KeyEventArgs e) {
			if (e.Control && e.KeyCode == Keys.V) {
				if (Clipboard.GetText() != null)
					txtXml.SelectedText = (string)Clipboard.GetText(); // .GetData("Text");
				e.Handled = true;
			} else if (e.KeyCode == Keys.Escape) {
				this.Close();
			}

		}

		public static string PrintFriendlyXML(string xml) {
			try {
				var stringBuilder = new StringBuilder();
				var element = XElement.Parse(xml);

				var settings = new XmlWriterSettings();
				settings.OmitXmlDeclaration = true;
				settings.Indent = true;
				settings.NewLineOnAttributes = true;

				using (var xmlWriter = XmlWriter.Create(stringBuilder, settings)) {
					element.Save(xmlWriter);
				}
				return stringBuilder.ToString();
			} catch {
				return xml;
			}
		}

	}
}
