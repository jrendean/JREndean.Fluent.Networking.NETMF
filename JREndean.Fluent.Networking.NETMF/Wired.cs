

namespace JREndean.Fluent.Networking.NETMF
{
    using System;

    using GT = Gadgeteer;
    using GHI.Networking;

    using Microsoft.SPOT.Hardware;

    public class Wired
    {
        public WiredNetwork ENC28(int socketNumber)
        {
            // TODO: validate the inputs
            
            var socket = GT.Socket.GetSocket(socketNumber, true, null, null);

            if (!socket.SupportsType('S'))
            {
                throw new NotSupportedException("That socket does not support the required socket type");
            }

            return this.ENC28(socket.SPIModule, socket.CpuPins[6], socket.CpuPins[3], socket.CpuPins[4]);
        }

        public WiredNetwork ENC28(SPI.SPI_module spi, Cpu.Pin chipSelect, Cpu.Pin externalInterrupt, Cpu.Pin reset)
        {
            // TODO: validate the inputs

            return new WiredNetwork(new EthernetENC28J60(spi, chipSelect, externalInterrupt, reset));
        }

        public WiredNetwork BuiltIn()
        {
            // TODO: validate that built in exists?

            return new WiredNetwork(new EthernetBuiltIn());
        }
    }
}
