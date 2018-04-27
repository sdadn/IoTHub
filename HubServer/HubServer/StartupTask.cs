using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.Networking.Sockets;
using Windows.System.Threading;
using Windows.Storage.Streams;
using System.Net.Sockets;
using System.Net;
using Windows.Networking;
using HubLibrary;
using System.Diagnostics;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace HubServer
{
    public sealed class StartupTask : IBackgroundTask
    {
        //private  int _port;
        //public int Port { get { return _port; } }

        //private StreamSocketListener listener;

        //public delegate void DataRecived(string data);
        //public event DataRecived OnDataRecived;


        //public delegate void Error(string message);
        //public event Error OnError;

        StreamSocketClass SocketManager;
         StreamSocketListener DataListener;

        private string ServerPort = "12345";

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // 
            // TODO: Insert code to perform background work
            //
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            //

            //_port = 9000;
            //taskInstance.GetDeferral();
            ////var socket = new SocketServer(9000);
            //ThreadPool.RunAsync(x => {
            //                    Start();
            //                    OnError += socket_OnError;
            //                    OnDataRecived += Socket_OnDataRecived;
            //                    });

            // TcpListener s = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);

            Debug.WriteLine("starting");

            //SocketManager = new StreamSocketClass();

            DataListener = new StreamSocketListener();
            DataListener.ConnectionReceived += this.StreamSocketListener_ConnectionReceived;
            await DataListener.BindServiceNameAsync(ServerPort);

            // Declaring IsServer (True = server, False = client)
            StreamSocketClass.IsServer = true;
            // Declaring HostName of Server
            HostName ServerAdress = new HostName("heathHub");
            // Open Listening ports and start listening.
            //SocketManager.DataListener_OpenListenPorts();
            // Server
            if(StreamSocketClass.IsServer)
            {
                Debug.WriteLine("[SERVER] Ready to receive");
            }
            // Client
            //else
            //{
            //    SocketManager.SendResponse(ServerAdress, "Hello WindowsInstructed");
            //}
        }

        private async void StreamSocketListener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Debug.WriteLine("event fired");
            //DataReader DataListener_Reader;
            //StringBuilder DataListener_StrBuilder;
            //string DataReceived;

            //using (DataListener_Reader = new DataReader(args.Socket.InputStream))
            //{
            //    DataListener_StrBuilder = new StringBuilder();
            //    DataListener_Reader.InputStreamOptions = InputStreamOptions.Partial;
            //    DataListener_Reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
            //    DataListener_Reader.ByteOrder = ByteOrder.LittleEndian;
            //    await DataListener_Reader.LoadAsync(256);
            //    while (DataListener_Reader.UnconsumedBufferLength > 0)
            //    {
            //        DataListener_StrBuilder.Append(DataListener_Reader.ReadString(DataListener_Reader.UnconsumedBufferLength));
            //        await DataListener_Reader.LoadAsync(256);
            //    }
            //    DataListener_Reader.DetachStream();
            //    DataReceived = DataListener_StrBuilder.ToString();
            //}

            //if (DataReceived != null)
            //{
            //    //// Server
            //    //if (IsServer)
            //    //{
            //    //    Debug.WriteLine("[SERVER] I've received " + DataReceived + " from " + args.Socket.Information.RemoteHostName);
            //    //    // Sending reply
            //    //    SendResponse(args.Socket.Information.RemoteAddress, "Hello Client!");
            //    //}
            //    //// Client
            //    //else
            //    //{
            //    //    Debug.WriteLine("[CLIENT] I've received " + DataReceived + " from " + args.Socket.Information.RemoteHostName);
            //    //}
            //}
            //else
            //{
            //    Debug.WriteLine("Received data was empty. Check if you sent data.");
            //}

        }


        //public async void Start()
        //{
        //    listener = new StreamSocketListener();
        //    listener.ConnectionReceived += Listener_ConnectionReceived;
        //    await listener.BindServiceNameAsync(Port.ToString());
        //}

        //private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        //{
        //    var reader = new DataReader(args.Socket.InputStream);
        //    try
        //    {
        //        while (true)
        //        {
        //            uint sizeFieldCount = await reader.LoadAsync(sizeof(uint));
        //            //if disconnected 
        //            if (sizeFieldCount != sizeof(uint)) 
        //                return;
        //            uint stringLenght = reader.ReadUInt32(); 
        //            uint actualStringLength = await reader.LoadAsync(stringLenght); 
        //            //if disconnected 
        //            if (stringLenght != actualStringLength) 
        //                return; 
        //            if (OnDataRecived != null) 
        //                OnDataRecived(reader.ReadString(actualStringLength));
        //            } 
        //    } 
        //    catch (Exception ex)
        //    { 
        //        if (OnError != null) 
        //            OnError(ex.Message);
        //    }
        //}


    }
}
