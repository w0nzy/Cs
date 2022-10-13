// Decompiled with JetBrains decompiler
// Type: Shelf.Views.AGAlcOrderApprove
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using Newtonsoft.Json;
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
  [XamlFilePath("Views\\AGAlcOrderApprove.xaml")]
  public class AGAlcOrderApprove : ContentPage
  {
    private ListView lstCustomer;
    private List<pIOGetAlcPackageCustomersReturnModel> customerList;
    private List<pIOPurchasePackageDetailsReturnModel> detailList;
    private pIOGetAlcPackageCustomersReturnModel selectedCustomer;
    private PickerItem selectedPackage;
    private string lastSearchText;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage approve;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtCustomerCode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckPackageBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckPackages;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtDesc;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnCompleted;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstPackages;

    public Color ButtonColor => Color.FromRgb(142, 81, 152);

    public Color TextColor => Color.White;

    public AGAlcOrderApprove()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Koli Onaylama";
      ToolbarItem toolbarItem = new ToolbarItem();
      ((MenuItem) toolbarItem).Text = "";
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(toolbarItem);
    }

    protected virtual void OnAppearing() => ((Page) this).OnAppearing();

    private async void txtCustomerCode_Focused(object sender, FocusEventArgs e)
    {
      AGAlcOrderApprove agAlcOrderApprove = this;
      StackLayout stck = new StackLayout();
      stck.Orientation = (StackOrientation) 0;
      agAlcOrderApprove.customerList = new List<pIOGetAlcPackageCustomersReturnModel>();
      agAlcOrderApprove.lstCustomer = GlobalMob.GetListview("CurrAccDescription,CurrAccCode", 2, 1, hasUnEvenRows: true);
      SearchBar searchBar1 = new SearchBar();
      ((InputView) searchBar1).Placeholder = "Ara";
      ((InputView) searchBar1).PlaceholderColor = Color.Orange;
      ((InputView) searchBar1).TextColor = Color.Orange;
      searchBar1.HorizontalTextAlignment = (TextAlignment) 1;
      searchBar1.FontSize = Device.GetNamedSize((NamedSize) 3, typeof (SearchBar));
      searchBar1.FontAttributes = (FontAttributes) 2;
      SearchBar searchBar = searchBar1;
      searchBar.SearchButtonPressed += new EventHandler(agAlcOrderApprove.SearchBar_SearchButtonPressed);
      ((ItemsView<Cell>) agAlcOrderApprove.lstCustomer).ItemsSource = (IEnumerable) agAlcOrderApprove.customerList;
      agAlcOrderApprove.lstCustomer.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(agAlcOrderApprove.LstCustomer_ItemSelected);
      ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) searchBar);
      ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) agAlcOrderApprove.lstCustomer);
      SelectItem selectItem = new SelectItem(stck, "Müşteri Seçiniz");
      await ((NavigableElement) agAlcOrderApprove).Navigation.PushAsync((Page) selectItem);
      ((VisualElement) searchBar).Focus();
      searchBar = (SearchBar) null;
    }

    private void SearchBar_SearchButtonPressed(object sender, EventArgs e) => this.SearchCustomer(((InputView) sender).Text.ToLower());

    private void SearchCustomer(string text)
    {
      if (!string.IsNullOrEmpty(text))
      {
        if (text.Length < 2 || this.lastSearchText == text)
          return;
        ReturnModel returnModel = GlobalMob.PostJson("GetAlcPackageCustomers?searchText=" + text, (ContentPage) this);
        if (returnModel.Success)
          this.customerList = GlobalMob.JsonDeserialize<List<pIOGetAlcPackageCustomersReturnModel>>(returnModel.Result);
        ((ItemsView<Cell>) this.lstCustomer).ItemsSource = (IEnumerable) this.customerList;
        this.lastSearchText = text;
      }
      else
        ((ItemsView<Cell>) this.lstCustomer).ItemsSource = (IEnumerable) null;
    }

    private async void LstCustomer_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      AGAlcOrderApprove page1 = this;
      page1.selectedCustomer = (pIOGetAlcPackageCustomersReturnModel) e.SelectedItem;
      ((InputView) page1.txtCustomerCode).Text = page1.selectedCustomer.CurrAccDescription;
      ReturnModel result = GlobalMob.PostJson(string.Format("GetCustomerPurchasePackage?currAccCode={0}&currAccTypeCode={1}&subCurrAccID={2}", (object) page1.selectedCustomer.CurrAccCode, (object) page1.selectedCustomer.CurrAccTypeCode, (object) page1.selectedCustomer.SubCurrAccID), (ContentPage) page1);
      if (!result.Success)
      {
        result = (ReturnModel) null;
      }
      else
      {
        Page page2 = await ((NavigableElement) page1).Navigation.PopAsync();
        List<pIOGetCustomerPurchasePackageReturnModel> packageReturnModelList = GlobalMob.JsonDeserialize<List<pIOGetCustomerPurchasePackageReturnModel>>(result.Result);
        List<PickerItem> pickerItemList = new List<PickerItem>();
        foreach (pIOGetCustomerPurchasePackageReturnModel packageReturnModel in packageReturnModelList)
        {
          string str = !string.IsNullOrEmpty(packageReturnModel.CompanyName) ? packageReturnModel.CompanyName : packageReturnModel.CurrAccDesc;
          pickerItemList.Add(new PickerItem()
          {
            Caption = packageReturnModel.PackageCode,
            Code = packageReturnModel.PackageAllocateID,
            Description = str
          });
        }
        page1.selectedPackage = (PickerItem) null;
        page1.pckPackages.ItemsSource = (IList) pickerItemList;
        if (packageReturnModelList.Count > 1)
        {
          ((VisualElement) page1.pckPackages).Focus();
          result = (ReturnModel) null;
        }
        else if (packageReturnModelList.Count == 1)
        {
          page1.selectedPackage = pickerItemList[0];
          page1.pckPackages.SelectedItem = (object) pickerItemList[0];
          page1.BarcodeFocus();
          result = (ReturnModel) null;
        }
        else
        {
          int num = await ((Page) page1).DisplayAlert("Bilgi", "Müşteriye ait koli bulunamadı", "", "Tamam") ? 1 : 0;
          page1.BarcodeFocus();
          result = (ReturnModel) null;
        }
      }
    }

    private void LoadDetails(int packageAllocateID)
    {
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetPurchasePackageDetails?packageID={0}", (object) packageAllocateID), (ContentPage) this);
      if (!returnModel.Success)
        return;
      this.detailList = GlobalMob.JsonDeserialize<List<pIOPurchasePackageDetailsReturnModel>>(returnModel.Result);
      ((VisualElement) this.btnCompleted).IsVisible = true;
      ((VisualElement) this.txtBarcode).IsVisible = true;
      ((VisualElement) this.pckPackages).IsVisible = true;
      this.RefreshData();
      this.BarcodeFocus();
    }

    private void ClearForm()
    {
      this.detailList = new List<pIOPurchasePackageDetailsReturnModel>();
      ((ItemsView<Cell>) this.lstPackages).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstPackages).ItemsSource = (IEnumerable) this.detailList;
      this.selectedPackage = (PickerItem) null;
      this.selectedCustomer = (pIOGetAlcPackageCustomersReturnModel) null;
      ((InputView) this.txtCustomerCode).Text = "";
      ((InputView) this.txtDesc).Text = "";
      ((VisualElement) this.txtDesc).IsVisible = false;
      this.pckPackages.ItemsSource = (IList) null;
      ((VisualElement) this.txtBarcode).IsVisible = false;
      ((VisualElement) this.pckPackages).IsVisible = false;
      ((VisualElement) this.btnCompleted).IsVisible = false;
    }

    private void RefreshData()
    {
      this.detailList = this.detailList.OrderByDescending<pIOPurchasePackageDetailsReturnModel, bool>((Func<pIOPurchasePackageDetailsReturnModel, bool>) (x => x.LastReadBarcode)).ToList<pIOPurchasePackageDetailsReturnModel>();
      ((ItemsView<Cell>) this.lstPackages).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstPackages).ItemsSource = (IEnumerable) this.detailList;
      int num1 = this.detailList.Sum<pIOPurchasePackageDetailsReturnModel>((Func<pIOPurchasePackageDetailsReturnModel, int>) (x => x.ApproveQty));
      int num2 = this.detailList.Sum<pIOPurchasePackageDetailsReturnModel>((Func<pIOPurchasePackageDetailsReturnModel, int>) (x => x.OrderQty));
      if (((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Count <= 0)
        return;
      ((MenuItem) ((Page) this).ToolbarItems[0]).Text = num1.ToString() + "/" + num2.ToString();
    }

    private void BarcodeFocus() => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(100);
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode)?.Focus();
    }));

    private async void btnCompleted_Clicked(object sender, EventArgs e)
    {
      AGAlcOrderApprove page = this;
      if (page.selectedPackage == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Müşteriye ait koli bulunamadı", "", "Tamam") ? 1 : 0;
        page.BarcodeFocus();
      }
      else
      {
        if (!await ((Page) page).DisplayAlert("Onay?", "Onaylamak istiyor musunuz", "Evet", "Hayır"))
          return;
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("UpdatePackageIsCompleted?packageAllocateID={0}", (object) page.selectedPackage.Code), (ContentPage) page);
        if (!returnModel.Success || !JsonConvert.DeserializeObject<bool>(returnModel.Result))
          return;
        page.ClearForm();
      }
    }

    private async void txtBarcode_Completed(object sender, EventArgs e)
    {
      AGAlcOrderApprove page = this;
      string barcode = ((InputView) page.txtBarcode).Text;
      if (string.IsNullOrEmpty(barcode))
        return;
      if (page.selectedPackage == null)
      {
        GlobalMob.PlayError();
        int num = await ((Page) page).DisplayAlert("Bilgi", "Lütfen koli seçiniz", "", "Tamam") ? 1 : 0;
        page.BarcodeFocus();
      }
      else
      {
        pIOPurchasePackageDetailsReturnModel detailsReturnModel = page.detailList.Where<pIOPurchasePackageDetailsReturnModel>((Func<pIOPurchasePackageDetailsReturnModel, bool>) (x => x.Barcode.Contains(barcode) && x.ApproveQty < x.OrderQty)).FirstOrDefault<pIOPurchasePackageDetailsReturnModel>();
        if (detailsReturnModel == null)
        {
          GlobalMob.PlayError();
          int num = await ((Page) page).DisplayAlert("Bilgi", "Ürün bulunamadı", "", "Tamam") ? 1 : 0;
          page.BarcodeFocus();
        }
        else
        {
          ReturnModel returnModel = GlobalMob.PostJson(string.Format("UpdatePackageDetailApprove?detailID={0}&userName={1}", (object) detailsReturnModel.PackageAllocateDetailID, (object) GlobalMob.User.UserName), (ContentPage) page);
          if (!returnModel.Success || !JsonConvert.DeserializeObject<bool>(returnModel.Result))
            return;
          GlobalMob.PlaySave();
          page.detailList.Select<pIOPurchasePackageDetailsReturnModel, pIOPurchasePackageDetailsReturnModel>((Func<pIOPurchasePackageDetailsReturnModel, pIOPurchasePackageDetailsReturnModel>) (c =>
          {
            c.LastReadBarcode = false;
            return c;
          })).ToList<pIOPurchasePackageDetailsReturnModel>();
          ++detailsReturnModel.ApproveQty;
          detailsReturnModel.LastReadBarcode = true;
          page.RefreshData();
          page.BarcodeFocus();
        }
      }
    }

    private void pckPackages_SelectedIndexChanged(object sender, EventArgs e)
    {
      PickerItem selectedItem = (PickerItem) this.pckPackages.SelectedItem;
      if (selectedItem == null)
        return;
      this.selectedPackage = selectedItem;
      ((VisualElement) this.txtDesc).IsVisible = true;
      ((InputView) this.txtDesc).Text = selectedItem.Description;
      this.LoadDetails(selectedItem.Code);
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (AGAlcOrderApprove).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/AGAlcOrderApprove.xaml",
        Instance = (object) this
      }))
        this.__InitComponentRuntime();
      else if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(((object) this).GetType()) != null)
      {
        this.__InitComponentRuntime();
      }
      else
      {
        Xamarin.Forms.Entry entry1 = new Xamarin.Forms.Entry();
        StackLayout stackLayout1 = new StackLayout();
        BindingExtension bindingExtension1 = new BindingExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        Picker picker = new Picker();
        StackLayout stackLayout2 = new StackLayout();
        Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
        StackLayout stackLayout3 = new StackLayout();
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension3 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension4 = new BindingExtension();
        Button button = new Button();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry = new SoftkeyboardDisabledEntry();
        StackLayout stackLayout4 = new StackLayout();
        BindingExtension bindingExtension5 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout5 = new StackLayout();
        AGAlcOrderApprove agAlcOrderApprove;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (agAlcOrderApprove = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) agAlcOrderApprove, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("approve", (object) agAlcOrderApprove);
        if (((Element) agAlcOrderApprove).StyleId == null)
          ((Element) agAlcOrderApprove).StyleId = "approve";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout5);
        if (((Element) stackLayout5).StyleId == null)
          ((Element) stackLayout5).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtCustomerCode", (object) entry1);
        if (((Element) entry1).StyleId == null)
          ((Element) entry1).StyleId = "txtCustomerCode";
        ((INameScope) nameScope).RegisterName("stckPackageBarcode", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckPackageBarcode";
        ((INameScope) nameScope).RegisterName("pckPackages", (object) picker);
        if (((Element) picker).StyleId == null)
          ((Element) picker).StyleId = "pckPackages";
        ((INameScope) nameScope).RegisterName("txtDesc", (object) entry2);
        if (((Element) entry2).StyleId == null)
          ((Element) entry2).StyleId = "txtDesc";
        ((INameScope) nameScope).RegisterName("btnCompleted", (object) button);
        if (((Element) button).StyleId == null)
          ((Element) button).StyleId = "btnCompleted";
        ((INameScope) nameScope).RegisterName("stckBarcode", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckBarcode";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry);
        if (((Element) softkeyboardDisabledEntry).StyleId == null)
          ((Element) softkeyboardDisabledEntry).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("lstPackages", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstPackages";
        this.approve = (ContentPage) agAlcOrderApprove;
        this.stckForm = stackLayout5;
        this.txtCustomerCode = entry1;
        this.stckPackageBarcode = stackLayout2;
        this.pckPackages = picker;
        this.txtDesc = entry2;
        this.btnCompleted = button;
        this.stckBarcode = stackLayout4;
        this.txtBarcode = softkeyboardDisabledEntry;
        this.lstPackages = listView;
        ((BindableObject) stackLayout5).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Müşteri Seçiniz");
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((VisualElement) entry1).Focused += new EventHandler<FocusEventArgs>(agAlcOrderApprove.txtCustomerCode_Focused);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\AGAlcOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\AGAlcOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 14);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) picker).SetValue(Picker.TitleProperty, (object) "Koli Seçiniz");
        bindingExtension1.Path = ".";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker).SetBinding(Picker.ItemsSourceProperty, bindingBase1);
        ((BindableObject) picker).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        picker.SelectedIndexChanged += new EventHandler(agAlcOrderApprove.pckPackages_SelectedIndexChanged);
        bindingExtension2.Path = "Caption";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        picker.ItemDisplayBinding = bindingBase2;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase2, new Uri("Views\\AGAlcOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 33);
        ((BindableObject) picker).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) picker);
        VisualDiagnostics.RegisterSourceInfo((object) picker, new Uri("Views\\AGAlcOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\AGAlcOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 14);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Tedarikçi");
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\AGAlcOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 24, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\AGAlcOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 14);
        ((BindableObject) button).SetValue(Button.TextProperty, (object) "Tamamla");
        button.Clicked += new EventHandler(agAlcOrderApprove.btnCompleted_Clicked);
        referenceExtension1.Name = "approve";
        ReferenceExtension referenceExtension3 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 4];
        objArray1[0] = (object) bindingExtension3;
        objArray1[1] = (object) button;
        objArray1[2] = (object) stackLayout5;
        objArray1[3] = (object) agAlcOrderApprove;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver1.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (AGAlcOrderApprove).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(27, 21)));
        object obj2 = ((IMarkupExtension) referenceExtension3).ProvideValue((IServiceProvider) xamlServiceProvider1);
        bindingExtension3.Source = obj2;
        VisualDiagnostics.RegisterSourceInfo(obj2, new Uri("Views\\AGAlcOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 27, 21);
        bindingExtension3.Path = "ButtonColor";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) button).SetBinding(VisualElement.BackgroundColorProperty, bindingBase3);
        referenceExtension2.Name = "approve";
        ReferenceExtension referenceExtension4 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 4];
        objArray2[0] = (object) bindingExtension4;
        objArray2[1] = (object) button;
        objArray2[2] = (object) stackLayout5;
        objArray2[3] = (object) agAlcOrderApprove;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray2, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver2.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver2.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver2.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (AGAlcOrderApprove).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(27, 91)));
        object obj4 = ((IMarkupExtension) referenceExtension4).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension4.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\AGAlcOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 27, 91);
        bindingExtension4.Path = "TextColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button).SetBinding(Button.TextColorProperty, bindingBase4);
        ((BindableObject) button).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) button);
        VisualDiagnostics.RegisterSourceInfo((object) button, new Uri("Views\\AGAlcOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 26, 14);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod Okutunuz");
        ((BindableObject) softkeyboardDisabledEntry).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        softkeyboardDisabledEntry.Completed += new EventHandler(agAlcOrderApprove.txtBarcode_Completed);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) softkeyboardDisabledEntry);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry, new Uri("Views\\AGAlcOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 30, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\AGAlcOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 29, 14);
        bindingExtension5.Path = ".";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase5);
        ((BindableObject) listView).SetValue(ListView.RowHeightProperty, (object) 120);
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 5000.0);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AGAlcOrderApprove.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_3 xamlCdataTemplate3 = new AGAlcOrderApprove.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_3();
        object[] objArray3 = new object[0 + 4];
        objArray3[0] = (object) dataTemplate1;
        objArray3[1] = (object) listView;
        objArray3[2] = (object) stackLayout5;
        objArray3[3] = (object) agAlcOrderApprove;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate3.parentValues = objArray3;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate3.root = agAlcOrderApprove;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate3.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\AGAlcOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 35, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\AGAlcOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 33, 14);
        ((BindableObject) agAlcOrderApprove).SetValue(ContentPage.ContentProperty, (object) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\AGAlcOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 10);
        VisualDiagnostics.RegisterSourceInfo((object) agAlcOrderApprove, new Uri("Views\\AGAlcOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Extensions.LoadFromXaml<AGAlcOrderApprove>(this, typeof (AGAlcOrderApprove));
      this.approve = NameScopeExtensions.FindByName<ContentPage>((Element) this, "approve");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtCustomerCode = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtCustomerCode");
      this.stckPackageBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckPackageBarcode");
      this.pckPackages = NameScopeExtensions.FindByName<Picker>((Element) this, "pckPackages");
      this.txtDesc = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtDesc");
      this.btnCompleted = NameScopeExtensions.FindByName<Button>((Element) this, "btnCompleted");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.lstPackages = NameScopeExtensions.FindByName<ListView>((Element) this, "lstPackages");
    }
  }
}
