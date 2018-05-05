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

using Windows.Networking;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace pulse_App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        const string deviceID = "pulse12345";
        static string deviceName = "healthSensor";
        bool hasHub;
        //StreamSocketClass socketManager;

        public MainPage()
        {
            this.InitializeComponent();

            DataAccess.Device.InitializeDB_DEVICE();



            deviceName = "Sensor";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // wifi.test_access();
            // wifi.Get_adapters();
            //wifi.networks_scan("SSM");

            StreamSocketClass.OpenListenPorts( t: "device", eventfunction: this.__ConnectionReceived);
        }
        public async void __ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            //Debug.WriteLine("[ " + deviceName + " ]: Receive event fired.");

            //DataReader DataListener_Reader;
            string DataReceived = await StreamSocketClass.ExtractReceivedData(args.Socket.InputStream);

            if (DataReceived == null)
            {
                Debug.WriteLine("Received data was empty. Check if you sent data.");
                return;
            }

            Debug.WriteLine("[ " + deviceName + " ] received input from " + args.Socket.Information.RemoteHostName);

            //lbl_received_data.Text = DataReceived;
            //lbl_sender.Text = args.Socket.Information.RemoteHostName.ToString();
            //lbl_sender_ip.Text = args.Socket.Information.RemoteAddress.ToString();

            // Sending reply
            //this.SocketManager.SendData(args.Socket.Information.RemoteAddress, "Hello Client!");
            //args.Socket.Information.ho

            Response r = this.ParseInput(args.Socket.Information.RemoteHostName,DataReceived);

            StreamSocketClass.SendData(r.dest, r.message);
        }

        public Response ParseInput(HostName host, string input)
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
                    break;
                case "3":
                    //add device
                    //3__deviceHost__admin__pass

                    //DataAccess.Hub.AddDevice("ip", s[2]);

                    //ret.message = "5_" + HubData.HubHost;
                    //ret.dest = new HostName(s[2]);
                    break;
                case "4":

                    break;
                case "5":
                    //add hub
                    //5__username__password
                    //Debug.WriteLine("case 5 started");
                    //if(DataAccess.Device.CheckHub())
                    //{
                    //    Debug.WriteLine("Hub already exists");
                    //    ret.message = "exists";
                    //    break;
                    //}

                    //DataAccess.Device.addHub(s[1], s[2], s[3]);
                    Debug.WriteLine("Registered to Hub: [hub001]");

                    hasHub = true;
                    //Debug.WriteLine("case 5");
                    ret.message = "8__devicesuccess";
                    ret.dest = new HostName("healthHub");
                    break;

                case "6":
                    //Debug.WriteLine("case 6 started");
                    //DataAccess.Device.resetDB();
                    Debug.WriteLine("Db Reset");

                    ret.message = "6__success";
                    break;
                case "7":

                    //read data
              
                    if(hasHub)
                    {
                        if(s[1]=="user")
                        {
                            Debug.WriteLine("ignoring user request");

                            ret.dest = new HostName("MSI");
                            ret.message = "7__fail";
                            break;
                        }
                        Debug.WriteLine("Sending data to Hub");


                        ret.message = "7__hub__sensorData";
                        ret.dest = new HostName("healthHub");
                        break;
                    }

                    ret.message = "7__sensorData";
                    break;

                default:

                    break;
            }
            return ret;
        }

    }
}
