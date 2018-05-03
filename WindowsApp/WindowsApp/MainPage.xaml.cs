﻿using HubLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Popups;
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

        public MainPage()
        {
            this.InitializeComponent();
            StreamSocketClass.OpenListenPorts(eventfunction: Pages.WinData.__ConnectionReceivedDefault, t: "win" );
            DataAccess.Win.InitializeDB_WIN();
            Pages.WinData.hasHub = false;
            //check_host:

            if(DataAccess.Win.CheckHub())
            {
                Hashtable h = DataAccess.Win.GetHub();

                HubData.HubHost = (string)h["Hostname"];

                Debug.WriteLine(HubData.HubHost);
            }

            //txt_hostname.Text = "MSI";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            contentFrame.Navigate(typeof(devicePage));

            
        }

        private void __btn_send_Click(object sender, RoutedEventArgs e)
        {
            DataAccess.Hub.resetDB();

            bool x = DataAccess.Hub.CheckAdmin();

            Debug.WriteLine("Num IsAdmin rows = " + x.ToString());


            //SocketManager.SendData(new HostName(txt_hostname.Text), txt_msg.Text);
        }

        private void __btn_addAdmin_Click(object sender, RoutedEventArgs e)
        {
            DataAccess.Hub.AddAdmin("name", "pass");
        }

        private void __btn_readData_Click(object sender, RoutedEventArgs e)
        {

        }

        private void __btn_addDevice_Click(object sender, RoutedEventArgs e)
        {

        }


        public async void __connectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Debug.WriteLine("event fired");

            var m =  new MessageDialog("event fired");

            await m.ShowAsync();

            string data = await StreamSocketClass.ExtractReceivedData(args.Socket.InputStream);

        }


        // ---------------- NavView Events -------------------

        private void __NavView_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void __navView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            args.SelectedItem.ToString();
        }

        private void __navView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            //contentFrame.Navigate()

            NavigationViewItem item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
      
            switch (item.Tag){
                case "devicePage":
                    contentFrame.Navigate(typeof(devicePage));

                    break;
                case "userPage":
                    contentFrame.Navigate(typeof(userPage));

                    break;
                case "adminPage":
                    contentFrame.Navigate(typeof(adminPage));

                    break;
            }



        }
    }
}
