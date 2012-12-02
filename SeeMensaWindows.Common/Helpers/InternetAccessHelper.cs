using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace SeeMensaWindows.Common.Helpers
{
    public static class InternetAccessHelper
    {
        /// <summary>
        /// Checks the internet connection.
        /// </summary>
        /// <returns>True, if internet is available.</returns>
        public static bool IsInternet()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }
    }
}
