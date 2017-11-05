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

[assembly: Xamarin.Forms.Dependency( typeof( XPlatform.Core.ILocale ) )]
namespace XPlatform.Core {
  public class Locale {
    public string GetDefaultLanguage() {
      var androidLocale = Java.Util.Locale.Default;
      string lang = androidLocale.ToString().Replace( "_", "-" );
      return lang;
    }

    //public string GetSupportedLanguage() {
    //  return null;
    //}

  }

}