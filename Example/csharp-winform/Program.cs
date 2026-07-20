// ============================================================
// C# WinForms - DWG to SVG Converter (Full API)
//
// Prerequisites:
//   regsvr32 "path\to\64bit\DWG2SVGX64.dll"   (64-bit)
//   regsvr32 "path\to\DWG2SVGX.dll"           (32-bit)
//   .NET 8.0 SDK
//
// Build: dotnet build    Run: dotnet run
// ============================================================

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DWG2SVGConverter
{
    static class Program
    {
        [STAThread]
        static void Main() { Application.EnableVisualStyles(); Application.SetCompatibleTextRenderingDefault(false); Application.Run(new MainForm()); }
    }

    public class MainForm : Form
    {
        private TextBox txtInput, txtOutput, txtFontPath;
        private ComboBox cmbColor, cmbLayout;
        private NumericUpDown nudW, nudH;
        private CheckBox chkLayoutSize, chkExplodeShx;
        private NumericUpDown nudOutputLayer, nudHatchType, nudOutputType;
        private TextBox txtLineWeightScale, txtOutScale, txtConvertType;
        private Button btnConvert;
        private Label lblStatus;
        private TextBox txtLog;

        public MainForm()
        {
            Text = "AutoDWG DWG to SVG Converter";
            Size = new Size(620, 680);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            int y = 10;

            // Input
            Controls.Add(new Label { Text = "Input DWG:", Location = new Point(15, y + 3), Width = 90 });
            txtInput = new TextBox { Location = new Point(110, y), Width = 370 };
            Controls.Add(txtInput);
            var btnIn = new Button { Text = "...", Location = new Point(485, y), Width = 35, Height = 25 };
            btnIn.Click += (s, e) => { using var d = new OpenFileDialog { Filter = "DWG|*.dwg" }; if (d.ShowDialog() == DialogResult.OK) txtInput.Text = d.FileName; };
            Controls.Add(btnIn);
            y += 30;

            // Output
            Controls.Add(new Label { Text = "Output Path:", Location = new Point(15, y + 3), Width = 90 });
            txtOutput = new TextBox { Location = new Point(110, y), Width = 370, Text = @"f:\" };
            Controls.Add(txtOutput);
            var btnOut = new Button { Text = "...", Location = new Point(485, y), Width = 35, Height = 25 };
            btnOut.Click += (s, e) => { using var d = new FolderBrowserDialog(); if (d.ShowDialog() == DialogResult.OK) txtOutput.Text = d.SelectedPath + "\\"; };
            Controls.Add(btnOut);
            y += 35;

            // Basic settings group
            var grpBasic = new GroupBox { Text = "Basic Settings", Location = new Point(15, y), Size = new Size(560, 100) };
            grpBasic.Controls.Add(new Label { Text = "Color Type:", Location = new Point(10, 22), Width = 80 });
            cmbColor = new ComboBox { Location = new Point(95, 20), Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbColor.Items.AddRange(new object[] { "0 - True Color", "1 - Black & White", "2 - Gray" }); cmbColor.SelectedIndex = 0;
            grpBasic.Controls.Add(cmbColor);
            grpBasic.Controls.Add(new Label { Text = "Layout Type:", Location = new Point(270, 22), Width = 80 });
            cmbLayout = new ComboBox { Location = new Point(355, 20), Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbLayout.Items.AddRange(new object[] { "0 - All Layouts", "1 - Model Space", "2 - Paper Space" }); cmbLayout.SelectedIndex = 0;
            grpBasic.Controls.Add(cmbLayout);
            grpBasic.Controls.Add(new Label { Text = "Size W:", Location = new Point(10, 52), Width = 50 });
            nudW = new NumericUpDown { Location = new Point(65, 50), Width = 70, Minimum = 100, Maximum = 10000, Value = 800 };
            grpBasic.Controls.Add(nudW);
            grpBasic.Controls.Add(new Label { Text = "H:", Location = new Point(140, 52), Width = 20 });
            nudH = new NumericUpDown { Location = new Point(165, 50), Width = 70, Minimum = 100, Maximum = 10000, Value = 600 };
            grpBasic.Controls.Add(nudH);
            grpBasic.Controls.Add(new Label { Text = "Font Path:", Location = new Point(270, 52), Width = 75 });
            txtFontPath = new TextBox { Location = new Point(350, 50), Width = 180, Text = @"C:\fonts\" };
            grpBasic.Controls.Add(txtFontPath);
            Controls.Add(grpBasic);
            y += 110;

            // Advanced settings group
            var grpAdv = new GroupBox { Text = "Advanced Settings (optional)", Location = new Point(15, y), Size = new Size(560, 130) };
            grpAdv.Controls.Add(new Label { Text = "OutputLayer:", Location = new Point(10, 22), Width = 80 });
            nudOutputLayer = new NumericUpDown { Location = new Point(95, 20), Width = 60, Minimum = 0, Maximum = 10 };
            grpAdv.Controls.Add(nudOutputLayer);
            grpAdv.Controls.Add(new Label { Text = "HatchType:", Location = new Point(170, 22), Width = 70 });
            nudHatchType = new NumericUpDown { Location = new Point(245, 20), Width = 60, Minimum = 0, Maximum = 10 };
            grpAdv.Controls.Add(nudHatchType);
            grpAdv.Controls.Add(new Label { Text = "OutputType:", Location = new Point(320, 22), Width = 75 });
            nudOutputType = new NumericUpDown { Location = new Point(400, 20), Width = 60, Minimum = 0, Maximum = 10 };
            grpAdv.Controls.Add(nudOutputType);

            grpAdv.Controls.Add(new Label { Text = "LineWeightScale:", Location = new Point(10, 52), Width = 100 });
            txtLineWeightScale = new TextBox { Location = new Point(115, 50), Width = 60, Text = "1.0" };
            grpAdv.Controls.Add(txtLineWeightScale);
            grpAdv.Controls.Add(new Label { Text = "OutScale:", Location = new Point(190, 52), Width = 65 });
            txtOutScale = new TextBox { Location = new Point(260, 50), Width = 60, Text = "1.0" };
            grpAdv.Controls.Add(txtOutScale);
            grpAdv.Controls.Add(new Label { Text = "ConvertType:", Location = new Point(340, 52), Width = 80 });
            txtConvertType = new TextBox { Location = new Point(425, 50), Width = 100 };
            grpAdv.Controls.Add(txtConvertType);

            chkLayoutSize = new CheckBox { Text = "ConvertAsLayoutSizeOut", Location = new Point(10, 82), Width = 170 };
            grpAdv.Controls.Add(chkLayoutSize);
            chkExplodeShx = new CheckBox { Text = "ExplodeShxText", Location = new Point(190, 82), Width = 130 };
            grpAdv.Controls.Add(chkExplodeShx);
            Controls.Add(grpAdv);
            y += 140;

            // Convert
            btnConvert = new Button { Text = "Convert to SVG", Location = new Point(200, y), Size = new Size(180, 35), BackColor = Color.FromArgb(0, 120, 212), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnConvert.Click += (s, e) => DoConvert();
            Controls.Add(btnConvert);
            y += 45;

            // Status
            lblStatus = new Label { Text = "Ready", Location = new Point(15, y), Width = 500, ForeColor = Color.Gray };
            Controls.Add(lblStatus);
            y += 25;

            // Log
            txtLog = new TextBox { Location = new Point(15, y), Size = new Size(560, 100), Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, BackColor = Color.FromArgb(245, 245, 245) };
            Controls.Add(txtLog);
        }

        private void Log(string msg) => txtLog.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + msg + "\r\n");

        private void DoConvert()
        {
            if (string.IsNullOrWhiteSpace(txtInput.Text) || !System.IO.File.Exists(txtInput.Text))
            { MessageBox.Show("Please select a valid DWG file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            btnConvert.Enabled = false;
            lblStatus.Text = "Converting..."; lblStatus.ForeColor = Color.Blue;
            Log("Starting conversion...");

            try
            {
                Type comType = Type.GetTypeFromProgID("DWG2SVGX.DWG2SVG");
                dynamic obj = Activator.CreateInstance(comType);

                // Basic
                string fp = txtFontPath.Text.Trim();
                if (!string.IsNullOrEmpty(fp)) obj.AddFontPath(fp);
                obj.ColorType = cmbColor.SelectedIndex;
                obj.LayoutType = cmbLayout.SelectedIndex;
                obj.SetSize((int)nudW.Value, (int)nudH.Value);

                // Advanced
                obj.OutputLayer = (short)nudOutputLayer.Value;
                obj.HatchType = (short)nudHatchType.Value;
                obj.Outputtype = (short)nudOutputType.Value;
                obj.ConvertAsLayoutSizeOut = (short)(chkLayoutSize.Checked ? 1 : 0);
                obj.ExplodeShxText = (short)(chkExplodeShx.Checked ? 1 : 0);
                if (double.TryParse(txtLineWeightScale.Text, out double lws)) obj.LineWeightScale = lws;
                if (float.TryParse(txtOutScale.Text, out float os)) obj.OutScale = os;
                string ct = txtConvertType.Text.Trim();
                if (!string.IsNullOrEmpty(ct)) obj.SetConvertType(ct);

                Log($"Settings: Color={cmbColor.SelectedIndex}, Layout={cmbLayout.SelectedIndex}, Size={nudW.Value}x{nudH.Value}");

                obj.Convert(txtInput.Text, txtOutput.Text);

                Marshal.ReleaseComObject(obj);

                // Scan output directory for generated SVG files (named as {dwgName}-{layoutName}.svg)
                string dwgBaseName = System.IO.Path.GetFileNameWithoutExtension(txtInput.Text);
                var svgFiles = System.IO.Directory.GetFiles(txtOutput.Text, dwgBaseName + "-*.svg");
                int fileCount = svgFiles.Length;
                foreach (var sf in svgFiles) Log("  Generated: " + System.IO.Path.GetFileName(sf));

                lblStatus.Text = "Done!"; lblStatus.ForeColor = Color.Green;
                MessageBox.Show($"Conversion completed!\n\nGenerated {fileCount} SVG file(s).\n\nOutput: {txtOutput.Text}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { Log("ERROR: 0x" + (ex.HResult & 0xFFFFFFFF).ToString("X8")); lblStatus.Text = "Error!"; lblStatus.ForeColor = Color.Red; MessageBox.Show("Conversion failed. Please check the input file and settings, then try again.\n\nError: 0x" + (ex.HResult & 0xFFFFFFFF).ToString("X8"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { btnConvert.Enabled = true; }
        }
    }
}