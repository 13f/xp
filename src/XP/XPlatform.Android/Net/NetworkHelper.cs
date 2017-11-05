using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Net;
using Java.Net;

[assembly: Xamarin.Forms.Dependency( typeof( XPlatform.Net.NetworkHelper ) )]
namespace XPlatform.Net {
  public class NetworkHelper : INetworkHelper {
    public static string HostName = "http://www.bing.com";
    readonly ConnectivityManager _connectivityManager;
    readonly Context _context;
    readonly int _connectionTimeOutInMillisec = 3000;

    public NetworkHelper(Context context, string host = "http://www.bing.com") {
      this._context = context;
      HostName = host;
      _connectivityManager = (ConnectivityManager)_context.GetSystemService( Context.ConnectivityService );
    }

    #region IReachability implementation

    public bool IsHostReachable(string host = null) {
      host = host ?? HostName;
      var isConnected = false;
      var activeConnection = _connectivityManager.ActiveNetworkInfo;
      if (( activeConnection != null ) && activeConnection.IsConnected) {
        try {
          var task = Task.Factory.StartNew( () => {
            URL url = new URL( HostName );
            HttpURLConnection urlc = (HttpURLConnection)url.OpenConnection();
            urlc.SetRequestProperty( "User-Agent", "Android Application" );
            urlc.SetRequestProperty( "Connection", "close" );
            urlc.ConnectTimeout = _connectionTimeOutInMillisec;
            urlc.Connect();
            isConnected = ( urlc.ResponseCode == HttpStatus.Ok );
            isConnected = true;
          } );
          task.Wait();
        }
        catch (Exception e) {
          System.Diagnostics.Trace.WriteLine( "Connectivity issue: " + e.ToString() );
        }
      }
      return isConnected;
    }

    //public NetworkStatus RemoteHostStatus() {
    //  var networkStatus = InternetConnectionStatus();
    //  if (networkStatus == NetworkStatus.Disconnected)
    //    return NetworkStatus.Disconnected;

    //  return networkStatus;
    //}

    public NetworkStatus InternetConnectionStatus() {
      if (_connectivityManager.ActiveNetworkInfo == null)
        return NetworkStatus.Disconnected;
      if (!_connectivityManager.ActiveNetworkInfo.IsConnected)
        return NetworkStatus.Disconnected;

      if (_connectivityManager.ActiveNetworkInfo.Type == ConnectivityType.Wifi && IsHostReachable())
        return NetworkStatus.ConnectedViaWifi;
      if (_connectivityManager.ActiveNetworkInfo.Type == ConnectivityType.Mobile && IsHostReachable())
        return NetworkStatus.ConnectedViaMobile;

      return NetworkStatus.Disconnected;
    }

    public NetworkStatus LocalWifiConnectionStatus() {
      if (_connectivityManager.ActiveNetworkInfo == null)
        return NetworkStatus.Disconnected;

      if (_connectivityManager.ActiveNetworkInfo.Type == ConnectivityType.Wifi && IsHostReachable())
        return NetworkStatus.ConnectedViaWifi;

      return NetworkStatus.Disconnected;
    }

    public string GetClientIP() {
      string ip = System.Net.NetworkInformation.NetworkInterface
               .GetAllNetworkInterfaces()
               .Where( x => x.Name.Equals( "en0" ) )
               .Select( n => n.GetIPProperties().UnicastAddresses.First().Address )
               .Where( x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork )
               .First()
               //.Address.ToString();  // Address已弃用
               .ToString();
      return ip;
    }

    #endregion
  }

}