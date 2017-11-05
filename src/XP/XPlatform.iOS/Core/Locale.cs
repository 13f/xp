using Foundation;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: Xamarin.Forms.Dependency( typeof( XPlatform.Core.ILocale ) )]
namespace XPlatform.Core {
  public class Locale {
    public string GetDefaultLanguage() {
      var lang = "en-us";
      if (NSLocale.PreferredLanguages.Length > 0) {
        var pref = NSLocale.PreferredLanguages[0];

        lang = iOSToDotnetLanguage( pref );
      }
      return lang;
    }

    //public string GetSupportedLanguage() {
    //  return null;
    //}

  }

}