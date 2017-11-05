using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

[assembly: Xamarin.Forms.Dependency( typeof( XPlatform.IO.FileHelper ) )]
namespace XPlatform.IO {
  public class FileHelper : IFileHelper {
    public string GetLocalFilePath(string user_id, string filename) {
      return Path.Combine( ApplicationData.Current.LocalFolder.Path, user_id, filename );
    }
  }

}
