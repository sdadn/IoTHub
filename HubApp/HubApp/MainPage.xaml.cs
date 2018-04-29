using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Devices.Gpio;
using System.Diagnostics;
using Windows.ApplicationModel.Background;

using Windows.Devices.WiFi;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Enumeration;
using System.Text;
using Windows.Security.Credentials;
using HubLibrary;
using Windows.Networking.Sockets;
using Windows.Networking;
using Windows.Storage.Streams;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HubApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        static string deviceName = "Hub";
        // WiFiAdapter w_adapter;

        StreamSocketClass SocketManager ;
        static string PortNumber = "4040";

        public MainPage()
        {
            this.InitializeComponent();
            SocketManager = new StreamSocketClass();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            SocketManager.OpenListenPorts();

            Debug.WriteLine("["+ deviceName +"] Ready to send & receive");

        }

        private async void socket_Listener(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs e)
        {
            string req;
            using (var reader = new StreamReader(e.Socket.InputStream.AsStreamForRead()))
            {
                req = await reader.ReadLineAsync();
            }
            //await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, ()=>this.)

            using (Stream output = e.Socket.OutputStream.AsStreamForWrite())
            {
                using (var streamWriter = new StreamWriter(output))
                {
                    await streamWriter.WriteLineAsync(req);
                    await streamWriter.FlushAsync();
                }
            }
        }

        private  void btn_scanWifi_Click(object sender, RoutedEventArgs e)
        {
            // await wifi.networks_scan("SSM");
        }


        public async void __ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Debug.WriteLine("[ " + deviceName + " ]: Receive event fired.");

            //DataReader DataListener_Reader;
            string DataReceived = await SocketManager.ExtractReceivedData(args.Socket.InputStream);


            if (DataReceived == null)
            {
                Debug.WriteLine("Received data was empty. Check if you sent data.");
                return;
            }
       
            Debug.WriteLine("[ "+ deviceName +" ] received " + DataReceived + " from " + args.Socket.Information.RemoteHostName);

            lbl_received_data.Text = DataReceived;
            lbl_sender.Text = args.Socket.Information.RemoteHostName.ToString();
            lbl_sender_ip.Text = args.Socket.Information.RemoteAddress.ToString();
         
            // Sending reply
            //this.SocketManager.SendData(args.Socket.Information.RemoteAddress, "Hello Client!");




        }


    }
}
