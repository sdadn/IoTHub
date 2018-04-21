using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.WiFi;
using Windows.UI.Popups;

namespace HubApp
{
    class wifiConnection
    {
        bool adapter_access;


        public bool hasAccess
        {
            get { return this.adapter_access; }
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


    }
}
