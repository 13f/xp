using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlatform.Net {
  public interface INetworkHelper {
    bool IsHostReachable(string hostName);
    //NetworkStatus RemoteHostStatus();
    NetworkStatus InternetConnectionStatus();
    NetworkStatus LocalWifiConnectionStatus();
    string GetClientIP();
  }

}
