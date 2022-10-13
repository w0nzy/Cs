// Decompiled with JetBrains decompiler
// Type: PDFPrintingSample.Droid.CustomPrintDocumentAdapter
// Assembly: Shelf.Android, Version=2021.3.19.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B8786C0-7837-439B-8375-6172DDE07210
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.Android.dll

using Android.OS;
using Android.Print;
using Java.IO;
using System;

namespace PDFPrintingSample.Droid
{
  internal class CustomPrintDocumentAdapter : PrintDocumentAdapter
  {
    internal string FileToPrint { get; set; }

    internal CustomPrintDocumentAdapter(string fileDesc) => this.FileToPrint = fileDesc;

    public override void OnLayout(
      PrintAttributes oldAttributes,
      PrintAttributes newAttributes,
      CancellationSignal cancellationSignal,
      PrintDocumentAdapter.LayoutResultCallback callback,
      Bundle extras)
    {
      if (cancellationSignal.IsCanceled)
      {
        callback.OnLayoutCancelled();
      }
      else
      {
        PrintDocumentInfo info = new PrintDocumentInfo.Builder(this.FileToPrint).SetContentType(PrintContentType.Document).Build();
        callback.OnLayoutFinished(info, true);
      }
    }

    public override void OnWrite(
      PageRange[] pages,
      ParcelFileDescriptor destination,
      CancellationSignal cancellationSignal,
      PrintDocumentAdapter.WriteResultCallback callback)
    {
      InputStream inputStream = (InputStream) null;
      OutputStream outputStream = (OutputStream) null;
      try
      {
        inputStream = (InputStream) new FileInputStream(this.FileToPrint);
        outputStream = (OutputStream) new FileOutputStream(destination.FileDescriptor);
        byte[] b = new byte[1024];
        int len;
        while ((len = inputStream.Read(b)) > 0)
          outputStream.Write(b, 0, len);
        callback.OnWriteFinished(new PageRange[1]
        {
          PageRange.AllPages
        });
      }
      catch (FileNotFoundException ex)
      {
      }
      catch (Exception ex)
      {
      }
      finally
      {
        try
        {
          inputStream.Close();
          outputStream.Close();
        }
        catch (IOException ex)
        {
          ex.PrintStackTrace();
        }
      }
    }
  }
}
