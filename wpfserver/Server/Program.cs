using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Listening();
        }
        static void Listening()
        { 
            int port=11000;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            Socket serverSocket = new Socket(ipep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipep);
            serverSocket.Listen(10);
            Console.WriteLine("Listening At port " + port);

            Socket clientSocket;
            while (true)
            {
                clientSocket = serverSocket.Accept();
                Working(clientSocket);
            }
        }
        
        static void Working(Socket client)
        {
            IPEndPoint ipep = (IPEndPoint)client.RemoteEndPoint;
            string address = ipep.Address.ToString();
            string port=ipep.Port.ToString();
            Console.WriteLine("Accept Tcp Client From " + address + ":" + port+" At "+DateTime.Now.ToString("HH:mm:ss"));
           // DrawImage.MainWindow drawWindow = new DrawImage.MainWindow();
            //drawWindow.ShowDialog();
            byte[] data = new byte[60000];
            while (true)
            {
                try
                {
                    int readLength= client.Receive(data, data.Length, SocketFlags.None);
                    
                    if (readLength > 0)
                    {
                        //Console.WriteLine("read length = " + readLength);
                        for (int i = 0; i < readLength; i++)
                        {
                            Console.Write((char)data[i]);
                        }
                       // Console.WriteLine();
                    }
                    if (0 == readLength)
                    {
                        //Console.WriteLine("read length = " + 0);
                        break;
                    }
                }
                catch { break; }
            }
            Console.WriteLine("Tcp Client Close At "+DateTime.Now.ToString("HH:mm:ss"));
        }
    }
}
