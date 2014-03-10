using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace YouMaps
{
    public static class InternetConnectionChange
    {
        public delegate void InternetConnectionChangedHandler(object sender, InternetConnectionChangedEventArgs args);
 
        public static event InternetConnectionChangedHandler InternetConnectionChanged;

        static InternetConnectionChange()
        {
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }
 
        private static void NetworkInformation_NetworkStatusChanged(object sender)
        {
            var arg = new InternetConnectionChangedEventArgs
                          {IsConnected = (NetworkInformation.GetInternetConnectionProfile() != null)};
 
            if (InternetConnectionChanged != null)
                InternetConnectionChanged(null, arg);
        }
 
        public static bool IsConnected
        {
            get
            {
                return NetworkInformation.GetInternetConnectionProfile() != null;
            }
        }
    }
 
    public class InternetConnectionChangedEventArgs : EventArgs
    {
        public bool IsConnected { get; set; }
    }
 
    
}
