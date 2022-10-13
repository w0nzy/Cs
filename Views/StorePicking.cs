// Decompiled with JetBrains decompiler
// Type: Shelf.Views.StorePicking
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Shelf.Manager;
using Shelf.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;
using XFNoSoftKeyboadEntryControl;

namespace Shelf.Views
{
  [XamlCompilation]
  [XamlFilePath("Views\\StorePicking.xaml")]
  public class StorePicking : ContentPage
  {
    private List<pIOGetStorePickMissingOrderReturnModel> list;
    private pIOGetStoreInfoReturnModel selectedStore;
    private ToolbarItem tInfo;
    private ListView lstStore;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtStore;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtQty;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtShelfOrder;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfDetail;

    public StorePicking()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Mağaza Toplama";
      ToolbarItem toolbarItem = new ToolbarItem();
      ((MenuItem) toolbarItem).Text = "";
      this.tInfo = toolbarItem;
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(this.tInfo);
    }

    private void SetInfo()
    {
      int int32 = Convert.ToInt32((object) this.list.Sum<pIOGetStorePickMissingOrderReturnModel>((Func<pIOGetStorePickMissingOrderReturnModel, double?>) (x => x.Qty)));
      ((MenuItem) this.tInfo).Text = Convert.ToInt32((object) this.list.Sum<pIOGetStorePickMissingOrderReturnModel>((Func<pIOGetStorePickMissingOrderReturnModel, double?>) (x => x.ApproveQty))).ToString() + "/" + int32.ToString();
    }

