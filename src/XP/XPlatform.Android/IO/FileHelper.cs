using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency( typeof( XPlatform.IO.FileHelper ) )]
namespace XPlatform.IO {
  public class FileHelper : IFileHelper {
    public string GetLocalFilePath(string user_id, string filename) {
      string path = Environment.GetFolderPath( Environment.SpecialFolder.Personal );
      return Path.Combine( path, user_id, filename );
    }
  }

}