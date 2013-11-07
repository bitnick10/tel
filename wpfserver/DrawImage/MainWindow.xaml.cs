using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;


//using System.Drawing;

namespace DrawImage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Drawing.Bitmap display;
        public MainWindow()
        {
            InitializeComponent();
        }
        public void Init(int width, int height)
        {
            display = new System.Drawing.Bitmap(width, height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(display);
            //System.Drawing.Brushes.Black;
            g.FillRectangle(System.Drawing.Brushes.Black, 0, 0, 100, 100);
            Update();
        }
        public void Update()
        {
            image1.Width = display.Width;
            image1.Height = display.Height;
            image1.Source = display.ToBitmapImage();
        }
    }
    public static class noname
    {
        public static BitmapImage ToBitmapImage(this System.Drawing.Bitmap self)
        {
            BitmapImage ret = new BitmapImage();
            MemoryStream ms = new MemoryStream();
            self.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ret.BeginInit();
            ret.StreamSource = ms;
            ret.EndInit();
            return ret;
        }
    }
}
