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
using System.Windows.Shapes;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for DrawImageWindow.xaml
    /// </summary>
    public partial class DrawImageWindow : Window
    {
        System.Drawing.Bitmap display;
        List<double> xs = new List<double>();
        List<double> ys = new List<double>();
        List<System.Drawing.Color> colors = new List<System.Drawing.Color>();
        public DrawImageWindow()
        {
            InitializeComponent();
            Init();
            Test();
        }
        private void Test()
        {
            Random rd = new Random();
            for (int i = 0; i < 100; i++)
            {
                Add(i, rd.Next(int.MinValue, int.MaxValue), rd.Next(int.MinValue, int.MaxValue));
            }
            Draw();
        }
        private void InvokeInit()
        {
            this.Dispatcher.Invoke(() => Init());
        }
        public void InvokeAdd(double x, double y, System.Drawing.Color color)
        {
            this.Dispatcher.Invoke(() => Add(x, y, color));
        }
        public void InvokeAdd(double x, double y, int color)
        {
            this.Dispatcher.Invoke(() => Add(x, y, color));
        }
        public void InvokeDraw()
        {
            this.Dispatcher.Invoke(() => Draw());
        }
        public void InvokeClear()
        {
            this.Dispatcher.Invoke(() => Clear());
        }
        private void Init()
        {
            display = new System.Drawing.Bitmap(800, 600);
            Clear();
            Update();
        }
        private void Add(double x, double y, int color)
        {
            Add(x, y, System.Drawing.Color.FromArgb(color));
        }
        private void Add(double x, double y, System.Drawing.Color color)
        {
            xs.Add(x);
            ys.Add(y);
            colors.Add(color);
        }
        private void Draw()
        {
            double minX = xs.Min();
            double maxX = xs.Max();

            double minY = ys.Min();
            double maxY = ys.Max();

            double marginX = (maxX - minX) * 0.025;
            double marginY = (maxY - minY) * 0.025;
            if (marginX == 0) marginX = 1;
            if (marginY == 0) marginY = 1;

            maxX = maxX + marginX;
            maxY = maxY + marginY;

            minX = minX - marginX;
            minY = minY - marginY;

            for (int i = 0; i < xs.Count; i++)
            {
                double drawX = (xs[i] - minX) / (maxX - minX) * display.Width;
                double drawY = (ys[i] - minY) / (maxY - minY) * display.Height;
                int x = (int)drawX;
                int y = (int)drawY;
                display.SetPixel(x, y, colors[i]);
                if (i < xs.Count - 1)
                {
                    double drawX2 = (xs[i+1] - minX) / (maxX - minX) * display.Width;
                    double drawY2 = (ys[i+1] - minY) / (maxY - minY) * display.Height;
                    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(display);
                    System.Drawing.Pen pen = new System.Drawing.Pen(colors[i],1f);
                    System.Drawing.PointF pt1 = new System.Drawing.PointF((float)drawX, (float)drawY);
                    System.Drawing.PointF pt2 = new System.Drawing.PointF((float)drawX2, (float)drawY2);
                    g.DrawLine(pen, pt1, pt2);
                    
                }
                //display.SetPixel(0,0, System.Drawing.Color.White);
            }

            textBlock1.Text = "Draw X = [" + minX + "," + maxX + "]";
            textBlock1.Text += ", ";
            textBlock1.Text += "Draw Y = [" + minY + "," + maxY + "]";
            textBlock1.Text += ", ";
            textBlock1.Text += "X = [" + xs.Min() + "," + xs.Max() + "]";
            textBlock1.Text += ", ";
            textBlock1.Text += "Y = [" + ys.Min() + "," + ys.Max() + "]";
            textBlock1.Text += ", ";
            textBlock1.Text += "TOTAL = " + xs.Count;
            Update();
        }
        private void Clear()
        {
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(display);
            g.FillRectangle(System.Drawing.Brushes.Black, 0, 0, display.Width, display.Height);
            xs.Clear();
            ys.Clear();
            colors.Clear();
            Update();
        }
        private void Update()
        {
            image1.Source = display.ToBitmapImage();
        }
    }
}
