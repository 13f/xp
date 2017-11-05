using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlatform.IO {
  public interface IFileHelper {
    string GetLocalFilePath(string user_id, string filename);

  }

}
