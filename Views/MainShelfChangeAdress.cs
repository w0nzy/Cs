// Decompiled with JetBrains decompiler
// Type: Shelf.Views.MainShelfChangeAdress
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Shelf.Controls;
using Shelf.Manager;
using Shelf.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
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
  [XamlFilePath("Views\\MainShelfChangeAdress.xaml")]
  public class MainShelfChangeAdress : ContentPage
  {
    private ztIOShelf targetShelf;
    private ztIOShelf sourceShelf;
    private ShelfList pageShelf;
    private List<ztIOShelf> subShelfList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage mainShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtSourceShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnClearSource;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtTargetShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnClearTarget;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckAllPackage;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnAllPackage;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelfList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtPackageBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfList;

    public Color ButtonColor => Color.FromRgb(52, 203, 201);

    public Color TextColor => Color.White;

    public MainShelfChangeAdress()
    {
      this.InitializeComponent();
      GlobalMob.AddShelfBarcodeLongPress((Xamarin.Forms.Entry) this.txtSourceShelf);
      ((ICollection<Effect>) ((Element) this.txtTargetShelf).Effects).Add((Effect) new LongPressedEffect());
      LongPressedEffect.SetCommand((BindableObject) this.txtTargetShelf, this.LongPressShelf);
      ((Page) this).Title = "Adres Koli Taşı";
      Device.BeginInvokeOnMainThread((Action) (async () =>
      {
        await Task.Delay(250);
        ((VisualElement) this.txtSourceShelf)?.Focus();
      }));
    }

    private ICommand LongPressShelf => (ICommand) new Command((Action) (async () =>
    {
      MainShelfChangeAdress shelfChangeAdress = this;
      shelfChangeAdress.pageShelf = new ShelfList();
      shelfChangeAdress.pageShelf.ShelfSelectedItem += new EventHandler(shelfChangeAdress.PageShelf_ShelfSelectedItem);
      await ((NavigableElement) GlobalMob.currentPage).Navigation.PushAsync((Page) shelfChangeAdress.pageShelf);
    }));

    private void PageShelf_ShelfSelectedItem(object sender, EventArgs e)
    {
      pIOGetShelfFromTextReturnModel selectedShelf = this.pageShelf.selectedShelf;
      if (!string.IsNullOrEmpty(selectedShelf.Code))
      {
        ((InputView) this.txtTargetShelf).Text = selectedShelf.Code;
        ((IEntryController) this.txtTargetShelf).SendCompleted();
      }
      else
        ((Page) GlobalMob.currentPage).DisplayAlert("Bilgi", "Raf tanımlı değil", "", "Tamam");
    }

    private async void txtSourceShelf_Completed(object sender, EventArgs e)
    {
      MainShelfChangeAdress page = this;
      string text = ((InputView) page.txtSourceShelf).Text;
      if (string.IsNullOrEmpty(((InputView) page.txtSourceShelf).Text))
        return;
      ReturnModel returnModel1 = GlobalMob.PostJson(string.Format("GetMainShelf?shelfCode={0}", (object) text), (ContentPage) page);
      if (!returnModel1.Success || string.IsNullOrEmpty(returnModel1.Result))
        return;
      ReturnModel returnModel2 = JsonConvert.DeserializeObject<ReturnModel>(returnModel1.Result);
      if (returnModel2.Success)
      {
        List<ztIOShelf> source = JsonConvert.DeserializeObject<List<ztIOShelf>>(returnModel2.Result);
        page.sourceShelf = source.Where<ztIOShelf>((Func<ztIOShelf, bool>) (x => !x.MainShelfID.HasValue)).FirstOrDefault<ztIOShelf>();
        page.subShelfList = source.Where<ztIOShelf>((Func<ztIOShelf, bool>) (x => x.MainShelfID.HasValue)).ToList<ztIOShelf>();
        ((InputView) page.txtSourceShelf).Text = page.sourceShelf.Code;
        ((VisualElement) page.stckShelfList).IsVisible = true;
        ((VisualElement) page.stckAllPackage).IsVisible = true;
        page.RefreshDataSource();
        if (string.IsNullOrEmpty(((InputView) page.txtTargetShelf).Text))
        {
          // ISSUE: reference to a compiler-generated method
          Device.BeginInvokeOnMainThread(new Action(page.\u003CtxtSourceShelf_Completed\u003Eb__12_2));
        }
        else
          page.warehouseCodeControl();
      }
      else
      {
        ((ItemsView<Cell>) page.lstShelfList).ItemsSource = (IEnumerable) null;
        ((VisualElement) page.stckShelfList).IsVisible = false;
        page.sourceShelf = (ztIOShelf) null;
        int num = await ((Page) page).DisplayAlert("Hata", returnModel2.ErrorMessage, "", "Tamam") ? 1 : 0;
        page.SourceBarcodeFocus();
      }
    }

    private async void txtTargetShelf_Completed(object sender, EventArgs e)
    {
      MainShelfChangeAdress page = this;
      string text = ((InputView) page.txtTargetShelf).Text;
      if (string.IsNullOrEmpty(text))
        return;
      ReturnModel shelf = GlobalMob.GetShelf(text, (ContentPage) page);
      if (!shelf.Success)
        return;
      if (!string.IsNullOrEmpty(shelf.Result))
      {
        page.targetShelf = JsonConvert.DeserializeObject<ztIOShelf>(shelf.Result);
        if (page.targetShelf == null || page.targetShelf.MainShelfID.HasValue)
        {
          page.targetShelf = (ztIOShelf) null;
          GlobalMob.PlayError();
          page.TargetBarcodeFocus();
          string str = page.targetShelf == null ? "Raf bulunamadı" : "Lütfen koli yerine adres rafı okutunuz";
          int num = await ((Page) page).DisplayAlert("Hata", str, "", "Tamam") ? 1 : 0;
        }
        else
        {
          if (!page.warehouseCodeControl())
            return;
          // ISSUE: reference to a compiler-generated method
          Device.BeginInvokeOnMainThread(new Action(page.\u003CtxtTargetShelf_Completed\u003Eb__13_0));
        }
      }
      else
      {
        int num = await ((Page) page).DisplayAlert("Hata", "Raf Bulunamadı", "", "Tamam") ? 1 : 0;
        page.TargetBarcodeFocus();
      }
    }

    private bool warehouseCodeControl()
    {
      if (this.targetShelf == null || this.sourceShelf == null || !(this.targetShelf.WarehouseCode != this.sourceShelf.WarehouseCode))
        return true;
      GlobalMob.PlayError();
      string str = "Hedef ve kaynak depo kodları farklı.\nHedef Depo Kodu:" + this.sourceShelf.WarehouseCode + "\nKaynak Depo Kodu:" + this.targetShelf.WarehouseCode;
      this.targetShelf = (ztIOShelf) null;
      ((Page) this).DisplayAlert("Hata", str, "", "Tamam");
      this.TargetBarcodeFocus();
      return false;
    }

    private void TargetBarcodeFocus()
    {
      ((InputView) this.txtTargetShelf).Text = "";
      ((VisualElement) this.txtTargetShelf).Focus();
    }

    private void SourceBarcodeFocus()
    {
      ((InputView) this.txtSourceShelf).Text = "";
      ((VisualElement) this.txtSourceShelf).Focus();
    }

    private void RefreshDataSource()
    {
      ((ItemsView<Cell>) this.lstShelfList).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstShelfList).ItemsSource = (IEnumerable) this.subShelfList;
    }

    private async void btnAllPackage_Clicked(object sender, EventArgs e)
    {
      MainShelfChangeAdress page = this;
      MainShelfChangeAdressProp prop;
      if (page.sourceShelf == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Kaynak Raf Okutunuz", "", "Tamam") ? 1 : 0;
        page.SourceBarcodeFocus();
        prop = (MainShelfChangeAdressProp) null;
      }
      else if (page.targetShelf == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Hedef Raf Okutunuz", "", "Tamam") ? 1 : 0;
        page.TargetBarcodeFocus();
        prop = (MainShelfChangeAdressProp) null;
      }
      else
      {
        prop = new MainShelfChangeAdressProp()
        {
          NewShelfID = page.targetShelf.ShelfID,
          OldShelfID = page.sourceShelf.ShelfID,
          UserName = GlobalMob.User.UserName
        };
        await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
        ReturnModel result = GlobalMob.PostJson(nameof (MainShelfChangeAdress), new Dictionary<string, string>()
        {
          {
            "json",
            JsonConvert.SerializeObject((object) prop)
          }
        }, (ContentPage) page).Result;
        if (result.Success)
        {
          ReturnModel returnModel = JsonConvert.DeserializeObject<ReturnModel>(result.Result);
          if (returnModel.Success)
          {
            GlobalMob.PlaySave();
            ((IEntryController) page.txtSourceShelf).SendCompleted();
          }
          else
          {
            GlobalMob.PlayError();
            int num = await ((Page) page).DisplayAlert("Hata", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
            page.PackageBarcodeFocus();
          }
        }
        GlobalMob.CloseLoading();
        prop = (MainShelfChangeAdressProp) null;
      }
    }

    private async void txtPackageBarcode_Completed(object sender, EventArgs e)
    {
      MainShelfChangeAdress page = this;
      string barcode = ((InputView) page.txtPackageBarcode).Text;
      if (string.IsNullOrEmpty(barcode))
        return;
      if (page.sourceShelf == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Kaynak Raf Okutunuz", "", "Tamam") ? 1 : 0;
        ((InputView) page.txtPackageBarcode).Text = "";
        page.SourceBarcodeFocus();
      }
      else if (page.targetShelf == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Hedef Raf Okutunuz", "", "Tamam") ? 1 : 0;
        ((InputView) page.txtPackageBarcode).Text = "";
        page.TargetBarcodeFocus();
      }
      else if (page.subShelfList.Where<ztIOShelf>((Func<ztIOShelf, bool>) (x => x.Code == barcode)).FirstOrDefault<ztIOShelf>() == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Koli kaynak rafta bulunamadı." + barcode, "", "Tamam") ? 1 : 0;
        page.PackageBarcodeFocus();
      }
      else
      {
        ReturnModel result = GlobalMob.PostJson("MainShelfChange", new Dictionary<string, string>()
        {
          {
            "json",
            JsonConvert.SerializeObject((object) new MainShelfChangeProp()
            {
              NewMainShelfID = page.targetShelf.ShelfID,
              SubShelfCode = barcode,
              UserName = GlobalMob.User.UserName
            })
          }
        }, (ContentPage) page).Result;
        if (!result.Success)
          return;
        ReturnModel returnModel = JsonConvert.DeserializeObject<ReturnModel>(result.Result);
        if (returnModel.Success)
        {
          GlobalMob.PlaySave();
          ztIOShelf subShelf = JsonConvert.DeserializeObject<ztIOShelf>(returnModel.Result);
          ztIOShelf ztIoShelf = page.subShelfList.Where<ztIOShelf>((Func<ztIOShelf, bool>) (x => x.Code == subShelf.Code)).FirstOrDefault<ztIOShelf>();
          if (ztIoShelf != null)
            page.subShelfList.Remove(ztIoShelf);
          page.RefreshDataSource();
          page.PackageBarcodeFocus();
        }
        else
        {
          GlobalMob.PlayError();
          int num = await ((Page) page).DisplayAlert("Hata", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
          page.PackageBarcodeFocus();
        }
      }
    }

    private void PackageBarcodeFocus()
    {
      ((InputView) this.txtPackageBarcode).Text = "";
      ((VisualElement) this.txtPackageBarcode).Focus();
    }

    private void btnClearSource_Clicked(object sender, EventArgs e)
    {
      this.sourceShelf = (ztIOShelf) null;
      Device.BeginInvokeOnMainThread((Action) (async () =>
      {
        await Task.Delay(250);
        this.SourceBarcodeFocus();
      }));
    }

    private void btnClearTarget_Clicked(object sender, EventArgs e)
    {
      this.targetShelf = (ztIOShelf) null;
      Device.BeginInvokeOnMainThread((Action) (async () =>
      {
        await Task.Delay(250);
        this.TargetBarcodeFocus();
      }));
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (MainShelfChangeAdress).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/MainShelfChangeAdress.xaml",
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
        StackLayout stackLayout3 = new StackLayout();
        ReferenceExtension referenceExtension5 = new ReferenceExtension();
        BindingExtension bindingExtension5 = new BindingExtension();
        ReferenceExtension referenceExtension6 = new ReferenceExtension();
        BindingExtension bindingExtension6 = new BindingExtension();
        Button button3 = new Button();
        StackLayout stackLayout4 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry3 = new SoftkeyboardDisabledEntry();
        StackLayout stackLayout5 = new StackLayout();
        BindingExtension bindingExtension7 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout6 = new StackLayout();
        StackLayout stackLayout7 = new StackLayout();
        MainShelfChangeAdress shelfChangeAdress;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (shelfChangeAdress = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) shelfChangeAdress, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("mainShelf", (object) shelfChangeAdress);
        if (((Element) shelfChangeAdress).StyleId == null)
          ((Element) shelfChangeAdress).StyleId = "mainShelf";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout3);
        if (((Element) stackLayout3).StyleId == null)
          ((Element) stackLayout3).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtSourceShelf", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtSourceShelf";
        ((INameScope) nameScope).RegisterName("btnClearSource", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnClearSource";
        ((INameScope) nameScope).RegisterName("txtTargetShelf", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtTargetShelf";
        ((INameScope) nameScope).RegisterName("btnClearTarget", (object) button2);
        if (((Element) button2).StyleId == null)
          ((Element) button2).StyleId = "btnClearTarget";
        ((INameScope) nameScope).RegisterName("stckAllPackage", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckAllPackage";
        ((INameScope) nameScope).RegisterName("btnAllPackage", (object) button3);
        if (((Element) button3).StyleId == null)
          ((Element) button3).StyleId = "btnAllPackage";
        ((INameScope) nameScope).RegisterName("stckShelfList", (object) stackLayout6);
        if (((Element) stackLayout6).StyleId == null)
          ((Element) stackLayout6).StyleId = "stckShelfList";
        ((INameScope) nameScope).RegisterName("stckBarcode", (object) stackLayout5);
        if (((Element) stackLayout5).StyleId == null)
          ((Element) stackLayout5).StyleId = "stckBarcode";
        ((INameScope) nameScope).RegisterName("txtPackageBarcode", (object) softkeyboardDisabledEntry3);
        if (((Element) softkeyboardDisabledEntry3).StyleId == null)
          ((Element) softkeyboardDisabledEntry3).StyleId = "txtPackageBarcode";
        ((INameScope) nameScope).RegisterName("lstShelfList", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstShelfList";
        this.mainShelf = (ContentPage) shelfChangeAdress;
        this.stckForm = stackLayout3;
        this.txtSourceShelf = softkeyboardDisabledEntry1;
        this.btnClearSource = button1;
        this.txtTargetShelf = softkeyboardDisabledEntry2;
        this.btnClearTarget = button2;
        this.stckAllPackage = stackLayout4;
        this.btnAllPackage = button3;
        this.stckShelfList = stackLayout6;
        this.stckBarcode = stackLayout5;
        this.txtPackageBarcode = softkeyboardDisabledEntry3;
        this.lstShelfList = listView;
        ((BindableObject) stackLayout7).SetValue(View.MarginProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Kaynak Raf Okutunuz");
        softkeyboardDisabledEntry1.Completed += new EventHandler(shelfChangeAdress.txtSourceShelf_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 22);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "x");
        ((BindableObject) button1).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Button button4 = button1;
        BindableProperty fontSizeProperty1 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter1 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 5];
        objArray1[0] = (object) button1;
        objArray1[1] = (object) stackLayout1;
        objArray1[2] = (object) stackLayout3;
        objArray1[3] = (object) stackLayout7;
        objArray1[4] = (object) shelfChangeAdress;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver1.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (MainShelfChangeAdress).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(16, 121)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter1).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider1);
        ((BindableObject) button4).SetValue(fontSizeProperty1, obj2);
        button1.Clicked += new EventHandler(shelfChangeAdress.btnClearSource_Clicked);
        referenceExtension1.Name = "mainShelf";
        ReferenceExtension referenceExtension7 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 6];
        objArray2[0] = (object) bindingExtension1;
        objArray2[1] = (object) button1;
        objArray2[2] = (object) stackLayout1;
        objArray2[3] = (object) stackLayout3;
        objArray2[4] = (object) stackLayout7;
        objArray2[5] = (object) shelfChangeAdress;
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
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (MainShelfChangeAdress).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(17, 25)));
        object obj4 = ((IMarkupExtension) referenceExtension7).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension1.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 25);
        bindingExtension1.Path = "ButtonColor";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase1);
        referenceExtension2.Name = "mainShelf";
        ReferenceExtension referenceExtension8 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 6];
        objArray3[0] = (object) bindingExtension2;
        objArray3[1] = (object) button1;
        objArray3[2] = (object) stackLayout1;
        objArray3[3] = (object) stackLayout3;
        objArray3[4] = (object) stackLayout7;
        objArray3[5] = (object) shelfChangeAdress;
        SimpleValueTargetProvider valueTargetProvider3;
        object obj5 = (object) (valueTargetProvider3 = new SimpleValueTargetProvider(objArray3, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider3.Add(type5, (object) valueTargetProvider3);
        xamlServiceProvider3.Add(typeof (IReferenceProvider), obj5);
        Type type6 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver3 = new XmlNamespaceResolver();
        namespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver3.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver3.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver3.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (MainShelfChangeAdress).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(17, 97)));
        object obj6 = ((IMarkupExtension) referenceExtension8).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension2.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 97);
        bindingExtension2.Path = "TextColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(Button.TextColorProperty, bindingBase2);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Hedef Raf Okutunuz");
        softkeyboardDisabledEntry2.Completed += new EventHandler(shelfChangeAdress.txtTargetShelf_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 22);
        ((BindableObject) button2).SetValue(Button.TextProperty, (object) "x");
        ((BindableObject) button2).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button2).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        ((BindableObject) button2).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Button button5 = button2;
        BindableProperty fontSizeProperty2 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter2 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 5];
        objArray4[0] = (object) button2;
        objArray4[1] = (object) stackLayout2;
        objArray4[2] = (object) stackLayout3;
        objArray4[3] = (object) stackLayout7;
        objArray4[4] = (object) shelfChangeAdress;
        SimpleValueTargetProvider valueTargetProvider4;
        object obj7 = (object) (valueTargetProvider4 = new SimpleValueTargetProvider(objArray4, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider4.Add(type7, (object) valueTargetProvider4);
        xamlServiceProvider4.Add(typeof (IReferenceProvider), obj7);
        Type type8 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver4 = new XmlNamespaceResolver();
        namespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver4.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver4.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver4.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (MainShelfChangeAdress).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(22, 121)));
        object obj8 = ((IExtendedTypeConverter) fontSizeConverter2).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider4);
        ((BindableObject) button5).SetValue(fontSizeProperty2, obj8);
        button2.Clicked += new EventHandler(shelfChangeAdress.btnClearTarget_Clicked);
        referenceExtension3.Name = "mainShelf";
        ReferenceExtension referenceExtension9 = referenceExtension3;
        XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
        Type type9 = typeof (IProvideValueTarget);
        object[] objArray5 = new object[0 + 6];
        objArray5[0] = (object) bindingExtension3;
        objArray5[1] = (object) button2;
        objArray5[2] = (object) stackLayout2;
        objArray5[3] = (object) stackLayout3;
        objArray5[4] = (object) stackLayout7;
        objArray5[5] = (object) shelfChangeAdress;
        SimpleValueTargetProvider valueTargetProvider5;
        object obj9 = (object) (valueTargetProvider5 = new SimpleValueTargetProvider(objArray5, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider5.Add(type9, (object) valueTargetProvider5);
        xamlServiceProvider5.Add(typeof (IReferenceProvider), obj9);
        Type type10 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver5 = new XmlNamespaceResolver();
        namespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver5.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver5.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver5.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver5 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver5, typeof (MainShelfChangeAdress).GetTypeInfo().Assembly);
        xamlServiceProvider5.Add(type10, (object) xamlTypeResolver5);
        xamlServiceProvider5.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(23, 25)));
        object obj10 = ((IMarkupExtension) referenceExtension9).ProvideValue((IServiceProvider) xamlServiceProvider5);
        bindingExtension3.Source = obj10;
        VisualDiagnostics.RegisterSourceInfo(obj10, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 25);
        bindingExtension3.Path = "ButtonColor";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase3);
        referenceExtension4.Name = "mainShelf";
        ReferenceExtension referenceExtension10 = referenceExtension4;
        XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
        Type type11 = typeof (IProvideValueTarget);
        object[] objArray6 = new object[0 + 6];
        objArray6[0] = (object) bindingExtension4;
        objArray6[1] = (object) button2;
        objArray6[2] = (object) stackLayout2;
        objArray6[3] = (object) stackLayout3;
        objArray6[4] = (object) stackLayout7;
        objArray6[5] = (object) shelfChangeAdress;
        SimpleValueTargetProvider valueTargetProvider6;
        object obj11 = (object) (valueTargetProvider6 = new SimpleValueTargetProvider(objArray6, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider6.Add(type11, (object) valueTargetProvider6);
        xamlServiceProvider6.Add(typeof (IReferenceProvider), obj11);
        Type type12 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver6 = new XmlNamespaceResolver();
        namespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver6.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver6.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver6.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver6 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver6, typeof (MainShelfChangeAdress).GetTypeInfo().Assembly);
        xamlServiceProvider6.Add(type12, (object) xamlTypeResolver6);
        xamlServiceProvider6.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(23, 97)));
        object obj12 = ((IMarkupExtension) referenceExtension10).ProvideValue((IServiceProvider) xamlServiceProvider6);
        bindingExtension4.Source = obj12;
        VisualDiagnostics.RegisterSourceInfo(obj12, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 97);
        bindingExtension4.Path = "TextColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(Button.TextColorProperty, bindingBase4);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) button2);
        VisualDiagnostics.RegisterSourceInfo((object) button2, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 22, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 19, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 14);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout4).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) button3).SetValue(Button.TextProperty, (object) "Tüm Kolileri Hedefe Taşı");
        button3.Clicked += new EventHandler(shelfChangeAdress.btnAllPackage_Clicked);
        ((BindableObject) button3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        referenceExtension5.Name = "mainShelf";
        ReferenceExtension referenceExtension11 = referenceExtension5;
        XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
        Type type13 = typeof (IProvideValueTarget);
        object[] objArray7 = new object[0 + 5];
        objArray7[0] = (object) bindingExtension5;
        objArray7[1] = (object) button3;
        objArray7[2] = (object) stackLayout4;
        objArray7[3] = (object) stackLayout7;
        objArray7[4] = (object) shelfChangeAdress;
        SimpleValueTargetProvider valueTargetProvider7;
        object obj13 = (object) (valueTargetProvider7 = new SimpleValueTargetProvider(objArray7, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider7.Add(type13, (object) valueTargetProvider7);
        xamlServiceProvider7.Add(typeof (IReferenceProvider), obj13);
        Type type14 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver7 = new XmlNamespaceResolver();
        namespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver7.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver7.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver7.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver7 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver7, typeof (MainShelfChangeAdress).GetTypeInfo().Assembly);
        xamlServiceProvider7.Add(type14, (object) xamlTypeResolver7);
        xamlServiceProvider7.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(28, 21)));
        object obj14 = ((IMarkupExtension) referenceExtension11).ProvideValue((IServiceProvider) xamlServiceProvider7);
        bindingExtension5.Source = obj14;
        VisualDiagnostics.RegisterSourceInfo(obj14, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 21);
        bindingExtension5.Path = "ButtonColor";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(VisualElement.BackgroundColorProperty, bindingBase5);
        referenceExtension6.Name = "mainShelf";
        ReferenceExtension referenceExtension12 = referenceExtension6;
        XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
        Type type15 = typeof (IProvideValueTarget);
        object[] objArray8 = new object[0 + 5];
        objArray8[0] = (object) bindingExtension6;
        objArray8[1] = (object) button3;
        objArray8[2] = (object) stackLayout4;
        objArray8[3] = (object) stackLayout7;
        objArray8[4] = (object) shelfChangeAdress;
        SimpleValueTargetProvider valueTargetProvider8;
        object obj15 = (object) (valueTargetProvider8 = new SimpleValueTargetProvider(objArray8, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider8.Add(type15, (object) valueTargetProvider8);
        xamlServiceProvider8.Add(typeof (IReferenceProvider), obj15);
        Type type16 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver8 = new XmlNamespaceResolver();
        namespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver8.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver8.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver8.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver8 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver8, typeof (MainShelfChangeAdress).GetTypeInfo().Assembly);
        xamlServiceProvider8.Add(type16, (object) xamlTypeResolver8);
        xamlServiceProvider8.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(28, 93)));
        object obj16 = ((IMarkupExtension) referenceExtension12).ProvideValue((IServiceProvider) xamlServiceProvider8);
        bindingExtension6.Source = obj16;
        VisualDiagnostics.RegisterSourceInfo(obj16, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 93);
        bindingExtension6.Path = "TextColor";
        BindingBase bindingBase6 = ((IMarkupExtension<BindingBase>) bindingExtension6).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(Button.TextColorProperty, bindingBase6);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) button3);
        VisualDiagnostics.RegisterSourceInfo((object) button3, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 27, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 26, 14);
        ((BindableObject) stackLayout6).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout6).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout6).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) stackLayout5).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry3).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Koli barkodu okutunuz");
        softkeyboardDisabledEntry3.Completed += new EventHandler(shelfChangeAdress.txtPackageBarcode_Completed);
        ((BindableObject) softkeyboardDisabledEntry3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) softkeyboardDisabledEntry3);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry3, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 32, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 31, 18);
        ((BindableObject) listView).SetValue(ListView.RowHeightProperty, (object) 50);
        bindingExtension7.Path = ".";
        BindingBase bindingBase7 = ((IMarkupExtension<BindingBase>) bindingExtension7).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase7);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MainShelfChangeAdress.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_11 xamlCdataTemplate11 = new MainShelfChangeAdress.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_11();
        object[] objArray9 = new object[0 + 5];
        objArray9[0] = (object) dataTemplate1;
        objArray9[1] = (object) listView;
        objArray9[2] = (object) stackLayout6;
        objArray9[3] = (object) stackLayout7;
        objArray9[4] = (object) shelfChangeAdress;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate11.parentValues = objArray9;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate11.root = shelfChangeAdress;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate11.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 37, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 35, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 30, 14);
        ((BindableObject) shelfChangeAdress).SetValue(ContentPage.ContentProperty, (object) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 10);
        VisualDiagnostics.RegisterSourceInfo((object) shelfChangeAdress, new Uri("Views\\MainShelfChangeAdress.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<MainShelfChangeAdress>(this, typeof (MainShelfChangeAdress));
      this.mainShelf = NameScopeExtensions.FindByName<ContentPage>((Element) this, "mainShelf");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtSourceShelf = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtSourceShelf");
      this.btnClearSource = NameScopeExtensions.FindByName<Button>((Element) this, "btnClearSource");
      this.txtTargetShelf = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtTargetShelf");
      this.btnClearTarget = NameScopeExtensions.FindByName<Button>((Element) this, "btnClearTarget");
      this.stckAllPackage = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckAllPackage");
      this.btnAllPackage = NameScopeExtensions.FindByName<Button>((Element) this, "btnAllPackage");
      this.stckShelfList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfList");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.txtPackageBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtPackageBarcode");
      this.lstShelfList = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfList");
    }
  }
}