    private void LoadData()
    {
      this.list = new List<pIOGetStorePickMissingOrderReturnModel>();
      ((VisualElement) this.lstShelfDetail).IsVisible = true;
      Device.BeginInvokeOnMainThread((Action) (() =>
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetStorePickMissingOrder?currAccCode={0}&currAccTypeCode={1}", (object) this.selectedStore.CurrAccCode, (object) this.selectedStore.CurrAccTypeCode), (ContentPage) this);
        if (!returnModel.Success)
          return;
        ((NavigableElement) this).Navigation.PopAsync();
        this.list = GlobalMob.JsonDeserialize<List<pIOGetStorePickMissingOrderReturnModel>>(returnModel.Result);
        this.RefreshData();
        this.SetInfo();
        this.BarcodeFocus();
      }));
    }

    private void RefreshData()
    {
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
      if (this.list.Count <= 0)
        return;
      ((VisualElement) this.lstShelfDetail).IsVisible = true;
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) this.list.OrderByDescending<pIOGetStorePickMissingOrderReturnModel, bool>((Func<pIOGetStorePickMissingOrderReturnModel, bool>) (x => x.LastReadBarcode));
    }

    private async void txtStore_Focused(object sender, FocusEventArgs e)
    {
      StorePicking page = this;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetStoreInfo?searchText={0}", (object) ""), (ContentPage) page);
      if (!returnModel.Success)
        return;
      List<pIOGetStoreInfoReturnModel> storeInfoReturnModelList = GlobalMob.JsonDeserialize<List<pIOGetStoreInfoReturnModel>>(returnModel.Result);
      StackLayout stck = new StackLayout();
      SearchBar searchBar = new SearchBar();
      ((InputView) searchBar).Placeholder = "Ara";
      searchBar.SearchButtonPressed += new EventHandler(page.Search_SearchButtonPressed);
      ((InputView) searchBar).TextChanged += new EventHandler<TextChangedEventArgs>(page.Search_TextChanged);
      page.lstStore = GlobalMob.GetShelfListview("CurrAccDescription,CurrAccCode");
      ((ItemsView<Cell>) page.lstStore).ItemsSource = (IEnumerable) storeInfoReturnModelList;
      page.lstStore.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(page.Lst_ItemSelected);
      ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) searchBar);
      ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) page.lstStore);
      SelectItem selectItem = new SelectItem(stck, "Mağaza Seçiniz");
      await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
    }

    private void Search_TextChanged(object sender, TextChangedEventArgs e) => this.SearchStore(e.NewTextValue);

    private void Search_SearchButtonPressed(object sender, EventArgs e) => this.SearchStore(((InputView) sender).Text);

    private void SearchStore(string text)
    {
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetStoreInfo?searchText={0}", (object) text), (ContentPage) this);
      if (!returnModel.Success)
        return;
      List<pIOGetStoreInfoReturnModel> storeInfoReturnModelList = GlobalMob.JsonDeserialize<List<pIOGetStoreInfoReturnModel>>(returnModel.Result);
      ((ItemsView<Cell>) this.lstStore).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstStore).ItemsSource = (IEnumerable) storeInfoReturnModelList;
    }

    private async void Lst_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      StorePicking storePicking = this;
      storePicking.selectedStore = (pIOGetStoreInfoReturnModel) e.SelectedItem;
      ((InputView) storePicking.txtStore).Text = storePicking.selectedStore.CurrAccDescription;
      await NavigationExtension.PushPopupAsync(((NavigableElement) storePicking).Navigation, GlobalMob.ShowLoading(), true);
      storePicking.LoadData();
      GlobalMob.CloseLoading();
    }

    private async void txtBarcode_Completed(object sender, EventArgs e)
    {
      StorePicking page = this;
      string barcode = ((InputView) page.txtBarcode).Text;
      if (string.IsNullOrEmpty(barcode))
        return;
      int qty1 = page.GetQty();
      pIOGetStorePickMissingOrderReturnModel orderReturnModel1 = page.list.Where<pIOGetStorePickMissingOrderReturnModel>((Func<pIOGetStorePickMissingOrderReturnModel, bool>) (x =>
      {
        if (!x.Barcode.Contains(barcode))
          return false;
        double? approveQty = x.ApproveQty;
        double? qty2 = x.Qty;
        return approveQty.GetValueOrDefault() < qty2.GetValueOrDefault() & approveQty.HasValue & qty2.HasValue;
      })).FirstOrDefault<pIOGetStorePickMissingOrderReturnModel>();
      if (orderReturnModel1 == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Ürün Bulunamadı", "", "Tamam") ? 1 : 0;
        ((InputView) page.txtBarcode).Text = "";
        ((VisualElement) page.txtBarcode).Focus();
      }
      else
      {
        PickMissingOrder pickMissingOrder = new PickMissingOrder();
        pickMissingOrder.PickMissingOrderID = orderReturnModel1.PickMissingOrderID;
        pickMissingOrder.ShelfOrderDetailID = orderReturnModel1.ShelfOrderDetailID;
        pickMissingOrder.UserName = GlobalMob.User.UserName;
        pickMissingOrder.ShelfOrderID = orderReturnModel1.ShelfOrderID;
        pickMissingOrder.DispOrderNumber = orderReturnModel1.DispOrderNumber;
        pickMissingOrder.ShelfOrderType = (int) orderReturnModel1.ShelfOrderType;
        pickMissingOrder.Qty = qty1;
        Dictionary<string, string> paramList = new Dictionary<string, string>();
        string str = JsonConvert.SerializeObject((object) pickMissingOrder);
        paramList.Add("json", str);
        ReturnModel result = GlobalMob.PostJson("PickMissingOrder", paramList, (ContentPage) page).Result;
        if (result.Success)
        {
          ReturnModel returnModel = JsonConvert.DeserializeObject<ReturnModel>(result.Result);
          if (returnModel.Success)
          {
            page.list.Select<pIOGetStorePickMissingOrderReturnModel, pIOGetStorePickMissingOrderReturnModel>((Func<pIOGetStorePickMissingOrderReturnModel, pIOGetStorePickMissingOrderReturnModel>) (c =>
            {
              c.LastReadBarcode = false;
              return c;
            })).ToList<pIOGetStorePickMissingOrderReturnModel>();
            orderReturnModel1.LastReadBarcode = true;
            pIOGetStorePickMissingOrderReturnModel orderReturnModel2 = orderReturnModel1;
            double? approveQty = orderReturnModel2.ApproveQty;
            double num = (double) qty1;
            orderReturnModel2.ApproveQty = approveQty.HasValue ? new double?(approveQty.GetValueOrDefault() + num) : new double?();
            ((InputView) page.txtShelfOrder).Text = orderReturnModel1.ShelfOrderNumber;
            GlobalMob.PlaySave();
            page.RefreshData();
            page.BarcodeFocus();
          }
          else
          {
            GlobalMob.PlayError();
            int num = await ((Page) page).DisplayAlert("Hata", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
            page.BarcodeFocus();
          }
        }
        else
        {
          GlobalMob.PlayError();
          int num = await ((Page) page).DisplayAlert("Hata", result.ErrorMessage, "", "Tamam") ? 1 : 0;
          page.BarcodeFocus();
        }
      }
    }

    private void BarcodeFocus()
    {
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode).Focus();
    }

    private int GetQty()
    {
      try
      {
        return Convert.ToInt32(((InputView) this.txtQty).Text);
      }
      catch (Exception ex)
      {
        ((InputView) this.txtQty).Text = "1";
        return 1;
      }
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (StorePicking).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/StorePicking.xaml",
        Instance = (object) this
      }))
        this.__InitComponentRuntime();
      else if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(((object) this).GetType()) != null)
      {
        this.__InitComponentRuntime();
      }
      else
      {
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry1 = new SoftkeyboardDisabledEntry();
        StackLayout stackLayout1 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry2 = new SoftkeyboardDisabledEntry();
        Xamarin.Forms.Entry entry1 = new Xamarin.Forms.Entry();
        StackLayout stackLayout2 = new StackLayout();
        Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
        StackLayout stackLayout3 = new StackLayout();
        BindingExtension bindingExtension = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout4 = new StackLayout();
        StorePicking storePicking;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (storePicking = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) storePicking, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtStore", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtStore";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("txtQty", (object) entry1);
        if (((Element) entry1).StyleId == null)
          ((Element) entry1).StyleId = "txtQty";
        ((INameScope) nameScope).RegisterName("txtShelfOrder", (object) entry2);
        if (((Element) entry2).StyleId == null)
          ((Element) entry2).StyleId = "txtShelfOrder";
        ((INameScope) nameScope).RegisterName("lstShelfDetail", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstShelfDetail";
        this.stckForm = stackLayout4;
        this.txtStore = softkeyboardDisabledEntry1;
        this.txtBarcode = softkeyboardDisabledEntry2;
        this.txtQty = entry1;
        this.txtShelfOrder = entry2;
        this.lstShelfDetail = listView;
        ((BindableObject) stackLayout4).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Mağaza Seçiniz..");
        ((VisualElement) softkeyboardDisabledEntry1).Focused += new EventHandler<FocusEventArgs>(storePicking.txtStore_Focused);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\StorePicking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\StorePicking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 14);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        softkeyboardDisabledEntry2.Completed += new EventHandler(storePicking.txtBarcode_Completed);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod Numarası Giriniz");
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\StorePicking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 22, 18);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.TextProperty, (object) "1");
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Miktar");
        ((BindableObject) entry1).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\StorePicking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 24, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\StorePicking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 21, 14);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf Emri");
        ((BindableObject) entry2).SetValue(VisualElement.BackgroundColorProperty, (object) Color.White);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.TextColorProperty, (object) Color.Red);
        Xamarin.Forms.Entry entry3 = entry2;
        BindableProperty fontSizeProperty = Xamarin.Forms.Entry.FontSizeProperty;
        FontSizeConverter fontSizeConverter = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 4];
        objArray1[0] = (object) entry2;
        objArray1[1] = (object) stackLayout3;
        objArray1[2] = (object) stackLayout4;
        objArray1[3] = (object) storePicking;
        SimpleValueTargetProvider valueTargetProvider;
        object obj1 = (object) (valueTargetProvider = new SimpleValueTargetProvider(objArray1, (object) Xamarin.Forms.Entry.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider.Add(type1, (object) valueTargetProvider);
        xamlServiceProvider.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver = new XmlNamespaceResolver();
        namespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver, typeof (StorePicking).GetTypeInfo().Assembly);
        xamlServiceProvider.Add(type2, (object) xamlTypeResolver);
        xamlServiceProvider.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(28, 48)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider);
        ((BindableObject) entry3).SetValue(fontSizeProperty, obj2);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\StorePicking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 27, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\StorePicking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 26, 14);
        bindingExtension.Path = ".";
        BindingBase bindingBase = ((IMarkupExtension<BindingBase>) bindingExtension).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
        ((BindableObject) listView).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) listView).SetValue(ListView.RowHeightProperty, (object) 100);
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 5000.0);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        StorePicking.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_39 xamlCdataTemplate39 = new StorePicking.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_39();
        object[] objArray2 = new object[0 + 4];
        objArray2[0] = (object) dataTemplate1;
        objArray2[1] = (object) listView;
        objArray2[2] = (object) stackLayout4;
        objArray2[3] = (object) storePicking;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate39.parentValues = objArray2;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate39.root = storePicking;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate39.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\StorePicking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 34, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\StorePicking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 31, 14);
        ((BindableObject) storePicking).SetValue(ContentPage.ContentProperty, (object) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\StorePicking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 10);
        VisualDiagnostics.RegisterSourceInfo((object) storePicking, new Uri("Views\\StorePicking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<StorePicking>(this, typeof (StorePicking));
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtStore = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtStore");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.txtQty = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtQty");
      this.txtShelfOrder = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtShelfOrder");
      this.lstShelfDetail = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfDetail");
    }
  }
}
