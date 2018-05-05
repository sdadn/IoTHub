
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
    public struct Response
    {
        public HostName dest;
        public string message;

        public Response(HostName h, string m)
        {
            dest = h;
            message = m;
        }
    }

    public static class HubData
    {
        public static string HubIP;
        public static string HubHost;
        public static string HubSR;

        public static string DeviceIP;
        public static string DeviceHost;
    }

    public static class StreamSocketClass
    {
      
        public static bool IsServer { get; set; }
        // Change this. True = server, false = client

        public static string type;
        private static string serverPort; 
        private static  StreamSocket connectionSocket;

        static StreamSocketListener Listener { get; set; }

        public static async void OpenListenPorts(string t, string port = "12345")
        {
            type = t;
            serverPort = port;
            Listener = new StreamSocketListener();
            Listener.ConnectionReceived += __ConnectionReceivedDefault;
            await Listener.BindServiceNameAsync(serverPort);
        }
        public static async void OpenListenPorts(Windows.Foundation.TypedEventHandler<StreamSocketListener, StreamSocketListenerConnectionReceivedEventArgs> eventfunction, string t, string port = "12345")
        {
            type = t;
            serverPort = port;
            Listener = new StreamSocketListener();
            Listener.ConnectionReceived += eventfunction;
            await Listener.BindServiceNameAsync(serverPort);
        }

        public static void getIP()
        {
        }

        public static void IP_Scan()
        {

        }

        public static async Task<string> ExtractReceivedData(IInputStream stream)
        {
            DataReader DataListener_Reader = new DataReader(stream);

            StringBuilder builder = new StringBuilder();

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

            return builder.ToString();
        }

        async static void __ConnectionReceivedDefault(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Debug.WriteLine("Default Event fired");

            string DataReceived;

            //using (DataReader DataListener_Reader = new DataReader(args.Socket.InputStream))
            //{
            //    StringBuilder builder;
            //    builder = new StringBuilder();
            //    DataListener_Reader.InputStreamOptions = InputStreamOptions.Partial;
            //    DataListener_Reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
            //    DataListener_Reader.ByteOrder = ByteOrder.LittleEndian;

            //    await DataListener_Reader.LoadAsync(256);

            //    while (DataListener_Reader.UnconsumedBufferLength > 0)
            //    {
            //        builder.Append(DataListener_Reader.ReadString(DataListener_Reader.UnconsumedBufferLength));
            //        await DataListener_Reader.LoadAsync(256);
            //    }
            //    DataListener_Reader.DetachStream();
            //    DataReceived = builder.ToString();
            //}

            DataReceived = await ExtractReceivedData(args.Socket.InputStream);
            
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

                //return;
            }
            // Client
            Debug.WriteLine("[Hub] I've received input from " + args.Socket.Information.RemoteHostName);

            Response r = new Response(null, null);

            switch(type)
            {
                case "win":
                    r = ParseInput_Win(args.Socket.Information.RemoteAddress, DataReceived);
                    break;
                case "hub":
                    //Debug.WriteLine("case hub");
                    r = ParseInput_Hub(args.Socket.Information.RemoteAddress, DataReceived);
                    break;
                case "device":
                    break;

            }
            SendData(r.dest, r.message);
            Debug.WriteLine(r.dest);
        }

        public static async void SendData(HostName address, string DataToSend)
        {
            if (address == null)
                return;

           try
            {
                // Try connect
                Debug.WriteLine("Attempting to connect. " + Environment.NewLine);

                connectionSocket = new StreamSocket();

                // Wait on connection
                await connectionSocket.ConnectAsync(address, serverPort);

                // Create a DataWriter
                DataWriter writer = new DataWriter(connectionSocket.OutputStream);

                byte[] data = Encoding.UTF8.GetBytes(DataToSend);

                // Write the bytes
                writer.WriteBytes(data);

                // Store the written data
                await writer.StoreAsync();
                writer.DetachStream();

                // Dispose the data
                writer.Dispose();
                //Debug.WriteLine("Connection has been made and your message " + DataToSend + " has been sent." + Environment.NewLine);

                // Dispose the connection.
                connectionSocket.Dispose();
                connectionSocket = new StreamSocket();
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Failed to connect " + exception.Message);
                connectionSocket.Dispose();
                connectionSocket = null;
 
            }
        }

        public static Response ParseInput_Hub(HostName host, string input)
        {
            var s = input.Split("__");

            Response ret = new Response(host, "");


            switch (s[0])
            {
                // Create User
                case "1":

                    break;
                case "2":
                    //add admin
                    //2_username_pass
                    int result = DataAccess.Hub.AddAdmin(s[1], s[2]);

                    if (result == 0)
                    {
                        ret = new Response(host,"2__fail");
                        break;
                    }

                    ret = new Response(host, "2__success");

                    break;
                case "3":
                    //add device
                    //3__deviceHost__admin_pass
                    //if (DataAccess.Hub.AuthAdmin(s[2], s[3]))
                    //{
                    //    ret.message = "3_authfail";
                    //    break;
                    //}

                    //if (DataAccess.Hub.CheckAdmin())
                    //{
                    //    ret.message = "3__authfail";
                    //    break;
                    //}


                    //DataAccess.Hub.AddDevice("ip", s[2]);
                    Debug.WriteLine("Adding device ...");

                    ret.message = "5__addhub";
                    ret.dest = new HostName("healthSensor");

                    break;
                case "4":

                    break;
                case "5":
                    //add hub
                    //5__username__password
                    Debug.WriteLine("Setting up Hub");
                    //DataAccess.Hub.AddAdmin(s[1], s[2]);
                    //Debug.WriteLine("case 5");
                    ret.message = "5__success__"+HubData.HubSR+"__"+HubData.HubHost+"__"+HubData.HubIP;
                    break;

                case "6":
                    DataAccess.Hub.resetDB();
                    Debug.WriteLine("Db Reset");

                    ret.message = "6__success";
                    break;

                case "7":
                    Debug.WriteLine("Relaying sensor data to PC");

                    ret.message = "7__sensordata";
                    ret.dest = new HostName("MSI");


                    break;
                case "8":
                    Debug.WriteLine("Device registered");

                    ret.message = "8";
                    ret.dest = new HostName("MSI");
                    break;

                default:

                    break;
            }
            return ret;
        }

        public static Response ParseInput_Win(HostName host, string input)
        {
            Response ret = new Response(host, null);
            string[] s = input.Split("__");
            switch(Int32.Parse(s[0]))
            {
                case 5:
                    if(s[1]=="success")
                    {
                        DataAccess.Win.addHub(HubData.HubSR, HubData.HubHost, HubData.HubIP);
                    }
                    break;
                case 6:
                    if(s[1] == "success")
                    ret.message = "6__success";
                    break;
            }

            return ret;
        }

    }
}