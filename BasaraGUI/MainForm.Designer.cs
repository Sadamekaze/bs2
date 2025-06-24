using System.Drawing.Drawing2D;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace BasaraGUI
{
    public class CustomLabel : Label
    {
        public CustomLabel()
        {
            OutlineForeColor = Color.Black;
            OutlineWidth = 1.5f;
        }
        public Color OutlineForeColor { get; set; }
        public float OutlineWidth { get; set; }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
            using (GraphicsPath gp = new GraphicsPath())
            using (Pen outline = new Pen(OutlineForeColor, OutlineWidth)
            { LineJoin = LineJoin.Round })
            using (StringFormat sf = new StringFormat())
            using (Brush foreBrush = new SolidBrush(ForeColor))
            {
                gp.AddString(Text, Font.FontFamily, (int)Font.Style,
                    Font.Size, ClientRectangle, sf);
                e.Graphics.ScaleTransform(1.3f, 1.35f);
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.DrawPath(outline, gp);
                e.Graphics.FillPath(foreBrush, gp);
            }
        }
    }
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /// 


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.Export = new System.Windows.Forms.Button();
            this.Import = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.CustomTable = new System.Windows.Forms.Button();
            this.SADAMEKAZE = new BasaraGUI.CustomLabel();
            this.SuspendLayout();
            // 
            // Export
            // 
            this.Export.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Export.Location = new System.Drawing.Point(13, 178);
            this.Export.Name = "Export";
            this.Export.Size = new System.Drawing.Size(73, 26);
            this.Export.TabIndex = 0;
            this.Export.TabStop = false;
            this.Export.Text = "Export";
            this.Export.UseVisualStyleBackColor = true;
            this.Export.Click += new System.EventHandler(this.ExportClick);
            // 
            // Import
            // 
            this.Import.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Import.Location = new System.Drawing.Point(378, 178);
            this.Import.Name = "Import";
            this.Import.Size = new System.Drawing.Size(73, 26);
            this.Import.TabIndex = 1;
            this.Import.TabStop = false;
            this.Import.Text = "Import";
            this.Import.UseVisualStyleBackColor = true;
            this.Import.Click += new System.EventHandler(this.ImportClick);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "PACK TOOL",
            "MSG TOOL"});
            this.comboBox1.Location = new System.Drawing.Point(283, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(168, 21);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.TabStop = false;
            // 
            // CustomTable
            // 
            this.CustomTable.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CustomTable.Location = new System.Drawing.Point(373, 34);
            this.CustomTable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CustomTable.Name = "CustomTable";
            this.CustomTable.Size = new System.Drawing.Size(79, 20);
            this.CustomTable.TabIndex = 2;
            this.CustomTable.TabStop = false;
            this.CustomTable.Text = "Custom Table";
            this.CustomTable.UseVisualStyleBackColor = true;
            this.CustomTable.Click += new System.EventHandler(this.CustomTableClick);
            // 
            // SADAMEKAZE
            // 
            this.SADAMEKAZE.AutoSize = true;
            this.SADAMEKAZE.BackColor = System.Drawing.Color.Transparent;
            this.SADAMEKAZE.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SADAMEKAZE.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SADAMEKAZE.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.SADAMEKAZE.Location = new System.Drawing.Point(166, 185);
            this.SADAMEKAZE.Name = "SADAMEKAZE";
            this.SADAMEKAZE.OutlineForeColor = System.Drawing.Color.Black;
            this.SADAMEKAZE.OutlineWidth = 1.5F;
            this.SADAMEKAZE.Size = new System.Drawing.Size(121, 19);
            this.SADAMEKAZE.TabIndex = 4;
            this.SADAMEKAZE.Text = "SADAMEKAZE";
            this.SADAMEKAZE.Click += new System.EventHandler(this.HandleMouseClick);
            this.SADAMEKAZE.MouseLeave += new System.EventHandler(this.HandleMouseLeave);
            this.SADAMEKAZE.MouseHover += new System.EventHandler(this.HandleMouseHover);
            // 
            // MainForm
            // 
            this.AutoSize = true;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(464, 216);
            this.Controls.Add(this.CustomTable);
            this.Controls.Add(this.SADAMEKAZE);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.Import);
            this.Controls.Add(this.Export);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BasaraGUI PACK and MSG Tools";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        

        private System.Windows.Forms.Button Export;
        private System.Windows.Forms.Button Import;
        private System.Windows.Forms.ComboBox comboBox1;
        private CustomLabel SADAMEKAZE;
        private System.Windows.Forms.Button CustomTable;
    }
}

