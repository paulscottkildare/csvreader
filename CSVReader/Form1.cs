using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSVReader {
	public partial class Form1 : Form {
		private string filename = "";
		private bool gridDirty = false;
		private bool firstRowHeader = true;
		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			
		}

		private void exitMenuItem_Click(object sender, EventArgs e) {
			Close();
		}

		private void openMenuItem_Click(object sender, EventArgs e) {
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				csvgrid.Columns.Clear();
				csvgrid.Rows.Clear();
				string file = File.ReadAllText(openFileDialog1.FileName);
				StringReader reader = new StringReader(file);
				CsvParser csv = new CsvParser();
				string[][] dataStrings = csv.Parse(reader);
				filename = openFileDialog1.FileName;
				populateGrid(dataStrings);
				this.Focus();
			}
		}

		private void populateGrid(string[][] dataStrings) {
			csvgrid.RowsDefaultCellStyle.BackColor = Color.Azure;
			csvgrid.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
			csvgrid.CellBorderStyle = DataGridViewCellBorderStyle.None;
			if (dataStrings.Length > 1) {
				for (uint i = 0; i < dataStrings[0].Length; i++) {
					csvgrid.Columns.Add(dataStrings[0][i], firstRowHeader?dataStrings[0][i]:(i+1)+"");
				}
				//int start
				for (uint i = (uint)(firstRowHeader?1:0); i < dataStrings.Length; i++) {
					csvgrid.Rows.Add(dataStrings[i]);
				}
				csvgrid.AutoResizeColumns();
				this.Text = "CSV Reader - " + filename;
				statustext.Text = (dataStrings.Length - 1) + " Rows";
			}
			else {
				for (uint i = 0; i < dataStrings[0].Length; i++) {
					csvgrid.Columns.Add(dataStrings[0][i], (i + 1) +  "");
				}
				csvgrid.Rows.Add(dataStrings[0]);
				csvgrid.AutoResizeColumns();
				this.Text = "CSV Reader - " + filename;
				statustext.Text = (dataStrings[0].Length) + " Columns, 1 Row";
			}
			csvgrid.Refresh();
			csvgrid.CurrentCell = csvgrid[0, 0];
			gridDirty = false;
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			AboutBox1 a = new AboutBox1();
			a.ShowDialog();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			if (gridDirty) {
				MessageBox.Show("Your changes will not be saved", "Read only", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void csvgrid_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			gridDirty = true;
			statustext.Text = "Changed";
		}

		private void closeButton_Click(object sender, EventArgs e) {
			Close();
		}

		private void Form1_Shown(object sender, EventArgs e) {
			if (Environment.GetCommandLineArgs().Length == 1) return;
			string[] args = Environment.GetCommandLineArgs();

			string file = File.ReadAllText(args[1]);
			StringReader reader = new StringReader(file);
			CsvParser csv = new CsvParser();
			string[][] dataStrings = csv.Parse(reader);
			filename = args[1];
			headerButton.Checked = firstRowHeader;
			headerOptionMenuItem.Checked = firstRowHeader;
			populateGrid(dataStrings);
			this.Focus();
		}

		private void csvgrid_DragEnter(object sender, DragEventArgs e) {
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.All;
			else
				e.Effect = DragDropEffects.None;
			statustext.Text = "DragEnter";
		}

		private void csvgrid_DragDrop(object sender, DragEventArgs e) {
			CsvParser csv = new CsvParser();
			//StringReader sr = new StringReader(e.Data.GetData(DataFormats.Text).ToString());
			csvgrid.Columns.Clear();
			csvgrid.Rows.Clear();
			string filename = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
			//string file = File.ReadAllText(e.Data.GetData(DataFormats.FileDrop));
			string file = File.ReadAllText(filename);
			StringReader reader = new StringReader(file);
			string[][] dataStrings = csv.Parse(reader);
			filename = openFileDialog1.FileName;
			populateGrid(dataStrings);
			this.Focus();
		}

		private void headerButton_Click(object sender, EventArgs e) {
			
		}

		private void useFirstRowAsHeadersToolStripMenuItem_Click(object sender, EventArgs e) {

		}

		private void headerButton_Click_1(object sender, EventArgs e) {
			if (headerButton.Checked) {
				headerButton.Checked = false;
				headerOptionMenuItem.Checked = false;
				firstRowHeader = false;
			}
			else {
				headerButton.Checked = true;
				headerOptionMenuItem.Checked = true;
				firstRowHeader = true;
			}
		}

	}
}
