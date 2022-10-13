// Decompiled with JetBrains decompiler
// Type: Shelf.Views.AGAlcOrder
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
using System.Threading.Tasks;
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
  [XamlFilePath("Views\\AGAlcOrder.xaml")]
  public class AGAlcOrder : ContentPage
  {
    private ListView lstVendor;
    private List<pIOGetShelfVendorSubCurrAccReturnModel> vendorList;
    private pIOGetShelfVendorSubCurrAccReturnModel selectedVendor;
    private List<pIOPurchaseOrderAllocatePackageReturnModel> goodsInList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtVendorCode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckBarcodeType;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckCustomer;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtCustomer;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstGoodsInDetails;

    public AGAlcOrder()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Mal Kabul(Sipariş)";
      ToolbarItem toolbarItem = new ToolbarItem();
      ((MenuItem) toolbarItem).Text = "";
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(toolbarItem);
      GlobalMob.FillBarcodeType(this.pckBarcodeType);
    }

    protected virtual void OnAppearing() => ((Page) this).OnAppearing();

    private async void txtVendorCode_Focused(object sender, FocusEventArgs e)
    {
      AGAlcOrder agAlcOrder = this;
      StackLayout stck = new StackLayout();
      stck.Orientation = (StackOrientation) 0;
      agAlcOrder.vendorList = new List<pIOGetShelfVendorSubCurrAccReturnModel>();
      agAlcOrder.lstVendor = GlobalMob.GetListview("CurrAccDescription,CurrAccCode", 2, 1, hasUnEvenRows: true);
      SearchBar searchBar1 = new SearchBar();
      ((InputView) searchBar1).Placeholder = "Ara";
      ((InputView) searchBar1).PlaceholderColor = Color.Orange;
      ((InputView) searchBar1).TextColor = Color.Orange;
      searchBar1.HorizontalTextAlignment = (TextAlignment) 1;
      searchBar1.FontSize = Device.GetNamedSize((NamedSize) 3, typeof (SearchBar));
      searchBar1.FontAttributes = (FontAttributes) 2;
      SearchBar searchBar = searchBar1;
      ((InputView) searchBar).TextChanged += new EventHandler<TextChangedEventArgs>(agAlcOrder.SearchBar_TextChanged);
      ((ItemsView<Cell>) agAlcOrder.lstVendor).ItemsSource = (IEnumerable) agAlcOrder.vendorList;
      agAlcOrder.lstVendor.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(agAlcOrder.LstVendor_ItemSelected);
      ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) searchBar);
      ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) agAlcOrder.lstVendor);
      SelectItem selectItem = new SelectItem(stck, "Tedarikçi Seçiniz");
      await ((NavigableElement) agAlcOrder).Navigation.PushAsync((Page) selectItem);
      ((VisualElement) searchBar).Focus();
      searchBar = (SearchBar) null;
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
      SearchBar searchBar = (SearchBar) sender;
      string lower = ((InputView) searchBar).Text.ToLower();
      if (!string.IsNullOrEmpty(((InputView) searchBar).Text))
      {
        if (lower.Length < 2)
          return;
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetVendorsSubCurrAcc?searchText={0}", (object) lower), (ContentPage) this);
        if (returnModel.Success)
          this.vendorList = GlobalMob.JsonDeserialize<List<pIOGetShelfVendorSubCurrAccReturnModel>>(returnModel.Result);
        ((ItemsView<Cell>) this.lstVendor).ItemsSource = (IEnumerable) this.vendorList;
      }
      else
        ((ItemsView<Cell>) this.lstVendor).ItemsSource = (IEnumerable) null;
    }

    private async void LstVendor_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      AGAlcOrder page1 = this;
      await NavigationExtension.PushPopupAsync(((NavigableElement) page1).Navigation, GlobalMob.ShowLoading(), true);
      page1.selectedVendor = (pIOGetShelfVendorSubCurrAccReturnModel) e.SelectedItem;
      ((InputView) page1.txtVendorCode).Text = page1.selectedVendor.CurrAccCode;
      Page page2 = await ((NavigableElement) page1).Navigation.PopAsync();
      ReturnModel returnModel = GlobalMob.PostJson("GetOrderAllocatePackage?currAccTypeCode=" + page1.selectedVendor.CurrAccTypeCode.ToString() + "&currAccCode=" + page1.selectedVendor.CurrAccCode + "&subCurrAccID=" + page1.selectedVendor.SubCurrAccID.ToString(), (ContentPage) page1);
      if (returnModel.Success)
      {
        page1.goodsInList = GlobalMob.JsonDeserialize<List<pIOPurchaseOrderAllocatePackageReturnModel>>(returnModel.Result);
        page1.RefreshData();
      }
      GlobalMob.CloseLoading();
      page1.BarcodeFocus();
    }

    private void RefreshData()
    {
      ((ItemsView<Cell>) this.lstGoodsInDetails).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstGoodsInDetails).ItemsSource = (IEnumerable) this.goodsInList.OrderByDescending<pIOPurchaseOrderAllocatePackageReturnModel, bool>((Func<pIOPurchaseOrderAllocatePackageReturnModel, bool>) (x => x.LastReadBarcode));
    }

    private void BarcodeFocus() => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(100);
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode)?.Focus();
    }));

    private async void txtBarcode_Completed(object sender, EventArgs e)
    {
      AGAlcOrder page = this;
      string barcode = ((InputView) page.txtBarcode).Text;
      if (string.IsNullOrEmpty(barcode))
        return;
      bool flag = false;
      PickerItem selectedItem = (PickerItem) page.pckBarcodeType.SelectedItem;
      if (selectedItem != null && selectedItem.Code == 2 && ((VisualElement) page.pckBarcodeType).IsVisible)
        flag = true;
      if (flag)
      {
        List<AlcPackage> alcPackageList = new List<AlcPackage>();
        pIOPurchaseOrderAllocatePackageReturnModel findItem = page.goodsInList.Where<pIOPurchaseOrderAllocatePackageReturnModel>((Func<pIOPurchaseOrderAllocatePackageReturnModel, bool>) (x =>
        {
          if (!x.LotBarcode.Contains(barcode))
            return false;
          double? orderQty = x.OrderQty;
          double num = 0.0;
          return orderQty.GetValueOrDefault() > num & orderQty.HasValue;
        })).FirstOrDefault<pIOPurchaseOrderAllocatePackageReturnModel>();
        if (findItem == null)
        {
          alcPackageList.Add(new AlcPackage()
          {
            Barcode = barcode,
            UserName = GlobalMob.User.UserName,
            VendorCurrAccCode = page.selectedVendor.CurrAccCode,
            VendorCurrAccTypeCode = Convert.ToInt32((object) page.selectedVendor.CurrAccTypeCode),
            VendorSubCurrAccID = Convert.ToString((object) page.selectedVendor.SubCurrAccID),
            LotBarcode = barcode
          });
        }
        else
        {
          foreach (pIOPurchaseOrderAllocatePackageReturnModel packageReturnModel in page.goodsInList.Where<pIOPurchaseOrderAllocatePackageReturnModel>((Func<pIOPurchaseOrderAllocatePackageReturnModel, bool>) (x =>
          {
            if (!(x.OrderNumber == findItem.OrderNumber))
              return false;
            int? orderLineSumId1 = x.OrderLineSumID;
            int? orderLineSumId2 = findItem.OrderLineSumID;
            return orderLineSumId1.GetValueOrDefault() == orderLineSumId2.GetValueOrDefault() & orderLineSumId1.HasValue == orderLineSumId2.HasValue;
          })).ToList<pIOPurchaseOrderAllocatePackageReturnModel>())
            alcPackageList.Add(new AlcPackage()
            {
              Barcode = barcode,
              OrderLineID = Convert.ToString((object) packageReturnModel.OrderLineID),
              UserName = GlobalMob.User.UserName,
              VendorCurrAccCode = page.selectedVendor.CurrAccCode,
              VendorCurrAccTypeCode = Convert.ToInt32((object) page.selectedVendor.CurrAccTypeCode),
              VendorSubCurrAccID = Convert.ToString((object) page.selectedVendor.SubCurrAccID),
              LotBarcode = packageReturnModel.LotBarcode,
              LotCode = packageReturnModel.LotCode,
              OrderHeaderID = Convert.ToString((object) packageReturnModel.OrderHeaderID),
              OrderLineSumID = Convert.ToInt32((object) packageReturnModel.OrderLineSumID),
              OrderQty = Convert.ToInt32((object) packageReturnModel.OrderQty)
            });
        }
        ReturnModel result = GlobalMob.PostJson("InsAlcOrderLot", new Dictionary<string, string>()
        {
          {
            "json",
            JsonConvert.SerializeObject((object) alcPackageList)
          }
        }, (ContentPage) page).Result;
        if (!result.Success)
          return;
        pIOGetRemainingDispOrderByItemAlcOrderReturnModel orderReturnModel = GlobalMob.JsonDeserialize<pIOGetRemainingDispOrderByItemAlcOrderReturnModel>(result.Result);
        if (orderReturnModel == null)
        {
          GlobalMob.PlayError();
          int num = await ((Page) page).DisplayAlert("Bilgi", "Bu ürünün dağıtılacağı sipariş bulunamadı", "", "Tamam") ? 1 : 0;
          page.BarcodeFocus();
        }
        else
        {
          page.goodsInList.Select<pIOPurchaseOrderAllocatePackageReturnModel, pIOPurchaseOrderAllocatePackageReturnModel>((Func<pIOPurchaseOrderAllocatePackageReturnModel, pIOPurchaseOrderAllocatePackageReturnModel>) (c =>
          {
            c.LastReadBarcode = false;
            return c;
          })).ToList<pIOPurchaseOrderAllocatePackageReturnModel>();
          findItem.LastReadBarcode = true;
          pIOPurchaseOrderAllocatePackageReturnModel packageReturnModel1 = findItem;
          double? orderQty = packageReturnModel1.OrderQty;
          double num = 1.0;
          packageReturnModel1.OrderQty = orderQty.HasValue ? new double?(orderQty.GetValueOrDefault() - num) : new double?();
          pIOPurchaseOrderAllocatePackageReturnModel packageReturnModel2 = findItem;
          int? pdetailQty = packageReturnModel2.PDetailQty;
          packageReturnModel2.PDetailQty = pdetailQty.HasValue ? new int?(pdetailQty.GetValueOrDefault() + 1) : new int?();
          ((InputView) page.txtCustomer).Text = orderReturnModel.PackageCode + "-" + orderReturnModel.CustomerDesc;
          if (((IEnumerable<ToolbarItem>) ((Page) page).ToolbarItems).Count<ToolbarItem>() > 0)
            ((MenuItem) ((Page) page).ToolbarItems[0]).Text = orderReturnModel.RemainingQty;
          GlobalMob.PlaySave();
          page.RefreshData();
          page.BarcodeFocus();
        }
      }
      else
      {
        pIOPurchaseOrderAllocatePackageReturnModel packageReturnModel3 = page.goodsInList.Where<pIOPurchaseOrderAllocatePackageReturnModel>((Func<pIOPurchaseOrderAllocatePackageReturnModel, bool>) (x =>
        {
          if (!x.Barcode.Contains(barcode))
            return false;
          double? orderQty = x.OrderQty;
          double num = 0.0;
          return orderQty.GetValueOrDefault() > num & orderQty.HasValue;
        })).FirstOrDefault<pIOPurchaseOrderAllocatePackageReturnModel>();
        if (packageReturnModel3 == null)
        {
          packageReturnModel3 = page.goodsInList.Where<pIOPurchaseOrderAllocatePackageReturnModel>((Func<pIOPurchaseOrderAllocatePackageReturnModel, bool>) (x => x.Barcode.Contains(barcode))).FirstOrDefault<pIOPurchaseOrderAllocatePackageReturnModel>();
          if (packageReturnModel3 == null)
          {
            GlobalMob.PlayError();
            int num = await ((Page) page).DisplayAlert("Bilgi", "Ürün bulunamadı", "", "Tamam") ? 1 : 0;
            page.BarcodeFocus();
            return;
          }
        }
        string str = "";
        if (packageReturnModel3 != null)
          str = Convert.ToString((object) packageReturnModel3.OrderLineID);
        ReturnModel result = GlobalMob.PostJson("InsAlcOrder", new Dictionary<string, string>()
        {
          {
            "json",
            JsonConvert.SerializeObject((object) new AlcPackage()
            {
              Barcode = barcode,
              OrderLineID = str,
              UserName = GlobalMob.User.UserName,
              VendorCurrAccCode = page.selectedVendor.CurrAccCode,
              VendorCurrAccTypeCode = Convert.ToInt32((object) page.selectedVendor.CurrAccTypeCode),
              VendorSubCurrAccID = Convert.ToString((object) page.selectedVendor.SubCurrAccID)
            })
          }
        }, (ContentPage) page).Result;
        if (!result.Success)
          return;
        pIOGetRemainingDispOrderByItemAlcOrderReturnModel orderReturnModel = GlobalMob.JsonDeserialize<pIOGetRemainingDispOrderByItemAlcOrderReturnModel>(result.Result);
        if (orderReturnModel == null)
        {
          GlobalMob.PlayError();
          int num = await ((Page) page).DisplayAlert("Bilgi", "Bu ürünün dağıtılacağı sipariş bulunamadı", "", "Tamam") ? 1 : 0;
          page.BarcodeFocus();
        }
        else
        {
          page.goodsInList.Select<pIOPurchaseOrderAllocatePackageReturnModel, pIOPurchaseOrderAllocatePackageReturnModel>((Func<pIOPurchaseOrderAllocatePackageReturnModel, pIOPurchaseOrderAllocatePackageReturnModel>) (c =>
          {
            c.LastReadBarcode = false;
            return c;
          })).ToList<pIOPurchaseOrderAllocatePackageReturnModel>();
          packageReturnModel3.LastReadBarcode = true;
          pIOPurchaseOrderAllocatePackageReturnModel packageReturnModel4 = packageReturnModel3;
          double? orderQty = packageReturnModel4.OrderQty;
          double num = 1.0;
          packageReturnModel4.OrderQty = orderQty.HasValue ? new double?(orderQty.GetValueOrDefault() - num) : new double?();
          pIOPurchaseOrderAllocatePackageReturnModel packageReturnModel5 = packageReturnModel3;
          int? pdetailQty = packageReturnModel5.PDetailQty;
          packageReturnModel5.PDetailQty = pdetailQty.HasValue ? new int?(pdetailQty.GetValueOrDefault() + 1) : new int?();
          ((InputView) page.txtCustomer).Text = orderReturnModel.PackageCode + "-" + orderReturnModel.CustomerDesc;
          if (((IEnumerable<ToolbarItem>) ((Page) page).ToolbarItems).Count<ToolbarItem>() > 0)
            ((MenuItem) ((Page) page).ToolbarItems[0]).Text = orderReturnModel.RemainingQty;
          GlobalMob.PlaySave();
          page.RefreshData();
          page.BarcodeFocus();
        }
      }
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (AGAlcOrder).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/AGAlcOrder.xaml",
        Instance = (object) this
      }))
        this.__InitComponentRuntime();
      else if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(((object) this).GetType()) != null)
      {
        this.__InitComponentRuntime();
      }
      else
      {
        Xamarin.Forms.Entry entry = new Xamarin.Forms.Entry();
        StackLayout stackLayout1 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry1 = new SoftkeyboardDisabledEntry();
        BindingExtension bindingExtension1 = new BindingExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        Picker picker = new Picker();
        StackLayout stackLayout2 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry2 = new SoftkeyboardDisabledEntry();
        StackLayout stackLayout3 = new StackLayout();
        BindingExtension bindingExtension3 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout4 = new StackLayout();
        AGAlcOrder agAlcOrder;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (agAlcOrder = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) agAlcOrder, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtVendorCode", (object) entry);
        if (((Element) entry).StyleId == null)
          ((Element) entry).StyleId = "txtVendorCode";
        ((INameScope) nameScope).RegisterName("stckBarcode", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckBarcode";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("pckBarcodeType", (object) picker);
        if (((Element) picker).StyleId == null)
          ((Element) picker).StyleId = "pckBarcodeType";
        ((INameScope) nameScope).RegisterName("stckCustomer", (object) stackLayout3);
        if (((Element) stackLayout3).StyleId == null)
          ((Element) stackLayout3).StyleId = "stckCustomer";
        ((INameScope) nameScope).RegisterName("txtCustomer", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtCustomer";
        ((INameScope) nameScope).RegisterName("lstGoodsInDetails", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstGoodsInDetails";
        this.stckForm = stackLayout4;
        this.txtVendorCode = entry;
        this.stckBarcode = stackLayout2;
        this.txtBarcode = softkeyboardDisabledEntry1;
        this.pckBarcodeType = picker;
        this.stckCustomer = stackLayout3;
        this.txtCustomer = softkeyboardDisabledEntry2;
        this.lstGoodsInDetails = listView;
        ((BindableObject) stackLayout4).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Tedarikçi Seçiniz");
        ((VisualElement) entry).Focused += new EventHandler<FocusEventArgs>(agAlcOrder.txtVendorCode_Focused);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) entry);
        VisualDiagnostics.RegisterSourceInfo((object) entry, new Uri("Views\\AGAlcOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\AGAlcOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 14);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Dağıtım İçin Barkod No Okutunuz");
        softkeyboardDisabledEntry1.Completed += new EventHandler(agAlcOrder.txtBarcode_Completed);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\AGAlcOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 18);
        ((BindableObject) picker).SetValue(Picker.TitleProperty, (object) "Tip");
        bindingExtension1.Path = ".";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker).SetBinding(Picker.ItemsSourceProperty, bindingBase1);
        bindingExtension2.Path = "Caption";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        picker.ItemDisplayBinding = bindingBase2;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase2, new Uri("Views\\AGAlcOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 33);
        ((BindableObject) picker).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) picker).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) picker);
        VisualDiagnostics.RegisterSourceInfo((object) picker, new Uri("Views\\AGAlcOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\AGAlcOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 14);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Müşteri");
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(VisualElement.InputTransparentProperty, (object) true);
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry3 = softkeyboardDisabledEntry2;
        BindableProperty fontSizeProperty = Xamarin.Forms.Entry.FontSizeProperty;
        FontSizeConverter fontSizeConverter = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 4];
        objArray1[0] = (object) softkeyboardDisabledEntry2;
        objArray1[1] = (object) stackLayout3;
        objArray1[2] = (object) stackLayout4;
        objArray1[3] = (object) agAlcOrder;
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
        XamlTypeResolver xamlTypeResolver = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver, typeof (AGAlcOrder).GetTypeInfo().Assembly);
        xamlServiceProvider.Add(type2, (object) xamlTypeResolver);
        xamlServiceProvider.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(21, 96)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter).ConvertFromInvariantString("Medium", (IServiceProvider) xamlServiceProvider);
        ((BindableObject) softkeyboardDisabledEntry3).SetValue(fontSizeProperty, obj2);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\AGAlcOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 21, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\AGAlcOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 14);
        bindingExtension3.Path = ".";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase3);
        ((BindableObject) listView).SetValue(ListView.RowHeightProperty, (object) 90);
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 5000.0);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AGAlcOrder.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_2 xamlCdataTemplate2 = new AGAlcOrder.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_2();
        object[] objArray2 = new object[0 + 4];
        objArray2[0] = (object) dataTemplate1;
        objArray2[1] = (object) listView;
        objArray2[2] = (object) stackLayout4;
        objArray2[3] = (object) agAlcOrder;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate2.parentValues = objArray2;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate2.root = agAlcOrder;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate2.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\AGAlcOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 26, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\AGAlcOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 24, 14);
        ((BindableObject) agAlcOrder).SetValue(ContentPage.ContentProperty, (object) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\AGAlcOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 10);
        VisualDiagnostics.RegisterSourceInfo((object) agAlcOrder, new Uri("Views\\AGAlcOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<AGAlcOrder>(this, typeof (AGAlcOrder));
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtVendorCode = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtVendorCode");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.pckBarcodeType = NameScopeExtensions.FindByName<Picker>((Element) this, "pckBarcodeType");
      this.stckCustomer = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckCustomer");
      this.txtCustomer = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtCustomer");
      this.lstGoodsInDetails = NameScopeExtensions.FindByName<ListView>((Element) this, "lstGoodsInDetails");
    }
  }
}
