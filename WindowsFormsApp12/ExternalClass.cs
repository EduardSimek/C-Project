using AForge.Imaging.Filters;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public class ImageProcessor
    {
        public void ApplyAforgeNETGrayscale(Bitmap bmp, PictureBox pictureBox)
        {
            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            Bitmap grayImage = filter.Apply(bmp);
            pictureBox.Image = grayImage;
        }

        public void halo(Bitmap bmp, PictureBox pictureBox)
        {
            ApplyAforgeNETGrayscale(bmp, pictureBox);
        }




    }
}
