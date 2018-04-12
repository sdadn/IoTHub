using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

using Windows.Devices.WiFi;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Enumeration;
using System.Text;
using Windows.Security.Credentials;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HubApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        WiFiAdapter w_adapter;
        public MainPage()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            var access = await WiFiAdapter.RequestAccessAsync();

            if (access != WiFiAccessStatus.Allowed)
            {
                MessageDialog msg = new MessageDialog("No adapter access.");
                await msg.ShowAsync();
                return;
            }

            var adapterResults = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());

            if (adapterResults.Count < 1)
            {
                MessageDialog msg = new MessageDialog("No adapters found.");
                await msg.ShowAsync();
                return;
            }

            await new MessageDialog("adapter set").ShowAsync();

            w_adapter = await WiFiAdapter.FromIdAsync(adapterResults[0].Id);

            await w_adapter.ScanAsync();

            StringBuilder s = new StringBuilder();
            var networks = w_adapter.NetworkReport;
            WiFiAvailableNetwork jayhawk = null;

            foreach (var net in networks.AvailableNetworks )
            {
                s.Append(net.Ssid);

                s.AppendLine();

                if (net.Ssid == "JAYHAWK")
                    jayhawk = net;
            }
            if (jayhawk == null)
                return;

            MessageDialog m = new MessageDialog(s.ToString());
            await m.ShowAsync();

            var credential = new PasswordCredential();

            credential.UserName = "m743a983";
            credential.Password = "ethanolC2H5";

            var x = await w_adapter.ConnectAsync(jayhawk, WiFiReconnectionKind.Automatic, credential);

            if(x.ConnectionStatus == WiFiConnectionStatus.Success)
                await new MessageDialog("success").ShowAsync();

        }

        private async void btn_scanWifi_Click(object sender, RoutedEventArgs e)
        {
            //await new MessageDialog(w_adapter.NetworkAdapter.NetworkAdapterId.ToString()).ShowAsync();

            await w_adapter.ScanAsync();

            StringBuilder s = new StringBuilder();
            var networks = w_adapter.NetworkReport;
            WiFiAvailableNetwork jayhawk = null;

            foreach (var net in networks.AvailableNetworks )
            {
                s.Append(net.Ssid);

                s.AppendLine();

                if (net.Ssid == "JAYHAWK")
                    jayhawk = net;
            }
            if (jayhawk == null)
                return;

            MessageDialog m = new MessageDialog(s.ToString());
            await m.ShowAsync();

            var credential = new PasswordCredential();

            credential.UserName = "m743a983";
            credential.Password = "ethanolC2H5";

            var x = await w_adapter.ConnectAsync(jayhawk, WiFiReconnectionKind.Automatic, credential);

            if(x.ConnectionStatus == WiFiConnectionStatus.Success)
                await new MessageDialog("success").ShowAsync();
        }
    }
}
