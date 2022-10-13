// Decompiled with JetBrains decompiler
// Type: Shelf.Views.StoreSorting
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
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;
using Xamarin.KeyboardHelper;
using XFNoSoftKeyboadEntryControl;

namespace Shelf.Views
{
  [XamlCompilation]
  [XamlFilePath("Views\\StoreSorting.xaml")]
  public class StoreSorting : ContentPage
  {
    private ztIOShelfPackageHeader selectPackageHeader;
    private pIOUserShelfOrdersForStorePivotReturnModel selectedShelfOrder;
    private List<pIOShelfOrderDetailForStorePivotReturnModel> detailList;
    private pIOShelfOrderDetailForStorePivotReturnModel selectedUser;
    private ToolbarItem tRefresh;
    private string lastCurrAccCode;
    private string lastCustomerName;
    private List<pIOShelfPackageDetailFromShelfOrderIDReturnModel> packageDetails;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage storepivot;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckContent;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelfOrderList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckEmptyMessage;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfOrder;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckFormAndList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtPackageNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnClear;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnCreateNewPackage;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtCustomerName;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtQty;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckBarcodeType;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnApprove;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfDetail;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Label lblListHeader;

    public Color ButtonColor => Color.FromRgb(0, 0, 0);

    public Color TextColor => Color.White;

    public StoreSorting()
    {
      this.InitializeComponent();
      ((ICollection<Effect>) ((Element) this.txtPackageNumber).Effects).Add((Effect) new LongPressedEffect());
      LongPressedEffect.SetCommand((BindableObject) this.txtPackageNumber, this.LongPressPackage);
      ToolbarItem toolbarItem = new ToolbarItem();
      ((MenuItem) toolbarItem).Text = "";
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(toolbarItem);
      ((Page) this).Title = "Pivotla";
      if (GlobalMob.User.IsBarcodeType)
      {
        List<PickerItem> pickerItemList = new List<PickerItem>();
        pickerItemList.Add(new PickerItem()
        {
          Caption = "Tek",
          Code = 1,
          Description = "Tek"
        });
        pickerItemList.Add(new PickerItem()
        {
          Caption = "Lot",
          Code = 2,
          Description = "Lot"
        });
        this.pckBarcodeType.ItemsSource = (IList) pickerItemList;
        this.pckBarcodeType.SelectedItem = (object) pickerItemList[0];
        ((VisualElement) this.pckBarcodeType).IsVisible = true;
      }
      this.LoadShelfOrder();
    }

    private async void TRefresh_Clicked(object sender, EventArgs e)
    {
      StoreSorting storeSorting = this;
      await NavigationExtension.PushPopupAsync(((NavigableElement) storeSorting).Navigation, GlobalMob.ShowLoading(), true);
      storeSorting.GetShelfOrderDetail();
      GlobalMob.CloseLoading();
    }

    private ICommand LongPressPackage => (ICommand) new Command((Action) (async () =>
    {
      StoreSorting page = this;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfPackageDetailFromShelfOrderID?shelfOrderID={0}", (object) page.selectedShelfOrder.ShelfOrderID), (ContentPage) page);
      if (!returnModel.Success)
        return;
      page.packageDetails = GlobalMob.JsonDeserialize<List<pIOShelfPackageDetailFromShelfOrderIDReturnModel>>(returnModel.Result);
      List<pIOShelfPackageDetailFromShelfOrderIDReturnModel> list = page.packageDetails.GroupBy(c => new
      {
        CustomerName = c.CustomerName,
        PackageCode = c.PackageCode
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType21<string, string>, pIOShelfPackageDetailFromShelfOrderIDReturnModel>, pIOShelfPackageDetailFromShelfOrderIDReturnModel>(gcs => new pIOShelfPackageDetailFromShelfOrderIDReturnModel()
      {
        CustomerName = gcs.Key.CustomerName,
        PackageCode = gcs.Key.PackageCode
      }).ToList<pIOShelfPackageDetailFromShelfOrderIDReturnModel>();
      ListView shelfListview = GlobalMob.GetShelfListview("CustomerName,PackageCode");
      ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) list;
      shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(page.Lst_ItemSelected);
      SelectItem selectItem = new SelectItem(shelfListview, "Ürünlerin Alınacağı Koliler");
      await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
    }));

    private async void Lst_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      StoreSorting storeSorting = this;
      pIOShelfPackageDetailFromShelfOrderIDReturnModel item = (pIOShelfPackageDetailFromShelfOrderIDReturnModel) e.SelectedItem;
      List<pIOShelfPackageDetailFromShelfOrderIDReturnModel> list = storeSorting.packageDetails.Where<pIOShelfPackageDetailFromShelfOrderIDReturnModel>((Func<pIOShelfPackageDetailFromShelfOrderIDReturnModel, bool>) (x => x.PackageCode == item.PackageCode)).ToList<pIOShelfPackageDetailFromShelfOrderIDReturnModel>();
      ListView listviewWithGrid = GlobalMob.GetShelfListviewWithGrid("ItemCodeLong,Qty");
      ((ItemsView<Cell>) listviewWithGrid).ItemsSource = (IEnumerable) list;
      int int32 = Convert.ToInt32((object) list.Sum<pIOShelfPackageDetailFromShelfOrderIDReturnModel>((Func<pIOShelfPackageDetailFromShelfOrderIDReturnModel, int?>) (x => x.Qty)));
      SelectItem selectItem = new SelectItem(listviewWithGrid, item.PackageCode + "(" + int32.ToString() + ")-" + item.CustomerName);
      await ((NavigableElement) storeSorting).Navigation.PushAsync((Page) selectItem);
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
    }

    private void LoadShelfOrder()
    {
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetUserShelfOrdersForStorePivot?userID={0}", (object) GlobalMob.User.UserID), (ContentPage) this);
      if (!returnModel.Success)
        return;
      List<pIOUserShelfOrdersForStorePivotReturnModel> pivotReturnModelList = GlobalMob.JsonDeserialize<List<pIOUserShelfOrdersForStorePivotReturnModel>>(returnModel.Result);
      this.lstShelfOrder.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(this.ShelfOrderSelect_SelectedItem);
      ((VisualElement) this.lstShelfOrder).IsVisible = pivotReturnModelList.Count > 0;
      ((VisualElement) this.stckEmptyMessage).IsVisible = pivotReturnModelList.Count == 0;
      if (((VisualElement) this.stckEmptyMessage).IsVisible)
        ((View) this.stckContent).VerticalOptions = LayoutOptions.Center;
      ((BindableObject) this.lstShelfOrder).BindingContext = (object) pivotReturnModelList;
    }

