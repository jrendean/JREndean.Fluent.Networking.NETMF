

namespace JREndean.Fluent.Networking.NETMF
{
    public static class Network
    {
        public static Wired wired = new Wired();
        public static Wireless wireless = new Wireless();

        /// <summary>Gets the wired network interface</summary>
        public static Wired Wired
        {
            get { return wired; }
        }

        /// <summary>Gets the wireless network interface</summary>
        public static Wireless Wireless
        {
            get { return wireless; }
        }
    }
}
