// Decompiled with JetBrains decompiler
// Type: Shelf.Views.MainShelfChange
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using Newtonsoft.Json;
using Shelf.Controls;
using Shelf.Manager;
using Shelf.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
  [XamlFilePath("Views\\MainShelfChange.xaml")]
  public class MainShelfChange : ContentPage
  {
    private CustomShelf mainShelf;
    private List<CustomShelf> subShelfList;
    private ShelfList pageShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage mainshelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtMainShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnClearMainShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtSubShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelfList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfList;

    public Color ButtonColor => Color.FromRgb(52, 203, 201);

    public Color TextColor => Color.White;

    public MainShelfChange()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Koli Taşıma";
      GlobalMob.AddShelfBarcodeLongPress((Xamarin.Forms.Entry) this.txtMainShelf);
      ((ICollection<Effect>) ((Element) this.txtSubShelf).Effects).Add((Effect) new LongPressedEffect());
      LongPressedEffect.SetCommand((BindableObject) this.txtSubShelf, this.LongPressShelf);
    }

    private ICommand LongPressShelf => (ICommand) new Command((Action) (async () =>
    {
      MainShelfChange mainShelfChange = this;
      mainShelfChange.pageShelf = new ShelfList();
      mainShelfChange.pageShelf.ShelfSelectedItem += new EventHandler(mainShelfChange.PageShelf_ShelfSelectedItem);
      await ((NavigableElement) GlobalMob.currentPage).Navigation.PushAsync((Page) mainShelfChange.pageShelf);
    }));

    private void PageShelf_ShelfSelectedItem(object sender, EventArgs e)
    {
      pIOGetShelfFromTextReturnModel selectedShelf = this.pageShelf.selectedShelf;
      if (!string.IsNullOrEmpty(selectedShelf.Code))
      {
        ((InputView) this.txtSubShelf).Text = selectedShelf.Code;
        ((IEntryController) this.txtSubShelf).SendCompleted();
      }
      else
        ((Page) GlobalMob.currentPage).DisplayAlert("Bilgi", "Raf tanımlı değil", "", "Tamam");
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
      ((VisualElement) this.txtMainShelf).Focus();
    }

    private async void txtMainShelf_Completed(object sender, EventArgs e)
    {
      MainShelfChange page = this;
      if (string.IsNullOrEmpty(((InputView) page.txtMainShelf).Text))
        return;
      ReturnModel returnModel1 = GlobalMob.PostJson(string.Format("GetMainShelf?shelfCode={0}", (object) ((InputView) page.txtMainShelf).Text), (ContentPage) page);
      if (!returnModel1.Success || string.IsNullOrEmpty(returnModel1.Result))
        return;
      ReturnModel returnModel2 = JsonConvert.DeserializeObject<ReturnModel>(returnModel1.Result);
      if (returnModel2.Success)
      {
        List<CustomShelf> source = JsonConvert.DeserializeObject<List<CustomShelf>>(returnModel2.Result);
        page.mainShelf = source.Where<CustomShelf>((Func<CustomShelf, bool>) (x => !x.MainShelfID.HasValue)).FirstOrDefault<CustomShelf>();
        page.subShelfList = source.Where<CustomShelf>((Func<CustomShelf, bool>) (x => x.MainShelfID.HasValue)).ToList<CustomShelf>();
        ((InputView) page.txtMainShelf).Text = page.mainShelf.Code;
        page.RefreshDataSource();
      }
      else
      {
        int num = await ((Page) page).DisplayAlert("Hata", returnModel2.ErrorMessage, "", "Tamam") ? 1 : 0;
        ((InputView) page.txtMainShelf).Text = "";
        ((VisualElement) page.txtMainShelf).Focus();
      }
    }

    private void RefreshDataSource()
    {
      ((ItemsView<Cell>) this.lstShelfList).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstShelfList).ItemsSource = (IEnumerable) this.subShelfList.OrderByDescending<CustomShelf, bool>((Func<CustomShelf, bool>) (x => x.LastReadBarcode));
      this.BarcodeFocus();
    }

    private void BarcodeFocus()
    {
      ((InputView) this.txtSubShelf).Text = "";
      ((VisualElement) this.txtSubShelf).Focus();
    }

    private async void txtSubShelf_Completed(object sender, EventArgs e)
    {
      MainShelfChange page = this;
      if (string.IsNullOrEmpty(((InputView) page.txtMainShelf).Text) || page.mainShelf == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Hedef Raf Okutunuz", "", "Tamam") ? 1 : 0;
        page.BarcodeFocus();
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        if (page.subShelfList.Where<CustomShelf>(new Func<CustomShelf, bool>(page.\u003CtxtSubShelf_Completed\u003Eb__15_0)).Count<CustomShelf>() > 0)
        {
          int num = await ((Page) page).DisplayAlert("Bilgi", "Bu koli zaten bu adreste gözüküyor.", "", "Tamam") ? 1 : 0;
          page.BarcodeFocus();
        }
        else
        {
          ReturnModel result = GlobalMob.PostJson(nameof (MainShelfChange), new Dictionary<string, string>()
          {
            {
              "json",
              JsonConvert.SerializeObject((object) new MainShelfChangeProp()
              {
                NewMainShelfID = page.mainShelf.ShelfID,
                SubShelfCode = ((InputView) page.txtSubShelf).Text,
                UserName = GlobalMob.User.UserName,
                MainShelfWarehouseCode = page.mainShelf.WarehouseCode
              })
            }
          }, (ContentPage) page).Result;
          if (!result.Success)
            return;
          ReturnModel returnModel = JsonConvert.DeserializeObject<ReturnModel>(result.Result);
          if (returnModel.Success)
          {
            GlobalMob.PlaySave();
            CustomShelf customShelf = JsonConvert.DeserializeObject<CustomShelf>(returnModel.Result);
            page.subShelfList.Select<CustomShelf, CustomShelf>((Func<CustomShelf, CustomShelf>) (c =>
            {
              c.LastReadBarcode = false;
              return c;
            })).ToList<CustomShelf>();
            customShelf.LastReadBarcode = true;
            page.subShelfList.Add(customShelf);
            page.RefreshDataSource();
          }
          else
          {
            GlobalMob.PlayError();
            int num = await ((Page) page).DisplayAlert("Hata", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
            ((InputView) page.txtSubShelf).Text = "";
            ((VisualElement) page.txtSubShelf).Focus();
          }
        }
      }
    }

    private void btnClearMainShelf_Clicked(object sender, EventArgs e)
    {
      ((InputView) this.txtMainShelf).Text = "";
      ((VisualElement) this.txtMainShelf).Focus();
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (MainShelfChange).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/MainShelfChange.xaml",
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
        StackLayout stackLayout2 = new StackLayout();
        StackLayout stackLayout3 = new StackLayout();
        BindingExtension bindingExtension3 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout4 = new StackLayout();
        StackLayout stackLayout5 = new StackLayout();
        MainShelfChange mainShelfChange;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (mainShelfChange = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) mainShelfChange, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("mainshelf", (object) mainShelfChange);
        if (((Element) mainShelfChange).StyleId == null)
          ((Element) mainShelfChange).StyleId = "mainshelf";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout3);
        if (((Element) stackLayout3).StyleId == null)
          ((Element) stackLayout3).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtMainShelf", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtMainShelf";
        ((INameScope) nameScope).RegisterName("btnClearMainShelf", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnClearMainShelf";
        ((INameScope) nameScope).RegisterName("txtSubShelf", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtSubShelf";
        ((INameScope) nameScope).RegisterName("stckShelfList", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckShelfList";
        ((INameScope) nameScope).RegisterName("lstShelfList", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstShelfList";
        this.mainshelf = (ContentPage) mainShelfChange;
        this.stckForm = stackLayout3;
        this.txtMainShelf = softkeyboardDisabledEntry1;
        this.btnClearMainShelf = button1;
        this.txtSubShelf = softkeyboardDisabledEntry2;
        this.stckShelfList = stackLayout4;
        this.lstShelfList = listView;
        ((BindableObject) stackLayout5).SetValue(View.MarginProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Hedef Raf Okutunuz");
        softkeyboardDisabledEntry1.Completed += new EventHandler(mainShelfChange.txtMainShelf_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\MainShelfChange.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 22);
        ((BindableObject) button1).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "x");
        ((BindableObject) button1).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Button button2 = button1;
        BindableProperty fontSizeProperty = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 5];
        objArray1[0] = (object) button1;
        objArray1[1] = (object) stackLayout1;
        objArray1[2] = (object) stackLayout3;
        objArray1[3] = (object) stackLayout5;
        objArray1[4] = (object) mainShelfChange;
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
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (MainShelfChange).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(16, 124)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider1);
        ((BindableObject) button2).SetValue(fontSizeProperty, obj2);
        button1.Clicked += new EventHandler(mainShelfChange.btnClearMainShelf_Clicked);
        referenceExtension1.Name = "mainshelf";
        ReferenceExtension referenceExtension3 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 6];
        objArray2[0] = (object) bindingExtension1;
        objArray2[1] = (object) button1;
        objArray2[2] = (object) stackLayout1;
        objArray2[3] = (object) stackLayout3;
        objArray2[4] = (object) stackLayout5;
        objArray2[5] = (object) mainShelfChange;
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
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (MainShelfChange).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(17, 25)));
        object obj4 = ((IMarkupExtension) referenceExtension3).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension1.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\MainShelfChange.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 25);
        bindingExtension1.Path = "ButtonColor";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase1);
        referenceExtension2.Name = "mainshelf";
        ReferenceExtension referenceExtension4 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 6];
        objArray3[0] = (object) bindingExtension2;
        objArray3[1] = (object) button1;
        objArray3[2] = (object) stackLayout1;
        objArray3[3] = (object) stackLayout3;
        objArray3[4] = (object) stackLayout5;
        objArray3[5] = (object) mainShelfChange;
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
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (MainShelfChange).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(17, 97)));
        object obj6 = ((IMarkupExtension) referenceExtension4).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension2.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\MainShelfChange.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 97);
        bindingExtension2.Path = "TextColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(Button.TextColorProperty, bindingBase2);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\MainShelfChange.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\MainShelfChange.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Koli Okutunuz");
        softkeyboardDisabledEntry2.Completed += new EventHandler(mainShelfChange.txtSubShelf_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\MainShelfChange.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\MainShelfChange.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 19, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\MainShelfChange.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 14);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout4).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        bindingExtension3.Path = ".";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase3);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        ((BindableObject) listView).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MainShelfChange.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_10 xamlCdataTemplate10 = new MainShelfChange.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_10();
        object[] objArray4 = new object[0 + 5];
        objArray4[0] = (object) dataTemplate1;
        objArray4[1] = (object) listView;
        objArray4[2] = (object) stackLayout4;
        objArray4[3] = (object) stackLayout5;
        objArray4[4] = (object) mainShelfChange;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate10.parentValues = objArray4;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate10.root = mainShelfChange;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate10.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\MainShelfChange.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 27, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\MainShelfChange.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 25, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\MainShelfChange.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 24, 14);
        ((BindableObject) mainShelfChange).SetValue(ContentPage.ContentProperty, (object) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\MainShelfChange.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 10);
        VisualDiagnostics.RegisterSourceInfo((object) mainShelfChange, new Uri("Views\\MainShelfChange.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Extensions.LoadFromXaml<MainShelfChange>(this, typeof (MainShelfChange));
      this.mainshelf = NameScopeExtensions.FindByName<ContentPage>((Element) this, "mainshelf");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtMainShelf = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtMainShelf");
      this.btnClearMainShelf = NameScopeExtensions.FindByName<Button>((Element) this, "btnClearMainShelf");
      this.txtSubShelf = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtSubShelf");
      this.stckShelfList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfList");
      this.lstShelfList = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfList");
    }
  }
}