    private void GetShelfOrderDetail()
    {
      this.detailList = new List<pIOShelfOrderDetailForStorePivotReturnModel>();
      Device.BeginInvokeOnMainThread((Action) (() =>
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfDetailForStorePivot?shelfOrderID={0}", (object) this.selectedShelfOrder.ShelfOrderID), (ContentPage) this);
        if (!returnModel.Success)
          return;
        this.detailList = GlobalMob.JsonDeserialize<List<pIOShelfOrderDetailForStorePivotReturnModel>>(returnModel.Result);
        if (this.detailList.Count <= 0)
          return;
        List<pIOShelfOrderDetailForStorePivotReturnModel> list = this.detailList.GroupBy(c => new
        {
          FirstLastName = c.FirstLastName,
          ShelfUserID = c.ShelfUserID
        }).Select<IGrouping<\u003C\u003Ef__AnonymousType22<string, int?>, pIOShelfOrderDetailForStorePivotReturnModel>, pIOShelfOrderDetailForStorePivotReturnModel>(gcs => new pIOShelfOrderDetailForStorePivotReturnModel()
        {
          FirstLastName = gcs.Key.FirstLastName,
          ShelfUserID = gcs.Key.ShelfUserID
        }).ToList<pIOShelfOrderDetailForStorePivotReturnModel>();
        if (list.Count > 1)
        {
          if (this.selectedUser == null)
          {
            ListView shelfListview = GlobalMob.GetShelfListview("FirstLastName");
            ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) list;
            shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(this.LstUser_ItemSelected);
            ((NavigableElement) this).Navigation.PushAsync((Page) new SelectItem(shelfListview, "Kullanıcılar"));
          }
          else
            this.LstUser_ItemSelected((object) null, new SelectedItemChangedEventArgs((object) this.selectedUser, 0));
        }
        else
        {
          ((VisualElement) this.lstShelfDetail).IsVisible = true;
          ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
          ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) this.detailList;
          ((VisualElement) this.stckShelfOrderList).IsVisible = false;
          ((VisualElement) this.stckFormAndList).IsVisible = true;
          this.SetInfo();
          ((VisualElement) this.btnClear).IsVisible = this.selectedShelfOrder.ReadPackageCode;
          if (this.selectedShelfOrder.ReadPackageCode)
            Device.BeginInvokeOnMainThread((Action) (async () =>
            {
              await Task.Delay(200);
              ((InputView) this.txtPackageNumber).Text = "";
              ((VisualElement) this.txtPackageNumber).Focus();
            }));
          else
            Device.BeginInvokeOnMainThread((Action) (async () =>
            {
              await Task.Delay(200);
              this.BarcodeFocus();
            }));
        }
      }));
    }

    private void LstUser_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      pIOShelfOrderDetailForStorePivotReturnModel item = (pIOShelfOrderDetailForStorePivotReturnModel) e.SelectedItem;
      this.selectedUser = item;
      ((VisualElement) this.lstShelfDetail).IsVisible = true;
      ((VisualElement) this.stckShelfOrderList).IsVisible = false;
      ((VisualElement) this.stckFormAndList).IsVisible = true;
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
      this.detailList = this.detailList.Where<pIOShelfOrderDetailForStorePivotReturnModel>((Func<pIOShelfOrderDetailForStorePivotReturnModel, bool>) (x =>
      {
        int? shelfUserId1 = x.ShelfUserID;
        int? shelfUserId2 = item.ShelfUserID;
        return shelfUserId1.GetValueOrDefault() == shelfUserId2.GetValueOrDefault() & shelfUserId1.HasValue == shelfUserId2.HasValue;
      })).ToList<pIOShelfOrderDetailForStorePivotReturnModel>();
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) this.detailList;
      if (sender != null)
        ((NavigableElement) this).Navigation.PopAsync();
      this.SetInfo();
      Device.BeginInvokeOnMainThread((Action) (async () =>
      {
        await Task.Delay(200);
        this.BarcodeFocus();
      }));
    }

    private void BarcodeFocus()
    {
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode).Focus();
    }

    private void SetInfo()
    {
      double num1 = this.detailList.Sum<pIOShelfOrderDetailForStorePivotReturnModel>((Func<pIOShelfOrderDetailForStorePivotReturnModel, double>) (x => x.ApproveQty));
      double num2 = this.detailList.Sum<pIOShelfOrderDetailForStorePivotReturnModel>((Func<pIOShelfOrderDetailForStorePivotReturnModel, double>) (x => x.PickingQty));
      ((MenuItem) ((Page) this).ToolbarItems[0]).Text = num1.ToString() + "/" + num2.ToString();
    }

    private void ShelfOrderSelect_SelectedItem(object sender, SelectedItemChangedEventArgs e)
    {
      this.selectedShelfOrder = (pIOUserShelfOrdersForStorePivotReturnModel) e.SelectedItem;
      ((Page) this).Title = this.selectedShelfOrder.ShelfOrderNumber + "-" + ((Page) this).Title;
      ToolbarItem toolbarItem = new ToolbarItem();
      ((MenuItem) toolbarItem).Text = "";
      ((MenuItem) toolbarItem).Icon = FileImageSource.op_Implicit("refresh.png");
      this.tRefresh = toolbarItem;
      ((MenuItem) this.tRefresh).Clicked += new EventHandler(this.TRefresh_Clicked);
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(this.tRefresh);
      this.GetShelfOrderDetail();
    }

    private async void btnApprove_Clicked(object sender, EventArgs e)
    {
      StoreSorting page1 = this;
      if (!await ((Page) page1).DisplayAlert("Devam?", "Emri tamamlarsanız birdaha işlem yapamayacaksınız.Emri tamamlamak istiyor musunuz?", "Evet", "Hayır") || !GlobalMob.PostJson(string.Format("ShelfOrderApprove?shelfOrderID={0}", (object) page1.selectedShelfOrder.ShelfOrderID), (ContentPage) page1).Success)
        return;
      int num = await ((Page) page1).DisplayAlert("Bilgi", "Emir tamamlandı", "", "Tamam") ? 1 : 0;
      Page page2 = await ((NavigableElement) page1).Navigation.PopAsync();
    }

    private async void txtBarcode_Completed(object sender, EventArgs e)
    {
      StoreSorting page = this;
      string barcode = ((InputView) page.txtBarcode).Text;
      if (string.IsNullOrEmpty(barcode))
        ;
      else if (page.selectedShelfOrder.ReadPackageCode && page.selectPackageHeader == null)
      {
        page.ShowError("Lütfen öncelikle koli okutunuz");
        ((VisualElement) page.txtPackageNumber).Focus();
      }
      else
      {
        string str1 = "";
        bool isLot = false;
        PickerItem selectedItem = (PickerItem) page.pckBarcodeType.SelectedItem;
        if (selectedItem != null && selectedItem.Code == 2 && ((VisualElement) page.pckBarcodeType).IsVisible)
          isLot = true;
        int qty = page.GetQty();
        page.detailList.Select<pIOShelfOrderDetailForStorePivotReturnModel, pIOShelfOrderDetailForStorePivotReturnModel>((Func<pIOShelfOrderDetailForStorePivotReturnModel, pIOShelfOrderDetailForStorePivotReturnModel>) (c =>
        {
          c.LastReadBarcode = false;
          return c;
        })).ToList<pIOShelfOrderDetailForStorePivotReturnModel>();
        pIOShelfOrderDetailForStorePivotReturnModel item = page.GetItem(barcode);
        if (isLot && item == null)
          item = page.GetListWithOrderBy().Where<pIOShelfOrderDetailForStorePivotReturnModel>((Func<pIOShelfOrderDetailForStorePivotReturnModel, bool>) (x => x.LotBarcode == barcode)).FirstOrDefault<pIOShelfOrderDetailForStorePivotReturnModel>();
        if (item == null)
        {
          page.ShowError("Ürün Bulunamadı");
        }
        else
        {
          if (page.selectedShelfOrder.ReadPackageCode && page.selectPackageHeader != null)
            item.PackageHeaderID = new int?(page.selectPackageHeader.PackageHeaderID);
          int? packageHeaderId = item.PackageHeaderID;
          int num = 0;
          if (packageHeaderId.GetValueOrDefault() <= num & packageHeaderId.HasValue && !isLot && Convert.ToBoolean(page.selectedShelfOrder.IsAskPackageCode))
          {
            str1 = await GlobalMob.InputBox(((NavigableElement) page).Navigation, "Koli Kodu", "Koli kodu giriniz/okutunuz", Keyboard.Chat);
            if (string.IsNullOrEmpty(str1))
              return;
          }
          PickAndSort pickAndSort1 = new PickAndSort()
          {
            ShelfOrderDetailID = item.ShelfOrderDetailID,
            PickQty = qty,
            barcode = item.Barcode,
            userName = GlobalMob.User.UserName,
            DispOrderNumber = item.DispOrderNumber,
            PivotShelfCode = str1,
            ShelfOrderID = item.ShelfOrderID,
            ShelfOrderType = (int) item.ShelfOrderType,
            PackageHeaderID = Convert.ToInt32((object) item.PackageHeaderID),
            PackageDetailID = Convert.ToInt32((object) item.PackageDetailID),
            DispOrderLineID = item.DispOrderLineID,
            IsAskPackageCode = page.selectedShelfOrder.IsAskPackageCode,
            ReadPackageCode = page.selectedShelfOrder.ReadPackageCode
          };
          ReturnModel returnModel1 = new ReturnModel();
          Dictionary<string, string> paramList = new Dictionary<string, string>();
          ReturnModel result;
          if (isLot)
          {
            List<pIOShelfOrderDetailForStorePivotReturnModel> listWithOrderBy = page.GetListWithOrderBy();
            pickAndSort1.Detail = new List<PickAndSortDetail>();
            Func<pIOShelfOrderDetailForStorePivotReturnModel, bool> predicate = (Func<pIOShelfOrderDetailForStorePivotReturnModel, bool>) (x =>
            {
              if (x.DispOrderNumber == item.DispOrderNumber)
              {
                int? orderLineSumId1 = x.OrderLineSumID;
                int? orderLineSumId2 = item.OrderLineSumID;
                if (orderLineSumId1.GetValueOrDefault() == orderLineSumId2.GetValueOrDefault() & orderLineSumId1.HasValue == orderLineSumId2.HasValue)
                  return x.LotBarcode == barcode;
              }
              return false;
            });
            foreach (pIOShelfOrderDetailForStorePivotReturnModel pivotReturnModel in listWithOrderBy.Where<pIOShelfOrderDetailForStorePivotReturnModel>(predicate).ToList<pIOShelfOrderDetailForStorePivotReturnModel>())
              pickAndSort1.Detail.Add(new PickAndSortDetail()
              {
                Barcode = pivotReturnModel.Barcode,
                ColorCode = pivotReturnModel.ColorCode,
                DispOrderNumber = pivotReturnModel.DispOrderNumber,
                ItemCode = pivotReturnModel.ItemCode,
                ItemDim1Code = pivotReturnModel.ItemDim1Code,
                ItemDim2Code = pivotReturnModel.ItemDim2Code,
                OrderQty = pivotReturnModel.PickingQty,
                PickingQty = pivotReturnModel.ApproveQty,
                ShelfOrderDetailID = pivotReturnModel.ShelfOrderDetailID,
                ShelfOrderID = pivotReturnModel.ShelfOrderID,
                PackageHeaderID = page.selectPackageHeader != null ? Convert.ToInt32(page.selectPackageHeader.PackageHeaderID) : Convert.ToInt32((object) pivotReturnModel.PackageHeaderID)
              });
            pickAndSort1.barcode = ((InputView) page.txtBarcode).Text;
            string str2 = JsonConvert.SerializeObject((object) pickAndSort1);
            paramList.Add("json", str2);
            result = GlobalMob.PostJson("PackageForPivotLot", paramList, (ContentPage) page).Result;
          }
          else
          {
            string str3 = JsonConvert.SerializeObject((object) pickAndSort1);
            paramList.Add("json", str3);
            result = GlobalMob.PostJson("PackageForPivot", paramList, (ContentPage) page).Result;
          }
          if (result.Success)
          {
            ReturnModel returnModel2 = JsonConvert.DeserializeObject<ReturnModel>(result.Result);
            if (returnModel2.Success)
            {
              if (isLot)
              {
                List<PickAndSort> pickAndSortList = JsonConvert.DeserializeObject<List<PickAndSort>>(returnModel2.Result);
                GlobalMob.PlaySave();
                PickAndSort pickFirst = pickAndSortList[0];
                ((InputView) page.txtCustomerName).Text = pickFirst.PivotShelfCode;
                page.lastCustomerName = pickFirst.PivotShelfCode;
                pIOShelfOrderDetailForStorePivotReturnModel firstItem = page.detailList.Where<pIOShelfOrderDetailForStorePivotReturnModel>((Func<pIOShelfOrderDetailForStorePivotReturnModel, bool>) (x => x.ShelfOrderDetailID == pickFirst.ShelfOrderDetailID)).FirstOrDefault<pIOShelfOrderDetailForStorePivotReturnModel>();
                if (firstItem != null)
                {
                  page.lastCurrAccCode = firstItem.CurrAccCode;
                  foreach (pIOShelfOrderDetailForStorePivotReturnModel pivotReturnModel in page.detailList.Where<pIOShelfOrderDetailForStorePivotReturnModel>((Func<pIOShelfOrderDetailForStorePivotReturnModel, bool>) (x =>
                  {
                    if (x.CurrAccCode == firstItem.CurrAccCode)
                    {
                      byte? currAccTypeCode = x.CurrAccTypeCode;
                      int? nullable1 = currAccTypeCode.HasValue ? new int?((int) currAccTypeCode.GetValueOrDefault()) : new int?();
                      currAccTypeCode = firstItem.CurrAccTypeCode;
                      int? nullable2 = currAccTypeCode.HasValue ? new int?((int) currAccTypeCode.GetValueOrDefault()) : new int?();
                      if (nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue)
                      {
                        Guid? subCurrAccId1 = x.SubCurrAccID;
                        Guid? subCurrAccId2 = firstItem.SubCurrAccID;
                        if (subCurrAccId1.HasValue != subCurrAccId2.HasValue)
                          return false;
                        return !subCurrAccId1.HasValue || subCurrAccId1.GetValueOrDefault() == subCurrAccId2.GetValueOrDefault();
                      }
                    }
                    return false;
                  })).ToList<pIOShelfOrderDetailForStorePivotReturnModel>())
                  {
                    pivotReturnModel.PackageHeaderID = new int?(pickFirst.PackageHeaderID);
                    pivotReturnModel.PackageCode = pickFirst.userName;
                  }
                }
                foreach (PickAndSort pickAndSort2 in pickAndSortList)
                {
                  PickAndSort pickItem = pickAndSort2;
                  pIOShelfOrderDetailForStorePivotReturnModel pivotReturnModel = page.detailList.Where<pIOShelfOrderDetailForStorePivotReturnModel>((Func<pIOShelfOrderDetailForStorePivotReturnModel, bool>) (x => x.ShelfOrderDetailID == pickItem.ShelfOrderDetailID)).FirstOrDefault<pIOShelfOrderDetailForStorePivotReturnModel>();
                  page.lastCurrAccCode = pivotReturnModel.CurrAccCode;
                  pivotReturnModel.ApproveQty += (double) pickItem.PickQty;
                  pivotReturnModel.LastReadBarcode = true;
                  if (page.selectPackageHeader == null)
                    ((InputView) page.txtPackageNumber).Text = pivotReturnModel.PackageCode;
                }
                ((InputView) page.txtQty).Text = "1";
                page.RefreshDataSource();
                page.BarcodeFocus();
              }
              else
              {
                NewPackageHeader newPackageHeader = JsonConvert.DeserializeObject<NewPackageHeader>(returnModel2.Result);
                foreach (pIOShelfOrderDetailForStorePivotReturnModel pivotReturnModel in page.detailList.Where<pIOShelfOrderDetailForStorePivotReturnModel>((Func<pIOShelfOrderDetailForStorePivotReturnModel, bool>) (x =>
                {
                  if (x.PickingQty != x.ApproveQty && x.CurrAccCode == item.CurrAccCode)
                  {
                    byte? currAccTypeCode = x.CurrAccTypeCode;
                    int? nullable3 = currAccTypeCode.HasValue ? new int?((int) currAccTypeCode.GetValueOrDefault()) : new int?();
                    currAccTypeCode = item.CurrAccTypeCode;
                    int? nullable4 = currAccTypeCode.HasValue ? new int?((int) currAccTypeCode.GetValueOrDefault()) : new int?();
                    if (nullable3.GetValueOrDefault() == nullable4.GetValueOrDefault() & nullable3.HasValue == nullable4.HasValue)
                    {
                      Guid? subCurrAccId3 = x.SubCurrAccID;
                      Guid? subCurrAccId4 = item.SubCurrAccID;
                      if (subCurrAccId3.HasValue != subCurrAccId4.HasValue)
                        return false;
                      return !subCurrAccId3.HasValue || subCurrAccId3.GetValueOrDefault() == subCurrAccId4.GetValueOrDefault();
                    }
                  }
                  return false;
                })).ToList<pIOShelfOrderDetailForStorePivotReturnModel>())
                {
                  pivotReturnModel.PackageHeaderID = new int?(Convert.ToInt32(newPackageHeader.PackageHeaderID));
                  pivotReturnModel.PackageCode = newPackageHeader.PackageCode;
                }
                page.lastCurrAccCode = item.CurrAccCode;
                GlobalMob.PlaySave();
                if (!page.selectedShelfOrder.ReadPackageCode)
                  ((InputView) page.txtPackageNumber).Text = item.PackageCode;
                ((InputView) page.txtCustomerName).Text = newPackageHeader.CurrAccDesc;
                page.lastCustomerName = newPackageHeader.CurrAccDesc;
                item.ApproveQty += (double) qty;
                item.LastReadBarcode = true;
                ((InputView) page.txtQty).Text = "1";
                page.RefreshDataSource();
                page.BarcodeFocus();
              }
            }
            else if (returnModel2.ErrorMessage == "-1")
            {
              int detailID = Convert.ToInt32(returnModel2.Result);
              pIOShelfOrderDetailForStorePivotReturnModel fItem = page.detailList.Where<pIOShelfOrderDetailForStorePivotReturnModel>((Func<pIOShelfOrderDetailForStorePivotReturnModel, bool>) (x => x.ShelfOrderDetailID == detailID)).FirstOrDefault<pIOShelfOrderDetailForStorePivotReturnModel>();
              string str4 = await GlobalMob.InputBox(((NavigableElement) page).Navigation, "Koli Kodu-" + fItem.CurrAccCode, "Koli kodu giriniz/okutunuz", Keyboard.Chat);
              if (string.IsNullOrEmpty(str4))
              {
                page.ShowError("");
              }
              else
              {
                ztIOShelfPackageHeader newPackageHeader = new ztIOShelfPackageHeader()
                {
                  CreatedDate = DateTime.Now,
                  CreatedUserName = GlobalMob.User.UserName,
                  ShelfOrderID = page.selectedShelfOrder.ShelfOrderID,
                  UpdatedDate = new DateTime?(DateTime.Now),
                  UpdatedUserName = GlobalMob.User.UserName,
                  PackageDate = DateTime.Now,
                  CurrAccCode = fItem.CurrAccCode,
                  CurrAccTypeCode = new short?((short) Convert.ToSByte((object) fItem.CurrAccTypeCode)),
                  SubCurrAccID = fItem.SubCurrAccID,
                  PackageTypeID = new int?(0),
                  Description = str4
                };
                await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
                if (page.AddPackage(newPackageHeader, true))
                {
                  ((InputView) page.txtBarcode).Text = barcode;
                  ((IEntryController) page.txtBarcode).SendCompleted();
                }
                else
                  page.ShowError("");
                GlobalMob.CloseLoading();
                fItem = (pIOShelfOrderDetailForStorePivotReturnModel) null;
                newPackageHeader = (ztIOShelfPackageHeader) null;
              }
            }
            else
              page.ShowError(returnModel2.ErrorMessage);
          }
          else
            page.ShowError(result.ErrorMessage);
        }
      }
    }

    private int GetQty()
    {
      try
      {
        return Convert.ToInt32(((InputView) this.txtQty).Text);
      }
      catch (Exception ex)
      {
        return 1;
      }
    }

    private void ShowError(string msg)
    {
      if (!string.IsNullOrEmpty(msg))
        ((Page) this).DisplayAlert("Hata", msg, "", "Tamam");
      GlobalMob.PlayError();
      if (!this.selectedShelfOrder.ReadPackageCode)
        ((InputView) this.txtPackageNumber).Text = "";
      ((InputView) this.txtCustomerName).Text = "";
      this.BarcodeFocus();
    }

    private void RefreshDataSource()
    {
      this.SetInfo();
      this.detailList = this.detailList.OrderByDescending<pIOShelfOrderDetailForStorePivotReturnModel, bool>((Func<pIOShelfOrderDetailForStorePivotReturnModel, bool>) (x => x.LastReadBarcode)).ToList<pIOShelfOrderDetailForStorePivotReturnModel>();
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) this.detailList;
    }

    private pIOShelfOrderDetailForStorePivotReturnModel GetItem(
      string barcode)
    {
      List<pIOShelfOrderDetailForStorePivotReturnModel> listWithOrderBy = this.GetListWithOrderBy();
      return !GlobalMob.User.BarcodeSearchEqual ? listWithOrderBy.Where<pIOShelfOrderDetailForStorePivotReturnModel>((Func<pIOShelfOrderDetailForStorePivotReturnModel, bool>) (x => x.Barcode.Contains(barcode))).FirstOrDefault<pIOShelfOrderDetailForStorePivotReturnModel>() : listWithOrderBy.Where<pIOShelfOrderDetailForStorePivotReturnModel>((Func<pIOShelfOrderDetailForStorePivotReturnModel, bool>) (x => x.Barcode.Replace(",", "").Trim() == barcode)).FirstOrDefault<pIOShelfOrderDetailForStorePivotReturnModel>();
    }

    private List<pIOShelfOrderDetailForStorePivotReturnModel> GetListWithOrderBy()
    {
      List<pIOShelfOrderDetailForStorePivotReturnModel> pivotReturnModelList = new List<pIOShelfOrderDetailForStorePivotReturnModel>();
      return !this.selectedShelfOrder.ReadPackageCode || this.selectPackageHeader == null ? this.detailList.Where<pIOShelfOrderDetailForStorePivotReturnModel>((Func<pIOShelfOrderDetailForStorePivotReturnModel, bool>) (x => x.PickingQty != x.ApproveQty)).OrderBy<pIOShelfOrderDetailForStorePivotReturnModel, string>((Func<pIOShelfOrderDetailForStorePivotReturnModel, string>) (x => x.CurrAccCode)).OrderBy<pIOShelfOrderDetailForStorePivotReturnModel, int>((Func<pIOShelfOrderDetailForStorePivotReturnModel, int>) (x => !(x.CurrAccCode == this.lastCurrAccCode) ? 2 : 1)).ToList<pIOShelfOrderDetailForStorePivotReturnModel>() : this.detailList.Where<pIOShelfOrderDetailForStorePivotReturnModel>((Func<pIOShelfOrderDetailForStorePivotReturnModel, bool>) (x =>
      {
        if (x.PickingQty != x.ApproveQty)
        {
          byte? currAccTypeCode1 = x.CurrAccTypeCode;
          int? nullable1 = currAccTypeCode1.HasValue ? new int?((int) currAccTypeCode1.GetValueOrDefault()) : new int?();
          short? currAccTypeCode2 = this.selectPackageHeader.CurrAccTypeCode;
          int? nullable2 = currAccTypeCode2.HasValue ? new int?((int) currAccTypeCode2.GetValueOrDefault()) : new int?();
          if (nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue && x.CurrAccCode == this.selectPackageHeader.CurrAccCode)
          {
            Guid? subCurrAccId1 = x.SubCurrAccID;
            Guid? subCurrAccId2 = this.selectPackageHeader.SubCurrAccID;
            return (subCurrAccId1.HasValue == subCurrAccId2.HasValue ? (subCurrAccId1.HasValue ? (subCurrAccId1.GetValueOrDefault() == subCurrAccId2.GetValueOrDefault() ? 1 : 0) : 1) : 0) != 0 || !this.selectPackageHeader.SubCurrAccID.HasValue;
          }
        }
        return false;
      })).OrderBy<pIOShelfOrderDetailForStorePivotReturnModel, string>((Func<pIOShelfOrderDetailForStorePivotReturnModel, string>) (x => x.CurrAccCode)).OrderBy<pIOShelfOrderDetailForStorePivotReturnModel, int>((Func<pIOShelfOrderDetailForStorePivotReturnModel, int>) (x => !(x.CurrAccCode == this.lastCurrAccCode) ? 2 : 1)).ToList<pIOShelfOrderDetailForStorePivotReturnModel>();
    }

    private async void CreateNewPackage(
      int shelfOrderID,
      string currAccCode,
      int currAccTypeCode,
      Guid? subcurrAccID,
      string customerName)
    {
      StoreSorting storeSorting = this;
      string str = "";
      if (Convert.ToBoolean(storeSorting.selectedShelfOrder.IsAskPackageCode))
      {
        str = await GlobalMob.InputBox(((NavigableElement) storeSorting).Navigation, "Koli Kodu", "Koli kodu giriniz/okutunuz", Keyboard.Chat);
        if (string.IsNullOrEmpty(str))
          return;
      }
      ztIOShelfPackageHeader header = new ztIOShelfPackageHeader()
      {
        CreatedDate = DateTime.Now,
        CreatedUserName = GlobalMob.User.UserName,
        ShelfOrderID = shelfOrderID,
        UpdatedDate = new DateTime?(DateTime.Now),
        UpdatedUserName = GlobalMob.User.UserName,
        PackageDate = DateTime.Now,
        CurrAccCode = currAccCode,
        CurrAccTypeCode = new short?((short) Convert.ToSByte(currAccTypeCode)),
        SubCurrAccID = subcurrAccID,
        PackageTypeID = new int?(0),
        Description = str
      };
      storeSorting.AddPackage(header);
    }

    private bool AddPackage(ztIOShelfPackageHeader header, bool isQuiet = false)
    {
      ztIOShelfPackageHeader shelfPackageHeader = (ztIOShelfPackageHeader) null;
      Dictionary<string, string> paramList = new Dictionary<string, string>();
      string str = JsonConvert.SerializeObject((object) header);
      paramList.Add("json", str);
      ReturnModel result = GlobalMob.PostJson("CreateNewPackage", paramList, (ContentPage) this).Result;
      if (result.Success)
      {
        shelfPackageHeader = JsonConvert.DeserializeObject<ztIOShelfPackageHeader>(result.Result);
        if (shelfPackageHeader != null && shelfPackageHeader.PackageHeaderID > 0)
        {
          foreach (pIOShelfOrderDetailForStorePivotReturnModel pivotReturnModel in this.detailList.Where<pIOShelfOrderDetailForStorePivotReturnModel>((Func<pIOShelfOrderDetailForStorePivotReturnModel, bool>) (x =>
          {
            if (x.PickingQty != x.ApproveQty && x.CurrAccCode == header.CurrAccCode)
            {
              byte? currAccTypeCode1 = x.CurrAccTypeCode;
              int? nullable1 = currAccTypeCode1.HasValue ? new int?((int) currAccTypeCode1.GetValueOrDefault()) : new int?();
              short? currAccTypeCode2 = header.CurrAccTypeCode;
              int? nullable2 = currAccTypeCode2.HasValue ? new int?((int) currAccTypeCode2.GetValueOrDefault()) : new int?();
              if (nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue)
              {
                Guid? subCurrAccId1 = x.SubCurrAccID;
                Guid? subCurrAccId2 = header.SubCurrAccID;
                if (subCurrAccId1.HasValue != subCurrAccId2.HasValue)
                  return false;
                return !subCurrAccId1.HasValue || subCurrAccId1.GetValueOrDefault() == subCurrAccId2.GetValueOrDefault();
              }
            }
            return false;
          })).ToList<pIOShelfOrderDetailForStorePivotReturnModel>())
          {
            pivotReturnModel.PackageHeaderID = new int?(shelfPackageHeader.PackageHeaderID);
            pivotReturnModel.PackageCode = shelfPackageHeader.Description;
          }
          if (!isQuiet)
          {
            ((NavigableElement) this).Navigation.PopAsync();
            ((Page) this).DisplayAlert("Yeni Koli", "Yeni Koli eklendi\nKoli No:" + shelfPackageHeader.Description, "", "Tamam");
            this.BarcodeFocus();
          }
        }
        else
        {
          ((Page) this).DisplayAlert("Hata", "Bu koli daha önce eklemiş\nKoli No:" + header.Description, "", "Tamam");
          if (!isQuiet)
          {
            ((NavigableElement) this).Navigation.PopAsync();
            this.BarcodeFocus();
          }
        }
      }
      return shelfPackageHeader != null;
    }

    private async void btnCreateNewPackage_Clicked(object sender, EventArgs e)
    {
      StoreSorting page = this;
      if (!await ((Page) page).DisplayAlert("Uyarı", "Yeni koli oluşturulacak emin misiniz?", "Evet", "Hayır"))
        return;
      ReturnModel returnModel = GlobalMob.PostJson("GetShelfOrderCustomer?shelfOrderID=" + page.selectedShelfOrder.ShelfOrderID.ToString(), (ContentPage) page);
      if (!returnModel.Success)
        return;
      List<pIOShelfOrderCustomerReturnModel> source = GlobalMob.JsonDeserialize<List<pIOShelfOrderCustomerReturnModel>>(returnModel.Result);
      if (source.Count<pIOShelfOrderCustomerReturnModel>() == 1)
      {
        SelectedItemChangedEventArgs e1 = new SelectedItemChangedEventArgs((object) source[0]);
        page.LstCustomer_ItemSelected((object) null, e1);
      }
      else
      {
        ListView shelfListview = GlobalMob.GetShelfListview("CurrAccCode,CurrAccDescription");
        ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) source;
        shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(page.LstCustomer_ItemSelected);
        SelectItem selectItem = new SelectItem(shelfListview, "Müşteri Seçiniz");
        await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
      }
    }

    private void LstCustomer_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      pIOShelfOrderCustomerReturnModel selectedItem = (pIOShelfOrderCustomerReturnModel) e.SelectedItem;
      this.CreateNewPackage(this.selectedShelfOrder.ShelfOrderID, selectedItem.CurrAccCode, Convert.ToInt32((object) selectedItem.CurrAccTypeCode), selectedItem.SubCurrAccID, selectedItem.CurrAccDescription);
    }

    private async void txtPackageNumber_Completed(object sender, EventArgs e)
    {
      StoreSorting page = this;
      if (!page.selectedShelfOrder.ReadPackageCode)
        return;
      string text = ((InputView) page.txtPackageNumber).Text;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetPackageHeaderFromCode?shelfOrderID={0}&pCode={1}", (object) page.selectedShelfOrder.ShelfOrderID, (object) text), (ContentPage) page);
      if (!returnModel.Success)
        return;
      page.selectPackageHeader = GlobalMob.JsonDeserialize<ztIOShelfPackageHeader>(returnModel.Result);
      if (page.selectPackageHeader == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Koli bulunamadı", "", "Tamam") ? 1 : 0;
        page.PackageBarcodeFocus();
      }
      else
      {
        ((InputView) page.txtCustomerName).Text = "";
        page.BarcodeFocus();
      }
    }

    private void btnClear_Clicked(object sender, EventArgs e)
    {
      this.selectPackageHeader = (ztIOShelfPackageHeader) null;
      this.PackageBarcodeFocus();
    }

    private void PackageBarcodeFocus()
    {
      ((InputView) this.txtPackageNumber).Text = "";
      ((VisualElement) this.txtPackageNumber).Focus();
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (StoreSorting).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/StoreSorting.xaml",
        Instance = (object) this
      }))
        this.__InitComponentRuntime();
      else if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(((object) this).GetType()) != null)
      {
        this.__InitComponentRuntime();
      }
      else
      {
        Label label1 = new Label();
        StackLayout stackLayout1 = new StackLayout();
        BindingExtension bindingExtension1 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView1 = new ListView();
        StackLayout stackLayout2 = new StackLayout();
        Xamarin.Forms.Entry entry1 = new Xamarin.Forms.Entry();
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension3 = new BindingExtension();
        Button button1 = new Button();
        ReferenceExtension referenceExtension3 = new ReferenceExtension();
        BindingExtension bindingExtension4 = new BindingExtension();
        ReferenceExtension referenceExtension4 = new ReferenceExtension();
        BindingExtension bindingExtension5 = new BindingExtension();
        Button button2 = new Button();
        StackLayout stackLayout3 = new StackLayout();
        Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
        StackLayout stackLayout4 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry = new SoftkeyboardDisabledEntry();
        Xamarin.Forms.Entry entry3 = new Xamarin.Forms.Entry();
        BindingExtension bindingExtension6 = new BindingExtension();
        BindingExtension bindingExtension7 = new BindingExtension();
        Picker picker = new Picker();
        StackLayout stackLayout5 = new StackLayout();
        ReferenceExtension referenceExtension5 = new ReferenceExtension();
        BindingExtension bindingExtension8 = new BindingExtension();
        ReferenceExtension referenceExtension6 = new ReferenceExtension();
        BindingExtension bindingExtension9 = new BindingExtension();
        Button button3 = new Button();
        StackLayout stackLayout6 = new StackLayout();
        BindingExtension bindingExtension10 = new BindingExtension();
        ReferenceExtension referenceExtension7 = new ReferenceExtension();
        BindingExtension bindingExtension11 = new BindingExtension();
        ReferenceExtension referenceExtension8 = new ReferenceExtension();
        BindingExtension bindingExtension12 = new BindingExtension();
        Label label2 = new Label();
        StackLayout stackLayout7 = new StackLayout();
        DataTemplate dataTemplate2 = new DataTemplate();
        ListView listView2 = new ListView();
        StackLayout stackLayout8 = new StackLayout();
        StackLayout stackLayout9 = new StackLayout();
        StoreSorting storeSorting;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (storeSorting = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) storeSorting, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("storepivot", (object) storeSorting);
        if (((Element) storeSorting).StyleId == null)
          ((Element) storeSorting).StyleId = "storepivot";
        ((INameScope) nameScope).RegisterName("stckContent", (object) stackLayout9);
        if (((Element) stackLayout9).StyleId == null)
          ((Element) stackLayout9).StyleId = "stckContent";
        ((INameScope) nameScope).RegisterName("stckShelfOrderList", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckShelfOrderList";
        ((INameScope) nameScope).RegisterName("stckEmptyMessage", (object) stackLayout1);
        if (((Element) stackLayout1).StyleId == null)
          ((Element) stackLayout1).StyleId = "stckEmptyMessage";
        ((INameScope) nameScope).RegisterName("lstShelfOrder", (object) listView1);
        if (((Element) listView1).StyleId == null)
          ((Element) listView1).StyleId = "lstShelfOrder";
        ((INameScope) nameScope).RegisterName("stckFormAndList", (object) stackLayout8);
        if (((Element) stackLayout8).StyleId == null)
          ((Element) stackLayout8).StyleId = "stckFormAndList";
        ((INameScope) nameScope).RegisterName("txtPackageNumber", (object) entry1);
        if (((Element) entry1).StyleId == null)
          ((Element) entry1).StyleId = "txtPackageNumber";
        ((INameScope) nameScope).RegisterName("btnClear", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnClear";
        ((INameScope) nameScope).RegisterName("btnCreateNewPackage", (object) button2);
        if (((Element) button2).StyleId == null)
          ((Element) button2).StyleId = "btnCreateNewPackage";
        ((INameScope) nameScope).RegisterName("txtCustomerName", (object) entry2);
        if (((Element) entry2).StyleId == null)
          ((Element) entry2).StyleId = "txtCustomerName";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry);
        if (((Element) softkeyboardDisabledEntry).StyleId == null)
          ((Element) softkeyboardDisabledEntry).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("txtQty", (object) entry3);
        if (((Element) entry3).StyleId == null)
          ((Element) entry3).StyleId = "txtQty";
        ((INameScope) nameScope).RegisterName("pckBarcodeType", (object) picker);
        if (((Element) picker).StyleId == null)
          ((Element) picker).StyleId = "pckBarcodeType";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout6);
        if (((Element) stackLayout6).StyleId == null)
          ((Element) stackLayout6).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("btnApprove", (object) button3);
        if (((Element) button3).StyleId == null)
          ((Element) button3).StyleId = "btnApprove";
        ((INameScope) nameScope).RegisterName("lstShelfDetail", (object) listView2);
        if (((Element) listView2).StyleId == null)
          ((Element) listView2).StyleId = "lstShelfDetail";
        ((INameScope) nameScope).RegisterName("lblListHeader", (object) label2);
        if (((Element) label2).StyleId == null)
          ((Element) label2).StyleId = "lblListHeader";
        this.storepivot = (ContentPage) storeSorting;
        this.stckContent = stackLayout9;
        this.stckShelfOrderList = stackLayout2;
        this.stckEmptyMessage = stackLayout1;
        this.lstShelfOrder = listView1;
        this.stckFormAndList = stackLayout8;
        this.txtPackageNumber = entry1;
        this.btnClear = button1;
        this.btnCreateNewPackage = button2;
        this.txtCustomerName = entry2;
        this.txtBarcode = softkeyboardDisabledEntry;
        this.txtQty = entry3;
        this.pckBarcodeType = picker;
        this.stckForm = stackLayout6;
        this.btnApprove = button3;
        this.lstShelfDetail = listView2;
        this.lblListHeader = label2;
        ((BindableObject) stackLayout2).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout1).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) stackLayout1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) stackLayout1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) label1).SetValue(Label.TextProperty, (object) "Bekleyen Raf Emri Bulunmamaktadır.");
        ((BindableObject) label1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.CenterAndExpand);
        ((BindableObject) label1).SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Label label3 = label1;
        BindableProperty fontSizeProperty1 = Label.FontSizeProperty;
        FontSizeConverter fontSizeConverter1 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 5];
        objArray1[0] = (object) label1;
        objArray1[1] = (object) stackLayout1;
        objArray1[2] = (object) stackLayout2;
        objArray1[3] = (object) stackLayout9;
        objArray1[4] = (object) storeSorting;
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
        namespaceResolver1.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (StoreSorting).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(16, 128)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter1).ConvertFromInvariantString("Medium", (IServiceProvider) xamlServiceProvider1);
        ((BindableObject) label3).SetValue(fontSizeProperty1, obj2);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) label1);
        VisualDiagnostics.RegisterSourceInfo((object) label1, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 18);
        bindingExtension1.Path = ".";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView1).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase1);
        ((BindableObject) listView1).SetValue(ListView.RowHeightProperty, (object) 60);
        ((BindableObject) listView1).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        ((BindableObject) listView1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        DataTemplate dataTemplate3 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        StoreSorting.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_40 xamlCdataTemplate40 = new StoreSorting.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_40();
        object[] objArray2 = new object[0 + 5];
        objArray2[0] = (object) dataTemplate1;
        objArray2[1] = (object) listView1;
        objArray2[2] = (object) stackLayout2;
        objArray2[3] = (object) stackLayout9;
        objArray2[4] = (object) storeSorting;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate40.parentValues = objArray2;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate40.root = storeSorting;
        // ISSUE: reference to a compiler-generated method
        Func<object> func1 = new Func<object>(xamlCdataTemplate40.LoadDataTemplate);
        ((IDataTemplate) dataTemplate3).LoadTemplate = func1;
        ((BindableObject) listView1).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) listView1);
        VisualDiagnostics.RegisterSourceInfo((object) listView1, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 18, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout9).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 14);
        ((BindableObject) stackLayout8).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) stackLayout8).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Koli Kodu");
        ((BindableObject) entry1).SetValue(VisualElement.BackgroundColorProperty, (object) Color.White);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) entry1).SetValue(KeyboardEffect.EnableKeyboardProperty, (object) false);
        entry1.Completed += new EventHandler(storeSorting.txtPackageNumber_Completed);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.TextColorProperty, (object) Color.Red);
        Xamarin.Forms.Entry entry4 = entry1;
        BindableProperty fontSizeProperty2 = Xamarin.Forms.Entry.FontSizeProperty;
        FontSizeConverter fontSizeConverter2 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 5];
        objArray3[0] = (object) entry1;
        objArray3[1] = (object) stackLayout3;
        objArray3[2] = (object) stackLayout8;
        objArray3[3] = (object) stackLayout9;
        objArray3[4] = (object) storeSorting;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray3, (object) Xamarin.Forms.Entry.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver2.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver2.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver2.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        namespaceResolver2.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (StoreSorting).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(51, 48)));
        object obj4 = ((IExtendedTypeConverter) fontSizeConverter2).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider2);
        ((BindableObject) entry4).SetValue(fontSizeProperty2, obj4);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 47, 22);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "x");
        ((BindableObject) button1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((BindableObject) button1).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        button1.Clicked += new EventHandler(storeSorting.btnClear_Clicked);
        ((BindableObject) button1).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) button1).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        referenceExtension1.Name = "storepivot";
        ReferenceExtension referenceExtension9 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 6];
        objArray4[0] = (object) bindingExtension2;
        objArray4[1] = (object) button1;
        objArray4[2] = (object) stackLayout3;
        objArray4[3] = (object) stackLayout8;
        objArray4[4] = (object) stackLayout9;
        objArray4[5] = (object) storeSorting;
        SimpleValueTargetProvider valueTargetProvider3;
        object obj5 = (object) (valueTargetProvider3 = new SimpleValueTargetProvider(objArray4, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider3.Add(type5, (object) valueTargetProvider3);
        xamlServiceProvider3.Add(typeof (IReferenceProvider), obj5);
        Type type6 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver3 = new XmlNamespaceResolver();
        namespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver3.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver3.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver3.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        namespaceResolver3.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (StoreSorting).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(55, 28)));
        object obj6 = ((IMarkupExtension) referenceExtension9).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension2.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 55, 28);
        bindingExtension2.Path = "ButtonColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase2);
        referenceExtension2.Name = "storepivot";
        ReferenceExtension referenceExtension10 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray5 = new object[0 + 6];
        objArray5[0] = (object) bindingExtension3;
        objArray5[1] = (object) button1;
        objArray5[2] = (object) stackLayout3;
        objArray5[3] = (object) stackLayout8;
        objArray5[4] = (object) stackLayout9;
        objArray5[5] = (object) storeSorting;
        SimpleValueTargetProvider valueTargetProvider4;
        object obj7 = (object) (valueTargetProvider4 = new SimpleValueTargetProvider(objArray5, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider4.Add(type7, (object) valueTargetProvider4);
        xamlServiceProvider4.Add(typeof (IReferenceProvider), obj7);
        Type type8 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver4 = new XmlNamespaceResolver();
        namespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver4.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver4.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver4.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        namespaceResolver4.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (StoreSorting).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(56, 28)));
        object obj8 = ((IMarkupExtension) referenceExtension10).ProvideValue((IServiceProvider) xamlServiceProvider4);
        bindingExtension3.Source = obj8;
        VisualDiagnostics.RegisterSourceInfo(obj8, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 56, 28);
        bindingExtension3.Path = "TextColor";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(Button.TextColorProperty, bindingBase3);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 53, 22);
        ((BindableObject) button2).SetValue(Button.TextProperty, (object) "+");
        ((BindableObject) button2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((BindableObject) button2).SetValue(VisualElement.WidthRequestProperty, (object) 50.0);
        ((BindableObject) button2).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        button2.Clicked += new EventHandler(storeSorting.btnCreateNewPackage_Clicked);
        ((BindableObject) button2).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        referenceExtension3.Name = "storepivot";
        ReferenceExtension referenceExtension11 = referenceExtension3;
        XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
        Type type9 = typeof (IProvideValueTarget);
        object[] objArray6 = new object[0 + 6];
        objArray6[0] = (object) bindingExtension4;
        objArray6[1] = (object) button2;
        objArray6[2] = (object) stackLayout3;
        objArray6[3] = (object) stackLayout8;
        objArray6[4] = (object) stackLayout9;
        objArray6[5] = (object) storeSorting;
        SimpleValueTargetProvider valueTargetProvider5;
        object obj9 = (object) (valueTargetProvider5 = new SimpleValueTargetProvider(objArray6, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider5.Add(type9, (object) valueTargetProvider5);
        xamlServiceProvider5.Add(typeof (IReferenceProvider), obj9);
        Type type10 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver5 = new XmlNamespaceResolver();
        namespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver5.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver5.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver5.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        namespaceResolver5.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver5 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver5, typeof (StoreSorting).GetTypeInfo().Assembly);
        xamlServiceProvider5.Add(type10, (object) xamlTypeResolver5);
        xamlServiceProvider5.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(60, 28)));
        object obj10 = ((IMarkupExtension) referenceExtension11).ProvideValue((IServiceProvider) xamlServiceProvider5);
        bindingExtension4.Source = obj10;
        VisualDiagnostics.RegisterSourceInfo(obj10, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 60, 28);
        bindingExtension4.Path = "ButtonColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase4);
        referenceExtension4.Name = "storepivot";
        ReferenceExtension referenceExtension12 = referenceExtension4;
        XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
        Type type11 = typeof (IProvideValueTarget);
        object[] objArray7 = new object[0 + 6];
        objArray7[0] = (object) bindingExtension5;
        objArray7[1] = (object) button2;
        objArray7[2] = (object) stackLayout3;
        objArray7[3] = (object) stackLayout8;
        objArray7[4] = (object) stackLayout9;
        objArray7[5] = (object) storeSorting;
        SimpleValueTargetProvider valueTargetProvider6;
        object obj11 = (object) (valueTargetProvider6 = new SimpleValueTargetProvider(objArray7, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider6.Add(type11, (object) valueTargetProvider6);
        xamlServiceProvider6.Add(typeof (IReferenceProvider), obj11);
        Type type12 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver6 = new XmlNamespaceResolver();
        namespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver6.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver6.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver6.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        namespaceResolver6.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver6 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver6, typeof (StoreSorting).GetTypeInfo().Assembly);
        xamlServiceProvider6.Add(type12, (object) xamlTypeResolver6);
        xamlServiceProvider6.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(61, 28)));
        object obj12 = ((IMarkupExtension) referenceExtension12).ProvideValue((IServiceProvider) xamlServiceProvider6);
        bindingExtension5.Source = obj12;
        VisualDiagnostics.RegisterSourceInfo(obj12, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 61, 28);
        bindingExtension5.Path = "TextColor";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(Button.TextColorProperty, bindingBase5);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) button2);
        VisualDiagnostics.RegisterSourceInfo((object) button2, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 57, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 46, 18);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Müşteri Adı");
        ((BindableObject) entry2).SetValue(VisualElement.BackgroundColorProperty, (object) Color.White);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.TextColorProperty, (object) Color.Red);
        Xamarin.Forms.Entry entry5 = entry2;
        BindableProperty fontSizeProperty3 = Xamarin.Forms.Entry.FontSizeProperty;
        FontSizeConverter fontSizeConverter3 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
        Type type13 = typeof (IProvideValueTarget);
        object[] objArray8 = new object[0 + 5];
        objArray8[0] = (object) entry2;
        objArray8[1] = (object) stackLayout4;
        objArray8[2] = (object) stackLayout8;
        objArray8[3] = (object) stackLayout9;
        objArray8[4] = (object) storeSorting;
        SimpleValueTargetProvider valueTargetProvider7;
        object obj13 = (object) (valueTargetProvider7 = new SimpleValueTargetProvider(objArray8, (object) Xamarin.Forms.Entry.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider7.Add(type13, (object) valueTargetProvider7);
        xamlServiceProvider7.Add(typeof (IReferenceProvider), obj13);
        Type type14 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver7 = new XmlNamespaceResolver();
        namespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver7.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver7.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver7.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        namespaceResolver7.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver7 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver7, typeof (StoreSorting).GetTypeInfo().Assembly);
        xamlServiceProvider7.Add(type14, (object) xamlTypeResolver7);
        xamlServiceProvider7.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(65, 66)));
        object obj14 = ((IExtendedTypeConverter) fontSizeConverter3).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider7);
        ((BindableObject) entry5).SetValue(fontSizeProperty3, obj14);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 64, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 63, 18);
        ((BindableObject) stackLayout5).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        softkeyboardDisabledEntry.Completed += new EventHandler(storeSorting.txtBarcode_Completed);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod Numarası Giriniz");
        ((BindableObject) softkeyboardDisabledEntry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) softkeyboardDisabledEntry);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 69, 22);
        ((BindableObject) entry3).SetValue(Xamarin.Forms.Entry.TextProperty, (object) "1");
        ((BindableObject) entry3).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Miktar");
        ((BindableObject) entry3).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry3).SetValue(Xamarin.Forms.Entry.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) entry3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) entry3);
        VisualDiagnostics.RegisterSourceInfo((object) entry3, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 71, 22);
        ((BindableObject) picker).SetValue(Picker.TitleProperty, (object) "Tip");
        bindingExtension6.Path = ".";
        BindingBase bindingBase6 = ((IMarkupExtension<BindingBase>) bindingExtension6).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker).SetBinding(Picker.ItemsSourceProperty, bindingBase6);
        bindingExtension7.Path = "Caption";
        BindingBase bindingBase7 = ((IMarkupExtension<BindingBase>) bindingExtension7).ProvideValue((IServiceProvider) null);
        picker.ItemDisplayBinding = bindingBase7;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase7, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 74, 33);
        ((BindableObject) picker).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) picker).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) picker);
        VisualDiagnostics.RegisterSourceInfo((object) picker, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 73, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 68, 18);
        ((BindableObject) stackLayout6).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) button3).SetValue(Button.TextProperty, (object) "TAMAMLA");
        button3.Clicked += new EventHandler(storeSorting.btnApprove_Clicked);
        referenceExtension5.Name = "storepivot";
        ReferenceExtension referenceExtension13 = referenceExtension5;
        XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
        Type type15 = typeof (IProvideValueTarget);
        object[] objArray9 = new object[0 + 6];
        objArray9[0] = (object) bindingExtension8;
        objArray9[1] = (object) button3;
        objArray9[2] = (object) stackLayout6;
        objArray9[3] = (object) stackLayout8;
        objArray9[4] = (object) stackLayout9;
        objArray9[5] = (object) storeSorting;
        SimpleValueTargetProvider valueTargetProvider8;
        object obj15 = (object) (valueTargetProvider8 = new SimpleValueTargetProvider(objArray9, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider8.Add(type15, (object) valueTargetProvider8);
        xamlServiceProvider8.Add(typeof (IReferenceProvider), obj15);
        Type type16 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver8 = new XmlNamespaceResolver();
        namespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver8.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver8.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver8.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        namespaceResolver8.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver8 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver8, typeof (StoreSorting).GetTypeInfo().Assembly);
        xamlServiceProvider8.Add(type16, (object) xamlTypeResolver8);
        xamlServiceProvider8.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(79, 25)));
        object obj16 = ((IMarkupExtension) referenceExtension13).ProvideValue((IServiceProvider) xamlServiceProvider8);
        bindingExtension8.Source = obj16;
        VisualDiagnostics.RegisterSourceInfo(obj16, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 79, 25);
        bindingExtension8.Path = "ButtonColor";
        BindingBase bindingBase8 = ((IMarkupExtension<BindingBase>) bindingExtension8).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(VisualElement.BackgroundColorProperty, bindingBase8);
        referenceExtension6.Name = "storepivot";
        ReferenceExtension referenceExtension14 = referenceExtension6;
        XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
        Type type17 = typeof (IProvideValueTarget);
        object[] objArray10 = new object[0 + 6];
        objArray10[0] = (object) bindingExtension9;
        objArray10[1] = (object) button3;
        objArray10[2] = (object) stackLayout6;
        objArray10[3] = (object) stackLayout8;
        objArray10[4] = (object) stackLayout9;
        objArray10[5] = (object) storeSorting;
        SimpleValueTargetProvider valueTargetProvider9;
        object obj17 = (object) (valueTargetProvider9 = new SimpleValueTargetProvider(objArray10, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider9.Add(type17, (object) valueTargetProvider9);
        xamlServiceProvider9.Add(typeof (IReferenceProvider), obj17);
        Type type18 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver9 = new XmlNamespaceResolver();
        namespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver9.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver9.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver9.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        namespaceResolver9.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver9 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver9, typeof (StoreSorting).GetTypeInfo().Assembly);
        xamlServiceProvider9.Add(type18, (object) xamlTypeResolver9);
        xamlServiceProvider9.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(79, 98)));
        object obj18 = ((IMarkupExtension) referenceExtension14).ProvideValue((IServiceProvider) xamlServiceProvider9);
        bindingExtension9.Source = obj18;
        VisualDiagnostics.RegisterSourceInfo(obj18, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 79, 98);
        bindingExtension9.Path = "TextColor";
        BindingBase bindingBase9 = ((IMarkupExtension<BindingBase>) bindingExtension9).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(Button.TextColorProperty, bindingBase9);
        ((BindableObject) button3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) button3);
        VisualDiagnostics.RegisterSourceInfo((object) button3, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 78, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 77, 18);
        bindingExtension10.Path = ".";
        BindingBase bindingBase10 = ((IMarkupExtension<BindingBase>) bindingExtension10).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView2).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase10);
        ((BindableObject) listView2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) listView2).SetValue(VisualElement.HeightRequestProperty, (object) 5000.0);
        ((BindableObject) listView2).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        ((BindableObject) listView2).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        ((BindableObject) label2).SetValue(Label.TextProperty, (object) "Raf Detayları");
        ((BindableObject) label2).SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        referenceExtension7.Name = "storepivot";
        ReferenceExtension referenceExtension15 = referenceExtension7;
        XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
        Type type19 = typeof (IProvideValueTarget);
        object[] objArray11 = new object[0 + 7];
        objArray11[0] = (object) bindingExtension11;
        objArray11[1] = (object) label2;
        objArray11[2] = (object) stackLayout7;
        objArray11[3] = (object) listView2;
        objArray11[4] = (object) stackLayout8;
        objArray11[5] = (object) stackLayout9;
        objArray11[6] = (object) storeSorting;
        SimpleValueTargetProvider valueTargetProvider10;
        object obj19 = (object) (valueTargetProvider10 = new SimpleValueTargetProvider(objArray11, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider10.Add(type19, (object) valueTargetProvider10);
        xamlServiceProvider10.Add(typeof (IReferenceProvider), obj19);
        Type type20 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver10 = new XmlNamespaceResolver();
        namespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver10.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver10.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver10.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        namespaceResolver10.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver10 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver10, typeof (StoreSorting).GetTypeInfo().Assembly);
        xamlServiceProvider10.Add(type20, (object) xamlTypeResolver10);
        xamlServiceProvider10.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(88, 36)));
        object obj20 = ((IMarkupExtension) referenceExtension15).ProvideValue((IServiceProvider) xamlServiceProvider10);
        bindingExtension11.Source = obj20;
        VisualDiagnostics.RegisterSourceInfo(obj20, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 88, 36);
        bindingExtension11.Path = "ButtonColor";
        BindingBase bindingBase11 = ((IMarkupExtension<BindingBase>) bindingExtension11).ProvideValue((IServiceProvider) null);
        ((BindableObject) label2).SetBinding(Label.TextColorProperty, bindingBase11);
        referenceExtension8.Name = "storepivot";
        ReferenceExtension referenceExtension16 = referenceExtension8;
        XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
        Type type21 = typeof (IProvideValueTarget);
        object[] objArray12 = new object[0 + 7];
        objArray12[0] = (object) bindingExtension12;
        objArray12[1] = (object) label2;
        objArray12[2] = (object) stackLayout7;
        objArray12[3] = (object) listView2;
        objArray12[4] = (object) stackLayout8;
        objArray12[5] = (object) stackLayout9;
        objArray12[6] = (object) storeSorting;
        SimpleValueTargetProvider valueTargetProvider11;
        object obj21 = (object) (valueTargetProvider11 = new SimpleValueTargetProvider(objArray12, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider11.Add(type21, (object) valueTargetProvider11);
        xamlServiceProvider11.Add(typeof (IReferenceProvider), obj21);
        Type type22 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver11 = new XmlNamespaceResolver();
        namespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver11.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver11.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver11.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        namespaceResolver11.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver11 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver11, typeof (StoreSorting).GetTypeInfo().Assembly);
        xamlServiceProvider11.Add(type22, (object) xamlTypeResolver11);
        xamlServiceProvider11.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(89, 36)));
        object obj22 = ((IMarkupExtension) referenceExtension16).ProvideValue((IServiceProvider) xamlServiceProvider11);
        bindingExtension12.Source = obj22;
        VisualDiagnostics.RegisterSourceInfo(obj22, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 89, 36);
        bindingExtension12.Path = "TextColor";
        BindingBase bindingBase12 = ((IMarkupExtension<BindingBase>) bindingExtension12).ProvideValue((IServiceProvider) null);
        ((BindableObject) label2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase12);
        ((BindableObject) label2).SetValue(Label.FontProperty, new FontTypeConverter().ConvertFromInvariantString("Bold,20"));
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) label2);
        VisualDiagnostics.RegisterSourceInfo((object) label2, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 86, 30);
        ((BindableObject) listView2).SetValue(ListView.HeaderProperty, (object) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 85, 26);
        DataTemplate dataTemplate4 = dataTemplate2;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        StoreSorting.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_41 xamlCdataTemplate41 = new StoreSorting.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_41();
        object[] objArray13 = new object[0 + 5];
        objArray13[0] = (object) dataTemplate2;
        objArray13[1] = (object) listView2;
        objArray13[2] = (object) stackLayout8;
        objArray13[3] = (object) stackLayout9;
        objArray13[4] = (object) storeSorting;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate41.parentValues = objArray13;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate41.root = storeSorting;
        // ISSUE: reference to a compiler-generated method
        Func<object> func2 = new Func<object>(xamlCdataTemplate41.LoadDataTemplate);
        ((IDataTemplate) dataTemplate4).LoadTemplate = func2;
        ((BindableObject) listView2).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate2);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate2, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 94, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) listView2);
        VisualDiagnostics.RegisterSourceInfo((object) listView2, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 82, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout9).Children).Add((View) stackLayout8);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout8, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 45, 14);
        ((BindableObject) storeSorting).SetValue(ContentPage.ContentProperty, (object) stackLayout9);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout9, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 10);
        VisualDiagnostics.RegisterSourceInfo((object) storeSorting, new Uri("Views\\StoreSorting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<StoreSorting>(this, typeof (StoreSorting));
      this.storepivot = NameScopeExtensions.FindByName<ContentPage>((Element) this, "storepivot");
      this.stckContent = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckContent");
      this.stckShelfOrderList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfOrderList");
      this.stckEmptyMessage = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckEmptyMessage");
      this.lstShelfOrder = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfOrder");
      this.stckFormAndList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckFormAndList");
      this.txtPackageNumber = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtPackageNumber");
      this.btnClear = NameScopeExtensions.FindByName<Button>((Element) this, "btnClear");
      this.btnCreateNewPackage = NameScopeExtensions.FindByName<Button>((Element) this, "btnCreateNewPackage");
      this.txtCustomerName = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtCustomerName");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.txtQty = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtQty");
      this.pckBarcodeType = NameScopeExtensions.FindByName<Picker>((Element) this, "pckBarcodeType");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.btnApprove = NameScopeExtensions.FindByName<Button>((Element) this, "btnApprove");
      this.lstShelfDetail = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfDetail");
      this.lblListHeader = NameScopeExtensions.FindByName<Label>((Element) this, "lblListHeader");
    }
  }
}
