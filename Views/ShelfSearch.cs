// Decompiled with JetBrains decompiler
// Type: Shelf.Views.ShelfSearch
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

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
  [XamlFilePath("Views\\ShelfSearch.xaml")]
  public class ShelfSearch : ContentPage
  {
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage shelfSearch;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelfList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfList;

    public Color ButtonColor => Color.FromRgb(93, 48, 243);

    public Color TextColor => Color.White;

    public ShelfSearch()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Raf Bul";
      GlobalMob.AddBarcodeLongPress(this.txtBarcode);
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
      this.BarcodeFocus(200);
    }

    private void TxtBarcode_Completed(object sender, EventArgs e)
    {
      string text = ((InputView) this.txtBarcode).Text;
      if (string.IsNullOrEmpty(text))
        return;
      if (text.Length < GlobalMob.User.MinimumBarcodeLength)
      {
        GlobalMob.PlayError();
        ((InputView) this.txtBarcode).Text = "";
        ((VisualElement) this.txtBarcode).Focus();
      }
      else
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfFromBarcode?barcode={0}&purchaseOrderID=-1", (object) ((InputView) this.txtBarcode).Text), (ContentPage) this);
        if (returnModel.Success)
        {
          List<pIOGetShelfFromBarcodeReturnModel> barcodeReturnModelList = GlobalMob.JsonDeserialize<List<pIOGetShelfFromBarcodeReturnModel>>(returnModel.Result);
          ((ItemsView<Cell>) this.lstShelfList).ItemsSource = (IEnumerable) barcodeReturnModelList;
          ((VisualElement) this.stckShelfList).IsVisible = true;
          if (barcodeReturnModelList == null || barcodeReturnModelList.Count == 0)
          {
            ((VisualElement) this.stckShelfList).IsVisible = false;
            GlobalMob.PlayError();
            ((Page) this).DisplayAlert("Hata", "Raf Bulunamadı", "", "Tamam");
          }
        }
        this.BarcodeFocus(200);
      }
    }

    private void BarcodeFocus(int time) => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(time);
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode)?.Focus();
    }));

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (ShelfSearch).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/ShelfSearch.xaml",
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
        StackLayout stackLayout1 = new StackLayout();
        StackLayout stackLayout2 = new StackLayout();
        BindingExtension bindingExtension = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout3 = new StackLayout();
        StackLayout stackLayout4 = new StackLayout();
        ShelfSearch shelfSearch;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (shelfSearch = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) shelfSearch, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("shelfSearch", (object) shelfSearch);
        if (((Element) shelfSearch).StyleId == null)
          ((Element) shelfSearch).StyleId = "shelfSearch";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry);
        if (((Element) softkeyboardDisabledEntry).StyleId == null)
          ((Element) softkeyboardDisabledEntry).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("stckShelfList", (object) stackLayout3);
        if (((Element) stackLayout3).StyleId == null)
          ((Element) stackLayout3).StyleId = "stckShelfList";
        ((INameScope) nameScope).RegisterName("lstShelfList", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstShelfList";
        this.shelfSearch = (ContentPage) shelfSearch;
        this.stckForm = stackLayout2;
        this.txtBarcode = softkeyboardDisabledEntry;
        this.stckShelfList = stackLayout3;
        this.lstShelfList = listView;
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout2).SetValue(StackLayout.SpacingProperty, (object) 20.0);
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(Entry.PlaceholderProperty, (object) "Barkod Okutunuz");
        softkeyboardDisabledEntry.Completed += new EventHandler(shelfSearch.TxtBarcode_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) softkeyboardDisabledEntry);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry, new Uri("Views\\ShelfSearch.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\ShelfSearch.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\ShelfSearch.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 9, 14);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout3).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) listView).SetValue(ListView.RowHeightProperty, (object) 80);
        bindingExtension.Path = ".";
        BindingBase bindingBase = ((IMarkupExtension<BindingBase>) bindingExtension).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ShelfSearch.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_35 xamlCdataTemplate35 = new ShelfSearch.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_35();
        object[] objArray = new object[0 + 5];
        objArray[0] = (object) dataTemplate1;
        objArray[1] = (object) listView;
        objArray[2] = (object) stackLayout3;
        objArray[3] = (object) stackLayout4;
        objArray[4] = (object) shelfSearch;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate35.parentValues = objArray;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate35.root = shelfSearch;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate35.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\ShelfSearch.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\ShelfSearch.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\ShelfSearch.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 14);
        ((BindableObject) shelfSearch).SetValue(ContentPage.ContentProperty, (object) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\ShelfSearch.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 8, 10);
        VisualDiagnostics.RegisterSourceInfo((object) shelfSearch, new Uri("Views\\ShelfSearch.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Extensions.LoadFromXaml<ShelfSearch>(this, typeof (ShelfSearch));
      this.shelfSearch = NameScopeExtensions.FindByName<ContentPage>((Element) this, "shelfSearch");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.stckShelfList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfList");
      this.lstShelfList = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfList");
    }
  }
}
