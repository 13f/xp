using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;

[assembly: Xamarin.Forms.Dependency( typeof( XPlatform.Net.NetworkHelper ) )]
namespace XPlatform.Net {
  public class NetworkHelper : INetworkHelper {

    private static string HostName = "www.bing.com";
    readonly int _connectionTimeOutInMillisec = 3000;

    public NetworkHelper(string hostName = "www.bing.com") {
      HostName = hostName;
    }
    
    public NetworkStatus InternetConnectionStatus() {
      var connProfile = NetworkInformation.GetInternetConnectionProfile();
      if (connProfile == null)
        return NetworkStatus.Disconnected;
      else if (connProfile.IsWlanConnectionProfile)
        return NetworkStatus.ConnectedViaWifi;
      else if (connProfile.IsWwanConnectionProfile)
        return NetworkStatus.ConnectedViaMobile;

      return NetworkStatus.Disconnected;
    }

    public NetworkStatus LocalWifiConnectionStatus() {
      var connProfile = NetworkInformation.GetInternetConnectionProfile();
      if (connProfile == null)
        return NetworkStatus.Disconnected;

      return connProfile.IsWlanConnectionProfile == true ? NetworkStatus.ConnectedViaWifi : NetworkStatus.Disconnected;
    }


    //public NetworkStatus RemoteHostStatus() {

    //  return ( flags & NetworkReachabilityFlags.IsWWAN ) != 0 ? NetworkStatus.ConnectedViaMobile : NetworkStatus.ConnectedViaWifi;
    //}

    // Is the host reachable with the current network configuration
    public async Task<bool> IsHostReachable(string host) {
      if (string.IsNullOrEmpty( host ))
        throw new ArgumentNullException( "host" );

      var ics = InternetConnectionStatus();
      if (ics == NetworkStatus.Disconnected || ics == NetworkStatus.Unknown)
        return false;

      try {
        var serverHost = new HostName( host );
        using (var client = new StreamSocket()) {
          var cancellationTokenSource = new CancellationTokenSource();
          cancellationTokenSource.CancelAfter( _connectionTimeOutInMillisec );

          await client.ConnectAsync( serverHost, "http" ).AsTask( cancellationTokenSource.Token );
          return true;
        }
      }
      catch (Exception ex) {
        if(System.Diagnostics.Debugger.IsAttached)
          System.Diagnostics.Debug.WriteLine( "Unable to reach: " + host + " Error: " + ex );
        return false;
      }
    }

    public string GetClientIP() {
      // Ref: http://developer.nokia.com/Community/Wiki/How_to_get_the_device_IP_addresses_on_Windows_Phone
      List<string> ipAddresses = new List<string>();
      var hostnames = NetworkInformation.GetHostNames();

      foreach (var hn in hostnames) {
        //IanaInterfaceType == 71 => Wifi
        //IanaInterfaceType == 6 => Ethernet (Emulator)
        if (hn.IPInformation != null &&
            ( hn.IPInformation.NetworkAdapter.IanaInterfaceType == 71
            || hn.IPInformation.NetworkAdapter.IanaInterfaceType == 6 )) {
          string ipAddress = hn.DisplayName;
          ipAddresses.Add( ipAddress );
        }
      }

      if (ipAddresses.Count < 1) {
        // 从此处获取：http://www.chuci.info/tool/client-ip
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        var t = client.GetStringAsync( "http://www.chuci.info/tool/client-ip" );
        t.Wait();
        return t.Result;
      }
      else if (ipAddresses.Count == 1) {
        return ipAddresses[0];
      }
      else {
        //if multiple suitable address were found use the last one
        //(regularly the external interface of an emulated device)
        return ipAddresses[ipAddresses.Count - 1];
      }
    }

  }

}
