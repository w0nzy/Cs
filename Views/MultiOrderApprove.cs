// Decompiled with JetBrains decompiler
// Type: Shelf.Views.MultiOrderApprove
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
  [XamlFilePath("Views\\MultiOrderApprove.xaml")]
  public class MultiOrderApprove : ContentPage
  {
    private List<pIOShelfOrderDetailForMultiOrderReturnModel> shelfOrderDetail;
    private List<pIOUserShelfOrdersForMultiOrderReturnModel> shelfOrderList;
    private pIOUserShelfOrdersForMultiOrderReturnModel selectedShelfOrder;
    private pIOShelfOrderDetailForMultiOrderReturnModel selectedCustomer;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage multi;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckContent;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtShelfOrderNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtCustomer;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfDetail;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Label lblListHeader;

    public Color ButtonColor => Color.FromRgb(3, 10, 53);

    public Color TextColor => Color.White;

    public MultiOrderApprove()
    {
      this.InitializeComponent();
      this.shelfOrderDetail = new List<pIOShelfOrderDetailForMultiOrderReturnModel>();
      ((Page) this).Title = "Koli Onay (Çoklu)";
      ToolbarItem toolbarItem = new ToolbarItem();
      ((MenuItem) toolbarItem).Text = "";
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(toolbarItem);
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
    }

    private void lstShelfOrders_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      this.selectedShelfOrder = (pIOUserShelfOrdersForMultiOrderReturnModel) e.SelectedItem;
      ((InputView) this.txtShelfOrderNumber).Text = this.selectedShelfOrder.ShelfOrderNumber;
      ((InputView) this.txtCustomer).Text = "";
      this.GetShelfDetail();
      ((NavigableElement) this).Navigation.PopAsync();
      this.txtCustomer_Focused((object) null, (FocusEventArgs) null);
    }

    private void GetShelfDetail()
    {
      this.shelfOrderDetail = new List<pIOShelfOrderDetailForMultiOrderReturnModel>();
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfDetailForMultiOrder?shelfOrderNumber={0}", (object) this.selectedShelfOrder.ShelfOrderNumber), (ContentPage) this);
      if (!returnModel.Success)
        return;
      this.shelfOrderDetail = GlobalMob.JsonDeserialize<List<pIOShelfOrderDetailForMultiOrderReturnModel>>(returnModel.Result);
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
      if (this.shelfOrderDetail.Count <= 0)
        return;
      ((VisualElement) this.txtCustomer).IsVisible = true;
    }

    private async void FillCustomerList()
    {
      MultiOrderApprove multiOrderApprove = this;
      List<pIOShelfOrderDetailForMultiOrderReturnModel> list = multiOrderApprove.shelfOrderDetail.GroupBy(c => new
      {
        CurrAccCode = c.CurrAccCode,
        CurrAccDescription = c.CurrAccDescription
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType8<string, string>, pIOShelfOrderDetailForMultiOrderReturnModel>, pIOShelfOrderDetailForMultiOrderReturnModel>(gcs => new pIOShelfOrderDetailForMultiOrderReturnModel()
      {
        CurrAccCode = gcs.Key.CurrAccCode,
        CurrAccDescription = gcs.Key.CurrAccDescription
      }).ToList<pIOShelfOrderDetailForMultiOrderReturnModel>();
      StackLayout stck = new StackLayout();
      stck.Orientation = (StackOrientation) 0;
      ListView listview = GlobalMob.GetListview("CurrAccDescription,CurrAccCode", 2, 1, hasUnEvenRows: true);
      ((ItemsView<Cell>) listview).ItemsSource = (IEnumerable) list;
      listview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(multiOrderApprove.LstCustomer_ItemSelected);
      ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) listview);
      SelectItem selectItem = new SelectItem(stck, "Müşteri Seçiniz");
      await ((NavigableElement) multiOrderApprove).Navigation.PushAsync((Page) selectItem);
    }

    private void LstCustomer_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      this.selectedCustomer = (pIOShelfOrderDetailForMultiOrderReturnModel) e.SelectedItem;
      if (this.selectedCustomer == null)
        return;
      ((InputView) this.txtCustomer).Text = this.selectedCustomer.CurrAccCode;
      this.RefreshDetails();
      this.lblListHeader.Text = this.selectedCustomer.CurrAccDescription;
      ((VisualElement) this.lstShelfDetail).IsVisible = true;
      ((VisualElement) this.txtBarcode).IsVisible = true;
      ((NavigableElement) this).Navigation.PopAsync();
      this.BarcodeFocus();
    }

    private void RefreshDetails()
    {
      List<pIOShelfOrderDetailForMultiOrderReturnModel> list = this.shelfOrderDetail.Where<pIOShelfOrderDetailForMultiOrderReturnModel>((Func<pIOShelfOrderDetailForMultiOrderReturnModel, bool>) (x => x.CurrAccCode == this.selectedCustomer.CurrAccCode)).OrderByDescending<pIOShelfOrderDetailForMultiOrderReturnModel, bool>((Func<pIOShelfOrderDetailForMultiOrderReturnModel, bool>) (x => x.LastReadBarcode)).ToList<pIOShelfOrderDetailForMultiOrderReturnModel>();
      double num = list.Sum<pIOShelfOrderDetailForMultiOrderReturnModel>((Func<pIOShelfOrderDetailForMultiOrderReturnModel, double>) (x => x.PickingQty));
      ((MenuItem) ((Page) this).ToolbarItems[0]).Text = list.Sum<pIOShelfOrderDetailForMultiOrderReturnModel>((Func<pIOShelfOrderDetailForMultiOrderReturnModel, double>) (x => x.ApproveQty)).ToString() + "/" + num.ToString();
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) list;
    }

    private void txtCustomer_Focused(object sender, FocusEventArgs e)
    {
      if (this.shelfOrderDetail == null || this.shelfOrderDetail.Count <= 0)
        return;
      this.FillCustomerList();
    }

    private async void txtShelfOrderNumber_Focused(object sender, FocusEventArgs e)
    {
      MultiOrderApprove page = this;
      StackLayout stck = new StackLayout();
      stck.Orientation = (StackOrientation) 0;
      ((View) stck).HorizontalOptions = LayoutOptions.FillAndExpand;
      page.shelfOrderList = new List<pIOUserShelfOrdersForMultiOrderReturnModel>();
      ListView listview = GlobalMob.GetListview("ShelfOrderNumber,CustomerName", 2, 1, hasUnEvenRows: true);
      ((View) listview).VerticalOptions = LayoutOptions.FillAndExpand;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetUserShelfOrdersForMultiOrder?userID={0}", (object) GlobalMob.User.UserID), (ContentPage) page);
      if (returnModel.Success)
      {
        List<pIOUserShelfOrdersForMultiOrderReturnModel> orderReturnModelList = GlobalMob.JsonDeserialize<List<pIOUserShelfOrdersForMultiOrderReturnModel>>(returnModel.Result);
        ((ItemsView<Cell>) listview).ItemsSource = (IEnumerable) orderReturnModelList;
      }
      ((VisualElement) page.lstShelfDetail).IsVisible = false;
      ((VisualElement) page.txtBarcode).IsVisible = false;
      listview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(page.lstShelfOrders_ItemSelected);
      ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) listview);
      SelectItem selectItem = new SelectItem(stck, "Raf Emri Seçiniz");
      await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
    }

    private void BarcodeFocus() => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(250);
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode).Focus();
    }));

    private void txtBarcode_Completed(object sender, EventArgs e)
    {
      string barcode = ((InputView) this.txtBarcode).Text;
      if (string.IsNullOrEmpty(barcode))
        return;
      pIOShelfOrderDetailForMultiOrderReturnModel orderReturnModel = this.shelfOrderDetail.Where<pIOShelfOrderDetailForMultiOrderReturnModel>((Func<pIOShelfOrderDetailForMultiOrderReturnModel, bool>) (x => x.Barcode.Contains(barcode) & x.CurrAccCode == this.selectedCustomer.CurrAccCode && x.ApproveQty < x.PickingQty)).FirstOrDefault<pIOShelfOrderDetailForMultiOrderReturnModel>();
      if (orderReturnModel != null)
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("UpdateDetailApproveForMultiOrder?detailID={0}", (object) orderReturnModel.ShelfOrderDetailID), (ContentPage) this);
        if (!returnModel.Success || !JsonConvert.DeserializeObject<bool>(returnModel.Result))
          return;
        GlobalMob.PlaySave();
        this.shelfOrderDetail.Select<pIOShelfOrderDetailForMultiOrderReturnModel, pIOShelfOrderDetailForMultiOrderReturnModel>((Func<pIOShelfOrderDetailForMultiOrderReturnModel, pIOShelfOrderDetailForMultiOrderReturnModel>) (c =>
        {
          c.LastReadBarcode = false;
          return c;
        })).ToList<pIOShelfOrderDetailForMultiOrderReturnModel>();
        ++orderReturnModel.ApproveQty;
        orderReturnModel.LastReadBarcode = true;
        this.RefreshDetails();
        this.BarcodeFocus();
      }
      else
      {
        GlobalMob.PlayError();
        string str = "Ürün bulunamadı";
        if (this.shelfOrderDetail.Where<pIOShelfOrderDetailForMultiOrderReturnModel>((Func<pIOShelfOrderDetailForMultiOrderReturnModel, bool>) (x => x.Barcode.Contains(barcode) & x.CurrAccCode == this.selectedCustomer.CurrAccCode)).FirstOrDefault<pIOShelfOrderDetailForMultiOrderReturnModel>() != null)
          str = "Ürün daha önce okutuldu";
        ((Page) this).DisplayAlert("Bilgi", str, "", "Tamam");
        this.BarcodeFocus();
      }
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (MultiOrderApprove).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/MultiOrderApprove.xaml",
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
        Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
        StackLayout stackLayout2 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry = new SoftkeyboardDisabledEntry();
        BindingExtension bindingExtension1 = new BindingExtension();
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension3 = new BindingExtension();
        Label label = new Label();
        StackLayout stackLayout3 = new StackLayout();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout4 = new StackLayout();
        StackLayout stackLayout5 = new StackLayout();
        MultiOrderApprove multiOrderApprove;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (multiOrderApprove = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) multiOrderApprove, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("multi", (object) multiOrderApprove);
        if (((Element) multiOrderApprove).StyleId == null)
          ((Element) multiOrderApprove).StyleId = "multi";
        ((INameScope) nameScope).RegisterName("stckContent", (object) stackLayout5);
        if (((Element) stackLayout5).StyleId == null)
          ((Element) stackLayout5).StyleId = "stckContent";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtShelfOrderNumber", (object) entry1);
        if (((Element) entry1).StyleId == null)
          ((Element) entry1).StyleId = "txtShelfOrderNumber";
        ((INameScope) nameScope).RegisterName("txtCustomer", (object) entry2);
        if (((Element) entry2).StyleId == null)
          ((Element) entry2).StyleId = "txtCustomer";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry);
        if (((Element) softkeyboardDisabledEntry).StyleId == null)
          ((Element) softkeyboardDisabledEntry).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("lstShelfDetail", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstShelfDetail";
        ((INameScope) nameScope).RegisterName("lblListHeader", (object) label);
        if (((Element) label).StyleId == null)
          ((Element) label).StyleId = "lblListHeader";
        this.multi = (ContentPage) multiOrderApprove;
        this.stckContent = stackLayout5;
        this.stckForm = stackLayout4;
        this.txtShelfOrderNumber = entry1;
        this.txtCustomer = entry2;
        this.txtBarcode = softkeyboardDisabledEntry;
        this.lstShelfDetail = listView;
        this.lblListHeader = label;
        ((BindableObject) stackLayout5).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout4).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf Emri Numarası Giriniz");
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((VisualElement) entry1).Focused += new EventHandler<FocusEventArgs>(multiOrderApprove.txtShelfOrderNumber_Focused);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\MultiOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\MultiOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Müşteri Seçiniz");
        ((BindableObject) entry2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((VisualElement) entry2).Focused += new EventHandler<FocusEventArgs>(multiOrderApprove.txtCustomer_Focused);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\MultiOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 18, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\MultiOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 18);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod Okutunuz");
        ((BindableObject) softkeyboardDisabledEntry).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) softkeyboardDisabledEntry).SetValue(VisualElement.WidthRequestProperty, (object) 200.0);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        softkeyboardDisabledEntry.Completed += new EventHandler(multiOrderApprove.txtBarcode_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) softkeyboardDisabledEntry);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry, new Uri("Views\\MultiOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 21, 18);
        bindingExtension1.Path = ".";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase1);
        ((BindableObject) listView).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) listView).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        ((BindableObject) listView).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 5000.0);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        ((BindableObject) label).SetValue(Label.TextProperty, (object) "");
        ((BindableObject) label).SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        referenceExtension1.Name = "multi";
        ReferenceExtension referenceExtension3 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 7];
        objArray1[0] = (object) bindingExtension2;
        objArray1[1] = (object) label;
        objArray1[2] = (object) stackLayout3;
        objArray1[3] = (object) listView;
        objArray1[4] = (object) stackLayout4;
        objArray1[5] = (object) stackLayout5;
        objArray1[6] = (object) multiOrderApprove;
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
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (MultiOrderApprove).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(30, 36)));
        object obj2 = ((IMarkupExtension) referenceExtension3).ProvideValue((IServiceProvider) xamlServiceProvider1);
        bindingExtension2.Source = obj2;
        VisualDiagnostics.RegisterSourceInfo(obj2, new Uri("Views\\MultiOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 30, 36);
        bindingExtension2.Path = "ButtonColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) label).SetBinding(Label.TextColorProperty, bindingBase2);
        referenceExtension2.Name = "multi";
        ReferenceExtension referenceExtension4 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 7];
        objArray2[0] = (object) bindingExtension3;
        objArray2[1] = (object) label;
        objArray2[2] = (object) stackLayout3;
        objArray2[3] = (object) listView;
        objArray2[4] = (object) stackLayout4;
        objArray2[5] = (object) stackLayout5;
        objArray2[6] = (object) multiOrderApprove;
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
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (MultiOrderApprove).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(31, 36)));
        object obj4 = ((IMarkupExtension) referenceExtension4).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension3.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\MultiOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 31, 36);
        bindingExtension3.Path = "TextColor";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) label).SetBinding(VisualElement.BackgroundColorProperty, bindingBase3);
        ((BindableObject) label).SetValue(Label.FontProperty, new FontTypeConverter().ConvertFromInvariantString("Bold,20"));
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) label);
        VisualDiagnostics.RegisterSourceInfo((object) label, new Uri("Views\\MultiOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 30);
        ((BindableObject) listView).SetValue(ListView.HeaderProperty, (object) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\MultiOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 27, 26);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MultiOrderApprove.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_12 xamlCdataTemplate12 = new MultiOrderApprove.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_12();
        object[] objArray3 = new object[0 + 5];
        objArray3[0] = (object) dataTemplate1;
        objArray3[1] = (object) listView;
        objArray3[2] = (object) stackLayout4;
        objArray3[3] = (object) stackLayout5;
        objArray3[4] = (object) multiOrderApprove;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate12.parentValues = objArray3;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate12.root = multiOrderApprove;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate12.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\MultiOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 36, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\MultiOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\MultiOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 14);
        ((BindableObject) multiOrderApprove).SetValue(ContentPage.ContentProperty, (object) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\MultiOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 10);
        VisualDiagnostics.RegisterSourceInfo((object) multiOrderApprove, new Uri("Views\\MultiOrderApprove.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Extensions.LoadFromXaml<MultiOrderApprove>(this, typeof (MultiOrderApprove));
      this.multi = NameScopeExtensions.FindByName<ContentPage>((Element) this, "multi");
      this.stckContent = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckContent");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtShelfOrderNumber = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtShelfOrderNumber");
      this.txtCustomer = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtCustomer");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.lstShelfDetail = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfDetail");
      this.lblListHeader = NameScopeExtensions.FindByName<Label>((Element) this, "lblListHeader");
    }
  }
}
