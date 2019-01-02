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
using SoPechExcel.Files;
using SoPechExcel.Model;

namespace SoPechExcel
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            InitalComboBoxMonth();
            InitalComboBoxYear();

        }

        public void InitalComboBoxMonth()
        {
            List<ComboBoxItem> datasource = new List<ComboBoxItem>();
            datasource.Add(new ComboBoxItem() { Text = "-- Please Select --", Value = "0" });
            datasource.Add(new ComboBoxItem() { Text = "January", Value = "1" });
            datasource.Add(new ComboBoxItem() { Text = "February", Value = "2" });
            datasource.Add(new ComboBoxItem() { Text = "March", Value = "3" });
            datasource.Add(new ComboBoxItem() { Text = "April", Value = "4" });
            datasource.Add(new ComboBoxItem() { Text = "May", Value = "5" });
            datasource.Add(new ComboBoxItem() { Text = "June", Value = "6" });
            datasource.Add(new ComboBoxItem() { Text = "July", Value = "7" });
            datasource.Add(new ComboBoxItem() { Text = "August", Value = "8" });
            datasource.Add(new ComboBoxItem() { Text = "September", Value = "9" });
            datasource.Add(new ComboBoxItem() { Text = "October", Value = "10" });
            datasource.Add(new ComboBoxItem() { Text = "November", Value = "11" });
            datasource.Add(new ComboBoxItem() { Text = "December", Value = "12" });

            cbbMonth.DisplayMember = "Text";
            cbbMonth.ValueMember = "Value";
            cbbMonth.DataSource = datasource;
            cbbMonth.SelectedValue = DateTime.Today.Month.ToString();
        }

        public void InitalComboBoxYear()
        {
            List<ComboBoxItem> datasource = new List<ComboBoxItem>();
            datasource.Add(new ComboBoxItem() { Text = "-- Please Select --", Value = "0" });
            for (int i = 2018; i <= 2025; i++)
            {
                datasource.Add(new ComboBoxItem() { Text = i.ToString(), Value = i.ToString() });
            }

            cbbYear.DisplayMember = "Text";
            cbbYear.ValueMember = "Value";
            cbbYear.DataSource = datasource;
            cbbYear.SelectedValue = DateTime.Today.Year.ToString();
        }

        private void btnBrowseFileA_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialogA.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                try
                {
                    tbFileA.Text = openFileDialogA.FileName;
                }
                catch (IOException)
                {
                }
            }
        }

        private void btnBrowseFileB_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialogB.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                try
                {
                    tbFileB.Text = string.Join(Environment.NewLine, openFileDialogB.FileNames.ToList());
                }
                catch (IOException)
                {
                }
            }
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(openFileDialogA.FileName))
            {
                MessageBox.Show(this, @"Please Select File A !!!", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (openFileDialogB.FileNames == null || !openFileDialogB.FileNames.Any())
            {
                MessageBox.Show(this, @"Please Select File B !!!", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            progressBar1.Visible = true;
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var excelReport = new ExcelReport(openFileDialogA.FileName, openFileDialogB.FileNames.ToList());
            excelReport.Proceed();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Visible = false;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                MessageBox.Show(@"Cancel");
            }
            else
            {
                MessageBox.Show(this, @"Proceed Success.", @"Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}