// Decompiled with JetBrains decompiler
// Type: NativeHelper
// Assembly: Shelf.Android, Version=2021.3.19.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B8786C0-7837-439B-8375-6172DDE07210
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.Android.dll

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Print;
using Android.Provider;
using Android.Support.V4.Print;
using Android.Widget;
using PDFPrintingSample.Droid;
using Shelf.Droid;
using Shelf.Droid.Helpers;
using Shelf.Droid.Helpers.MobilePrinter;
using Shelf.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Essentials;

public class NativeHelper : INativeHelper
{
  public void RestartApp()
  {
    try
    {
      new Intent(Application.Context, typeof (MainActivity)).AddFlags(ActivityFlags.ClearTop);
    }
    catch (Exception ex)
    {
    }
  }

  public void CloseApp() => Process.KillProcess(Process.MyPid());

  public string GetSerialNumber() => Build.Serial;

  public string GetVersion()
  {
    try
    {
      Context context = Application.Context;
      return context.PackageManager.GetPackageInfo(context.PackageName, (PackageInfoFlags) 0).VersionName;
    }
    catch (Exception ex)
    {
      return "";
    }
  }

  public ReturnModel ConnectPrinter(MobilePrinterProp printer)
  {
    ReturnModel returnModel = new ReturnModel();
    if (GlobalMobAnd.sewooPrint == null)
      GlobalMobAnd.sewooPrint = new Sewoo();
    if (GlobalMobAnd.bixolonPrint == null)
      GlobalMobAnd.bixolonPrint = new Bixolon();
    switch (printer.BrandID)
    {
      case 1:
        returnModel = GlobalMobAnd.bixolonPrint.Connect(printer);
        break;
      case 2:
        returnModel = GlobalMobAnd.sewooPrint.Connect(printer);
        break;
    }
    return returnModel;
  }

  public ReturnModel Print(MobilePrinterProp printer)
  {
    ReturnModel returnModel = new ReturnModel();
    if (GlobalMobAnd.sewooPrint == null)
      GlobalMobAnd.sewooPrint = new Sewoo();
    if (GlobalMobAnd.bixolonPrint == null)
      GlobalMobAnd.bixolonPrint = new Bixolon();
    if (string.IsNullOrEmpty(printer.PrintText))
      return new ReturnModel()
      {
        Success = false,
        ErrorMessage = "Yazdırılacak metin yok"
      };
    switch (printer.BrandID)
    {
      case 1:
        returnModel = !printer.IsImagePrint ? GlobalMobAnd.bixolonPrint.PrintText(printer) : GlobalMobAnd.bixolonPrint.PrintBitMap(printer);
        break;
      case 2:
        returnModel = !printer.IsImagePrint ? GlobalMobAnd.sewooPrint.PrintText(printer) : GlobalMobAnd.sewooPrint.PrintImage(printer);
        break;
    }
    return returnModel;
  }

  public IList<string> GetDeviceList()
  {
    using (BluetoothAdapter defaultAdapter = BluetoothAdapter.DefaultAdapter)
      return defaultAdapter != null ? (IList<string>) defaultAdapter.BondedDevices.Select<BluetoothDevice, string>((Func<BluetoothDevice, string>) (i => i.Name)).ToList<string>() : (IList<string>) null;
  }

  public void ShowMessage(string message) => Toast.MakeText(Application.Context, message, ToastLength.Long).Show();

  public string GetDeviceNumber()
  {
    string deviceNumber = Build.Serial;
    if (string.IsNullOrEmpty(deviceNumber) || deviceNumber == "unknown" || deviceNumber == "bilinmeyen")
      deviceNumber = this.GetAndroidID();
    return deviceNumber;
  }

  private string GetAndroidID()
  {
    try
    {
      return Settings.Secure.GetString(Application.Context.ContentResolver, "android_id");
    }
    catch (Exception ex)
    {
      return (string) null;
    }
  }

  public ReturnModel PrintPdf(Stream inputStream, string fileName)
  {
    ReturnModel returnModel = new ReturnModel();
    try
    {
      if (inputStream.CanSeek)
        inputStream.Position = 0L;
      string path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), fileName);
      using (FileStream destination = File.OpenWrite(path))
        inputStream.CopyTo((Stream) destination);
      string fileDesc = path;
      PrintManager systemService = (PrintManager) Platform.CurrentActivity.GetSystemService("print");
      PrintDocumentAdapter printDocumentAdapter = (PrintDocumentAdapter) new CustomPrintDocumentAdapter(fileDesc);
      string printJobName = fileName;
      PrintDocumentAdapter documentAdapter = printDocumentAdapter;
      systemService.Print(printJobName, documentAdapter, (PrintAttributes) null);
      returnModel.Success = true;
    }
    catch (Exception ex)
    {
      returnModel.Success = false;
      returnModel.ErrorMessage = ex.ToString();
    }
    return returnModel;
  }

  public ReturnModel PrintImage(Stream img, string fileName)
  {
    ReturnModel returnModel = new ReturnModel();
    PrintHelper printHelper = new PrintHelper(Xamarin.Forms.Forms.Context);
    printHelper.ScaleMode = 1;
    Bitmap bitmap = BitmapFactory.DecodeStream(img);
    printHelper.PrintBitmap(fileName, bitmap);
    returnModel.Success = true;
    return returnModel;
  }
}
