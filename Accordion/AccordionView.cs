// Decompiled with JetBrains decompiler
// Type: Accordion.AccordionView
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Accordion
{
  public class AccordionView : ScrollView
  {
    private StackLayout _layout = new StackLayout()
    {
      Spacing = 1.0
    };
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof (ItemsSource), typeof (IList), typeof (AccordionSectionView), (object) null, (BindingMode) 2, (BindableProperty.ValidateValueDelegate) null, new BindableProperty.BindingPropertyChangedDelegate((object) null, __methodptr(PopulateList)), (BindableProperty.BindingPropertyChangingDelegate) null, (BindableProperty.CoerceValueDelegate) null, (BindableProperty.CreateDefaultValueDelegate) null);

    public DataTemplate Template { get; set; }

    public DataTemplate SubTemplate { get; set; }

    public IList ItemsSource
    {
      get => (IList) ((BindableObject) this).GetValue(AccordionView.ItemsSourceProperty);
      set => ((BindableObject) this).SetValue(AccordionView.ItemsSourceProperty, (object) value);
    }

    public AccordionView(DataTemplate itemTemplate)
    {
      AccordionView parent = this;
      this.SubTemplate = itemTemplate;
      this.Template = new DataTemplate((Func<object>) (() => (object) new AccordionSectionView(itemTemplate, (ScrollView) parent)));
      this.Content = (View) this._layout;
    }

    private void PopulateList()
    {
      ((ICollection<View>) ((Layout<View>) this._layout).Children).Clear();
      foreach (object obj in (IEnumerable) this.ItemsSource)
      {
        View content = (View) ((ElementTemplate) this.Template).CreateContent();
        ((BindableObject) content).BindingContext = obj;
        ((ICollection<View>) ((Layout<View>) this._layout).Children).Add(content);
      }
    }

    private static void PopulateList(BindableObject bindable, object oldValue, object newValue)
    {
      if (oldValue == newValue)
        return;
      ((AccordionView) bindable).PopulateList();
    }
  }
}
