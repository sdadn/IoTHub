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

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace HubServer
{
    public sealed class StartupTask : IBackgroundTask
    {
        private  int _port;
        public int Port { get { return _port; } }

        private StreamSocketListener listener;

        public delegate void DataRecived(string data);
        public event DataRecived OnDataRecived;


        public delegate void Error(string message);
        public event Error OnError;

        public void Run(IBackgroundTaskInstance taskInstance)
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

            TcpListener s = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);
        }


        public async void Start()
        {
            listener = new StreamSocketListener();
            listener.ConnectionReceived += Listener_ConnectionReceived;
            await listener.BindServiceNameAsync(Port.ToString());
        }

        private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            var reader = new DataReader(args.Socket.InputStream);
            try
            {
                while (true)
                {
                    uint sizeFieldCount = await reader.LoadAsync(sizeof(uint));
                    //if disconnected 
                    if (sizeFieldCount != sizeof(uint)) 
                        return;
                    uint stringLenght = reader.ReadUInt32(); 
                    uint actualStringLength = await reader.LoadAsync(stringLenght); 
                    //if disconnected 
                    if (stringLenght != actualStringLength) 
                        return; 
                    if (OnDataRecived != null) 
                        OnDataRecived(reader.ReadString(actualStringLength));
                    } 
            } 
            catch (Exception ex)
            { 
                if (OnError != null) 
                    OnError(ex.Message);
            }
        }


    }
}
