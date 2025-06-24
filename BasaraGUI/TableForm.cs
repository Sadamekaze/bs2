using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace BasaraGUI
{
    public partial class TableForm : Form
    {
        public TableForm()
        {
            InitializeComponent();
            string[] tableDefault = Properties.Resources.TableStrings.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string hexStrings in tableDefault)
            {
                string[] hexString = hexStrings.Split(new[] { '=' }, 2);
                dataGridView1.Rows.Add(hexString[0], hexString[1]);

            }

        }
        public DataGridView DataGridViewControl
        {
            get { return this.dataGridView1; }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == dataGridView1.Columns["deletebtn"].Index)
            {
                if (e.RowIndex != -1) dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxHex.Text) || string.IsNullOrEmpty(textBoxReplace.Text))
            {
                MessageBox.Show("Don't leave it empty.");
                return;
            }

            Regex regex = new Regex(@"^0x[0-9a-fA-F]+$");
            if (!regex.IsMatch(textBoxHex.Text))
            {
                MessageBox.Show("The hex part must start with \"0x\" and must be a valid hexadecimal.");
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value?.ToString() == textBoxHex.Text || row.Cells[1].Value?.ToString() == textBoxReplace.Text)
                {
                    MessageBox.Show("Duplicates are not allowed.");
                    return;
                }
            }
            dataGridView1.Rows.Add(textBoxHex.Text, textBoxReplace.Text);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialogTable = new SaveFileDialog();
            fileDialogTable.Filter = "TBL Files (*.tbl)|*.tbl";
            fileDialogTable.Title = "Select the TBL File";
            if (fileDialogTable.ShowDialog() == DialogResult.OK)
            {
                using StreamWriter tableExport = new StreamWriter(File.Create(fileDialogTable.FileName));
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    tableExport.WriteLine($"{row.Cells[0].Value.ToString()}={row.Cells[1].Value.ToString()}");
                }
            }
        }
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialogTable = new OpenFileDialog();
            fileDialogTable.Filter = "TBL Files (*.tbl)|*.tbl";
            fileDialogTable.Title = "Select the TBL File";
            if (fileDialogTable.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.Rows.Clear();
                string[] allTable = File.ReadAllLines(fileDialogTable.FileName);
                for (int i = 0; i < allTable.Length; i++)
                {
                    if (!string.IsNullOrEmpty(allTable[i]))
                    {
                        string[] keyAndValue = allTable[i].Split('=');
                        string key = keyAndValue[0];
                        string value = keyAndValue[1];
                        foreach (string entry in allTable.Skip(i + 1))
                        {
                            string[] stringSplit = entry.Split('=');
                            Regex regex = new Regex(@"^0x[0-9a-fA-F]+$");
                            if (!regex.IsMatch(stringSplit[0]))
                            {
                                MessageBox.Show($"The hex part must start with \"0x\" and must be a valid hexadecimal value.\n---->{stringSplit[0]}<----");
                                return;
                            }
                            if (key == stringSplit[0])
                            {
                                MessageBox.Show($"There is a duplicate on line {i + 1}");
                                return;
                            }
                            if (value == stringSplit[1])
                            {
                                MessageBox.Show($"There is a duplicate on line {i + 1}");
                                return;
                            }
                        }
                       dataGridView1.Rows.Add(key, value);
                    }
                }
            }
        }
    }
}
