using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

using SneknetRacing.Model;

namespace SneknetRacing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private const int _listenPort = 20777;
        
        public ManualResetEvent allDone = new ManualResetEvent(false);
        
        public MainWindow()
        {
            var viewModel = new PacketMotionData();
            viewModel.Header.PacketFormat = 1;

            DataContext = viewModel;

            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            StartListener();    
        }

        private void StartListener()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPEndPoint localEP = new IPEndPoint(IPAddress.Parse("127.0.0.0"), _listenPort);

            InfoLabel.Content = $"Local address and port : {localEP.ToString()}";

            Socket listener = new Socket(localEP.Address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            try
            {
                listener.Bind(localEP);
                listener.Listen(10);

                while(true)
                {
                    allDone.Reset();

                    InfoLabel.Content = "Waiting for a connection...";

                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    allDone.WaitOne();
                }
            }
            catch (SocketException e)
            {
                InfoLabel.Content = e;
            }
            finally
            {
                listener.Close();
            }

        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Get the socket that handles the client request
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Signal the main thread to continue
            allDone.Set();

            // Create the state object
        }
    }
}
