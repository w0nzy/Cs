// Decompiled with JetBrains decompiler
// Type: Xam.Plugin.AutoUpdate.Droid.FileOpener
// Assembly: Shelf.Android, Version=2021.3.19.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B8786C0-7837-439B-8375-6172DDE07210
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.Android.dll

using Android.Content;
using Android.OS;
using Android.Support.V4.Content;
using System;
using System.IO;
using Xam.Plugin.AutoUpdate.Services;

namespace Xam.Plugin.AutoUpdate.Droid
{
  public class FileOpener : IFileOpener
  {
    public string OpenFile(byte[] data, string name)
    {
      string str1 = "";
      try
      {
        string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        string str2 = Path.Combine(folderPath, name);
        foreach (string file in Directory.GetFiles(folderPath))
        {
          if (Path.GetExtension(file) == ".apk")
            System.IO.File.Delete(file);
        }
        str1 = "1";
        System.IO.File.WriteAllBytes(str2, data);
        str1 = "2";
        Intent intent = new Intent("android.intent.action.VIEW");
        str1 = "3";
        Java.IO.File file1 = new Java.IO.File(str2);
        if (!file1.Exists())
          return str2 + " dosya bulunamadı";
        Android.Net.Uri data1 = Build.VERSION.SdkInt >= BuildVersionCodes.M ? FileProvider.GetUriForFile(Xam.Plugin.AutoUpdate.Droid.AutoUpdate.Context, Xam.Plugin.AutoUpdate.Droid.AutoUpdate.Authority, file1) : Android.Net.Uri.FromFile(file1);
        str1 = "4";
        intent.SetDataAndType(data1, "application/vnd.android.package-archive");
        str1 = "5";
        intent.SetFlags(ActivityFlags.GrantReadUriPermission);
        str1 = "6";
        Xam.Plugin.AutoUpdate.Droid.AutoUpdate.Context.StartActivity(intent);
      }
      catch (Exception ex)
      {
        return str1 + " Adım " + ex.ToString();
      }
      return "";
    }
  }
}
