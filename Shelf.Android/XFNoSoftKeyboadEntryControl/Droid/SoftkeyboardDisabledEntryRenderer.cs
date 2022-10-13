// Decompiled with JetBrains decompiler
// Type: XFNoSoftKeyboadEntryControl.Droid.SoftkeyboardDisabledEntryRenderer
// Assembly: Shelf.Android, Version=2021.3.19.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B8786C0-7837-439B-8375-6172DDE07210
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.Android.dll

using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace XFNoSoftKeyboadEntryControl.Droid
{
  public class SoftkeyboardDisabledEntryRenderer : EntryRenderer
  {
    public SoftkeyboardDisabledEntryRenderer(Context context)
      : base(context)
    {
    }

    protected virtual void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
      ((EntryRendererBase<FormsEditText>) this).OnElementChanged(e);
      if (e.NewElement != null)
      {
        // ISSUE: method pointer
        ((BindableObject) e.NewElement).PropertyChanging += new PropertyChangingEventHandler((object) this, __methodptr(OnPropertyChanging));
      }
      if (e.OldElement != null)
      {
        // ISSUE: method pointer
        ((BindableObject) e.OldElement).PropertyChanging -= new PropertyChangingEventHandler((object) this, __methodptr(OnPropertyChanging));
      }
      ((TextView) ((ViewRenderer<Entry, FormsEditText>) this).Control).ShowSoftInputOnFocus = false;
    }

    private void OnPropertyChanging(
      object sender,
      PropertyChangingEventArgs propertyChangingEventArgs)
    {
      if (!(propertyChangingEventArgs.PropertyName == VisualElement.IsFocusedProperty.PropertyName))
        return;
      ((InputMethodManager) ((View) this).Context.GetSystemService("input_method")).HideSoftInputFromWindow(((View) ((ViewRenderer<Entry, FormsEditText>) this).Control).WindowToken, HideSoftInputFlags.None);
    }
  }
}
