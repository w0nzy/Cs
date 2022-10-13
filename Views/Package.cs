// Decompiled with JetBrains decompiler
// Type: Shelf.Views.Package
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
  [XamlFilePath("Views\\Package.xaml")]
  public class Package : ContentPage
  {
    private List<pIOShelfOrderDetailFromPackageReturnModel> detailList;
    private List<pIOShelfOrderDetailFromPackageReturnModel> filterList;
    private pIOUserShelfOrderForPackageReturnModel selectedShelfOrder;
    private ztIOShelfPackageHeader selectPackageHeader;
    private List<pIOGetPackageDetailFromShelfOrderIDReturnModel> packageDetails;
    private List<ztIOShelfPackageType> packageTypeList;
    private ToolbarItem tInfo;
    private ListView lstDetails;
    private ztIOShelfPackageHeader newPackageHeader;
    private string selectPackageCustomerName = "";
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage package;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckContent;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelfOrderList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtSearch;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ImageButton imgSearch;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckEmptyMessage;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfOrder;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckPackage;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtPackageBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnCekiListesiPrint;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnCreateNewPackage;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtQty;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckBarcodeType;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfDetail;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Label lblListHeader;

    public Color ButtonColor => Color.FromRgb(142, 81, 152);

    public Color TextColor => Color.White;

    public Package()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Koli Kapama";
      ((ICollection<Effect>) ((Element) this.txtPackageBarcode).Effects).Add((Effect) new LongPressedEffect());
      LongPressedEffect.SetCommand((BindableObject) this.txtPackageBarcode, this.LongPressPackage);
      ToolbarItem toolbarItem = new ToolbarItem();
      ((MenuItem) toolbarItem).Text = "";
      this.tInfo = toolbarItem;
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(this.tInfo);
      GlobalMob.AddBarcodeLongPress(this.txtBarcode);
      ((VisualElement) this.btnCekiListesiPrint).IsVisible = GlobalMob.User.IsBluetoothPrinter;
      if (!GlobalMob.User.IsBarcodeType)
        return;
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
      pickerItemList.Add(new PickerItem()
      {
        Caption = "Uni",
        Code = 3,
        Description = "Uni"
      });
      this.pckBarcodeType.ItemsSource = (IList) pickerItemList;
      this.pckBarcodeType.SelectedItem = (object) pickerItemList[0];
      ((VisualElement) this.pckBarcodeType).IsVisible = true;
    }

    private ICommand LongPressPackage => (ICommand) new Command((Action) (async () =>
    {
      Package page = this;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetPackageDetailFromShelfOrderID?shelfOrderID={0}&userID={1}", (object) page.selectedShelfOrder.ShelfOrderID, (object) GlobalMob.User.UserID), (ContentPage) page);
      if (!returnModel.Success)
        return;
      page.packageDetails = GlobalMob.JsonDeserialize<List<pIOGetPackageDetailFromShelfOrderIDReturnModel>>(returnModel.Result);
      List<pIOShelfOrderDetailFromPackageReturnModel> list = page.packageDetails.GroupBy(c => new
      {
        Description = c.Description,
        PackageCode = c.PackageCode
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType9<string, string>, pIOGetPackageDetailFromShelfOrderIDReturnModel>, pIOShelfOrderDetailFromPackageReturnModel>(gcs => new pIOShelfOrderDetailFromPackageReturnModel()
      {
        PackageCode = gcs.Key.PackageCode,
        PackageDescription = gcs.Key.Description
      }).ToList<pIOShelfOrderDetailFromPackageReturnModel>();
      List<CustomMenuItemParameter> contextList = new List<CustomMenuItemParameter>();
      contextList.Add(new CustomMenuItemParameter()
      {
        Text = "Detaylar",
        ClickedEvent = new EventHandler(page.Mn_Clicked)
      });
      if (page.packageDetails.Count > 0 && !string.IsNullOrEmpty(page.packageDetails[0].FileName))
        contextList.Add(new CustomMenuItemParameter()
        {
          Text = "Çeki Listesi",
          ClickedEvent = new EventHandler(page.MnPackageItems_Clicked)
        });
      ListView shelfListview = GlobalMob.GetShelfListview("PackageDescription", contextList);
      ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) list;
      shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(page.Lst_ItemSelected);
      SelectItem selectItem = new SelectItem(shelfListview, "Koliler");
      await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
    }));

    private void MnPackageItems_Clicked(object sender, EventArgs e)
    {
      pIOShelfOrderDetailFromPackageReturnModel item = (pIOShelfOrderDetailFromPackageReturnModel) (sender as MenuItem).CommandParameter;
      pIOGetPackageDetailFromShelfOrderIDReturnModel orderIdReturnModel = this.packageDetails.Where<pIOGetPackageDetailFromShelfOrderIDReturnModel>((Func<pIOGetPackageDetailFromShelfOrderIDReturnModel, bool>) (x => x.PackageCode == item.PackageCode)).FirstOrDefault<pIOGetPackageDetailFromShelfOrderIDReturnModel>();
      int num = orderIdReturnModel.FileName.Contains(".repx") ? 2 : 1;
      List<BLReport> repList = new List<BLReport>();
      BLReport blReport = new BLReport()
      {
        ReportTypeID = 2,
        UserID = GlobalMob.User.UserID,
        FileType = num,
        PrinterBrandID = Convert.ToInt32((object) orderIdReturnModel.PrinterBrandID),
        NetworkPrinter = orderIdReturnModel.NetworkPrinter
      };
      blReport.ParamList = new List<BLReportParam>();
      blReport.ParamList.Add(new BLReportParam()
      {
        ParamName = "PackageCode",
        ParamValue = item.PackageCode,
        ParamType = 20
      });
      repList.Add(blReport);
      GlobalMob.BLPrint(repList, (object) item, (ContentPage) this);
    }

    private async void Mn_Clicked(object sender, EventArgs e)
    {
      Package package = this;
      pIOShelfOrderDetailFromPackageReturnModel selectItem = (pIOShelfOrderDetailFromPackageReturnModel) (sender as MenuItem).CommandParameter;
      List<pIOGetPackageDetailFromShelfOrderIDReturnModel> list = package.packageDetails.Where<pIOGetPackageDetailFromShelfOrderIDReturnModel>((Func<pIOGetPackageDetailFromShelfOrderIDReturnModel, bool>) (x => x.Description == selectItem.PackageDescription)).ToList<pIOGetPackageDetailFromShelfOrderIDReturnModel>().GroupBy(c => new
      {
        ItemCode = c.ItemCode,
        ColorCode = c.ColorCode,
        ItemDim1Code = c.ItemDim1Code
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType10<string, string, string>, pIOGetPackageDetailFromShelfOrderIDReturnModel>, pIOGetPackageDetailFromShelfOrderIDReturnModel>(gcs => new pIOGetPackageDetailFromShelfOrderIDReturnModel()
      {
        ItemCode = gcs.Key.ItemCode,
        ColorCode = gcs.Key.ColorCode,
        ItemDim1Code = gcs.Key.ItemDim1Code,
        Qty = gcs.Sum<pIOGetPackageDetailFromShelfOrderIDReturnModel>((Func<pIOGetPackageDetailFromShelfOrderIDReturnModel, int>) (x => x.Qty))
      }).ToList<pIOGetPackageDetailFromShelfOrderIDReturnModel>();
      ListView shelfListview = GlobalMob.GetShelfListview("ItemCode,ColorCode,ItemDim1Code,Qty");
      ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) list;
      shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(package.Lst_ItemSelected);
      SelectItem selectItem1 = new SelectItem(shelfListview, selectItem.PackageDescription);
      await ((NavigableElement) package).Navigation.PushAsync((Page) selectItem1);
    }

    private void Lst_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      ((InputView) this.txtPackageBarcode).Text = ((pIOShelfOrderDetailFromPackageReturnModel) e.SelectedItem).PackageDescription;
      this.txtPackageBarcode_Completed((object) null, (EventArgs) null);
      this.BarcodeFocus();
      ((NavigableElement) this).Navigation.PopAsync();
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
      this.LoadShelfOrder();
      this.lstShelfOrder.ItemSelected += (EventHandler<SelectedItemChangedEventArgs>) ((sender, e) =>
      {
        object selectedItem = ((ListView) sender).SelectedItem;
        if (selectedItem == null)
          return;
        ((Page) this).Title = "Koli Kapama";
        pIOUserShelfOrderForPackageReturnModel packageReturnModel = (pIOUserShelfOrderForPackageReturnModel) selectedItem;
        this.selectedShelfOrder = packageReturnModel;
        ((Page) this).Title = packageReturnModel.ShelfOrderNumber + "-" + ((Page) this).Title;
        ((VisualElement) this.stckShelfOrderList).IsVisible = false;
        ((VisualElement) this.stckForm).IsVisible = true;
        this.GetShelfDetail();
      });
    }

    private void LoadShelfOrder()
    {
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetUserShelfOrderForPackage?userID={0}&searchText={1}", (object) GlobalMob.User.UserID, (object) ((InputView) this.txtSearch).Text), (ContentPage) this);
      if (!returnModel.Success)
        return;
      List<pIOUserShelfOrderForPackageReturnModel> packageReturnModelList = GlobalMob.JsonDeserialize<List<pIOUserShelfOrderForPackageReturnModel>>(returnModel.Result);
      ((ItemsView<Cell>) this.lstShelfOrder).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstShelfOrder).ItemsSource = (IEnumerable) packageReturnModelList;
    }

    private void GetShelfDetail()
    {
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfOrderDetailForPackage?shelfOrderID={0}", (object) this.selectedShelfOrder.ShelfOrderID), (ContentPage) this);
      if (!returnModel.Success)
        return;
      this.detailList = GlobalMob.JsonDeserialize<List<pIOShelfOrderDetailFromPackageReturnModel>>(returnModel.Result);
      this.RefreshData();
      Device.BeginInvokeOnMainThread((Action) (async () =>
      {
        await Task.Delay(250);
        this.PackageBarcodeFocus();
      }));
    }

    private void RefreshData()
    {
      this.filterList = this.detailList;
      if (this.selectPackageHeader != null)
        this.filterList = this.selectPackageHeader.SubCurrAccID.HasValue ? this.detailList.Where<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, bool>) (x =>
        {
          if (x.CurrAccCode == this.selectPackageHeader.CurrAccCode)
          {
            int currAccTypeCode1 = x.CurrAccTypeCode;
            short? currAccTypeCode2 = this.selectPackageHeader.CurrAccTypeCode;
            int? nullable = currAccTypeCode2.HasValue ? new int?((int) currAccTypeCode2.GetValueOrDefault()) : new int?();
            int valueOrDefault = nullable.GetValueOrDefault();
            if (currAccTypeCode1 == valueOrDefault & nullable.HasValue)
            {
              Guid? subCurrAccId1 = x.SubCurrAccID;
              Guid? subCurrAccId2 = this.selectPackageHeader.SubCurrAccID;
              if (subCurrAccId1.HasValue != subCurrAccId2.HasValue)
                return false;
              return !subCurrAccId1.HasValue || subCurrAccId1.GetValueOrDefault() == subCurrAccId2.GetValueOrDefault();
            }
          }
          return false;
        })).ToList<pIOShelfOrderDetailFromPackageReturnModel>() : this.detailList.Where<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, bool>) (x =>
        {
          if (!(x.CurrAccCode == this.selectPackageHeader.CurrAccCode))
            return false;
          int currAccTypeCode3 = x.CurrAccTypeCode;
          short? currAccTypeCode4 = this.selectPackageHeader.CurrAccTypeCode;
          int? nullable = currAccTypeCode4.HasValue ? new int?((int) currAccTypeCode4.GetValueOrDefault()) : new int?();
          int valueOrDefault = nullable.GetValueOrDefault();
          return currAccTypeCode3 == valueOrDefault & nullable.HasValue;
        })).ToList<pIOShelfOrderDetailFromPackageReturnModel>();
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) this.filterList.OrderByDescending<pIOShelfOrderDetailFromPackageReturnModel, bool>((Func<pIOShelfOrderDetailFromPackageReturnModel, bool>) (x => x.LastReadBarcode));
      this.SetInfo();
    }

    private void SetInfo()
    {
      double num = this.detailList.Sum<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, double>) (x => x.PickingQty));
      ((MenuItem) this.tInfo).Text = this.detailList.Sum<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, int?>) (x => x.PackageApproveQty)).ToString() + "/" + num.ToString();
    }

    private bool IsUniqueControl(string barcode)
    {
      pIOShelfOrderDetailFromPackageReturnModel packageReturnModel = this.filterList.Where<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, bool>) (x => x.UsedBarcode.Contains(barcode) || x.Barcode.Contains(barcode))).FirstOrDefault<pIOShelfOrderDetailFromPackageReturnModel>();
      return packageReturnModel != null && packageReturnModel.UseSerialNumber;
    }

    private async void txtBarcode_Completed(object sender, EventArgs e)
    {
      Package page1 = this;
      Xamarin.Forms.Entry txt = (Xamarin.Forms.Entry) sender;
      string barcode = ((InputView) txt).Text;
      if (string.IsNullOrEmpty(barcode))
        txt = (Xamarin.Forms.Entry) null;
      else if (page1.selectPackageHeader == null)
      {
        GlobalMob.PlayError();
        int num = await ((Page) page1).DisplayAlert("Uyarı", "Lütfen öncelikle koli seçiniz.", "", "Tamam") ? 1 : 0;
        ((InputView) txt).Text = "";
        page1.PackageBarcodeFocus();
        txt = (Xamarin.Forms.Entry) null;
      }
      else if (barcode.Length < GlobalMob.User.MinimumBarcodeLength)
      {
        page1.BarcodeFocus();
        txt = (Xamarin.Forms.Entry) null;
      }
      else
      {
        bool flag1 = false;
        bool flag2 = txt.ReturnCommandParameter != null;
        bool flag3 = page1.IsUniqueControl(barcode);
        PickerItem selectedItem = (PickerItem) page1.pckBarcodeType.SelectedItem;
        if (selectedItem != null && selectedItem.Code == 2 && ((VisualElement) page1.pckBarcodeType).IsVisible)
          flag1 = true;
        pIOShelfOrderDetailFromPackageReturnModel findItem = page1.filterList.Where<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, bool>) (x =>
        {
          if (!x.Barcode.Contains(barcode))
            return false;
          double pickingQty = x.PickingQty;
          int? packageApproveQty = x.PackageApproveQty;
          double? nullable = packageApproveQty.HasValue ? new double?((double) packageApproveQty.GetValueOrDefault()) : new double?();
          double valueOrDefault = nullable.GetValueOrDefault();
          return pickingQty > valueOrDefault & nullable.HasValue;
        })).FirstOrDefault<pIOShelfOrderDetailFromPackageReturnModel>();
        if (flag2)
        {
          pIOShelfOrderDetailFromPackageReturnModel setItem = (pIOShelfOrderDetailFromPackageReturnModel) txt.ReturnCommandParameter;
          findItem = page1.filterList.Where<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, bool>) (x =>
          {
            if (x.Barcode.Contains(barcode))
            {
              double pickingQty = x.PickingQty;
              int? packageApproveQty = x.PackageApproveQty;
              double? nullable = packageApproveQty.HasValue ? new double?((double) packageApproveQty.GetValueOrDefault()) : new double?();
              double valueOrDefault = nullable.GetValueOrDefault();
              if (pickingQty > valueOrDefault & nullable.HasValue)
                return x.DispOrderLineID == setItem.DispOrderLineID;
            }
            return false;
          })).FirstOrDefault<pIOShelfOrderDetailFromPackageReturnModel>();
        }
        if (flag3)
          findItem = page1.filterList.Where<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, bool>) (x =>
          {
            if (!x.UsedBarcode.Contains(barcode))
              return false;
            double pickingQty = x.PickingQty;
            int? packageApproveQty = x.PackageApproveQty;
            double? nullable = packageApproveQty.HasValue ? new double?((double) packageApproveQty.GetValueOrDefault()) : new double?();
            double valueOrDefault = nullable.GetValueOrDefault();
            return pickingQty > valueOrDefault & nullable.HasValue;
          })).FirstOrDefault<pIOShelfOrderDetailFromPackageReturnModel>();
        if (flag1 && findItem == null)
          findItem = page1.filterList.Where<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, bool>) (x =>
          {
            double pickingQty = x.PickingQty;
            int? packageApproveQty = x.PackageApproveQty;
            double? nullable = packageApproveQty.HasValue ? new double?((double) packageApproveQty.GetValueOrDefault()) : new double?();
            double valueOrDefault = nullable.GetValueOrDefault();
            return !(pickingQty == valueOrDefault & nullable.HasValue) && x.LotBarcode == barcode;
          })).FirstOrDefault<pIOShelfOrderDetailFromPackageReturnModel>();
        if (findItem == null)
        {
          GlobalMob.PlayError();
          int num = await ((Page) page1).DisplayAlert("Uyarı", "Ürün bulunamadı", "", "Tamam") ? 1 : 0;
          ((InputView) txt).Text = "";
          ((VisualElement) txt).Focus();
          txt = (Xamarin.Forms.Entry) null;
        }
        else
        {
          if (!flag2 && page1.selectedShelfOrder.ShelfOrderType == (byte) 11)
          {
            List<pIOShelfOrderDetailFromPackageReturnModel> list = page1.filterList.Where<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, bool>) (x =>
            {
              if (!(x.DispOrderLineID == findItem.DispOrderLineID))
                return false;
              double pickingQty = x.PickingQty;
              int? packageApproveQty = x.PackageApproveQty;
              double? nullable = packageApproveQty.HasValue ? new double?((double) packageApproveQty.GetValueOrDefault()) : new double?();
              double valueOrDefault = nullable.GetValueOrDefault();
              return pickingQty > valueOrDefault & nullable.HasValue;
            })).ToList<pIOShelfOrderDetailFromPackageReturnModel>();
            if (list.Count > 0)
            {
              int pickSetQty = page1.GetPickSetQty(list);
              ((InputView) page1.txtBarcode).Text = "";
              StackLayout stck = new StackLayout();
              stck.Orientation = (StackOrientation) 0;
              ((View) stck).Margin = Thickness.op_Implicit(5.0);
              SoftkeyboardDisabledEntry ent = new SoftkeyboardDisabledEntry();
              ((InputView) ent).Placeholder = "Barkod Okutunuz";
              ent.ReturnCommandParameter = (object) findItem;
              ent.Completed += new EventHandler(page1.txtBarcode_Completed);
              ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) ent);
              bool flag4 = true;
              foreach (pIOShelfOrderDetailFromPackageReturnModel packageReturnModel1 in list)
              {
                int? packageApproveQty = packageReturnModel1.PackageApproveQty;
                int setContentItemQty1 = packageReturnModel1.SetContentItemQty;
                if (packageApproveQty.GetValueOrDefault() < setContentItemQty1 & packageApproveQty.HasValue)
                  packageReturnModel1.SetReadQty = Convert.ToInt32((object) packageReturnModel1.PackageApproveQty);
                packageApproveQty = packageReturnModel1.PackageApproveQty;
                int setContentItemQty2 = packageReturnModel1.SetContentItemQty;
                if (packageApproveQty.GetValueOrDefault() > setContentItemQty2 & packageApproveQty.HasValue)
                {
                  pIOShelfOrderDetailFromPackageReturnModel packageReturnModel2 = packageReturnModel1;
                  packageApproveQty = packageReturnModel1.PackageApproveQty;
                  int setContentItemQty3 = packageReturnModel1.SetContentItemQty;
                  int int32 = Convert.ToInt32((object) (packageApproveQty.HasValue ? new int?(packageApproveQty.GetValueOrDefault() % setContentItemQty3) : new int?()));
                  packageReturnModel2.SetReadQty = int32;
                }
                packageApproveQty = packageReturnModel1.PackageApproveQty;
                int setContentItemQty4 = packageReturnModel1.SetContentItemQty;
                if (packageApproveQty.GetValueOrDefault() == setContentItemQty4 & packageApproveQty.HasValue)
                  packageReturnModel1.SetReadQty = packageReturnModel1.SetContentItemQty;
                packageApproveQty = packageReturnModel1.PackageApproveQty;
                int setContentItemQty5 = packageReturnModel1.SetContentItemQty;
                if (!(packageApproveQty.GetValueOrDefault() == setContentItemQty5 & packageApproveQty.HasValue))
                  flag4 = false;
              }
              if (flag4)
                list.Select<pIOShelfOrderDetailFromPackageReturnModel, pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, pIOShelfOrderDetailFromPackageReturnModel>) (c =>
                {
                  c.SetReadQty = 0;
                  return c;
                })).ToList<pIOShelfOrderDetailFromPackageReturnModel>();
              page1.lstDetails = GlobalMob.GetListview("ItemCodeLong,PickingQty,ItemDescription,PackageApproveQty,SetRemainingQty,Barcode", 2, 2, hasUnEvenRows: true, rowColorCode: "SetRowColorCode");
              ((ItemsView<Cell>) page1.lstDetails).ItemsSource = (IEnumerable) list;
              ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) page1.lstDetails);
              string title = findItem.SetItemCode;
              if (findItem.SetQty > 0)
                title = title + "-" + findItem.SetQty.ToString() + "/" + pickSetQty.ToString();
              SelectItem selectItem = new SelectItem(stck, title);
              await ((NavigableElement) page1).Navigation.PushAsync((Page) selectItem);
              ((VisualElement) ent).Focus();
              txt = (Xamarin.Forms.Entry) null;
              return;
            }
          }
          int qty = page1.GetQty();
          Dictionary<string, string> paramList = new Dictionary<string, string>();
          PickAndSort pickAndSort1 = new PickAndSort()
          {
            PickQty = qty,
            barcode = findItem.Barcode,
            ShelfOrderDetailID = findItem.ShelfOrderDetailID,
            userName = GlobalMob.User.UserName,
            PackageHeaderID = page1.selectPackageHeader.PackageHeaderID,
            DispOrderLineID = findItem.DispOrderLineID,
            DispOrderNumber = findItem.DispOrderNumber
          };
          ReturnModel returnModel1 = new ReturnModel();
          ReturnModel result;
          if (flag1)
          {
            List<pIOShelfOrderDetailFromPackageReturnModel> list = page1.filterList.Where<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, bool>) (x =>
            {
              double pickingQty = x.PickingQty;
              int? nullable1 = x.PackageApproveQty;
              double? nullable2 = nullable1.HasValue ? new double?((double) nullable1.GetValueOrDefault()) : new double?();
              double valueOrDefault = nullable2.GetValueOrDefault();
              if (!(pickingQty == valueOrDefault & nullable2.HasValue) && x.DispOrderNumber == findItem.DispOrderNumber)
              {
                nullable1 = x.OrderLineSumID;
                int? orderLineSumId = findItem.OrderLineSumID;
                if (nullable1.GetValueOrDefault() == orderLineSumId.GetValueOrDefault() & nullable1.HasValue == orderLineSumId.HasValue)
                  return x.LotBarcode == barcode;
              }
              return false;
            })).ToList<pIOShelfOrderDetailFromPackageReturnModel>();
            pickAndSort1.Detail = new List<PickAndSortDetail>();
            foreach (pIOShelfOrderDetailFromPackageReturnModel packageReturnModel in list)
              pickAndSort1.Detail.Add(new PickAndSortDetail()
              {
                Barcode = packageReturnModel.Barcode,
                ColorCode = packageReturnModel.ColorCode,
                DispOrderNumber = packageReturnModel.DispOrderNumber,
                ItemCode = packageReturnModel.ItemCode,
                ItemDim1Code = packageReturnModel.ItemDim1Code,
                ItemDim2Code = packageReturnModel.ItemDim2Code,
                OrderQty = packageReturnModel.PickingQty,
                PickingQty = (double) Convert.ToInt32((object) packageReturnModel.PackageApproveQty),
                ShelfOrderDetailID = packageReturnModel.ShelfOrderDetailID,
                ShelfOrderID = packageReturnModel.ShelfOrderID
              });
            pickAndSort1.barcode = barcode;
            string str = JsonConvert.SerializeObject((object) pickAndSort1);
            paramList.Add("json", str);
            result = GlobalMob.PostJson("SavePackageDetailLot", paramList, (ContentPage) page1).Result;
          }
          else
          {
            string str = JsonConvert.SerializeObject((object) pickAndSort1);
            paramList.Add("json", str);
            result = GlobalMob.PostJson("SavePackageDetail", paramList, (ContentPage) page1).Result;
          }
          if (result.Success)
          {
            if (flag1)
            {
              ReturnModel returnModel2 = JsonConvert.DeserializeObject<ReturnModel>(result.Result);
              if (returnModel2.Success)
              {
                List<PickAndSort> pickAndSortList = JsonConvert.DeserializeObject<List<PickAndSort>>(returnModel2.Result);
                page1.filterList.Select<pIOShelfOrderDetailFromPackageReturnModel, pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, pIOShelfOrderDetailFromPackageReturnModel>) (c =>
                {
                  c.LastReadBarcode = false;
                  return c;
                })).ToList<pIOShelfOrderDetailFromPackageReturnModel>();
                foreach (PickAndSort pickAndSort2 in pickAndSortList)
                {
                  PickAndSort pickItem = pickAndSort2;
                  pIOShelfOrderDetailFromPackageReturnModel packageReturnModel = page1.filterList.Where<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, bool>) (x => x.ShelfOrderDetailID == pickItem.ShelfOrderDetailID)).FirstOrDefault<pIOShelfOrderDetailFromPackageReturnModel>();
                  packageReturnModel.ApproveQty += (double) pickItem.PickQty;
                  packageReturnModel.PackedQty += pickItem.PickQty;
                  int? packageApproveQty = packageReturnModel.PackageApproveQty;
                  int pickQty = pickItem.PickQty;
                  packageReturnModel.PackageApproveQty = packageApproveQty.HasValue ? new int?(packageApproveQty.GetValueOrDefault() + pickQty) : new int?();
                  packageReturnModel.PackageCode = page1.selectPackageHeader.PackageCode;
                  packageReturnModel.PackageHeaderID = new int?(page1.selectPackageHeader.PackageHeaderID);
                  packageReturnModel.LastReadBarcode = true;
                }
                GlobalMob.PlaySave();
                page1.RefreshData();
                page1.CompleteControl();
                page1.BarcodeFocus();
              }
              else
              {
                GlobalMob.PlayError();
                int num = await ((Page) page1).DisplayAlert("Uyarı", returnModel2.ErrorMessage, "", "Tamam") ? 1 : 0;
                page1.BarcodeFocus();
                txt = (Xamarin.Forms.Entry) null;
                return;
              }
            }
            else
            {
              ReturnModel returnModel3 = JsonConvert.DeserializeObject<ReturnModel>(result.Result);
              if (returnModel3.Success)
              {
                GlobalMob.PlaySave();
                findItem.ApproveQty += (double) pickAndSort1.PickQty;
                findItem.PackedQty += pickAndSort1.PickQty;
                findItem.SetReadQty += pickAndSort1.PickQty;
                pIOShelfOrderDetailFromPackageReturnModel packageReturnModel = findItem;
                int? packageApproveQty = packageReturnModel.PackageApproveQty;
                int pickQty = pickAndSort1.PickQty;
                packageReturnModel.PackageApproveQty = packageApproveQty.HasValue ? new int?(packageApproveQty.GetValueOrDefault() + pickQty) : new int?();
                findItem.PackageCode = page1.selectPackageHeader.PackageCode;
                findItem.PackageHeaderID = new int?(page1.selectPackageHeader.PackageHeaderID);
                page1.detailList.Select<pIOShelfOrderDetailFromPackageReturnModel, pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, pIOShelfOrderDetailFromPackageReturnModel>) (c =>
                {
                  c.LastReadBarcode = false;
                  return c;
                })).ToList<pIOShelfOrderDetailFromPackageReturnModel>();
                findItem.LastReadBarcode = true;
                page1.RefreshData();
                page1.CompleteControl();
                page1.BarcodeFocus();
              }
              else
              {
                GlobalMob.PlayError();
                int num = await ((Page) page1).DisplayAlert("Uyarı", returnModel3.ErrorMessage, "", "Tamam") ? 1 : 0;
                page1.BarcodeFocus();
                txt = (Xamarin.Forms.Entry) null;
                return;
              }
            }
          }
          if (!flag2)
          {
            txt = (Xamarin.Forms.Entry) null;
          }
          else
          {
            pIOShelfOrderDetailFromPackageReturnModel item = (pIOShelfOrderDetailFromPackageReturnModel) txt.ReturnCommandParameter;
            List<pIOShelfOrderDetailFromPackageReturnModel> list = page1.filterList.Where<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, bool>) (x =>
            {
              if (!(x.DispOrderLineID == item.DispOrderLineID))
                return false;
              double pickingQty = x.PickingQty;
              int? packageApproveQty = x.PackageApproveQty;
              double? nullable = packageApproveQty.HasValue ? new double?((double) packageApproveQty.GetValueOrDefault()) : new double?();
              double valueOrDefault = nullable.GetValueOrDefault();
              return pickingQty > valueOrDefault & nullable.HasValue;
            })).OrderByDescending<pIOShelfOrderDetailFromPackageReturnModel, bool>((Func<pIOShelfOrderDetailFromPackageReturnModel, bool>) (x => x.LastReadBarcode)).ToList<pIOShelfOrderDetailFromPackageReturnModel>();
            ((ItemsView<Cell>) page1.lstDetails).ItemsSource = (IEnumerable) null;
            ((ItemsView<Cell>) page1.lstDetails).ItemsSource = (IEnumerable) list;
            if (list.Where<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, bool>) (x => x.SetReadQty != x.SetContentItemQty)).Count<pIOShelfOrderDetailFromPackageReturnModel>() <= 0 && page1.selectedShelfOrder.IsCloseScreenForSet)
            {
              list.Select<pIOShelfOrderDetailFromPackageReturnModel, pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, pIOShelfOrderDetailFromPackageReturnModel>) (c =>
              {
                c.LastReadBarcode = true;
                return c;
              })).ToList<pIOShelfOrderDetailFromPackageReturnModel>();
              list.Select<pIOShelfOrderDetailFromPackageReturnModel, pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, pIOShelfOrderDetailFromPackageReturnModel>) (c =>
              {
                c.SetReadQty = 0;
                return c;
              })).ToList<pIOShelfOrderDetailFromPackageReturnModel>();
              Page page2 = await ((NavigableElement) page1).Navigation.PopAsync();
              page1.BarcodeFocus();
              txt = (Xamarin.Forms.Entry) null;
            }
            else
            {
              ((InputView) txt).Text = "";
              ((VisualElement) txt).Focus();
              txt = (Xamarin.Forms.Entry) null;
            }
          }
        }
      }
    }

    private int GetPickSetQty(
      List<pIOShelfOrderDetailFromPackageReturnModel> list)
    {
      return Convert.ToInt32((object) list.Min<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, int?>) (x =>
      {
        int? packageApproveQty = x.PackageApproveQty;
        int setContentItemQty = x.SetContentItemQty;
        return !packageApproveQty.HasValue ? new int?() : new int?(packageApproveQty.GetValueOrDefault() / setContentItemQty);
      })));
    }

    private async void CompleteControl()
    {
      Package package = this;
      double num1 = package.detailList.Sum<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, double>) (x => x.PickingQty));
      int? nullable1 = package.detailList.Sum<pIOShelfOrderDetailFromPackageReturnModel>((Func<pIOShelfOrderDetailFromPackageReturnModel, int?>) (x => x.PackageApproveQty));
      double num2 = num1;
      int? nullable2 = nullable1;
      double? nullable3 = nullable2.HasValue ? new double?((double) nullable2.GetValueOrDefault()) : new double?();
      if (!(num2 == nullable3.GetValueOrDefault() & nullable3.HasValue))
        return;
      int num3 = await ((Page) package).DisplayAlert("Uyarı", "Ürünler tamamlandı", "", "Tamam") ? 1 : 0;
    }

    private int GetQty()
    {
      try
      {
        return Convert.ToInt32(((InputView) this.txtQty).Text);
      }
      catch (Exception ex)
      {
        ((InputView) this.txtQty).Text = "1";
        return 1;
      }
    }

    private void PackageBarcodeFocus()
    {
      ((InputView) this.txtPackageBarcode).Text = "";
      ((VisualElement) this.txtPackageBarcode).Focus();
    }

    private void BarcodeFocus()
    {
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode).Focus();
    }

    private async void imgSearch_Clicked(object sender, EventArgs e)
    {
      Package package = this;
      await NavigationExtension.PushPopupAsync(((NavigableElement) package).Navigation, GlobalMob.ShowLoading(), true);
      package.LoadShelfOrder();
      GlobalMob.CloseLoading();
    }

    private async void txtSearch_Completed(object sender, EventArgs e)
    {
      Package package = this;
      await NavigationExtension.PushPopupAsync(((NavigableElement) package).Navigation, GlobalMob.ShowLoading(), true);
      package.LoadShelfOrder();
      GlobalMob.CloseLoading();
    }

    private async void btnCreateNewPackage_Clicked(object sender, EventArgs e)
    {
      Package page = this;
      if (!await ((Page) page).DisplayAlert("", "Yeni koli oluşturulacak emin misiniz?", "Evet", "Hayır"))
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

    private async void LstCustomer_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      Package package = this;
      pIOShelfOrderCustomerReturnModel findItem = (pIOShelfOrderCustomerReturnModel) e.SelectedItem;
      if (sender != null)
      {
        Page page = await ((NavigableElement) package).Navigation.PopAsync();
      }
      package.CreateNewPackage(package.selectedShelfOrder.ShelfOrderID, findItem.CurrAccCode, Convert.ToInt32((object) findItem.CurrAccTypeCode), findItem.SubCurrAccID, findItem.CurrAccDescription);
      findItem = (pIOShelfOrderCustomerReturnModel) null;
    }

    private async void CreateNewPackage(
      int shelfOrderID,
      string currAccCode,
      int currAccTypeCode,
      Guid? subcurrAccID,
      string customerName)
    {
      Package package = this;
      package.selectPackageCustomerName = customerName;
      string text = "";
      if (GlobalMob.User.IsAskPackageCode)
      {
        text = await GlobalMob.InputBox(((NavigableElement) package).Navigation, "Koli Kodu", "Koli kodu giriniz/okutunuz", Keyboard.Chat);
        if (string.IsNullOrEmpty(text))
          return;
        if (!GlobalMob.PrefixControl(text, package.selectedShelfOrder.PackageNumberPrefix))
        {
          GlobalMob.PlayError();
          int num = await ((Page) package).DisplayAlert("Hata", "Hatalı koli kodu : " + text + "\nKoli kodları önekleri:" + package.selectedShelfOrder.PackageNumberPrefix, "", "Tamam") ? 1 : 0;
          package.PackageBarcodeFocus();
          return;
        }
      }
      package.newPackageHeader = new ztIOShelfPackageHeader()
      {
        CreatedDate = DateTime.Now,
        CreatedUserName = GlobalMob.User.UserName,
        ShelfOrderID = shelfOrderID,
        UpdatedDate = new DateTime?(DateTime.Now),
        UpdatedUserName = GlobalMob.User.UserName,
        PackageDate = DateTime.Now,
        CurrAccCode = currAccCode,
        Description = text,
        CurrAccTypeCode = new short?((short) Convert.ToSByte(currAccTypeCode)),
        SubCurrAccID = subcurrAccID,
        UserID = new int?(GlobalMob.User.UserID),
        PackageTypeID = new int?(0)
      };
      if (GlobalMob.User.IsAskPackageType)
      {
        package.GetPackageTypes();
        ListView shelfListview = GlobalMob.GetShelfListview("PackageType,PackageDesc");
        ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) package.packageTypeList;
        shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(package.LstPackageType_ItemSelected);
        SelectItem selectItem = new SelectItem(shelfListview, "Koli tipi seçiniz");
        await ((NavigableElement) package).Navigation.PushAsync((Page) selectItem);
      }
      else
        package.AddPackage(package.newPackageHeader, customerName, false);
    }

    private void LstPackageType_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      ((NavigableElement) this).Navigation.PopAsync();
      this.newPackageHeader.PackageTypeID = new int?(((ztIOShelfPackageType) e.SelectedItem).PackageTypeID);
      this.AddPackage(this.newPackageHeader, this.selectPackageCustomerName, true);
      this.selectPackageCustomerName = "";
    }

    private void GetPackageTypes()
    {
      if (this.packageTypeList != null && this.packageTypeList.Count > 0)
        return;
      ReturnModel returnModel = GlobalMob.PostJson(nameof (GetPackageTypes), (ContentPage) this);
      if (!returnModel.Success)
        return;
      this.packageTypeList = JsonConvert.DeserializeObject<List<ztIOShelfPackageType>>(returnModel.Result);
    }

    private void AddPackage(ztIOShelfPackageHeader header, string customerName, bool isPackageType)
    {
      Dictionary<string, string> paramList = new Dictionary<string, string>();
      string str = JsonConvert.SerializeObject((object) header);
      paramList.Add("json", str);
      ReturnModel result = GlobalMob.PostJson("CreateNewPackage", paramList, (ContentPage) this).Result;
      if (!result.Success)
        return;
      ztIOShelfPackageHeader shelfPackageHeader = JsonConvert.DeserializeObject<ztIOShelfPackageHeader>(result.Result);
      if (shelfPackageHeader == null)
      {
        GlobalMob.PlayError();
        ((Page) this).DisplayAlert("Hata", "Bu koli daha önce eklemiş\nKoli No:" + header.Description, "", "Tamam");
        this.PackageBarcodeFocus();
      }
      else
      {
        if (!GlobalMob.User.IsAskPackageCode)
          shelfPackageHeader.PackageCode = shelfPackageHeader.Description;
        this.selectPackageHeader = shelfPackageHeader;
        ((VisualElement) this.txtBarcode).IsEnabled = true;
        this.lblListHeader.Text = this.selectPackageHeader.CurrAccCode;
        this.RefreshData();
        if (shelfPackageHeader == null || shelfPackageHeader.PackageHeaderID <= 0)
          return;
        ((Page) this).DisplayAlert("Yeni Koli", "Yeni Koli eklendi\nKoli No:" + this.selectPackageHeader.Description, "", "Tamam");
        ((InputView) this.txtPackageBarcode).Text = shelfPackageHeader.Description;
        ((InputView) this.txtBarcode).Text = "";
        ((VisualElement) this.txtBarcode).Focus();
      }
    }

    private void txtPackageBarcode_Completed(object sender, EventArgs e)
    {
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetPackageHeader?shelfOrderID={0}&description={1}", (object) this.selectedShelfOrder.ShelfOrderID, (object) ((InputView) this.txtPackageBarcode).Text), (ContentPage) this);
      if (!returnModel.Success)
        return;
      this.selectPackageHeader = JsonConvert.DeserializeObject<ztIOShelfPackageHeader>(returnModel.Result);
      if (this.selectPackageHeader == null)
      {
        GlobalMob.PlayError();
        ((Page) this).DisplayAlert("Uyarı", "Koli bulunamadı." + ((InputView) this.txtPackageBarcode).Text, "", "Tamam");
        this.PackageBarcodeFocus();
      }
      else
      {
        ((InputView) this.txtPackageBarcode).Text = this.selectPackageHeader.Description;
        this.lblListHeader.Text = this.selectPackageHeader.CurrAccCode;
        this.RefreshData();
        this.BarcodeFocus();
      }
    }

    private async void btnCekiListesiPrint_Clicked(object sender, EventArgs e)
    {
      Package page = this;
      if (page.selectPackageHeader == null)
      {
        GlobalMob.PlayError();
        int num = await ((Page) page).DisplayAlert("Hata", "Lütfen öncelikle koli seçiniz", "", "Tamam") ? 1 : 0;
        page.PackageBarcodeFocus();
      }
      else
      {
        await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
        int num = 2;
        List<BLReport> repList = new List<BLReport>();
        BLReport blReport = new BLReport()
        {
          ReportTypeID = 2,
          UserID = GlobalMob.User.UserID,
          FileType = num,
          PrinterBrandID = 2,
          NetworkPrinter = page.selectedShelfOrder.NetworkPrinter
        };
        blReport.ParamList = new List<BLReportParam>();
        blReport.ParamList.Add(new BLReportParam()
        {
          ParamName = "PackageCode",
          ParamValue = page.selectPackageHeader.PackageCode,
          ParamType = 20
        });
        repList.Add(blReport);
        GlobalMob.BLPrint(repList, (object) page.selectPackageHeader, (ContentPage) page);
        ((VisualElement) page.txtBarcode).IsEnabled = false;
        GlobalMob.CloseLoading();
      }
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (Package).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/Package.xaml",
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
        ImageButton imageButton = new ImageButton();
        StackLayout stackLayout1 = new StackLayout();
        Label label1 = new Label();
        StackLayout stackLayout2 = new StackLayout();
        BindingExtension bindingExtension1 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView1 = new ListView();
        StackLayout stackLayout3 = new StackLayout();
        KeyboardEnableEffect keyboardEnableEffect1 = new KeyboardEnableEffect();
        Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
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
        StackLayout stackLayout4 = new StackLayout();
        KeyboardEnableEffect keyboardEnableEffect2 = new KeyboardEnableEffect();
        Xamarin.Forms.Entry entry3 = new Xamarin.Forms.Entry();
        Xamarin.Forms.Entry entry4 = new Xamarin.Forms.Entry();
        BindingExtension bindingExtension6 = new BindingExtension();
        BindingExtension bindingExtension7 = new BindingExtension();
        Picker picker = new Picker();
        StackLayout stackLayout5 = new StackLayout();
        BindingExtension bindingExtension8 = new BindingExtension();
        ReferenceExtension referenceExtension5 = new ReferenceExtension();
        BindingExtension bindingExtension9 = new BindingExtension();
        ReferenceExtension referenceExtension6 = new ReferenceExtension();
        BindingExtension bindingExtension10 = new BindingExtension();
        Label label2 = new Label();
        StackLayout stackLayout6 = new StackLayout();
        DataTemplate dataTemplate2 = new DataTemplate();
        ListView listView2 = new ListView();
        StackLayout stackLayout7 = new StackLayout();
        StackLayout stackLayout8 = new StackLayout();
        Package package;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (package = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) package, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("package", (object) package);
        if (((Element) package).StyleId == null)
          ((Element) package).StyleId = "package";
        ((INameScope) nameScope).RegisterName("stckContent", (object) stackLayout8);
        if (((Element) stackLayout8).StyleId == null)
          ((Element) stackLayout8).StyleId = "stckContent";
        ((INameScope) nameScope).RegisterName("stckShelfOrderList", (object) stackLayout3);
        if (((Element) stackLayout3).StyleId == null)
          ((Element) stackLayout3).StyleId = "stckShelfOrderList";
        ((INameScope) nameScope).RegisterName("txtSearch", (object) entry1);
        if (((Element) entry1).StyleId == null)
          ((Element) entry1).StyleId = "txtSearch";
        ((INameScope) nameScope).RegisterName("imgSearch", (object) imageButton);
        if (((Element) imageButton).StyleId == null)
          ((Element) imageButton).StyleId = "imgSearch";
        ((INameScope) nameScope).RegisterName("stckEmptyMessage", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckEmptyMessage";
        ((INameScope) nameScope).RegisterName("lstShelfOrder", (object) listView1);
        if (((Element) listView1).StyleId == null)
          ((Element) listView1).StyleId = "lstShelfOrder";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout7);
        if (((Element) stackLayout7).StyleId == null)
          ((Element) stackLayout7).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("stckPackage", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckPackage";
        ((INameScope) nameScope).RegisterName("txtPackageBarcode", (object) entry2);
        if (((Element) entry2).StyleId == null)
          ((Element) entry2).StyleId = "txtPackageBarcode";
        ((INameScope) nameScope).RegisterName("btnCekiListesiPrint", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnCekiListesiPrint";
        ((INameScope) nameScope).RegisterName("btnCreateNewPackage", (object) button2);
        if (((Element) button2).StyleId == null)
          ((Element) button2).StyleId = "btnCreateNewPackage";
        ((INameScope) nameScope).RegisterName("stckBarcode", (object) stackLayout5);
        if (((Element) stackLayout5).StyleId == null)
          ((Element) stackLayout5).StyleId = "stckBarcode";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) entry3);
        if (((Element) entry3).StyleId == null)
          ((Element) entry3).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("txtQty", (object) entry4);
        if (((Element) entry4).StyleId == null)
          ((Element) entry4).StyleId = "txtQty";
        ((INameScope) nameScope).RegisterName("pckBarcodeType", (object) picker);
        if (((Element) picker).StyleId == null)
          ((Element) picker).StyleId = "pckBarcodeType";
        ((INameScope) nameScope).RegisterName("lstShelfDetail", (object) listView2);
        if (((Element) listView2).StyleId == null)
          ((Element) listView2).StyleId = "lstShelfDetail";
        ((INameScope) nameScope).RegisterName("lblListHeader", (object) label2);
        if (((Element) label2).StyleId == null)
          ((Element) label2).StyleId = "lblListHeader";
        this.package = (ContentPage) package;
        this.stckContent = stackLayout8;
        this.stckShelfOrderList = stackLayout3;
        this.txtSearch = entry1;
        this.imgSearch = imageButton;
        this.stckEmptyMessage = stackLayout2;
        this.lstShelfOrder = listView1;
        this.stckForm = stackLayout7;
        this.stckPackage = stackLayout4;
        this.txtPackageBarcode = entry2;
        this.btnCekiListesiPrint = button1;
        this.btnCreateNewPackage = button2;
        this.stckBarcode = stackLayout5;
        this.txtBarcode = entry3;
        this.txtQty = entry4;
        this.pckBarcodeType = picker;
        this.lstShelfDetail = listView2;
        this.lblListHeader = label2;
        ((BindableObject) stackLayout8).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout8).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout8).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout3).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Ara");
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry1).SetValue(View.MarginProperty, (object) new Thickness(5.0));
        entry1.Completed += new EventHandler(package.txtSearch_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 22);
        ((BindableObject) imageButton).SetValue(ImageButton.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("search.png"));
        ((BindableObject) imageButton).SetValue(ImageButton.AspectProperty, (object) (Aspect) 0);
        ((BindableObject) imageButton).SetValue(VisualElement.BackgroundColorProperty, (object) new Color(0.55686277151107788, 0.31764706969261169, 0.59607845544815063, 1.0));
        ((BindableObject) imageButton).SetValue(View.MarginProperty, (object) new Thickness(5.0));
        imageButton.Clicked += new EventHandler(package.imgSearch_Clicked);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) imageButton);
        VisualDiagnostics.RegisterSourceInfo((object) imageButton, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 18);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) stackLayout2).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) stackLayout2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) label1).SetValue(Label.TextProperty, (object) "Bekleyen Raf Emri Bulunmamaktadır.");
        ((BindableObject) label1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.CenterAndExpand);
        ((BindableObject) label1).SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Label label3 = label1;
        BindableProperty fontSizeProperty = Label.FontSizeProperty;
        FontSizeConverter fontSizeConverter = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 5];
        objArray1[0] = (object) label1;
        objArray1[1] = (object) stackLayout2;
        objArray1[2] = (object) stackLayout3;
        objArray1[3] = (object) stackLayout8;
        objArray1[4] = (object) package;
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
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (Package).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(21, 128)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter).ConvertFromInvariantString("Medium", (IServiceProvider) xamlServiceProvider1);
        ((BindableObject) label3).SetValue(fontSizeProperty, obj2);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) label1);
        VisualDiagnostics.RegisterSourceInfo((object) label1, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 21, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 18);
        ((BindableObject) listView1).SetValue(ListView.RowHeightProperty, (object) 80);
        bindingExtension1.Path = ".";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView1).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase1);
        ((BindableObject) listView1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        DataTemplate dataTemplate3 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Package.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_13 xamlCdataTemplate13 = new Package.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_13();
        object[] objArray2 = new object[0 + 5];
        objArray2[0] = (object) dataTemplate1;
        objArray2[1] = (object) listView1;
        objArray2[2] = (object) stackLayout3;
        objArray2[3] = (object) stackLayout8;
        objArray2[4] = (object) package;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate13.parentValues = objArray2;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate13.root = package;
        // ISSUE: reference to a compiler-generated method
        Func<object> func1 = new Func<object>(xamlCdataTemplate13.LoadDataTemplate);
        ((IDataTemplate) dataTemplate3).LoadTemplate = func1;
        ((BindableObject) listView1).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 25, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) listView1);
        VisualDiagnostics.RegisterSourceInfo((object) listView1, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 14);
        ((BindableObject) stackLayout7).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout7).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout7).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) stackLayout7).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Koli Barkodu");
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        entry2.Completed += new EventHandler(package.txtPackageBarcode_Completed);
        ((BindableObject) entry2).SetValue(KeyboardEffect.EnableKeyboardProperty, (object) false);
        ((ICollection<Effect>) ((Element) entry2).Effects).Add((Effect) keyboardEnableEffect1);
        VisualDiagnostics.RegisterSourceInfo((object) keyboardEnableEffect1, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 56, 30);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 52, 22);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "Ç");
        ((BindableObject) button1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        button1.Clicked += new EventHandler(package.btnCekiListesiPrint_Clicked);
        ((BindableObject) button1).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) button1).SetValue(VisualElement.WidthRequestProperty, (object) 50.0);
        ((BindableObject) button1).SetValue(VisualElement.HeightRequestProperty, (object) 50.0);
        referenceExtension1.Name = "package";
        ReferenceExtension referenceExtension7 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 6];
        objArray3[0] = (object) bindingExtension2;
        objArray3[1] = (object) button1;
        objArray3[2] = (object) stackLayout4;
        objArray3[3] = (object) stackLayout7;
        objArray3[4] = (object) stackLayout8;
        objArray3[5] = (object) package;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray3, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver2.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver2.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver2.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (Package).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(62, 28)));
        object obj4 = ((IMarkupExtension) referenceExtension7).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension2.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 62, 28);
        bindingExtension2.Path = "ButtonColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase2);
        referenceExtension2.Name = "package";
        ReferenceExtension referenceExtension8 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 6];
        objArray4[0] = (object) bindingExtension3;
        objArray4[1] = (object) button1;
        objArray4[2] = (object) stackLayout4;
        objArray4[3] = (object) stackLayout7;
        objArray4[4] = (object) stackLayout8;
        objArray4[5] = (object) package;
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
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (Package).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(63, 28)));
        object obj6 = ((IMarkupExtension) referenceExtension8).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension3.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 63, 28);
        bindingExtension3.Path = "TextColor";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(Button.TextColorProperty, bindingBase3);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 59, 22);
        ((BindableObject) button2).SetValue(Button.TextProperty, (object) "+");
        ((BindableObject) button2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        button2.Clicked += new EventHandler(package.btnCreateNewPackage_Clicked);
        ((BindableObject) button2).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) button2).SetValue(VisualElement.WidthRequestProperty, (object) 50.0);
        ((BindableObject) button2).SetValue(VisualElement.HeightRequestProperty, (object) 50.0);
        referenceExtension3.Name = "package";
        ReferenceExtension referenceExtension9 = referenceExtension3;
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray5 = new object[0 + 6];
        objArray5[0] = (object) bindingExtension4;
        objArray5[1] = (object) button2;
        objArray5[2] = (object) stackLayout4;
        objArray5[3] = (object) stackLayout7;
        objArray5[4] = (object) stackLayout8;
        objArray5[5] = (object) package;
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
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (Package).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(67, 28)));
        object obj8 = ((IMarkupExtension) referenceExtension9).ProvideValue((IServiceProvider) xamlServiceProvider4);
        bindingExtension4.Source = obj8;
        VisualDiagnostics.RegisterSourceInfo(obj8, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 67, 28);
        bindingExtension4.Path = "ButtonColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase4);
        referenceExtension4.Name = "package";
        ReferenceExtension referenceExtension10 = referenceExtension4;
        XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
        Type type9 = typeof (IProvideValueTarget);
        object[] objArray6 = new object[0 + 6];
        objArray6[0] = (object) bindingExtension5;
        objArray6[1] = (object) button2;
        objArray6[2] = (object) stackLayout4;
        objArray6[3] = (object) stackLayout7;
        objArray6[4] = (object) stackLayout8;
        objArray6[5] = (object) package;
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
        XamlTypeResolver xamlTypeResolver5 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver5, typeof (Package).GetTypeInfo().Assembly);
        xamlServiceProvider5.Add(type10, (object) xamlTypeResolver5);
        xamlServiceProvider5.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(68, 28)));
        object obj10 = ((IMarkupExtension) referenceExtension10).ProvideValue((IServiceProvider) xamlServiceProvider5);
        bindingExtension5.Source = obj10;
        VisualDiagnostics.RegisterSourceInfo(obj10, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 68, 28);
        bindingExtension5.Path = "TextColor";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(Button.TextColorProperty, bindingBase5);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) button2);
        VisualDiagnostics.RegisterSourceInfo((object) button2, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 64, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 51, 18);
        ((BindableObject) stackLayout5).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry3).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod No Giriniz/Okutunuz");
        ((BindableObject) entry3).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("True"));
        entry3.Completed += new EventHandler(package.txtBarcode_Completed);
        ((BindableObject) entry3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry3).SetValue(KeyboardEffect.EnableKeyboardProperty, (object) false);
        ((ICollection<Effect>) ((Element) entry3).Effects).Add((Effect) keyboardEnableEffect2);
        VisualDiagnostics.RegisterSourceInfo((object) keyboardEnableEffect2, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 75, 30);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) entry3);
        VisualDiagnostics.RegisterSourceInfo((object) entry3, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 71, 22);
        ((BindableObject) entry4).SetValue(Xamarin.Forms.Entry.TextProperty, (object) "1");
        ((BindableObject) entry4).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Miktar");
        ((BindableObject) entry4).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry4).SetValue(Xamarin.Forms.Entry.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) entry4).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) entry4);
        VisualDiagnostics.RegisterSourceInfo((object) entry4, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 78, 22);
        ((BindableObject) picker).SetValue(Picker.TitleProperty, (object) "Tip");
        bindingExtension6.Path = ".";
        BindingBase bindingBase6 = ((IMarkupExtension<BindingBase>) bindingExtension6).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker).SetBinding(Picker.ItemsSourceProperty, bindingBase6);
        bindingExtension7.Path = "Caption";
        BindingBase bindingBase7 = ((IMarkupExtension<BindingBase>) bindingExtension7).ProvideValue((IServiceProvider) null);
        picker.ItemDisplayBinding = bindingBase7;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase7, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 81, 33);
        ((BindableObject) picker).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) picker).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) picker);
        VisualDiagnostics.RegisterSourceInfo((object) picker, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 80, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 70, 18);
        bindingExtension8.Path = ".";
        BindingBase bindingBase8 = ((IMarkupExtension<BindingBase>) bindingExtension8).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView2).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase8);
        ((BindableObject) listView2).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) listView2).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        ((BindableObject) listView2).SetValue(VisualElement.HeightRequestProperty, (object) 5000.0);
        ((BindableObject) listView2).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        ((BindableObject) label2).SetValue(Label.TextProperty, (object) "Ürünler");
        ((BindableObject) label2).SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        referenceExtension5.Name = "package";
        ReferenceExtension referenceExtension11 = referenceExtension5;
        XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
        Type type11 = typeof (IProvideValueTarget);
        object[] objArray7 = new object[0 + 7];
        objArray7[0] = (object) bindingExtension9;
        objArray7[1] = (object) label2;
        objArray7[2] = (object) stackLayout6;
        objArray7[3] = (object) listView2;
        objArray7[4] = (object) stackLayout7;
        objArray7[5] = (object) stackLayout8;
        objArray7[6] = (object) package;
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
        XamlTypeResolver xamlTypeResolver6 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver6, typeof (Package).GetTypeInfo().Assembly);
        xamlServiceProvider6.Add(type12, (object) xamlTypeResolver6);
        xamlServiceProvider6.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(91, 36)));
        object obj12 = ((IMarkupExtension) referenceExtension11).ProvideValue((IServiceProvider) xamlServiceProvider6);
        bindingExtension9.Source = obj12;
        VisualDiagnostics.RegisterSourceInfo(obj12, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 91, 36);
        bindingExtension9.Path = "ButtonColor";
        BindingBase bindingBase9 = ((IMarkupExtension<BindingBase>) bindingExtension9).ProvideValue((IServiceProvider) null);
        ((BindableObject) label2).SetBinding(Label.TextColorProperty, bindingBase9);
        referenceExtension6.Name = "package";
        ReferenceExtension referenceExtension12 = referenceExtension6;
        XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
        Type type13 = typeof (IProvideValueTarget);
        object[] objArray8 = new object[0 + 7];
        objArray8[0] = (object) bindingExtension10;
        objArray8[1] = (object) label2;
        objArray8[2] = (object) stackLayout6;
        objArray8[3] = (object) listView2;
        objArray8[4] = (object) stackLayout7;
        objArray8[5] = (object) stackLayout8;
        objArray8[6] = (object) package;
        SimpleValueTargetProvider valueTargetProvider7;
        object obj13 = (object) (valueTargetProvider7 = new SimpleValueTargetProvider(objArray8, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider7.Add(type13, (object) valueTargetProvider7);
        xamlServiceProvider7.Add(typeof (IReferenceProvider), obj13);
        Type type14 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver7 = new XmlNamespaceResolver();
        namespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver7.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver7.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver7.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver7 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver7, typeof (Package).GetTypeInfo().Assembly);
        xamlServiceProvider7.Add(type14, (object) xamlTypeResolver7);
        xamlServiceProvider7.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(92, 36)));
        object obj14 = ((IMarkupExtension) referenceExtension12).ProvideValue((IServiceProvider) xamlServiceProvider7);
        bindingExtension10.Source = obj14;
        VisualDiagnostics.RegisterSourceInfo(obj14, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 92, 36);
        bindingExtension10.Path = "TextColor";
        BindingBase bindingBase10 = ((IMarkupExtension<BindingBase>) bindingExtension10).ProvideValue((IServiceProvider) null);
        ((BindableObject) label2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase10);
        ((BindableObject) label2).SetValue(Label.FontProperty, new FontTypeConverter().ConvertFromInvariantString("Bold,20"));
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) label2);
        VisualDiagnostics.RegisterSourceInfo((object) label2, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 89, 30);
        ((BindableObject) listView2).SetValue(ListView.HeaderProperty, (object) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 88, 26);
        DataTemplate dataTemplate4 = dataTemplate2;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Package.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_14 xamlCdataTemplate14 = new Package.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_14();
        object[] objArray9 = new object[0 + 5];
        objArray9[0] = (object) dataTemplate2;
        objArray9[1] = (object) listView2;
        objArray9[2] = (object) stackLayout7;
        objArray9[3] = (object) stackLayout8;
        objArray9[4] = (object) package;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate14.parentValues = objArray9;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate14.root = package;
        // ISSUE: reference to a compiler-generated method
        Func<object> func2 = new Func<object>(xamlCdataTemplate14.LoadDataTemplate);
        ((IDataTemplate) dataTemplate4).LoadTemplate = func2;
        ((BindableObject) listView2).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate2);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate2, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 97, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) listView2);
        VisualDiagnostics.RegisterSourceInfo((object) listView2, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 84, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 50, 14);
        ((BindableObject) package).SetValue(ContentPage.ContentProperty, (object) stackLayout8);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout8, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 10);
        VisualDiagnostics.RegisterSourceInfo((object) package, new Uri("Views\\Package.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<Package>(this, typeof (Package));
      this.package = NameScopeExtensions.FindByName<ContentPage>((Element) this, "package");
      this.stckContent = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckContent");
      this.stckShelfOrderList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfOrderList");
      this.txtSearch = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtSearch");
      this.imgSearch = NameScopeExtensions.FindByName<ImageButton>((Element) this, "imgSearch");
      this.stckEmptyMessage = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckEmptyMessage");
      this.lstShelfOrder = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfOrder");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.stckPackage = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckPackage");
      this.txtPackageBarcode = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtPackageBarcode");
      this.btnCekiListesiPrint = NameScopeExtensions.FindByName<Button>((Element) this, "btnCekiListesiPrint");
      this.btnCreateNewPackage = NameScopeExtensions.FindByName<Button>((Element) this, "btnCreateNewPackage");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.txtBarcode = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtBarcode");
      this.txtQty = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtQty");
      this.pckBarcodeType = NameScopeExtensions.FindByName<Picker>((Element) this, "pckBarcodeType");
      this.lstShelfDetail = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfDetail");
      this.lblListHeader = NameScopeExtensions.FindByName<Label>((Element) this, "lblListHeader");
    }
  }
}
