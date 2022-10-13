// Decompiled with JetBrains decompiler
// Type: Shelf.Views.SetSolve
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
  [XamlFilePath("Views\\SetSolve.xaml")]
  public class SetSolve : ContentPage
  {
    private List<ShelfTransaction> allTransList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage setsolve;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckDocumentNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtDocumentNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnCreateDocumentNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfDetail;

    public Color ButtonColor => Color.FromRgb(52, 203, 201);

    public Color TextColor => Color.White;

    public ztIOShelf selectShelf { get; set; }

    public SetSolve()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Set Çöz";
      this.selectShelf = (ztIOShelf) null;
      this.allTransList = new List<ShelfTransaction>();
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
      ((VisualElement) this.txtShelf).Focus();
    }

    private async void txtShelf_Completed(object sender, EventArgs e)
    {
      SetSolve page = this;
      page.selectShelf = (ztIOShelf) null;
      if (string.IsNullOrEmpty(((InputView) page.txtShelf).Text))
        return;
      ReturnModel shelf = GlobalMob.GetShelf(((InputView) page.txtShelf).Text, (ContentPage) page);
      if (!shelf.Success)
        return;
      if (!string.IsNullOrEmpty(shelf.Result))
      {
        page.selectShelf = JsonConvert.DeserializeObject<ztIOShelf>(shelf.Result);
        ((InputView) page.txtShelf).Text = page.selectShelf.Description;
        ((VisualElement) page.stckBarcode).IsVisible = true;
        // ISSUE: reference to a compiler-generated method
        Device.BeginInvokeOnMainThread(new Action(page.\u003CtxtShelf_Completed\u003Eb__11_0));
      }
      else
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Hatalı Raf", "", "Tamam") ? 1 : 0;
        ((InputView) page.txtShelf).Text = "";
        ((VisualElement) page.txtShelf).Focus();
      }
    }

    private void BarcodeFocus()
    {
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode).Focus();
    }

    private async void txtBarcode_Completed_1(object sender, EventArgs e)
    {
      SetSolve page = this;
      if (page.selectShelf == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Öncelikle raf okutunuz", "", "Tamam") ? 1 : 0;
        ((InputView) page.txtShelf).Text = "";
        ((VisualElement) page.txtShelf).Focus();
      }
      if (string.IsNullOrEmpty(((InputView) page.txtBarcode).Text))
        return;
      ReturnModel result = GlobalMob.PostJson(nameof (SetSolve), new Dictionary<string, string>()
      {
        {
          "json",
          JsonConvert.SerializeObject((object) new ShelfTransaction()
          {
            ShelfID = page.selectShelf.ShelfID,
            ProcessTypeID = 1,
            WareHouseCode = page.selectShelf.WarehouseCode,
            Barcode = ((InputView) page.txtBarcode).Text,
            UserName = GlobalMob.User.UserName,
            Qty = 1,
            TransTypeID = 16,
            DocumentNumber = ((InputView) page.txtDocumentNumber).Text
          })
        }
      }, (ContentPage) page).Result;
      if (!result.Success)
        return;
      List<ShelfTransaction> collection = JsonConvert.DeserializeObject<List<ShelfTransaction>>(result.Result);
      if (collection == null)
      {
        GlobalMob.PlayError();
        int num = await ((Page) page).DisplayAlert("Bilgi", "Set rafta bulunamadı", "", "Tamam") ? 1 : 0;
        page.BarcodeFocus();
      }
      else
      {
        ((VisualElement) page.lstShelfDetail).IsVisible = true;
        page.allTransList.AddRange((IEnumerable<ShelfTransaction>) collection);
        page.allTransList = page.allTransList.OrderByDescending<ShelfTransaction, int>((Func<ShelfTransaction, int>) (x => x.ShelfTransID)).ToList<ShelfTransaction>();
        ((ItemsView<Cell>) page.lstShelfDetail).ItemsSource = (IEnumerable) null;
        ((ItemsView<Cell>) page.lstShelfDetail).ItemsSource = (IEnumerable) page.allTransList;
        page.BarcodeFocus();
      }
    }

    private string GetLastDocumentNumber()
    {
      int num = 0;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetLastDocumentNumber?transTypeID={0}", (object) 16), (ContentPage) this);
      if (returnModel.Success && !string.IsNullOrEmpty(returnModel.Result))
        num = JsonConvert.DeserializeObject<int>(returnModel.Result) + 1;
      return "S" + Convert.ToString(num);
    }

    private void btnCreateDocumentNumber_Clicked(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(((InputView) this.txtDocumentNumber).Text))
        ((InputView) this.txtDocumentNumber).Text = ((InputView) this.txtDocumentNumber).Text + "_";
      ((InputView) this.txtDocumentNumber).Text = this.GetLastDocumentNumber();
      ((VisualElement) this.txtShelf).Focus();
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (SetSolve).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/SetSolve.xaml",
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
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension1 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        Button button1 = new Button();
        StackLayout stackLayout1 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry1 = new SoftkeyboardDisabledEntry();
        StackLayout stackLayout2 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry2 = new SoftkeyboardDisabledEntry();
        StackLayout stackLayout3 = new StackLayout();
        BindingExtension bindingExtension3 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout4 = new StackLayout();
        SetSolve setSolve;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (setSolve = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) setSolve, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("setsolve", (object) setSolve);
        if (((Element) setSolve).StyleId == null)
          ((Element) setSolve).StyleId = "setsolve";
        ((INameScope) nameScope).RegisterName("stckDocumentNumber", (object) stackLayout1);
        if (((Element) stackLayout1).StyleId == null)
          ((Element) stackLayout1).StyleId = "stckDocumentNumber";
        ((INameScope) nameScope).RegisterName("txtDocumentNumber", (object) entry);
        if (((Element) entry).StyleId == null)
          ((Element) entry).StyleId = "txtDocumentNumber";
        ((INameScope) nameScope).RegisterName("btnCreateDocumentNumber", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnCreateDocumentNumber";
        ((INameScope) nameScope).RegisterName("stckShelf", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckShelf";
        ((INameScope) nameScope).RegisterName("txtShelf", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtShelf";
        ((INameScope) nameScope).RegisterName("stckBarcode", (object) stackLayout3);
        if (((Element) stackLayout3).StyleId == null)
          ((Element) stackLayout3).StyleId = "stckBarcode";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("lstShelfDetail", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstShelfDetail";
        this.setsolve = (ContentPage) setSolve;
        this.stckDocumentNumber = stackLayout1;
        this.txtDocumentNumber = entry;
        this.btnCreateDocumentNumber = button1;
        this.stckShelf = stackLayout2;
        this.txtShelf = softkeyboardDisabledEntry1;
        this.stckBarcode = stackLayout3;
        this.txtBarcode = softkeyboardDisabledEntry2;
        this.lstShelfDetail = listView;
        ((BindableObject) stackLayout4).SetValue(Layout.PaddingProperty, (object) new Thickness(3.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Döküman Numarası Giriniz");
        ((BindableObject) entry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) entry);
        VisualDiagnostics.RegisterSourceInfo((object) entry, new Uri("Views\\SetSolve.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "+");
        ((BindableObject) button1).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Button button2 = button1;
        BindableProperty fontSizeProperty = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 4];
        objArray1[0] = (object) button1;
        objArray1[1] = (object) stackLayout1;
        objArray1[2] = (object) stackLayout4;
        objArray1[3] = (object) setSolve;
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
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (SetSolve).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(14, 89)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider1);
        ((BindableObject) button2).SetValue(fontSizeProperty, obj2);
        button1.Clicked += new EventHandler(setSolve.btnCreateDocumentNumber_Clicked);
        referenceExtension1.Name = "setsolve";
        ReferenceExtension referenceExtension3 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 5];
        objArray2[0] = (object) bindingExtension1;
        objArray2[1] = (object) button1;
        objArray2[2] = (object) stackLayout1;
        objArray2[3] = (object) stackLayout4;
        objArray2[4] = (object) setSolve;
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
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (SetSolve).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(15, 25)));
        object obj4 = ((IMarkupExtension) referenceExtension3).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension1.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\SetSolve.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 25);
        bindingExtension1.Path = "ButtonColor";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase1);
        referenceExtension2.Name = "setsolve";
        ReferenceExtension referenceExtension4 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 5];
        objArray3[0] = (object) bindingExtension2;
        objArray3[1] = (object) button1;
        objArray3[2] = (object) stackLayout1;
        objArray3[3] = (object) stackLayout4;
        objArray3[4] = (object) setSolve;
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
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (SetSolve).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(15, 96)));
        object obj6 = ((IMarkupExtension) referenceExtension4).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension2.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\SetSolve.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 96);
        bindingExtension2.Path = "TextColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(Button.TextColorProperty, bindingBase2);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\SetSolve.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\SetSolve.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 14);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf No Giriniz/Okutunuz");
        softkeyboardDisabledEntry1.Completed += new EventHandler(setSolve.txtShelf_Completed);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\SetSolve.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 18, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\SetSolve.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 14);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout3).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod No Giriniz/Okutunuz");
        softkeyboardDisabledEntry2.Completed += new EventHandler(setSolve.txtBarcode_Completed_1);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\SetSolve.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\SetSolve.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 22, 14);
        bindingExtension3.Path = ".";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase3);
        ((BindableObject) listView).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) listView).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        ((BindableObject) listView).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 5000.0);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SetSolve.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_25 xamlCdataTemplate25 = new SetSolve.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_25();
        object[] objArray4 = new object[0 + 4];
        objArray4[0] = (object) dataTemplate1;
        objArray4[1] = (object) listView;
        objArray4[2] = (object) stackLayout4;
        objArray4[3] = (object) setSolve;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate25.parentValues = objArray4;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate25.root = setSolve;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate25.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\SetSolve.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 32, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\SetSolve.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 14);
        ((BindableObject) setSolve).SetValue(ContentPage.ContentProperty, (object) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\SetSolve.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 10);
        VisualDiagnostics.RegisterSourceInfo((object) setSolve, new Uri("Views\\SetSolve.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Extensions.LoadFromXaml<SetSolve>(this, typeof (SetSolve));
      this.setsolve = NameScopeExtensions.FindByName<ContentPage>((Element) this, "setsolve");
      this.stckDocumentNumber = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckDocumentNumber");
      this.txtDocumentNumber = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtDocumentNumber");
      this.btnCreateDocumentNumber = NameScopeExtensions.FindByName<Button>((Element) this, "btnCreateDocumentNumber");
      this.stckShelf = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelf");
      this.txtShelf = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtShelf");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.lstShelfDetail = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfDetail");
    }
  }
}
