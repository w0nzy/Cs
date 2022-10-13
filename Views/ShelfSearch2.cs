// Decompiled with JetBrains decompiler
// Type: Shelf.Views.ShelfSearch2
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using Rg.Plugins.Popup.Extensions;
using Shelf.Controls;
using Shelf.Manager;
using Shelf.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;
using XFNoSoftKeyboadEntryControl;

namespace Shelf.Views
{
  [XamlCompilation]
  [XamlFilePath("Views\\ShelfSearch2.xaml")]
  public class ShelfSearch2 : ContentPage
  {
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage shelfSearch;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckBarcodeType;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelfList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfList;

    public Color ButtonColor => Color.FromRgb(93, 48, 243);

    public Color TextColor => Color.White;

    public ShelfSearch2()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Raf Bul";
      GlobalMob.AddBarcodeLongPress(this.txtBarcode);
      GlobalMob.FillBarcodeType(this.pckBarcodeType);
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
      this.BarcodeFocus(200);
    }

    private async void TxtBarcode_Completed(object sender, EventArgs e)
    {
      ShelfSearch2 page = this;
      string barcode = ((InputView) page.txtBarcode).Text;
      if (string.IsNullOrEmpty(barcode))
      {
        barcode = (string) null;
      }
      else
      {
        await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
        bool flag = false;
        string url = string.Format("GetShelfFromBarcode?barcode={0}&purchaseOrderID=-1", (object) barcode);
        PickerItem selectedItem = (PickerItem) page.pckBarcodeType.SelectedItem;
        if (selectedItem != null && selectedItem.Code == 2 && ((VisualElement) page.pckBarcodeType).IsVisible)
          flag = true;
        if (flag)
          url = string.Format("GetShelfFromLotBarcode?barcode={0}", (object) barcode);
        ReturnModel returnModel = GlobalMob.PostJson(url, (ContentPage) page);
        if (returnModel.Success)
        {
          List<pIOGetShelfFromBarcodeReturnModel> barcodeReturnModelList = GlobalMob.JsonDeserialize<List<pIOGetShelfFromBarcodeReturnModel>>(returnModel.Result);
          ((ItemsView<Cell>) page.lstShelfList).ItemsSource = (IEnumerable) barcodeReturnModelList;
          ((VisualElement) page.stckShelfList).IsVisible = true;
          if (barcodeReturnModelList == null || barcodeReturnModelList.Count == 0)
          {
            ((VisualElement) page.stckShelfList).IsVisible = false;
            GlobalMob.PlayError();
            int num = await ((Page) page).DisplayAlert("Hata", "Raf Bulunamadı", "", "Tamam") ? 1 : 0;
          }
        }
        GlobalMob.CloseLoading();
        page.BarcodeFocus(200);
        barcode = (string) null;
      }
    }

    private void BarcodeFocus(int time) => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(time);
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode)?.Focus();
    }));

    private void cmImage_Clicked(object sender, EventArgs e)
    {
      pIOGetShelfFromBarcodeReturnModel commandParameter = (pIOGetShelfFromBarcodeReturnModel) (sender as MenuItem).CommandParameter;
      ((NavigableElement) this).Navigation.PushAsync((Page) new ImagePage(commandParameter.Url, commandParameter.ItemDescription));
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (ShelfSearch2).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/ShelfSearch2.xaml",
        Instance = (object) this
      }))
        this.__InitComponentRuntime();
      else if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(((object) this).GetType()) != null)
      {
        this.__InitComponentRuntime();
      }
      else
      {
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry = new SoftkeyboardDisabledEntry();
        BindingExtension bindingExtension1 = new BindingExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        Picker picker = new Picker();
        StackLayout stackLayout1 = new StackLayout();
        StackLayout stackLayout2 = new StackLayout();
        BindingExtension bindingExtension3 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout3 = new StackLayout();
        StackLayout stackLayout4 = new StackLayout();
        ShelfSearch2 shelfSearch2;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (shelfSearch2 = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) shelfSearch2, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("shelfSearch", (object) shelfSearch2);
        if (((Element) shelfSearch2).StyleId == null)
          ((Element) shelfSearch2).StyleId = "shelfSearch";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry);
        if (((Element) softkeyboardDisabledEntry).StyleId == null)
          ((Element) softkeyboardDisabledEntry).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("pckBarcodeType", (object) picker);
        if (((Element) picker).StyleId == null)
          ((Element) picker).StyleId = "pckBarcodeType";
        ((INameScope) nameScope).RegisterName("stckShelfList", (object) stackLayout3);
        if (((Element) stackLayout3).StyleId == null)
          ((Element) stackLayout3).StyleId = "stckShelfList";
        ((INameScope) nameScope).RegisterName("lstShelfList", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstShelfList";
        this.shelfSearch = (ContentPage) shelfSearch2;
        this.stckForm = stackLayout2;
        this.txtBarcode = softkeyboardDisabledEntry;
        this.pckBarcodeType = picker;
        this.stckShelfList = stackLayout3;
        this.lstShelfList = listView;
        ((BindableObject) stackLayout4).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0, 2.0, 5.0, 5.0));
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout2).SetValue(StackLayout.SpacingProperty, (object) 20.0);
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(Entry.PlaceholderProperty, (object) "Barkod Okutunuz");
        softkeyboardDisabledEntry.Completed += new EventHandler(shelfSearch2.TxtBarcode_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) softkeyboardDisabledEntry);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry, new Uri("Views\\ShelfSearch2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 22);
        ((BindableObject) picker).SetValue(Picker.TitleProperty, (object) "Tip");
        bindingExtension1.Path = ".";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker).SetBinding(Picker.ItemsSourceProperty, bindingBase1);
        bindingExtension2.Path = "Caption";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        picker.ItemDisplayBinding = bindingBase2;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase2, new Uri("Views\\ShelfSearch2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 33);
        ((BindableObject) picker).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) picker).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) picker);
        VisualDiagnostics.RegisterSourceInfo((object) picker, new Uri("Views\\ShelfSearch2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\ShelfSearch2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\ShelfSearch2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 9, 14);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout3).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) listView).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        bindingExtension3.Path = ".";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase3);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ShelfSearch2.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_36 xamlCdataTemplate36 = new ShelfSearch2.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_36();
        object[] objArray = new object[0 + 5];
        objArray[0] = (object) dataTemplate1;
        objArray[1] = (object) listView;
        objArray[2] = (object) stackLayout3;
        objArray[3] = (object) stackLayout4;
        objArray[4] = (object) shelfSearch2;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate36.parentValues = objArray;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate36.root = shelfSearch2;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate36.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\ShelfSearch2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\ShelfSearch2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 18, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\ShelfSearch2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 14);
        ((BindableObject) shelfSearch2).SetValue(ContentPage.ContentProperty, (object) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\ShelfSearch2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 8, 10);
        VisualDiagnostics.RegisterSourceInfo((object) shelfSearch2, new Uri("Views\\ShelfSearch2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<ShelfSearch2>(this, typeof (ShelfSearch2));
      this.shelfSearch = NameScopeExtensions.FindByName<ContentPage>((Element) this, "shelfSearch");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.pckBarcodeType = NameScopeExtensions.FindByName<Picker>((Element) this, "pckBarcodeType");
      this.stckShelfList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfList");
      this.lstShelfList = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfList");
    }
  }
}
