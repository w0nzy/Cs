// Decompiled with JetBrains decompiler
// Type: Shelf.Views.SelectItem
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace Shelf.Views
{
  [XamlCompilation]
  [XamlFilePath("Views\\SelectItem.xaml")]
  public class SelectItem : ContentPage
  {
    private object selectedItem;
    private bool IsClose;

    public SelectItem()
    {
      this.InitializeComponent();
      this.IsClose = true;
    }

    public SelectItem(StackLayout stck, string title)
    {
      this.InitializeComponent();
      ((Page) this).Title = title;
      this.Content = (View) stck;
    }

    public SelectItem(ListView cnt, string title, List<ToolbarItem> toolbarItems = null)
    {
      this.InitializeComponent();
      ((Page) this).Title = title;
      this.Content = (View) cnt;
      if (toolbarItems == null)
        return;
      foreach (ToolbarItem toolbarItem in toolbarItems)
        ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(toolbarItem);
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (SelectItem).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/SelectItem.xaml",
        Instance = (object) this
      }))
        this.__InitComponentRuntime();
      else if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(((object) this).GetType()) != null)
      {
        this.__InitComponentRuntime();
      }
      else
      {
        Label label = new Label();
        StackLayout stackLayout = new StackLayout();
        SelectItem selectItem;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (selectItem = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) selectItem, (INameScope) nameScope);
        ((BindableObject) label).SetValue(Label.TextProperty, (object) "Welcome to Xamarin.Forms!");
        ((BindableObject) label).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.CenterAndExpand);
        ((BindableObject) label).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.CenterAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout).Children).Add((View) label);
        VisualDiagnostics.RegisterSourceInfo((object) label, new Uri("Views\\SelectItem.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 7, 14);
        ((BindableObject) selectItem).SetValue(ContentPage.ContentProperty, (object) stackLayout);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout, new Uri("Views\\SelectItem.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 6, 10);
        VisualDiagnostics.RegisterSourceInfo((object) selectItem, new Uri("Views\\SelectItem.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime() => Extensions.LoadFromXaml<SelectItem>(this, typeof (SelectItem));
  }
}
