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

using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.IO;

namespace Wpf
{
    public static class BitmapExtension
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
    public static class RichTextBoxExtension
    {
        public static void InvokeAppendText(this RichTextBox self, string textData)
        {
            self.Dispatcher.Invoke(() => self.AppendText(textData));
        }
        public static void InvokeAppendLine(this RichTextBox self, string textData)
        {
            self.Dispatcher.Invoke(() => self.AppendText(textData));
            self.Dispatcher.Invoke(() => self.AppendText(Environment.NewLine));
        }
        public static void InvokeScrollToEnd(this RichTextBox self)
        {
            self.Dispatcher.Invoke(() => self.ScrollToEnd());
        }
    }
    public static class WindowExtension
    {
        public static void InvokeVisible(this Window self)
        {
            self.Dispatcher.Invoke(() => self.Visibility=Visibility.Visible);
        }
        public static void InvokeHidden(this Window self)
        {
            self.Dispatcher.Invoke(() => self.Visibility = Visibility.Hidden);
        }
    }
}
