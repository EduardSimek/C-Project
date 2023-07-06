using System;
using System.Drawing;
using System.Diagnostics;  // stopwatch
using System.Windows.Forms;
using ZedGraph;

using Emgu.CV;
using Emgu.CV.Structure;


using AForge.Imaging.Filters;
using AForge;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApp12
{
    public partial class Form1 : Form
    {
        // The heading * means that both libraries for image processing support the certain filter

        #region Grayscale transformation

        Image<Gray, byte> img;
        Bitmap grayImage;
        Grayscale filter;

        #endregion

        #region Color space transformations

        Image<Hsv, byte> img8;
        Image<Luv, byte> img10;
        Image<Xyz, byte> img12;
        Image<Lab, byte> img14;
        Image<Hls, byte> img1;
        Image<Ycc, byte> img3;
        Image<Bgr, byte> img9, img11, img13, img15, img2, img4;

        #endregion

        #region Edge detection

        Image<Gray, Byte> gray;
        Image<Gray, Byte> canny;

        #endregion

        #region Image smoothing

        Image<Bgr, Byte> image;
        Image<Bgr, Byte> gauss;

        #endregion

        #region Erosion - EmguCV * 
        Image<Bgr, byte> img6;

        #endregion
        #region Erosion - AFORGE.NET * 
        Erosion filter6;
        Bitmap Erosion;

        #endregion 

        #region Dilatation - EmguCV * 

        Image<Bgr, byte> img5;

        #endregion
        #region Dilatation - AFORGE.NET * 

        Dilatation filter5;
        Bitmap Dilatation;

        #endregion 

        #region Binary thresholding

        Image<Gray, byte> img7;
        Image<Gray, byte> imgBinarize;
        Threshold filter7;
        Bitmap c1, c2;

        #endregion

        #region Color filtering

        HSLFiltering filter1;
        YCbCrFiltering filter2;
        Bitmap rr1, YCbCr;

        #endregion

        #region Image processing

        CannyEdgeDetector filter3;
        AdaptiveSmoothing filter4;
        Bitmap canny3, canny4, r;

        #endregion

        int number = 10;
        int second_number = 231;
        int tr_max = 400;
        public static string EmguCVText = "Image processing time for EmguCV is";
        public static string AforgeNETText = "Image processing time for AForge.NET is ";
        public Form1()
        {
            InitializeComponent();
        }

        private void CalculateAndDisplayStatistics(double[] values, string labelPrefix)
        {
            double max = values.Max();
            double min = values.Min();
            double sum = values.Sum();
            double average = values.Average();

            DisplayMessageBox(max, min, sum, average);
            UpdateLabels(max, min, sum, average, labelPrefix);
        }

        private void DisplayMessageBox(double max, double min, double sum, double average)
        {
            Action<string> showMsg = (msg) => MessageBox.Show(msg);

            string message = "The highest value of the array is " + max + Environment.NewLine +
                             "the lowest value of the array is " + min + Environment.NewLine +
                             "the sum of the numeric array is " + sum + Environment.NewLine +
                             "the average value of the numeric array is " + average + ".";

            showMsg(message);
        }

        private void UpdateLabels(double max, double min, double sum, double average, string labelPrefix)
        {
            Func<string, double, string> formatLabel = (label, value) => $"{labelPrefix} {label} {value}";        

            label16.Text = formatLabel("The highest value of the array is", max);
            label17.Text = formatLabel("The lowest value of the array is", min);
            label18.Text = formatLabel("The sum of the numeric array is", sum);
            label19.Text = formatLabel("The average value of the numeric array is", average);
        }

        double[] y = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] y2 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private void hodnoty_y()
        {
            CalculateAndDisplayStatistics(y, "EmguCV");
        }

        private void hodnoty_y2()
        {
            CalculateAndDisplayStatistics(y2, "AForge.NET");
        }

        private void AddInfo()
        {
            MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");
        }

        private void remove()
        {
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private void falsed()
        {
            trackBar1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
        }

        private void time_format_emgucv()
        {
            label3.Text = EmguCVText + String.Format("{0,00}", abcd.Elapsed) + " s.";
            label4.Text = EmguCVText + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.";
        }

        private void time_format_aforgenet()
        {
            label9.Text = AforgeNETText + String.Format("{0,00}", AForge.Elapsed) + " s.";
            label10.Text = AforgeNETText + String.Format("{0,00}", AForge.ElapsedMilliseconds) + " ms.";
        }

        private void ConfigureGraphPane()
        {
            string[] schools = GetValues();
            var pane = zedGraphControl1.GraphPane;
            pane.Title.Text = "EmguCV vs AForge.NET";
            pane.XAxis.Title.Text = "Number of measurements (-)";
            pane.YAxis.Title.Text = "Processing time (ms)";
            pane.XAxis.Scale.IsVisible = true;
            pane.YAxis.Scale.IsVisible = true;
            pane.XAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MajorGrid.IsVisible = true;
            pane.XAxis.Scale.TextLabels = schools;
            pane.XAxis.Type = AxisType.Text;
        }

        private string[] GetValues()
        {
            List<string> schools = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            return schools.ToArray();
        }
        private void AddBarGraph(double[] values, string label, Color color)
        {
            var pane = zedGraphControl1.GraphPane;
            BarItem pointsCurve = pane.AddBar(label, null, values, color);
            pointsCurve.IsVisible = true;
            pane.AxisChange();
            zedGraphControl1.Refresh();
            this.Controls.Add(zedGraphControl1);
        }

        private void DrawGraph(double[] y)
        {
            ConfigureGraphPane();
            AddBarGraph(y, "EmguCV", Color.Red);
        }

        private void DrawGraph2(double[] y2)
        {
            ConfigureGraphPane();
            AddBarGraph(y2, "AForge.NET", Color.Blue);
        }

        //loading the image (bmp)
        Bitmap bmp = new Bitmap("c:\\Emgucv2\\configEmguCV\\abc.jpg");
        Stopwatch abcd = new Stopwatch();
        Stopwatch AForge = new Stopwatch();

        private void button7_Click(object sender, EventArgs e)
        {
            falsed();
            OpenFileDialog a = new OpenFileDialog();
            a.Filter = "*BMP Image|*.bmp|*JPG Image|*.jpg|*JPEG Image|*.jpeg";

            if (a.ShowDialog() == DialogResult.OK)
            {
                abcd = Stopwatch.StartNew();
                #region Loading the image from PC(bmp)

                pictureBox1.Image = bmp;
                bmp = new Bitmap(a.FileName);
                pictureBox1.Image = bmp;

                #endregion
                abcd.Stop();
                label7.Text = "Loading time is " + String.Format("{0,00}", abcd.Elapsed) + " s.";
                label8.Text = "Loading time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.";
                label5.Text = ("Image resolution is " + bmp.Width + " x " + bmp.Height);
                comboBox1.FormattingEnabled = true;
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(GetTransformationOptions());
            }
        }

        private string[] GetTransformationOptions()
        {
            return new List<string>
            {
                "0. Grayscale transformation* ",
                "1. HSV",
                "2. HLS * ",
                "3. LUV",
                "4. Ycc * ",
                "5. XYZ",
                "6. LAB",
                "7. Edge detection * ",
                "8. Image smoothing* ",
                "9. Thresholding * ",
                "10. Dilatation * ",
                "11. Erosion * "
            }.ToArray();
        }

        public static int i;

        #region Defining operations in the main methods
        private void ProcessEmguCVGrayscale()  // ComboBox Original image ---> Grayscale (EmguCV)
        {
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            remove();

            for (i = 1; i <= number; i++)
            {
                img = new Image<Gray, byte>(bmp);
                pictureBox2.Image = img.ToBitmap();

                abcd = Stopwatch.StartNew();

                #region EmguCV - Grayscale

                img = new Image<Gray, byte>(bmp);
                pictureBox2.Image = img.ToBitmap();

                #endregion

                abcd.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");
                //AddInfo();

                if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                time_format_emgucv();
            }

            DrawGraph(y);
            hodnoty_y();
        }

        public ImageProcessor image2;
        private void ProcessAforgeNETToGrayscale() // Combobox Original image ---> Grayscale (AForge.NET)
        {
            falsed();
            image2 = new ImageProcessor();

            for (int i = 1; i <= number; i++)
            {

                AForge = Stopwatch.StartNew();
                image2.ApplyAforgeNETGrayscale(bmp, pictureBox3);
                AForge.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", AForge.ElapsedMilliseconds) + " ms.");
                if (i > 0) y2[i - 1] = Convert.ToDouble(AForge.Elapsed.TotalMilliseconds);
                time_format_aforgenet();
            }
            DrawGraph2(y2);
            hodnoty_y2();
        }

        private void ProcessEmguCVToHSV()
        {
            pictureBox3.Image = null;
            label9.Text = " ";
            label10.Text = " ";
            falsed();
            remove();

            for (int i = 1; i <= number; i++)
            {
                abcd = Stopwatch.StartNew();

                #region EmguCV - HSV

                img8 = new Image<Hsv, byte>(bmp);
                img9 = new Image<Bgr, byte>(bmp);
                img9.Data = img8.Data;
                pictureBox2.Image = img9.Bitmap;

                #endregion

                abcd.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                time_format_emgucv();

            }
            DrawGraph(y);
            hodnoty_y();
        }

        private void ProcessEmguCVToHLS()
        {
            falsed();
            remove();

            for (int i = 1; i <= number; i++)
            {
                img1 = new Image<Hls, byte>(bmp);
                img2 = new Image<Bgr, byte>(bmp);

                abcd = Stopwatch.StartNew();

                #region EmguCV - HLS

                img1 = new Image<Hls, byte>(bmp);
                img2 = new Image<Bgr, byte>(bmp);
                img2.Data = img1.Data;
                pictureBox2.Image = img2.Bitmap;

                #endregion

                abcd.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                time_format_emgucv();

            }
            DrawGraph(y);
            hodnoty_y();
        }

        private void ProcessAforgeNETToHLS()
        {
            falsed();

            for (int i = 1; i <= number; i++)
            {
                AForge = Stopwatch.StartNew();

                #region AForge.NET - HLS

                filter1 = new HSLFiltering();
                filter1.Hue = new AForge.IntRange(340, 20);
                filter1.UpdateLuminance = false;
                filter1.UpdateSaturation = false;
                rr1 = filter1.Apply(bmp);
                pictureBox3.Image = rr1;

                #endregion

                AForge.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", AForge.ElapsedMilliseconds) + " ms.");

                if (i > 0) y2[i - 1] = Convert.ToDouble(AForge.Elapsed.TotalMilliseconds);

                time_format_aforgenet();

            }
            DrawGraph2(y2);
            hodnoty_y2();
        }

        private void ProcessEmguCVToLuv()
        {
            pictureBox3.Image = null;
            label9.Text = " ";
            label10.Text = " ";
            falsed();
            remove();

            for (int i = 1; i <= number; i++)
            {

                abcd = Stopwatch.StartNew();

                #region EmguCV - LUV

                img10 = new Image<Luv, byte>(bmp);
                img11 = new Image<Bgr, byte>(bmp);
                img11.Data = img10.Data;
                pictureBox2.Image = img11.Bitmap;

                #endregion

                abcd.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                time_format_emgucv();

            }
            DrawGraph(y);
            hodnoty_y();
        }

        private void ProcessEmguCVToYcc()
        {
            falsed();
            remove();

            for (int i = 1; i <= number; i++)
            {

                abcd = Stopwatch.StartNew();

                #region EmguCV - Ycc

                img3 = new Image<Ycc, byte>(bmp);
                img4 = new Image<Bgr, byte>(bmp);
                img4.Data = img3.Data;
                pictureBox2.Image = img4.Bitmap;

                #endregion

                abcd.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                time_format_emgucv();

            }
            DrawGraph(y);
            hodnoty_y();
        }

        // ox Original image ---> YCbCr (AForge.NET)
        private void ProcessAForgeNETToYCbCr()
        {
            falsed();

            for (int i = 1; i <= number; i++)
            {

                filter2 = new YCbCrFiltering();
                filter2.Y = new AForge.Range(200, 225);
                filter2.Cb = new AForge.Range(200, 225);
                filter2.UpdateCr = false;
                YCbCr = filter2.Apply(bmp);

                AForge = Stopwatch.StartNew();

                #region AFORGE.NET - YCbCr  

                filter2 = new YCbCrFiltering();
                filter2.Y = new AForge.Range(200, 225);
                filter2.Cb = new AForge.Range(200, 225);
                filter2.UpdateCr = false;
                YCbCr = filter2.Apply(bmp);
                pictureBox3.Image = YCbCr;

                #endregion

                AForge.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", AForge.ElapsedMilliseconds) + " ms.");
                if (i > 0) y2[i - 1] = Convert.ToDouble(AForge.Elapsed.TotalMilliseconds);

                time_format_aforgenet();
            }
            DrawGraph2(y2);
            hodnoty_y2();
        }

        private void ProcessEmguCVToXYZ()
        {
            pictureBox3.Image = null;
            label9.Text = " ";
            label10.Text = " ";
            falsed();
            remove();

            for (int i = 1; i <= number; i++)
            {

                img12 = new Image<Xyz, byte>(bmp);
                img13 = new Image<Bgr, byte>(bmp);

                abcd = Stopwatch.StartNew();

                #region EmguCV - XYZ

                img12 = new Image<Xyz, byte>(bmp);
                img13 = new Image<Bgr, byte>(bmp);
                img13.Data = img12.Data;
                pictureBox2.Image = img13.Bitmap;

                #endregion

                abcd.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");
                if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                time_format_emgucv();

            }
            DrawGraph(y);
            hodnoty_y();
        }

        // Combobox Original image ---> LAB (EmguCV)
        private void ProcessEmguCVToLab()
        {
            pictureBox3.Image = null;
            label9.Text = " ";
            label10.Text = " ";
            falsed();
            remove();

            for (int i = 1; i <= number; i++)
            {
                img14 = new Image<Lab, byte>(bmp);
                img15 = new Image<Bgr, byte>(bmp);

                abcd = Stopwatch.StartNew();

                #region EmguCV - LAB

                img14 = new Image<Lab, byte>(bmp);
                img15 = new Image<Bgr, byte>(bmp);
                img15.Data = img14.Data;
                pictureBox2.Image = img15.Bitmap;

                #endregion

                abcd.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                time_format_emgucv();

            }
            DrawGraph(y);
            hodnoty_y();

        }

        private void ProcessEmguCVToEdgeDetection()
        {
            trackBar1.Enabled = true;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            remove();

            label6.Text = "Edge detection (EmguCV)";

            for (int i = 1; i <= number; i++)
            {

                gray = new Image<Gray, Byte>(bmp);
                canny = gray.Canny(11, 30);

                abcd = Stopwatch.StartNew();

                #region EmguCV - Edge detection

                gray = new Image<Gray, Byte>(bmp);
                canny = gray.Canny(11, 30);
                pictureBox2.Image = canny.ToBitmap();

                #endregion

                abcd.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                time_format_emgucv();

            }
            DrawGraph(y);
            hodnoty_y();
        }

        private void ProcessAforgeNETToEdgeDetection()
        {
            trackBar1.Enabled = true;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;

            for (int i = 1; i <= number; i++)
            {

                var bmp16bpp = Grayscale.CommonAlgorithms.BT709.Apply(bmp);
                filter3 = new CannyEdgeDetector();
                filter3.HighThreshold = 11;
                Bitmap canny3 = filter3.Apply(bmp16bpp);

                AForge = Stopwatch.StartNew();

                #region AForge.NET - Edge detection

                var bmp8bpp = Grayscale.CommonAlgorithms.BT709.Apply(bmp);
                filter3 = new CannyEdgeDetector();
                filter3.HighThreshold = 11;
                Bitmap canny4 = filter3.Apply(bmp8bpp);
                pictureBox3.Image = canny4;

                #endregion

                AForge.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", AForge.ElapsedMilliseconds) + " ms.");
                if (i > 0) y2[i - 1] = Convert.ToDouble(AForge.Elapsed.TotalMilliseconds);

                time_format_aforgenet();
            }
            DrawGraph2(y2);
            hodnoty_y2();
        }

        private void ProcessEmguCVToImageSmoothing()
        {
            trackBar1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = true;
            remove();

            for (int i = 1; i <= 10; i++)
            {
                image = new Image<Bgr, byte>(bmp);
                gauss = image.SmoothGaussian(3, 3, 34.3, 45.3);

                abcd = Stopwatch.StartNew();

                #region EmguCV - Image smoothing

                image = new Image<Bgr, byte>(bmp);
                Image<Bgr, Byte> blur = image.SmoothBlur(10, 10, true);
                Image<Bgr, Byte> bilat = image.SmoothBilateral(7, 255, 34);
                gauss = image.SmoothGaussian(3, 3, 34.3, 45.3);
                Image<Bgr, Byte> mediansmooth = image.SmoothMedian(15);
                pictureBox2.Image = gauss.ToBitmap();

                #endregion

                abcd.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                time_format_emgucv();

            }
            DrawGraph(y);
            hodnoty_y();
        }

        private void ProcessAForgeNETToImageSmoothing()
        {
            img_smoothing = new Image<Bgr, byte>(bmp);
            for (int i = 1; i <= number; i++)
            {
                filter4 = new AdaptiveSmoothing();
                filter4.Factor = 11;
                r = filter4.Apply(bmp);

                AForge = Stopwatch.StartNew();

                #region AForge.NET - Image smoothing

                filter4 = new AdaptiveSmoothing();
                filter4.Factor = 11;
                r = filter4.Apply(bmp);
                pictureBox3.Image = r;

                #endregion

                AForge.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", AForge.ElapsedMilliseconds) + " ms.");

                if (i > 0) y2[i - 1] = Convert.ToDouble(AForge.Elapsed.TotalMilliseconds);

                time_format_aforgenet();
            }
            DrawGraph2(y2);
            hodnoty_y2();
            trackBar1.Enabled = true;
            groupBox2.Enabled = false;
        }
        private void ProcessEmguCVToDilatation()
        {
            groupBox3.Enabled = false;
            groupBox2.Enabled = false;
            trackBar1.Enabled = true;
            trackBar2.Enabled = false;
            trackBar3.Enabled = false;
            remove();

            for (int i = 1; i <= number; i++)
            {
                img5 = new Image<Bgr, byte>(bmp);
                pictureBox2.Image = img5.Dilate(1).Bitmap;

                abcd = Stopwatch.StartNew();

                #region EmguCV - Dilatation

                img5 = new Image<Bgr, byte>(bmp);
                pictureBox2.Image = img5.Dilate(1).Bitmap;

                #endregion

                abcd.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                time_format_emgucv();
            }
            DrawGraph(y);
            hodnoty_y();
            label6.Text = "Dilatácia";
        }
        private void ProcessAForgeNETToDilatation()
        {
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            trackBar1.Enabled = true;

            for (int i = 1; i <= number; i++)
            {
                filter5 = new Dilatation();
                Dilatation = filter5.Apply(bmp);

                AForge = Stopwatch.StartNew();

                #region AForge.NET - Dilatation

                filter5 = new Dilatation();
                Dilatation = filter5.Apply(bmp);
                pictureBox3.Image = Dilatation;

                #endregion

                AForge.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", AForge.ElapsedMilliseconds) + " ms.");
                if (i > 0) y2[i - 1] = Convert.ToDouble(AForge.Elapsed.TotalMilliseconds);

                time_format_aforgenet();
            }
            DrawGraph2(y2);
            hodnoty_y2();
        }

        private void ProcessEmguCVToErosion()
        {
            groupBox3.Enabled = false;
            groupBox2.Enabled = false;
            remove();

            for (int i = 1; i <= number; i++)
            {
                img6 = new Image<Bgr, byte>(bmp);
                pictureBox2.Image = img6.Erode(1).Bitmap;

                abcd = Stopwatch.StartNew();

                #region EmguCV - Erosion

                img6 = new Image<Bgr, byte>(bmp);
                pictureBox2.Image = img6.Erode(1).Bitmap;

                #endregion

                abcd.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                time_format_emgucv();

            }
            DrawGraph(y);
            hodnoty_y();
            label6.Text = "Erózia";
        }

        private void ProcessAForgeNETToErosion()
        {
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;

            for (int i = 1; i <= number; i++)
            {
                filter6 = new Erosion();
                Erosion = filter6.Apply(bmp);

                AForge = Stopwatch.StartNew();

                #region AForge.NET - Erosion

                filter6 = new Erosion();
                Erosion = filter6.Apply(bmp);
                pictureBox3.Image = Erosion;

                #endregion

                AForge.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", AForge.ElapsedMilliseconds) + " ms.");

                if (i > 0) y2[i - 1] = Convert.ToDouble(AForge.Elapsed.TotalMilliseconds);
                time_format_aforgenet();

            }
            DrawGraph2(y2);
            hodnoty_y2();
        }

        private void ProcessEmguCVToBinaryThresholding()
        {
            groupBox2.Enabled = true;
            groupBox3.Enabled = false;
            remove();

            for (int i = 1; i <= number; i++)
            {
                img7 = new Image<Gray, byte>(bmp);
                imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));

                abcd = Stopwatch.StartNew();

                #region EmguCV - Binary thresholding

                img7 = new Image<Gray, byte>(bmp);
                imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                CvInvoke.Threshold(img7, imgBinarize, 40, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
                pictureBox2.Image = imgBinarize.ToBitmap();

                #endregion

                abcd.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");
                if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                time_format_emgucv();

            }
            DrawGraph(y);
            hodnoty_y();
            label13.Text = "Binary thresholding (EmguCV)";
        }

        private void ProcessAForgeNETToBinaryThresholding()
        {
            groupBox3.Enabled = false;
            groupBox2.Enabled = true;
            trackBar1.Enabled = true;

            for (int i = 1; i <= number; i++)
            {
                var bmp8bpp = Grayscale.CommonAlgorithms.BT709.Apply(bmp);
                filter7 = new Threshold();
                filter7.ThresholdValue = 40;
                Bitmap c1 = filter7.Apply(bmp8bpp);

                AForge = Stopwatch.StartNew();

                #region AForge.NET - Binary thresholding

                var bmp16bpp = Grayscale.CommonAlgorithms.BT709.Apply(bmp);
                filter7 = new Threshold();
                filter7.ThresholdValue = 40;
                Bitmap c2 = filter7.Apply(bmp16bpp);
                pictureBox3.Image = c2;

                #endregion

                AForge.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", AForge.ElapsedMilliseconds) + " ms.");

                if (i > 0) y2[i - 1] = Convert.ToDouble(AForge.Elapsed.TotalMilliseconds);

                time_format_aforgenet();
            }
            DrawGraph2(y2);
            hodnoty_y2();
            label6.Text = "Binary thresholding (AForge.NET)";
        }
        #endregion

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboBox1.SelectedIndex;

            switch (selectedIndex)
            {
                case 0:
                    ProcessEmguCVGrayscale();
                    ProcessAforgeNETToGrayscale();
                    break;

                case 1:
                    ProcessEmguCVToHSV();
                    break;

                case 2:
                    ProcessEmguCVToHLS();
                    ProcessAforgeNETToHLS();
                    break;

                case 3:
                    ProcessEmguCVToLuv();
                    break;

                case 4:
                    ProcessEmguCVToYcc();
                    ProcessAForgeNETToYCbCr();
                    break;

                case 5:
                    ProcessEmguCVToXYZ();
                    break;

                case 6:
                    ProcessEmguCVToLab();
                    break;

                case 7:
                    ProcessEmguCVToEdgeDetection();
                    ProcessAforgeNETToEdgeDetection();
                    break;

                case 8:
                    ProcessEmguCVToImageSmoothing();
                    ProcessAForgeNETToImageSmoothing();
                    break;

                case 10:
                    ProcessEmguCVToDilatation();
                    ProcessAForgeNETToDilatation();
                    break;

                case 11:
                    ProcessEmguCVToErosion();
                    ProcessAForgeNETToErosion();
                    break;

                case 9:
                    ProcessEmguCVToBinaryThresholding();
                    ProcessAForgeNETToBinaryThresholding();
                    break;
            }

        }

        #region Another Color transformations
        private void _ProcessEmguCVToBinaryThresholding()
        {
            remove();
            for (int i = 1; i <= number; i++)
            {
                abcd = Stopwatch.StartNew();
                #region EmguCV - Binary thresholding

                Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
                pictureBox2.Image = imgBinarize.ToBitmap();

                #endregion
                abcd.Stop();
                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");
                if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);
                time_format_emgucv();
            }
            DrawGraph(y);
            hodnoty_y();
        }

        private void _ProcessEmguCVToBinaryInversionThresholding()
        {
            remove();

            for (int i = 1; i <= number; i++)
            {
                abcd = Stopwatch.StartNew();

                #region EmguCV - Binary inversion threshold

                Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.BinaryInv);
                pictureBox2.Image = imgBinarize.ToBitmap();

                #endregion

                abcd.Stop();

                MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                time_format_emgucv();
            }
            DrawGraph(y);
            hodnoty_y();
        }

        #endregion

        private void button14_Click(object sender, EventArgs e)
        {
            label3.Text = " ";
            label4.Text = " ";
            label7.Text = " ";
            label8.Text = " ";
            label9.Text = " ";
            label10.Text = " ";
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            remove();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (aaa.Checked == true)   // Binary hresholding (EmguCV)
            {
                _ProcessEmguCVToBinaryThresholding();
            }

            if (radioButton2.Checked == true) // Binary inversion thresholding (EmguCV)
            {
                _ProcessEmguCVToBinaryInversionThresholding();
            }

            if (radioButton3.Checked == true) // Thresholding mask (EmguCV)

            {
                remove();

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Thresholding mask

                    Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                    imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                    CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.Mask);
                    pictureBox2.Image = imgBinarize.ToBitmap();
                    #endregion

                    abcd.Stop();

                    MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }

            if (radioButton4.Checked == true) // Threshold value (EmguCV)

            {
                remove();

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Threshold value

                    Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                    imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                    CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.Otsu);
                    pictureBox2.Image = imgBinarize.ToBitmap();

                    #endregion

                    abcd.Stop();

                    MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }

            if (radioButton5.Checked == true) // Zero thresholding (EmguCV)

                remove();
            {
                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Zero thresholding

                    Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                    imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                    CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.ToZero);
                    pictureBox2.Image = imgBinarize.ToBitmap();

                    #endregion

                    abcd.Stop();

                    MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }

            if (radioButton6.Checked == true) // Zero inversion thresholding (EmguCV)

            {
                remove();

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Zero inversion thresholding

                    Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                    imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                    CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.ToZeroInv);
                    pictureBox2.Image = imgBinarize.ToBitmap();

                    #endregion

                    abcd.Stop();

                    MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }

            if (radioButton7.Checked == true) // Threshold triangle (EmguCV)
            {
                remove();

                for (int i = 1; i <= number; i++)
                {

                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Threshold triangle

                    Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                    imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                    CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.Triangle);
                    pictureBox2.Image = imgBinarize.ToBitmap();

                    #endregion

                    abcd.Stop();

                    MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }

            if (radioButton8.Checked == true) // Abbreviated thresholding (EmguCV)

            {
                remove();
                for (int i = 1; i <= number; i++)
                {

                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Abbreviated thresholding

                    Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                    imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                    CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.Trunc);
                    pictureBox2.Image = imgBinarize.ToBitmap();

                    #endregion

                    abcd.Stop();

                    MessageBox.Show("Number of measurement is " + i + "." + " and image processing time is " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);
                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }
        }

        Image<Bgr, byte> img_smoothing;
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 7) // Edge detection (EmguCV)
            {
                remove();
                groupBox2.Enabled = false;
                label6.Text = "Edge detection (Emgu.CV) - Canny1";
                label13.Text = "Edge detection (Emgu.CV) - Canny2";
                trackBar1.Maximum = tr_max;
                trackBar2.Maximum = tr_max;

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Edge detection

                    Image<Gray, Byte> gray = new Image<Gray, Byte>(bmp);
                    Image<Gray, Byte> canny = gray.Canny(trackBar1.Value, trackBar2.Value);
                    pictureBox2.Image = canny.ToBitmap();

                    #endregion

                    abcd.Stop();

                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);
                    time_format_emgucv();
                }
                DrawGraph(y);
            }

            if (comboBox1.SelectedIndex == 8) // Image smoothing (AForge.NET)
            {
                trackBar1.Enabled = true;
                label6.Text = "Image smoothing (AForge.NET)";
                remove();

                for (int i = 1; i <= number; i++)
                {
                    AForge = Stopwatch.StartNew();

                    #region AForge.NET - Image smoothing

                    AdaptiveSmoothing filter = new AdaptiveSmoothing();
                    filter.Factor = trackBar1.Value;
                    Bitmap r = filter.Apply(bmp);
                    pictureBox3.Image = r;

                    #endregion

                    AForge.Stop();



                    if (i > 0) y2[i - 1] = Convert.ToDouble(AForge.Elapsed.TotalMilliseconds);

                    time_format_aforgenet();
                }
                DrawGraph2(y2);
                //hodnoty_y2();
            }

            if (comboBox1.SelectedIndex == 11) // Erosion (EmguCV)
            {
                groupBox2.Enabled = false;
                trackBar2.Enabled = false;
                trackBar3.Enabled = false;
                label6.Text = "Erosion (EmguCV)";
                remove();

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Erosion

                    Image<Bgr, byte> img = new Image<Bgr, byte>(bmp);
                    pictureBox2.Image = img.Erode(trackBar1.Value).Bitmap;

                    #endregion

                    abcd.Stop();



                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();

                }
                DrawGraph(y);
                hodnoty_y();
            }

            if (comboBox1.SelectedIndex == 10) // Dilatation (EmguCV)
            {
                groupBox2.Enabled = false;
                trackBar2.Enabled = false;
                trackBar3.Enabled = false;
                label6.Text = "Dilatation (EmguCV)";
                remove();

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Dilatation

                    Image<Bgr, byte> img = new Image<Bgr, byte>(bmp);
                    pictureBox2.Image = img.Dilate(trackBar1.Value).Bitmap;

                    #endregion

                    abcd.Stop();

                    //MessageBox.Show("Číslo merania je " + i + "." + " a Čas spracovania je " + String.Format("{0,00}", abcd.ElapsedMilliseconds) + " ms.");

                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);
                    time_format_emgucv();

                }
                DrawGraph(y);
                hodnoty_y();
            }

            if (comboBox1.SelectedIndex == 9) // Binary thresholding (AForge.NET)
            {
                remove();
                trackBar1.Maximum = second_number;
                label6.Text = "Binary thresholding (AForge.NET)";

                for (int i = 1; i <= number; i++)
                {
                    AForge = Stopwatch.StartNew();

                    #region AFORGE.NET - Binary thresholding

                    var bmp16bpp = Grayscale.CommonAlgorithms.BT709.Apply(bmp);
                    Threshold filter = new Threshold();
                    filter.ThresholdValue = trackBar1.Value;   // trackBar2.Value;
                    Bitmap c = filter.Apply(bmp16bpp);
                    pictureBox3.Image = c;

                    #endregion

                    AForge.Stop();

                    //MessageBox.Show("Číslo merania je " + i + "." + " a Čas spracovania je " + String.Format("{0,00}", AForge.ElapsedMilliseconds) + " ms.");

                    if (i > 0) y2[i - 1] = Convert.ToDouble(AForge.Elapsed.TotalMilliseconds);

                    time_format_aforgenet();
                }
                DrawGraph2(y2);
                hodnoty_y2();
            }
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label11.Text = trackBar1.Value.ToString();
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label12.Text = trackBar2.Value.ToString();
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            label15.Text = trackBar3.Value.ToString();
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 7)
            {
                label14.Text = "AForge.NET - Gaussian";
                trackBar1.Enabled = true;
                groupBox2.Enabled = false;
                groupBox3.Enabled = false;

                for (int i = 1; i <= number; i++)
                {

                    AForge = Stopwatch.StartNew();

                    #region AFORGE.NET - Gaussian

                    var bmp8bpp = Grayscale.CommonAlgorithms.BT709.Apply(bmp);
                    CannyEdgeDetector filter = new CannyEdgeDetector();
                    filter.GaussianSize = trackBar3.Value;
                    Bitmap canny = filter.Apply(bmp8bpp);
                    pictureBox3.Image = canny;

                    #endregion AFORGE.NET

                    AForge.Stop();
                    // MessageBox.Show("Číslo merania je " + i + "." + " a Čas spracovania je " + String.Format("{0,00}", AForge.ElapsedMilliseconds) + " ms.");
                    if (i > 0) y2[i - 1] = Convert.ToDouble(AForge.Elapsed.TotalMilliseconds);

                    time_format_aforgenet();
                }
                DrawGraph2(y2);
                hodnoty_y2();
            }
        }
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 7) // Edge detection (EmguCV)
            {
                remove();
                groupBox2.Enabled = false;
                label6.Text = "Edge detection (EmguCV) - Canny1";
                label13.Text = "Edge detection (EmguCV) - Canny2";
                trackBar1.Maximum = tr_max;
                trackBar2.Maximum = tr_max;

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Edge detection

                    Image<Gray, Byte> gray = new Image<Gray, Byte>(bmp);
                    Image<Gray, Byte> canny = gray.Canny(trackBar1.Value, trackBar2.Value);
                    pictureBox2.Image = canny.ToBitmap();

                    #endregion

                    abcd.Stop();

                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();
                }
                DrawGraph(y);
            }

            if (comboBox1.SelectedIndex == 9) // Binary thresholding (EmguCV)

                remove();
            {
                label13.Text = "Binary thresholding (EmguCV)";
                trackBar2.Enabled = true;

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Binary thresholding

                    Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                    imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                    CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.Trunc);

                    #endregion

                    abcd.Stop();

                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }
        }


        private void button13_Click_3(object sender, EventArgs e)
        {
            if (aaa.Checked == true) // Binary thresholding (EmguCV)
            {
                remove();
                label13.Text = "Binary thresholding (EmguCV)";
                trackBar2.Maximum = second_number;

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Binary thresholding

                    Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                    imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                    CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
                    pictureBox2.Image = imgBinarize.ToBitmap();

                    #endregion

                    abcd.Stop();

                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }

            if (radioButton2.Checked == true) // Binary inversion thresholding (EmguCV)
            {
                remove();
                label13.Text = "Binary inversion thresholding (EmguCV)";
                trackBar2.Maximum = second_number;

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Binary inversion thresholding 

                    Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                    imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                    CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.BinaryInv);
                    pictureBox2.Image = imgBinarize.ToBitmap();

                    #endregion 

                    abcd.Stop();

                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }

            if (radioButton3.Checked == true) // Threshold mask (EmguCV)
            {
                remove();
                label13.Text = "Threshold mask (EmguCV)";
                trackBar2.Maximum = second_number;

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Threshold mask

                    Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                    imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                    CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.Mask);
                    pictureBox2.Image = imgBinarize.ToBitmap();

                    #endregion

                    abcd.Stop();

                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }
            if (radioButton4.Checked == true) // Threshold value (EmguCV)
            {
                remove();
                label13.Text = "Threshold value (EmguCV)";
                trackBar2.Maximum = second_number;

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Threshold value

                    Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                    imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                    CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.Otsu);
                    pictureBox2.Image = imgBinarize.ToBitmap();

                    #endregion

                    abcd.Stop();
                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }

            if (radioButton5.Checked == true) // Zero thresholding (EmguCV)

            {
                remove();
                label13.Text = "Zero thresholding (EmguCV)";
                trackBar2.Maximum = second_number;

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Zero thresholding

                    Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                    imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                    CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.ToZero);
                    pictureBox2.Image = imgBinarize.ToBitmap();

                    #endregion 

                    abcd.Stop();
                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }
            if (radioButton6.Checked == true) // Zero inversion thresholding (EmguCV)
            {
                remove();
                label13.Text = "Zero inversion thresholding (EmguCV)";
                trackBar2.Maximum = second_number;

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Zero inversion thresholding 

                    Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                    imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                    CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.ToZeroInv);
                    pictureBox2.Image = imgBinarize.ToBitmap();

                    #endregion

                    abcd.Stop();
                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);
                    time_format_emgucv();

                }
                DrawGraph(y);
                hodnoty_y();
            }
            if (radioButton7.Checked == true) // Threshold triangle (EmguCV)
            {
                remove();
                label13.Text = "Threshold triangle  (EmguCV)";
                trackBar2.Maximum = second_number;

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Threshold triangle 

                    Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                    imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                    CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.Triangle);
                    pictureBox2.Image = imgBinarize.ToBitmap();

                    #endregion

                    abcd.Stop();
                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }
            if (radioButton8.Checked == true) // Abbreviated thresholding (EmguCV)
            {
                remove();
                label13.Text = "Abbreviated thresholding (EmguCV)";
                trackBar2.Maximum = second_number;

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Abbreviated thresholding 

                    Image<Gray, byte> img = new Image<Gray, byte>(bmp);
                    imgBinarize = new Image<Gray, byte>(bmp.Width, bmp.Height, new Gray(0));
                    CvInvoke.Threshold(img, imgBinarize, trackBar2.Value, 255, Emgu.CV.CvEnum.ThresholdType.Trunc);
                    pictureBox2.Image = imgBinarize.ToBitmap();

                    #endregion

                    abcd.Stop();
                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            trackBar2.Enabled = true;
            trackBar3.Enabled = true;

            if (radioButton1.Checked == true) // Bilateral image smoothing (EmguCV)
            {
                label13.Text = "Bilateral image smoothing  - core size (EmguCV)";
                label14.Text = "Bilateral image smoothing - color sigma (EmguCV)";
                remove();

                for (int i = 1; i <= number; i++)

                {

                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Bilateral image smoothing 

                    Image<Bgr, Byte> image = new Image<Bgr, byte>(bmp);
                    Image<Bgr, Byte> bilat = image.SmoothBilateral(trackBar2.Value, trackBar3.Value, 34);
                    pictureBox2.Image = bilat.ToBitmap();

                    #endregion

                    abcd.Stop();

                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();

                }
                DrawGraph(y);
                hodnoty_y();
            }

            if (radioButton9.Checked == true)   // Image smoothing - blur  (EmguCV)
            {
                trackBar2.Minimum = 1;
                trackBar3.Minimum = 1;
                remove();
                label13.Text = "Image smoothing - blur - width (EmguCV)";
                label14.Text = "Image smoothing - blur - height (EmguCV)";

                for (int i = 1; i <= number; i++)

                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Image smoothing - blur

                    Image<Bgr, Byte> image = new Image<Bgr, byte>(bmp);
                    Image<Bgr, Byte> blur = image.SmoothBlur(trackBar3.Value, trackBar2.Value, true);
                    pictureBox2.Image = blur.ToBitmap();

                    #endregion

                    abcd.Stop();

                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }

            if (radioButton10.Checked == true) // Image smoothing - center blur (EmguCV)
            {
                remove();
                label13.Text = "Image smoothing - center blur (EmguCV)";

                for (int i = 1; i <= number; i++)
                {

                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Image smoothing - center blur

                    Image<Bgr, Byte> image = new Image<Bgr, byte>(bmp);
                    Image<Bgr, Byte> mediansmooth = image.SmoothMedian(15);
                    pictureBox2.Image = mediansmooth.ToBitmap();

                    #endregion

                    abcd.Stop();

                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }

            if (radioButton11.Checked == true) // Gaussian image smoothing (EmguCV)
            {
                remove();
                label13.Text = "Gaussian image smoothing (EmguCV)";

                for (int i = 1; i <= number; i++)
                {
                    abcd = Stopwatch.StartNew();

                    #region EmguCV - Gaussian image smoothing 

                    Image<Bgr, Byte> image = new Image<Bgr, byte>(bmp);
                    Image<Bgr, Byte> gauss = image.SmoothGaussian(1, 3, 34.3, 45.3);
                    pictureBox2.Image = gauss.ToBitmap();

                    #endregion

                    abcd.Stop();



                    if (i > 0) y[i - 1] = Convert.ToDouble(abcd.Elapsed.TotalMilliseconds);

                    time_format_emgucv();
                }
                DrawGraph(y);
                hodnoty_y();
            }
        }




    }

}









