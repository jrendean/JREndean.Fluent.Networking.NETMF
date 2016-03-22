

namespace JREndean.Fluent.Networking.NETMF
{
    using System.Threading;

    using GHI.Networking;

    using Microsoft.SPOT;
    using Microsoft.SPOT.Net.NetworkInformation;

    public class WiredNetwork
    {
        private bool handleOnConnection = false;
        private bool handledOnConnection = false;

        public delegate void OnConnected(string ipAddress);

        public WiredNetwork(BaseInterface networkInterface)
        {
            this.NetworkInterface = networkInterface;

            // TODO: hook up events
            //NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
        }

        private WiredNetwork()
        {
        }

        public BaseInterface NetworkInterface
        {
            get;
            private set;
        }

        public WiredNetwork UseDhcp(bool blockUntilIpReceived = true)
        {
            this.NetworkInterface.Open();
            this.NetworkInterface.EnableDhcp();
            this.NetworkInterface.EnableDynamicDns();

            if (blockUntilIpReceived)
            {
                while (this.NetworkInterface.IPAddress == "0.0.0.0")
                {
                    Thread.Sleep(250);
                }
            }

            return this;
        }

        public WiredNetwork UseStatic(string ipAddress, string subnetMask, string gatewayAddress, params string[] dnsAddresses)
        {
            // TODO: validate the inputs

            this.NetworkInterface.Open();
            this.NetworkInterface.EnableStaticIP(ipAddress, subnetMask, gatewayAddress);
            this.NetworkInterface.EnableStaticDns(dnsAddresses);

            return this;
        }

        public WiredNetwork OnConnection(OnConnected onConnectedAction)
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
