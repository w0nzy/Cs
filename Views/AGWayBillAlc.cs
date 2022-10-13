// Decompiled with JetBrains decompiler
// Type: Shelf.Views.AGWayBillAlc
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
  [XamlFilePath("Views\\AGWayBillAlc.xaml")]
  public class AGWayBillAlc : ContentPage
  {
    private List<pIOGetRemainingDispOrderByItemReturnModel> dispOrderList;
    private pIOGetRemainingDispOrderByItemReturnModel selectedDispOrder;
    private List<pIOGetErpGoodsInDetailReturnModel> goodsInDetailList;
    private List<pIOGetRemainingDispOrderByItemReturnModel> dispOrderDetails;
    private List<pIOGetRemainingDispOrderByItemReturnModel> dispOrderDetailsGroup;
    private pIOGetErpGoodsInReturnModel selectGoodsIn;
    private ztIOShelf selectShelf;
    private ToolbarItem tItem;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage agwaybillalc;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtSelectionCode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnShelfBack;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnShelfNext;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtShelfBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnClearShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtQty;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstDispOrderDetails;

    public Color ButtonColor => Color.FromRgb(142, 81, 152);

    public Color TextColor => Color.White;

    public AGWayBillAlc(
      List<pIOGetRemainingDispOrderByItemReturnModel> fDispOrderList,
      List<pIOGetErpGoodsInDetailReturnModel> fGoodsInDetailList,
      pIOGetErpGoodsInReturnModel fSelectGoodsIn)
    {
      this.InitializeComponent();
      ((Page) this).Title = "Sipariş Karşılama";
      ((VisualElement) this.btnClearShelf).HeightRequest = ((VisualElement) this.txtShelfBarcode).HeightRequest;
      this.dispOrderList = fDispOrderList;
      this.goodsInDetailList = fGoodsInDetailList;
      this.selectGoodsIn = fSelectGoodsIn;
      fDispOrderList = fDispOrderList.OrderBy<pIOGetRemainingDispOrderByItemReturnModel, int?>((Func<pIOGetRemainingDispOrderByItemReturnModel, int?>) (x => x.Priority)).ToList<pIOGetRemainingDispOrderByItemReturnModel>();
      this.selectedDispOrder = fDispOrderList[0];
      ((Page) this).Title = ((Page) this).Title + "-" + this.selectedDispOrder.SelectionCode;
      ((VisualElement) this.txtQty).IsVisible = !GlobalMob.User.HideQty;
      ToolbarItem toolbarItem = new ToolbarItem();
      ((MenuItem) toolbarItem).Text = "";
      this.tItem = toolbarItem;
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(this.tItem);
      this.SetInfo();
      ((ICollection<Effect>) ((Element) this.btnShelfNext).Effects).Add((Effect) new LongPressedEffect());
      ((ICollection<Effect>) ((Element) this.btnShelfBack).Effects).Add((Effect) new LongPressedEffect());
      ((ICollection<Effect>) ((Element) this.txtShelfBarcode).Effects).Add((Effect) new LongPressedEffect());
      LongPressedEffect.SetCommand((BindableObject) this.btnShelfNext, this.LongPress);
      LongPressedEffect.SetCommand((BindableObject) this.btnShelfBack, this.LongPress);
      LongPressedEffect.SetCommand((BindableObject) this.txtShelfBarcode, this.LongPressShelfBarcode);
      LongPressedEffect.SetCommand((BindableObject) this.txtSelectionCode, this.LongPressShelfBarcode);
      this.ShelfBarcodeFocus();
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
    }

    private ICommand LongPressShelfBarcode => (ICommand) new Command((Action) (async () =>
    {
      AGWayBillAlc agWayBillAlc = this;
      IEnumerable<pIOGetRemainingDispOrderByItemReturnModel> byItemReturnModels = agWayBillAlc.dispOrderList.GroupBy(c => new
      {
        ShelfCodes = c.ShelfCodes,
        DispOrderNumber = c.DispOrderNumber,
        CustomerDesc = c.CustomerDesc,
        CustomerCode = c.CustomerCode,
        Priority = c.Priority,
        SelectionCode = c.SelectionCode
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType5<string, string, string, string, int?, string>, pIOGetRemainingDispOrderByItemReturnModel>, pIOGetRemainingDispOrderByItemReturnModel>(gcs => new pIOGetRemainingDispOrderByItemReturnModel()
      {
        ShelfCodes = gcs.Key.ShelfCodes,
        DispOrderNumber = gcs.Key.DispOrderNumber,
        CustomerDesc = gcs.Key.CustomerDesc,
        CustomerCode = gcs.Key.CustomerCode,
        Priority = gcs.Key.Priority,
        SelectionCode = gcs.Key.SelectionCode
      });
      ListView shelfListview = GlobalMob.GetShelfListview("DispOrderNumber,ShelfCodes");
      ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) byItemReturnModels;
      shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(agWayBillAlc.SelectShelfBarcode_SelectedItem);
      SelectItem selectItem = new SelectItem(shelfListview, "Raf Seçiniz");
      await ((NavigableElement) agWayBillAlc).Navigation.PushAsync((Page) selectItem);
    }));

    private void SelectShelfBarcode_SelectedItem(object sender, SelectedItemChangedEventArgs e)
    {
      this.selectedDispOrder = (pIOGetRemainingDispOrderByItemReturnModel) e.SelectedItem;
      if (this.selectedDispOrder == null)
        return;
      this.SetInfo();
      if (this.selectedDispOrder.ShelfCodes.Split(',').Length != 0)
      {
        ((InputView) this.txtShelfBarcode).Text = this.selectedDispOrder.ShelfCodes.Split(',')[0];
        ((IEntryController) this.txtShelfBarcode).SendCompleted();
      }
      ((NavigableElement) this).Navigation.PopAsync();
    }

    private ICommand LongPress => (ICommand) new Command((Action) (async () =>
    {
      AGWayBillAlc agWayBillAlc = this;
      IEnumerable<pIOGetRemainingDispOrderByItemReturnModel> byItemReturnModels = agWayBillAlc.dispOrderList.GroupBy(c => new
      {
        ShelfCodes = c.ShelfCodes,
        DispOrderNumber = c.DispOrderNumber,
        CustomerDesc = c.CustomerDesc,
        CustomerCode = c.CustomerCode,
        Priority = c.Priority,
        SelectionCode = c.SelectionCode
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType5<string, string, string, string, int?, string>, pIOGetRemainingDispOrderByItemReturnModel>, pIOGetRemainingDispOrderByItemReturnModel>(gcs => new pIOGetRemainingDispOrderByItemReturnModel()
      {
        ShelfCodes = gcs.Key.ShelfCodes,
        DispOrderNumber = gcs.Key.DispOrderNumber,
        CustomerDesc = gcs.Key.CustomerDesc,
        CustomerCode = gcs.Key.CustomerCode,
        Priority = gcs.Key.Priority,
        SelectionCode = gcs.Key.SelectionCode
      });
      ListView shelfListview = GlobalMob.GetShelfListview("DispOrderNumber,ShelfCodes");
      ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) byItemReturnModels;
      shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(agWayBillAlc.Select_SelectedItem);
      SelectItem selectItem = new SelectItem(shelfListview, "Raf Seçiniz");
      await ((NavigableElement) agWayBillAlc).Navigation.PushAsync((Page) selectItem);
    }));

    private void Select_SelectedItem(object sender, SelectedItemChangedEventArgs e)
    {
      this.selectedDispOrder = (pIOGetRemainingDispOrderByItemReturnModel) e.SelectedItem;
      if (this.selectedDispOrder == null)
        return;
      this.SetInfo();
      ((NavigableElement) this).Navigation.PopAsync();
    }

    private void SetInfo()
    {
      string.Join(",", this.dispOrderList.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x => x.ShelfCodes == this.selectedDispOrder.ShelfCodes)).ToList<pIOGetRemainingDispOrderByItemReturnModel>().Select<pIOGetRemainingDispOrderByItemReturnModel, string>((Func<pIOGetRemainingDispOrderByItemReturnModel, string>) (m => m.DispOrderNumber)).Distinct<string>().ToArray<string>());
      ((InputView) this.txtShelf).Text = this.selectedDispOrder.ShelfCodes;
      ((InputView) this.txtShelf).Placeholder = string.IsNullOrEmpty(this.selectedDispOrder.ShelfCodes) ? "Cariye tanımlı raf yok" : "";
      ((InputView) this.txtSelectionCode).Text = this.selectedDispOrder.CustomerDesc;
      ((VisualElement) this.btnShelfNext).IsEnabled = false;
      ((VisualElement) this.btnShelfBack).IsEnabled = false;
      int num = this.dispOrderList.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x =>
      {
        int? priority1 = x.Priority;
        int? priority2 = this.selectedDispOrder.Priority;
        return priority1.GetValueOrDefault() > priority2.GetValueOrDefault() & priority1.HasValue & priority2.HasValue;
      })).Any<pIOGetRemainingDispOrderByItemReturnModel>() ? 1 : 0;
      bool flag = this.dispOrderList.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x =>
      {
        int? priority3 = x.Priority;
        int? priority4 = this.selectedDispOrder.Priority;
        return priority3.GetValueOrDefault() < priority4.GetValueOrDefault() & priority3.HasValue & priority4.HasValue;
      })).Any<pIOGetRemainingDispOrderByItemReturnModel>();
      if (num != 0)
        ((VisualElement) this.btnShelfNext).IsEnabled = true;
      if (!flag)
        return;
      ((VisualElement) this.btnShelfNext).IsEnabled = true;
    }

    private void LoadDetails()
    {
      this.dispOrderDetails = this.dispOrderList.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x => x.ShelfCodes == this.selectedDispOrder.ShelfCodes)).ToList<pIOGetRemainingDispOrderByItemReturnModel>();
      foreach (pIOGetRemainingDispOrderByItemReturnModel dispOrderDetail in this.dispOrderDetails)
      {
        pIOGetRemainingDispOrderByItemReturnModel item = dispOrderDetail;
        int num = this.goodsInDetailList.Where<pIOGetErpGoodsInDetailReturnModel>((Func<pIOGetErpGoodsInDetailReturnModel, bool>) (x => x.ItemCode == item.ItemCode && x.ColorCode == item.ColorCode && x.ItemDim1Code == item.ItemDim1Code && x.ItemDim2Code == item.ItemDim2Code && x.ItemDim3Code == item.ItemDim3Code)).Sum<pIOGetErpGoodsInDetailReturnModel>((Func<pIOGetErpGoodsInDetailReturnModel, int>) (x => x.RemainingQty));
        item.ShipmentRemainingQty = new int?(num);
      }
      this.RefreshData();
    }

    private void ShelfBarcodeFocus() => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(250);
      ((InputView) this.txtShelfBarcode).Text = "";
      ((VisualElement) this.txtShelfBarcode)?.Focus();
    }));

    private void BarcodeFocus() => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(250);
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode)?.Focus();
    }));

    private async void txtBarcode_Completed(object sender, EventArgs e)
    {
      AGWayBillAlc page = this;
      string barcode = ((InputView) page.txtBarcode).Text;
      if (string.IsNullOrEmpty(barcode))
        return;
      if (page.dispOrderDetails == null || page.selectShelf == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Lütfen raf okutunuz", "", "Tamam") ? 1 : 0;
        ((InputView) page.txtBarcode).Text = "";
        page.ShelfBarcodeFocus();
      }
      else
      {
        if (string.IsNullOrEmpty(((InputView) page.txtQty).Text))
          ((InputView) page.txtQty).Text = "1";
        int qty = Convert.ToInt32(((InputView) page.txtQty).Text);
        pIOGetRemainingDispOrderByItemReturnModel byItemReturnModel1 = page.dispOrderDetailsGroup.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x =>
        {
          if (!x.Barcode.Contains(barcode))
            return false;
          int? alcQty = x.AlcQty;
          int num = qty;
          double? nullable = alcQty.HasValue ? new double?((double) (alcQty.GetValueOrDefault() + num)) : new double?();
          double qty1 = x.Qty;
          return nullable.GetValueOrDefault() <= qty1 & nullable.HasValue;
        })).FirstOrDefault<pIOGetRemainingDispOrderByItemReturnModel>();
        if (byItemReturnModel1 != null)
        {
          pIOGetErpGoodsInDetailReturnModel detailReturnModel1 = page.goodsInDetailList.Where<pIOGetErpGoodsInDetailReturnModel>((Func<pIOGetErpGoodsInDetailReturnModel, bool>) (x => x.Barcode.Contains(barcode) && x.RemainingQty > 0)).FirstOrDefault<pIOGetErpGoodsInDetailReturnModel>();
          if (detailReturnModel1 != null)
          {
            ztIOPurchaseOrderAllocate purchaseOrderAllocate = new ztIOPurchaseOrderAllocate()
            {
              CreatedDate = DateTime.Now,
              CreatedUserName = GlobalMob.User.UserName,
              DispOrderLineID = byItemReturnModel1.OrderLineID,
              ShipmentLineID = detailReturnModel1.ShipmentLineID,
              ShelfID = page.selectShelf.ShelfID,
              OrderQty = qty
            };
            ReturnModel result = GlobalMob.PostJson("SavePurchaseOrderAllocate", new Dictionary<string, string>()
            {
              {
                "json",
                JsonConvert.SerializeObject((object) new PurchaseOrderAllocateAndTrans()
                {
                  Allocate = purchaseOrderAllocate,
                  Trans = new ShelfTransaction()
                  {
                    Barcode = barcode,
                    ColorCode = detailReturnModel1.ColorCode,
                    DocumentNumber = "",
                    ItemCode = detailReturnModel1.ItemCode,
                    ItemDim1Code = detailReturnModel1.ItemDim1Code,
                    ItemDim2Code = detailReturnModel1.ItemDim2Code,
                    ItemDim3Code = detailReturnModel1.ItemDim3Code,
                    ProcessTypeID = 1,
                    Qty = qty,
                    ShelfID = page.selectShelf.ShelfID,
                    TransTypeID = 6,
                    WareHouseCode = page.selectShelf.WarehouseCode,
                    UserName = GlobalMob.User.UserName
                  }
                })
              }
            }, (ContentPage) page).Result;
            if (!result.Success)
              return;
            if (JsonConvert.DeserializeObject<ztIOShelfTransactionDetail>(result.Result) != null)
            {
              pIOGetErpGoodsInDetailReturnModel detailReturnModel2 = detailReturnModel1;
              int? nullable1 = detailReturnModel2.AlcQty;
              int num1 = qty;
              detailReturnModel2.AlcQty = nullable1.HasValue ? new int?(nullable1.GetValueOrDefault() + num1) : new int?();
              bool flag = false;
              foreach (pIOGetRemainingDispOrderByItemReturnModel dispOrderDetail in page.dispOrderDetails)
              {
                pIOGetRemainingDispOrderByItemReturnModel item = dispOrderDetail;
                if (item.OrderLineID == byItemReturnModel1.OrderLineID)
                {
                  nullable1 = item.ShelfID;
                  int? nullable2 = byItemReturnModel1.ShelfID;
                  if (nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue && !flag)
                  {
                    flag = true;
                    pIOGetRemainingDispOrderByItemReturnModel byItemReturnModel2 = item;
                    nullable2 = byItemReturnModel2.AlcQty;
                    int num2 = qty;
                    int? nullable3;
                    if (!nullable2.HasValue)
                    {
                      nullable1 = new int?();
                      nullable3 = nullable1;
                    }
                    else
                      nullable3 = new int?(nullable2.GetValueOrDefault() + num2);
                    byItemReturnModel2.AlcQty = nullable3;
                    item.LastReadBarcode = true;
                  }
                }
                int num3 = page.goodsInDetailList.Where<pIOGetErpGoodsInDetailReturnModel>((Func<pIOGetErpGoodsInDetailReturnModel, bool>) (x => x.ItemCode == item.ItemCode && x.ColorCode == item.ColorCode && x.ItemDim1Code == item.ItemDim1Code && x.ItemDim2Code == item.ItemDim2Code && x.ItemDim3Code == item.ItemDim3Code)).Sum<pIOGetErpGoodsInDetailReturnModel>((Func<pIOGetErpGoodsInDetailReturnModel, int>) (x => x.RemainingQty));
                item.ShipmentRemainingQty = new int?(num3);
              }
              ((InputView) page.txtQty).Text = "1";
              page.RefreshData();
              GlobalMob.PlaySave();
              page.BarcodeFocus();
            }
            else
            {
              GlobalMob.PlayError();
              int num = await ((Page) page).DisplayAlert("Bilgi", "Geçersiz Barkot:" + barcode, "", "Tamam") ? 1 : 0;
              page.BarcodeFocus();
            }
          }
          else
          {
            GlobalMob.PlayError();
            int num = await ((Page) page).DisplayAlert("Bilgi", "Ürün Mal kabul irsaliyesinde bulunamadı", "", "Tamam") ? 1 : 0;
            page.BarcodeFocus();
          }
        }
        else
        {
          string str = page.dispOrderDetails.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x => x.Barcode.Contains(barcode))).FirstOrDefault<pIOGetRemainingDispOrderByItemReturnModel>() != null ? "Ürün daha önce tamamlandı." : "Ürün siparişte bulunamadı";
          GlobalMob.PlayError();
          int num = await ((Page) page).DisplayAlert("Bilgi", str, "", "Tamam") ? 1 : 0;
          page.BarcodeFocus();
        }
      }
    }

    private void RefreshData()
    {
      this.dispOrderDetails = this.dispOrderDetails.OrderByDescending<pIOGetRemainingDispOrderByItemReturnModel, bool>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x => x.LastReadBarcode)).ToList<pIOGetRemainingDispOrderByItemReturnModel>();
      this.dispOrderDetailsGroup = this.dispOrderDetails.GroupBy(c => new
      {
        Barcode = c.Barcode,
        OrderLineID = c.OrderLineID,
        ColorCode = c.ColorCode,
        ItemCode = c.ItemCode,
        ItemDim1Code = c.ItemDim1Code,
        ItemDim2Code = c.ItemDim2Code,
        ItemDim3Code = c.ItemDim3Code,
        ItemTypeCode = c.ItemTypeCode,
        Qty = c.Qty,
        ShelfID = c.ShelfID,
        DispOrderNumber = c.DispOrderNumber,
        ShipmentRemainingQty = c.ShipmentRemainingQty
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType6<string, Guid, string, string, string, string, string, byte, double, int?, string, int?>, pIOGetRemainingDispOrderByItemReturnModel>, pIOGetRemainingDispOrderByItemReturnModel>(gcs => new pIOGetRemainingDispOrderByItemReturnModel()
      {
        Barcode = gcs.Key.Barcode,
        ColorCode = gcs.Key.ColorCode,
        ItemCode = gcs.Key.ItemCode,
        ItemDim1Code = gcs.Key.ItemDim1Code,
        ItemDim2Code = gcs.Key.ItemDim2Code,
        ItemDim3Code = gcs.Key.ItemDim3Code,
        ItemTypeCode = gcs.Key.ItemTypeCode,
        DispOrderNumber = gcs.Key.DispOrderNumber,
        ShipmentRemainingQty = gcs.Key.ShipmentRemainingQty,
        OrderLineID = gcs.Key.OrderLineID,
        Qty = gcs.Key.Qty,
        ShelfID = gcs.Key.ShelfID,
        LastReadBarcode = gcs.Max<pIOGetRemainingDispOrderByItemReturnModel, bool>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x => x.LastReadBarcode)),
        AlcQty = gcs.Sum<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, int?>) (x => x.AlcQty))
      }).ToList<pIOGetRemainingDispOrderByItemReturnModel>();
      ((ItemsView<Cell>) this.lstDispOrderDetails).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstDispOrderDetails).ItemsSource = (IEnumerable) this.dispOrderDetailsGroup;
      this.SetToolbarText();
    }

    private void SetToolbarText()
    {
      try
      {
        int int32_1 = Convert.ToInt32((object) this.dispOrderDetails.Sum<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, int?>) (x => x.AlcQty)));
        int int32_2 = Convert.ToInt32(this.dispOrderDetails.Sum<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, double>) (x => x.Qty)));
        ((MenuItem) this.tItem).Text = int32_1.ToString() + "/" + int32_2.ToString();
      }
      catch (Exception ex)
      {
      }
    }

    private void btnShelfNext_Clicked(object sender, EventArgs e)
    {
      pIOGetRemainingDispOrderByItemReturnModel byItemReturnModel = this.dispOrderList.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x =>
      {
        int? priority1 = x.Priority;
        int? priority2 = this.selectedDispOrder.Priority;
        return priority1.GetValueOrDefault() > priority2.GetValueOrDefault() & priority1.HasValue & priority2.HasValue;
      })).OrderBy<pIOGetRemainingDispOrderByItemReturnModel, int?>((Func<pIOGetRemainingDispOrderByItemReturnModel, int?>) (x => x.Priority)).FirstOrDefault<pIOGetRemainingDispOrderByItemReturnModel>();
      if (byItemReturnModel == null)
        return;
      this.selectedDispOrder = byItemReturnModel;
      this.SetInfo();
      this.BarcodeFocus();
    }

    private void btnShelfBack_Clicked(object sender, EventArgs e)
    {
      pIOGetRemainingDispOrderByItemReturnModel byItemReturnModel = this.dispOrderList.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x =>
      {
        int? priority1 = x.Priority;
        int? priority2 = this.selectedDispOrder.Priority;
        return priority1.GetValueOrDefault() < priority2.GetValueOrDefault() & priority1.HasValue & priority2.HasValue;
      })).OrderBy<pIOGetRemainingDispOrderByItemReturnModel, int?>((Func<pIOGetRemainingDispOrderByItemReturnModel, int?>) (x => x.Priority)).FirstOrDefault<pIOGetRemainingDispOrderByItemReturnModel>();
      if (byItemReturnModel == null)
        return;
      this.selectedDispOrder = byItemReturnModel;
      this.SetInfo();
      this.BarcodeFocus();
    }

    private void txtShelfBarcode_Completed(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(((InputView) this.txtShelfBarcode).Text))
        return;
      if (!this.selectedDispOrder.IsFree && !("," + ((InputView) this.txtShelf).Text + ",").Contains("," + ((InputView) this.txtShelfBarcode).Text + ","))
      {
        ((Page) this).DisplayAlert("Hata", "Lütfen sıradaki rafı okutunuz.", "", "Tamam");
        this.ShelfBarcodeFocus();
      }
      else
      {
        ReturnModel shelf = GlobalMob.GetShelf(((InputView) this.txtShelfBarcode).Text, (ContentPage) this);
        if (shelf.Success)
        {
          this.selectShelf = JsonConvert.DeserializeObject<ztIOShelf>(shelf.Result);
          this.LoadDetails();
          this.BarcodeFocus();
        }
        else
          ((Page) this).DisplayAlert("Bilgi", "Raf Bulunamadı", "", "Tamam");
      }
    }

    private void btnClearShelf_Clicked(object sender, EventArgs e)
    {
      this.selectShelf = (ztIOShelf) null;
      this.ShelfBarcodeFocus();
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (AGWayBillAlc).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/AGWayBillAlc.xaml",
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
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension1 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        Button button1 = new Button();
        ReferenceExtension referenceExtension3 = new ReferenceExtension();
        BindingExtension bindingExtension3 = new BindingExtension();
        ReferenceExtension referenceExtension4 = new ReferenceExtension();
        BindingExtension bindingExtension4 = new BindingExtension();
        Button button2 = new Button();
        StackLayout stackLayout1 = new StackLayout();
        Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
        StackLayout stackLayout2 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry1 = new SoftkeyboardDisabledEntry();
        ReferenceExtension referenceExtension5 = new ReferenceExtension();
        BindingExtension bindingExtension5 = new BindingExtension();
        ReferenceExtension referenceExtension6 = new ReferenceExtension();
        BindingExtension bindingExtension6 = new BindingExtension();
        Button button3 = new Button();
        StackLayout stackLayout3 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry2 = new SoftkeyboardDisabledEntry();
        Xamarin.Forms.Entry entry3 = new Xamarin.Forms.Entry();
        StackLayout stackLayout4 = new StackLayout();
        BindingExtension bindingExtension7 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout5 = new StackLayout();
        AGWayBillAlc agWayBillAlc;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (agWayBillAlc = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) agWayBillAlc, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("agwaybillalc", (object) agWayBillAlc);
        if (((Element) agWayBillAlc).StyleId == null)
          ((Element) agWayBillAlc).StyleId = "agwaybillalc";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout5);
        if (((Element) stackLayout5).StyleId == null)
          ((Element) stackLayout5).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtSelectionCode", (object) entry1);
        if (((Element) entry1).StyleId == null)
          ((Element) entry1).StyleId = "txtSelectionCode";
        ((INameScope) nameScope).RegisterName("btnShelfBack", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnShelfBack";
        ((INameScope) nameScope).RegisterName("btnShelfNext", (object) button2);
        if (((Element) button2).StyleId == null)
          ((Element) button2).StyleId = "btnShelfNext";
        ((INameScope) nameScope).RegisterName("txtShelf", (object) entry2);
        if (((Element) entry2).StyleId == null)
          ((Element) entry2).StyleId = "txtShelf";
        ((INameScope) nameScope).RegisterName("stckShelf", (object) stackLayout3);
        if (((Element) stackLayout3).StyleId == null)
          ((Element) stackLayout3).StyleId = "stckShelf";
        ((INameScope) nameScope).RegisterName("txtShelfBarcode", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtShelfBarcode";
        ((INameScope) nameScope).RegisterName("btnClearShelf", (object) button3);
        if (((Element) button3).StyleId == null)
          ((Element) button3).StyleId = "btnClearShelf";
        ((INameScope) nameScope).RegisterName("stckBarcode", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckBarcode";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("txtQty", (object) entry3);
        if (((Element) entry3).StyleId == null)
          ((Element) entry3).StyleId = "txtQty";
        ((INameScope) nameScope).RegisterName("lstDispOrderDetails", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstDispOrderDetails";
        this.agwaybillalc = (ContentPage) agWayBillAlc;
        this.stckForm = stackLayout5;
        this.txtSelectionCode = entry1;
        this.btnShelfBack = button1;
        this.btnShelfNext = button2;
        this.txtShelf = entry2;
        this.stckShelf = stackLayout3;
        this.txtShelfBarcode = softkeyboardDisabledEntry1;
        this.btnClearShelf = button3;
        this.stckBarcode = stackLayout4;
        this.txtBarcode = softkeyboardDisabledEntry2;
        this.txtQty = entry3;
        this.lstDispOrderDetails = listView;
        ((BindableObject) stackLayout5).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry1).SetValue(VisualElement.InputTransparentProperty, (object) true);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "<");
        referenceExtension1.Name = "agwaybillalc";
        ReferenceExtension referenceExtension7 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 5];
        objArray1[0] = (object) bindingExtension1;
        objArray1[1] = (object) button1;
        objArray1[2] = (object) stackLayout1;
        objArray1[3] = (object) stackLayout5;
        objArray1[4] = (object) agWayBillAlc;
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
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (AGWayBillAlc).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(15, 25)));
        object obj2 = ((IMarkupExtension) referenceExtension7).ProvideValue((IServiceProvider) xamlServiceProvider1);
        bindingExtension1.Source = obj2;
        VisualDiagnostics.RegisterSourceInfo(obj2, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 25);
        bindingExtension1.Path = "ButtonColor";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase1);
        ((BindableObject) button1).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        referenceExtension2.Name = "agwaybillalc";
        ReferenceExtension referenceExtension8 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 5];
        objArray2[0] = (object) bindingExtension2;
        objArray2[1] = (object) button1;
        objArray2[2] = (object) stackLayout1;
        objArray2[3] = (object) stackLayout5;
        objArray2[4] = (object) agWayBillAlc;
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
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (AGWayBillAlc).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(17, 25)));
        object obj4 = ((IMarkupExtension) referenceExtension8).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension2.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 25);
        bindingExtension2.Path = "TextColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(Button.TextColorProperty, bindingBase2);
        button1.Clicked += new EventHandler(agWayBillAlc.btnShelfBack_Clicked);
        ((BindableObject) button1).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 18);
        ((BindableObject) button2).SetValue(Button.TextProperty, (object) ">");
        button2.Clicked += new EventHandler(agWayBillAlc.btnShelfNext_Clicked);
        ((BindableObject) button2).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        referenceExtension3.Name = "agwaybillalc";
        ReferenceExtension referenceExtension9 = referenceExtension3;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 5];
        objArray3[0] = (object) bindingExtension3;
        objArray3[1] = (object) button2;
        objArray3[2] = (object) stackLayout1;
        objArray3[3] = (object) stackLayout5;
        objArray3[4] = (object) agWayBillAlc;
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
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (AGWayBillAlc).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(20, 25)));
        object obj6 = ((IMarkupExtension) referenceExtension9).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension3.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 25);
        bindingExtension3.Path = "ButtonColor";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase3);
        referenceExtension4.Name = "agwaybillalc";
        ReferenceExtension referenceExtension10 = referenceExtension4;
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 5];
        objArray4[0] = (object) bindingExtension4;
        objArray4[1] = (object) button2;
        objArray4[2] = (object) stackLayout1;
        objArray4[3] = (object) stackLayout5;
        objArray4[4] = (object) agWayBillAlc;
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
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (AGWayBillAlc).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(21, 25)));
        object obj8 = ((IMarkupExtension) referenceExtension10).ProvideValue((IServiceProvider) xamlServiceProvider4);
        bindingExtension4.Source = obj8;
        VisualDiagnostics.RegisterSourceInfo(obj8, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 21, 25);
        bindingExtension4.Path = "TextColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(Button.TextColorProperty, bindingBase4);
        ((BindableObject) button2).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button2);
        VisualDiagnostics.RegisterSourceInfo((object) button2, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 19, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 14);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "");
        ((BindableObject) entry2).SetValue(VisualElement.InputTransparentProperty, (object) true);
        ((BindableObject) entry2).SetValue(VisualElement.WidthRequestProperty, (object) 100.0);
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 25, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 24, 14);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf No Okutunuz");
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(VisualElement.WidthRequestProperty, (object) 200.0);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        softkeyboardDisabledEntry1.Completed += new EventHandler(agWayBillAlc.txtShelfBarcode_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 29, 18);
        ((BindableObject) button3).SetValue(Button.TextProperty, (object) "x");
        referenceExtension5.Name = "agwaybillalc";
        ReferenceExtension referenceExtension11 = referenceExtension5;
        XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
        Type type9 = typeof (IProvideValueTarget);
        object[] objArray5 = new object[0 + 5];
        objArray5[0] = (object) bindingExtension5;
        objArray5[1] = (object) button3;
        objArray5[2] = (object) stackLayout3;
        objArray5[3] = (object) stackLayout5;
        objArray5[4] = (object) agWayBillAlc;
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
        XamlTypeResolver xamlTypeResolver5 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver5, typeof (AGWayBillAlc).GetTypeInfo().Assembly);
        xamlServiceProvider5.Add(type10, (object) xamlTypeResolver5);
        xamlServiceProvider5.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(32, 25)));
        object obj10 = ((IMarkupExtension) referenceExtension11).ProvideValue((IServiceProvider) xamlServiceProvider5);
        bindingExtension5.Source = obj10;
        VisualDiagnostics.RegisterSourceInfo(obj10, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 32, 25);
        bindingExtension5.Path = "ButtonColor";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(VisualElement.BackgroundColorProperty, bindingBase5);
        referenceExtension6.Name = "agwaybillalc";
        ReferenceExtension referenceExtension12 = referenceExtension6;
        XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
        Type type11 = typeof (IProvideValueTarget);
        object[] objArray6 = new object[0 + 5];
        objArray6[0] = (object) bindingExtension6;
        objArray6[1] = (object) button3;
        objArray6[2] = (object) stackLayout3;
        objArray6[3] = (object) stackLayout5;
        objArray6[4] = (object) agWayBillAlc;
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
        XamlTypeResolver xamlTypeResolver6 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver6, typeof (AGWayBillAlc).GetTypeInfo().Assembly);
        xamlServiceProvider6.Add(type12, (object) xamlTypeResolver6);
        xamlServiceProvider6.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(33, 25)));
        object obj12 = ((IMarkupExtension) referenceExtension12).ProvideValue((IServiceProvider) xamlServiceProvider6);
        bindingExtension6.Source = obj12;
        VisualDiagnostics.RegisterSourceInfo(obj12, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 33, 25);
        bindingExtension6.Path = "TextColor";
        BindingBase bindingBase6 = ((IMarkupExtension<BindingBase>) bindingExtension6).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(Button.TextColorProperty, bindingBase6);
        button3.Clicked += new EventHandler(agWayBillAlc.btnClearShelf_Clicked);
        ((BindableObject) button3).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) button3);
        VisualDiagnostics.RegisterSourceInfo((object) button3, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 31, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 14);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod No Giriniz/Okutunuz");
        softkeyboardDisabledEntry2.Completed += new EventHandler(agWayBillAlc.txtBarcode_Completed);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 37, 18);
        ((BindableObject) entry3).SetValue(Xamarin.Forms.Entry.TextProperty, (object) "1");
        ((BindableObject) entry3).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Miktar");
        ((BindableObject) entry3).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry3).SetValue(Xamarin.Forms.Entry.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) entry3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) entry3);
        VisualDiagnostics.RegisterSourceInfo((object) entry3, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 38, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 36, 14);
        bindingExtension7.Path = ".";
        BindingBase bindingBase7 = ((IMarkupExtension<BindingBase>) bindingExtension7).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase7);
        ((BindableObject) listView).SetValue(ListView.RowHeightProperty, (object) 90);
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 5000.0);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AGWayBillAlc.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_6 xamlCdataTemplate6 = new AGWayBillAlc.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_6();
        object[] objArray7 = new object[0 + 4];
        objArray7[0] = (object) dataTemplate1;
        objArray7[1] = (object) listView;
        objArray7[2] = (object) stackLayout5;
        objArray7[3] = (object) agWayBillAlc;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate6.parentValues = objArray7;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate6.root = agWayBillAlc;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate6.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 42, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 40, 14);
        ((BindableObject) agWayBillAlc).SetValue(ContentPage.ContentProperty, (object) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 10);
        VisualDiagnostics.RegisterSourceInfo((object) agWayBillAlc, new Uri("Views\\AGWayBillAlc.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Extensions.LoadFromXaml<AGWayBillAlc>(this, typeof (AGWayBillAlc));
      this.agwaybillalc = NameScopeExtensions.FindByName<ContentPage>((Element) this, "agwaybillalc");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtSelectionCode = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtSelectionCode");
      this.btnShelfBack = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelfBack");
      this.btnShelfNext = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelfNext");
      this.txtShelf = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtShelf");
      this.stckShelf = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelf");
      this.txtShelfBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtShelfBarcode");
      this.btnClearShelf = NameScopeExtensions.FindByName<Button>((Element) this, "btnClearShelf");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.txtQty = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtQty");
      this.lstDispOrderDetails = NameScopeExtensions.FindByName<ListView>((Element) this, "lstDispOrderDetails");
    }
  }
}
