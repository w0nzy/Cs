// Decompiled with JetBrains decompiler
// Type: Shelf.Droid.MainActivity
// Assembly: Shelf.Android, Version=2021.3.19.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B8786C0-7837-439B-8375-6172DDE07210
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.Android.dll

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.KeyboardHelper.Platform.Droid;

namespace Shelf.Droid
{
  [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, Icon = "@drawable/icon", Label = "Raf Takip", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/MainTheme")]
  public class MainActivity : FormsAppCompatActivity
  {
    protected virtual void OnCreate(Bundle bundle)
    {
      FormsAppCompatActivity.TabLayoutResource = 2131427411;
      FormsAppCompatActivity.ToolbarResource = 2131427412;
      base.OnCreate(bundle);
      Effects.Init((Activity) this);
      Rg.Plugins.Popup.Popup.Init((Context) this, bundle);
      Xam.Plugin.AutoUpdate.Droid.AutoUpdate.Init((Context) this, "com.companyname.shelf");
      Xamarin.Forms.Forms.Init((Context) this, bundle);
      Xamarin.Essentials.Platform.Init((Activity) this, bundle);
      this.LoadApplication((Application) new Shelf.App());
    }
  }
}
