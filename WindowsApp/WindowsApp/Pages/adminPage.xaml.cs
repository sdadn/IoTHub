using HubLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WindowsApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class adminPage : Page
    {
        string abc;
        public adminPage()
        {
            this.InitializeComponent();

            //Binding binding = new Binding();
            //binding.Source = typeof(MyStaticClass);
            // System.InvalidOperationException: 'Binding.StaticSource cannot be set while using Binding.Source.'
            //binding.Path = new PropertyPath(typeof(Pages.WinData).GetProperty(nameof(Pages.WinData.deviceIP)));
            //binding.Mode = BindingMode.TwoWay;
            //binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //this.SetBinding(txt_test.Text, binding);
        }

        private void __btn_send_Click(object sender, RoutedEventArgs e)
        {
            //DataAccess.Hub.resetDB();

            //bool x = DataAccess.Hub.CheckAdmin();

            //Debug.WriteLine("Num IsAdmin rows = " + x.ToString());


            //SocketManager.SendData(new HostName(txt_hostname.Text), txt_msg.Text);

            StreamSocketClass.SendData(new Windows.Networking.HostName(txt_hostname.Text), txt_msg.Text);
        }


        private void btn_resetHubDB_Click(object sender, RoutedEventArgs e)
        {
            StreamSocketClass.SendData(new Windows.Networking.HostName("healthHub"), "6");
        }
    }
}
