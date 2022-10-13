// Decompiled with JetBrains decompiler
// Type: Shelf.Views.TrendyolCancel
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Shelf.Manager;
using Shelf.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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
  [XamlFilePath("Views\\TrendyolCancel.xaml")]
  public class TrendyolCancel : ContentPage
  {
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage trendyol;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckContent;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtDocumentNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Label lblStatus;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnTrendyol;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnClear;

    public Color ButtonColor => Color.FromRgb(142, 81, 152);

    public Color TextColor => Color.White;

    public TrendyolCancel()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Trendyol İptal";
      ((VisualElement) this.txtDocumentNumber).Focus();
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
    }

    private void txtDocumentNumber_Completed(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(((InputView) this.txtDocumentNumber).Text))
        return;
      this.Sorgula();
    }

    private async void Sorgula()
    {
      TrendyolCancel page = this;
      await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("TrendyolControl?documentNumber={0}", (object) ((InputView) page.txtDocumentNumber).Text), (ContentPage) page);
      GlobalMob.CloseLoading();
      if (returnModel.Success)
      {
        InvoiceResultModel invoiceResultModel = JsonConvert.DeserializeObject<InvoiceResultModel>(returnModel.Result);
        if (invoiceResultModel != null)
        {
          if (invoiceResultModel.Success)
          {
            page.lblStatus.Text = "Siparişte iptal yok";
            page.lblStatus.TextColor = Color.Green;
            GlobalMob.PlaySave();
          }
          else
          {
            page.lblStatus.Text = invoiceResultModel.ExceptionString;
            page.lblStatus.TextColor = Color.Red;
            GlobalMob.PlayError();
          }
          Label lblStatus = page.lblStatus;
          lblStatus.Text = lblStatus.Text + "\n" + invoiceResultModel.UnofficialInvoiceString;
        }
      }
      page.BarcodeFocus();
    }

    private async void BarcodeFocus()
    {
      await Task.Delay(100);
      ((InputView) this.txtDocumentNumber).Text = "";
      ((VisualElement) this.txtDocumentNumber)?.Focus();
    }

    private void btnTrendyol_Clicked(object sender, EventArgs e) => this.Sorgula();

    private void btnClear_Clicked(object sender, EventArgs e)
    {
      ((InputView) this.txtDocumentNumber).Text = "";
      ((VisualElement) this.txtDocumentNumber).Focus();
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (TrendyolCancel).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/TrendyolCancel.xaml",
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
        Label label1 = new Label();
        StackLayout stackLayout2 = new StackLayout();
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension1 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        Button button1 = new Button();
        StackLayout stackLayout3 = new StackLayout();
        ReferenceExtension referenceExtension3 = new ReferenceExtension();
        BindingExtension bindingExtension3 = new BindingExtension();
        ReferenceExtension referenceExtension4 = new ReferenceExtension();
        BindingExtension bindingExtension4 = new BindingExtension();
        Button button2 = new Button();
        StackLayout stackLayout4 = new StackLayout();
        StackLayout stackLayout5 = new StackLayout();
        TrendyolCancel trendyolCancel;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (trendyolCancel = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) trendyolCancel, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("trendyol", (object) trendyolCancel);
        if (((Element) trendyolCancel).StyleId == null)
          ((Element) trendyolCancel).StyleId = "trendyol";
        ((INameScope) nameScope).RegisterName("stckContent", (object) stackLayout5);
        if (((Element) stackLayout5).StyleId == null)
          ((Element) stackLayout5).StyleId = "stckContent";
        ((INameScope) nameScope).RegisterName("txtDocumentNumber", (object) softkeyboardDisabledEntry);
        if (((Element) softkeyboardDisabledEntry).StyleId == null)
          ((Element) softkeyboardDisabledEntry).StyleId = "txtDocumentNumber";
        ((INameScope) nameScope).RegisterName("lblStatus", (object) label1);
        if (((Element) label1).StyleId == null)
          ((Element) label1).StyleId = "lblStatus";
        ((INameScope) nameScope).RegisterName("btnTrendyol", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnTrendyol";
        ((INameScope) nameScope).RegisterName("btnClear", (object) button2);
        if (((Element) button2).StyleId == null)
          ((Element) button2).StyleId = "btnClear";
        this.trendyol = (ContentPage) trendyolCancel;
        this.stckContent = stackLayout5;
        this.txtDocumentNumber = softkeyboardDisabledEntry;
        this.lblStatus = label1;
        this.btnTrendyol = button1;
        this.btnClear = button2;
        ((BindableObject) stackLayout5).SetValue(Layout.PaddingProperty, (object) new Thickness(3.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Döküman Numarası Okutunuz.");
        ((BindableObject) softkeyboardDisabledEntry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        softkeyboardDisabledEntry.Completed += new EventHandler(trendyolCancel.txtDocumentNumber_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) softkeyboardDisabledEntry);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry, new Uri("Views\\TrendyolCancel.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\TrendyolCancel.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 14);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) label1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        Label label2 = label1;
        BindableProperty fontSizeProperty = Label.FontSizeProperty;
        FontSizeConverter fontSizeConverter = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 4];
        objArray1[0] = (object) label1;
        objArray1[1] = (object) stackLayout2;
        objArray1[2] = (object) stackLayout5;
        objArray1[3] = (object) trendyolCancel;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) Label.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver1.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (TrendyolCancel).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(17, 75)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider1);
        ((BindableObject) label2).SetValue(fontSizeProperty, obj2);
        ((BindableObject) label1).SetValue(VisualElement.HeightRequestProperty, (object) 300.0);
        ((BindableObject) label1).SetValue(VisualElement.WidthRequestProperty, (object) 200.0);
        ((BindableObject) label1).SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) label1).SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) label1).SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) label1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) label1);
        VisualDiagnostics.RegisterSourceInfo((object) label1, new Uri("Views\\TrendyolCancel.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\TrendyolCancel.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 14);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout3).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "Sorgula");
        button1.Clicked += new EventHandler(trendyolCancel.btnTrendyol_Clicked);
        ((BindableObject) button1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        referenceExtension1.Name = "trendyol";
        ReferenceExtension referenceExtension5 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 5];
        objArray2[0] = (object) bindingExtension1;
        objArray2[1] = (object) button1;
        objArray2[2] = (object) stackLayout3;
        objArray2[3] = (object) stackLayout5;
        objArray2[4] = (object) trendyolCancel;
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
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (TrendyolCancel).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(23, 21)));
        object obj4 = ((IMarkupExtension) referenceExtension5).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension1.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\TrendyolCancel.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 21);
        bindingExtension1.Path = "ButtonColor";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase1);
        referenceExtension2.Name = "trendyol";
        ReferenceExtension referenceExtension6 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 5];
        objArray3[0] = (object) bindingExtension2;
        objArray3[1] = (object) button1;
        objArray3[2] = (object) stackLayout3;
        objArray3[3] = (object) stackLayout5;
        objArray3[4] = (object) trendyolCancel;
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
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (TrendyolCancel).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(23, 92)));
        object obj6 = ((IMarkupExtension) referenceExtension6).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension2.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\TrendyolCancel.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 92);
        bindingExtension2.Path = "TextColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(Button.TextColorProperty, bindingBase2);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\TrendyolCancel.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 22, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\TrendyolCancel.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 21, 14);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout4).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) button2).SetValue(Button.TextProperty, (object) "Temizle");
        button2.Clicked += new EventHandler(trendyolCancel.btnClear_Clicked);
        ((BindableObject) button2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        referenceExtension3.Name = "trendyol";
        ReferenceExtension referenceExtension7 = referenceExtension3;
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 5];
        objArray4[0] = (object) bindingExtension3;
        objArray4[1] = (object) button2;
        objArray4[2] = (object) stackLayout4;
        objArray4[3] = (object) stackLayout5;
        objArray4[4] = (object) trendyolCancel;
        SimpleValueTargetProvider valueTargetProvider4;
        object obj7 = (object) (valueTargetProvider4 = new SimpleValueTargetProvider(objArray4, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider4.Add(type7, (object) valueTargetProvider4);
        xamlServiceProvider4.Add(typeof (IReferenceProvider), obj7);
        Type type8 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver4 = new XmlNamespaceResolver();
        namespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver4.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver4.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver4.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (TrendyolCancel).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(27, 21)));
        object obj8 = ((IMarkupExtension) referenceExtension7).ProvideValue((IServiceProvider) xamlServiceProvider4);
        bindingExtension3.Source = obj8;
        VisualDiagnostics.RegisterSourceInfo(obj8, new Uri("Views\\TrendyolCancel.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 27, 21);
        bindingExtension3.Path = "ButtonColor";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase3);
        referenceExtension4.Name = "trendyol";
        ReferenceExtension referenceExtension8 = referenceExtension4;
        XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
        Type type9 = typeof (IProvideValueTarget);
        object[] objArray5 = new object[0 + 5];
        objArray5[0] = (object) bindingExtension4;
        objArray5[1] = (object) button2;
        objArray5[2] = (object) stackLayout4;
        objArray5[3] = (object) stackLayout5;
        objArray5[4] = (object) trendyolCancel;
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
        XamlTypeResolver xamlTypeResolver5 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver5, typeof (TrendyolCancel).GetTypeInfo().Assembly);
        xamlServiceProvider5.Add(type10, (object) xamlTypeResolver5);
        xamlServiceProvider5.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(27, 92)));
        object obj10 = ((IMarkupExtension) referenceExtension8).ProvideValue((IServiceProvider) xamlServiceProvider5);
        bindingExtension4.Source = obj10;
        VisualDiagnostics.RegisterSourceInfo(obj10, new Uri("Views\\TrendyolCancel.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 27, 92);
        bindingExtension4.Path = "TextColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(Button.TextColorProperty, bindingBase4);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) button2);
        VisualDiagnostics.RegisterSourceInfo((object) button2, new Uri("Views\\TrendyolCancel.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 26, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\TrendyolCancel.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 25, 14);
        ((BindableObject) trendyolCancel).SetValue(ContentPage.ContentProperty, (object) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\TrendyolCancel.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 10);
        VisualDiagnostics.RegisterSourceInfo((object) trendyolCancel, new Uri("Views\\TrendyolCancel.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<TrendyolCancel>(this, typeof (TrendyolCancel));
      this.trendyol = NameScopeExtensions.FindByName<ContentPage>((Element) this, "trendyol");
      this.stckContent = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckContent");
      this.txtDocumentNumber = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtDocumentNumber");
      this.lblStatus = NameScopeExtensions.FindByName<Label>((Element) this, "lblStatus");
      this.btnTrendyol = NameScopeExtensions.FindByName<Button>((Element) this, "btnTrendyol");
      this.btnClear = NameScopeExtensions.FindByName<Button>((Element) this, "btnClear");
    }
  }
}
