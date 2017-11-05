using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency( typeof( XPlatform.Core.ILocale ) )]
namespace XPlatform.Core {
  public class Locale {
    public string GetDefaultLanguage() {
      string lang = Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride;
      if (string.IsNullOrWhiteSpace( lang ))
        lang = Windows.Globalization.ApplicationLanguages.Languages.FirstOrDefault();
      return lang;
    }

    //public string GetSupportedLanguage() {
    //  return null;
    //}

  }

}