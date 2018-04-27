using HubLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WindowsApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        StreamSocketClass SocketManager = new StreamSocketClass();
        HostName ServerAdress = new HostName("healthHub");
        public MainPage()
        {
            this.InitializeComponent();

            //StreamSocketClass.IsServer = false;
            // Declaring HostName of Server
            
            // Open Listening ports and start listening.
            SocketManager.DataListener_OpenListenPorts();
            // Server
            //if (StreamSocketClass.IsServer)
            //{
            //    Debug.WriteLine("[SERVER] Ready to receive");
            //}
            // Client
            //else
            //{
            //SocketManager.SendResponse(ServerAdress, "Hello WindowsInstructed");
            //}
        }

        private void __btn_send_Click(object sender, RoutedEventArgs e)
        {
            SocketManager.SendResponse(new HostName(txt_hostname.Text), txt_msg.Text);
        }
    }
}
