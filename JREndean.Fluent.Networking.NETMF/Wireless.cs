

namespace JREndean.Fluent.Networking.NETMF
{
    using System;
    using System.IO.Ports;
    using System.Text;
    using System.Threading;

    using GT = Gadgeteer;
    using GHI.Networking;

    using Microsoft.SPOT.Hardware;

    public class Wireless
    {
        public WirelessNetwork RS9110(int socketNumber)
        {
            // TODO: validate the inputs

            var socket = GT.Socket.GetSocket(socketNumber, true, null, null);

            if (!socket.SupportsType('S'))
            {
                throw new NotSupportedException("That socket does not support the required socket type");
            }

            return this.RS9110(socket.SPIModule, socket.CpuPins[6], socket.CpuPins[3], socket.CpuPins[4]);
        }

        public WirelessNetwork RS9110(SPI.SPI_module spi, Cpu.Pin chipSelect, Cpu.Pin externalInterrupt, Cpu.Pin reset)
        {
            // TODO: validate the inputs

            return new WirelessNetwork(new WiFiRS9110(spi, chipSelect, externalInterrupt, reset));
        }

        public WirelessNetwork PPP(int socketNumber, string apn)
        {
            // TODO: validate the inputs

            var socket = GT.Socket.GetSocket(socketNumber, true, null, null);

            if (!socket.SupportsType('K'))
            {
                throw new NotSupportedException("That socket does not support the required socket type");
            }

            //new CellularRadio(socketNumber).

            return this.PPP(socket.SerialPortName, apn);
        }

        public WirelessNetwork PPP(string comPort, string apn)
        {
            // TODO: validate the inputs

            var evt = new AutoResetEvent(false);
            PPPSerialModem netif = null;

            Microsoft.SPOT.Net.NetworkInformation.NetworkChange.NetworkAvailabilityChanged += (o, e) =>
            {
                if (e.IsAvailable)
                    evt.Set();
            };

            Microsoft.SPOT.Net.NetworkInformation.NetworkChange.NetworkAddressChanged += (o, e) =>
            {
                Microsoft.SPOT.Debug.Print(netif.IPAddress);
            };

            using (var port = new SerialPort(comPort, 115200, Parity.None, 8, StopBits.One))
            {
                port.Open();

                port.DiscardInBuffer();
                port.DiscardOutBuffer();

                SendATCommand(port, "AT+CGDCONT=2,\"IP\",\"" + apn + "\"");
                SendATCommand(port, "ATDT*99***2#");

                using (netif = new PPPSerialModem(port))
                {
                    netif.Open();
                    netif.Connect(PPPSerialModem.AuthenticationType.Pap, "", "");

                    evt.WaitOne();

                    //The network is now ready to use.

                    return new WirelessNetwork(netif);
                }
            }
        }

        private static void SendATCommand(SerialPort port, string command)
        {
            var sendBuffer = Encoding.UTF8.GetBytes(command + "\r");
            var readBuffer = new byte[256];
            var read = 0;

            port.Write(sendBuffer, 0, sendBuffer.Length);

            while (true)
            {
                read += port.Read(readBuffer, read, readBuffer.Length - read);

                var response = new string(Encoding.UTF8.GetChars(readBuffer, 0, read));

                if (response.IndexOf("OK") != -1 || response.IndexOf("CONNECT") != -1)
                    break;
            }
        }
    }
}
