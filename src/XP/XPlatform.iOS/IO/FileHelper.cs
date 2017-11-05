using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency( typeof( XPlatform.IO.FileHelper ) )]
namespace XPlatform.IO {
  public class FileHelper : IFileHelper {
    public string GetLocalFilePath(string user_id, string filename) {
      string docFolder = Environment.GetFolderPath( Environment.SpecialFolder.Personal );
      string libFolder = Path.Combine( docFolder, "..", "Library", user_id );

      if (!Directory.Exists( libFolder )) {
        Directory.CreateDirectory( libFolder );
      }

      return Path.Combine( libFolder, filename );
    }
  }

}