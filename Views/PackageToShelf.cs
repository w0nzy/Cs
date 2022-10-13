// Decompiled with JetBrains decompiler
// Type: Shelf.Views.PackageToShelf
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
  [XamlFilePath("Views\\PackageToShelf.xaml")]
  public class PackageToShelf : ContentPage
  {
    private ToolbarItem tItem;
    private ztIOShelf mkShelf;
    private ztIOShelf selectShelf;
    private List<pIOGetPackageInventoryReturnModel> packageDetails;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage package;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckContent;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtPackageNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnClearShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnClear;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnMkToShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelfList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstPackageList;

    public Color ButtonColor => Color.FromRgb(3, 10, 53);

    public Color TextColor => Color.White;

    public PackageToShelf()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Koli Taşıma";
      this.tItem = new ToolbarItem();
      this.mkShelf = new ztIOShelf();
      this.mkShelf.ShelfID = GlobalMob.User.MKShelfID;
      this.SetShelf();
      this.PackageNoFocus();
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
    }

    private void SetShelf()
    {
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfFromShelfID?shelfId={0}", (object) GlobalMob.User.MKShelfID), (ContentPage) this);
      if (!returnModel.Success || string.IsNullOrEmpty(returnModel.Result))
        return;
      this.mkShelf = JsonConvert.DeserializeObject<ztIOShelf>(returnModel.Result);
    }

    private async void txtPackageNumber_Completed(object sender, EventArgs e)
    {
      PackageToShelf page = this;
      if (string.IsNullOrEmpty(((InputView) page.txtPackageNumber).Text))
        return;
      await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetPackageInventory?packageBarcode={0}&shelfID={1}", (object) ((InputView) page.txtPackageNumber).Text, (object) page.mkShelf.ShelfID), (ContentPage) page);
      if (returnModel.Success)
      {
        page.packageDetails = GlobalMob.JsonDeserialize<List<pIOGetPackageInventoryReturnModel>>(returnModel.Result);
        ((ItemsView<Cell>) page.lstPackageList).ItemsSource = (IEnumerable) null;
        ((ItemsView<Cell>) page.lstPackageList).ItemsSource = page.GetGroupItem();
        string str = Convert.ToString((object) page.packageDetails.Sum<pIOGetPackageInventoryReturnModel>((Func<pIOGetPackageInventoryReturnModel, double?>) (x => x.InventoryQty)));
        ((MenuItem) page.tItem).Text = str;
        ((InputView) page.txtShelf).Text = "";
        ((VisualElement) page.txtShelf).Focus();
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
      PackageNumber = c.PackageNumber
    }).Select<IGrouping<\u003C\u003Ef__AnonymousType11<string, string, string, string, string, string>, pIOGetPackageInventoryReturnModel>, pIOGetPackageInventoryReturnModel>(gcs => new pIOGetPackageInventoryReturnModel()
    {
      PackageNumber = gcs.Key.PackageNumber,
      ItemCode = gcs.Key.ItemCode,
      ColorDescription = gcs.Key.ColorDescription,
      ColorCode = gcs.Key.ColorCode,
      ItemDim1Code = gcs.Key.ItemDim1Code,
      ItemDescription = gcs.Key.ItemDescription,
      InventoryQty = gcs.Sum<pIOGetPackageInventoryReturnModel>((Func<pIOGetPackageInventoryReturnModel, double?>) (x => x.InventoryQty))
    }).ToList<pIOGetPackageInventoryReturnModel>();

    private async void txtShelf_Completed(object sender, EventArgs e)
    {
      PackageToShelf page = this;
      ReturnModel shelf = GlobalMob.GetShelf(((InputView) page.txtShelf).Text, (ContentPage) page);
      if (!shelf.Success)
        return;
      page.selectShelf = JsonConvert.DeserializeObject<ztIOShelf>(shelf.Result);
      if (page.selectShelf != null)
      {
        ((InputView) page.txtShelf).Text = page.selectShelf.Code;
      }
      else
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Raf bulunamadı", "", "Tamam") ? 1 : 0;
      }
    }

    private void btnClearShelf_Clicked(object sender, EventArgs e) => ((InputView) this.txtPackageNumber).Text = "";

    private void PackageNoFocus()
    {
      ((InputView) this.txtPackageNumber).Text = "";
      ((VisualElement) this.txtPackageNumber).Focus();
    }

    private async void btnMkToShelf_Clicked(object sender, EventArgs e)
    {
      PackageToShelf page = this;
      if (page.selectShelf == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Hedef Rafı Okutunuz", "", "Tamam") ? 1 : 0;
        GlobalMob.PlayError();
        ((InputView) page.txtShelf).Text = "";
        ((VisualElement) page.txtShelf).Focus();
      }
      else if (string.IsNullOrEmpty(((InputView) page.txtPackageNumber).Text))
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Koli kodu boş olamaz", "", "Tamam") ? 1 : 0;
        GlobalMob.PlayError();
        page.PackageNoFocus();
      }
      else if (page.packageDetails == null || page.packageDetails.Count<pIOGetPackageInventoryReturnModel>() <= 0)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Aktarılacak ürün bulunamadı", "", "Tamam") ? 1 : 0;
        GlobalMob.PlayError();
        page.PackageNoFocus();
      }
      else
      {
        if (page.mkShelf.ShelfID <= 0)
          return;
        if (!await ((Page) page).DisplayAlert("Devam?", "Ürünler aktarılacak.Emin misiniz?", "Evet", "Hayır"))
          return;
        await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
        List<ShelfTransaction> shelfTransactionList = new List<ShelfTransaction>();
        foreach (pIOGetPackageInventoryReturnModel packageDetail in page.packageDetails)
        {
          shelfTransactionList.Add(new ShelfTransaction()
          {
            ShelfID = page.mkShelf.ShelfID,
            PackageNumber = packageDetail.PackageNumber,
            ProcessTypeID = 2,
            Qty = Convert.ToInt32((object) packageDetail.InventoryQty),
            WareHouseCode = page.mkShelf.WarehouseCode,
            Barcode = "",
            UserName = GlobalMob.User.UserName,
            TransTypeID = 18,
            DocumentNumber = "",
            ItemCode = packageDetail.ItemCode,
            ColorCode = packageDetail.ColorCode,
            ItemDim1Code = packageDetail.ItemDim1Code,
            ItemDim2Code = packageDetail.ItemDim2Code,
            ItemDim3Code = packageDetail.ItemDim3Code
          });
          shelfTransactionList.Add(new ShelfTransaction()
          {
            ShelfID = page.selectShelf.ShelfID,
            PackageNumber = packageDetail.PackageNumber,
            ProcessTypeID = 1,
            Qty = Convert.ToInt32((object) packageDetail.InventoryQty),
            WareHouseCode = page.selectShelf.WarehouseCode,
            Barcode = "",
            UserName = GlobalMob.User.UserName,
            TransTypeID = 18,
            DocumentNumber = "",
            ItemCode = packageDetail.ItemCode,
            ColorCode = packageDetail.ColorCode,
            ItemDim1Code = packageDetail.ItemDim1Code,
            ItemDim2Code = packageDetail.ItemDim2Code,
            ItemDim3Code = packageDetail.ItemDim3Code
          });
        }
        if (shelfTransactionList.Count > 0)
        {
          ReturnModel result = GlobalMob.PostJson("AddMKToShelf", new Dictionary<string, string>()
          {
            {
              "json",
              JsonConvert.SerializeObject((object) shelfTransactionList)
            }
          }, (ContentPage) page).Result;
          if (!result.Success)
            return;
          ReturnModel returnModel = JsonConvert.DeserializeObject<ReturnModel>(result.Result);
          if (returnModel.Success)
          {
            GlobalMob.CloseLoading();
            page.packageDetails = new List<pIOGetPackageInventoryReturnModel>();
            ((ItemsView<Cell>) page.lstPackageList).ItemsSource = (IEnumerable) null;
            ((MenuItem) page.tItem).Text = "";
            int num = await ((Page) page).DisplayAlert("Bilgi", "Ürünler rafa aktarıldı", "", "Tamam") ? 1 : 0;
            ((InputView) page.txtShelf).Text = "";
            page.PackageNoFocus();
          }
          else
          {
            GlobalMob.CloseLoading();
            int num = await ((Page) page).DisplayAlert("Bilgi", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
            page.PackageNoFocus();
          }
        }
        else
        {
          GlobalMob.CloseLoading();
          int num = await ((Page) page).DisplayAlert("Bilgi", "Aktarılacak Ürün bulunamadı", "", "Tamam") ? 1 : 0;
          GlobalMob.PlayError();
          page.PackageNoFocus();
        }
      }
    }

    private void btnClear_Clicked(object sender, EventArgs e)
    {
      this.selectShelf = (ztIOShelf) null;
      ((InputView) this.txtShelf).Text = "";
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (PackageToShelf).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/PackageToShelf.xaml",
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
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension1 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        Button button1 = new Button();
        StackLayout stackLayout1 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry2 = new SoftkeyboardDisabledEntry();
        ReferenceExtension referenceExtension3 = new ReferenceExtension();
        BindingExtension bindingExtension3 = new BindingExtension();
        ReferenceExtension referenceExtension4 = new ReferenceExtension();
        BindingExtension bindingExtension4 = new BindingExtension();
        Button button2 = new Button();
        StackLayout stackLayout2 = new StackLayout();
        ReferenceExtension referenceExtension5 = new ReferenceExtension();
        BindingExtension bindingExtension5 = new BindingExtension();
        ReferenceExtension referenceExtension6 = new ReferenceExtension();
        BindingExtension bindingExtension6 = new BindingExtension();
        Button button3 = new Button();
        StackLayout stackLayout3 = new StackLayout();
        BindingExtension bindingExtension7 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout4 = new StackLayout();
        StackLayout stackLayout5 = new StackLayout();
        PackageToShelf packageToShelf;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (packageToShelf = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) packageToShelf, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("package", (object) packageToShelf);
        if (((Element) packageToShelf).StyleId == null)
          ((Element) packageToShelf).StyleId = "package";
        ((INameScope) nameScope).RegisterName("stckContent", (object) stackLayout5);
        if (((Element) stackLayout5).StyleId == null)
          ((Element) stackLayout5).StyleId = "stckContent";
        ((INameScope) nameScope).RegisterName("txtPackageNumber", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtPackageNumber";
        ((INameScope) nameScope).RegisterName("btnClearShelf", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnClearShelf";
        ((INameScope) nameScope).RegisterName("txtShelf", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtShelf";
        ((INameScope) nameScope).RegisterName("btnClear", (object) button2);
        if (((Element) button2).StyleId == null)
          ((Element) button2).StyleId = "btnClear";
        ((INameScope) nameScope).RegisterName("btnMkToShelf", (object) button3);
        if (((Element) button3).StyleId == null)
          ((Element) button3).StyleId = "btnMkToShelf";
        ((INameScope) nameScope).RegisterName("stckShelfList", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckShelfList";
        ((INameScope) nameScope).RegisterName("lstPackageList", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstPackageList";
        this.package = (ContentPage) packageToShelf;
        this.stckContent = stackLayout5;
        this.txtPackageNumber = softkeyboardDisabledEntry1;
        this.btnClearShelf = button1;
        this.txtShelf = softkeyboardDisabledEntry2;
        this.btnClear = button2;
        this.btnMkToShelf = button3;
        this.stckShelfList = stackLayout4;
        this.lstPackageList = listView;
        ((BindableObject) stackLayout5).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Koli Okutunuz.");
        softkeyboardDisabledEntry1.Completed += new EventHandler(packageToShelf.txtPackageNumber_Completed);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "x");
        referenceExtension1.Name = "package";
        ReferenceExtension referenceExtension7 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 5];
        objArray1[0] = (object) bindingExtension1;
        objArray1[1] = (object) button1;
        objArray1[2] = (object) stackLayout1;
        objArray1[3] = (object) stackLayout5;
        objArray1[4] = (object) packageToShelf;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver1.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (PackageToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(16, 25)));
        object obj2 = ((IMarkupExtension) referenceExtension7).ProvideValue((IServiceProvider) xamlServiceProvider1);
        bindingExtension1.Source = obj2;
        VisualDiagnostics.RegisterSourceInfo(obj2, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 25);
        bindingExtension1.Path = "ButtonColor";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase1);
        referenceExtension2.Name = "package";
        ReferenceExtension referenceExtension8 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 5];
        objArray2[0] = (object) bindingExtension2;
        objArray2[1] = (object) button1;
        objArray2[2] = (object) stackLayout1;
        objArray2[3] = (object) stackLayout5;
        objArray2[4] = (object) packageToShelf;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray2, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver2.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver2.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver2.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (PackageToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(17, 25)));
        object obj4 = ((IMarkupExtension) referenceExtension8).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension2.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 25);
        bindingExtension2.Path = "TextColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(Button.TextColorProperty, bindingBase2);
        button1.Clicked += new EventHandler(packageToShelf.btnClearShelf_Clicked);
        ((BindableObject) button1).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 14);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Hedef Raf Okutunuz.");
        softkeyboardDisabledEntry2.Completed += new EventHandler(packageToShelf.txtShelf_Completed);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 21, 18);
        ((BindableObject) button2).SetValue(Button.TextProperty, (object) "x");
        referenceExtension3.Name = "package";
        ReferenceExtension referenceExtension9 = referenceExtension3;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 5];
        objArray3[0] = (object) bindingExtension3;
        objArray3[1] = (object) button2;
        objArray3[2] = (object) stackLayout2;
        objArray3[3] = (object) stackLayout5;
        objArray3[4] = (object) packageToShelf;
        SimpleValueTargetProvider valueTargetProvider3;
        object obj5 = (object) (valueTargetProvider3 = new SimpleValueTargetProvider(objArray3, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider3.Add(type5, (object) valueTargetProvider3);
        xamlServiceProvider3.Add(typeof (IReferenceProvider), obj5);
        Type type6 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver3 = new XmlNamespaceResolver();
        namespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver3.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver3.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver3.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (PackageToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(24, 25)));
        object obj6 = ((IMarkupExtension) referenceExtension9).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension3.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 24, 25);
        bindingExtension3.Path = "ButtonColor";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase3);
        referenceExtension4.Name = "package";
        ReferenceExtension referenceExtension10 = referenceExtension4;
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 5];
        objArray4[0] = (object) bindingExtension4;
        objArray4[1] = (object) button2;
        objArray4[2] = (object) stackLayout2;
        objArray4[3] = (object) stackLayout5;
        objArray4[4] = (object) packageToShelf;
        SimpleValueTargetProvider valueTargetProvider4;
        object obj7 = (object) (valueTargetProvider4 = new SimpleValueTargetProvider(objArray4, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider4.Add(type7, (object) valueTargetProvider4);
        xamlServiceProvider4.Add(typeof (IReferenceProvider), obj7);
        Type type8 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver4 = new XmlNamespaceResolver();
        namespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver4.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver4.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver4.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (PackageToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(25, 25)));
        object obj8 = ((IMarkupExtension) referenceExtension10).ProvideValue((IServiceProvider) xamlServiceProvider4);
        bindingExtension4.Source = obj8;
        VisualDiagnostics.RegisterSourceInfo(obj8, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 25, 25);
        bindingExtension4.Path = "TextColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(Button.TextColorProperty, bindingBase4);
        button2.Clicked += new EventHandler(packageToShelf.btnClear_Clicked);
        ((BindableObject) button2).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) button2);
        VisualDiagnostics.RegisterSourceInfo((object) button2, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 14);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) button3).SetValue(Button.TextProperty, (object) "Koliyi Rafa Aktar");
        referenceExtension5.Name = "package";
        ReferenceExtension referenceExtension11 = referenceExtension5;
        XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
        Type type9 = typeof (IProvideValueTarget);
        object[] objArray5 = new object[0 + 5];
        objArray5[0] = (object) bindingExtension5;
        objArray5[1] = (object) button3;
        objArray5[2] = (object) stackLayout3;
        objArray5[3] = (object) stackLayout5;
        objArray5[4] = (object) packageToShelf;
        SimpleValueTargetProvider valueTargetProvider5;
        object obj9 = (object) (valueTargetProvider5 = new SimpleValueTargetProvider(objArray5, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider5.Add(type9, (object) valueTargetProvider5);
        xamlServiceProvider5.Add(typeof (IReferenceProvider), obj9);
        Type type10 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver5 = new XmlNamespaceResolver();
        namespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver5.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver5.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver5.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        XamlTypeResolver xamlTypeResolver5 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver5, typeof (PackageToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider5.Add(type10, (object) xamlTypeResolver5);
        xamlServiceProvider5.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(30, 25)));
        object obj10 = ((IMarkupExtension) referenceExtension11).ProvideValue((IServiceProvider) xamlServiceProvider5);
        bindingExtension5.Source = obj10;
        VisualDiagnostics.RegisterSourceInfo(obj10, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 30, 25);
        bindingExtension5.Path = "ButtonColor";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(VisualElement.BackgroundColorProperty, bindingBase5);
        referenceExtension6.Name = "package";
        ReferenceExtension referenceExtension12 = referenceExtension6;
        XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
        Type type11 = typeof (IProvideValueTarget);
        object[] objArray6 = new object[0 + 5];
        objArray6[0] = (object) bindingExtension6;
        objArray6[1] = (object) button3;
        objArray6[2] = (object) stackLayout3;
        objArray6[3] = (object) stackLayout5;
        objArray6[4] = (object) packageToShelf;
        SimpleValueTargetProvider valueTargetProvider6;
        object obj11 = (object) (valueTargetProvider6 = new SimpleValueTargetProvider(objArray6, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider6.Add(type11, (object) valueTargetProvider6);
        xamlServiceProvider6.Add(typeof (IReferenceProvider), obj11);
        Type type12 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver6 = new XmlNamespaceResolver();
        namespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver6.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver6.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver6.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        XamlTypeResolver xamlTypeResolver6 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver6, typeof (PackageToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider6.Add(type12, (object) xamlTypeResolver6);
        xamlServiceProvider6.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(31, 25)));
        object obj12 = ((IMarkupExtension) referenceExtension12).ProvideValue((IServiceProvider) xamlServiceProvider6);
        bindingExtension6.Source = obj12;
        VisualDiagnostics.RegisterSourceInfo(obj12, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 31, 25);
        bindingExtension6.Path = "TextColor";
        BindingBase bindingBase6 = ((IMarkupExtension<BindingBase>) bindingExtension6).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(Button.TextColorProperty, bindingBase6);
        button3.Clicked += new EventHandler(packageToShelf.btnMkToShelf_Clicked);
        ((BindableObject) button3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) button3);
        VisualDiagnostics.RegisterSourceInfo((object) button3, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 29, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 14);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout4).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) listView).SetValue(ListView.RowHeightProperty, (object) 120);
        ((BindableObject) listView).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 800.0);
        bindingExtension7.Path = ".";
        BindingBase bindingBase7 = ((IMarkupExtension<BindingBase>) bindingExtension7).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase7);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        ((BindableObject) listView).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        PackageToShelf.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_15 xamlCdataTemplate15 = new PackageToShelf.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_15();
        object[] objArray7 = new object[0 + 5];
        objArray7[0] = (object) dataTemplate1;
        objArray7[1] = (object) listView;
        objArray7[2] = (object) stackLayout4;
        objArray7[3] = (object) stackLayout5;
        objArray7[4] = (object) packageToShelf;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate15.parentValues = objArray7;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate15.root = packageToShelf;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate15.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 39, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 35, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 34, 14);
        ((BindableObject) packageToShelf).SetValue(ContentPage.ContentProperty, (object) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 10);
        VisualDiagnostics.RegisterSourceInfo((object) packageToShelf, new Uri("Views\\PackageToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<PackageToShelf>(this, typeof (PackageToShelf));
      this.package = NameScopeExtensions.FindByName<ContentPage>((Element) this, "package");
      this.stckContent = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckContent");
      this.txtPackageNumber = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtPackageNumber");
      this.btnClearShelf = NameScopeExtensions.FindByName<Button>((Element) this, "btnClearShelf");
      this.txtShelf = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtShelf");
      this.btnClear = NameScopeExtensions.FindByName<Button>((Element) this, "btnClear");
      this.btnMkToShelf = NameScopeExtensions.FindByName<Button>((Element) this, "btnMkToShelf");
      this.stckShelfList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfList");
      this.lstPackageList = NameScopeExtensions.FindByName<ListView>((Element) this, "lstPackageList");
    }
  }
}
