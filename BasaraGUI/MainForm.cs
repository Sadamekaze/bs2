using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Basara;
using FolderBrowserExDialog = FolderBrowserEx.FolderBrowserDialog;

namespace BasaraGUI
{
    public partial class MainForm : Form
    {
        private enum MethodExportImport{PACK = 0, MSG = 1};
        private TableForm tableForm = new TableForm();
        private DataGridView dataGridView1;
        private Dictionary<string, string> table = new Dictionary<string, string>();
        public MainForm()
        {
            InitializeComponent();;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;
            dataGridView1 = tableForm.DataGridViewControl;
        }        

        private void ExportClick(object sender, EventArgs e)
        {
            table.Clear();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string value = row.Cells[0].Value.ToString();
                string value1 = row.Cells[1].Value.ToString();
                table.Add(value, value1);
            }
            switch ((MethodExportImport)comboBox1.SelectedIndex)
            {
                case MethodExportImport.PACK:
                    OpenFileDialog fileDialogPACK = new OpenFileDialog();
                    fileDialogPACK.Filter = "All PACK Files (*.pack;*.pack_yz)|*.pack;*.pack_yz|PACK Files (*.pack)|*.pack|PACK_YZ Files (*.pack_yz)|*.pack_yz";
                    fileDialogPACK.Title = "Select the PACK File";

                    FolderBrowserExDialog pathFolderDialogPACK = new FolderBrowserExDialog();
                    pathFolderDialogPACK.AllowMultiSelect = false;
                    if (fileDialogPACK.ShowDialog() == DialogResult.OK)
                    {
                        string inputPath = fileDialogPACK.FileName;
                        if (pathFolderDialogPACK.ShowDialog() == DialogResult.OK)
                        {
                            string outputPath = pathFolderDialogPACK.SelectedFolder;
                            Pack.Export(inputPath, outputPath);
                        }
                    }
                    break;
                case MethodExportImport.MSG:
                    OpenFileDialog fileDialogSTR = new OpenFileDialog();
                    SaveFileDialog saveFileDialogSTR = new SaveFileDialog();
                    fileDialogSTR.Filter = "MSG Files (*.msg)|*.msg";
                    fileDialogSTR.Title = "Select the MSG File";
                    if (fileDialogSTR.ShowDialog() == DialogResult.OK)
                    {
                        string input = fileDialogSTR.FileName;
                        saveFileDialogSTR.Filter = "TXT Files (*.txt)|*.txt";
                        saveFileDialogSTR.Title = "Save the TXT File";
                        if (saveFileDialogSTR.ShowDialog() == DialogResult.OK)
                        {
                            string output = saveFileDialogSTR.FileName;
                            Msg.Export(input, output,table);
                        }
                    }
                    break;
            }
        }
        private void ImportClick(object sender, EventArgs e)
        {
            table.Clear();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string value = row.Cells[0].Value.ToString();
                string value1 = row.Cells[1].Value.ToString();
                table.Add(value1, value);
            }
            switch ((MethodExportImport)comboBox1.SelectedIndex)
            {
                case MethodExportImport.PACK:
                    FolderBrowserExDialog pathFolderDialogPACK = new FolderBrowserExDialog();
                    pathFolderDialogPACK.AllowMultiSelect = false;

                    SaveFileDialog saveFileDialogPACK = new SaveFileDialog();
                    saveFileDialogPACK.Filter = "All PACK Files (*.pack;*.pack_yz)|*.pack;*.pack_yz|PACK Files (*.pack)|*.pack|PACK_YZ Files (*.pack_yz)|*.pack_yz";
                    saveFileDialogPACK.Title = "Save the PACK File";

                    if (pathFolderDialogPACK.ShowDialog() == DialogResult.OK)
                    {
                        string inputPath = pathFolderDialogPACK.SelectedFolder;

                        if (saveFileDialogPACK.ShowDialog() == DialogResult.OK)
                        {
                            string outputPath = saveFileDialogPACK.FileName;
                            Pack.Import(inputPath, outputPath);
                        }
                    }
                    break;
                case MethodExportImport.MSG:
                    OpenFileDialog fileDialogSTR = new OpenFileDialog();
                    SaveFileDialog saveFileDialogSTR = new SaveFileDialog();
                    fileDialogSTR.Filter = "TXT Files (*.txt)|*.txt";
                    fileDialogSTR.Title = "Select the TXT File";
                    if (fileDialogSTR.ShowDialog() == DialogResult.OK)
                    {
                        string input = fileDialogSTR.FileName;
                        saveFileDialogSTR.Filter = "MSG Files (*.msg)|*.msg";
                        saveFileDialogSTR.Title = "Save the MSG File";
                        if (saveFileDialogSTR.ShowDialog() == DialogResult.OK)
                        {
                            string output = saveFileDialogSTR.FileName;
                            Msg.Import(input, output, table);
                        }
                    }
                    break;
            }
        }
        private void CustomTableClick(object sender, EventArgs e)
        {
            tableForm.ShowDialog();
        }
        private void HandleMouseClick(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/ryJQMZXHnK");
        }
        private void HandleMouseHover(object sender,EventArgs e)
        {
            this.SADAMEKAZE.Font = new System.Drawing.Font("Arial", 12F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }
        private void HandleMouseLeave(object sender, EventArgs e)
        {
            this.SADAMEKAZE.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }
    }
}
