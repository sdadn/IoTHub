using HubLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using HubLibrary;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace pulse_App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        const string deviceID = "pulse12345";
        static string deviceName;
        //StreamSocketClass socketManager;

        public MainPage()
        {
            this.InitializeComponent();

            deviceName = "Sensor";
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // wifi.test_access();
            // wifi.Get_adapters();
            //wifi.networks_scan("SSM");

            StreamSocketClass.OpenListenPorts("device");
        }
        public async void __ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Debug.WriteLine("[ " + deviceName + " ]: Receive event fired.");

            //DataReader DataListener_Reader;
            string DataReceived = await StreamSocketClass.ExtractReceivedData(args.Socket.InputStream);

            if (DataReceived == null)
            {
                Debug.WriteLine("Received data was empty. Check if you sent data.");
                return;
            }

            Debug.WriteLine("[ " + deviceName + " ] received " + DataReceived + " from " + args.Socket.Information.RemoteHostName);

            lbl_received_data.Text = DataReceived;
            lbl_sender.Text = args.Socket.Information.RemoteHostName.ToString();
            lbl_sender_ip.Text = args.Socket.Information.RemoteAddress.ToString();

            // Sending reply
            //this.SocketManager.SendData(args.Socket.Information.RemoteAddress, "Hello Client!");
        }

    }
}
