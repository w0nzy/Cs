// Decompiled with JetBrains decompiler
// Type: Xam.Plugin.AutoUpdate.Droid.AutoUpdate
// Assembly: Shelf.Android, Version=2021.3.19.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B8786C0-7837-439B-8375-6172DDE07210
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.Android.dll

using Android.Content;

namespace Xam.Plugin.AutoUpdate.Droid
{
  public static class AutoUpdate
  {
    public static Context Context { get; set; }

    public static string Authority { get; set; }

    public static void Init(Context activity, string fileProviderAuthority)
    {
      Xam.Plugin.AutoUpdate.Droid.AutoUpdate.Context = activity;
      Xam.Plugin.AutoUpdate.Droid.AutoUpdate.Authority = fileProviderAuthority;
    }
  }
}
