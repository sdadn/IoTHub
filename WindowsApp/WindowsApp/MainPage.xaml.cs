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
        StreamSocketClass SocketManager;
        HostName ServerAdress = new HostName("healthHub");

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            SocketManager = new StreamSocketClass(event_function = this.__connectionReceived);
            SocketManager.OpenListenPorts();
        }

        private void __btn_send_Click(object sender, RoutedEventArgs e)
        {
            SocketManager.SendResponse(new HostName(txt_hostname.Text), txt_msg.Text);
        }

        public async void __connectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Debug.WriteLine("event fired");
            DataReader DataListener_Reader;
            string DataReceived;

            using (DataListener_Reader = new DataReader(args.Socket.InputStream))
            {
                StringBuilder builder;
                builder = new StringBuilder();
                DataListener_Reader.InputStreamOptions = InputStreamOptions.Partial;
                DataListener_Reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                DataListener_Reader.ByteOrder = ByteOrder.LittleEndian;

                await DataListener_Reader.LoadAsync(256);

                while (DataListener_Reader.UnconsumedBufferLength > 0)
                {
                    builder.Append(DataListener_Reader.ReadString(DataListener_Reader.UnconsumedBufferLength));
                    await DataListener_Reader.LoadAsync(256);
                }
                DataListener_Reader.DetachStream();
                DataReceived = builder.ToString();
            }
 
            if(DataReceived != null)
            {
                // Server
                if(IsServer)
                {
                    Debug.WriteLine("[SERVER] I've received " + DataReceived + " from " + args.Socket.Information.RemoteHostName);
                    // Sending reply
                    SendResponse(args.Socket.Information.RemoteAddress, "Hello Client!");
                }
                // Client
                else
                {
                    Debug.WriteLine("[CLIENT] I've received " + DataReceived + " from " + args.Socket.Information.RemoteHostName);
                }
            }
            else
            {
                Debug.WriteLine("Received data was empty. Check if you sent data.");
            }

        }



    }
}
