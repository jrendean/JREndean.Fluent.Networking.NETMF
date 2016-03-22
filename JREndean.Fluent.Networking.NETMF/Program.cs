using Gadgeteer.Modules.GHIElectronics;


namespace JREndean.Fluent.Networking.NETMF
{
    using System.Net;
    using System.Text;
    using System.Threading;

    using Gadgeteer.Modules.GHIElectronics;

    using Microsoft.SPOT;
    using Microsoft.SPOT.Hardware;
    using Microsoft.SPOT.Net.NetworkInformation;

    public partial class Program
    {
        // This method is run when the mainboard is powered up or reset.   
        private void ProgramStarted()
        {
            ////
            //// WIRED
            ////

            //// use a ENC28 connected on socket 11
            //// use dhcp
            //Network.Wired.ENC28(11).UseDhcp();
            //// use static ip, subnet, and gateway with optional dns list
            //Network.Wired.ENC28(11).UseStatic("[ip]", "[subnet]", "[gateway]", new[] { "[dns]", "[dns]" });
            
            //// use an ENC28 specifying the SPI connection and other pins
            //// dhcp
            //Network.Wired.ENC28(SPI.SPI_module.SPI1, Cpu.Pin.GPIO_Pin1, Cpu.Pin.GPIO_Pin2, Cpu.Pin.GPIO_Pin3).UseDhcp();
            //// or static
            //Network.Wired.ENC28(SPI.SPI_module.SPI1, Cpu.Pin.GPIO_Pin1, Cpu.Pin.GPIO_Pin2, Cpu.Pin.GPIO_Pin3).UseStatic("[ip]", "[subnet]", "[gateway]", new[] { "[dns]", "[dns]" });
            
            //// use the builtin network
            //// dhcp
            //Network.Wired.BuiltIn().UseDhcp();
            //// or static
            //Network.Wired.BuiltIn().UseStatic("[ip]", "[subnet]", "[gateway]", new[] { "[dns]", "[dns]" });



            ////
            //// WIRELESS
            ////

            //// use a RS9110 wireless adapter on sockect 1
            //// use dhcp and join the wireless network with ssid and password
            //Network.Wireless.RS9110(1).UseDhcp().Join("[ssid]", "[password]");
            //// use static and join the wireless network with ssid and password
            //Network.Wireless.RS9110(1).UseStatic("[ip]", "[subnet]", "[gateway]", new[] { "[dns]", "[dns]" }).Join("[ssid]", "[password]");

            //// use a RS9110 wireless adapter specifying the SPI connection and other pins
            //// use dhcp and join the wireless network
            //// reset pin 4
            //// int pin 3
            //// wol pin 5
            //Network.Wireless.RS9110(SPI.SPI_module.SPI2, Cpu.Pin.GPIO_Pin1, Cpu.Pin.GPIO_Pin2, Cpu.Pin.GPIO_Pin3).UseDhcp().Join("[ssid]", "[password]");
            //// use static and join the wireless network
            //Network.Wireless.RS9110(SPI.SPI_module.SPI2, Cpu.Pin.GPIO_Pin1, Cpu.Pin.GPIO_Pin2, Cpu.Pin.GPIO_Pin3).UseStatic("[ip]", "[subnet]", "[gateway]", new[] { "[dns]", "[dns]" }).Join("[ssid]", "[password]");

            //// use cellular radio module on socket 4
            //// dhcp
            //Network.Wireless.PPP(4, "[apn]").UseDhcp();
            //// or static
            //Network.Wireless.PPP(4, "[apn]").UseStatic("[ip]", "[subnet]", "[gateway]", new[] { "[dns]", "[dns]" });

            //// user cellular radio module on com port 1
            //// dhcp
            //Network.Wireless.PPP("COM2", "[apn]").UseDhcp();
            //// or static
            //Network.Wireless.PPP("COM2", "[apn]").UseStatic("[ip]", "[subnet]", "[gateway]", new[] { "[dns]", "[dns]" });



            //
            //
            //
            //...OnConnection(ipaddress => { Debug.Print("Connected with IP Address: " + ipaddress); });






            // tmobile APN: epc.tmobile.com  but you it’s an LTE Device, use: fast.tmobile.com instead
           
            //cellularRadio.ModuleInitialized += cellularRadio_ModuleInitialized;
            //cellularRadio.PowerOn(30);
            Network.Wireless.PPP(4, "epc.tmobile.com").OnConnection((ip) => { Debug.Print("Connected: " + ip); });
            //cellularRadio.AttachGprs("epc.tmobile.com", "", "");


            using (var req = HttpWebRequest.Create("http://www.ghielectronics.com/") as HttpWebRequest)
            {
                req.KeepAlive = false;
                req.ContentLength = 0;

                using (var res = req.GetResponse() as HttpWebResponse)
                {
                    using (var stream = res.GetResponseStream())
                    {
                        int read = 0, total = 0;
                        var buffer = new byte[1024];

                        do
                        {
                            read = stream.Read(buffer, 0, buffer.Length);
                            total += read;

                            Thread.Sleep(20);
                        } while (read != 0);

                        Debug.Print(new string(Encoding.UTF8.GetChars(buffer)));
                    }
                }
            }

            Debug.Print("Program Started");
        }
    }
}
