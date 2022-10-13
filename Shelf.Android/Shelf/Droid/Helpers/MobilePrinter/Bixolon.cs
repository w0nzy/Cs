// Decompiled with JetBrains decompiler
// Type: Shelf.Droid.Helpers.MobilePrinter.Bixolon
// Assembly: Shelf.Android, Version=2021.3.19.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B8786C0-7837-439B-8375-6172DDE07210
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.Android.dll

using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Graphics;
using Android.OS;
using Com.Bixolon.Labelprinter;
using Shelf.Models;
using System;
using System.Linq;
using System.Text;

namespace Shelf.Droid.Helpers.MobilePrinter
{
  public class Bixolon
  {
    public MyHandler backHandler;
    public Message msg;
    public ScanCallback scan;
    public BixolonLabelPrinter mBixolonLabelPrinter;

    public ReturnModel Connect(MobilePrinterProp prop)
    {
      ReturnModel returnModel = new ReturnModel();
      try
      {
        using (BluetoothAdapter defaultAdapter = BluetoothAdapter.DefaultAdapter)
        {
          if (defaultAdapter == null)
            return new ReturnModel()
            {
              Success = false,
              ErrorMessage = "Bluetooth kapalı"
            };
          BluetoothDevice bluetoothDevice = defaultAdapter?.BondedDevices.Where<BluetoothDevice>((Func<BluetoothDevice, bool>) (bd => bd?.Name == prop.PrinterName)).FirstOrDefault<BluetoothDevice>();
          if (bluetoothDevice == null)
            return new ReturnModel()
            {
              Success = false,
              ErrorMessage = prop.PrinterName + " cihaz bulunamadı"
            };
          this.backHandler = new MyHandler();
          GlobalMobAnd.bixolonPrint.mBixolonLabelPrinter = new BixolonLabelPrinter(Application.Context, (Handler) this.backHandler, Looper.MainLooper);
          GlobalMobAnd.bixolonPrint.mBixolonLabelPrinter.Connect(bluetoothDevice.Address);
          returnModel.Success = GlobalMobAnd.bixolonPrint.mBixolonLabelPrinter.IsConnected;
          if (!returnModel.Success)
            returnModel.ErrorMessage = "Cihaza bağlanılamadı";
        }
      }
      catch (Exception ex)
      {
        returnModel.Success = false;
        returnModel.ErrorMessage = ex.ToString();
      }
      return returnModel;
    }

    public ReturnModel PrintText(MobilePrinterProp prop)
    {
      ReturnModel returnModel1 = new ReturnModel();
      try
      {
        if (GlobalMobAnd.bixolonPrint.mBixolonLabelPrinter == null || !GlobalMobAnd.bixolonPrint.mBixolonLabelPrinter.IsConnected)
        {
          ReturnModel returnModel2 = GlobalMobAnd.bixolonPrint.Connect(prop);
          if (!returnModel2.Success)
            return returnModel2;
        }
        this.backHandler = new MyHandler();
        GlobalMobAnd.bixolonPrint.mBixolonLabelPrinter.BeginTransactionPrint();
        GlobalMobAnd.bixolonPrint.mBixolonLabelPrinter.SetCharacterSet(0, 8);
        byte[] bytes = Encoding.GetEncoding("CP857").GetBytes(prop.PrintText);
        GlobalMobAnd.bixolonPrint.mBixolonLabelPrinter.ExecuteDirectIo(bytes, false, 0);
        GlobalMobAnd.bixolonPrint.mBixolonLabelPrinter.EndTransactionPrint();
        returnModel1.Success = true;
      }
      catch (Exception ex)
      {
        returnModel1.Success = false;
        returnModel1.ErrorMessage = ex.ToString();
      }
      return returnModel1;
    }

    public ReturnModel PrintBitMap(MobilePrinterProp prop)
    {
      ReturnModel returnModel1 = new ReturnModel();
      try
      {
        if (GlobalMobAnd.bixolonPrint.mBixolonLabelPrinter == null || !GlobalMobAnd.bixolonPrint.mBixolonLabelPrinter.IsConnected)
        {
          ReturnModel returnModel2 = GlobalMobAnd.bixolonPrint.Connect(prop);
          if (!returnModel2.Success)
            return returnModel2;
        }
        this.backHandler = new MyHandler();
        GlobalMobAnd.bixolonPrint.mBixolonLabelPrinter.BeginTransactionPrint();
        Bitmap bitmap = BitmapFactory.DecodeByteArray(prop.ImageFile, 0, prop.ImageFile.Length, new BitmapFactory.Options()
        {
          InJustDecodeBounds = false,
          InSampleSize = 1,
          InPreferredConfig = Bitmap.Config.Rgb565
        });
        if (bitmap == null)
        {
          returnModel1.ErrorMessage = "Dosya hatalı";
          returnModel1.Success = false;
          return returnModel1;
        }
        GlobalMobAnd.bixolonPrint.mBixolonLabelPrinter.DrawBitmap(bitmap, 0, 0, 500, 50, true);
        GlobalMobAnd.bixolonPrint.mBixolonLabelPrinter.Print(1, 1);
        GlobalMobAnd.bixolonPrint.mBixolonLabelPrinter.EndTransactionPrint();
        returnModel1.Success = true;
      }
      catch (Exception ex)
      {
        returnModel1.Success = false;
        returnModel1.ErrorMessage = ex.ToString();
      }
      return returnModel1;
    }
  }
}
