// Decompiled with JetBrains decompiler
// Type: Shelf.Views.ShelfEntry3
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
  [XamlFilePath("Views\\ShelfEntry3.xaml")]
  public class ShelfEntry3 : ContentPage
  {
    private List<pIOShelfPurchaseOrderDetailReturnModel> shelfOrderDetail;
    private string selectedShelfCode;
    private ztIOShelf shelf;
    private bool IsPlusOrderQty;
    private List<pIOUserShelfPurchaseOrdersReturnModel> shelfOrderList;
    public ztIOShelf mkShelf;
    private pIOUserShelfPurchaseOrdersReturnModel selectedPurchaseOrder;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage shelfentry;
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
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtShelfOrderNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtShelfBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnCreateShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnClearShelfBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckProcessType;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtQty;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckBarcodeType;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnPickOrder;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnShelfOrderSuccess;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfDetail;

    public Color ButtonColor => Color.FromRgb(52, 203, 201);

    public Color TextColor => Color.White;

    public ShelfEntry3()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Raf Girişi";
      GlobalMob.AddBarcodeLongPress(this.txtBarcode);
      ToolbarItem toolbarItem = new ToolbarItem();
      ((MenuItem) toolbarItem).Text = "";
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(toolbarItem);
      ((VisualElement) this.txtQty).IsVisible = !GlobalMob.User.HideQty;
      ((VisualElement) this.btnCreateShelf).IsVisible = GlobalMob.User.IsMainShelf;
      this.GetMKShelf();
      ((ICollection<Effect>) ((Element) this.btnClearShelfBarcode).Effects).Add((Effect) new LongPressedEffect());
      LongPressedEffect.SetCommand((BindableObject) this.btnClearShelfBarcode, this.LongPressShelfBarcode);
      GlobalMob.AddShelfBarcodeLongPress((Xamarin.Forms.Entry) this.txtShelfBarcode);
      GlobalMob.FillBarcodeType(this.pckBarcodeType);
      GlobalMob.FillProcessType(this.pckProcessType);
    }

    private ICommand LongPressShelfBarcode => (ICommand) new Command((Action) (async () =>
    {
      ShelfEntry3 page = this;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfPurchaseOrderPackage?shelfPurchaseOrderID={0}", (object) page.selectedPurchaseOrder.PurchaseOrderID), (ContentPage) page);
      if (!returnModel.Success)
        return;
      List<pIOGetShelfPurchaseOrderPackageReturnModel> packageReturnModelList = GlobalMob.JsonDeserialize<List<pIOGetShelfPurchaseOrderPackageReturnModel>>(returnModel.Result);
      ListView shelfListview = GlobalMob.GetShelfListview("Code,HallCode,FloorCode");
      ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) packageReturnModelList;
      shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(page.SelectShelfBarcode_SelectedItem);
      SelectItem selectItem = new SelectItem(shelfListview, "Raf Seçiniz");
      await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
    }));

    private void SelectShelfBarcode_SelectedItem(object sender, SelectedItemChangedEventArgs e)
    {
      ((InputView) this.txtShelfBarcode).Text = ((pIOGetShelfPurchaseOrderPackageReturnModel) e.SelectedItem).Code;
      ((IEntryController) this.txtShelfBarcode).SendCompleted();
      ((NavigableElement) this).Navigation.PopAsync();
    }

    private void GetMKShelf()
    {
      if (!GlobalMob.User.IsMainShelf)
        return;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfFromID?shelfID={0}", (object) GlobalMob.User.MKShelfID), (ContentPage) this);
      if (!returnModel.Success || string.IsNullOrEmpty(returnModel.Result))
        return;
      this.mkShelf = JsonConvert.DeserializeObject<ztIOShelf>(returnModel.Result);
    }

    private void SetInfo()
    {
      double num = this.shelfOrderDetail.Sum<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, double>) (x => x.OrderQty));
      ((MenuItem) ((Page) this).ToolbarItems[0]).Text = this.shelfOrderDetail.Sum<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, double>) (x => x.AllocatingQty)).ToString() + "/" + num.ToString();
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetUserShelfPurchaseOrders?userID={0}&shelfPurchaseOrderType={1}&searchText={2}", (object) GlobalMob.User.UserID, (object) -1, (object) ""), (ContentPage) this);
      if (returnModel.Success)
      {
        this.shelfOrderList = GlobalMob.JsonDeserialize<List<pIOUserShelfPurchaseOrdersReturnModel>>(returnModel.Result);
        this.LoadPurchaseOrder(this.shelfOrderList);
      }
      this.lstShelfOrder.ItemSelected += (EventHandler<SelectedItemChangedEventArgs>) ((sender, e) =>
      {
        object selectedItem = ((ListView) sender).SelectedItem;
        if (selectedItem == null)
          return;
        ((Page) this).Title = "Raf Giriş(İrsaliyeli)";
        pIOUserShelfPurchaseOrdersReturnModel ordersReturnModel = (pIOUserShelfPurchaseOrdersReturnModel) selectedItem;
        this.selectedPurchaseOrder = ordersReturnModel;
        ((InputView) this.txtShelfOrderNumber).Text = "";
        ((InputView) this.txtShelfOrderNumber).Text = ordersReturnModel.PurchaseOrderNumber.Replace("S", "");
        ((VisualElement) this.stckShelfOrderList).IsVisible = false;
        ((VisualElement) this.stckForm).IsVisible = true;
        this.GetShelfDetail();
      });
      this.lstShelfDetail.ItemSelected += (EventHandler<SelectedItemChangedEventArgs>) ((sender, e) =>
      {
        object selectedItem = ((ListView) sender).SelectedItem;
        if (selectedItem == null)
          return;
        ((Page) this).DisplayAlert("Bilgi", ((pIOShelfPurchaseOrderDetailReturnModel) selectedItem).ItemDescription, "", "Tamam");
        this.lstShelfDetail.SelectedItem = (object) null;
      });
    }

    private void LoadPurchaseOrder(List<pIOUserShelfPurchaseOrdersReturnModel> list)
    {
      ((BindableObject) this.lstShelfOrder).BindingContext = (object) null;
      ((BindableObject) this.lstShelfOrder).BindingContext = (object) list;
    }

    private void GetShelfDetail()
    {
      this.shelfOrderDetail = new List<pIOShelfPurchaseOrderDetailReturnModel>();
      Device.BeginInvokeOnMainThread((Action) (() =>
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfPurchaseOrderDetailAll?purchaseOrderNumber=S{0}", (object) ((InputView) this.txtShelfOrderNumber).Text), (ContentPage) this);
        if (!returnModel.Success)
          return;
        this.shelfOrderDetail = GlobalMob.JsonDeserialize<List<pIOShelfPurchaseOrderDetailReturnModel>>(returnModel.Result);
        this.SetEmptyForNullBarcode();
        ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
        if (this.shelfOrderDetail.Count > 0)
        {
          this.IsPlusOrderQty = Convert.ToBoolean((object) this.shelfOrderDetail[0].PlusOrderQty);
          ((VisualElement) this.lstShelfDetail).IsVisible = true;
          ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) this.shelfOrderDetail;
          this.SetInfo();
          ((VisualElement) this.stckBarcode).IsVisible = true;
          ((VisualElement) this.stckShelf).IsVisible = true;
          ((VisualElement) this.btnShelfOrderSuccess).IsVisible = true;
          Device.BeginInvokeOnMainThread((Action) (async () =>
          {
            await Task.Delay(150);
            ((InputView) this.txtShelfBarcode).Text = "";
            ((VisualElement) this.txtShelfBarcode).Focus();
          }));
        }
        else
          ((VisualElement) this.btnShelfOrderSuccess).IsVisible = true;
      }));
    }

    private void SetEmptyForNullBarcode()
    {
      this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.Barcode == null)).Select<pIOShelfPurchaseOrderDetailReturnModel, pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, pIOShelfPurchaseOrderDetailReturnModel>) (c =>
      {
        c.Barcode = "";
        return c;
      })).ToList<pIOShelfPurchaseOrderDetailReturnModel>();
      this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.UsedBarcode == null)).Select<pIOShelfPurchaseOrderDetailReturnModel, pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, pIOShelfPurchaseOrderDetailReturnModel>) (c =>
      {
        c.UsedBarcode = "";
        return c;
      })).ToList<pIOShelfPurchaseOrderDetailReturnModel>();
    }

    private void FillListView()
    {
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
      if (this.shelfOrderDetail.Count > 0)
      {
        ((VisualElement) this.lstShelfDetail).IsVisible = true;
        ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) this.shelfOrderDetail;
      }
      else
        ((InputView) this.txtShelfBarcode).Text = "";
    }

    private ReturnModel GetBarcode(pIOShelfPurchaseOrderDetailReturnModel sItem = null)
    {
      ReturnModel barcode1 = new ReturnModel();
      string barcode = ((InputView) this.txtBarcode).Text;
      if (this.shelf == null || string.IsNullOrEmpty(((InputView) this.txtShelfBarcode).Text))
      {
        GlobalMob.PlayError();
        ((Page) this).DisplayAlert("Bilgi", "Öncelikle raf okutunuz", "", "Tamam");
        ((InputView) this.txtBarcode).Text = "";
        ((VisualElement) this.txtBarcode).Focus();
        return barcode1;
      }
      if (!string.IsNullOrEmpty(barcode))
      {
        if (barcode.Length < GlobalMob.User.MinimumBarcodeLength)
        {
          GlobalMob.PlayError();
          ((InputView) this.txtBarcode).Text = "";
          ((VisualElement) this.txtBarcode).Focus();
          return barcode1;
        }
        PickerItem selectedItem1 = (PickerItem) this.pckProcessType.SelectedItem;
        if (((VisualElement) this.pckProcessType).IsVisible && selectedItem1.Code == 2)
        {
          this.ReverseOperation();
          return barcode1;
        }
        int qty = Convert.ToInt32(((InputView) this.txtQty).Text);
        bool flag1 = false;
        bool flag2 = false;
        PickerItem selectedItem2 = (PickerItem) this.pckBarcodeType.SelectedItem;
        if (selectedItem2 != null && selectedItem2.Code == 2 && ((VisualElement) this.pckBarcodeType).IsVisible)
          flag1 = true;
        pIOShelfPurchaseOrderDetailReturnModel detailReturnModel1;
        if (flag1)
        {
          detailReturnModel1 = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode && x.AllocatingQty != x.OrderQty)).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>();
          if (this.IsPlusOrderQty && detailReturnModel1 == null)
            detailReturnModel1 = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode)).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>();
          if (detailReturnModel1 == null)
            detailReturnModel1 = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.AllocatingQty != x.OrderQty)).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>();
        }
        else
        {
          flag2 = this.IsUniqueControl(barcode);
          if (flag2)
          {
            detailReturnModel1 = sItem != null ? sItem : this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.UsedBarcode.Contains(barcode) && x.OrderQty != x.AllocatingQty)).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>();
          }
          else
          {
            detailReturnModel1 = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.Barcode.Contains(barcode) && x.OrderQty >= x.AllocatingQty + (double) qty)).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>();
            if (this.IsPlusOrderQty && detailReturnModel1 == null)
            {
              detailReturnModel1 = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.Barcode.Contains(barcode))).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>();
              if (detailReturnModel1 != null)
              {
                double num1 = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.Barcode.Contains(barcode))).Sum<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, double>) (x => x.AllocatingQty));
                double num2 = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.Barcode.Contains(barcode))).Sum<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, double>) (x => x.OrderQty));
                int plusPercent = detailReturnModel1.PlusPercent;
                double num3 = 100.0;
                if (plusPercent > 0 && num1 + (double) qty > num2 * ((num3 + (double) plusPercent) / num3))
                {
                  GlobalMob.PlayError();
                  ((Page) this).DisplayAlert("Bilgi", "Yüzdelik miktardan fazla ürün okuttunuz" + ("\nÜrüne ait Toplam Sipariş Miktarı : " + num2.ToString() + "\nYeni Miktar : " + (detailReturnModel1.AllocatingQty + (double) qty).ToString() + "\nEn Fazla Okutulacak miktar : " + Convert.ToInt32(detailReturnModel1.OrderQty * ((num3 + (double) plusPercent) / num3)).ToString()), "", "Tamam");
                  ((InputView) this.txtBarcode).Text = "";
                  ((VisualElement) this.txtBarcode).Focus();
                  return barcode1;
                }
              }
            }
          }
        }
        if (detailReturnModel1 != null)
        {
          int purchaseOrderDetailId = detailReturnModel1.PurchaseOrderDetailID;
          if (flag1)
          {
            PickAndSort pickAndSort1 = new PickAndSort()
            {
              ShelfOrderDetailID = purchaseOrderDetailId,
              PickQty = qty,
              barcode = barcode,
              ShelfID = this.shelf.ShelfID,
              IsPlusOrderQty = this.IsPlusOrderQty,
              userName = GlobalMob.User.UserName,
              ShelfOrderID = detailReturnModel1.PurchaseOrderID
            };
            List<pIOShelfPurchaseOrderDetailReturnModel> list = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode && x.AllocatingQty != x.OrderQty)).ToList<pIOShelfPurchaseOrderDetailReturnModel>();
            if (list == null || list.Count <= 0)
              list = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.AllocatingQty != x.OrderQty)).ToList<pIOShelfPurchaseOrderDetailReturnModel>();
            pickAndSort1.Detail = new List<PickAndSortDetail>();
            foreach (pIOShelfPurchaseOrderDetailReturnModel detailReturnModel2 in list)
              pickAndSort1.Detail.Add(new PickAndSortDetail()
              {
                Barcode = detailReturnModel2.Barcode,
                ColorCode = detailReturnModel2.ColorCode,
                DispOrderNumber = "",
                ItemCode = detailReturnModel2.ItemCode,
                ItemDim1Code = detailReturnModel2.ItemDim1Code,
                ItemDim2Code = detailReturnModel2.ItemDim2Code,
                OrderQty = detailReturnModel2.OrderQty,
                PickingQty = detailReturnModel2.AllocatingQty,
                ShelfOrderDetailID = detailReturnModel2.PurchaseOrderDetailID,
                ShelfOrderID = detailReturnModel2.PurchaseOrderID
              });
            pickAndSort1.barcode = ((InputView) this.txtBarcode).Text;
            Dictionary<string, string> paramList = new Dictionary<string, string>();
            string str = JsonConvert.SerializeObject((object) pickAndSort1);
            paramList.Add("json", str);
            ReturnModel result = GlobalMob.PostJson("ShelfInsertLotWithDispatch", paramList, (ContentPage) this).Result;
            if (result.Success)
            {
              List<PickAndSort> pickAndSortList = JsonConvert.DeserializeObject<List<PickAndSort>>(result.Result);
              if (pickAndSortList == null)
              {
                GlobalMob.PlayError();
                ((Page) this).DisplayAlert("Bilgi", "Lot için yeterli miktar yok", "", "Tamam");
                ((InputView) this.txtBarcode).Text = "";
                ((VisualElement) this.txtBarcode).Focus();
                return barcode1;
              }
              if (pickAndSortList.Count > 0)
              {
                PickAndSort pickAndSort2 = pickAndSortList[0];
                foreach (PickAndSort pickAndSort3 in pickAndSortList)
                {
                  PickAndSort pickItem = pickAndSort3;
                  pIOShelfPurchaseOrderDetailReturnModel detailReturnModel3 = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.PurchaseOrderDetailID == pickItem.ShelfOrderDetailID)).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>();
                  if (detailReturnModel3 != null)
                  {
                    detailReturnModel3.LastReadBarcode = true;
                    detailReturnModel3.AllocatingQty += (double) pickItem.PickQty;
                  }
                }
                barcode1.Success = true;
                this.RefreshScreen();
              }
              else
              {
                GlobalMob.PlayError();
                ((Page) this).DisplayAlert("Bilgi", "Hatalı Lot Barkodu : " + barcode, "", "Tamam");
                ((InputView) this.txtBarcode).Text = "";
                ((VisualElement) this.txtBarcode).Focus();
                return barcode1;
              }
            }
          }
          else if (qty > 0)
          {
            PickAndSort pickAndSort = new PickAndSort();
            pickAndSort.ShelfOrderDetailID = purchaseOrderDetailId;
            pickAndSort.PickQty = qty;
            pickAndSort.barcode = barcode;
            pickAndSort.IsPlusOrderQty = this.IsPlusOrderQty;
            pickAndSort.userName = GlobalMob.User.UserName;
            pickAndSort.ShelfOrderID = detailReturnModel1.PurchaseOrderID;
            pickAndSort.ShelfID = this.shelf.ShelfID;
            int? mainShelfId = this.shelf.MainShelfID;
            int mkShelfId = GlobalMob.User.MKShelfID;
            pickAndSort.MKShelfID = !(mainShelfId.GetValueOrDefault() == mkShelfId & mainShelfId.HasValue) ? GlobalMob.User.MKShelfID : 0;
            Dictionary<string, string> paramList = new Dictionary<string, string>();
            string str = JsonConvert.SerializeObject((object) pickAndSort);
            paramList.Add("json", str);
            ReturnModel result = GlobalMob.PostJson(flag2 ? "UpdatePurchaseOrderDetailUniquePost" : "UpdatePurchaseOrderDetailPost", paramList, (ContentPage) this).Result;
            if (result.Success)
            {
              int int32 = Convert.ToInt32(result.Result);
              if (int32 > 0)
              {
                this.shelf.MainShelfID = new int?(GlobalMob.User.MKShelfID);
                detailReturnModel1.AllocatingQty += (double) int32;
                this.shelfOrderDetail.Select<pIOShelfPurchaseOrderDetailReturnModel, pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, pIOShelfPurchaseOrderDetailReturnModel>) (c =>
                {
                  c.LastReadBarcode = false;
                  return c;
                })).ToList<pIOShelfPurchaseOrderDetailReturnModel>();
                detailReturnModel1.LastReadBarcode = true;
                barcode1.Success = true;
                this.RefreshScreen();
              }
              else if (int32 == -1)
              {
                GlobalMob.PlayError();
                ((Page) this).DisplayAlert("Bilgi", "Hata Oluştu", "", "Tamam");
                ((InputView) this.txtBarcode).Text = "";
                ((VisualElement) this.txtBarcode).Focus();
              }
              else
              {
                GlobalMob.PlayError();
                ((Page) this).DisplayAlert("Bilgi", "Miktar Yetersiz", "", "Tamam");
                ((InputView) this.txtBarcode).Text = "";
                ((VisualElement) this.txtBarcode).Focus();
              }
            }
            else
            {
              GlobalMob.PlayError();
              ((Page) this).DisplayAlert("Bilgi", "Servera ulaşılamadı", "", "Tamam");
              ((InputView) this.txtBarcode).Text = "";
              ((VisualElement) this.txtBarcode).Focus();
            }
          }
          else
          {
            GlobalMob.PlayError();
            ((Page) this).DisplayAlert("Bilgi", "Miktar sıfır olamaz", "", "Tamam");
            ((InputView) this.txtBarcode).Text = "";
            ((VisualElement) this.txtBarcode).Focus();
          }
        }
        else
        {
          int num = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.Barcode.Contains(barcode) && x.ShelfCode == this.selectedShelfCode && x.AllocatingQty == x.OrderQty)).Any<pIOShelfPurchaseOrderDetailReturnModel>() ? 1 : 0;
          GlobalMob.PlayError();
          string str = num != 0 ? "Sipariş miktarı tamamlandı" : "Ürün bulunamadı";
          if (num == 0 & flag2 && this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.UsedBarcode.Contains(barcode))).Count<pIOShelfPurchaseOrderDetailReturnModel>() > 0)
            str = "Bu ürün daha önce okutuldu";
          ((Page) this).DisplayAlert("Bilgi", str, "", "Tamam");
          ((InputView) this.txtBarcode).Text = "";
          ((VisualElement) this.txtBarcode).Focus();
        }
      }
      return barcode1;
    }

    private bool IsUniqueControl(string barcode)
    {
      pIOShelfPurchaseOrderDetailReturnModel detailReturnModel = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.UsedBarcode.Contains(barcode) || x.Barcode.Contains(barcode))).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>();
      return detailReturnModel != null && detailReturnModel.UseSerialNumber;
    }

    private void ReverseOperation()
    {
      string barcode = ((InputView) this.txtBarcode).Text;
      int qty = Convert.ToInt32(((InputView) this.txtQty).Text);
      bool flag = false;
      PickerItem selectedItem = (PickerItem) this.pckBarcodeType.SelectedItem;
      if (selectedItem != null && selectedItem.Code == 2)
      {
        int num = ((VisualElement) this.pckBarcodeType).IsVisible ? 1 : 0;
      }
      if (selectedItem != null && selectedItem.Code == 3 && ((VisualElement) this.pckBarcodeType).IsVisible)
        flag = true;
      pIOShelfPurchaseOrderDetailReturnModel detailReturnModel = !flag ? this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.Barcode.Contains(barcode) && x.AllocatingQty - (double) qty >= 0.0)).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>() : this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.UsedBarcode.Contains(barcode) && x.AllocatingQty - (double) qty >= 0.0)).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>();
      if (detailReturnModel == null)
      {
        GlobalMob.PlayError();
        ((Page) this).DisplayAlert("Bilgi", "İade alınacak ürün bulunamadı", "", "Tamam");
        ((InputView) this.txtBarcode).Text = "";
        ((VisualElement) this.txtBarcode).Focus();
      }
      else
      {
        PickAndSort pickAndSort = new PickAndSort();
        pickAndSort.ShelfOrderDetailID = detailReturnModel.PurchaseOrderDetailID;
        pickAndSort.PickQty = qty;
        pickAndSort.barcode = barcode;
        pickAndSort.ShelfID = this.shelf.ShelfID;
        pickAndSort.IsPlusOrderQty = this.IsPlusOrderQty;
        pickAndSort.userName = GlobalMob.User.UserName;
        pickAndSort.ShelfOrderID = detailReturnModel.PurchaseOrderID;
        Dictionary<string, string> paramList = new Dictionary<string, string>();
        string str = JsonConvert.SerializeObject((object) pickAndSort);
        paramList.Add("json", str);
        ReturnModel result = GlobalMob.PostJson(flag ? "ReverseShelfPurchaseDetailUnique" : "ReversePurchaseOrderDetailPost", paramList, (ContentPage) this).Result;
        if (!result.Success)
          return;
        int int32 = Convert.ToInt32(result.Result);
        detailReturnModel.AllocatingQty -= (double) int32;
        this.shelfOrderDetail.Select<pIOShelfPurchaseOrderDetailReturnModel, pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, pIOShelfPurchaseOrderDetailReturnModel>) (c =>
        {
          c.LastReadBarcode = false;
          return c;
        })).ToList<pIOShelfPurchaseOrderDetailReturnModel>();
        detailReturnModel.LastReadBarcode = true;
        this.RefreshScreen();
      }
    }

    private void RefreshScreen()
    {
      GlobalMob.PlaySave();
      this.shelfOrderDetail = this.shelfOrderDetail.OrderByDescending<pIOShelfPurchaseOrderDetailReturnModel, bool>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.LastReadBarcode)).ToList<pIOShelfPurchaseOrderDetailReturnModel>();
      this.SetInfo();
      this.FillListView();
      ((InputView) this.txtQty).Text = "1";
      Device.BeginInvokeOnMainThread((Action) (async () =>
      {
        await Task.Delay(250);
        ((InputView) this.txtBarcode).Text = "";
        ((VisualElement) this.txtBarcode)?.Focus();
      }));
    }

    private async void txtShelfBarcode_Completed(object sender, EventArgs e)
    {
      ShelfEntry3 page = this;
      if (string.IsNullOrEmpty(((InputView) page.txtShelfBarcode).Text))
        return;
      page.shelf = new ztIOShelf();
      ReturnModel shelf = GlobalMob.GetShelf(((InputView) page.txtShelfBarcode).Text, (ContentPage) page);
      if (shelf.Success && !string.IsNullOrEmpty(shelf.Result))
      {
        page.shelf = JsonConvert.DeserializeObject<ztIOShelf>(shelf.Result);
        ((InputView) page.txtShelfBarcode).Text = page.shelf.Code;
        page.selectedShelfCode = page.shelf.Code;
        page.BarcodeFocus(page.txtBarcode);
      }
      else
      {
        ((InputView) page.txtShelfBarcode).Text = "";
        int num = await ((Page) page).DisplayAlert("Hata", "Raf bulunamadı", "", "Tamam") ? 1 : 0;
        page.BarcodeFocus((Xamarin.Forms.Entry) page.txtShelfBarcode);
      }
    }

    private void txtBarcode_Completed(object sender, EventArgs e) => this.GetBarcode();

    private async void btnShelfOrderSuccess_Clicked(object sender, EventArgs e)
    {
      ShelfEntry3 page1 = this;
      if (!string.IsNullOrEmpty(((InputView) page1.txtShelfOrderNumber).Text))
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("ShelfOrderPurchaseCompleted?shelfPurchaseOrderNumber={0}&isCompleted=false", (object) ((InputView) page1.txtShelfOrderNumber).Text), (ContentPage) page1);
        if (!returnModel.Success)
          return;
        if (!string.IsNullOrEmpty(JsonConvert.DeserializeObject<string>(returnModel.Result)))
        {
          if (await ((Page) page1).DisplayAlert("Devam?", "Ürünler tamamlanmadı.Devam etmek istiyor musunuz?", "Evet", "Hayır"))
          {
            if (!GlobalMob.PostJson(string.Format("ShelfOrderPurchaseCompleted?shelfPurchaseOrderNumber={0}&isCompleted=true", (object) ((InputView) page1.txtShelfOrderNumber).Text), (ContentPage) page1).Success)
              return;
            int num = await ((Page) page1).DisplayAlert("Bilgi", "Raf Emri Tamamlandı", "", "Tamam") ? 1 : 0;
            Page page2 = await ((NavigableElement) page1).Navigation.PopAsync();
          }
          else
            page1.GetShelfDetail();
        }
        else
        {
          if (!GlobalMob.PostJson(string.Format("ShelfOrderPurchaseCompleted?shelfPurchaseOrderNumber={0}&isCompleted=true", (object) ((InputView) page1.txtShelfOrderNumber).Text), (ContentPage) page1).Success)
            return;
          int num = await ((Page) page1).DisplayAlert("Bilgi", "Raf Emri Tamamlandı", "", "Tamam") ? 1 : 0;
          Page page3 = await ((NavigableElement) page1).Navigation.PopAsync();
        }
      }
      else
      {
        int num1 = await ((Page) page1).DisplayAlert("Bilgi", "Lütfen raf emri seçiniz", "", "Tamam") ? 1 : 0;
      }
    }

    private void txtShelfBarcode_Completed_1(object sender, EventArgs e)
    {
    }

    private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
    }

    private async void btnCreateShelf_Clicked(object sender, EventArgs e)
    {
      ShelfEntry3 page = this;
      string pCode;
      ztIOShelf newShelf;
      if (page.mkShelf == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "MK Rafı parametre tablosunda tanımlı değil", "", "Tamam") ? 1 : 0;
        pCode = (string) null;
        newShelf = (ztIOShelf) null;
      }
      else
      {
        pCode = await GlobalMob.AskShelfCode((Page) page);
        if (pCode == "-1")
        {
          pCode = (string) null;
          newShelf = (ztIOShelf) null;
        }
        else
        {
          newShelf = new ztIOShelf()
          {
            CreatedUserName = GlobalMob.User.UserName,
            CreatedDate = new DateTime?(DateTime.Now),
            MainShelfID = new int?(page.mkShelf.ShelfID),
            IsBlocked = new bool?(false),
            ShelfType = (byte) 0,
            UpdatedDate = new DateTime?(DateTime.Now),
            UpdatedUserName = GlobalMob.User.UserName,
            WarehouseCode = page.mkShelf.WarehouseCode,
            HallID = page.mkShelf.HallID,
            SortOrder = new int?(0),
            ZoneID = page.mkShelf.ZoneID,
            Description = page.mkShelf.Code,
            Code = pCode
          };
          await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
          Dictionary<string, string> paramList = new Dictionary<string, string>();
          string str = JsonConvert.SerializeObject((object) newShelf);
          paramList.Add("json", str);
          ReturnModel result = GlobalMob.PostJson("CreateShelf", paramList, (ContentPage) page).Result;
          if (result.Success)
          {
            page.shelf = JsonConvert.DeserializeObject<ztIOShelf>(result.Result);
            if (page.shelf != null)
            {
              ((InputView) page.txtShelfBarcode).Text = page.shelf.Code;
              page.selectedShelfCode = page.shelf.Code;
            }
            else
            {
              ((InputView) page.txtShelfBarcode).Text = "";
              int num = await ((Page) page).DisplayAlert("Hata", "Bu kod zaten tanımlı:" + pCode, "", "Tamam") ? 1 : 0;
            }
          }
          GlobalMob.CloseLoading();
          if (page.shelf != null)
          {
            page.BarcodeFocus(page.txtBarcode);
            pCode = (string) null;
            newShelf = (ztIOShelf) null;
          }
          else
          {
            page.BarcodeFocus((Xamarin.Forms.Entry) page.txtShelfBarcode);
            pCode = (string) null;
            newShelf = (ztIOShelf) null;
          }
        }
      }
    }

    private void BarcodeFocus(Xamarin.Forms.Entry txt) => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(200);
      ((VisualElement) txt)?.Focus();
    }));

    private void btnClearShelfBarcode_Clicked(object sender, EventArgs e)
    {
      this.shelf = (ztIOShelf) null;
      ((InputView) this.txtShelfBarcode).Text = "";
      this.selectedShelfCode = "";
      Device.BeginInvokeOnMainThread((Action) (async () =>
      {
        await Task.Delay(200);
        ((VisualElement) this.txtShelfBarcode)?.Focus();
      }));
    }

    private void txtSearch_Completed(object sender, EventArgs e)
    {
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetUserShelfPurchaseOrders?userID={0}&shelfPurchaseOrderType={1}&searchText={2}", (object) GlobalMob.User.UserID, (object) -1, (object) ((InputView) this.txtSearch).Text), (ContentPage) this);
      if (!returnModel.Success)
        return;
      this.shelfOrderList = GlobalMob.JsonDeserialize<List<pIOUserShelfPurchaseOrdersReturnModel>>(returnModel.Result);
      this.LoadPurchaseOrder(this.shelfOrderList);
    }

    private void btnClear_Clicked(object sender, EventArgs e)
    {
      ((InputView) this.txtSearch).Text = "";
      ((IEntryController) this.txtSearch).SendCompleted();
    }

    private void imgSearch_Clicked(object sender, EventArgs e) => this.txtSearch_Completed((object) null, (EventArgs) null);

    private void txtBarcode_Focused(object sender, FocusEventArgs e)
    {
    }

    private void cmReverse_Clicked(object sender, EventArgs e) => ((VisualElement) this.pckProcessType).IsVisible = !((VisualElement) this.pckProcessType).IsVisible;

    private async void cmReadUniqeBarcode_Clicked(object sender, EventArgs e)
    {
      ShelfEntry3 shelfEntry3 = this;
      bool flag = false;
      PickerItem selectedItem = (PickerItem) shelfEntry3.pckBarcodeType.SelectedItem;
      if (selectedItem != null && selectedItem.Code == 3 && ((VisualElement) shelfEntry3.pckBarcodeType).IsVisible)
        flag = true;
      if (!flag)
        ;
      else
      {
        pIOShelfPurchaseOrderDetailReturnModel selectItem = (pIOShelfPurchaseOrderDetailReturnModel) (sender as MenuItem).CommandParameter;
        double sumQty = shelfEntry3.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.ItemCode == selectItem.ItemCode && x.ColorCode == selectItem.ColorCode && x.ItemDim1Code == selectItem.ItemDim1Code && x.OrderQty > x.AllocatingQty)).Sum<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, double>) (x => x.OrderQty));
        while (sumQty > 0.0)
        {
          string uniqeBarcode = await GlobalMob.InputBarcode(((NavigableElement) shelfEntry3).Navigation, "Unique Barkod Okutunuz", "Unique Barkod", Keyboard.Numeric, true);
          if (!string.IsNullOrEmpty(uniqeBarcode))
          {
            if (shelfEntry3.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.UsedBarcode == uniqeBarcode)).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>() != null)
            {
              GlobalMob.PlayError();
              int num = await ((Page) shelfEntry3).DisplayAlert("Hata", "Bu uniqe barkod daha önce okutuldu." + uniqeBarcode, "", "Tamam") ? 1 : 0;
            }
            else
            {
              pIOShelfPurchaseOrderDetailReturnModel sItem = shelfEntry3.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.ItemCode == selectItem.ItemCode && x.ColorCode == selectItem.ColorCode && x.ItemDim1Code == selectItem.ItemDim1Code && x.OrderQty > x.AllocatingQty)).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>();
              ((InputView) shelfEntry3.txtBarcode).Text = uniqeBarcode;
              ReturnModel barcode = shelfEntry3.GetBarcode(sItem);
              ((InputView) shelfEntry3.txtBarcode).Text = "";
              if (barcode.Success)
              {
                sItem.UsedBarcode = uniqeBarcode;
                --sumQty;
              }
              else
                break;
            }
          }
          else
            break;
        }
        shelfEntry3.BarcodeFocus(shelfEntry3.txtBarcode);
      }
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (ShelfEntry3).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/ShelfEntry3.xaml",
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
        ImageButton imageButton = new ImageButton();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        Button button1 = new Button();
        StackLayout stackLayout1 = new StackLayout();
        Label label1 = new Label();
        StackLayout stackLayout2 = new StackLayout();
        BindingExtension bindingExtension3 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView1 = new ListView();
        StackLayout stackLayout3 = new StackLayout();
        Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
        StackLayout stackLayout4 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry = new SoftkeyboardDisabledEntry();
        ReferenceExtension referenceExtension3 = new ReferenceExtension();
        BindingExtension bindingExtension4 = new BindingExtension();
        ReferenceExtension referenceExtension4 = new ReferenceExtension();
        BindingExtension bindingExtension5 = new BindingExtension();
        Button button2 = new Button();
        ReferenceExtension referenceExtension5 = new ReferenceExtension();
        BindingExtension bindingExtension6 = new BindingExtension();
        ReferenceExtension referenceExtension6 = new ReferenceExtension();
        BindingExtension bindingExtension7 = new BindingExtension();
        Button button3 = new Button();
        StackLayout stackLayout5 = new StackLayout();
        BindingExtension bindingExtension8 = new BindingExtension();
        BindingExtension bindingExtension9 = new BindingExtension();
        Picker picker1 = new Picker();
        KeyboardEnableEffect keyboardEnableEffect = new KeyboardEnableEffect();
        Xamarin.Forms.Entry entry3 = new Xamarin.Forms.Entry();
        Xamarin.Forms.Entry entry4 = new Xamarin.Forms.Entry();
        BindingExtension bindingExtension10 = new BindingExtension();
        BindingExtension bindingExtension11 = new BindingExtension();
        Picker picker2 = new Picker();
        StackLayout stackLayout6 = new StackLayout();
        ReferenceExtension referenceExtension7 = new ReferenceExtension();
        BindingExtension bindingExtension12 = new BindingExtension();
        ReferenceExtension referenceExtension8 = new ReferenceExtension();
        BindingExtension bindingExtension13 = new BindingExtension();
        Button button4 = new Button();
        ReferenceExtension referenceExtension9 = new ReferenceExtension();
        BindingExtension bindingExtension14 = new BindingExtension();
        ReferenceExtension referenceExtension10 = new ReferenceExtension();
        BindingExtension bindingExtension15 = new BindingExtension();
        Button button5 = new Button();
        BindingExtension bindingExtension16 = new BindingExtension();
        DataTemplate dataTemplate2 = new DataTemplate();
        ListView listView2 = new ListView();
        StackLayout stackLayout7 = new StackLayout();
        StackLayout stackLayout8 = new StackLayout();
        ShelfEntry3 shelfEntry3;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (shelfEntry3 = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) shelfEntry3, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("shelfentry", (object) shelfEntry3);
        if (((Element) shelfEntry3).StyleId == null)
          ((Element) shelfEntry3).StyleId = "shelfentry";
        ((INameScope) nameScope).RegisterName("stckContent", (object) stackLayout8);
        if (((Element) stackLayout8).StyleId == null)
          ((Element) stackLayout8).StyleId = "stckContent";
        ((INameScope) nameScope).RegisterName("stckShelfOrderList", (object) stackLayout3);
        if (((Element) stackLayout3).StyleId == null)
          ((Element) stackLayout3).StyleId = "stckShelfOrderList";
        ((INameScope) nameScope).RegisterName("stckSearch", (object) stackLayout1);
        if (((Element) stackLayout1).StyleId == null)
          ((Element) stackLayout1).StyleId = "stckSearch";
        ((INameScope) nameScope).RegisterName("txtSearch", (object) entry1);
        if (((Element) entry1).StyleId == null)
          ((Element) entry1).StyleId = "txtSearch";
        ((INameScope) nameScope).RegisterName("imgSearch", (object) imageButton);
        if (((Element) imageButton).StyleId == null)
          ((Element) imageButton).StyleId = "imgSearch";
        ((INameScope) nameScope).RegisterName("btnClear", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnClear";
        ((INameScope) nameScope).RegisterName("stckEmptyMessage", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckEmptyMessage";
        ((INameScope) nameScope).RegisterName("lstShelfOrder", (object) listView1);
        if (((Element) listView1).StyleId == null)
          ((Element) listView1).StyleId = "lstShelfOrder";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout7);
        if (((Element) stackLayout7).StyleId == null)
          ((Element) stackLayout7).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtShelfOrderNumber", (object) entry2);
        if (((Element) entry2).StyleId == null)
          ((Element) entry2).StyleId = "txtShelfOrderNumber";
        ((INameScope) nameScope).RegisterName("stckShelf", (object) stackLayout5);
        if (((Element) stackLayout5).StyleId == null)
          ((Element) stackLayout5).StyleId = "stckShelf";
        ((INameScope) nameScope).RegisterName("txtShelfBarcode", (object) softkeyboardDisabledEntry);
        if (((Element) softkeyboardDisabledEntry).StyleId == null)
          ((Element) softkeyboardDisabledEntry).StyleId = "txtShelfBarcode";
        ((INameScope) nameScope).RegisterName("btnCreateShelf", (object) button2);
        if (((Element) button2).StyleId == null)
          ((Element) button2).StyleId = "btnCreateShelf";
        ((INameScope) nameScope).RegisterName("btnClearShelfBarcode", (object) button3);
        if (((Element) button3).StyleId == null)
          ((Element) button3).StyleId = "btnClearShelfBarcode";
        ((INameScope) nameScope).RegisterName("stckBarcode", (object) stackLayout6);
        if (((Element) stackLayout6).StyleId == null)
          ((Element) stackLayout6).StyleId = "stckBarcode";
        ((INameScope) nameScope).RegisterName("pckProcessType", (object) picker1);
        if (((Element) picker1).StyleId == null)
          ((Element) picker1).StyleId = "pckProcessType";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) entry3);
        if (((Element) entry3).StyleId == null)
          ((Element) entry3).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("txtQty", (object) entry4);
        if (((Element) entry4).StyleId == null)
          ((Element) entry4).StyleId = "txtQty";
        ((INameScope) nameScope).RegisterName("pckBarcodeType", (object) picker2);
        if (((Element) picker2).StyleId == null)
          ((Element) picker2).StyleId = "pckBarcodeType";
        ((INameScope) nameScope).RegisterName("btnPickOrder", (object) button4);
        if (((Element) button4).StyleId == null)
          ((Element) button4).StyleId = "btnPickOrder";
        ((INameScope) nameScope).RegisterName("btnShelfOrderSuccess", (object) button5);
        if (((Element) button5).StyleId == null)
          ((Element) button5).StyleId = "btnShelfOrderSuccess";
        ((INameScope) nameScope).RegisterName("lstShelfDetail", (object) listView2);
        if (((Element) listView2).StyleId == null)
          ((Element) listView2).StyleId = "lstShelfDetail";
        this.shelfentry = (ContentPage) shelfEntry3;
        this.stckContent = stackLayout8;
        this.stckShelfOrderList = stackLayout3;
        this.stckSearch = stackLayout1;
        this.txtSearch = entry1;
        this.imgSearch = imageButton;
        this.btnClear = button1;
        this.stckEmptyMessage = stackLayout2;
        this.lstShelfOrder = listView1;
        this.stckForm = stackLayout7;
        this.txtShelfOrderNumber = entry2;
        this.stckShelf = stackLayout5;
        this.txtShelfBarcode = softkeyboardDisabledEntry;
        this.btnCreateShelf = button2;
        this.btnClearShelfBarcode = button3;
        this.stckBarcode = stackLayout6;
        this.pckProcessType = picker1;
        this.txtBarcode = entry3;
        this.txtQty = entry4;
        this.pckBarcodeType = picker2;
        this.btnPickOrder = button4;
        this.btnShelfOrderSuccess = button5;
        this.lstShelfDetail = listView2;
        ((BindableObject) stackLayout8).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout8).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout8).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout3).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout3).SetValue(View.MarginProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Ara");
        entry1.Completed += new EventHandler(shelfEntry3.txtSearch_Completed);
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 22);
        ((BindableObject) imageButton).SetValue(ImageButton.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("search.png"));
        ((BindableObject) imageButton).SetValue(ImageButton.AspectProperty, (object) (Aspect) 0);
        ((BindableObject) imageButton).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) imageButton).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        ((BindableObject) imageButton).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        imageButton.Clicked += new EventHandler(shelfEntry3.imgSearch_Clicked);
        referenceExtension1.Name = "shelfentry";
        ReferenceExtension referenceExtension11 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 6];
        objArray1[0] = (object) bindingExtension1;
        objArray1[1] = (object) imageButton;
        objArray1[2] = (object) stackLayout1;
        objArray1[3] = (object) stackLayout3;
        objArray1[4] = (object) stackLayout8;
        objArray1[5] = (object) shelfEntry3;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver1.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (ShelfEntry3).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(15, 54)));
        object obj2 = ((IMarkupExtension) referenceExtension11).ProvideValue((IServiceProvider) xamlServiceProvider1);
        bindingExtension1.Source = obj2;
        VisualDiagnostics.RegisterSourceInfo(obj2, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 54);
        bindingExtension1.Path = "ButtonColor";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) imageButton).SetBinding(VisualElement.BackgroundColorProperty, bindingBase1);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) imageButton);
        VisualDiagnostics.RegisterSourceInfo((object) imageButton, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 22);
        button1.Clicked += new EventHandler(shelfEntry3.btnClear_Clicked);
        ((BindableObject) button1).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        referenceExtension2.Name = "shelfentry";
        ReferenceExtension referenceExtension12 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 6];
        objArray2[0] = (object) bindingExtension2;
        objArray2[1] = (object) button1;
        objArray2[2] = (object) stackLayout1;
        objArray2[3] = (object) stackLayout3;
        objArray2[4] = (object) stackLayout8;
        objArray2[5] = (object) shelfEntry3;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray2, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver2.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver2.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (ShelfEntry3).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(16, 101)));
        object obj4 = ((IMarkupExtension) referenceExtension12).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension2.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 101);
        bindingExtension2.Path = "ButtonColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase2);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "X");
        ((BindableObject) button1).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        Button button6 = button1;
        BindableProperty fontSizeProperty1 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter1 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 5];
        objArray3[0] = (object) button1;
        objArray3[1] = (object) stackLayout1;
        objArray3[2] = (object) stackLayout3;
        objArray3[3] = (object) stackLayout8;
        objArray3[4] = (object) shelfEntry3;
        SimpleValueTargetProvider valueTargetProvider3;
        object obj5 = (object) (valueTargetProvider3 = new SimpleValueTargetProvider(objArray3, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider3.Add(type5, (object) valueTargetProvider3);
        xamlServiceProvider3.Add(typeof (IReferenceProvider), obj5);
        Type type6 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver3 = new XmlNamespaceResolver();
        namespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver3.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver3.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (ShelfEntry3).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(17, 57)));
        object obj6 = ((IExtendedTypeConverter) fontSizeConverter1).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider3);
        ((BindableObject) button6).SetValue(fontSizeProperty1, obj6);
        ((BindableObject) button1).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(Button.TextColorProperty, (object) Color.White);
        ((BindableObject) button1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 18);
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
        objArray4[3] = (object) stackLayout8;
        objArray4[4] = (object) shelfEntry3;
        SimpleValueTargetProvider valueTargetProvider4;
        object obj7 = (object) (valueTargetProvider4 = new SimpleValueTargetProvider(objArray4, (object) Label.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider4.Add(type7, (object) valueTargetProvider4);
        xamlServiceProvider4.Add(typeof (IReferenceProvider), obj7);
        Type type8 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver4 = new XmlNamespaceResolver();
        namespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver4.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver4.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (ShelfEntry3).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(21, 128)));
        object obj8 = ((IExtendedTypeConverter) fontSizeConverter2).ConvertFromInvariantString("Medium", (IServiceProvider) xamlServiceProvider4);
        ((BindableObject) label2).SetValue(fontSizeProperty2, obj8);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) label1);
        VisualDiagnostics.RegisterSourceInfo((object) label1, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 21, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 18);
        ((BindableObject) listView1).SetValue(ListView.RowHeightProperty, (object) 80);
        bindingExtension3.Path = ".";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView1).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase3);
        ((BindableObject) listView1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        DataTemplate dataTemplate3 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ShelfEntry3.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_31 xamlCdataTemplate31 = new ShelfEntry3.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_31();
        object[] objArray5 = new object[0 + 5];
        objArray5[0] = (object) dataTemplate1;
        objArray5[1] = (object) listView1;
        objArray5[2] = (object) stackLayout3;
        objArray5[3] = (object) stackLayout8;
        objArray5[4] = (object) shelfEntry3;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate31.parentValues = objArray5;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate31.root = shelfEntry3;
        // ISSUE: reference to a compiler-generated method
        Func<object> func1 = new Func<object>(xamlCdataTemplate31.LoadDataTemplate);
        ((IDataTemplate) dataTemplate3).LoadTemplate = func1;
        ((BindableObject) listView1).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 25, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) listView1);
        VisualDiagnostics.RegisterSourceInfo((object) listView1, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 14);
        ((BindableObject) stackLayout7).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout7).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout7).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) stackLayout7).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf Emri Numarası Giriniz");
        ((BindableObject) entry2).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry2).SetValue(VisualElement.InputTransparentProperty, (object) true);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 53, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 52, 18);
        ((BindableObject) stackLayout5).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout5).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) softkeyboardDisabledEntry).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf No Okutunuz");
        ((BindableObject) softkeyboardDisabledEntry).SetValue(VisualElement.WidthRequestProperty, (object) 200.0);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        softkeyboardDisabledEntry.Completed += new EventHandler(shelfEntry3.txtShelfBarcode_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) softkeyboardDisabledEntry);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 58, 22);
        ((BindableObject) button2).SetValue(Button.TextProperty, (object) "+");
        ((BindableObject) button2).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button2).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        ((BindableObject) button2).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Button button7 = button2;
        BindableProperty fontSizeProperty3 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter3 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
        Type type9 = typeof (IProvideValueTarget);
        object[] objArray6 = new object[0 + 5];
        objArray6[0] = (object) button2;
        objArray6[1] = (object) stackLayout5;
        objArray6[2] = (object) stackLayout7;
        objArray6[3] = (object) stackLayout8;
        objArray6[4] = (object) shelfEntry3;
        SimpleValueTargetProvider valueTargetProvider5;
        object obj9 = (object) (valueTargetProvider5 = new SimpleValueTargetProvider(objArray6, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider5.Add(type9, (object) valueTargetProvider5);
        xamlServiceProvider5.Add(typeof (IReferenceProvider), obj9);
        Type type10 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver5 = new XmlNamespaceResolver();
        namespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver5.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver5.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver5 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver5, typeof (ShelfEntry3).GetTypeInfo().Assembly);
        xamlServiceProvider5.Add(type10, (object) xamlTypeResolver5);
        xamlServiceProvider5.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(60, 121)));
        object obj10 = ((IExtendedTypeConverter) fontSizeConverter3).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider5);
        ((BindableObject) button7).SetValue(fontSizeProperty3, obj10);
        button2.Clicked += new EventHandler(shelfEntry3.btnCreateShelf_Clicked);
        referenceExtension3.Name = "shelfentry";
        ReferenceExtension referenceExtension13 = referenceExtension3;
        XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
        Type type11 = typeof (IProvideValueTarget);
        object[] objArray7 = new object[0 + 6];
        objArray7[0] = (object) bindingExtension4;
        objArray7[1] = (object) button2;
        objArray7[2] = (object) stackLayout5;
        objArray7[3] = (object) stackLayout7;
        objArray7[4] = (object) stackLayout8;
        objArray7[5] = (object) shelfEntry3;
        SimpleValueTargetProvider valueTargetProvider6;
        object obj11 = (object) (valueTargetProvider6 = new SimpleValueTargetProvider(objArray7, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider6.Add(type11, (object) valueTargetProvider6);
        xamlServiceProvider6.Add(typeof (IReferenceProvider), obj11);
        Type type12 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver6 = new XmlNamespaceResolver();
        namespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver6.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver6.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver6 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver6, typeof (ShelfEntry3).GetTypeInfo().Assembly);
        xamlServiceProvider6.Add(type12, (object) xamlTypeResolver6);
        xamlServiceProvider6.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(61, 25)));
        object obj12 = ((IMarkupExtension) referenceExtension13).ProvideValue((IServiceProvider) xamlServiceProvider6);
        bindingExtension4.Source = obj12;
        VisualDiagnostics.RegisterSourceInfo(obj12, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 61, 25);
        bindingExtension4.Path = "ButtonColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase4);
        referenceExtension4.Name = "shelfentry";
        ReferenceExtension referenceExtension14 = referenceExtension4;
        XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
        Type type13 = typeof (IProvideValueTarget);
        object[] objArray8 = new object[0 + 6];
        objArray8[0] = (object) bindingExtension5;
        objArray8[1] = (object) button2;
        objArray8[2] = (object) stackLayout5;
        objArray8[3] = (object) stackLayout7;
        objArray8[4] = (object) stackLayout8;
        objArray8[5] = (object) shelfEntry3;
        SimpleValueTargetProvider valueTargetProvider7;
        object obj13 = (object) (valueTargetProvider7 = new SimpleValueTargetProvider(objArray8, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider7.Add(type13, (object) valueTargetProvider7);
        xamlServiceProvider7.Add(typeof (IReferenceProvider), obj13);
        Type type14 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver7 = new XmlNamespaceResolver();
        namespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver7.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver7.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver7 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver7, typeof (ShelfEntry3).GetTypeInfo().Assembly);
        xamlServiceProvider7.Add(type14, (object) xamlTypeResolver7);
        xamlServiceProvider7.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(61, 98)));
        object obj14 = ((IMarkupExtension) referenceExtension14).ProvideValue((IServiceProvider) xamlServiceProvider7);
        bindingExtension5.Source = obj14;
        VisualDiagnostics.RegisterSourceInfo(obj14, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 61, 98);
        bindingExtension5.Path = "TextColor";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(Button.TextColorProperty, bindingBase5);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) button2);
        VisualDiagnostics.RegisterSourceInfo((object) button2, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 60, 22);
        ((BindableObject) button3).SetValue(Button.TextProperty, (object) "x");
        ((BindableObject) button3).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button3).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        ((BindableObject) button3).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Button button8 = button3;
        BindableProperty fontSizeProperty4 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter4 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
        Type type15 = typeof (IProvideValueTarget);
        object[] objArray9 = new object[0 + 5];
        objArray9[0] = (object) button3;
        objArray9[1] = (object) stackLayout5;
        objArray9[2] = (object) stackLayout7;
        objArray9[3] = (object) stackLayout8;
        objArray9[4] = (object) shelfEntry3;
        SimpleValueTargetProvider valueTargetProvider8;
        object obj15 = (object) (valueTargetProvider8 = new SimpleValueTargetProvider(objArray9, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider8.Add(type15, (object) valueTargetProvider8);
        xamlServiceProvider8.Add(typeof (IReferenceProvider), obj15);
        Type type16 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver8 = new XmlNamespaceResolver();
        namespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver8.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver8.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver8 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver8, typeof (ShelfEntry3).GetTypeInfo().Assembly);
        xamlServiceProvider8.Add(type16, (object) xamlTypeResolver8);
        xamlServiceProvider8.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(62, (int) sbyte.MaxValue)));
        object obj16 = ((IExtendedTypeConverter) fontSizeConverter4).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider8);
        ((BindableObject) button8).SetValue(fontSizeProperty4, obj16);
        button3.Clicked += new EventHandler(shelfEntry3.btnClearShelfBarcode_Clicked);
        referenceExtension5.Name = "shelfentry";
        ReferenceExtension referenceExtension15 = referenceExtension5;
        XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
        Type type17 = typeof (IProvideValueTarget);
        object[] objArray10 = new object[0 + 6];
        objArray10[0] = (object) bindingExtension6;
        objArray10[1] = (object) button3;
        objArray10[2] = (object) stackLayout5;
        objArray10[3] = (object) stackLayout7;
        objArray10[4] = (object) stackLayout8;
        objArray10[5] = (object) shelfEntry3;
        SimpleValueTargetProvider valueTargetProvider9;
        object obj17 = (object) (valueTargetProvider9 = new SimpleValueTargetProvider(objArray10, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider9.Add(type17, (object) valueTargetProvider9);
        xamlServiceProvider9.Add(typeof (IReferenceProvider), obj17);
        Type type18 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver9 = new XmlNamespaceResolver();
        namespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver9.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver9.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver9 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver9, typeof (ShelfEntry3).GetTypeInfo().Assembly);
        xamlServiceProvider9.Add(type18, (object) xamlTypeResolver9);
        xamlServiceProvider9.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(63, 25)));
        object obj18 = ((IMarkupExtension) referenceExtension15).ProvideValue((IServiceProvider) xamlServiceProvider9);
        bindingExtension6.Source = obj18;
        VisualDiagnostics.RegisterSourceInfo(obj18, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 63, 25);
        bindingExtension6.Path = "ButtonColor";
        BindingBase bindingBase6 = ((IMarkupExtension<BindingBase>) bindingExtension6).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(VisualElement.BackgroundColorProperty, bindingBase6);
        referenceExtension6.Name = "shelfentry";
        ReferenceExtension referenceExtension16 = referenceExtension6;
        XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
        Type type19 = typeof (IProvideValueTarget);
        object[] objArray11 = new object[0 + 6];
        objArray11[0] = (object) bindingExtension7;
        objArray11[1] = (object) button3;
        objArray11[2] = (object) stackLayout5;
        objArray11[3] = (object) stackLayout7;
        objArray11[4] = (object) stackLayout8;
        objArray11[5] = (object) shelfEntry3;
        SimpleValueTargetProvider valueTargetProvider10;
        object obj19 = (object) (valueTargetProvider10 = new SimpleValueTargetProvider(objArray11, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider10.Add(type19, (object) valueTargetProvider10);
        xamlServiceProvider10.Add(typeof (IReferenceProvider), obj19);
        Type type20 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver10 = new XmlNamespaceResolver();
        namespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver10.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver10.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver10 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver10, typeof (ShelfEntry3).GetTypeInfo().Assembly);
        xamlServiceProvider10.Add(type20, (object) xamlTypeResolver10);
        xamlServiceProvider10.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(63, 98)));
        object obj20 = ((IMarkupExtension) referenceExtension16).ProvideValue((IServiceProvider) xamlServiceProvider10);
        bindingExtension7.Source = obj20;
        VisualDiagnostics.RegisterSourceInfo(obj20, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 63, 98);
        bindingExtension7.Path = "TextColor";
        BindingBase bindingBase7 = ((IMarkupExtension<BindingBase>) bindingExtension7).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(Button.TextColorProperty, bindingBase7);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) button3);
        VisualDiagnostics.RegisterSourceInfo((object) button3, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 62, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 57, 18);
        ((BindableObject) stackLayout6).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout6).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) picker1).SetValue(Picker.TitleProperty, (object) "Tip");
        bindingExtension8.Path = ".";
        BindingBase bindingBase8 = ((IMarkupExtension<BindingBase>) bindingExtension8).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker1).SetBinding(Picker.ItemsSourceProperty, bindingBase8);
        bindingExtension9.Path = "Caption";
        BindingBase bindingBase9 = ((IMarkupExtension<BindingBase>) bindingExtension9).ProvideValue((IServiceProvider) null);
        picker1.ItemDisplayBinding = bindingBase9;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase9, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 67, 33);
        ((BindableObject) picker1).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) picker1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) picker1);
        VisualDiagnostics.RegisterSourceInfo((object) picker1, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 66, 22);
        ((BindableObject) entry3).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod No Giriniz/Okutunuz");
        ((BindableObject) entry3).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("True"));
        entry3.Completed += new EventHandler(shelfEntry3.txtBarcode_Completed);
        ((BindableObject) entry3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((VisualElement) entry3).Focused += new EventHandler<FocusEventArgs>(shelfEntry3.txtBarcode_Focused);
        ((BindableObject) entry3).SetValue(KeyboardEffect.EnableKeyboardProperty, (object) false);
        ((ICollection<Effect>) ((Element) entry3).Effects).Add((Effect) keyboardEnableEffect);
        VisualDiagnostics.RegisterSourceInfo((object) keyboardEnableEffect, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 74, 30);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) entry3);
        VisualDiagnostics.RegisterSourceInfo((object) entry3, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 69, 22);
        ((BindableObject) entry4).SetValue(Xamarin.Forms.Entry.TextProperty, (object) "1");
        ((BindableObject) entry4).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Miktar");
        ((BindableObject) entry4).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry4).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((BindableObject) entry4).SetValue(Xamarin.Forms.Entry.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) entry4);
        VisualDiagnostics.RegisterSourceInfo((object) entry4, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 78, 22);
        ((BindableObject) picker2).SetValue(Picker.TitleProperty, (object) "Tip");
        bindingExtension10.Path = ".";
        BindingBase bindingBase10 = ((IMarkupExtension<BindingBase>) bindingExtension10).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker2).SetBinding(Picker.ItemsSourceProperty, bindingBase10);
        bindingExtension11.Path = "Caption";
        BindingBase bindingBase11 = ((IMarkupExtension<BindingBase>) bindingExtension11).ProvideValue((IServiceProvider) null);
        picker2.ItemDisplayBinding = bindingBase11;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase11, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 80, 33);
        ((BindableObject) picker2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) picker2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) picker2);
        VisualDiagnostics.RegisterSourceInfo((object) picker2, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 79, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 65, 18);
        ((BindableObject) button4).SetValue(Button.TextProperty, (object) "Ekle/Okut");
        referenceExtension7.Name = "shelfentry";
        ReferenceExtension referenceExtension17 = referenceExtension7;
        XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
        Type type21 = typeof (IProvideValueTarget);
        object[] objArray12 = new object[0 + 5];
        objArray12[0] = (object) bindingExtension12;
        objArray12[1] = (object) button4;
        objArray12[2] = (object) stackLayout7;
        objArray12[3] = (object) stackLayout8;
        objArray12[4] = (object) shelfEntry3;
        SimpleValueTargetProvider valueTargetProvider11;
        object obj21 = (object) (valueTargetProvider11 = new SimpleValueTargetProvider(objArray12, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider11.Add(type21, (object) valueTargetProvider11);
        xamlServiceProvider11.Add(typeof (IReferenceProvider), obj21);
        Type type22 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver11 = new XmlNamespaceResolver();
        namespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver11.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver11.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver11 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver11, typeof (ShelfEntry3).GetTypeInfo().Assembly);
        xamlServiceProvider11.Add(type22, (object) xamlTypeResolver11);
        xamlServiceProvider11.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(84, 21)));
        object obj22 = ((IMarkupExtension) referenceExtension17).ProvideValue((IServiceProvider) xamlServiceProvider11);
        bindingExtension12.Source = obj22;
        VisualDiagnostics.RegisterSourceInfo(obj22, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 84, 21);
        bindingExtension12.Path = "ButtonColor";
        BindingBase bindingBase12 = ((IMarkupExtension<BindingBase>) bindingExtension12).ProvideValue((IServiceProvider) null);
        ((BindableObject) button4).SetBinding(VisualElement.BackgroundColorProperty, bindingBase12);
        referenceExtension8.Name = "shelfentry";
        ReferenceExtension referenceExtension18 = referenceExtension8;
        XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
        Type type23 = typeof (IProvideValueTarget);
        object[] objArray13 = new object[0 + 5];
        objArray13[0] = (object) bindingExtension13;
        objArray13[1] = (object) button4;
        objArray13[2] = (object) stackLayout7;
        objArray13[3] = (object) stackLayout8;
        objArray13[4] = (object) shelfEntry3;
        SimpleValueTargetProvider valueTargetProvider12;
        object obj23 = (object) (valueTargetProvider12 = new SimpleValueTargetProvider(objArray13, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider12.Add(type23, (object) valueTargetProvider12);
        xamlServiceProvider12.Add(typeof (IReferenceProvider), obj23);
        Type type24 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver12 = new XmlNamespaceResolver();
        namespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver12.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver12.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver12 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver12, typeof (ShelfEntry3).GetTypeInfo().Assembly);
        xamlServiceProvider12.Add(type24, (object) xamlTypeResolver12);
        xamlServiceProvider12.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(84, 94)));
        object obj24 = ((IMarkupExtension) referenceExtension18).ProvideValue((IServiceProvider) xamlServiceProvider12);
        bindingExtension13.Source = obj24;
        VisualDiagnostics.RegisterSourceInfo(obj24, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 84, 94);
        bindingExtension13.Path = "TextColor";
        BindingBase bindingBase13 = ((IMarkupExtension<BindingBase>) bindingExtension13).ProvideValue((IServiceProvider) null);
        ((BindableObject) button4).SetBinding(Button.TextColorProperty, bindingBase13);
        ((BindableObject) button4).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) button4);
        VisualDiagnostics.RegisterSourceInfo((object) button4, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 83, 18);
        ((BindableObject) button5).SetValue(Button.TextProperty, (object) "Tamamla");
        button5.Clicked += new EventHandler(shelfEntry3.btnShelfOrderSuccess_Clicked);
        referenceExtension9.Name = "shelfentry";
        ReferenceExtension referenceExtension19 = referenceExtension9;
        XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
        Type type25 = typeof (IProvideValueTarget);
        object[] objArray14 = new object[0 + 5];
        objArray14[0] = (object) bindingExtension14;
        objArray14[1] = (object) button5;
        objArray14[2] = (object) stackLayout7;
        objArray14[3] = (object) stackLayout8;
        objArray14[4] = (object) shelfEntry3;
        SimpleValueTargetProvider valueTargetProvider13;
        object obj25 = (object) (valueTargetProvider13 = new SimpleValueTargetProvider(objArray14, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider13.Add(type25, (object) valueTargetProvider13);
        xamlServiceProvider13.Add(typeof (IReferenceProvider), obj25);
        Type type26 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver13 = new XmlNamespaceResolver();
        namespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver13.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver13.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver13 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver13, typeof (ShelfEntry3).GetTypeInfo().Assembly);
        xamlServiceProvider13.Add(type26, (object) xamlTypeResolver13);
        xamlServiceProvider13.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(87, 21)));
        object obj26 = ((IMarkupExtension) referenceExtension19).ProvideValue((IServiceProvider) xamlServiceProvider13);
        bindingExtension14.Source = obj26;
        VisualDiagnostics.RegisterSourceInfo(obj26, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 87, 21);
        bindingExtension14.Path = "ButtonColor";
        BindingBase bindingBase14 = ((IMarkupExtension<BindingBase>) bindingExtension14).ProvideValue((IServiceProvider) null);
        ((BindableObject) button5).SetBinding(VisualElement.BackgroundColorProperty, bindingBase14);
        referenceExtension10.Name = "shelfentry";
        ReferenceExtension referenceExtension20 = referenceExtension10;
        XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
        Type type27 = typeof (IProvideValueTarget);
        object[] objArray15 = new object[0 + 5];
        objArray15[0] = (object) bindingExtension15;
        objArray15[1] = (object) button5;
        objArray15[2] = (object) stackLayout7;
        objArray15[3] = (object) stackLayout8;
        objArray15[4] = (object) shelfEntry3;
        SimpleValueTargetProvider valueTargetProvider14;
        object obj27 = (object) (valueTargetProvider14 = new SimpleValueTargetProvider(objArray15, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider14.Add(type27, (object) valueTargetProvider14);
        xamlServiceProvider14.Add(typeof (IReferenceProvider), obj27);
        Type type28 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver14 = new XmlNamespaceResolver();
        namespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver14.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver14.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver14 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver14, typeof (ShelfEntry3).GetTypeInfo().Assembly);
        xamlServiceProvider14.Add(type28, (object) xamlTypeResolver14);
        xamlServiceProvider14.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(87, 94)));
        object obj28 = ((IMarkupExtension) referenceExtension20).ProvideValue((IServiceProvider) xamlServiceProvider14);
        bindingExtension15.Source = obj28;
        VisualDiagnostics.RegisterSourceInfo(obj28, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 87, 94);
        bindingExtension15.Path = "TextColor";
        BindingBase bindingBase15 = ((IMarkupExtension<BindingBase>) bindingExtension15).ProvideValue((IServiceProvider) null);
        ((BindableObject) button5).SetBinding(Button.TextColorProperty, bindingBase15);
        ((BindableObject) button5).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) button5);
        VisualDiagnostics.RegisterSourceInfo((object) button5, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 86, 18);
        bindingExtension16.Path = ".";
        BindingBase bindingBase16 = ((IMarkupExtension<BindingBase>) bindingExtension16).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView2).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase16);
        ((BindableObject) listView2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) listView2).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) listView2).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        ((BindableObject) listView2).SetValue(VisualElement.HeightRequestProperty, (object) 5000.0);
        ((BindableObject) listView2).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        DataTemplate dataTemplate4 = dataTemplate2;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ShelfEntry3.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_32 xamlCdataTemplate32 = new ShelfEntry3.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_32();
        object[] objArray16 = new object[0 + 5];
        objArray16[0] = (object) dataTemplate2;
        objArray16[1] = (object) listView2;
        objArray16[2] = (object) stackLayout7;
        objArray16[3] = (object) stackLayout8;
        objArray16[4] = (object) shelfEntry3;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate32.parentValues = objArray16;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate32.root = shelfEntry3;
        // ISSUE: reference to a compiler-generated method
        Func<object> func2 = new Func<object>(xamlCdataTemplate32.LoadDataTemplate);
        ((IDataTemplate) dataTemplate4).LoadTemplate = func2;
        ((BindableObject) listView2).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate2);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate2, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 101, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) listView2);
        VisualDiagnostics.RegisterSourceInfo((object) listView2, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 89, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 51, 14);
        ((BindableObject) shelfEntry3).SetValue(ContentPage.ContentProperty, (object) stackLayout8);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout8, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 9, 10);
        VisualDiagnostics.RegisterSourceInfo((object) shelfEntry3, new Uri("Views\\ShelfEntry3.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<ShelfEntry3>(this, typeof (ShelfEntry3));
      this.shelfentry = NameScopeExtensions.FindByName<ContentPage>((Element) this, "shelfentry");
      this.stckContent = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckContent");
      this.stckShelfOrderList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfOrderList");
      this.stckSearch = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckSearch");
      this.txtSearch = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtSearch");
      this.imgSearch = NameScopeExtensions.FindByName<ImageButton>((Element) this, "imgSearch");
      this.btnClear = NameScopeExtensions.FindByName<Button>((Element) this, "btnClear");
      this.stckEmptyMessage = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckEmptyMessage");
      this.lstShelfOrder = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfOrder");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtShelfOrderNumber = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtShelfOrderNumber");
      this.stckShelf = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelf");
      this.txtShelfBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtShelfBarcode");
      this.btnCreateShelf = NameScopeExtensions.FindByName<Button>((Element) this, "btnCreateShelf");
      this.btnClearShelfBarcode = NameScopeExtensions.FindByName<Button>((Element) this, "btnClearShelfBarcode");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.pckProcessType = NameScopeExtensions.FindByName<Picker>((Element) this, "pckProcessType");
      this.txtBarcode = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtBarcode");
      this.txtQty = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtQty");
      this.pckBarcodeType = NameScopeExtensions.FindByName<Picker>((Element) this, "pckBarcodeType");
      this.btnPickOrder = NameScopeExtensions.FindByName<Button>((Element) this, "btnPickOrder");
      this.btnShelfOrderSuccess = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelfOrderSuccess");
      this.lstShelfDetail = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfDetail");
    }
  }
}
