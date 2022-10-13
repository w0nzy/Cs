// Decompiled with JetBrains decompiler
// Type: AndroidAppNamespace.Effects.AndroidLongPressedEffect
// Assembly: Shelf.Android, Version=2021.3.19.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B8786C0-7837-439B-8375-6172DDE07210
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.Android.dll

using Android.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AndroidAppNamespace.Effects
{
  public class AndroidLongPressedEffect : PlatformEffect
  {
    private bool _attached;

    public static void Initialize()
    {
    }

    protected virtual void OnAttached()
    {
      if (this._attached)
        return;
      if (((PlatformEffect<ViewGroup, View>) this).Control != null)
      {
        ((PlatformEffect<ViewGroup, View>) this).Control.LongClickable = true;
        ((PlatformEffect<ViewGroup, View>) this).Control.LongClick += new EventHandler<View.LongClickEventArgs>(this.Control_LongClick);
      }
      else
      {
        ((PlatformEffect<ViewGroup, View>) this).Container.LongClickable = true;
        ((PlatformEffect<ViewGroup, View>) this).Container.LongClick += new EventHandler<View.LongClickEventArgs>(this.Control_LongClick);
      }
      this._attached = true;
    }

    private void Control_LongClick(object sender, View.LongClickEventArgs e)
    {
      Console.WriteLine("Invoking long click command");
      LongPressedEffect.GetCommand((BindableObject) ((Effect) this).Element)?.Execute(LongPressedEffect.GetCommandParameter((BindableObject) ((Effect) this).Element));
    }

    protected virtual void OnDetached()
    {
      if (!this._attached)
        return;
      if (((PlatformEffect<ViewGroup, View>) this).Control != null)
      {
        ((PlatformEffect<ViewGroup, View>) this).Control.LongClickable = true;
        ((PlatformEffect<ViewGroup, View>) this).Control.LongClick -= new EventHandler<View.LongClickEventArgs>(this.Control_LongClick);
      }
      else
      {
        ((PlatformEffect<ViewGroup, View>) this).Container.LongClickable = true;
        ((PlatformEffect<ViewGroup, View>) this).Container.LongClick -= new EventHandler<View.LongClickEventArgs>(this.Control_LongClick);
      }
      this._attached = false;
    }
  }
}
