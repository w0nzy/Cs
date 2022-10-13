// Decompiled with JetBrains decompiler
// Type: Shelf.Views.ProductionAcceptance
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
  [XamlFilePath("Views\\ProductionAcceptance.xaml")]
  public class ProductionAcceptance : ContentPage
  {
    private List<pIOGetItemsFromPackageBarcodeReturnModel> packageDetails;
    private ToolbarItem tItem;
    private ztIOShelf mkShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage accept;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckContent;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtPackageNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnClearShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelfList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstPackageList;

    public Color ButtonColor => Color.FromRgb(3, 10, 53);

    public Color TextColor => Color.White;

    public ProductionAcceptance()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Mal Kabul(Üretim)";
      this.tItem = new ToolbarItem();
      this.mkShelf = new ztIOShelf();
      this.mkShelf.ShelfID = GlobalMob.User.MKShelfID;
      this.SetShelf();
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(this.tItem);
      this.PackageBarcodeFocus();
    }

    private void PackageBarcodeFocus() => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(250);
      ((InputView) this.txtPackageNumber).Text = "";
      ((VisualElement) this.txtPackageNumber).Focus();
    }));

    private void SetShelf()
    {
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfFromShelfID?shelfId={0}", (object) GlobalMob.User.MKShelfID), (ContentPage) this);
      if (!returnModel.Success || string.IsNullOrEmpty(returnModel.Result))
        return;
      this.mkShelf = JsonConvert.DeserializeObject<ztIOShelf>(returnModel.Result);
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
    }

    private async void txtPackageNumber_Completed(object sender, EventArgs e)
    {
      ProductionAcceptance page = this;
      if (string.IsNullOrEmpty(((InputView) page.txtPackageNumber).Text))
        return;
      await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetItemsFromPackageBarcode?packageBarcode={0}", (object) ((InputView) page.txtPackageNumber).Text), (ContentPage) page);
      if (returnModel.Success)
      {
        page.packageDetails = GlobalMob.JsonDeserialize<List<pIOGetItemsFromPackageBarcodeReturnModel>>(returnModel.Result);
        if (page.packageDetails != null && page.packageDetails.Count > 0)
        {
          ((ItemsView<Cell>) page.lstPackageList).ItemsSource = (IEnumerable) null;
          ((ItemsView<Cell>) page.lstPackageList).ItemsSource = page.GetGroupItem();
          string str = Convert.ToString(page.packageDetails.Sum<pIOGetItemsFromPackageBarcodeReturnModel>((Func<pIOGetItemsFromPackageBarcodeReturnModel, double>) (x => x.Qty1)));
          ((MenuItem) page.tItem).Text = str;
          if (page.packageDetails.Count == 1)
            page.AddMK(page.packageDetails[0], false);
        }
        else
        {
          int num = await ((Page) page).DisplayAlert("Hata", "Koli bulunamadı", "", "Tamam") ? 1 : 0;
          page.PackageBarcodeFocus();
        }
      }
      GlobalMob.CloseLoading();
    }

    private IEnumerable GetGroupItem() => (IEnumerable) this.packageDetails.GroupBy(c => new
    {
      ItemCode = c.ItemCode,
      ItemDescription = c.ItemDescription,
      ColorCode = c.ColorCode,
      ColorDescription = c.ColorDescription,
      ItemDim1Code = c.ItemDim1Code,
      ItemDim2Code = c.ItemDim2Code,
      ItemDim3Code = c.ItemDim3Code,
      PackageNumber = c.PackageNumber,
      Description = c.Description,
      PlusOrderQty = c.PlusOrderQty,
      MinusOrderQty = c.MinusOrderQty
    }).Select<IGrouping<\u003C\u003Ef__AnonymousType17<string, string, string, string, string, string, string, string, string, int?, int?>, pIOGetItemsFromPackageBarcodeReturnModel>, pIOGetItemsFromPackageBarcodeReturnModel>(gcs => new pIOGetItemsFromPackageBarcodeReturnModel()
    {
      PackageNumber = gcs.Key.PackageNumber,
      ItemCode = gcs.Key.ItemCode,
      ColorDescription = gcs.Key.ColorDescription,
      ColorCode = gcs.Key.ColorCode,
      ItemDim1Code = gcs.Key.ItemDim1Code,
      ItemDim2Code = gcs.Key.ItemDim2Code,
      ItemDim3Code = gcs.Key.ItemDim3Code,
      ItemDescription = gcs.Key.ItemDescription,
      Description = gcs.Key.Description,
      Qty1 = gcs.Sum<pIOGetItemsFromPackageBarcodeReturnModel>((Func<pIOGetItemsFromPackageBarcodeReturnModel, double>) (x => x.Qty1)),
      PlusOrderQty = gcs.Key.PlusOrderQty,
      MinusOrderQty = gcs.Key.MinusOrderQty
    }).ToList<pIOGetItemsFromPackageBarcodeReturnModel>();

    private async void AddMK(pIOGetItemsFromPackageBarcodeReturnModel pItem, bool isAsk = true)
    {
      ProductionAcceptance page = this;
      List<pIOGetItemsFromPackageBarcodeReturnModel> packageDetailFilter = page.packageDetails.Where<pIOGetItemsFromPackageBarcodeReturnModel>((Func<pIOGetItemsFromPackageBarcodeReturnModel, bool>) (x => x.ItemCode == pItem.ItemCode && x.ColorCode == pItem.ColorCode && x.ItemDim1Code == pItem.ItemDim1Code)).ToList<pIOGetItemsFromPackageBarcodeReturnModel>();
      string pBarcode = ((InputView) page.txtPackageNumber).Text;
      if (string.IsNullOrEmpty(pBarcode))
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Koli kodu boş olamaz", "", "Tamam") ? 1 : 0;
        GlobalMob.PlayError();
        page.PackageNoFocus();
        packageDetailFilter = (List<pIOGetItemsFromPackageBarcodeReturnModel>) null;
        pBarcode = (string) null;
      }
      else if (packageDetailFilter == null || packageDetailFilter.Count<pIOGetItemsFromPackageBarcodeReturnModel>() <= 0)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Aktarılacak ürün bulunamadı", "", "Tamam") ? 1 : 0;
        GlobalMob.PlayError();
        page.PackageNoFocus();
        packageDetailFilter = (List<pIOGetItemsFromPackageBarcodeReturnModel>) null;
        pBarcode = (string) null;
      }
      else
      {
        int? shelfId = packageDetailFilter[0].ShelfID;
        int num1 = 0;
        if (shelfId.GetValueOrDefault() > num1 & shelfId.HasValue)
        {
          int num2 = await ((Page) page).DisplayAlert("Bilgi", "Bu koli daha önce aktarılmış", "", "Tamam") ? 1 : 0;
          GlobalMob.PlayError();
          page.PackageNoFocus();
          packageDetailFilter = (List<pIOGetItemsFromPackageBarcodeReturnModel>) null;
          pBarcode = (string) null;
        }
        else if (page.mkShelf.ShelfID <= 0)
        {
          packageDetailFilter = (List<pIOGetItemsFromPackageBarcodeReturnModel>) null;
          pBarcode = (string) null;
        }
        else
        {
          bool flag = true;
          if (isAsk)
            flag = await ((Page) page).DisplayAlert("Devam?", "Ürünler aktarılacak.Emin misiniz?", "Evet", "Hayır");
          if (!flag)
          {
            packageDetailFilter = (List<pIOGetItemsFromPackageBarcodeReturnModel>) null;
            pBarcode = (string) null;
          }
          else
          {
            string qty1 = await GlobalMob.InputBox(((NavigableElement) page).Navigation, "Miktar", "Miktar Giriniz", Keyboard.Numeric);
            if (!GlobalMob.IsNumeric(qty1))
            {
              packageDetailFilter = (List<pIOGetItemsFromPackageBarcodeReturnModel>) null;
              pBarcode = (string) null;
            }
            else
            {
              int qty = 0;
              int num3 = 0;
              try
              {
                qty = Convert.ToInt32(qty1);
              }
              catch (Exception ex)
              {
                num3 = 1;
              }
              if (num3 == 1)
              {
                int num4 = await ((Page) page).DisplayAlert("Bilgi", "Lütfen geçerli bir miktar giriniz", "", "Tamam") ? 1 : 0;
                GlobalMob.PlayError();
                page.PackageNoFocus();
                packageDetailFilter = (List<pIOGetItemsFromPackageBarcodeReturnModel>) null;
                pBarcode = (string) null;
              }
              else
              {
                double qty1_1 = pItem.Qty1;
                int? nullable1 = pItem.PlusOrderQty;
                double? nullable2 = nullable1.HasValue ? new double?((double) nullable1.GetValueOrDefault()) : new double?();
                int int32_1 = Convert.ToInt32((object) (nullable2.HasValue ? new double?(qty1_1 + nullable2.GetValueOrDefault()) : new double?()));
                double qty1_2 = pItem.Qty1;
                nullable1 = pItem.MinusOrderQty;
                double? nullable3 = nullable1.HasValue ? new double?((double) nullable1.GetValueOrDefault()) : new double?();
                int int32_2 = Convert.ToInt32((object) (nullable3.HasValue ? new double?(qty1_2 - nullable3.GetValueOrDefault()) : new double?()));
                if (qty < int32_2 || qty > int32_1)
                {
                  string str = "Girilen Sipariş miktarı hatalı:" + qty.ToString() + "\nSipariş Miktarı:" + pItem.Qty1.ToString() + "\nGirilebilecek Maksimum Sipariş Miktarı:" + int32_1.ToString() + "\nGirilebilecek Minimum Sipariş Miktarı:" + int32_2.ToString();
                  int num5 = await ((Page) page).DisplayAlert("Bilgi", str, "", "Tamam") ? 1 : 0;
                  GlobalMob.PlayError();
                  page.PackageNoFocus();
                  packageDetailFilter = (List<pIOGetItemsFromPackageBarcodeReturnModel>) null;
                  pBarcode = (string) null;
                }
                else
                {
                  await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
                  List<ShelfTransaction> shelfTransactionList = new List<ShelfTransaction>();
                  shelfTransactionList.Add(new ShelfTransaction()
                  {
                    ShelfID = page.mkShelf.ShelfID,
                    ProcessTypeID = 1,
                    Qty = qty,
                    WareHouseCode = page.mkShelf.WarehouseCode,
                    Barcode = "",
                    PackageNumber = packageDetailFilter[0].PackageNumber,
                    UserName = GlobalMob.User.UserName,
                    TransTypeID = 17,
                    DocumentNumber = "",
                    ItemCode = pItem.ItemCode,
                    ColorCode = pItem.ColorCode,
                    ItemDim1Code = pItem.ItemDim1Code,
                    ItemDim2Code = pItem.ItemDim2Code,
                    ItemDim3Code = pItem.ItemDim3Code
                  });
                  if (shelfTransactionList.Count > 0)
                  {
                    ReturnModel result = GlobalMob.PostJson(nameof (AddMK), new Dictionary<string, string>()
                    {
                      {
                        "json",
                        JsonConvert.SerializeObject((object) shelfTransactionList)
                      }
                    }, (ContentPage) page).Result;
                    if (!result.Success)
                    {
                      packageDetailFilter = (List<pIOGetItemsFromPackageBarcodeReturnModel>) null;
                      pBarcode = (string) null;
                    }
                    else
                    {
                      ReturnModel returnModel = JsonConvert.DeserializeObject<ReturnModel>(result.Result);
                      if (returnModel.Success)
                      {
                        GlobalMob.CloseLoading();
                        page.packageDetails = new List<pIOGetItemsFromPackageBarcodeReturnModel>();
                        ((ItemsView<Cell>) page.lstPackageList).ItemsSource = (IEnumerable) null;
                        ((MenuItem) page.tItem).Text = "";
                        int num6 = await ((Page) page).DisplayAlert("Bilgi", "Ürünler " + pBarcode + " numaralı koliye aktarıldı", "", "Tamam") ? 1 : 0;
                        page.PackageNoFocus();
                        packageDetailFilter = (List<pIOGetItemsFromPackageBarcodeReturnModel>) null;
                        pBarcode = (string) null;
                      }
                      else
                      {
                        GlobalMob.CloseLoading();
                        int num7 = await ((Page) page).DisplayAlert("Bilgi", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
                        page.PackageNoFocus();
                        packageDetailFilter = (List<pIOGetItemsFromPackageBarcodeReturnModel>) null;
                        pBarcode = (string) null;
                      }
                    }
                  }
                  else
                  {
                    GlobalMob.CloseLoading();
                    int num8 = await ((Page) page).DisplayAlert("Bilgi", "Aktarılacak Ürün bulunamadı", "", "Tamam") ? 1 : 0;
                    GlobalMob.PlayError();
                    page.PackageNoFocus();
                    packageDetailFilter = (List<pIOGetItemsFromPackageBarcodeReturnModel>) null;
                    pBarcode = (string) null;
                  }
                }
              }
            }
          }
        }
      }
    }

    private void PackageNoFocus()
    {
      ((InputView) this.txtPackageNumber).Text = "";
      ((VisualElement) this.txtPackageNumber).Focus();
    }

    private void btnClearShelf_Clicked(object sender, EventArgs e)
    {
      this.packageDetails = new List<pIOGetItemsFromPackageBarcodeReturnModel>();
      ((ItemsView<Cell>) this.lstPackageList).ItemsSource = (IEnumerable) null;
      this.PackageBarcodeFocus();
    }

    private void cmAktar_Clicked(object sender, EventArgs e) => this.AddMK((pIOGetItemsFromPackageBarcodeReturnModel) (sender as MenuItem).CommandParameter);

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (ProductionAcceptance).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/ProductionAcceptance.xaml",
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
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension1 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        Button button = new Button();
        StackLayout stackLayout1 = new StackLayout();
        StackLayout stackLayout2 = new StackLayout();
        BindingExtension bindingExtension3 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout3 = new StackLayout();
        StackLayout stackLayout4 = new StackLayout();
        ProductionAcceptance productionAcceptance;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (productionAcceptance = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) productionAcceptance, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("accept", (object) productionAcceptance);
        if (((Element) productionAcceptance).StyleId == null)
          ((Element) productionAcceptance).StyleId = "accept";
        ((INameScope) nameScope).RegisterName("stckContent", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckContent";
        ((INameScope) nameScope).RegisterName("txtPackageNumber", (object) softkeyboardDisabledEntry);
        if (((Element) softkeyboardDisabledEntry).StyleId == null)
          ((Element) softkeyboardDisabledEntry).StyleId = "txtPackageNumber";
        ((INameScope) nameScope).RegisterName("btnClearShelf", (object) button);
        if (((Element) button).StyleId == null)
          ((Element) button).StyleId = "btnClearShelf";
        ((INameScope) nameScope).RegisterName("stckShelfList", (object) stackLayout3);
        if (((Element) stackLayout3).StyleId == null)
          ((Element) stackLayout3).StyleId = "stckShelfList";
        ((INameScope) nameScope).RegisterName("lstPackageList", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstPackageList";
        this.accept = (ContentPage) productionAcceptance;
        this.stckContent = stackLayout2;
        this.txtPackageNumber = softkeyboardDisabledEntry;
        this.btnClearShelf = button;
        this.stckShelfList = stackLayout3;
        this.lstPackageList = listView;
        ((BindableObject) stackLayout2).SetValue(Layout.PaddingProperty, (object) new Thickness(3.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Koli Okutunuz.");
        softkeyboardDisabledEntry.Completed += new EventHandler(productionAcceptance.txtPackageNumber_Completed);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) softkeyboardDisabledEntry);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry, new Uri("Views\\ProductionAcceptance.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 22);
        ((BindableObject) button).SetValue(Button.TextProperty, (object) "x");
        referenceExtension1.Name = "accept";
        ReferenceExtension referenceExtension3 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 6];
        objArray1[0] = (object) bindingExtension1;
        objArray1[1] = (object) button;
        objArray1[2] = (object) stackLayout1;
        objArray1[3] = (object) stackLayout2;
        objArray1[4] = (object) stackLayout4;
        objArray1[5] = (object) productionAcceptance;
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
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (ProductionAcceptance).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(17, 25)));
        object obj2 = ((IMarkupExtension) referenceExtension3).ProvideValue((IServiceProvider) xamlServiceProvider1);
        bindingExtension1.Source = obj2;
        VisualDiagnostics.RegisterSourceInfo(obj2, new Uri("Views\\ProductionAcceptance.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 25);
        bindingExtension1.Path = "ButtonColor";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) button).SetBinding(VisualElement.BackgroundColorProperty, bindingBase1);
        referenceExtension2.Name = "accept";
        ReferenceExtension referenceExtension4 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 6];
        objArray2[0] = (object) bindingExtension2;
        objArray2[1] = (object) button;
        objArray2[2] = (object) stackLayout1;
        objArray2[3] = (object) stackLayout2;
        objArray2[4] = (object) stackLayout4;
        objArray2[5] = (object) productionAcceptance;
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
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (ProductionAcceptance).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(18, 25)));
        object obj4 = ((IMarkupExtension) referenceExtension4).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension2.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\ProductionAcceptance.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 18, 25);
        bindingExtension2.Path = "TextColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) button).SetBinding(Button.TextColorProperty, bindingBase2);
        button.Clicked += new EventHandler(productionAcceptance.btnClearShelf_Clicked);
        ((BindableObject) button).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button);
        VisualDiagnostics.RegisterSourceInfo((object) button, new Uri("Views\\ProductionAcceptance.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\ProductionAcceptance.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\ProductionAcceptance.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 14);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout3).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) listView).SetValue(ListView.RowHeightProperty, (object) 120);
        ((BindableObject) listView).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 800.0);
        bindingExtension3.Path = ".";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase3);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        ((BindableObject) listView).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ProductionAcceptance.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_22 xamlCdataTemplate22 = new ProductionAcceptance.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_22();
        object[] objArray3 = new object[0 + 5];
        objArray3[0] = (object) dataTemplate1;
        objArray3[1] = (object) listView;
        objArray3[2] = (object) stackLayout3;
        objArray3[3] = (object) stackLayout4;
        objArray3[4] = (object) productionAcceptance;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate22.parentValues = objArray3;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate22.root = productionAcceptance;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate22.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\ProductionAcceptance.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 27, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\ProductionAcceptance.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\ProductionAcceptance.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 22, 14);
        ((BindableObject) productionAcceptance).SetValue(ContentPage.ContentProperty, (object) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\ProductionAcceptance.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 10);
        VisualDiagnostics.RegisterSourceInfo((object) productionAcceptance, new Uri("Views\\ProductionAcceptance.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<ProductionAcceptance>(this, typeof (ProductionAcceptance));
      this.accept = NameScopeExtensions.FindByName<ContentPage>((Element) this, "accept");
      this.stckContent = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckContent");
      this.txtPackageNumber = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtPackageNumber");
      this.btnClearShelf = NameScopeExtensions.FindByName<Button>((Element) this, "btnClearShelf");
      this.stckShelfList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfList");
      this.lstPackageList = NameScopeExtensions.FindByName<ListView>((Element) this, "lstPackageList");
    }
  }
}
