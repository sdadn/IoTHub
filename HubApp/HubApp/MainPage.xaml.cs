﻿using System;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HubApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        wifiConnection wifi = new wifiConnection();
        // WiFiAdapter w_adapter;
        StreamSocketClass SocketManager = new StreamSocketClass();
        static string PortNumber = "4040";

        public MainPage()
        {
            this.InitializeComponent();

            // Declaring IsServer (True = server, False = client)
            StreamSocketClass.IsServer = true;
            // Declaring HostName of Server
            HostName ServerAdress = new HostName("healthHub");
            // Open Listening ports and start listening.
            SocketManager.DataListener_OpenListenPorts();
            // Server
            if(StreamSocketClass.IsServer)
            {
                Debug.WriteLine("[SERVER] Ready to receive");
            }
            // Client
            else
            {
                SocketManager.SendResponse(ServerAdress, "Hello WindowsInstructed");
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

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

        private async void btn_scanWifi_Click(object sender, RoutedEventArgs e)
        {
            // await wifi.networks_scan("SSM");
        }


    }
}
