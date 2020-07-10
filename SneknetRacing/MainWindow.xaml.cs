using System;
using System.Collections.Generic;
using System.Linq;
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
using SneknetRacing.Network;

namespace SneknetRacing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Server server = new Server();
        Thread serverThread;
        Thread dataHandleThread;

        PacketMotionData packet = new PacketMotionData();

        public MainWindow()
        {
            serverThread = new Thread(() => server.Listen());
            dataHandleThread = new Thread(() => this.SubscribeToEvent(server));


            DataContext = packet;

            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            packet.Info = "Starting threads";
            
            // Start Server thread
            serverThread.Start();
            // Start Handler thread
            dataHandleThread.Start();
        }

        public void SubscribeToEvent(Server server)
        {
            server.DataReceivedEvent += server_DataReceivedEvent;
        }

        private void server_DataReceivedEvent(object sender, ReceivedDataArgs args)
        {
            packet.Info = Encoding.ASCII.GetString(args.receivedBytes);
            packet.Desserialize(args.receivedBytes);
        }

    }
}
