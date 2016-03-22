

namespace JREndean.Fluent.Networking.NETMF
{
    using System.Threading;

    using GHI.Networking;

    using Microsoft.SPOT;
    using Microsoft.SPOT.Net.NetworkInformation;
    
    public class WirelessNetwork
    {
        private bool blockUntilIpReceived = false;

        private bool handleOnConnection = false;
        private bool handledOnConnection = false;

        public delegate void OnConnected(string ipAddress);

        public WirelessNetwork(BaseInterface networkInterface)
        {
            this.NetworkInterface = networkInterface;

            // TODO: hook up events
            //NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
        }

        private WirelessNetwork()
        {
        }

        public BaseInterface NetworkInterface
        {
            get;
            private set;
        }

        public WirelessNetwork UseDhcp(bool blockUntilIpReceived = true)
        {
            this.NetworkInterface.Open();
            this.NetworkInterface.EnableDhcp();
            this.NetworkInterface.EnableDynamicDns();

            this.blockUntilIpReceived = blockUntilIpReceived;

            return this;
        }

        public WirelessNetwork UseStatic(string ipAddress, string subnetMask, string gatewayAddress, params string[] dnsAddresses)
        {
            // TODO: validate the inputs

            this.NetworkInterface.Open();
            this.NetworkInterface.EnableStaticIP(ipAddress, subnetMask, gatewayAddress);
            this.NetworkInterface.EnableStaticDns(dnsAddresses);

            return this;
        }

        public WirelessNetwork Join(string ssid, string key)
        {
            // TODO: validate the inputs
            
            var wifiRS9110 = this.NetworkInterface as WiFiRS9110;

            if (wifiRS9110 != null)
            {
                wifiRS9110.Join(ssid, key);
            }
            else
            {
                // TODOD: do anything here?
            }

            if (this.blockUntilIpReceived)
            {
                while (this.NetworkInterface.IPAddress == "0.0.0.0")
                {
                    Thread.Sleep(250);
                }

                this.handleOnConnection = true;
            }

            return this;
        }

        public WirelessNetwork OnConnection(OnConnected onConnectedAction)
        {
            if (this.handleOnConnection && !this.handledOnConnection)
            {
                onConnectedAction.Invoke(this.NetworkInterface.IPAddress);
                this.handledOnConnection = true;
            }

            return this;
        }

        private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            if (this.NetworkInterface.IPAddress != "0.0.0.0")
            {
                this.handleOnConnection = true;
            }
        }
    }
}
