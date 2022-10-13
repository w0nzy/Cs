// Decompiled with JetBrains decompiler
// Type: Shelf.Droid.Helpers.MobilePrinter.Sewoo
// Assembly: Shelf.Android, Version=2021.3.19.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B8786C0-7837-439B-8375-6172DDE07210
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.Android.dll

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using Com.Sewoo.Jpos.Command;
using Com.Sewoo.Jpos.Printer;
using Com.Sewoo.Port.Android;
using Com.Sewoo.Request.Android;
using Java.Lang;
using Shelf.Models;
using System;
using System.Linq;

namespace Shelf.Droid.Helpers.MobilePrinter
{
  public class Sewoo
  {
    public Thread bluetoothThread;
    public BluetoothPort sewooBluetoohPort;
    public Shelf.Droid.Helpers.MobilePrinter.Sewoo.SewooBroadcastReceiver sewooBroadcast;
    private static sbyte ESC = 27;
    private static sbyte LF = 10;
    private static int code128 = 111;
    private static int alignCenter = 1;
    private static int textBelow = 2;

    public ReturnModel PrintText(MobilePrinterProp printer)
    {
      ReturnModel returnModel1 = new ReturnModel();
      ESCPOSPrinter escposPrinter = new ESCPOSPrinter();
      escposPrinter.SetCharSet("iso-8859-9");
      if (this.sewooBluetoohPort == null)
      {
        ReturnModel returnModel2 = this.Connect(printer);
        if (!returnModel2.Success)
          return new ReturnModel()
          {
            Success = false,
            ErrorMessage = "Cihaza bağlanılamadı:" + printer.PrinterName + "." + returnModel2.ErrorMessage
          };
      }
      int num;
      try
      {
        num = escposPrinter.PrinterStatus();
      }
      catch (System.Exception ex)
      {
        return new ReturnModel()
        {
          Success = false,
          ErrorMessage = "Yazıcı bağlı değil" + ex.ToString()
        };
      }
      if (num != 0)
      {
        returnModel1.Success = false;
        returnModel1.ErrorMessage = "Yazıcı Hazır Değil.Hata Kodu:" + num.ToString();
        return returnModel1;
      }
      escposPrinter.PrintString(printer.PrintText);
      returnModel1.Success = true;
      return returnModel1;
    }

    public ReturnModel PrintImage(MobilePrinterProp printer)
    {
      ReturnModel returnModel1 = new ReturnModel();
      ESCPOSPrinter escposPrinter = new ESCPOSPrinter();
      escposPrinter.SetCharSet("iso-8859-9");
      if (this.sewooBluetoohPort == null)
      {
        ReturnModel returnModel2 = this.Connect(printer);
        if (!returnModel2.Success)
          return new ReturnModel()
          {
            Success = false,
            ErrorMessage = "Cihaza bağlanılamadı:" + printer.PrinterName + "." + returnModel2.ErrorMessage
          };
      }
      int num;
      try
      {
        num = escposPrinter.PrinterStatus();
      }
      catch (System.Exception ex)
      {
        return new ReturnModel()
        {
          Success = false,
          ErrorMessage = "Yazıcı bağlı değil" + ex.ToString()
        };
      }
      if (num != 0)
      {
        returnModel1.Success = false;
        returnModel1.ErrorMessage = "Yazıcı Hazır Değil.Hata Kodu:" + num.ToString();
        return returnModel1;
      }
      Bitmap bitmap = BitmapFactory.DecodeByteArray(printer.ImageFile, 0, printer.ImageFile.Length, new BitmapFactory.Options()
      {
        InJustDecodeBounds = false,
        InSampleSize = 1,
        InPreferredConfig = Bitmap.Config.Rgb565
      });
      CPCLPrinter cpclPrinter = new CPCLPrinter();
      ((CPCL) cpclPrinter).SetForm(0, 0, 0, bitmap.Height + 10, bitmap.Width, 1);
      ((CPCL) cpclPrinter).SetMedia(2);
      ((CPCL) cpclPrinter).PrintBitmap(bitmap, 0, 0);
      ((CPCL) cpclPrinter).PrintForm();
      returnModel1.Success = true;
      return returnModel1;
    }

    public ReturnModel Connect(MobilePrinterProp printer)
    {
      ReturnModel returnModel = new ReturnModel();
      try
      {
        using (BluetoothAdapter defaultAdapter = BluetoothAdapter.DefaultAdapter)
        {
          BluetoothDevice bluetoothDevice = defaultAdapter?.BondedDevices.Where<BluetoothDevice>((Func<BluetoothDevice, bool>) (bd => bd?.Name == printer.PrinterName)).FirstOrDefault<BluetoothDevice>();
          GlobalMobAnd.sewooPrint.sewooBluetoohPort = new BluetoothPort();
          GlobalMobAnd.sewooPrint.sewooBluetoohPort.SetMacFilter(false);
          GlobalMobAnd.sewooPrint.sewooBluetoohPort.Connect(bluetoothDevice, true);
          RequestHandler target = new RequestHandler();
          GlobalMobAnd.sewooPrint.bluetoothThread = new Thread((IRunnable) target);
          GlobalMobAnd.sewooPrint.bluetoothThread.Start();
          GlobalMobAnd.sewooPrint.sewooBroadcast = new Shelf.Droid.Helpers.MobilePrinter.Sewoo.SewooBroadcastReceiver();
          Application.Context.RegisterReceiver((BroadcastReceiver) GlobalMobAnd.sewooPrint.sewooBroadcast, new IntentFilter("android.bluetooth.device.action.ACL_CONNECTED"));
          Application.Context.RegisterReceiver((BroadcastReceiver) GlobalMobAnd.sewooPrint.sewooBroadcast, new IntentFilter("android.bluetooth.device.action.ACL_DISCONNECTED"));
          returnModel.Success = true;
        }
      }
      catch (System.Exception ex)
      {
        returnModel.ErrorMessage = ex.ToString();
        returnModel.Success = false;
      }
      return returnModel;
    }

    [BroadcastReceiver]
    public class SewooBroadcastReceiver : BroadcastReceiver
    {
      public override void OnReceive(Context context, Intent intent)
      {
        if ("android.bluetooth.device.action.ACL_CONNECTED".Equals(intent.Action))
        {
          Toast.MakeText(Application.Context, "BlueTooth Connect", ToastLength.Short).Show();
        }
        else
        {
          if (GlobalMobAnd.sewooPrint.sewooBluetoohPort.IsConnected)
          {
            GlobalMobAnd.sewooPrint.sewooBluetoohPort.Disconnect();
            GlobalMobAnd.sewooPrint.sewooBluetoohPort = (BluetoothPort) null;
            Application.Context.UnregisterReceiver((BroadcastReceiver) GlobalMobAnd.sewooPrint.sewooBroadcast);
          }
          Toast.MakeText(Application.Context, "BlueTooth Disconnect", ToastLength.Short).Show();
          if (GlobalMobAnd.sewooPrint.bluetoothThread == null || !GlobalMobAnd.sewooPrint.bluetoothThread.IsAlive)
            return;
          GlobalMobAnd.sewooPrint.bluetoothThread.Interrupt();
          GlobalMobAnd.sewooPrint.bluetoothThread = (Thread) null;
        }
      }
    }
  }
}
