// Decompiled with JetBrains decompiler
// Type: Shelf.Views.PendingShelfOrder
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
using System.Reflection;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace Shelf.Views
{
  [XamlCompilation]
  [XamlFilePath("Views\\PendingShelfOrder.xaml")]
  public class PendingShelfOrder : ContentPage
  {
    private List<pIOPendingShelfOrdersReturnModel> shelfOrderList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage pending;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckContent;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelfOrderList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckSearch;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtSearch;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ImageButton imgSearch;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnClear;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckEmptyMessage;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfOrder;

    public Color ButtonColor => Color.FromRgb(52, 203, 201);

    public Color TextColor => Color.White;

    public PendingShelfOrder()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Bekleyen Emirler";
      this.LoadData();
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
    }

    private void imgSearch_Clicked(object sender, EventArgs e) => this.LoadData();

    private void txtSearch_Completed(object sender, EventArgs e) => this.LoadData();

    private void btnClear_Clicked(object sender, EventArgs e)
    {
      ((InputView) this.txtSearch).Text = "";
      ((IEntryController) this.txtSearch).SendCompleted();
    }

    private void LoadData()
    {
      NavigationExtension.PushPopupAsync(((NavigableElement) this).Navigation, GlobalMob.ShowLoading(), true);
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetPendingShelfOrders?searchText={0}", (object) ((InputView) this.txtSearch).Text), (ContentPage) this);
      if (returnModel.Success)
      {
        this.shelfOrderList = GlobalMob.JsonDeserialize<List<pIOPendingShelfOrdersReturnModel>>(returnModel.Result);
        ((ItemsView<Cell>) this.lstShelfOrder).ItemsSource = (IEnumerable) null;
        ((ItemsView<Cell>) this.lstShelfOrder).ItemsSource = (IEnumerable) this.shelfOrderList;
      }
      GlobalMob.CloseLoading();
    }

    private async void cmAssignUser_Clicked(object sender, EventArgs e)
    {
      PendingShelfOrder page = this;
      pIOPendingShelfOrdersReturnModel commandParameter = (pIOPendingShelfOrdersReturnModel) (sender as MenuItem).CommandParameter;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("AssingShelfOrder?shelfOrderID={0}&userID={1}", (object) commandParameter.ShelfOrderID, (object) GlobalMob.User.UserID), (ContentPage) page);
      if (!returnModel.Success)
        return;
      if (JsonConvert.DeserializeObject<ReturnModel>(returnModel.Result).Success)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Emir " + GlobalMob.User.UserName + " kullanıcısına atandı." + commandParameter.ShelfOrderNumber, "", "Tamam") ? 1 : 0;
        page.LoadData();
      }
      else
      {
        int num1 = await ((Page) page).DisplayAlert("Hata", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
      }
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (PendingShelfOrder).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/PendingShelfOrder.xaml",
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
        ImageButton imageButton = new ImageButton();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        Button button1 = new Button();
        StackLayout stackLayout1 = new StackLayout();
        Label label1 = new Label();
        StackLayout stackLayout2 = new StackLayout();
        BindingExtension bindingExtension3 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout3 = new StackLayout();
        StackLayout stackLayout4 = new StackLayout();
        PendingShelfOrder pendingShelfOrder;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (pendingShelfOrder = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) pendingShelfOrder, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("pending", (object) pendingShelfOrder);
        if (((Element) pendingShelfOrder).StyleId == null)
          ((Element) pendingShelfOrder).StyleId = "pending";
        ((INameScope) nameScope).RegisterName("stckContent", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckContent";
        ((INameScope) nameScope).RegisterName("stckShelfOrderList", (object) stackLayout3);
        if (((Element) stackLayout3).StyleId == null)
          ((Element) stackLayout3).StyleId = "stckShelfOrderList";
        ((INameScope) nameScope).RegisterName("stckSearch", (object) stackLayout1);
        if (((Element) stackLayout1).StyleId == null)
          ((Element) stackLayout1).StyleId = "stckSearch";
        ((INameScope) nameScope).RegisterName("txtSearch", (object) entry);
        if (((Element) entry).StyleId == null)
          ((Element) entry).StyleId = "txtSearch";
        ((INameScope) nameScope).RegisterName("imgSearch", (object) imageButton);
        if (((Element) imageButton).StyleId == null)
          ((Element) imageButton).StyleId = "imgSearch";
        ((INameScope) nameScope).RegisterName("btnClear", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnClear";
        ((INameScope) nameScope).RegisterName("stckEmptyMessage", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckEmptyMessage";
        ((INameScope) nameScope).RegisterName("lstShelfOrder", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstShelfOrder";
        this.pending = (ContentPage) pendingShelfOrder;
        this.stckContent = stackLayout4;
        this.stckShelfOrderList = stackLayout3;
        this.stckSearch = stackLayout1;
        this.txtSearch = entry;
        this.imgSearch = imageButton;
        this.btnClear = button1;
        this.stckEmptyMessage = stackLayout2;
        this.lstShelfOrder = listView;
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout3).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout3).SetValue(View.MarginProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Ara");
        entry.Completed += new EventHandler(pendingShelfOrder.txtSearch_Completed);
        ((BindableObject) entry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) entry);
        VisualDiagnostics.RegisterSourceInfo((object) entry, new Uri("Views\\PendingShelfOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 22);
        ((BindableObject) imageButton).SetValue(ImageButton.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("search.png"));
        ((BindableObject) imageButton).SetValue(ImageButton.AspectProperty, (object) (Aspect) 0);
        ((BindableObject) imageButton).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) imageButton).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        ((BindableObject) imageButton).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        imageButton.Clicked += new EventHandler(pendingShelfOrder.imgSearch_Clicked);
        referenceExtension1.Name = "pending";
        ReferenceExtension referenceExtension3 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 6];
        objArray1[0] = (object) bindingExtension1;
        objArray1[1] = (object) imageButton;
        objArray1[2] = (object) stackLayout1;
        objArray1[3] = (object) stackLayout3;
        objArray1[4] = (object) stackLayout4;
        objArray1[5] = (object) pendingShelfOrder;
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
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (PendingShelfOrder).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(16, 54)));
        object obj2 = ((IMarkupExtension) referenceExtension3).ProvideValue((IServiceProvider) xamlServiceProvider1);
        bindingExtension1.Source = obj2;
        VisualDiagnostics.RegisterSourceInfo(obj2, new Uri("Views\\PendingShelfOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 54);
        bindingExtension1.Path = "ButtonColor";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) imageButton).SetBinding(VisualElement.BackgroundColorProperty, bindingBase1);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) imageButton);
        VisualDiagnostics.RegisterSourceInfo((object) imageButton, new Uri("Views\\PendingShelfOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 22);
        button1.Clicked += new EventHandler(pendingShelfOrder.btnClear_Clicked);
        ((BindableObject) button1).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        referenceExtension2.Name = "pending";
        ReferenceExtension referenceExtension4 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 6];
        objArray2[0] = (object) bindingExtension2;
        objArray2[1] = (object) button1;
        objArray2[2] = (object) stackLayout1;
        objArray2[3] = (object) stackLayout3;
        objArray2[4] = (object) stackLayout4;
        objArray2[5] = (object) pendingShelfOrder;
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
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (PendingShelfOrder).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(17, 101)));
        object obj4 = ((IMarkupExtension) referenceExtension4).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension2.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\PendingShelfOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 101);
        bindingExtension2.Path = "ButtonColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase2);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "X");
        ((BindableObject) button1).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        Button button2 = button1;
        BindableProperty fontSizeProperty1 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter1 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 5];
        objArray3[0] = (object) button1;
        objArray3[1] = (object) stackLayout1;
        objArray3[2] = (object) stackLayout3;
        objArray3[3] = (object) stackLayout4;
        objArray3[4] = (object) pendingShelfOrder;
        SimpleValueTargetProvider valueTargetProvider3;
        object obj5 = (object) (valueTargetProvider3 = new SimpleValueTargetProvider(objArray3, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider3.Add(type5, (object) valueTargetProvider3);
        xamlServiceProvider3.Add(typeof (IReferenceProvider), obj5);
        Type type6 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver3 = new XmlNamespaceResolver();
        namespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver3.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver3.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (PendingShelfOrder).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(18, 57)));
        object obj6 = ((IExtendedTypeConverter) fontSizeConverter1).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider3);
        ((BindableObject) button2).SetValue(fontSizeProperty1, obj6);
        ((BindableObject) button1).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(Button.TextColorProperty, (object) Color.White);
        ((BindableObject) button1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\PendingShelfOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\PendingShelfOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) stackLayout2).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) stackLayout2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) label1).SetValue(Label.TextProperty, (object) "Bekleyen Raf Emri Bulunmamaktadır.");
        ((BindableObject) label1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.CenterAndExpand);
        ((BindableObject) label1).SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Label label2 = label1;
        BindableProperty fontSizeProperty2 = Label.FontSizeProperty;
        FontSizeConverter fontSizeConverter2 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 5];
        objArray4[0] = (object) label1;
        objArray4[1] = (object) stackLayout2;
        objArray4[2] = (object) stackLayout3;
        objArray4[3] = (object) stackLayout4;
        objArray4[4] = (object) pendingShelfOrder;
        SimpleValueTargetProvider valueTargetProvider4;
        object obj7 = (object) (valueTargetProvider4 = new SimpleValueTargetProvider(objArray4, (object) Label.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider4.Add(type7, (object) valueTargetProvider4);
        xamlServiceProvider4.Add(typeof (IReferenceProvider), obj7);
        Type type8 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver4 = new XmlNamespaceResolver();
        namespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver4.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver4.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (PendingShelfOrder).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(22, 128)));
        object obj8 = ((IExtendedTypeConverter) fontSizeConverter2).ConvertFromInvariantString("Medium", (IServiceProvider) xamlServiceProvider4);
        ((BindableObject) label2).SetValue(fontSizeProperty2, obj8);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) label1);
        VisualDiagnostics.RegisterSourceInfo((object) label1, new Uri("Views\\PendingShelfOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 22, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\PendingShelfOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 21, 18);
        ((BindableObject) listView).SetValue(ListView.RowHeightProperty, (object) 80);
        bindingExtension3.Path = ".";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase3);
        ((BindableObject) listView).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        PendingShelfOrder.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_18 xamlCdataTemplate18 = new PendingShelfOrder.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_18();
        object[] objArray5 = new object[0 + 5];
        objArray5[0] = (object) dataTemplate1;
        objArray5[1] = (object) listView;
        objArray5[2] = (object) stackLayout3;
        objArray5[3] = (object) stackLayout4;
        objArray5[4] = (object) pendingShelfOrder;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate18.parentValues = objArray5;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate18.root = pendingShelfOrder;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate18.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\PendingShelfOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 26, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\PendingShelfOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 24, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\PendingShelfOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 14);
        ((BindableObject) pendingShelfOrder).SetValue(ContentPage.ContentProperty, (object) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\PendingShelfOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 10);
        VisualDiagnostics.RegisterSourceInfo((object) pendingShelfOrder, new Uri("Views\\PendingShelfOrder.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<PendingShelfOrder>(this, typeof (PendingShelfOrder));
      this.pending = NameScopeExtensions.FindByName<ContentPage>((Element) this, "pending");
      this.stckContent = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckContent");
      this.stckShelfOrderList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfOrderList");
      this.stckSearch = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckSearch");
      this.txtSearch = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtSearch");
      this.imgSearch = NameScopeExtensions.FindByName<ImageButton>((Element) this, "imgSearch");
      this.btnClear = NameScopeExtensions.FindByName<Button>((Element) this, "btnClear");
      this.stckEmptyMessage = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckEmptyMessage");
      this.lstShelfOrder = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfOrder");
    }
  }
}
