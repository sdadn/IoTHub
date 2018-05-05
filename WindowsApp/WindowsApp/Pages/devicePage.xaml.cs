using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using HubLibrary;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WindowsApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class devicePage : Page
    {
        public devicePage()
        {
            this.InitializeComponent();
        }

        // ------------ Button events

        private void btn_addDevice_Click(object sender, RoutedEventArgs e)
        {
            //   StreamSocketClass.SendData("");

            if (txt_deviceHostName_addDevice.Text == "")
                return;
            string command = "3__ip_" + txt_deviceHostName_addDevice.Text + "__" + txt_AdminName_newHub.Text + "__"+ txt_Adminpass_newHub.Password;

            StreamSocketClass.SendData(new Windows.Networking.HostName(HubData.HubHost),command);
        }

        private void btn_addHub_Click(object sender, RoutedEventArgs e)
        {
            if (txt_HubHost_newHub.Text == "")
                return;
            string command = "5__" + txt_AdminName_newHub.Text + "__" + txt_Adminpass_newHub.Password;
            StreamSocketClass.SendData(new Windows.Networking.HostName(txt_HubHost_newHub.Text), command);       
        }

        private void btn_addHub_Click_1(object sender, RoutedEventArgs e)
        {
            if (txt_HubHost_newHub.Text == "")
                return;
            string command = "5__" + txt_AdminName_newHub.Text + "__" + txt_Adminpass_newHub.Password;
            StreamSocketClass.SendData(new Windows.Networking.HostName(txt_HubHost_newHub.Text), command);
        }

        private void btn_addDevice_Click_1(object sender, RoutedEventArgs e)
        {
            if (txt_deviceHostName_addDevice.Text == "")
                return;
            string command = "3__ip_" + txt_deviceHostName_addDevice.Text + "__" + txt_AdminName_newHub.Text + "__" + txt_Adminpass_newHub.Password;

            StreamSocketClass.SendData(new Windows.Networking.HostName(HubData.HubHost), command);
        }
    }
}
