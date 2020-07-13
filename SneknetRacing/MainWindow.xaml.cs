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
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace SneknetRacing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Server server = new Server();
        readonly Thread serverThread;
        readonly Thread dataHandleThread;

        PacketHeader header = new PacketHeader();
        PacketMotionData packet = new PacketMotionData();

        ViGEmClient client = new ViGEmClient();

        IXbox360Controller controller;

        public MainWindow()
        {
            serverThread = new Thread(() => server.Listen());
            dataHandleThread = new Thread(() => this.SubscribeToEvent(server));

            controller = client.CreateXbox360Controller();

            DataContext = packet;

            InitializeComponent();
        }

        public void SubscribeToEvent(Server server)
        {
            server.DataReceivedEvent += server_DataReceivedEvent;
        }

        private void server_DataReceivedEvent(object sender, ReceivedDataArgs args)
        {
            packet.Info = "beep";
            header.Desserialize(args.receivedBytes);
            if(header.PacketID == 0)
                packet.Desserialize(args.receivedBytes);
            packet.Info = "boop";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            controller.Connect();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            controller.SetSliderValue(Xbox360Slider.RightTrigger, 250);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            controller.SetSliderValue(Xbox360Slider.RightTrigger, 0);
        }

        private void udpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            packet.Info = "Starting threads";
            // Start Server thread
            serverThread.Start();
            // Start Handler thread
            dataHandleThread.Start();
        }
    }
}
