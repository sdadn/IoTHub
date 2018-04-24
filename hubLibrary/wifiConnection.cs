using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;
using Windows.Security.Credentials;
using Windows.UI.Popups;

namespace HubLibrary
{
    class wifiConnection
    {
        bool adapter_access = false;
        bool adapter_available = false;
        WiFiAvailableNetwork target_net = null;

        WiFiAdapter adptr;

        public bool hasAccess
        {
            get { return this.adapter_access; }
        }

        public bool adapterIsAvailable
        {
            get { return this.adapter_available; }
        }

        public WiFiAdapter adapter{
            get {return this.adptr; }
        }


        public async void test_access()
        {
            adapter_access = false;

            var access = await WiFiAdapter.RequestAccessAsync();

            if (access != WiFiAccessStatus.Allowed)
            {
                MessageDialog msg = new MessageDialog("No adapter access.");
                await msg.ShowAsync();
                return;
            }

            adapter_access = true;
        }
        
        public async Task<int> Get_adapters()
        {
            adapter_available = false;

            var adapterResults = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());

            if (adapterResults.Count < 1)
            {
                MessageDialog msg = new MessageDialog("No adapters found.");
                await msg.ShowAsync();
                return 0;
            }

            this.adptr = await WiFiAdapter.FromIdAsync(adapterResults[0].Id);
            adapter_available = true;

            await new MessageDialog("adapter set").ShowAsync();
            return 1;
        }

        public async Task<int> networks_scan(string net_ssid)
        {
            await this.adptr.ScanAsync();

            StringBuilder s = new StringBuilder();

            var networks = this.adptr.NetworkReport;

            // WiFiAvailableNetwork target_net = null;

            foreach (var net in networks.AvailableNetworks)
            {
                s.Append(net.Ssid);

                s.AppendLine();

                if (net.Ssid == net_ssid)
                    target_net = net;
            }
            if (target_net == null)
                return 0;

            MessageDialog m = new MessageDialog(s.ToString());
            await m.ShowAsync();

            return 1;
        }

        public async void connectToNetwork()
        {
            if (target_net == null)
                return;

            var credential = new PasswordCredential();

            credential.UserName = "m743a983";
            credential.Password = "ethanolC2H5";

            var x = await this.adptr.ConnectAsync(target_net, WiFiReconnectionKind.Automatic, credential);

            if(x.ConnectionStatus == WiFiConnectionStatus.Success)
                await new MessageDialog("success").ShowAsync();
        }

        public async void connectToNetwork(string password, string UserName)
        {
            if (target_net == null)
                return;

            var credential = new PasswordCredential();

            credential.UserName = "m743a983";
            credential.Password = "ethanolC2H5";

            var x = await this.adptr.ConnectAsync(target_net, WiFiReconnectionKind.Automatic, credential);

            if(x.ConnectionStatus == WiFiConnectionStatus.Success)
                await new MessageDialog("success").ShowAsync();
        }

    }
}
