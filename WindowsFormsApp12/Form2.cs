using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public partial class Form2 : Form
    {
        public Form2(string text, string text1)
        {
            InitializeComponent();
            label1.Text = text;
            label2.Text = text1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable a = new DataTable();

            a.Columns.Add("Type of image processing");
            a.Columns.Add("Seconds");
            a.Columns.Add("Miliseconds");

            a.Rows.Add("Grayscale");
            a.Rows.Add("HSV");
            a.Rows.Add("HLS");
            a.Rows.Add("LAB");
            a.Rows.Add("YCC");
            a.Rows.Add("XYZ");
            a.Rows.Add("LUV");

            dataGridView1.DataSource = a;

            MessageBox.Show("The time is " + label1.Text + " s.");
            MessageBox.Show("The time is " + label2.Text + " ms.");



        }
    }
}
