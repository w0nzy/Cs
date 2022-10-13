// Decompiled with JetBrains decompiler
// Type: Shelf.Droid.Helpers.MobilePrinter.MyHandler
// Assembly: Shelf.Android, Version=2021.3.19.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B8786C0-7837-439B-8375-6172DDE07210
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.Android.dll

using Android.OS;
using Java.Lang;

namespace Shelf.Droid.Helpers.MobilePrinter
{
  public class MyHandler : Handler
  {
    public override void HandleMessage(Message msg)
    {
      base.HandleMessage(msg);
      if (msg.What != 7)
        return;
      Object @object = msg.Obj;
    }
  }
}
