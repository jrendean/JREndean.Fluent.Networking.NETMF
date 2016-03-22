//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JREndean.Fluent.Networking.NETMF {
    using Gadgeteer;
    using GTM = Gadgeteer.Modules;
    
    
    public partial class Program : Gadgeteer.Program {
        
        /// <summary>The Ethernet ENC28 module using socket 11 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.EthernetENC28 ethernetENC28;
        
        /// <summary>The CellularRadio module (not connected).</summary>
        private Gadgeteer.Modules.GHIElectronics.CellularRadio cellularRadio;
        
        /// <summary>The WiFi RS21 module using socket 1 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.WiFiRS21 wifiRS21;
        
        /// <summary>This property provides access to the Mainboard API. This is normally not necessary for an end user program.</summary>
        protected new static GHIElectronics.Gadgeteer.FEZRaptor Mainboard {
            get {
                return ((GHIElectronics.Gadgeteer.FEZRaptor)(Gadgeteer.Program.Mainboard));
            }
            set {
                Gadgeteer.Program.Mainboard = value;
            }
        }
        
        /// <summary>This method runs automatically when the device is powered, and calls ProgramStarted.</summary>
        public static void Main() {
            // Important to initialize the Mainboard first
            Program.Mainboard = new GHIElectronics.Gadgeteer.FEZRaptor();
            Program p = new Program();
            p.InitializeModules();
            p.ProgramStarted();
            // Starts Dispatcher
            p.Run();
        }
        
        private void InitializeModules() {
            this.ethernetENC28 = new GTM.GHIElectronics.EthernetENC28(11);
            Microsoft.SPOT.Debug.Print("The module \'cellularRadio\' was not connected in the designer and will be null.");
            this.wifiRS21 = new GTM.GHIElectronics.WiFiRS21(1);
        }
    }
}
