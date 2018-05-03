using HubLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.Networking;

namespace WindowsApp.Pages
{
    public static class WinData
    {
        // ---- Hub -----
        public static bool hasHub = false;
        public static string hubIP = null;
        public static string hubHostname = null;

        // ---- Device ----
        public static string deviceIP = null;
        public static string deviceHostname = null;

        public async static void __ConnectionReceivedDefault(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Debug.WriteLine("WinData Event fired");

            string DataReceived;

            DataReceived = await StreamSocketClass.ExtractReceivedData(args.Socket.InputStream);

            if (DataReceived == null)
            {
                Debug.WriteLine("Received data was empty. Check if you sent data.");
                return;
            }
     
            Debug.WriteLine("[Win] I've received " + DataReceived + " from " + args.Socket.Information.RemoteHostName);

            Response r = ParseInput(args.Socket.Information.RemoteHostName, DataReceived);


            //Debug.WriteLine("entering switch");
            //switch (type)
            //{
            //    case "win":
            //        r = StreamSocketClass.ParseInput_Win(args.Socket.Information.RemoteAddress, DataReceived);
            //        break;
            //    case "hub":
            //        //Debug.WriteLine("case hub");
            //        r = StreamSocketClass.ParseInput_Hub(args.Socket.Information.RemoteAddress, DataReceived);
            //        Debug.WriteLine("parsed input");
            //        break;
            //    case "device":
            //        break;

            //}



            //StreamSocketClass.SendData(r.dest, r.message);
            //Debug.WriteLine(r.dest);

        }
        public static Response ParseInput(HostName host, string input)
        {
            Response ret = new Response(host, null);
            string[] s = input.Split("__");
            switch (Int32.Parse(s[0]))
            {
                case 3:
                    //adding device
                    break;
                case 5:
                    //5__success__SR__hostname__IP
                    if (s[1] == "success")
                    {
                        HubData.HubSR = s[2];
                        HubData.HubHost = s[3];
                        HubData.HubIP = s[4];
                        DataAccess.Win.addHub(HubData.HubSR, HubData.HubHost, HubData.HubIP);
                        Debug.WriteLine("Hub [" + HubData.HubSR + "] successfully added");
                    }
                    Debug.WriteLine("Adding new hub failed. hub is either unavailable or already exists.");
                    break;
                case 6:
                    if (s[1] == "success")
                    {
                        Debug.WriteLine("Hub database successfully reset");
                        //ret.message = "6__success";
                        break;
                    }
                    break;
            }

            return ret;
        }


    }
}
    