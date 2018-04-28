
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
 
 
namespace HubLibrary
{
    class StreamSocketClass
    {
        public static bool IsServer { get; set; }
        // Change this. True = server, false = client
        private string ServerPort;
 
        private StreamSocket ConnectionSocket;

        StreamSocketListener listener { get; set; }

        public StreamSocketClass(string port = "12345")
        {
            this.ServerPort = port;

            listener = new StreamSocketListener();
            listener.ConnectionReceived += this.__ConnectionReceivedDefault;
        }

        public StreamSocketClass(Windows.Foundation.TypedEventHandler<StreamSocketListener, StreamSocketListenerConnectionReceivedEventArgs> event_function,
                                    string port = "12345")
        {
            this.ServerPort = port;

            listener = new StreamSocketListener();
            listener.ConnectionReceived += event_function;
        }

        public async void OpenListenPorts()
        {
            await listener.BindServiceNameAsync(this.ServerPort);
        }

        public void getIP()
        {

        }

        public void IP_Scan()
        {

        }

        public async void __ConnectionReceivedDefault(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Debug.WriteLine("Default Event fired");

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

            
 
            if(DataReceived == null)
            {
                Debug.WriteLine("Received data was empty. Check if you sent data.");
                return;
            }
                // Server
            if(IsServer)
            {
                Debug.WriteLine("[SERVER] I've received " + DataReceived + " from " + args.Socket.Information.RemoteHostName);
                // Sending reply
                SendData(args.Socket.Information.RemoteAddress, "Hello Client!");

                return;
            }
            // Client
            Debug.WriteLine("[CLIENT] I've received " + DataReceived + " from " + args.Socket.Information.RemoteHostName);
        }

        public async void SendData(HostName address, string DataToSend)
        {
           try
            {
                // Try connect
                Debug.WriteLine("Attempting to connect. " + Environment.NewLine);

                ConnectionSocket = new StreamSocket();

                // Wait on connection
                await ConnectionSocket.ConnectAsync(address, ServerPort);

                // Create a DataWriter
                DataWriter writer = new DataWriter(ConnectionSocket.OutputStream);
                byte[] data = Encoding.UTF8.GetBytes(DataToSend);

                // Write the bytes
                writer.WriteBytes(data);

                // Store the written data
                await writer.StoreAsync();
                writer.DetachStream();

                // Dispose the data
                writer.Dispose();
                Debug.WriteLine("Connection has been made and your message " + DataToSend + " has been sent." + Environment.NewLine);

                // Dispose the connection.
                ConnectionSocket.Dispose();
                ConnectionSocket = new StreamSocket();
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Failed to connect " + exception.Message);
                ConnectionSocket.Dispose();
                ConnectionSocket = null;
 
            }
        }
    }
}