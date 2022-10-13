// Decompiled with JetBrains decompiler
// Type: Shelf.Views.Return
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
  [XamlFilePath("Views\\Return.xaml")]
  public class Return : ContentPage
  {
    private pIOUserShelfPurchaseOrdersReturnModel selectedShelfOrder;
    private ztIOShelf selectedShelf;
    private List<pIOShelfPurchaseOrderDetailReturnModel> shelfOrderDetail;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage ret;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckContent;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtShelfOrderNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtQty;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtShelfBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfDetail;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Label lblListHeader;

    public Color ButtonColor => Color.FromRgb(3, 10, 53);

    public Color TextColor => Color.White;

    public Return()
    {
      this.InitializeComponent();
      ((Page) this).Title = "İADE AL";
      GlobalMob.AddBarcodeLongPress(this.txtBarcode);
      ToolbarItem toolbarItem = new ToolbarItem();
      ((MenuItem) toolbarItem).Text = "";
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(toolbarItem);
      ((VisualElement) this.txtQty).IsVisible = !GlobalMob.User.HideQty;
    }

    protected virtual void OnAppearing() => ((Page) this).OnAppearing();

    private void GetShelfOrderDetail()
    {
      this.shelfOrderDetail = new List<pIOShelfPurchaseOrderDetailReturnModel>();
      Device.BeginInvokeOnMainThread((Action) (() =>
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfPurchaseOrderDetail?purchaseOrderNumber=S{0}", (object) ((InputView) this.txtShelfOrderNumber).Text), (ContentPage) this);
        if (!returnModel.Success)
          return;
        this.shelfOrderDetail = GlobalMob.JsonDeserialize<List<pIOShelfPurchaseOrderDetailReturnModel>>(returnModel.Result);
        ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
        if (this.shelfOrderDetail.Count <= 0)
          return;
        ((VisualElement) this.lstShelfDetail).IsVisible = true;
        ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) this.shelfOrderDetail;
        this.SetInfo();
      }));
    }

    private async void txtShelfOrderNumber_Focused(object sender, FocusEventArgs e)
    {
      Return page = this;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetUserShelfPurchaseOrders?userID={0}&shelfPurchaseOrderType={1}", (object) -1, (object) 3), (ContentPage) page);
      if (!returnModel.Success)
        return;
      List<pIOUserShelfPurchaseOrdersReturnModel> ordersReturnModelList = GlobalMob.JsonDeserialize<List<pIOUserShelfPurchaseOrdersReturnModel>>(returnModel.Result);
      ListView shelfListview = GlobalMob.GetShelfListview("PurchaseOrderNumber");
      ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) ordersReturnModelList;
      shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(page.ShelfOrderSelect_SelectedItem);
      SelectItem selectItem = new SelectItem(shelfListview, "Raf Emri Seçiniz");
      await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
    }

    private void ShelfOrderSelect_SelectedItem(object sender, SelectedItemChangedEventArgs e)
    {
      this.selectedShelfOrder = (pIOUserShelfPurchaseOrdersReturnModel) e.SelectedItem;
      ((InputView) this.txtShelfOrderNumber).Text = this.selectedShelfOrder.PurchaseOrderNumber.Replace("S", "");
      this.GetShelfOrderDetail();
      ((VisualElement) this.txtBarcode).Focus();
      ((NavigableElement) this).Navigation.PopAsync();
    }

    private void SetInfo()
    {
      double num1 = this.shelfOrderDetail.Sum<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, double>) (x => x.AllocatingQty));
      double num2 = this.shelfOrderDetail.Sum<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, double>) (x => x.OrderQty));
      ((MenuItem) ((Page) this).ToolbarItems[0]).Text = num1.ToString() + "/" + num2.ToString();
    }

    private async void TxtBarcode_Completed(object sender, EventArgs e)
    {
      Return page = this;
      string barcode = ((InputView) page.txtBarcode).Text;
      if (string.IsNullOrEmpty(barcode))
        return;
      if (barcode.Length < GlobalMob.User.MinimumBarcodeLength)
      {
        GlobalMob.PlayError();
        ((InputView) page.txtBarcode).Text = "";
        ((VisualElement) page.txtBarcode).Focus();
      }
      else if (page.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.Barcode.Contains(barcode))).Count<pIOShelfPurchaseOrderDetailReturnModel>() <= 0)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Ürün Raf Emrinde Bulunamadı", "", "Tamam") ? 1 : 0;
        ((InputView) page.txtBarcode).Text = "";
        ((VisualElement) page.txtBarcode).Focus();
      }
      else
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfFromBarcodeForReturn?barcode={0}&purchaseOrderID={1}", (object) barcode, (object) page.selectedShelfOrder.PurchaseOrderID), (ContentPage) page);
        if (!returnModel.Success)
          return;
        List<pIOGetShelfFromBarcodeForReturnReturnModel> source = GlobalMob.JsonDeserialize<List<pIOGetShelfFromBarcodeForReturnReturnModel>>(returnModel.Result);
        if (source.Count <= 0)
        {
          ((VisualElement) page.txtShelfBarcode).Focus();
        }
        else
        {
          IEnumerable<pIOGetShelfFromBarcodeReturnModel> barcodeReturnModels = source.GroupBy(c => new
          {
            ShelfCode = c.ShelfCode,
            Description = c.Description
          }).Select<IGrouping<\u003C\u003Ef__AnonymousType18<string, string>, pIOGetShelfFromBarcodeForReturnReturnModel>, pIOGetShelfFromBarcodeReturnModel>(gcs => new pIOGetShelfFromBarcodeReturnModel()
          {
            ShelfCode = gcs.Key.ShelfCode,
            Description = gcs.Key.Description,
            PurchaseOrderDetailID = gcs.Max<pIOGetShelfFromBarcodeForReturnReturnModel>((Func<pIOGetShelfFromBarcodeForReturnReturnModel, int?>) (x => x.PurchaseOrderDetailID))
          });
          ListView shelfListview = GlobalMob.GetShelfListview("ShelfCode,Description");
          ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) barcodeReturnModels;
          shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(page.ShelfSelect_SelectedItem);
          SelectItem selectItem = new SelectItem(shelfListview, "İade Rafı Seçiniz");
          ((Page) selectItem).Disappearing += new EventHandler(page.Select_Disappearing);
          await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
        }
      }
    }

    private void Select_Disappearing(object sender, EventArgs e) => ((VisualElement) this.txtShelfBarcode).Focus();

    private void ShelfSelect_SelectedItem(object sender, SelectedItemChangedEventArgs e)
    {
      ReturnModel shelf = GlobalMob.GetShelf(((pIOGetShelfFromBarcodeReturnModel) e.SelectedItem).ShelfCode, (ContentPage) this);
      if (!shelf.Success || string.IsNullOrEmpty(shelf.Result))
        return;
      this.selectedShelf = JsonConvert.DeserializeObject<ztIOShelf>(shelf.Result);
      ((InputView) this.txtShelf).Text = this.selectedShelf.Code;
      ((NavigableElement) this).Navigation.PopAsync();
      ((InputView) this.txtShelfBarcode).Text = "";
      ((VisualElement) this.txtShelfBarcode).Focus();
    }

    private async void TxtShelfBarcode_Completed(object sender, EventArgs e)
    {
      Return page = this;
      if (string.IsNullOrEmpty(((InputView) page.txtShelfBarcode).Text))
        return;
      if (page.selectedShelf == null)
      {
        ReturnModel shelf = GlobalMob.GetShelf(((InputView) page.txtShelfBarcode).Text, (ContentPage) page);
        if (shelf.Success && !string.IsNullOrEmpty(shelf.Result))
        {
          page.selectedShelf = JsonConvert.DeserializeObject<ztIOShelf>(shelf.Result);
          ((InputView) page.txtShelf).Text = page.selectedShelf.Code;
        }
      }
      if (page.selectedShelf == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Raf Bulunamadı", "", "Tamam") ? 1 : 0;
        ((InputView) page.txtShelfBarcode).Text = "";
        ((VisualElement) page.txtShelfBarcode).Focus();
      }
      else if (page.selectedShelf.Code != ((InputView) page.txtShelf).Text && page.selectedShelf.Description != ((InputView) page.txtShelf).Text)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Lütfen önerilen rafı okutunuz", "", "Tamam") ? 1 : 0;
        ((InputView) page.txtShelfBarcode).Text = "";
        ((VisualElement) page.txtShelfBarcode).Focus();
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        pIOShelfPurchaseOrderDetailReturnModel detailReturnModel = page.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>(new Func<pIOShelfPurchaseOrderDetailReturnModel, bool>(page.\u003CTxtShelfBarcode_Completed\u003Eb__16_0)).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>();
        if (detailReturnModel == null)
        {
          int num = await ((Page) page).DisplayAlert("Bilgi", "Ürün daha önce okutuldu.", "", "Tamam") ? 1 : 0;
          page.ResetForm();
        }
        else
        {
          ShelfTransaction shelfTransaction = new ShelfTransaction();
          shelfTransaction.ShelfID = page.selectedShelf.ShelfID;
          shelfTransaction.ProcessTypeID = 1;
          shelfTransaction.WareHouseCode = page.selectedShelf.WarehouseCode;
          shelfTransaction.Barcode = ((InputView) page.txtBarcode).Text;
          shelfTransaction.UserName = GlobalMob.User.UserName;
          shelfTransaction.Qty = Convert.ToInt32(((InputView) page.txtQty).Text);
          shelfTransaction.TransTypeID = 8;
          shelfTransaction.ShelfOrderDetailID = detailReturnModel.PurchaseOrderDetailID;
          shelfTransaction.DocumentNumber = page.selectedShelfOrder.PurchaseOrderNumber;
          shelfTransaction.ShelfOrderID = page.selectedShelfOrder.PurchaseOrderID;
          if (detailReturnModel.AllocatingQty + (double) shelfTransaction.Qty > detailReturnModel.OrderQty)
          {
            int num1 = await ((Page) page).DisplayAlert("Bilgi", "Fazla miktar girilmiş.", "", "Tamam") ? 1 : 0;
          }
          else
          {
            ReturnModel result = GlobalMob.PostJson("UpdateReturnAllocate", new Dictionary<string, string>()
            {
              {
                "json",
                JsonConvert.SerializeObject((object) shelfTransaction)
              }
            }, (ContentPage) page).Result;
            if (!result.Success)
              return;
            if (JsonConvert.DeserializeObject<ztIOShelfTransactionDetail>(result.Result) != null)
            {
              detailReturnModel.AllocatingQty += (double) shelfTransaction.Qty;
              page.shelfOrderDetail.Select<pIOShelfPurchaseOrderDetailReturnModel, pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, pIOShelfPurchaseOrderDetailReturnModel>) (c =>
              {
                c.LastReadBarcode = false;
                return c;
              })).ToList<pIOShelfPurchaseOrderDetailReturnModel>();
              detailReturnModel.LastReadBarcode = true;
              page.shelfOrderDetail = page.shelfOrderDetail.OrderByDescending<pIOShelfPurchaseOrderDetailReturnModel, bool>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.LastReadBarcode)).ToList<pIOShelfPurchaseOrderDetailReturnModel>();
              page.LoadData();
              page.SetInfo();
              page.ResetForm();
            }
            else
            {
              int num2 = await ((Page) page).DisplayAlert("Bilgi", "Ürün zaten eklenmiş", "", "Tamam") ? 1 : 0;
            }
          }
        }
      }
    }

    private void LoadData()
    {
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) this.shelfOrderDetail;
    }

    private void ResetForm()
    {
      this.selectedShelf = (ztIOShelf) null;
      ((InputView) this.txtQty).Text = "1";
      ((InputView) this.txtBarcode).Text = "";
      ((InputView) this.txtShelfBarcode).Text = "";
      ((InputView) this.txtShelf).Text = "";
      ((VisualElement) this.txtBarcode).Focus();
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (Return).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/Return.xaml",
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
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry2 = new SoftkeyboardDisabledEntry();
        Xamarin.Forms.Entry entry1 = new Xamarin.Forms.Entry();
        StackLayout stackLayout2 = new StackLayout();
        Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
        StackLayout stackLayout3 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry3 = new SoftkeyboardDisabledEntry();
        StackLayout stackLayout4 = new StackLayout();
        StackLayout stackLayout5 = new StackLayout();
        BindingExtension bindingExtension1 = new BindingExtension();
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension3 = new BindingExtension();
        Label label = new Label();
        StackLayout stackLayout6 = new StackLayout();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout7 = new StackLayout();
        StackLayout stackLayout8 = new StackLayout();
        Return @return;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (@return = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) @return, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("ret", (object) @return);
        if (((Element) @return).StyleId == null)
          ((Element) @return).StyleId = "ret";
        ((INameScope) nameScope).RegisterName("stckContent", (object) stackLayout8);
        if (((Element) stackLayout8).StyleId == null)
          ((Element) stackLayout8).StyleId = "stckContent";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout7);
        if (((Element) stackLayout7).StyleId == null)
          ((Element) stackLayout7).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtShelfOrderNumber", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtShelfOrderNumber";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("txtQty", (object) entry1);
        if (((Element) entry1).StyleId == null)
          ((Element) entry1).StyleId = "txtQty";
        ((INameScope) nameScope).RegisterName("txtShelf", (object) entry2);
        if (((Element) entry2).StyleId == null)
          ((Element) entry2).StyleId = "txtShelf";
        ((INameScope) nameScope).RegisterName("txtShelfBarcode", (object) softkeyboardDisabledEntry3);
        if (((Element) softkeyboardDisabledEntry3).StyleId == null)
          ((Element) softkeyboardDisabledEntry3).StyleId = "txtShelfBarcode";
        ((INameScope) nameScope).RegisterName("lstShelfDetail", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstShelfDetail";
        ((INameScope) nameScope).RegisterName("lblListHeader", (object) label);
        if (((Element) label).StyleId == null)
          ((Element) label).StyleId = "lblListHeader";
        this.ret = (ContentPage) @return;
        this.stckContent = stackLayout8;
        this.stckForm = stackLayout7;
        this.txtShelfOrderNumber = softkeyboardDisabledEntry1;
        this.txtBarcode = softkeyboardDisabledEntry2;
        this.txtQty = entry1;
        this.txtShelf = entry2;
        this.txtShelfBarcode = softkeyboardDisabledEntry3;
        this.lstShelfDetail = listView;
        this.lblListHeader = label;
        ((BindableObject) stackLayout7).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf Emri Numarası Giriniz");
        ((VisualElement) softkeyboardDisabledEntry1).Focused += new EventHandler<FocusEventArgs>(@return.txtShelfOrderNumber_Focused);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 18);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        softkeyboardDisabledEntry2.Completed += new EventHandler(@return.TxtBarcode_Completed);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod Numarası Giriniz");
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 19, 26);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.TextProperty, (object) "1");
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Miktar");
        ((BindableObject) entry1).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 21, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 18, 18);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "");
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry2).SetValue(VisualElement.InputTransparentProperty, (object) true);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 24, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 18);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        softkeyboardDisabledEntry3.Completed += new EventHandler(@return.TxtShelfBarcode_Completed);
        ((BindableObject) softkeyboardDisabledEntry3).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf Okutunuz");
        ((BindableObject) softkeyboardDisabledEntry3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) softkeyboardDisabledEntry3);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry3, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 27, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 14);
        bindingExtension1.Path = ".";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase1);
        ((BindableObject) listView).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) listView).SetValue(ListView.RowHeightProperty, (object) 100);
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 5000.0);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        ((BindableObject) label).SetValue(Label.TextProperty, (object) "Raf Detayları");
        ((BindableObject) label).SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        referenceExtension1.Name = "ret";
        ReferenceExtension referenceExtension3 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 7];
        objArray1[0] = (object) bindingExtension2;
        objArray1[1] = (object) label;
        objArray1[2] = (object) stackLayout6;
        objArray1[3] = (object) listView;
        objArray1[4] = (object) stackLayout7;
        objArray1[5] = (object) stackLayout8;
        objArray1[6] = (object) @return;
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
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (Return).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(39, 36)));
        object obj2 = ((IMarkupExtension) referenceExtension3).ProvideValue((IServiceProvider) xamlServiceProvider1);
        bindingExtension2.Source = obj2;
        VisualDiagnostics.RegisterSourceInfo(obj2, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 39, 36);
        bindingExtension2.Path = "ButtonColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) label).SetBinding(Label.TextColorProperty, bindingBase2);
        referenceExtension2.Name = "ret";
        ReferenceExtension referenceExtension4 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 7];
        objArray2[0] = (object) bindingExtension3;
        objArray2[1] = (object) label;
        objArray2[2] = (object) stackLayout6;
        objArray2[3] = (object) listView;
        objArray2[4] = (object) stackLayout7;
        objArray2[5] = (object) stackLayout8;
        objArray2[6] = (object) @return;
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
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (Return).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(40, 36)));
        object obj4 = ((IMarkupExtension) referenceExtension4).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension3.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 40, 36);
        bindingExtension3.Path = "TextColor";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) label).SetBinding(VisualElement.BackgroundColorProperty, bindingBase3);
        ((BindableObject) label).SetValue(Label.FontProperty, new FontTypeConverter().ConvertFromInvariantString("Bold,20"));
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) label);
        VisualDiagnostics.RegisterSourceInfo((object) label, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 37, 30);
        ((BindableObject) listView).SetValue(ListView.HeaderProperty, (object) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 36, 26);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Return.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_23 xamlCdataTemplate23 = new Return.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_23();
        object[] objArray3 = new object[0 + 5];
        objArray3[0] = (object) dataTemplate1;
        objArray3[1] = (object) listView;
        objArray3[2] = (object) stackLayout7;
        objArray3[3] = (object) stackLayout8;
        objArray3[4] = (object) @return;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate23.parentValues = objArray3;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate23.root = @return;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate23.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 45, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 33, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 14);
        ((BindableObject) @return).SetValue(ContentPage.ContentProperty, (object) stackLayout8);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout8, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 10);
        VisualDiagnostics.RegisterSourceInfo((object) @return, new Uri("Views\\Return.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Extensions.LoadFromXaml<Return>(this, typeof (Return));
      this.ret = NameScopeExtensions.FindByName<ContentPage>((Element) this, "ret");
      this.stckContent = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckContent");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtShelfOrderNumber = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtShelfOrderNumber");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.txtQty = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtQty");
      this.txtShelf = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtShelf");
      this.txtShelfBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtShelfBarcode");
      this.lstShelfDetail = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfDetail");
      this.lblListHeader = NameScopeExtensions.FindByName<Label>((Element) this, "lblListHeader");
    }
  }
}
