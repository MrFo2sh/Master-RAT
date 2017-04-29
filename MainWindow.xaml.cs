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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static List<Socket> ClientSockets = new List<Socket>();
        private static Socket GlobalSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static int ClientsCounter = 0;
        private static Socket SendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            if (CommandBox.Text.Length > 0)
            {
                string CMD = CommandBox.Text;
                byte[] buffer = Encoding.ASCII.GetBytes(CMD);

                for (int i = 0; i < ClientSockets.Count; i++)
                {
                    try
                    {
                        SendSocket = ClientSockets.ElementAt(i);
                        SendSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), SendSocket);
                    }
                    catch
                    {
                        ClientSockets.Remove(SendSocket);
                        ClientsCounter--;
                    }
                }

                //foreach (Socket socket in ClientSockets)
                //{
                //    try
                //    {
                //        socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                //    }
                //    catch
                //    {
                //        ClientSockets.Remove(socket);
                //        ClientsCounter--;
                //    }
                //}
            }
            else
            {
                MessageBox.Show("Enter Command!");
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = false;
            setUpServer();
            MessageBox.Show("Server On!");
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            Socket LocalSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            try
            {
                LocalSocket = GlobalSocket.EndAccept(ar);
            }
            catch { }
            ClientSockets.Add(LocalSocket);
            ClientsCounter++;
            MessageBox.Show("Clients connected :" + ClientsCounter);
            try
            {
                GlobalSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch { }
        }

        private static void setUpServer()
        {
            try
            {
                GlobalSocket.Bind(new IPEndPoint(IPAddress.Any, 1234));
                GlobalSocket.Listen(15);
                GlobalSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch { }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket LocalSocket = (Socket)ar.AsyncState;
                LocalSocket.EndSend(ar);
            }
            catch { }
        }

        private void ShutdownBtn_Click(object sender, RoutedEventArgs e)
        {
            string CMD = "shutdown -s";
            byte[] buffer = Encoding.ASCII.GetBytes(CMD);
            foreach (Socket socket in ClientSockets)
            {
                try
                {
                    socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                }
                catch
                {
                    ClientSockets.Remove(socket);
                    ClientsCounter--;
                }
            }
        }

        private void RestartBtn_Click(object sender, RoutedEventArgs e)
        {
            string CMD = "shutdown -r";
            byte[] buffer = Encoding.ASCII.GetBytes(CMD);
            foreach (Socket socket in ClientSockets)
            {
                try
                {
                    socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                }
                catch
                {
                    ClientSockets.Remove(socket);
                    ClientsCounter--;
                }
            }
        }

        private void RunLolBtn_Click(object sender, RoutedEventArgs e)
        {
            string CMD = "cd C:/League of Legends && lol.launcher.admin.exe";
            byte[] buffer = Encoding.ASCII.GetBytes(CMD);
            foreach (Socket socket in ClientSockets)
            {
                try
                {
                    socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                }
                catch
                {
                    ClientSockets.Remove(socket);
                    ClientsCounter--;
                }
            }
        }

        private void RunSteamBtn_Click(object sender, RoutedEventArgs e)
        {
            string CMD = "cd C:/Steam && Steam.exe";
            byte[] buffer = Encoding.ASCII.GetBytes(CMD);
            foreach (Socket socket in ClientSockets)
            {
                try
                {
                    socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                }
                catch
                {
                    ClientSockets.Remove(socket);
                    ClientsCounter--;
                }
            }
        }
    }
}
