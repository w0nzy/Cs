// Decompiled with JetBrains decompiler
// Type: Xam.Plugin.AutoUpdate.Droid.PlayStoreOpener
// Assembly: Shelf.Android, Version=2021.3.19.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B8786C0-7837-439B-8375-6172DDE07210
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.Android.dll

using Android.Content;
using Android.Content.PM;
using System;
using System.Collections.Generic;
using Xam.Plugin.AutoUpdate.Services;

namespace Xam.Plugin.AutoUpdate.Droid
{
  public class PlayStoreOpener : IStoreOpener
  {
    public void OpenStore()
    {
      Intent intent = new Intent("android.intent.action.VIEW", Android.Net.Uri.Parse("market://details?id=" + Xam.Plugin.AutoUpdate.Droid.AutoUpdate.Context.PackageName));
      bool flag = false;
      foreach (ResolveInfo queryIntentActivity in (IEnumerable<ResolveInfo>) Xam.Plugin.AutoUpdate.Droid.AutoUpdate.Context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.Activities))
      {
        if (queryIntentActivity.ActivityInfo.ApplicationInfo.PackageName == "com.android.vending")
        {
          ActivityInfo activityInfo = queryIntentActivity.ActivityInfo;
          ComponentName component = new ComponentName(activityInfo.ApplicationInfo.PackageName, activityInfo.Name);
          intent.AddFlags(ActivityFlags.NewTask);
          intent.AddFlags(ActivityFlags.ResetTaskIfNeeded);
          intent.AddFlags(ActivityFlags.ClearTop);
          intent.SetComponent(component);
          Xam.Plugin.AutoUpdate.Droid.AutoUpdate.Context.StartActivity(intent);
          flag = true;
          break;
        }
      }
      if (!flag)
        throw new Exception("Could not find google play store app.");
    }
  }
}
