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

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum EnumCommand
    {
        ES_TEXT_NORMAL,
        ES_DRAW_CLEAR,
        ES_DRAW_ADD,
        ES_DRAW_DISPLAY,
    };
    public partial class MainWindow : Window
    {
        DrawImageWindow drawWindow = new DrawImageWindow();
        const int K = 1024;
        const int M = 1024 * 1024;
        public MainWindow()
        {
            InitializeComponent();
            drawWindow.Show();
            //drawWindow.Visibility = Visibility.Hidden;
        }
        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            richTextBox.Background = Brushes.Black;
            richTextBox.Foreground = Brushes.White;

            Thread threadListening = new Thread(new ThreadStart(Listening));
            threadListening.IsBackground = true;
            threadListening.Start();
        }
        void Listening()
        {
            int port = 11000;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            Socket serverSocket = new Socket(ipep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipep);
            serverSocket.Listen(10);

            richTextBox.Dispatcher.Invoke(() => richTextBox.Document.Blocks.Clear());
            richTextBox.InvokeAppendLine("Listening At port " + port + Environment.NewLine);

            Socket clientSocket;
            while (true)
            {
                try
                {
                    clientSocket = serverSocket.Accept();
                    Working(clientSocket);
                }
                catch (Exception e)
                {
                    richTextBox.InvokeAppendLine("Exception(Listening): " + e.Message);
                }
            }
        }

        void Working(Socket client)
        {
            IPEndPoint ipep = (IPEndPoint)client.RemoteEndPoint;
            string address = ipep.Address.ToString();
            string port = ipep.Port.ToString();
            richTextBox.InvokeAppendLine("Accept Tcp Client From " + address + ":" + port + " At " + DateTime.Now.ToString("HH:mm:ss"));
            richTextBox.InvokeScrollToEnd();

            byte[] data = new byte[60 * K];
            while (true)
            {
                try
                {
                    int readLength = client.Receive(data, data.Length, SocketFlags.None);
                    if (readLength > 0)
                    {
                        Analysis(data, readLength);
                        //break;
                    }
                    else if (0 == readLength)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    richTextBox.InvokeAppendLine("Exception " + e.Message);
                    break;
                }
            }
            richTextBox.InvokeAppendLine("Tcp Client Close At " + DateTime.Now.ToString("HH:mm:ss"));
            richTextBox.InvokeAppendLine("");
            richTextBox.InvokeScrollToEnd();
        }
        void Analysis(byte[] data, int readLength)
        {
            int pos = 0;
            while (pos < readLength)
            {
                int size = System.BitConverter.ToInt32(data, pos);
                EnumCommand cmd = (EnumCommand)System.BitConverter.ToInt32(data, pos + 4);
                switch (cmd)
                {
                    case EnumCommand.ES_TEXT_NORMAL:
                        int stringLength = size - 4;
                        string receiveString = System.Text.ASCIIEncoding.ASCII.GetString(data, pos + 8, stringLength);
                        richTextBox.InvokeAppendText(receiveString);
                        break;
                    case EnumCommand.ES_DRAW_CLEAR:
                        richTextBox.InvokeAppendLine("COMMAND: Draw Clear");
                        drawWindow.InvokeVisible();
                        drawWindow.InvokeClear();
                        break;
                    case EnumCommand.ES_DRAW_ADD:
                        unsafe
                        {
                            fixed (byte* pdata = data)
                            {
                                double* p = (double*)(pdata + pos + 8);
                                double x = *p;
                                double y = *(p + 1);
                                int color = *(int*)(p + 2);
                                string str = "(" + x + "," + y + ")";
                                //richTextBox.InvokeAppendText(str);
                                // str = xs.Count.ToString();
                                // textBlock1.Dispatcher.Invoke(() => textBlock1.Text = "point count = " + str);
                                drawWindow.InvokeAdd(x, y, color);
                            }
                        }
                        break;
                    case EnumCommand.ES_DRAW_DISPLAY:
                        richTextBox.InvokeAppendLine("COMMAND: Draw Display");
                        drawWindow.InvokeVisible();
                        drawWindow.InvokeDraw();
                        break;
                    default:
                        break;
                }
                pos += 4 + size;
            }
            richTextBox.InvokeScrollToEnd();
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            drawWindow.Close();
        }
    }
}
