// Decompiled with JetBrains decompiler
// Type: Shelf.Views.ShelfSorting
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
  [XamlFilePath("Views\\ShelfSorting.xaml")]
  public class ShelfSorting : ContentPage
  {
    private pIOUserShelfOrdersReturnModel selectedShelfOrder;
    private List<pIOShelfOrderDetailForPivotReturnModel> shelfOrderDetail;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage sorting;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckContent;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtShelfOrderNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckSettings;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckShelfType;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfDetail;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Label lblListHeader;

    public ztIOShelf selectShelf { get; set; }

    public Color ButtonColor => Color.FromRgb(0, 0, 0);

    public Color TextColor => Color.White;

    public ShelfSorting()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Pivotla";
      List<PickerItem> pickerItemList = new List<PickerItem>();
      ReturnModel returnModel = GlobalMob.PostJson("GetShelfTypes", (ContentPage) this);
      if (returnModel.Success)
      {
        foreach (ztIOShelfType ztIoShelfType in JsonConvert.DeserializeObject<List<ztIOShelfType>>(returnModel.Result).Where<ztIOShelfType>((Func<ztIOShelfType, bool>) (x => x.ShelfType > (byte) 0 && x.ShelfType < (byte) 100)).ToList<ztIOShelfType>())
          pickerItemList.Add(new PickerItem()
          {
            Caption = ztIoShelfType.ShelfTypeDescription,
            Code = (int) ztIoShelfType.ShelfType
          });
      }
      this.pckShelfType.ItemsSource = (IList) pickerItemList;
      GlobalMob.AddBarcodeLongPress(this.txtBarcode);
      ToolbarItem toolbarItem = new ToolbarItem();
      ((MenuItem) toolbarItem).Text = "";
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(toolbarItem);
      ((VisualElement) this.txtShelfOrderNumber).Focus();
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
    }

    private async void txtShelfOrderNumber_Focused(object sender, FocusEventArgs e)
    {
      ShelfSorting page = this;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetUserShelfOrdersForPivot?userID={0}", (object) GlobalMob.User.UserID), (ContentPage) page);
      if (!returnModel.Success)
        return;
      List<pIOUserShelfOrdersReturnModel> ordersReturnModelList = GlobalMob.JsonDeserialize<List<pIOUserShelfOrdersReturnModel>>(returnModel.Result);
      ListView shelfListview = GlobalMob.GetShelfListview("ShelfOrderNumber");
      ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) ordersReturnModelList;
      shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(page.ShelfOrderSelect_SelectedItem);
      SelectItem selectItem = new SelectItem(shelfListview, "Raf Emri Seçiniz");
      await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
    }

    private async void ShelfOrderSelect_SelectedItem(object sender, SelectedItemChangedEventArgs e)
    {
      ShelfSorting shelfSorting = this;
      shelfSorting.selectedShelfOrder = (pIOUserShelfOrdersReturnModel) e.SelectedItem;
      if (shelfSorting.selectedShelfOrder.ShelfOrderType == (byte) 3)
      {
        ((InputView) shelfSorting.txtShelfOrderNumber).Text = shelfSorting.selectedShelfOrder.ShelfOrderNumber.Replace("S", "");
        ((VisualElement) shelfSorting.stckSettings).IsVisible = true;
        shelfSorting.GetShelfOrderDetail();
        ((VisualElement) shelfSorting.pckShelfType).Focus();
        Page page = await ((NavigableElement) shelfSorting).Navigation.PopAsync();
      }
      else
      {
        int num = await ((Page) shelfSorting).DisplayAlert("Bilgi", "Bu raf emri çoklu raf emri değildir", "", "Tamam") ? 1 : 0;
      }
    }

    private void GetShelfOrderDetail()
    {
      this.shelfOrderDetail = new List<pIOShelfOrderDetailForPivotReturnModel>();
      Device.BeginInvokeOnMainThread((Action) (() =>
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfDetailForPivot?shelfOrderNumber=S{0}", (object) ((InputView) this.txtShelfOrderNumber).Text), (ContentPage) this);
        if (!returnModel.Success)
          return;
        this.shelfOrderDetail = GlobalMob.JsonDeserialize<List<pIOShelfOrderDetailForPivotReturnModel>>(returnModel.Result);
        ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
        if (this.shelfOrderDetail.Count <= 0)
          return;
        ((VisualElement) this.lstShelfDetail).IsVisible = true;
        ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) this.shelfOrderDetail;
        this.SetInfo();
      }));
    }

    private void SetInfo()
    {
      double num = this.shelfOrderDetail.Sum<pIOShelfOrderDetailForPivotReturnModel>((Func<pIOShelfOrderDetailForPivotReturnModel, double>) (x => x.PickingQty));
      ((MenuItem) ((Page) this).ToolbarItems[0]).Text = this.shelfOrderDetail.Sum<pIOShelfOrderDetailForPivotReturnModel>((Func<pIOShelfOrderDetailForPivotReturnModel, double>) (x => x.ApproveQty)).ToString() + "/" + num.ToString();
    }

    private void LoadData()
    {
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) this.shelfOrderDetail;
    }

    private pIOShelfOrderDetailForPivotReturnModel GetItem(
      string barcode)
    {
      return !GlobalMob.User.BarcodeSearchEqual ? this.shelfOrderDetail.Where<pIOShelfOrderDetailForPivotReturnModel>((Func<pIOShelfOrderDetailForPivotReturnModel, bool>) (x => x.Barcode.Contains(barcode) && x.PickingQty != x.ApproveQty)).FirstOrDefault<pIOShelfOrderDetailForPivotReturnModel>() : this.shelfOrderDetail.Where<pIOShelfOrderDetailForPivotReturnModel>((Func<pIOShelfOrderDetailForPivotReturnModel, bool>) (x => x.Barcode.Replace(",", "").Trim() == barcode && x.PickingQty != x.ApproveQty)).FirstOrDefault<pIOShelfOrderDetailForPivotReturnModel>();
    }

    private void txtBarcode_Completed(object sender, EventArgs e)
    {
      string barcode = ((InputView) this.txtBarcode).Text;
      if (string.IsNullOrEmpty(barcode))
        return;
      if (this.pckShelfType.SelectedItem == null)
        this.ShowErrorMessage("Bilgi", "Öncelikle Dağıtım Rafı Seçiniz");
      else if (this.shelfOrderDetail.Where<pIOShelfOrderDetailForPivotReturnModel>((Func<pIOShelfOrderDetailForPivotReturnModel, bool>) (x => x.PickingQty > x.ApproveQty)).Count<pIOShelfOrderDetailForPivotReturnModel>() == 0)
      {
        this.ShowErrorMessage("Bilgi", "Sepetteki Tüm Ürünler Okutuldu!");
      }
      else
      {
        pIOShelfOrderDetailForPivotReturnModel pivotReturnModel1 = this.GetItem(barcode);
        if (pivotReturnModel1 != null)
        {
          if (this.selectedShelfOrder.ShelfOrderType != (byte) 3 && this.selectedShelfOrder.ShelfOrderType != (byte) 5)
            return;
          pivotReturnModel1.UserName = GlobalMob.User.UserName;
          PickerItem selectedItem = (PickerItem) this.pckShelfType.SelectedItem;
          pivotReturnModel1.ShelfType = selectedItem.Code;
          ReturnModel result = GlobalMob.PostJson("CreatePivotAllocate", new Dictionary<string, string>()
          {
            {
              "json",
              JsonConvert.SerializeObject((object) pivotReturnModel1)
            }
          }, (ContentPage) this).Result;
          if (!result.Success)
            return;
          pIOShelfOrderDetailForPivotReturnModel pivotReturnModel2 = JsonConvert.DeserializeObject<pIOShelfOrderDetailForPivotReturnModel>(result.Result);
          if (pivotReturnModel2 == null)
          {
            this.ShowErrorMessage("Bilgi", "Dağıtılacak Raf Bulunamadı!");
          }
          else
          {
            this.shelfOrderDetail.Select<pIOShelfOrderDetailForPivotReturnModel, pIOShelfOrderDetailForPivotReturnModel>((Func<pIOShelfOrderDetailForPivotReturnModel, pIOShelfOrderDetailForPivotReturnModel>) (c =>
            {
              c.LastReadBarcode = false;
              return c;
            })).ToList<pIOShelfOrderDetailForPivotReturnModel>();
            pivotReturnModel1.ApproveQty = pivotReturnModel2.ApproveQty;
            pivotReturnModel1.LastReadBarcode = true;
            pivotReturnModel1.ShelfCode = pivotReturnModel2.ShelfCode;
            ((InputView) this.txtShelf).Text = pivotReturnModel2.ShelfCode;
            pivotReturnModel1.PivotShelfCode += string.IsNullOrEmpty(pivotReturnModel1.PivotShelfCode) ? pivotReturnModel2.ShelfCode : "," + pivotReturnModel2.ShelfCode;
            GlobalMob.PlaySave();
            this.shelfOrderDetail = this.shelfOrderDetail.OrderByDescending<pIOShelfOrderDetailForPivotReturnModel, bool>((Func<pIOShelfOrderDetailForPivotReturnModel, bool>) (x => x.LastReadBarcode)).ToList<pIOShelfOrderDetailForPivotReturnModel>();
            this.LoadData();
            this.SetInfo();
            this.BarcodeFocus();
          }
        }
        else
          this.ShowErrorMessage("Bilgi", this.shelfOrderDetail.Where<pIOShelfOrderDetailForPivotReturnModel>((Func<pIOShelfOrderDetailForPivotReturnModel, bool>) (x => x.Barcode.Contains(barcode))).Count<pIOShelfOrderDetailForPivotReturnModel>() > 0 ? "Ürün daha önce okutulmuş" : "Ürün Bulunamadı!");
      }
    }

    private void BarcodeFocus() => Device.BeginInvokeOnMainThread((Action) (() =>
    {
      Task.Delay(200);
      ((VisualElement) this.txtBarcode)?.Focus();
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode).Focus();
    }));

    private async void ShowErrorMessage(string title, string message)
    {
      ShelfSorting shelfSorting = this;
      GlobalMob.PlayError();
      int num = await ((Page) shelfSorting).DisplayAlert(title, message, "", "Tamam") ? 1 : 0;
      ((InputView) shelfSorting.txtBarcode).Text = "";
      ((VisualElement) shelfSorting.txtBarcode).Focus();
    }

    private void pckShelfType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.pckShelfType.SelectedItem == null)
        return;
      ((VisualElement) this.txtBarcode).Focus();
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (ShelfSorting).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/ShelfSorting.xaml",
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
        StackLayout stackLayout1 = new StackLayout();
        BindingExtension bindingExtension1 = new BindingExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        Picker picker = new Picker();
        StackLayout stackLayout2 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry2 = new SoftkeyboardDisabledEntry();
        StackLayout stackLayout3 = new StackLayout();
        Xamarin.Forms.Entry entry1 = new Xamarin.Forms.Entry();
        StackLayout stackLayout4 = new StackLayout();
        StackLayout stackLayout5 = new StackLayout();
        StackLayout stackLayout6 = new StackLayout();
        StackLayout stackLayout7 = new StackLayout();
        BindingExtension bindingExtension3 = new BindingExtension();
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension4 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension5 = new BindingExtension();
        Label label = new Label();
        StackLayout stackLayout8 = new StackLayout();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout9 = new StackLayout();
        ShelfSorting shelfSorting;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (shelfSorting = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) shelfSorting, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("sorting", (object) shelfSorting);
        if (((Element) shelfSorting).StyleId == null)
          ((Element) shelfSorting).StyleId = "sorting";
        ((INameScope) nameScope).RegisterName("stckContent", (object) stackLayout9);
        if (((Element) stackLayout9).StyleId == null)
          ((Element) stackLayout9).StyleId = "stckContent";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout7);
        if (((Element) stackLayout7).StyleId == null)
          ((Element) stackLayout7).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtShelfOrderNumber", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtShelfOrderNumber";
        ((INameScope) nameScope).RegisterName("stckSettings", (object) stackLayout5);
        if (((Element) stackLayout5).StyleId == null)
          ((Element) stackLayout5).StyleId = "stckSettings";
        ((INameScope) nameScope).RegisterName("pckShelfType", (object) picker);
        if (((Element) picker).StyleId == null)
          ((Element) picker).StyleId = "pckShelfType";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("txtShelf", (object) entry1);
        if (((Element) entry1).StyleId == null)
          ((Element) entry1).StyleId = "txtShelf";
        ((INameScope) nameScope).RegisterName("lstShelfDetail", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstShelfDetail";
        ((INameScope) nameScope).RegisterName("lblListHeader", (object) label);
        if (((Element) label).StyleId == null)
          ((Element) label).StyleId = "lblListHeader";
        this.sorting = (ContentPage) shelfSorting;
        this.stckContent = stackLayout9;
        this.stckForm = stackLayout7;
        this.txtShelfOrderNumber = softkeyboardDisabledEntry1;
        this.stckSettings = stackLayout5;
        this.pckShelfType = picker;
        this.txtBarcode = softkeyboardDisabledEntry2;
        this.txtShelf = entry1;
        this.lstShelfDetail = listView;
        this.lblListHeader = label;
        ((BindableObject) stackLayout7).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf Emri Numarası Giriniz");
        ((VisualElement) softkeyboardDisabledEntry1).Focused += new EventHandler<FocusEventArgs>(shelfSorting.txtShelfOrderNumber_Focused);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 22);
        ((BindableObject) stackLayout5).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) picker).SetValue(Picker.TitleProperty, (object) "Dağıtım Rafı Seçiniz");
        bindingExtension1.Path = ".";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker).SetBinding(Picker.ItemsSourceProperty, bindingBase1);
        picker.SelectedIndexChanged += new EventHandler(shelfSorting.pckShelfType_SelectedIndexChanged);
        bindingExtension2.Path = "Caption";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        picker.ItemDisplayBinding = bindingBase2;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase2, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 22, 33);
        ((BindableObject) picker).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) picker);
        VisualDiagnostics.RegisterSourceInfo((object) picker, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 30);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 19, 26);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        softkeyboardDisabledEntry2.Completed += new EventHandler(shelfSorting.txtBarcode_Completed);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod Numarası Giriniz");
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 26, 30);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 25, 26);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Ürünün Bırakılacağı Raf");
        ((BindableObject) entry1).SetValue(VisualElement.BackgroundColorProperty, (object) Color.White);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.TextColorProperty, (object) Color.Red);
        Xamarin.Forms.Entry entry2 = entry1;
        BindableProperty fontSizeProperty = Xamarin.Forms.Entry.FontSizeProperty;
        FontSizeConverter fontSizeConverter = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 7];
        objArray1[0] = (object) entry1;
        objArray1[1] = (object) stackLayout4;
        objArray1[2] = (object) stackLayout5;
        objArray1[3] = (object) stackLayout6;
        objArray1[4] = (object) stackLayout7;
        objArray1[5] = (object) stackLayout9;
        objArray1[6] = (object) shelfSorting;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) Xamarin.Forms.Entry.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver1.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (ShelfSorting).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(31, 48)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider1);
        ((BindableObject) entry2).SetValue(fontSizeProperty, obj2);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 30, 30);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 29, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 18, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout9).Children).Add((View) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 14);
        bindingExtension3.Path = ".";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase3);
        ((BindableObject) listView).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 5000.0);
        ((BindableObject) listView).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        ((BindableObject) label).SetValue(Label.TextProperty, (object) "Raf Detayları");
        ((BindableObject) label).SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        referenceExtension1.Name = "sorting";
        ReferenceExtension referenceExtension3 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 6];
        objArray2[0] = (object) bindingExtension4;
        objArray2[1] = (object) label;
        objArray2[2] = (object) stackLayout8;
        objArray2[3] = (object) listView;
        objArray2[4] = (object) stackLayout9;
        objArray2[5] = (object) shelfSorting;
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
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (ShelfSorting).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(44, 36)));
        object obj4 = ((IMarkupExtension) referenceExtension3).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension4.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 44, 36);
        bindingExtension4.Path = "ButtonColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) label).SetBinding(Label.TextColorProperty, bindingBase4);
        referenceExtension2.Name = "sorting";
        ReferenceExtension referenceExtension4 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 6];
        objArray3[0] = (object) bindingExtension5;
        objArray3[1] = (object) label;
        objArray3[2] = (object) stackLayout8;
        objArray3[3] = (object) listView;
        objArray3[4] = (object) stackLayout9;
        objArray3[5] = (object) shelfSorting;
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
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (ShelfSorting).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(45, 36)));
        object obj6 = ((IMarkupExtension) referenceExtension4).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension5.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 45, 36);
        bindingExtension5.Path = "TextColor";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) label).SetBinding(VisualElement.BackgroundColorProperty, bindingBase5);
        ((BindableObject) label).SetValue(Label.FontProperty, new FontTypeConverter().ConvertFromInvariantString("Bold,20"));
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) label);
        VisualDiagnostics.RegisterSourceInfo((object) label, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 42, 26);
        ((BindableObject) listView).SetValue(ListView.HeaderProperty, (object) stackLayout8);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout8, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 41, 22);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ShelfSorting.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_37 xamlCdataTemplate37 = new ShelfSorting.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_37();
        object[] objArray4 = new object[0 + 4];
        objArray4[0] = (object) dataTemplate1;
        objArray4[1] = (object) listView;
        objArray4[2] = (object) stackLayout9;
        objArray4[3] = (object) shelfSorting;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate37.parentValues = objArray4;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate37.root = shelfSorting;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate37.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 50, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout9).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 38, 14);
        ((BindableObject) shelfSorting).SetValue(ContentPage.ContentProperty, (object) stackLayout9);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout9, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 10);
        VisualDiagnostics.RegisterSourceInfo((object) shelfSorting, new Uri("Views\\ShelfSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Extensions.LoadFromXaml<ShelfSorting>(this, typeof (ShelfSorting));
      this.sorting = NameScopeExtensions.FindByName<ContentPage>((Element) this, "sorting");
      this.stckContent = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckContent");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtShelfOrderNumber = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtShelfOrderNumber");
      this.stckSettings = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckSettings");
      this.pckShelfType = NameScopeExtensions.FindByName<Picker>((Element) this, "pckShelfType");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.txtShelf = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtShelf");
      this.lstShelfDetail = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfDetail");
      this.lblListHeader = NameScopeExtensions.FindByName<Label>((Element) this, "lblListHeader");
    }
  }
}
