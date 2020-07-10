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

using SneknetRacing.Network;

namespace SneknetRacing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Server server = new Server();
        HandleDataClass hdc = new HandleDataClass();
        Thread serverThread;
        Thread dataHandleThread;

        public MainWindow()
        {
            serverThread = new Thread(() => server.Listen());
            dataHandleThread = new Thread(() => hdc.SubscribeToEvent(server));
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("poggers");
            // Start Server thread
            serverThread.Start();
            // Start Handler thread
            dataHandleThread.Start();
        }
    }
}
