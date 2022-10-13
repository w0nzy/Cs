// Decompiled with JetBrains decompiler
// Type: Shelf.Views.Picking
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Shelf.Controls;
using Shelf.Helpers;
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
  [XamlFilePath("Views\\Picking.xaml")]
  public class Picking : ContentPage
  {
    private List<pIOShelfOrderDetailFromIDReturnModel> shelfOrderDetail;
    private pIOUserShelfOrdersReturnModel selectedShelfOrder;
    private string selectedShelfCode;
    private bool isQtyFixed;
    private bool pickAndSort;
    private int lastShelfOrderDetailID;
    private string lastCustomerName;
    private List<ztIOShelfPackageType> packageTypeList;
    private int selectedShelfOrderDetailID;
    private string selectPackageCustomerName = "";
    private ztIOShelfPackageHeader newPackageHeader;
    private List<pIOGetPackageDetailFromShelfOrderIDReturnModel> packageDetails;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage picking;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckContent;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelfOrderList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckEmptyMessage;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfOrder;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtShelfOrderNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckShelfType;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckMainShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtMainShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnShelfBack;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnShelfNext;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtShelfBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckPackageNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtPackageNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckRulot;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtRulot;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtQty;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckBarcodeType;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckPivotShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtPivotShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnCreateNewPackage;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnPickOrder;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnShelfOrderSuccess;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfDetail;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Label lblListHeader;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ActivityIndicator loadingScreen;

    public ztIOShelf selectRulot { get; set; }

    public Color ButtonColor => Color.FromRgb(3, 10, 53);

    public Color TextColor => Color.White;

    public Picking()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Raf Emirleri";
      GlobalMob.AddBarcodeLongPress(this.txtBarcode);
      ((ICollection<Effect>) ((Element) this.btnShelfNext).Effects).Add((Effect) new LongPressedEffect());
      ((ICollection<Effect>) ((Element) this.btnShelfBack).Effects).Add((Effect) new LongPressedEffect());
      ((ICollection<Effect>) ((Element) this.txtShelfBarcode).Effects).Add((Effect) new LongPressedEffect());
      ((ICollection<Effect>) ((Element) this.txtQty).Effects).Add((Effect) new LongPressedEffect());
      ((ICollection<Effect>) ((Element) this.txtPackageNumber).Effects).Add((Effect) new LongPressedEffect());
      ((ICollection<Effect>) ((Element) this.btnCreateNewPackage).Effects).Add((Effect) new LongPressedEffect());
      ((ICollection<Effect>) ((Element) this.txtPivotShelf).Effects).Add((Effect) new LongPressedEffect());
      LongPressedEffect.SetCommand((BindableObject) this.btnShelfNext, this.LongPress);
      LongPressedEffect.SetCommand((BindableObject) this.btnShelfBack, this.LongPress);
      LongPressedEffect.SetCommand((BindableObject) this.txtShelfBarcode, this.LongPressShelfBarcode);
      LongPressedEffect.SetCommand((BindableObject) this.txtQty, this.LongPressQty);
      LongPressedEffect.SetCommand((BindableObject) this.txtPackageNumber, this.LongPressPackage);
      LongPressedEffect.SetCommand((BindableObject) this.btnCreateNewPackage, this.LongPressPivotShelf);
      LongPressedEffect.SetCommand((BindableObject) this.txtPivotShelf, this.LongPressTxtPivotShelf);
      ToolbarItem toolbarItem1 = new ToolbarItem();
      ((MenuItem) toolbarItem1).Text = "";
      ToolbarItem toolbarItem2 = toolbarItem1;
      ((MenuItem) toolbarItem2).Clicked += new EventHandler(this.TItem_Clicked);
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(toolbarItem2);
      this.pickAndSort = GlobalMob.User.PickAndSort;
      ((VisualElement) this.txtQty).IsVisible = !GlobalMob.User.HideQty;
      if (this.pickAndSort)
      {
        ((VisualElement) this.pckShelfType).IsVisible = true;
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
      }
      GlobalMob.FillBarcodeType(this.pckBarcodeType);
    }

    private async void TRefresh_Clicked(object sender, EventArgs e)
    {
      Picking picking = this;
      if (string.IsNullOrEmpty(((InputView) picking.txtShelfBarcode).Text))
      {
        int num = await ((Page) picking).DisplayAlert("Hata", "Lütfen önce raf okutunuz", "", "Tamam") ? 1 : 0;
      }
      else
      {
        await NavigationExtension.PushPopupAsync(((NavigableElement) picking).Navigation, GlobalMob.ShowLoading(), true);
        picking.RefreshDetailList(false);
        ((VisualElement) picking.txtBarcode).Focus();
        GlobalMob.CloseLoading();
      }
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

    private void TItem_Clicked(object sender, EventArgs e)
    {
      double num1 = this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode)).Sum<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.PickingQty));
      double num2 = this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode)).Sum<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.OrderQty));
      string str1 = "Raf : " + this.selectedShelfCode;
      string str2 = "Toplanan Miktar : " + num1.ToString() + "\n" + "Raftaki Toplam Miktar : " + num2.ToString() + "\n";
      Decimal num3 = 0M;
      double num4 = this.shelfOrderDetail.Sum<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.OrderQty));
      string str3 = str2 + "Raf Emri Toplam Miktar : " + num4.ToString() + "\n";
      if (num1 > 0.0)
        num3 = Convert.ToDecimal(num1 * 100.0 / num4);
      string str4 = str3 + "Raf Emri Toplama Yüzdesi : %" + num3.ToString("n2");
      ((Page) this).DisplayAlert(str1, str4, "", "Tamam");
    }

    private void SetInfo()
    {
      double num1 = this.shelfOrderDetail.Sum<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.PickingQty));
      double num2 = this.shelfOrderDetail.Sum<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.OrderQty));
      ((MenuItem) ((Page) this).ToolbarItems[0]).Text = num1.ToString() + "/" + num2.ToString();
    }

    private ICommand LongPressTxtPivotShelf => (ICommand) new Command((Action) (async () =>
    {
      if (this.selectedShelfOrder.ShelfOrderType != (byte) 2 && this.selectedShelfOrder.ShelfOrderType != (byte) 3)
        return;
      ((VisualElement) this.pckShelfType).Focus();
    }));

    private ICommand LongPressPivotShelf => (ICommand) new Command((Action) (async () =>
    {
      Picking page = this;
      ReturnModel returnModel = GlobalMob.PostJson("GetPackageHeaders?shelfOrderID=" + page.shelfOrderDetail[0].ShelfOrderID.ToString(), (ContentPage) page);
      if (!returnModel.Success)
        return;
      List<pIOGetPackageHeadersReturnModel> headersReturnModelList = GlobalMob.JsonDeserialize<List<pIOGetPackageHeadersReturnModel>>(returnModel.Result);
      ListView shelfListview = GlobalMob.GetShelfListview("PackageCode,CurrAccCode", new List<CustomMenuItemParameter>()
      {
        new CustomMenuItemParameter()
        {
          Text = "Çeki Listesi",
          ClickedEvent = new EventHandler(page.MnPackagePrint_Clicked)
        }
      });
      ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) headersReturnModelList;
      SelectItem selectItem = new SelectItem(shelfListview, "Müşteri Seçiniz");
      await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
    }));

    private void LstCustomerPackage_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      ((NavigableElement) this).Navigation.PopAsync();
      this.GetPackageList((pIOShelfOrderCustomerReturnModel) e.SelectedItem);
    }

    private async void GetPackageList(pIOShelfOrderCustomerReturnModel customer)
    {
      Picking picking = this;
      List<PackageHeader> source = new List<PackageHeader>();
      foreach (pIOShelfOrderDetailFromIDReturnModel fromIdReturnModel in picking.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x =>
      {
        if (!(x.CurrAccCode == customer.CurrAccCode))
          return false;
        if (!x.SubCurrAccID.HasValue && !customer.SubCurrAccID.HasValue)
          return true;
        Guid? subCurrAccId1 = x.SubCurrAccID;
        Guid? subCurrAccId2 = customer.SubCurrAccID;
        if (subCurrAccId1.HasValue != subCurrAccId2.HasValue)
          return false;
        return !subCurrAccId1.HasValue || subCurrAccId1.GetValueOrDefault() == subCurrAccId2.GetValueOrDefault();
      })))
      {
        if (!string.IsNullOrEmpty(fromIdReturnModel.PackageHeaderIds))
        {
          foreach (PackageHeader headerId in picking.GetHeaderIds(fromIdReturnModel))
          {
            PackageHeader pItem = headerId;
            if (source.Where<PackageHeader>((Func<PackageHeader, bool>) (x => x.PackageHeaderID == pItem.PackageHeaderID)).Count<PackageHeader>() <= 0)
              source.Add(pItem);
          }
        }
      }
      ListView shelfListview = GlobalMob.GetShelfListview("PackageCode");
      ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) source;
      shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(picking.lstPackage_ItemSelected);
      SelectItem selectItem = new SelectItem(shelfListview, "Tanımlı Koliler");
      await ((NavigableElement) picking).Navigation.PushAsync((Page) selectItem);
    }

    private async void MnPackagePrint_Clicked(object sender, EventArgs e)
    {
      Picking page = this;
      pIOGetPackageHeadersReturnModel item = (pIOGetPackageHeadersReturnModel) (sender as MenuItem).CommandParameter;
      await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
      int num = item.FileName.Contains(".repx") ? 2 : 1;
      List<BLReport> repList = new List<BLReport>();
      BLReport blReport = new BLReport()
      {
        ReportTypeID = 2,
        UserID = GlobalMob.User.UserID,
        FileType = num,
        PrinterBrandID = Convert.ToInt32((object) item.PrinterBrandID),
        NetworkPrinter = item.NetworkPrinter
      };
      blReport.ParamList = new List<BLReportParam>();
      blReport.ParamList.Add(new BLReportParam()
      {
        ParamName = "PackageCode",
        ParamValue = item.PackageCode,
        ParamType = 20
      });
      repList.Add(blReport);
      GlobalMob.BLPrint(repList, (object) item, (ContentPage) page);
      GlobalMob.CloseLoading();
      item = (pIOGetPackageHeadersReturnModel) null;
    }

    private async void lstPackage_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      Picking page1 = this;
      Page page2 = await ((NavigableElement) page1).Navigation.PopAsync();
      PackageHeader selectedItem = (PackageHeader) e.SelectedItem;
      ReturnModel returnModel = GlobalMob.PostJson("GetShelfPackageDetail?packageHeaderID=" + selectedItem.PackageHeaderID.ToString(), (ContentPage) page1);
      if (!returnModel.Success)
        return;
      List<pIOShelfPackageDetailFromHeaderIDReturnModel> headerIdReturnModelList = GlobalMob.JsonDeserialize<List<pIOShelfPackageDetailFromHeaderIDReturnModel>>(returnModel.Result);
      ListView listviewWithGrid = GlobalMob.GetShelfListviewWithGrid("ItemDescription,ItemCode,ColorDescription,ItemDim1Code,Qty,Barcode");
      ((ItemsView<Cell>) listviewWithGrid).ItemsSource = (IEnumerable) headerIdReturnModelList;
      SelectItem selectItem = new SelectItem(listviewWithGrid, "Koli Detayları-" + selectedItem.PackageCode);
      await ((NavigableElement) page1).Navigation.PushAsync((Page) selectItem);
    }

    private ICommand LongPressPackage => (ICommand) new Command((Action) (async () =>
    {
      Picking picking = this;
      IEnumerable<pIOShelfOrderDetailFromIDReturnModel> fromIdReturnModels = picking.shelfOrderDetail.GroupBy(c => new
      {
        ShelfCode = c.ShelfCode,
        PackageBarcode = c.PackageBarcode
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType12<string, string>, pIOShelfOrderDetailFromIDReturnModel>, pIOShelfOrderDetailFromIDReturnModel>(gcs => new pIOShelfOrderDetailFromIDReturnModel()
      {
        ShelfCode = gcs.Key.ShelfCode,
        PackageBarcode = gcs.Key.PackageBarcode
      });
      ListView shelfListview = GlobalMob.GetShelfListview("ShelfCode,PackageBarcode");
      ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) fromIdReturnModels;
      SelectItem selectItem = new SelectItem(shelfListview, "Ürünlerin Alınacağı Koliler");
      await ((NavigableElement) picking).Navigation.PushAsync((Page) selectItem);
    }));

    private ICommand LongPressQty => (ICommand) new Command((Action) (async () =>
    {
      Picking picking = this;
      bool flag = await ((Page) picking).DisplayAlert("Devam?", "Miktarı sabitlemek istiyor musunuz?", "Evet", "Hayır");
      picking.isQtyFixed = flag;
      ((VisualElement) picking.txtQty).BackgroundColor = picking.isQtyFixed ? Color.LightGray : Color.White;
    }));

    private ICommand LongPressShelfBarcode => (ICommand) new Command((Action) (async () =>
    {
      Picking picking = this;
      IOrderedEnumerable<pIOShelfOrderDetailFromIDReturnModel> orderedEnumerable = picking.shelfOrderDetail.GroupBy(c => new
      {
        ShelfCode = c.ShelfCode,
        HallCode = c.HallCode,
        FloorCode = c.FloorCode,
        MainShelfCode = c.MainShelfCode,
        SortOrder = c.SortOrder,
        SubSortOrder = c.SubSortOrder
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType13<string, string, string, string, int?, double>, pIOShelfOrderDetailFromIDReturnModel>, pIOShelfOrderDetailFromIDReturnModel>(gcs => new pIOShelfOrderDetailFromIDReturnModel()
      {
        ShelfCode = gcs.Key.ShelfCode,
        MainShelfCode = gcs.Key.MainShelfCode,
        HallCode = gcs.Key.HallCode,
        FloorCode = gcs.Key.FloorCode,
        OrderQty = gcs.Sum<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.OrderQty)),
        PickingQty = gcs.Sum<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.PickingQty)),
        SubSortOrder = gcs.Key.SubSortOrder
      }).OrderBy<pIOShelfOrderDetailFromIDReturnModel, double>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.SubSortOrder));
      ListView shelfListview = GlobalMob.GetShelfListview("ShelfCode,HallCode,FloorCode");
      ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) orderedEnumerable;
      shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(picking.SelectShelfBarcode_SelectedItem);
      SelectItem selectItem = new SelectItem(shelfListview, "Raf Seçiniz");
      await ((NavigableElement) picking).Navigation.PushAsync((Page) selectItem);
    }));

    private async void SelectShelfBarcode_SelectedItem(object sender, EventArgs e)
    {
      pIOShelfOrderDetailFromIDReturnModel selectedItem = (pIOShelfOrderDetailFromIDReturnModel) ((ListView) sender).SelectedItem;
      if (selectedItem == null)
        return;
      this.SelectShelf(Convert.ToString(selectedItem.ShelfCode), selectedItem.MainShelfCode);
    }

    private async void SelectShelf(string shelfCode, string mainShelfCode)
    {
      Picking picking = this;
      picking.SetMainShelf(mainShelfCode);
      if (GlobalMob.User.ShelfChangeBlock)
      {
        ((InputView) picking.txtShelf).Text = shelfCode;
        ((InputView) picking.txtShelfBarcode).Text = "";
        ((VisualElement) picking.txtShelfBarcode).Focus();
        Page page = await ((NavigableElement) picking).Navigation.PopAsync();
      }
      else
      {
        ((InputView) picking.txtShelfBarcode).Text = shelfCode;
        Page page = await ((NavigableElement) picking).Navigation.PopAsync();
        ((IEntryController) picking.txtShelfBarcode).SendCompleted();
      }
    }

    private void SetMainShelf(string mainShelfCode)
    {
      ((InputView) this.txtMainShelf).Text = mainShelfCode;
      ((VisualElement) this.stckMainShelf).IsVisible = !string.IsNullOrEmpty(mainShelfCode);
    }

    private ICommand LongPress => (ICommand) new Command((Action) (async () =>
    {
      Picking picking = this;
      IEnumerable<pIOShelfOrderDetailFromIDReturnModel> fromIdReturnModels = picking.shelfOrderDetail.GroupBy(c => new
      {
        ShelfCode = c.ShelfCode,
        HallCode = c.HallCode,
        FloorCode = c.FloorCode
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType14<string, string, string>, pIOShelfOrderDetailFromIDReturnModel>, pIOShelfOrderDetailFromIDReturnModel>(gcs => new pIOShelfOrderDetailFromIDReturnModel()
      {
        ShelfCode = gcs.Key.ShelfCode,
        HallCode = gcs.Key.HallCode,
        FloorCode = gcs.Key.FloorCode,
        OrderQty = gcs.Sum<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.OrderQty)),
        PickingQty = gcs.Sum<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.PickingQty))
      });
      ListView shelfListview = GlobalMob.GetShelfListview("ShelfCode,HallCode,FloorCode");
      ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) fromIdReturnModels;
      shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(picking.Select_SelectedItem);
      SelectItem selectItem = new SelectItem(shelfListview, "Raf Seçiniz");
      await ((NavigableElement) picking).Navigation.PushAsync((Page) selectItem);
    }));

    private async void Select_SelectedItem(object sender, EventArgs e)
    {
      Picking picking = this;
      pIOShelfOrderDetailFromIDReturnModel selectedItem = (pIOShelfOrderDetailFromIDReturnModel) ((ListView) sender).SelectedItem;
      if (selectedItem == null)
        ;
      else
      {
        string shelfCode = Convert.ToString(selectedItem.ShelfCode);
        if (picking.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode && x.OrderQty - x.PickingQty > 0.0)).ToList<pIOShelfOrderDetailFromIDReturnModel>().Count > 0 && picking.selectedShelfCode != ((InputView) picking.txtShelfBarcode).Text)
        {
          if (!await ((Page) picking).DisplayAlert("Devam?", "Rafdaki ürünler tamamlanmadı.Devam etmek istiyor musunuz?", "Evet", "Hayır"))
          {
            Page page = await ((NavigableElement) picking).Navigation.PopAsync();
            return;
          }
        }
        if (picking.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>() != null)
        {
          pIOShelfOrderDetailFromIDReturnModel fromIdReturnModel = picking.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == shelfCode)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>();
          ((InputView) picking.txtShelf).Text = fromIdReturnModel.ShelfCode;
          picking.SetMainShelf(fromIdReturnModel.MainShelfCode);
          picking.selectedShelfCode = ((InputView) picking.txtShelf).Text;
          ((InputView) picking.txtShelfBarcode).Text = "";
          ((VisualElement) picking.txtShelfBarcode).Focus();
        }
        picking.NextBackButtonEnabled();
        Page page1 = await ((NavigableElement) picking).Navigation.PopAsync();
      }
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetUserShelfOrders?userID={0}", (object) GlobalMob.User.UserID), (ContentPage) this);
      if (returnModel.Success)
      {
        List<pIOUserShelfOrdersReturnModel> ordersReturnModelList = GlobalMob.JsonDeserialize<List<pIOUserShelfOrdersReturnModel>>(returnModel.Result);
        ((VisualElement) this.lstShelfOrder).IsVisible = ordersReturnModelList.Count > 0;
        ((VisualElement) this.stckEmptyMessage).IsVisible = ordersReturnModelList.Count == 0;
        if (((VisualElement) this.stckEmptyMessage).IsVisible)
          ((View) this.stckContent).VerticalOptions = LayoutOptions.Center;
        ((BindableObject) this.lstShelfOrder).BindingContext = (object) ordersReturnModelList;
      }
      this.lstShelfOrder.ItemSelected += (EventHandler<SelectedItemChangedEventArgs>) ((sender, e) =>
      {
        object selectedItem = ((ListView) sender).SelectedItem;
        if (selectedItem == null)
          return;
        pIOUserShelfOrdersReturnModel ordersReturnModel = (pIOUserShelfOrdersReturnModel) selectedItem;
        ((Page) this).Title = ordersReturnModel.ShelfOrderNumber + "-Ürün Toplama";
        this.selectedShelfOrder = ordersReturnModel;
        this.pickAndSort = false;
        if (ordersReturnModel.ShelfOrderType == (byte) 7)
        {
          this.txtPivotShelf.FontSize = Device.GetNamedSize((NamedSize) 3, typeof (Xamarin.Forms.Entry));
          ((InputView) this.txtPivotShelf).Placeholder = "Ürünün Bırakılacağı Koli";
          ((VisualElement) this.btnCreateNewPackage).IsVisible = GlobalMob.User.IsCreatePackageOnShelfOrderPlan;
        }
        if (GlobalMob.User.PickAndSort && ordersReturnModel.ShelfOrderType == (byte) 3 || Convert.ToBoolean((object) ordersReturnModel.PickAndSort))
          this.pickAndSort = true;
        bool flag = false;
        if ((ordersReturnModel.ShelfOrderType == (byte) 2 || ordersReturnModel.ShelfOrderType == (byte) 3) && this.pickAndSort)
          flag = true;
        ((VisualElement) this.pckShelfType).IsVisible = flag;
        if (!string.IsNullOrEmpty(ordersReturnModel.ActiveShelfOrderNumber) && ordersReturnModel.ActiveShelfOrderNumber != ordersReturnModel.ShelfOrderNumber)
        {
          ((Page) this).DisplayAlert("Bilgi", string.Format("{0} numaralı raf emrini toplamaya başladınız,lütfen bu emir ile devam ediniz", (object) ordersReturnModel.ActiveShelfOrderNumber), "", "Tamam");
          this.lstShelfOrder.SelectedItem = (object) null;
        }
        else
        {
          ((InputView) this.txtShelfOrderNumber).Text = "";
          ((InputView) this.txtShelfOrderNumber).Text = ordersReturnModel.ShelfOrderNumber.Replace("S", "");
          ((VisualElement) this.stckShelfOrderList).IsVisible = false;
          ((VisualElement) this.stckForm).IsVisible = true;
          ((VisualElement) this.stckPackageNumber).IsVisible = GlobalMob.User.IsPackage;
          ToolbarItem toolbarItem1 = new ToolbarItem();
          ((MenuItem) toolbarItem1).Text = "";
          ((MenuItem) toolbarItem1).Icon = FileImageSource.op_Implicit("refresh.png");
          ToolbarItem toolbarItem2 = toolbarItem1;
          ((MenuItem) toolbarItem2).Clicked += new EventHandler(this.TRefresh_Clicked);
          ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(toolbarItem2);
          this.GetShelfDetail();
          if (this.pickAndSort && ((VisualElement) this.pckShelfType).IsVisible)
            ((VisualElement) this.pckShelfType).Focus();
          else
            Device.BeginInvokeOnMainThread((Action) (async () =>
            {
              await Task.Delay(250);
              ((VisualElement) this.txtShelfBarcode)?.Focus();
            }));
        }
      });
    }

    private void SetEmptyForNullBarcode()
    {
      this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.Barcode == null)).Select<pIOShelfOrderDetailFromIDReturnModel, pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, pIOShelfOrderDetailFromIDReturnModel>) (c =>
      {
        c.Barcode = "";
        return c;
      })).ToList<pIOShelfOrderDetailFromIDReturnModel>();
      this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.UsedBarcode == null)).Select<pIOShelfOrderDetailFromIDReturnModel, pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, pIOShelfOrderDetailFromIDReturnModel>) (c =>
      {
        c.UsedBarcode = "";
        return c;
      })).ToList<pIOShelfOrderDetailFromIDReturnModel>();
    }

    private void GetShelfDetail(
      pIOShelfOrderDetailFromIDReturnModel lastSelectedShelf = null)
    {
      this.shelfOrderDetail = new List<pIOShelfOrderDetailFromIDReturnModel>();
      Device.BeginInvokeOnMainThread((Action) (() =>
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfOrderDetailFromID?shelfOrderID={0}&userID={1}", (object) this.selectedShelfOrder.ShelfOrderID, (object) GlobalMob.User.UserID), (ContentPage) this);
        if (returnModel.Success)
        {
          this.shelfOrderDetail = GlobalMob.JsonDeserialize<List<pIOShelfOrderDetailFromIDReturnModel>>(returnModel.Result);
          this.SetEmptyForNullBarcode();
          this.SetSubSortOrder();
          ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
          if (this.shelfOrderDetail.Count > 0)
          {
            int shelfOrderType = (int) this.shelfOrderDetail[0].ShelfOrderType;
            if (!GlobalMob.User.IsUniqueBarcode)
            {
              bool flag = GlobalMob.User.IsBarcodeType;
              if (shelfOrderType == 2 || shelfOrderType == 3)
                flag = false;
              ((VisualElement) this.pckBarcodeType).IsVisible = flag;
              if (((VisualElement) this.pckBarcodeType).IsVisible && !string.IsNullOrEmpty(Settings.PickingBarcodeType))
                this.pckBarcodeType.SelectedIndex = Convert.ToInt32(Settings.PickingBarcodeType) - 1;
            }
            ((InputView) this.txtShelf).Text = this.shelfOrderDetail[0].ShelfCode;
            this.SetMainShelf(this.shelfOrderDetail[0].MainShelfCode);
            this.selectedShelfCode = ((InputView) this.txtShelf).Text;
            this.SetPackageNumber();
            ((InputView) this.txtShelfBarcode).Text = "";
            ((VisualElement) this.stckShelf).IsVisible = true;
            this.NextBackButtonEnabled();
            this.SetInfo();
            if (lastSelectedShelf != null)
            {
              pIOShelfOrderDetailFromIDReturnModel fromIdReturnModel = ((this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == lastSelectedShelf.ShelfCode && x.OrderQty - x.PickingQty > 0.0)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>() ?? this.shelfOrderDetail.OrderBy<pIOShelfOrderDetailFromIDReturnModel, double>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.SubSortOrder)).Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.SubSortOrder >= lastSelectedShelf.SubSortOrder && x.OrderQty - x.PickingQty > 0.0)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>()) ?? this.shelfOrderDetail.OrderBy<pIOShelfOrderDetailFromIDReturnModel, double>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.SubSortOrder)).Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.SubSortOrder < lastSelectedShelf.SubSortOrder && x.OrderQty - x.PickingQty > 0.0)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>()) ?? this.shelfOrderDetail[0];
              if (fromIdReturnModel != null)
                this.SelectShelf(fromIdReturnModel.ShelfCode, fromIdReturnModel.MainShelfCode);
            }
            ((VisualElement) this.txtShelfBarcode).Focus();
          }
          else
          {
            ((VisualElement) this.btnShelfOrderSuccess).IsVisible = true;
            ((VisualElement) this.stckPivotShelf).IsVisible = this.pickAndSort;
          }
        }
        else
          ((Page) this).DisplayAlert("Hata", returnModel.ErrorMessage, "", "Tamam");
      }));
    }

    private void SetSubSortOrder()
    {
      this.shelfOrderDetail = this.shelfOrderDetail.Select<pIOShelfOrderDetailFromIDReturnModel, pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, pIOShelfOrderDetailFromIDReturnModel>) (c =>
      {
        c.SubSortOrder = Convert.ToDouble((object) c.SortOrder);
        return c;
      })).ToList<pIOShelfOrderDetailFromIDReturnModel>();
      foreach (IGrouping<int?, pIOShelfOrderDetailFromIDReturnModel> grouping1 in this.shelfOrderDetail.GroupBy<pIOShelfOrderDetailFromIDReturnModel, int?>((Func<pIOShelfOrderDetailFromIDReturnModel, int?>) (x => x.SortOrder)))
      {
        IGrouping<int?, pIOShelfOrderDetailFromIDReturnModel> item = grouping1;
        List<pIOShelfOrderDetailFromIDReturnModel> list = this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x =>
        {
          int? sortOrder = x.SortOrder;
          int? key = item.Key;
          return sortOrder.GetValueOrDefault() == key.GetValueOrDefault() & sortOrder.HasValue == key.HasValue;
        })).ToList<pIOShelfOrderDetailFromIDReturnModel>();
        if (list.GroupBy<pIOShelfOrderDetailFromIDReturnModel, string>((Func<pIOShelfOrderDetailFromIDReturnModel, string>) (x => x.ShelfCode)).Count<IGrouping<string, pIOShelfOrderDetailFromIDReturnModel>>() > 1)
        {
          double num = 0.1;
          foreach (IGrouping<string, pIOShelfOrderDetailFromIDReturnModel> grouping2 in list.GroupBy<pIOShelfOrderDetailFromIDReturnModel, string>((Func<pIOShelfOrderDetailFromIDReturnModel, string>) (x => x.ShelfCode)))
          {
            IGrouping<string, pIOShelfOrderDetailFromIDReturnModel> sItem = grouping2;
            foreach (pIOShelfOrderDetailFromIDReturnModel fromIdReturnModel in list.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == sItem.Key)).ToList<pIOShelfOrderDetailFromIDReturnModel>())
              fromIdReturnModel.SubSortOrder += num;
            num += 0.1;
          }
        }
      }
    }

    private void FillListView()
    {
      List<pIOShelfOrderDetailFromIDReturnModel> list = this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == ((InputView) this.txtShelfBarcode).Text)).ToList<pIOShelfOrderDetailFromIDReturnModel>();
      if (GlobalMob.User.IsPackage && !string.IsNullOrEmpty(((InputView) this.txtPackageNumber).Text))
        list = list.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.PackageBarcode == ((InputView) this.txtPackageNumber).Text)).ToList<pIOShelfOrderDetailFromIDReturnModel>();
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
      this.lblListHeader.Text = "";
      this.selectedShelfCode = ((InputView) this.txtShelfBarcode).Text;
      ((InputView) this.txtShelf).Text = this.selectedShelfCode;
      this.SetPackageNumber();
      if (list.Count > 0)
      {
        ((VisualElement) this.lstShelfDetail).IsVisible = true;
        ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) list;
        this.lblListHeader.Text = "Raf Adı : " + ((InputView) this.txtShelf).Text;
      }
      else
        ((InputView) this.txtShelfBarcode).Text = "";
    }

    private void SetPackageNumber()
    {
      if (!GlobalMob.User.IsPackage)
        return;
      ((InputView) this.txtPackageNumber).Placeholder = string.Join(",", this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == ((InputView) this.txtShelf).Text && x.OrderQty != x.PickingQty)).OrderBy<pIOShelfOrderDetailFromIDReturnModel, string>((Func<pIOShelfOrderDetailFromIDReturnModel, string>) (x => x.PackageBarcode)).ToList<pIOShelfOrderDetailFromIDReturnModel>().Select<pIOShelfOrderDetailFromIDReturnModel, string>((Func<pIOShelfOrderDetailFromIDReturnModel, string>) (m => m.PackageBarcode)).ToArray<string>());
    }

    private async void GetBarcode()
    {
      Picking page = this;
      if (GlobalMob.User.IsRulot && page.selectRulot == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Öncelikle rulot okutunuz", "", "Tamam") ? 1 : 0;
        ((InputView) page.txtRulot).Text = "";
        ((VisualElement) page.txtRulot).Focus();
      }
      if (string.IsNullOrEmpty(((InputView) page.txtShelfBarcode).Text))
      {
        GlobalMob.PlayError();
        ((InputView) page.txtBarcode).Text = "";
        int num = await ((Page) page).DisplayAlert("Bilgi", "Lütfen öncelikle rafı okutunuz", "", "Tamam") ? 1 : 0;
        ((VisualElement) page.txtShelfBarcode).Focus();
      }
      else if (page.pickAndSort && page.pckShelfType.SelectedItem == null && (page.selectedShelfOrder.ShelfOrderType == (byte) 2 || page.selectedShelfOrder.ShelfOrderType == (byte) 3))
      {
        GlobalMob.PlayError();
        ((InputView) page.txtBarcode).Text = "";
        int num = await ((Page) page).DisplayAlert("Bilgi", "Lütfen dağıtım rafı seçiniz", "", "Tamam") ? 1 : 0;
        ((VisualElement) page.pckShelfType).Focus();
      }
      else
      {
        string barcode = ((InputView) page.txtBarcode).Text;
        if (string.IsNullOrEmpty(barcode))
          ;
        else if (barcode.Length < GlobalMob.User.MinimumBarcodeLength)
        {
          GlobalMob.PlayError();
          page.BarcodeFocus();
        }
        else if (page.selectedShelfCode == barcode && page.selectedShelfOrder.IsReadShelfCode)
        {
          page.PickAllShelf();
        }
        else
        {
          pIOShelfOrderDetailFromIDReturnModel item = page.GetItem(barcode);
          bool isLot = false;
          bool isUnique = false;
          bool flag = false;
          PickerItem selectedItem1 = (PickerItem) page.pckBarcodeType.SelectedItem;
          if (selectedItem1 != null && selectedItem1.Code == 2 && ((VisualElement) page.pckBarcodeType).IsVisible)
            isLot = true;
          if (selectedItem1 != null && selectedItem1.Code == 3 && ((VisualElement) page.pckBarcodeType).IsVisible)
            isUnique = true;
          if (selectedItem1 != null && selectedItem1.Code == 1 && ((VisualElement) page.pckBarcodeType).IsVisible)
            flag = true;
          if (isUnique && page.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.UsedBarcode == barcode && x.PickingQty > 0.0)).Count<pIOShelfOrderDetailFromIDReturnModel>() > 0)
          {
            GlobalMob.PlayError();
            int num = await ((Page) page).DisplayAlert("Bilgi", "Bu ürün daha önce okutuldu." + barcode, "", "Tamam") ? 1 : 0;
            page.BarcodeFocus();
          }
          else
          {
            if (isLot | isUnique && item == null)
              item = !isLot ? page.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode && x.PickingQty != x.OrderQty)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>() : page.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode && x.LotBarcode == barcode && x.PickingQty != x.OrderQty)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>();
            if (item != null)
            {
              int qty = page.GetQty();
              string newPackageHeaderIDs = "";
              int id = item.ShelfOrderDetailID;
              if (qty > 0)
              {
                if (((!GlobalMob.User.IsUniqueBarcode ? 0 : (item.UseSerialNumber ? 1 : 0)) & (flag ? 1 : 0)) != 0)
                {
                  GlobalMob.PlayError();
                  int num = await ((Page) page).DisplayAlert("Bilgi", "Lütfen bu ürün için ean yerine seri numarası okutunuz", "", "Tamam") ? 1 : 0;
                  page.BarcodeFocus();
                  return;
                }
                if (GlobalMob.User.IsRulot && page.selectRulot != null)
                {
                  int shelfId = page.selectRulot.ShelfID;
                }
                if (Convert.ToBoolean((object) page.selectedShelfOrder.PickPackageApprove))
                {
                  if (string.IsNullOrEmpty(item.PackageHeaderIds))
                  {
                    GlobalMob.PlayError();
                    int num = await ((Page) page).DisplayAlert("Bilgi", "Tanımlı koli bulunamadı", "", "Tamam") ? 1 : 0;
                    page.BarcodeFocus();
                    return;
                  }
                  List<PackageHeader> packageList = page.GetHeaderIds(item, qty);
                  string packageCodes = string.Join(",", packageList.Select<PackageHeader, string>((Func<PackageHeader, string>) (y => y.PackageCode.ToString() + "(" + y.PackageDescription + ")")).ToArray<string>());
                  string packageCode = await GlobalMob.InputBarcode(((NavigableElement) page).Navigation, "Koli Numaraları : " + packageCodes, "Koliyi Okutunuz.", Keyboard.Chat);
                  if (packageList == null || packageList.Where<PackageHeader>((Func<PackageHeader, bool>) (x => x.PackageCode == packageCode)).Count<PackageHeader>() <= 0)
                  {
                    if (!string.IsNullOrEmpty(packageCode))
                    {
                      GlobalMob.PlayError();
                      string str = "Yanlış koliyi okuttunuz.\nKoli Numaraları:" + packageCodes + "-Okutulan Koli:" + packageCode;
                      int num = await ((Page) page).DisplayAlert("Bilgi", str, "", "Tamam") ? 1 : 0;
                    }
                    page.BarcodeFocus();
                    return;
                  }
                  PackageHeader packageHeader = packageList.Where<PackageHeader>((Func<PackageHeader, bool>) (x => x.PackageCode == packageCode)).FirstOrDefault<PackageHeader>();
                  item.PackageHeaderID = new int?(packageHeader.PackageHeaderID);
                  item.PackageCode = packageHeader.PackageCode;
                  newPackageHeaderIDs = page.GetNewPackageHeaderQty(item, qty);
                  packageList = (List<PackageHeader>) null;
                  packageCodes = (string) null;
                }
                int num1 = 0;
                PickerItem selectedItem2 = (PickerItem) page.pckShelfType.SelectedItem;
                if (selectedItem2 != null)
                  num1 = selectedItem2.Code;
                PickAndSort pick = new PickAndSort()
                {
                  ShelfOrderDetailID = id,
                  PickQty = qty,
                  barcode = item.Barcode,
                  readbarcode = barcode,
                  userName = GlobalMob.User.UserName,
                  DispOrderNumber = item.DispOrderNumber,
                  ShelfID = item.ShelfID,
                  ShelfOrderID = item.ShelfOrderID,
                  ShelfOrderType = (int) item.ShelfOrderType,
                  PackageHeaderID = Convert.ToInt32((object) item.PackageHeaderID),
                  PackageDetailID = Convert.ToInt32((object) item.PackageDetailID),
                  ShelfCurrAccTypeCode = Convert.ToInt32((object) item.ShelfCurrAccTypeCode),
                  ShelfType = num1,
                  DispOrderLineID = item.DispOrderLineID,
                  isPickAndSort = page.pickAndSort,
                  AutoApproveQty = item.AutoApproveQty,
                  AutoPackedQty = item.AutoPackedQty,
                  PickPackageApprove = Convert.ToBoolean((object) page.selectedShelfOrder.PickPackageApprove)
                };
                Dictionary<string, string> paramList = new Dictionary<string, string>();
                ReturnModel returnModel = new ReturnModel();
                if (GlobalMob.User.ShowLoading)
                  await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
                ReturnModel result;
                if (isLot)
                {
                  List<pIOShelfOrderDetailFromIDReturnModel> list = page.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x =>
                  {
                    if (x.DispOrderNumber == item.DispOrderNumber)
                    {
                      int? orderLineSumId1 = x.OrderLineSumID;
                      int? orderLineSumId2 = item.OrderLineSumID;
                      if (orderLineSumId1.GetValueOrDefault() == orderLineSumId2.GetValueOrDefault() & orderLineSumId1.HasValue == orderLineSumId2.HasValue && x.LotBarcode == barcode)
                        return x.ShelfCode == this.selectedShelfCode;
                    }
                    return false;
                  })).ToList<pIOShelfOrderDetailFromIDReturnModel>();
                  pick.Detail = new List<PickAndSortDetail>();
                  foreach (pIOShelfOrderDetailFromIDReturnModel fromIdReturnModel in list)
                    pick.Detail.Add(new PickAndSortDetail()
                    {
                      Barcode = fromIdReturnModel.Barcode,
                      ColorCode = fromIdReturnModel.ColorCode,
                      DispOrderNumber = fromIdReturnModel.DispOrderNumber,
                      ItemCode = fromIdReturnModel.ItemCode,
                      ItemDim1Code = fromIdReturnModel.ItemDim1Code,
                      ItemDim2Code = fromIdReturnModel.ItemDim2Code,
                      OrderQty = fromIdReturnModel.OrderQty,
                      PickingQty = fromIdReturnModel.PickingQty,
                      ShelfOrderDetailID = fromIdReturnModel.ShelfOrderDetailID,
                      ShelfOrderID = fromIdReturnModel.ShelfOrderID
                    });
                  pick.barcode = ((InputView) page.txtBarcode).Text;
                  paramList.Add("json", JsonConvert.SerializeObject((object) pick));
                  result = GlobalMob.PostJson("PickAndSortLot", paramList, (ContentPage) page).Result;
                }
                else if (isUnique)
                {
                  pick.barcode = barcode;
                  paramList.Add("json", JsonConvert.SerializeObject((object) pick));
                  result = GlobalMob.PostJson("PickAndSortUnique", paramList, (ContentPage) page).Result;
                }
                else
                {
                  paramList.Add("json", JsonConvert.SerializeObject((object) pick));
                  result = GlobalMob.PostJson("PickAndSort", paramList, (ContentPage) page).Result;
                }
                if (GlobalMob.User.ShowLoading)
                  GlobalMob.CloseLoading();
                if (result.Success)
                {
                  List<PickAndSort> pickAndSortList = (List<PickAndSort>) null;
                  PickAndSort pickAndSort1;
                  if (isLot)
                  {
                    pickAndSortList = JsonConvert.DeserializeObject<List<PickAndSort>>(result.Result);
                    if (pickAndSortList == null)
                    {
                      GlobalMob.PlayError();
                      int num2 = await ((Page) page).DisplayAlert("Bilgi", "Lot için yeterli miktar yok", "", "Tamam") ? 1 : 0;
                      page.BarcodeFocus();
                      return;
                    }
                    if (pickAndSortList.Count > 0)
                    {
                      pickAndSort1 = pickAndSortList[0];
                    }
                    else
                    {
                      GlobalMob.PlayError();
                      int num3 = await ((Page) page).DisplayAlert("Bilgi", "Hatalı Lot Barkodu : " + barcode, "", "Tamam") ? 1 : 0;
                      page.BarcodeFocus();
                      return;
                    }
                  }
                  else
                    pickAndSort1 = JsonConvert.DeserializeObject<PickAndSort>(result.Result);
                  if (page.pickAndSort && (pickAndSort1 == null || string.IsNullOrEmpty(pickAndSort1.PivotShelfCode)))
                  {
                    GlobalMob.PlayError();
                    string str = pick.ShelfOrderType != 7 ? "Dağıtım rafı bulunamadı" : "Koli bulunamadı";
                    int num4 = await ((Page) page).DisplayAlert("Bilgi", str, "", "Tamam") ? 1 : 0;
                    page.BarcodeFocus();
                    return;
                  }
                  int int32 = Convert.ToInt32(pickAndSort1.ResultQty);
                  if (int32 > 0)
                  {
                    if (pick.ShelfOrderType == 7)
                    {
                      item.PackageDetailID = new int?(pickAndSort1.PackageDetailID);
                      if (!string.IsNullOrEmpty(item.PackageCode))
                      {
                        page.lastShelfOrderDetailID = Convert.ToInt32(item.ShelfOrderDetailID);
                        page.lastCustomerName = pickAndSort1.PivotShelfCode;
                        pickAndSort1.PivotShelfCode = item.PackageCode + "-" + pickAndSort1.PivotShelfCode;
                      }
                    }
                    if (page.pickAndSort)
                      ((InputView) page.txtPivotShelf).Text = pickAndSort1.PivotShelfCode;
                    page.shelfOrderDetail.Select<pIOShelfOrderDetailFromIDReturnModel, pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, pIOShelfOrderDetailFromIDReturnModel>) (c =>
                    {
                      c.LastReadBarcode = false;
                      return c;
                    })).ToList<pIOShelfOrderDetailFromIDReturnModel>();
                    if (isLot)
                    {
                      foreach (PickAndSort pickAndSort2 in pickAndSortList)
                      {
                        PickAndSort pickItem = pickAndSort2;
                        pIOShelfOrderDetailFromIDReturnModel fromIdReturnModel = page.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfOrderDetailID == pickItem.ShelfOrderDetailID)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>();
                        if (fromIdReturnModel != null)
                        {
                          fromIdReturnModel.LastReadBarcode = true;
                          fromIdReturnModel.PickingQty += (double) pickItem.PickQty;
                        }
                      }
                    }
                    else
                    {
                      item.LastReadBarcode = true;
                      item.PickingQty += (double) int32;
                      item.UsedBarcode = barcode;
                    }
                    if (!string.IsNullOrEmpty(item.PackageHeaderIds) && !string.IsNullOrEmpty(newPackageHeaderIDs))
                      item.PackageHeaderIds = newPackageHeaderIDs;
                    page.shelfOrderDetail = page.shelfOrderDetail.OrderByDescending<pIOShelfOrderDetailFromIDReturnModel, bool>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.LastReadBarcode)).ThenByDescending<pIOShelfOrderDetailFromIDReturnModel, double>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.OrderQty - x.PickingQty)).ToList<pIOShelfOrderDetailFromIDReturnModel>();
                    page.SetInfo();
                    page.FillListView();
                    if (!page.isQtyFixed)
                      ((InputView) page.txtQty).Text = "1";
                    ((InputView) page.txtBarcode).Text = "";
                    List<pIOShelfOrderDetailFromIDReturnModel> list1 = page.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode && x.OrderQty - x.PickingQty > 0.0)).ToList<pIOShelfOrderDetailFromIDReturnModel>();
                    if (list1 != null && list1.Count <= 0)
                    {
                      pIOShelfOrderDetailFromIDReturnModel itemShelf = page.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>();
                      if (page.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.SubSortOrder > itemShelf.SubSortOrder)).Any<pIOShelfOrderDetailFromIDReturnModel>())
                      {
                        GlobalMob.PlaySave();
                        ((ItemsView<Cell>) page.lstShelfDetail).ItemsSource = (IEnumerable) null;
                        page.lblListHeader.Text = "";
                        page.lastCustomerName = "";
                        page.lastShelfOrderDetailID = 0;
                        page.btnShelfNext_Clicked((object) null, (EventArgs) null);
                        return;
                      }
                    }
                    else if (GlobalMob.User.IsPackage)
                    {
                      List<pIOShelfOrderDetailFromIDReturnModel> list2 = page.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode && x.PackageBarcode == ((InputView) this.txtPackageNumber).Text && x.OrderQty - x.PickingQty > 0.0)).ToList<pIOShelfOrderDetailFromIDReturnModel>();
                      if (list2 != null && list2.Count <= 0)
                      {
                        Device.BeginInvokeOnMainThread((Action) (async () =>
                        {
                          GlobalMob.PlaySave();
                          await Task.Delay(250);
                          ((InputView) this.txtPackageNumber).Text = "";
                          ((VisualElement) this.txtPackageNumber)?.Focus();
                        }));
                        return;
                      }
                    }
                    GlobalMob.PlaySave();
                    page.BarcodeFocus();
                  }
                  else
                  {
                    switch (int32)
                    {
                      case -2:
                        GlobalMob.PlayError();
                        int num5 = await ((Page) page).DisplayAlert("Hata", pickAndSort1.ErrorMessage, "", "Tamam") ? 1 : 0;
                        page.BarcodeFocus();
                        break;
                      case -1:
                        GlobalMob.PlayError();
                        int num6 = await ((Page) page).DisplayAlert("Bilgi", "Hata Oluştu", "", "Tamam") ? 1 : 0;
                        page.BarcodeFocus();
                        break;
                      default:
                        GlobalMob.PlayError();
                        int num7 = await ((Page) page).DisplayAlert("Bilgi", "Miktar Yetersiz", "", "Tamam") ? 1 : 0;
                        page.BarcodeFocus();
                        break;
                    }
                  }
                }
                else
                {
                  int num8 = await ((Page) page).DisplayAlert("Hata", result.ErrorMessage, "", "Tamam") ? 1 : 0;
                  GlobalMob.PlayError();
                  page.BarcodeFocus();
                }
                pick = (PickAndSort) null;
                paramList = (Dictionary<string, string>) null;
              }
              newPackageHeaderIDs = (string) null;
            }
            else
            {
              int num9 = page.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.Barcode.Contains(barcode) && x.ShelfCode == this.selectedShelfCode && x.PickingQty == x.OrderQty)).Any<pIOShelfOrderDetailFromIDReturnModel>() ? 1 : 0;
              GlobalMob.PlayError();
              string str = num9 != 0 ? "Ürün miktarı tamamlandı" : "Ürün bulunamadı";
              int num10 = await ((Page) page).DisplayAlert("Bilgi", str, "", "Tamam") ? 1 : 0;
              ((InputView) page.txtBarcode).Text = "";
              ((VisualElement) page.txtBarcode).Focus();
            }
          }
        }
      }
    }

    private async void PickAllShelf()
    {
      Picking page = this;
      await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
      PickAndSort pickAndSort = new PickAndSort();
      // ISSUE: reference to a compiler-generated method
      List<pIOShelfOrderDetailFromIDReturnModel> list = page.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>(new Func<pIOShelfOrderDetailFromIDReturnModel, bool>(page.\u003CPickAllShelf\u003Eb__48_0)).ToList<pIOShelfOrderDetailFromIDReturnModel>();
      pIOShelfOrderDetailFromIDReturnModel fromIdReturnModel1 = list[0];
      pickAndSort.ShelfID = fromIdReturnModel1.ShelfID;
      pickAndSort.ShelfOrderID = fromIdReturnModel1.ShelfOrderID;
      pickAndSort.UserID = GlobalMob.User.UserID;
      pickAndSort.userName = GlobalMob.User.UserName;
      pickAndSort.isPickAndSort = page.pickAndSort;
      pickAndSort.AutoApproveQty = fromIdReturnModel1.AutoApproveQty;
      pickAndSort.AutoPackedQty = fromIdReturnModel1.AutoPackedQty;
      pickAndSort.PickPackageApprove = Convert.ToBoolean((object) page.selectedShelfOrder.PickPackageApprove);
      ReturnModel result = GlobalMob.PostJson("PickAndSortAllShelf", new Dictionary<string, string>()
      {
        {
          "json",
          JsonConvert.SerializeObject((object) pickAndSort)
        }
      }, (ContentPage) page).Result;
      if (result.Success)
      {
        ReturnModel returnModel = JsonConvert.DeserializeObject<ReturnModel>(result.Result);
        if (returnModel.Success)
        {
          foreach (pIOShelfOrderDetailFromIDReturnModel fromIdReturnModel2 in list)
          {
            fromIdReturnModel2.LastReadBarcode = true;
            fromIdReturnModel2.PickingQty = fromIdReturnModel2.OrderQty;
          }
          GlobalMob.PlaySave();
          ((ItemsView<Cell>) page.lstShelfDetail).ItemsSource = (IEnumerable) null;
          page.lblListHeader.Text = "";
          page.lastCustomerName = "";
          ((InputView) page.txtBarcode).Text = "";
          page.lastShelfOrderDetailID = 0;
          page.btnShelfNext_Clicked((object) null, (EventArgs) null);
        }
        else
        {
          GlobalMob.PlayError();
          int num = await ((Page) page).DisplayAlert("Hata", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
          page.BarcodeFocus();
        }
      }
      GlobalMob.CloseLoading();
    }

    private string GetNewPackageHeaderQty(pIOShelfOrderDetailFromIDReturnModel item, int qty)
    {
      string packageHeaderQty = "";
      if (!string.IsNullOrEmpty(item.PackageHeaderIds))
      {
        string packageHeaderIds = item.PackageHeaderIds;
        char[] chArray = new char[1]{ ',' };
        foreach (string str in packageHeaderIds.Split(chArray))
        {
          if (!string.IsNullOrEmpty(str) && str.Contains(";"))
          {
            int int32_1 = Convert.ToInt32(str.Split(';')[0]);
            int int32_2 = Convert.ToInt32(str.Split(';')[1]);
            int? packageHeaderId = item.PackageHeaderID;
            int num = int32_1;
            if (packageHeaderId.GetValueOrDefault() == num & packageHeaderId.HasValue)
              packageHeaderQty = packageHeaderQty + "," + int32_1.ToString() + ";" + Convert.ToString(int32_2 - qty);
            else
              packageHeaderQty = packageHeaderQty + "," + int32_1.ToString() + ";" + int32_2.ToString();
          }
        }
      }
      return packageHeaderQty;
    }

    private void BarcodeFocus() => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(250);
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode)?.Focus();
    }));

    private List<PackageHeader> GetHeaderIds(
      pIOShelfOrderDetailFromIDReturnModel item,
      int qty = -1)
    {
      List<PackageHeader> headerIds = new List<PackageHeader>();
      if (!string.IsNullOrEmpty(item.PackageHeaderIds))
      {
        string packageHeaderIds = item.PackageHeaderIds;
        char[] chArray = new char[1]{ ',' };
        foreach (string str1 in packageHeaderIds.Split(chArray))
        {
          if (!string.IsNullOrEmpty(str1))
          {
            if (str1.Contains(";") && qty != -1)
            {
              string str2 = "";
              int int32_1 = Convert.ToInt32(str1.Split(';')[0]);
              int int32_2 = Convert.ToInt32(str1.Split(';')[1]);
              if (str1.Split(';').Length > 2)
                str2 = Convert.ToString(str1.Split(';')[2]);
              if (int32_2 >= qty)
                headerIds.Add(new PackageHeader()
                {
                  PackageHeaderID = int32_1,
                  Qty = int32_2,
                  PackageDescription = str2
                });
            }
            else
              headerIds.Add(new PackageHeader()
              {
                PackageHeaderID = Convert.ToInt32(str1)
              });
          }
        }
      }
      return headerIds;
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

    private pIOShelfOrderDetailFromIDReturnModel GetItem(
      string barcode)
    {
      List<pIOShelfOrderDetailFromIDReturnModel> source = this.shelfOrderDetail;
      if (GlobalMob.User.IsPackage && !string.IsNullOrEmpty(((InputView) this.txtPackageNumber).Text))
        source = source.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.PackageBarcode == ((InputView) this.txtPackageNumber).Text)).ToList<pIOShelfOrderDetailFromIDReturnModel>();
      return !GlobalMob.User.BarcodeSearchEqual ? source.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.Barcode.Contains(barcode) && x.ShelfCode == this.selectedShelfCode && x.PickingQty != x.OrderQty)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>() : source.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.Barcode.Replace(",", "").Trim() == barcode && x.ShelfCode == this.selectedShelfCode && x.PickingQty != x.OrderQty)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>();
    }

    private async void btnShelf_Clicked(object sender, EventArgs e)
    {
      Picking picking = this;
      // ISSUE: reference to a compiler-generated method
      if (picking.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>(new Func<pIOShelfOrderDetailFromIDReturnModel, bool>(picking.\u003CbtnShelf_Clicked\u003Eb__54_0)).Count<pIOShelfOrderDetailFromIDReturnModel>() > 0)
      {
        // ISSUE: reference to a compiler-generated method
        if (picking.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>(new Func<pIOShelfOrderDetailFromIDReturnModel, bool>(picking.\u003CbtnShelf_Clicked\u003Eb__54_1)).ToList<pIOShelfOrderDetailFromIDReturnModel>().Count > 0 && picking.selectedShelfCode != ((InputView) picking.txtShelfBarcode).Text)
        {
          if (!await ((Page) picking).DisplayAlert("Devam?", "Rafdaki ürünler tamamlanmadı.Devam etmek istiyor musunuz?", "Evet", "Hayır"))
            return;
          picking.FillListView();
        }
        else
          picking.FillListView();
      }
      else
      {
        int num = await ((Page) picking).DisplayAlert("Bilgi", "Raf bulunamadı", "", "Tamam") ? 1 : 0;
        ((InputView) picking.txtShelf).Text = "";
        ((VisualElement) picking.txtShelf).Focus();
      }
    }

    private async void btnShelfNext_Clicked(object sender, EventArgs e)
    {
      Picking picking = this;
      if (picking.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode && x.OrderQty - x.PickingQty > 0.0)).ToList<pIOShelfOrderDetailFromIDReturnModel>().Count > 0 && picking.selectedShelfCode != ((InputView) picking.txtShelfBarcode).Text)
      {
        if (!await ((Page) picking).DisplayAlert("Devam?", "Rafdaki ürünler tamamlanmadı.Diğer rafa geçmek istiyor musunuz?", "Evet", "Hayır"))
          return;
      }
      picking.RefreshDetailList();
      pIOShelfOrderDetailFromIDReturnModel item = picking.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>();
      if (item != null)
      {
        pIOShelfOrderDetailFromIDReturnModel fromIdReturnModel = picking.shelfOrderDetail.OrderBy<pIOShelfOrderDetailFromIDReturnModel, double>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.SubSortOrder)).Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.SubSortOrder > item.SubSortOrder)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>();
        if (fromIdReturnModel != null)
        {
          ((InputView) picking.txtShelf).Text = fromIdReturnModel.ShelfCode;
          picking.SetMainShelf(fromIdReturnModel.MainShelfCode);
          if (GlobalMob.User.IsUniqueBarcode)
            picking.SetBarcodeType();
          picking.selectedShelfCode = ((InputView) picking.txtShelf).Text;
          ((InputView) picking.txtPackageNumber).Text = "";
          ((InputView) picking.txtShelfBarcode).Text = "";
          ((VisualElement) picking.txtShelfBarcode).Focus();
        }
      }
      picking.NextBackButtonEnabled();
    }

    private void SetBarcodeType() => this.pckBarcodeType.SelectedIndex = this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode && x.PickingQty < x.OrderQty && x.UseSerialNumber)).Sum<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.OrderQty - x.PickingQty)) > this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode && x.PickingQty < x.OrderQty && !x.UseSerialNumber)).Sum<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.OrderQty - x.PickingQty)) ? 2 : 0;

    private void RefreshDetailList(bool isDateControl = true)
    {
      if (isDateControl)
      {
        DateTime? updatedDate = this.selectedShelfOrder.UpdatedDate;
        DateTime minValue = DateTime.MinValue;
        if ((updatedDate.HasValue ? (updatedDate.HasValue ? (updatedDate.GetValueOrDefault() == minValue ? 1 : 0) : 1) : 0) != 0)
          return;
      }
      string str = Convert.ToDateTime((object) this.selectedShelfOrder.UpdatedDate).ToString("dd.MM.yyyy HH:mm");
      if (!isDateControl)
        str = Convert.ToDateTime((object) this.selectedShelfOrder.UpdatedDate).AddDays(-10.0).ToString("dd.MM.yyyy HH:mm");
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfOrderDetailFromIDChange?shelfOrderID={0}&lastDate={1}", (object) this.selectedShelfOrder.ShelfOrderID, (object) str), (ContentPage) this);
      if (!returnModel.Success)
        return;
      List<pIOGetAllShelfOrderDetailReturnModel> source = GlobalMob.JsonDeserialize<List<pIOGetAllShelfOrderDetailReturnModel>>(returnModel.Result);
      if (source == null || source.Count<pIOGetAllShelfOrderDetailReturnModel>() <= 0)
        return;
      if (isDateControl)
        this.selectedShelfOrder.UpdatedDate = new DateTime?(DateTime.Now);
      foreach (pIOShelfOrderDetailFromIDReturnModel fromIdReturnModel in this.shelfOrderDetail)
      {
        pIOShelfOrderDetailFromIDReturnModel det = fromIdReturnModel;
        pIOGetAllShelfOrderDetailReturnModel detailReturnModel = source.Where<pIOGetAllShelfOrderDetailReturnModel>((Func<pIOGetAllShelfOrderDetailReturnModel, bool>) (x => x.ShelfOrderDetailID == det.ShelfOrderDetailID)).FirstOrDefault<pIOGetAllShelfOrderDetailReturnModel>();
        if (detailReturnModel != null)
        {
          det.ShelfID = detailReturnModel.ShelfID;
          det.WarehouseCode = detailReturnModel.WarehouseCode;
          det.ShelfCode = detailReturnModel.ShelfCode;
          det.ShelfName = detailReturnModel.ShelfName;
          det.PickingQty = detailReturnModel.PickingQty;
        }
      }
      if (isDateControl)
        return;
      this.FillListView();
    }

    private void btnShelfBack_Clicked(object sender, EventArgs e)
    {
      pIOShelfOrderDetailFromIDReturnModel item = this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>();
      if (item != null)
      {
        pIOShelfOrderDetailFromIDReturnModel fromIdReturnModel = this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.SubSortOrder < item.SubSortOrder)).OrderByDescending<pIOShelfOrderDetailFromIDReturnModel, double>((Func<pIOShelfOrderDetailFromIDReturnModel, double>) (x => x.SubSortOrder)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>();
        ((InputView) this.txtShelf).Text = fromIdReturnModel.ShelfCode;
        this.SetMainShelf(fromIdReturnModel.MainShelfCode);
        this.selectedShelfCode = ((InputView) this.txtShelf).Text;
      }
      this.NextBackButtonEnabled();
    }

    private void NextBackButtonEnabled()
    {
      pIOShelfOrderDetailFromIDReturnModel item = this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>();
      ((VisualElement) this.btnShelfBack).IsEnabled = this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.SubSortOrder < item.SubSortOrder)).Any<pIOShelfOrderDetailFromIDReturnModel>();
      ((VisualElement) this.btnShelfNext).IsEnabled = this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.SubSortOrder > item.SubSortOrder)).Any<pIOShelfOrderDetailFromIDReturnModel>();
    }

    private async void txtShelfBarcode_Completed(object sender, EventArgs e)
    {
      Picking picking = this;
      string text = ((InputView) picking.txtShelfBarcode).Text;
      if (string.IsNullOrEmpty(text))
        return;
      if (picking.selectedShelfCode != text)
      {
        // ISSUE: reference to a compiler-generated method
        pIOShelfOrderDetailFromIDReturnModel fromIdReturnModel = picking.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>(new Func<pIOShelfOrderDetailFromIDReturnModel, bool>(picking.\u003CtxtShelfBarcode_Completed\u003Eb__60_1)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>();
        if (fromIdReturnModel != null)
        {
          ((InputView) picking.txtShelfBarcode).Text = fromIdReturnModel.ShelfCode;
          ((InputView) picking.txtPivotShelf).Text = "";
          ((VisualElement) picking.stckBarcode).IsVisible = true;
          ((VisualElement) picking.stckPivotShelf).IsVisible = picking.pickAndSort;
          ((VisualElement) picking.btnShelfOrderSuccess).IsVisible = true;
          picking.FillListView();
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          List<pIOShelfOrderDetailFromIDReturnModel> list1 = picking.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>(new Func<pIOShelfOrderDetailFromIDReturnModel, bool>(picking.\u003CtxtShelfBarcode_Completed\u003Eb__60_2)).ToList<pIOShelfOrderDetailFromIDReturnModel>();
          if (list1.Count > 0 && (list1[0].MainShelfDescription == ((InputView) picking.txtMainShelf).Text || list1[0].MainShelfCode == ((InputView) picking.txtMainShelf).Text))
          {
            List<pIOShelfOrderDetailFromIDReturnModel> list2 = list1.GroupBy(c => new
            {
              ShelfCode = c.ShelfCode
            }).Select<IGrouping<\u003C\u003Ef__AnonymousType15<string>, pIOShelfOrderDetailFromIDReturnModel>, pIOShelfOrderDetailFromIDReturnModel>(gcs => new pIOShelfOrderDetailFromIDReturnModel()
            {
              ShelfCode = gcs.Key.ShelfCode
            }).ToList<pIOShelfOrderDetailFromIDReturnModel>();
            if (list2.Count == 1)
            {
              ((InputView) picking.txtShelfBarcode).Text = list2[0].ShelfCode;
              ((IEntryController) picking.txtShelfBarcode).SendCompleted();
            }
            else
            {
              ((InputView) picking.txtShelfBarcode).Text = "";
              ListView shelfListview = GlobalMob.GetShelfListview("ShelfCode");
              ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) list2;
              shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(picking.LstSubShelf_ItemSelected);
              SelectItem selectItem = new SelectItem(shelfListview, "Koli Seçiniz-" + ((InputView) picking.txtShelfBarcode).Text);
              await ((NavigableElement) picking).Navigation.PushAsync((Page) selectItem);
            }
          }
          else
          {
            int num = await ((Page) picking).DisplayAlert("Bilgi", "Hatalı Raf Kodu", "", "Tamam") ? 1 : 0;
            ((InputView) picking.txtShelfBarcode).Text = "";
            ((VisualElement) picking.txtShelfBarcode).Focus();
            return;
          }
        }
      }
      else
      {
        ((VisualElement) picking.stckRulot).IsVisible = GlobalMob.User.IsRulot;
        ((VisualElement) picking.stckBarcode).IsVisible = true;
        ((VisualElement) picking.btnShelfOrderSuccess).IsVisible = true;
        ((VisualElement) picking.stckPivotShelf).IsVisible = picking.pickAndSort;
        picking.FillListView();
      }
      // ISSUE: reference to a compiler-generated method
      Device.BeginInvokeOnMainThread(new Action(picking.\u003CtxtShelfBarcode_Completed\u003Eb__60_0));
    }

    private void LstSubShelf_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      ((InputView) this.txtShelfBarcode).Text = ((pIOShelfOrderDetailFromIDReturnModel) e.SelectedItem).ShelfCode;
      ((NavigableElement) this).Navigation.PopAsync();
      ((IEntryController) this.txtShelfBarcode).SendCompleted();
    }

    private void txtBarcode_Completed(object sender, EventArgs e) => this.GetBarcode();

    private async void btnShelfOrderSuccess_Clicked(object sender, EventArgs e)
    {
      Picking page1 = this;
      if (!string.IsNullOrEmpty(((InputView) page1.txtShelfOrderNumber).Text))
      {
        if (Convert.ToBoolean((object) page1.selectedShelfOrder.IsOrderCompletionPassword))
        {
          string str = await GlobalMob.InputBox(((NavigableElement) page1).Navigation, "Yönetici Şifresi", "Lütfen Yönetici Şifresini Giriniz.", Keyboard.Chat);
          if (str != GlobalMob.User.OperationPermitPassword)
          {
            if (string.IsNullOrEmpty(str))
              return;
            GlobalMob.PlayError();
            int num = await ((Page) page1).DisplayAlert("Hata", "Yanlış Şifre", "", "Tamam") ? 1 : 0;
            return;
          }
        }
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("ShelfOrderCompleted?shelfOrderNumber={0}&isCompleted=false&userID={1}", (object) ((InputView) page1.txtShelfOrderNumber).Text, (object) GlobalMob.User.UserID), (ContentPage) page1);
        if (!returnModel.Success)
          return;
        if (!string.IsNullOrEmpty(JsonConvert.DeserializeObject<string>(returnModel.Result)))
        {
          if (await ((Page) page1).DisplayAlert("Devam?", "Toplanmayan ürünler mevcut.Yine de emri tamamlamak istiyor musunuz?", "Evet", "Hayır"))
          {
            if (GlobalMob.User.MissingCompleted)
            {
              string str = await GlobalMob.InputBox(((NavigableElement) page1).Navigation, "Yönetici Şifresi", "Lütfen Yönetici Şifresini Giriniz.", Keyboard.Chat);
              if (str != GlobalMob.User.OperationPermitPassword)
              {
                if (string.IsNullOrEmpty(str))
                  return;
                GlobalMob.PlayError();
                int num = await ((Page) page1).DisplayAlert("Hata", "Yanlış Şifre", "", "Tamam") ? 1 : 0;
                return;
              }
            }
            if (!GlobalMob.PostJson(string.Format("ShelfOrderCompleted?shelfOrderNumber={0}&isCompleted=true&userID={1}", (object) ((InputView) page1.txtShelfOrderNumber).Text, (object) GlobalMob.User.UserID), (ContentPage) page1).Success)
              return;
            int num1 = await ((Page) page1).DisplayAlert("Bilgi", "Raf Emri Tamamlandı", "", "Tamam") ? 1 : 0;
            Page page2 = await ((NavigableElement) page1).Navigation.PopAsync();
          }
          else
            page1.GetShelfDetail();
        }
        else
        {
          if (!GlobalMob.PostJson(string.Format("ShelfOrderCompleted?shelfOrderNumber={0}&isCompleted=true&userID={1}", (object) ((InputView) page1.txtShelfOrderNumber).Text, (object) GlobalMob.User.UserID), (ContentPage) page1).Success)
            return;
          int num = await ((Page) page1).DisplayAlert("Bilgi", "Raf Emri Tamamlandı", "", "Tamam") ? 1 : 0;
          Page page3 = await ((NavigableElement) page1).Navigation.PopAsync();
        }
      }
      else
      {
        int num2 = await ((Page) page1).DisplayAlert("Bilgi", "Lütfen raf emri seçiniz", "", "Tamam") ? 1 : 0;
      }
    }

    private async void CmShelfChange_Clicked(object sender, EventArgs e)
    {
      Picking page = this;
      await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
      pIOShelfOrderDetailFromIDReturnModel commandParameter = (pIOShelfOrderDetailFromIDReturnModel) (sender as MenuItem).CommandParameter;
      page.selectedShelfOrderDetailID = commandParameter.ShelfOrderDetailID;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format((string.IsNullOrEmpty(commandParameter.LotBarcode) ? "GetShelfInventoryForShelfChange" : "GetShelfLotInventoryForShelfChange") + "?barcode={0}&qty={1}&shelfOrderID={2}", (object) (string.IsNullOrEmpty(commandParameter.LotBarcode) ? commandParameter.Barcode : commandParameter.LotBarcode), (object) (string.IsNullOrEmpty(commandParameter.LotBarcode) ? Convert.ToInt32(commandParameter.OrderQty - commandParameter.PickingQty) : 1), (object) commandParameter.ShelfOrderID), (ContentPage) page);
      if (returnModel.Success)
      {
        List<pIOShelfInventoryForShelfChangeReturnModel> source = GlobalMob.JsonDeserialize<List<pIOShelfInventoryForShelfChangeReturnModel>>(returnModel.Result);
        ListView shelfListview = GlobalMob.GetShelfListview("ShelfCode,AvailableInventoryQty,Description");
        // ISSUE: reference to a compiler-generated method
        ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) source.Where<pIOShelfInventoryForShelfChangeReturnModel>(new Func<pIOShelfInventoryForShelfChangeReturnModel, bool>(page.\u003CCmShelfChange_Clicked\u003Eb__65_0)).ToList<pIOShelfInventoryForShelfChangeReturnModel>();
        shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(page.LstShelfChange_ItemSelected);
        string title = "Raf Seçiniz";
        if (!string.IsNullOrEmpty(commandParameter.LotCode))
          title = title + "-" + commandParameter.LotCode;
        SelectItem selectItem = new SelectItem(shelfListview, title);
        await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
      }
      GlobalMob.CloseLoading();
    }

    private void LstShelfChange_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      pIOShelfInventoryForShelfChangeReturnModel selectedItem = (pIOShelfInventoryForShelfChangeReturnModel) e.SelectedItem;
      pIOShelfOrderDetailFromIDReturnModel item = this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfOrderDetailID == this.selectedShelfOrderDetailID)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>();
      string url = string.Format("UpdateShelfOrderDetail?shelfOrderDetailID={0}&shelfID={1}&userName={2}", (object) this.selectedShelfOrderDetailID, (object) selectedItem.ShelfID, (object) GlobalMob.User.UserName);
      if (!string.IsNullOrEmpty(item.LotBarcode))
        url = string.Format("UpdateShelfOrderDetailLot?shelfOrderDetailIDs={0}&shelfID={1}&userName={2}", (object) string.Join<int>(",", (IEnumerable<int>) this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x =>
        {
          if (x.DispOrderNumber == item.DispOrderNumber)
          {
            int? orderLineSumId1 = x.OrderLineSumID;
            int? orderLineSumId2 = item.OrderLineSumID;
            if (orderLineSumId1.GetValueOrDefault() == orderLineSumId2.GetValueOrDefault() & orderLineSumId1.HasValue == orderLineSumId2.HasValue && x.LotBarcode == item.LotBarcode)
            {
              orderLineSumId2 = x.OrderLineSumID;
              int num = 0;
              return orderLineSumId2.GetValueOrDefault() > num & orderLineSumId2.HasValue;
            }
          }
          return false;
        })).ToList<pIOShelfOrderDetailFromIDReturnModel>().Select<pIOShelfOrderDetailFromIDReturnModel, int>((Func<pIOShelfOrderDetailFromIDReturnModel, int>) (m => m.ShelfOrderDetailID)).Distinct<int>().ToArray<int>()), (object) selectedItem.ShelfID, (object) GlobalMob.User.UserName);
      ReturnModel returnModel = GlobalMob.PostJson(url, (ContentPage) this);
      if (returnModel.Success)
      {
        this.GetShelfDetail(this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>());
        this.lblListHeader.Text = "";
      }
      ((Page) this).DisplayAlert("Bilgi", returnModel.Success ? "Raf Güncellendi" : "Raf Güncellenemedi", "", "Tamam");
    }

    private async void txtRulot_Completed(object sender, EventArgs e)
    {
      Picking page = this;
      if (string.IsNullOrEmpty(((InputView) page.txtRulot).Text))
        return;
      ReturnModel shelf = GlobalMob.GetShelf(((InputView) page.txtRulot).Text, (ContentPage) page);
      if (!shelf.Success)
        return;
      page.selectRulot = JsonConvert.DeserializeObject<ztIOShelf>(shelf.Result);
      if (page.selectRulot != null)
      {
        if (page.selectRulot.ShelfType != (byte) 100)
        {
          int num = await ((Page) page).DisplayAlert("Bilgi", "Okutulan Barkot Rulot Değildir", "", "Tamam") ? 1 : 0;
          page.RulotBarcodeFocus();
        }
        else
        {
          ((InputView) page.txtRulot).Text = page.selectRulot.Description;
          ((VisualElement) page.txtBarcode).Focus();
        }
      }
      else
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Rulot Bulunamadı", "", "Tamam") ? 1 : 0;
        page.RulotBarcodeFocus();
      }
    }

    private void RulotBarcodeFocus()
    {
      ((InputView) this.txtRulot).Text = "";
      ((VisualElement) this.txtRulot).Focus();
    }

    private void cmImage_Clicked(object sender, EventArgs e)
    {
      pIOShelfOrderDetailFromIDReturnModel commandParameter = (pIOShelfOrderDetailFromIDReturnModel) (sender as MenuItem).CommandParameter;
      ((NavigableElement) this).Navigation.PushAsync((Page) new ImagePage(commandParameter.Url, commandParameter.ItemDescription));
    }

    private void pckShelfType_SelectedIndexChanged(object sender, EventArgs e)
    {
      ((InputView) this.txtPivotShelf).Text = "";
      if (this.selectedShelfOrder.ShelfOrderType == (byte) 2 || this.selectedShelfOrder.ShelfOrderType == (byte) 3)
      {
        PickerItem selectedItem = (PickerItem) this.pckShelfType.SelectedItem;
        if (selectedItem != null)
          ((InputView) this.txtPivotShelf).Placeholder = selectedItem.Caption;
      }
      if (string.IsNullOrEmpty(((InputView) this.txtShelfBarcode).Text))
        ((VisualElement) this.txtShelfBarcode).Focus();
      else
        ((VisualElement) this.txtBarcode).Focus();
    }

    private async void cmShelfOutput_Clicked(object sender, EventArgs e)
    {
      Picking page = this;
      pIOShelfOrderDetailFromIDReturnModel selectItem = (pIOShelfOrderDetailFromIDReturnModel) (sender as MenuItem).CommandParameter;
      if (selectItem.PickingQty == selectItem.OrderQty && selectItem.PickingQty > 0.0)
      {
        GlobalMob.PlayError();
        int num = await ((Page) page).DisplayAlert("Uyarı", "Ürün zaten toplanmış gözüküyor.Toplanmış ürünü raftan çıkış yapamazsınız", "", "Tamam") ? 1 : 0;
        selectItem = (pIOShelfOrderDetailFromIDReturnModel) null;
      }
      else if (await GlobalMob.InputBox(((NavigableElement) page).Navigation, "Yönetici Şifresi", "Lütfen Yönetici Şifresini Giriniz.", Keyboard.Chat) != GlobalMob.User.OperationPermitPassword)
      {
        GlobalMob.PlayError();
        int num = await ((Page) page).DisplayAlert("Hata", "Yanlış Şifre", "", "Tamam") ? 1 : 0;
        selectItem = (pIOShelfOrderDetailFromIDReturnModel) null;
      }
      else
      {
        string[] strArray = selectItem.Barcode.Split(new string[1]
        {
          ","
        }, StringSplitOptions.RemoveEmptyEntries);
        if (strArray.Length == 0)
        {
          GlobalMob.PlayError();
          int num = await ((Page) page).DisplayAlert("Hata", "Barkod Tanımlı Değil", "", "Tamam") ? 1 : 0;
          selectItem = (pIOShelfOrderDetailFromIDReturnModel) null;
        }
        else
        {
          string str1 = strArray[0];
          ShelfTransaction shelfTransaction = new ShelfTransaction();
          shelfTransaction.ShelfID = selectItem.ShelfID;
          shelfTransaction.ProcessTypeID = 2;
          shelfTransaction.WareHouseCode = selectItem.WarehouseCode;
          shelfTransaction.ShelfOrderDetailID = selectItem.ShelfOrderDetailID;
          shelfTransaction.Barcode = str1;
          shelfTransaction.UserName = GlobalMob.User.UserName;
          shelfTransaction.Qty = Convert.ToInt32(((InputView) page.txtQty).Text);
          shelfTransaction.TransTypeID = 11;
          shelfTransaction.DocumentNumber = "";
          ReturnModel result = GlobalMob.PostJson("PickShelfOutputIns", new Dictionary<string, string>()
          {
            {
              "json",
              JsonConvert.SerializeObject((object) shelfTransaction)
            }
          }, (ContentPage) page).Result;
          if (!result.Success)
          {
            selectItem = (pIOShelfOrderDetailFromIDReturnModel) null;
          }
          else
          {
            ztIOShelfTransactionDetail transactionDetail = JsonConvert.DeserializeObject<ztIOShelfTransactionDetail>(result.Result);
            if (transactionDetail != null)
            {
              if (transactionDetail.TransactionDetailID == -1)
              {
                GlobalMob.PlayError();
                Convert.ToInt32((object) transactionDetail.ShelfOrderDetailID);
                Convert.ToInt32((object) transactionDetail.Qty);
                string str2 = "Miktar Yetersiz" + "Çıkış Yapılmak İstenen Miktar:" + shelfTransaction.Qty.ToString() + "\nRaftaki Miktar:" + transactionDetail.ShelfOrderDetailID.ToString();
                int num = await ((Page) page).DisplayAlert("Bilgi", str2, "", "Tamam") ? 1 : 0;
                selectItem = (pIOShelfOrderDetailFromIDReturnModel) null;
              }
              else
              {
                GlobalMob.PlaySave();
                int num = await ((Page) page).DisplayAlert("Bilgi", "Ürünün raftan çıkışı yapıldı.", "", "Tamam") ? 1 : 0;
                selectItem = (pIOShelfOrderDetailFromIDReturnModel) null;
              }
            }
            else
            {
              GlobalMob.PlayError();
              int num = await ((Page) page).DisplayAlert("Bilgi", "Ürün Bulunamadı", "", "Tamam") ? 1 : 0;
              selectItem = (pIOShelfOrderDetailFromIDReturnModel) null;
            }
          }
        }
      }
    }

    private void pckBarcodeType_SelectedIndexChanged(object sender, EventArgs e)
    {
      PickerItem selectedItem = (PickerItem) this.pckBarcodeType.SelectedItem;
      if (selectedItem == null || !((VisualElement) this.pckBarcodeType).IsVisible)
        return;
      Settings.PickingBarcodeType = Convert.ToString(selectedItem.Code);
    }

    private async void btnCreateNewPackage_Clicked(object sender, EventArgs e)
    {
      Picking page = this;
      int? pickingTypeId = page.selectedShelfOrder.PickingTypeID;
      int num1 = 3;
      pIOShelfOrderDetailFromIDReturnModel findItem;
      if (pickingTypeId.GetValueOrDefault() == num1 & pickingTypeId.HasValue)
      {
        int num2 = await ((Page) page).DisplayAlert("Uyarı", "Oto koli yönteminde yeni koli oluşturamazsınız", "", "Tamam") ? 1 : 0;
        findItem = (pIOShelfOrderDetailFromIDReturnModel) null;
      }
      else if (!await ((Page) page).DisplayAlert(page.lastCustomerName, "Yeni koli oluşturulacak emin misiniz?", "Evet", "Hayır"))
      {
        findItem = (pIOShelfOrderDetailFromIDReturnModel) null;
      }
      else
      {
        findItem = (pIOShelfOrderDetailFromIDReturnModel) null;
        if (page.lastShelfOrderDetailID <= 0 || !string.IsNullOrEmpty(((InputView) page.txtPivotShelf).Text))
        {
          ReturnModel returnModel = GlobalMob.PostJson("GetShelfOrderCustomer?shelfOrderID=" + page.shelfOrderDetail[0].ShelfOrderID.ToString(), (ContentPage) page);
          if (returnModel.Success)
          {
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
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          findItem = page.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>(new Func<pIOShelfOrderDetailFromIDReturnModel, bool>(page.\u003CbtnCreateNewPackage_Clicked\u003Eb__73_0)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>();
        }
        if (findItem == null)
        {
          findItem = (pIOShelfOrderDetailFromIDReturnModel) null;
        }
        else
        {
          page.CreateNewPackage(findItem.ShelfOrderID, findItem.CurrAccCode, findItem.CurrAccTypeCode, findItem.SubCurrAccID, "");
          findItem = (pIOShelfOrderDetailFromIDReturnModel) null;
        }
      }
    }

    private async void CreateNewPackage(
      int shelfOrderID,
      string currAccCode,
      int currAccTypeCode,
      Guid? subcurrAccID,
      string customerName)
    {
      Picking picking = this;
      picking.selectPackageCustomerName = customerName;
      picking.newPackageHeader = new ztIOShelfPackageHeader()
      {
        CreatedDate = DateTime.Now,
        CreatedUserName = GlobalMob.User.UserName,
        UserID = new int?(GlobalMob.User.UserID),
        ShelfOrderID = shelfOrderID,
        UpdatedDate = new DateTime?(DateTime.Now),
        UpdatedUserName = GlobalMob.User.UserName,
        PackageDate = DateTime.Now,
        CurrAccCode = currAccCode,
        CurrAccTypeCode = new short?((short) Convert.ToSByte(currAccTypeCode)),
        SubCurrAccID = subcurrAccID,
        PackageTypeID = new int?(0)
      };
      if (GlobalMob.User.IsAskPackageType)
      {
        picking.GetPackageTypes();
        ListView shelfListview = GlobalMob.GetShelfListview("PackageType,PackageDesc");
        ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) picking.packageTypeList;
        shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(picking.LstPackageType_ItemSelected);
        SelectItem selectItem = new SelectItem(shelfListview, "Koli tipi seçiniz");
        await ((NavigableElement) picking).Navigation.PushAsync((Page) selectItem);
      }
      else
        picking.AddPackage(picking.newPackageHeader, customerName, false);
    }

    private void LstPackageType_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      ((NavigableElement) this).Navigation.PopAsync();
      this.newPackageHeader.PackageTypeID = new int?(((ztIOShelfPackageType) e.SelectedItem).PackageTypeID);
      this.AddPackage(this.newPackageHeader, this.selectPackageCustomerName, true);
      this.selectPackageCustomerName = "";
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
      if (shelfPackageHeader == null || shelfPackageHeader.PackageHeaderID <= 0)
        return;
      foreach (pIOShelfOrderDetailFromIDReturnModel fromIdReturnModel1 in this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x =>
      {
        if (x.PickingQty != x.OrderQty && x.CurrAccCode == header.CurrAccCode)
        {
          int currAccTypeCode1 = x.CurrAccTypeCode;
          short? currAccTypeCode2 = header.CurrAccTypeCode;
          int? nullable = currAccTypeCode2.HasValue ? new int?((int) currAccTypeCode2.GetValueOrDefault()) : new int?();
          int valueOrDefault = nullable.GetValueOrDefault();
          if (currAccTypeCode1 == valueOrDefault & nullable.HasValue)
          {
            Guid? subCurrAccId1 = x.SubCurrAccID;
            Guid? subCurrAccId2 = header.SubCurrAccID;
            if (subCurrAccId1.HasValue != subCurrAccId2.HasValue)
              return false;
            return !subCurrAccId1.HasValue || subCurrAccId1.GetValueOrDefault() == subCurrAccId2.GetValueOrDefault();
          }
        }
        return false;
      })).ToList<pIOShelfOrderDetailFromIDReturnModel>())
      {
        if (isPackageType)
        {
          pIOShelfOrderDetailFromIDReturnModel fromIdReturnModel2 = fromIdReturnModel1;
          fromIdReturnModel2.PackageHeaderIds = fromIdReturnModel2.PackageHeaderIds + "," + shelfPackageHeader.PackageHeaderID.ToString();
        }
        else
        {
          fromIdReturnModel1.PackageHeaderID = new int?(shelfPackageHeader.PackageHeaderID);
          fromIdReturnModel1.PackageCode = shelfPackageHeader.Description;
        }
      }
      if (isPackageType)
        ((Page) this).DisplayAlert("Yeni Koli", "Yeni Koli eklendi\nKoli No:" + shelfPackageHeader.Description, "", "Tamam");
      if (!string.IsNullOrEmpty(customerName))
        ((InputView) this.txtPivotShelf).Text = shelfPackageHeader.Description + "-" + customerName;
      else
        ((InputView) this.txtPivotShelf).Text = shelfPackageHeader.Description + "-" + this.lastCustomerName;
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode).Focus();
    }

    private void LstCustomer_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      pIOShelfOrderCustomerReturnModel selectedItem = (pIOShelfOrderCustomerReturnModel) e.SelectedItem;
      if (sender != null && !Convert.ToBoolean((object) this.selectedShelfOrder.PickPackageApprove))
        ((NavigableElement) this).Navigation.PopAsync();
      this.CreateNewPackage(this.shelfOrderDetail[0].ShelfOrderID, selectedItem.CurrAccCode, Convert.ToInt32((object) selectedItem.CurrAccTypeCode), selectedItem.SubCurrAccID, selectedItem.CurrAccDescription);
    }

    private async void cmBlacklist_Clicked(object sender, EventArgs e)
    {
      Picking page = this;
      if (!await ((Page) page).DisplayAlert("Devam?", "Ürünü kara listeye eklemek istiyor musunuz?", "Evet", "Hayır"))
        return;
      pIOShelfOrderDetailFromIDReturnModel commandParameter = (pIOShelfOrderDetailFromIDReturnModel) (sender as MenuItem).CommandParameter;
      if (commandParameter.PickingQty == commandParameter.OrderQty && commandParameter.PickingQty > 0.0)
      {
        GlobalMob.PlayError();
        int num = await ((Page) page).DisplayAlert("Uyarı", "Ürün zaten toplanmış gözüküyor.Toplanmış ürünü kara listeye ekleyemezsiniz", "", "Tamam") ? 1 : 0;
      }
      else
      {
        ReturnModel result = GlobalMob.PostJson("AddBlackList", new Dictionary<string, string>()
        {
          {
            "json",
            JsonConvert.SerializeObject((object) new ztIOShelfOrderBlacklist()
            {
              ColorCode = commandParameter.ColorCode,
              ItemCode = commandParameter.ItemCode,
              ItemDim1Code = commandParameter.ItemDim1Code,
              CreatedDate = new DateTime?(DateTime.Now),
              CreatedUserName = GlobalMob.User.UserName,
              IsBlocked = true,
              ItemDim2Code = commandParameter.ItemDim2Code,
              ItemDim3Code = "",
              Qty = 1,
              ShelfOrderDetailID = commandParameter.ShelfOrderDetailID,
              ShelfOrderID = commandParameter.ShelfOrderID,
              ShelfID = commandParameter.ShelfID,
              ItemTypeCode = 1,
              UpdateDate = new DateTime?(DateTime.Now),
              UpdateUserName = GlobalMob.User.UserName
            })
          }
        }, (ContentPage) page).Result;
        if (!result.Success || !JsonConvert.DeserializeObject<bool>(result.Result))
          return;
        GlobalMob.PlaySave();
        int num = await ((Page) page).DisplayAlert("Uyarı", "Ürün karalisteye eklendi.", "", "Tamam") ? 1 : 0;
      }
    }

    private void txtPackageNumber_Completed(object sender, EventArgs e)
    {
      string packageNumber = ((InputView) this.txtPackageNumber).Text;
      if (string.IsNullOrEmpty(packageNumber))
        return;
      pIOShelfOrderDetailFromIDReturnModel fromIdReturnModel = this.shelfOrderDetail.Where<pIOShelfOrderDetailFromIDReturnModel>((Func<pIOShelfOrderDetailFromIDReturnModel, bool>) (x => x.PackageBarcode == packageNumber)).FirstOrDefault<pIOShelfOrderDetailFromIDReturnModel>();
      if (fromIdReturnModel != null)
      {
        ((InputView) this.txtShelfBarcode).Text = fromIdReturnModel.ShelfCode;
        this.txtShelfBarcode_Completed((object) null, (EventArgs) null);
        ((InputView) this.txtPackageNumber).Text = packageNumber;
      }
      else
      {
        ((InputView) this.txtPackageNumber).Text = "";
        ((VisualElement) this.txtPackageNumber).Focus();
      }
    }

    private void txtPackageNumber_Completed_1(object sender, EventArgs e)
    {
    }

    private void txtMainShelf_Completed(object sender, EventArgs e)
    {
    }

    private void cmPrint_Clicked(object sender, EventArgs e)
    {
      pIOShelfOrderDetailFromIDReturnModel commandParameter = (pIOShelfOrderDetailFromIDReturnModel) (sender as MenuItem).CommandParameter;
      if (!GlobalMob.User.IsBluetoothPrinter)
        ((Page) this).DisplayAlert("Uyarı", commandParameter.ItemDescription + "\n" + commandParameter.ItemCode + commandParameter.ColorCode + commandParameter.ItemDim1Code, "", "Tamam");
      else if (string.IsNullOrEmpty(Settings.MobilePrinter))
      {
        ((Page) this).DisplayAlert("Uyarı", "Lütfen öncelikle bluetooth yazıcı seçiniz", "", "Tamam");
      }
      else
      {
        ReturnModel returnModel1 = GlobalMob.PostJson(string.Format("GetMobileTemplateText?reportTypeID=" + 1.ToString()), (ContentPage) this);
        if (!returnModel1.Success)
          return;
        ztIOBluetoothReportTemplate tmp = JsonConvert.DeserializeObject<ztIOBluetoothReportTemplate>(returnModel1.Result);
        tmp.Description = tmp.Description.Replace("<Barcode>", commandParameter.Barcode.Replace(",", ""));
        tmp.Description = GlobalMob.MobileTemplateReplace((object) commandParameter, tmp);
        ReturnModel returnModel2 = DependencyService.Get<INativeHelper>((DependencyFetchTarget) 0).Print(new MobilePrinterProp()
        {
          BrandID = tmp.PrinterBrandID,
          PrinterName = Settings.MobilePrinter,
          PrintText = tmp.Description
        });
        if (returnModel2.Success)
          return;
        ((Page) this).DisplayAlert("Hata", returnModel2.ErrorMessage, "", "Tamam");
      }
    }

    private async void cmShelfOrderPrint_Clicked(object sender, EventArgs e)
    {
      Picking page = this;
      pIOUserShelfOrdersReturnModel commandParameter = (pIOUserShelfOrdersReturnModel) (sender as MenuItem).CommandParameter;
      if (string.IsNullOrEmpty(commandParameter.FileName))
      {
        int num1 = await ((Page) page).DisplayAlert("Uyarı", "Raf Emri template eklenmemiş", "", "Tamam") ? 1 : 0;
      }
      else
      {
        int num2 = commandParameter.FileName.Contains(".repx") ? 2 : 1;
        List<BLReport> repList = new List<BLReport>();
        BLReport blReport = new BLReport()
        {
          ReportTypeID = 1,
          UserID = GlobalMob.User.UserID,
          FileType = num2,
          PrinterBrandID = Convert.ToInt32((object) commandParameter.PrinterBrandID),
          NetworkPrinter = commandParameter.NetworkPrinter
        };
        blReport.ParamList = new List<BLReportParam>();
        blReport.ParamList.Add(new BLReportParam()
        {
          ParamName = "ShelfOrderNumber",
          ParamValue = commandParameter.ShelfOrderNumber,
          ParamType = 20
        });
        blReport.ParamList.Add(new BLReportParam()
        {
          ParamName = "ShelfOrderID",
          ParamValue = Convert.ToString(commandParameter.ShelfOrderID),
          ParamType = 30
        });
        repList.Add(blReport);
        GlobalMob.BLPrint(repList, (object) commandParameter, (ContentPage) page);
      }
    }

    private async void ShowPackageList(pIOUserShelfOrdersReturnModel shelfOrder)
    {
      Picking page = this;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetPackageDetailFromShelfOrderID?shelfOrderID={0}&userID={1}", (object) shelfOrder.ShelfOrderID, (object) GlobalMob.User.UserID), (ContentPage) page);
      if (!returnModel.Success)
        return;
      page.packageDetails = GlobalMob.JsonDeserialize<List<pIOGetPackageDetailFromShelfOrderIDReturnModel>>(returnModel.Result);
      if (page.packageDetails == null || page.packageDetails.Count <= 0)
      {
        int num = await ((Page) page).DisplayAlert("Uyarı", "Koli bulunamadı", "", "Tamam") ? 1 : 0;
      }
      else
      {
        List<pIOShelfOrderDetailFromPackageReturnModel> list = page.packageDetails.GroupBy(c => new
        {
          Description = c.Description,
          PackageCode = c.PackageCode
        }).Select<IGrouping<\u003C\u003Ef__AnonymousType9<string, string>, pIOGetPackageDetailFromShelfOrderIDReturnModel>, pIOShelfOrderDetailFromPackageReturnModel>(gcs => new pIOShelfOrderDetailFromPackageReturnModel()
        {
          PackageCode = gcs.Key.PackageCode,
          PackageDescription = gcs.Key.Description
        }).ToList<pIOShelfOrderDetailFromPackageReturnModel>();
        List<CustomMenuItemParameter> mContextList = new List<CustomMenuItemParameter>();
        mContextList.Add(new CustomMenuItemParameter()
        {
          Text = "Detaylar",
          ClickedEvent = new EventHandler(page.Mn_Clicked)
        });
        if (page.packageDetails.Count > 0 && !string.IsNullOrEmpty(page.packageDetails[0].FileName))
        {
          mContextList.Add(new CustomMenuItemParameter()
          {
            Text = "Yazdır",
            ClickedEvent = new EventHandler(page.MnPackageItems_Clicked)
          });
          mContextList.Add(new CustomMenuItemParameter()
          {
            Text = "Tümünü Yazdır",
            ClickedEvent = new EventHandler(page.MnPackageAllItems_Clicked)
          });
        }
        ListView listview = GlobalMob.GetListview("PackageDescription", 0, 0, mContextList, true);
        ((View) listview).Margin = Thickness.op_Implicit(3.0);
        ((ItemsView<Cell>) listview).ItemsSource = (IEnumerable) list;
        SelectItem selectItem = new SelectItem(listview, "Koliler");
        await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
      }
    }

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

    private async void MnPackageAllItems_Clicked(object sender, EventArgs e)
    {
      Picking page = this;
      pIOShelfOrderDetailFromPackageReturnModel item = (pIOShelfOrderDetailFromPackageReturnModel) (sender as MenuItem).CommandParameter;
      await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
      List<BLReport> repList = new List<BLReport>();
      foreach (IGrouping<string, pIOGetPackageDetailFromShelfOrderIDReturnModel> grouping in page.packageDetails.Where<pIOGetPackageDetailFromShelfOrderIDReturnModel>((Func<pIOGetPackageDetailFromShelfOrderIDReturnModel, bool>) (x => !string.IsNullOrEmpty(x.PackageCode))).GroupBy<pIOGetPackageDetailFromShelfOrderIDReturnModel, string>((Func<pIOGetPackageDetailFromShelfOrderIDReturnModel, string>) (x => x.PackageCode)))
      {
        IGrouping<string, pIOGetPackageDetailFromShelfOrderIDReturnModel> package = grouping;
        pIOGetPackageDetailFromShelfOrderIDReturnModel orderIdReturnModel = page.packageDetails.Where<pIOGetPackageDetailFromShelfOrderIDReturnModel>((Func<pIOGetPackageDetailFromShelfOrderIDReturnModel, bool>) (x => x.PackageCode == package.Key)).FirstOrDefault<pIOGetPackageDetailFromShelfOrderIDReturnModel>();
        int num = orderIdReturnModel.FileName.Contains(".repx") ? 2 : 1;
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
          ParamValue = orderIdReturnModel.PackageCode,
          ParamType = 20
        });
        repList.Add(blReport);
      }
      GlobalMob.BLPrint(repList, (object) item, (ContentPage) page);
      GlobalMob.CloseLoading();
      item = (pIOShelfOrderDetailFromPackageReturnModel) null;
    }

    private async void Mn_Clicked(object sender, EventArgs e)
    {
      Picking picking = this;
      pIOShelfOrderDetailFromPackageReturnModel selectItem = (pIOShelfOrderDetailFromPackageReturnModel) (sender as MenuItem).CommandParameter;
      List<pIOGetPackageDetailFromShelfOrderIDReturnModel> list = picking.packageDetails.Where<pIOGetPackageDetailFromShelfOrderIDReturnModel>((Func<pIOGetPackageDetailFromShelfOrderIDReturnModel, bool>) (x => x.Description == selectItem.PackageDescription)).ToList<pIOGetPackageDetailFromShelfOrderIDReturnModel>().GroupBy(c => new
      {
        ItemCode = c.ItemCode,
        ColorCode = c.ColorCode,
        ItemDim1Code = c.ItemDim1Code,
        ItemDescription = c.ItemDescription
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType16<string, string, string, string>, pIOGetPackageDetailFromShelfOrderIDReturnModel>, pIOGetPackageDetailFromShelfOrderIDReturnModel>(gcs => new pIOGetPackageDetailFromShelfOrderIDReturnModel()
      {
        ItemCode = gcs.Key.ItemCode,
        ColorCode = gcs.Key.ColorCode,
        ItemDim1Code = gcs.Key.ItemDim1Code,
        ItemDescription = gcs.Key.ItemDescription,
        Qty = gcs.Sum<pIOGetPackageDetailFromShelfOrderIDReturnModel>((Func<pIOGetPackageDetailFromShelfOrderIDReturnModel, int>) (x => x.Qty))
      }).ToList<pIOGetPackageDetailFromShelfOrderIDReturnModel>();
      ListView listview = GlobalMob.GetListview("ItemDescription,Qty", 2, 1, hasUnEvenRows: true);
      ((ItemsView<Cell>) listview).ItemsSource = (IEnumerable) list;
      ((View) listview).Margin = Thickness.op_Implicit(3.0);
      List<ToolbarItem> toolbarItems = new List<ToolbarItem>();
      ToolbarItem toolbarItem1 = new ToolbarItem();
      ((MenuItem) toolbarItem1).Text = Convert.ToString(list.Sum<pIOGetPackageDetailFromShelfOrderIDReturnModel>((Func<pIOGetPackageDetailFromShelfOrderIDReturnModel, int>) (x => x.Qty)));
      ToolbarItem toolbarItem2 = toolbarItem1;
      toolbarItems.Add(toolbarItem2);
      SelectItem selectItem1 = new SelectItem(listview, selectItem.PackageDescription, toolbarItems);
      await ((NavigableElement) picking).Navigation.PushAsync((Page) selectItem1);
    }

    private void cmPackagePrint_Clicked(object sender, EventArgs e) => this.ShowPackageList((pIOUserShelfOrdersReturnModel) (sender as MenuItem).CommandParameter);

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (Picking).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/Picking.xaml",
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
        StackLayout stackLayout3 = new StackLayout();
        BindingExtension bindingExtension2 = new BindingExtension();
        BindingExtension bindingExtension3 = new BindingExtension();
        Picker picker1 = new Picker();
        StackLayout stackLayout4 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry1 = new SoftkeyboardDisabledEntry();
        StackLayout stackLayout5 = new StackLayout();
        Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension4 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension5 = new BindingExtension();
        Button button1 = new Button();
        ReferenceExtension referenceExtension3 = new ReferenceExtension();
        BindingExtension bindingExtension6 = new BindingExtension();
        ReferenceExtension referenceExtension4 = new ReferenceExtension();
        BindingExtension bindingExtension7 = new BindingExtension();
        Button button2 = new Button();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry2 = new SoftkeyboardDisabledEntry();
        ReferenceExtension referenceExtension5 = new ReferenceExtension();
        BindingExtension bindingExtension8 = new BindingExtension();
        ReferenceExtension referenceExtension6 = new ReferenceExtension();
        BindingExtension bindingExtension9 = new BindingExtension();
        Button button3 = new Button();
        StackLayout stackLayout6 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry3 = new SoftkeyboardDisabledEntry();
        StackLayout stackLayout7 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry4 = new SoftkeyboardDisabledEntry();
        StackLayout stackLayout8 = new StackLayout();
        KeyboardEnableEffect keyboardEnableEffect = new KeyboardEnableEffect();
        Xamarin.Forms.Entry entry3 = new Xamarin.Forms.Entry();
        Xamarin.Forms.Entry entry4 = new Xamarin.Forms.Entry();
        BindingExtension bindingExtension10 = new BindingExtension();
        BindingExtension bindingExtension11 = new BindingExtension();
        Picker picker2 = new Picker();
        StackLayout stackLayout9 = new StackLayout();
        Xamarin.Forms.Entry entry5 = new Xamarin.Forms.Entry();
        ReferenceExtension referenceExtension7 = new ReferenceExtension();
        BindingExtension bindingExtension12 = new BindingExtension();
        ReferenceExtension referenceExtension8 = new ReferenceExtension();
        BindingExtension bindingExtension13 = new BindingExtension();
        Button button4 = new Button();
        StackLayout stackLayout10 = new StackLayout();
        ReferenceExtension referenceExtension9 = new ReferenceExtension();
        BindingExtension bindingExtension14 = new BindingExtension();
        ReferenceExtension referenceExtension10 = new ReferenceExtension();
        BindingExtension bindingExtension15 = new BindingExtension();
        Button button5 = new Button();
        ReferenceExtension referenceExtension11 = new ReferenceExtension();
        BindingExtension bindingExtension16 = new BindingExtension();
        ReferenceExtension referenceExtension12 = new ReferenceExtension();
        BindingExtension bindingExtension17 = new BindingExtension();
        Button button6 = new Button();
        StackLayout stackLayout11 = new StackLayout();
        BindingExtension bindingExtension18 = new BindingExtension();
        ReferenceExtension referenceExtension13 = new ReferenceExtension();
        BindingExtension bindingExtension19 = new BindingExtension();
        ReferenceExtension referenceExtension14 = new ReferenceExtension();
        BindingExtension bindingExtension20 = new BindingExtension();
        Label label2 = new Label();
        StackLayout stackLayout12 = new StackLayout();
        DataTemplate dataTemplate2 = new DataTemplate();
        ListView listView2 = new ListView();
        ActivityIndicator activityIndicator = new ActivityIndicator();
        AbsoluteLayout absoluteLayout = new AbsoluteLayout();
        StackLayout stackLayout13 = new StackLayout();
        StackLayout stackLayout14 = new StackLayout();
        Picking picking;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (picking = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) picking, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("picking", (object) picking);
        if (((Element) picking).StyleId == null)
          ((Element) picking).StyleId = "picking";
        ((INameScope) nameScope).RegisterName("stckContent", (object) stackLayout14);
        if (((Element) stackLayout14).StyleId == null)
          ((Element) stackLayout14).StyleId = "stckContent";
        ((INameScope) nameScope).RegisterName("stckShelfOrderList", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckShelfOrderList";
        ((INameScope) nameScope).RegisterName("stckEmptyMessage", (object) stackLayout1);
        if (((Element) stackLayout1).StyleId == null)
          ((Element) stackLayout1).StyleId = "stckEmptyMessage";
        ((INameScope) nameScope).RegisterName("lstShelfOrder", (object) listView1);
        if (((Element) listView1).StyleId == null)
          ((Element) listView1).StyleId = "lstShelfOrder";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout13);
        if (((Element) stackLayout13).StyleId == null)
          ((Element) stackLayout13).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtShelfOrderNumber", (object) entry1);
        if (((Element) entry1).StyleId == null)
          ((Element) entry1).StyleId = "txtShelfOrderNumber";
        ((INameScope) nameScope).RegisterName("pckShelfType", (object) picker1);
        if (((Element) picker1).StyleId == null)
          ((Element) picker1).StyleId = "pckShelfType";
        ((INameScope) nameScope).RegisterName("stckMainShelf", (object) stackLayout5);
        if (((Element) stackLayout5).StyleId == null)
          ((Element) stackLayout5).StyleId = "stckMainShelf";
        ((INameScope) nameScope).RegisterName("txtMainShelf", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtMainShelf";
        ((INameScope) nameScope).RegisterName("stckShelf", (object) stackLayout6);
        if (((Element) stackLayout6).StyleId == null)
          ((Element) stackLayout6).StyleId = "stckShelf";
        ((INameScope) nameScope).RegisterName("txtShelf", (object) entry2);
        if (((Element) entry2).StyleId == null)
          ((Element) entry2).StyleId = "txtShelf";
        ((INameScope) nameScope).RegisterName("btnShelfBack", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnShelfBack";
        ((INameScope) nameScope).RegisterName("btnShelfNext", (object) button2);
        if (((Element) button2).StyleId == null)
          ((Element) button2).StyleId = "btnShelfNext";
        ((INameScope) nameScope).RegisterName("txtShelfBarcode", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtShelfBarcode";
        ((INameScope) nameScope).RegisterName("btnShelf", (object) button3);
        if (((Element) button3).StyleId == null)
          ((Element) button3).StyleId = "btnShelf";
        ((INameScope) nameScope).RegisterName("stckPackageNumber", (object) stackLayout7);
        if (((Element) stackLayout7).StyleId == null)
          ((Element) stackLayout7).StyleId = "stckPackageNumber";
        ((INameScope) nameScope).RegisterName("txtPackageNumber", (object) softkeyboardDisabledEntry3);
        if (((Element) softkeyboardDisabledEntry3).StyleId == null)
          ((Element) softkeyboardDisabledEntry3).StyleId = "txtPackageNumber";
        ((INameScope) nameScope).RegisterName("stckRulot", (object) stackLayout8);
        if (((Element) stackLayout8).StyleId == null)
          ((Element) stackLayout8).StyleId = "stckRulot";
        ((INameScope) nameScope).RegisterName("txtRulot", (object) softkeyboardDisabledEntry4);
        if (((Element) softkeyboardDisabledEntry4).StyleId == null)
          ((Element) softkeyboardDisabledEntry4).StyleId = "txtRulot";
        ((INameScope) nameScope).RegisterName("stckBarcode", (object) stackLayout9);
        if (((Element) stackLayout9).StyleId == null)
          ((Element) stackLayout9).StyleId = "stckBarcode";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) entry3);
        if (((Element) entry3).StyleId == null)
          ((Element) entry3).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("txtQty", (object) entry4);
        if (((Element) entry4).StyleId == null)
          ((Element) entry4).StyleId = "txtQty";
        ((INameScope) nameScope).RegisterName("pckBarcodeType", (object) picker2);
        if (((Element) picker2).StyleId == null)
          ((Element) picker2).StyleId = "pckBarcodeType";
        ((INameScope) nameScope).RegisterName("stckPivotShelf", (object) stackLayout10);
        if (((Element) stackLayout10).StyleId == null)
          ((Element) stackLayout10).StyleId = "stckPivotShelf";
        ((INameScope) nameScope).RegisterName("txtPivotShelf", (object) entry5);
        if (((Element) entry5).StyleId == null)
          ((Element) entry5).StyleId = "txtPivotShelf";
        ((INameScope) nameScope).RegisterName("btnCreateNewPackage", (object) button4);
        if (((Element) button4).StyleId == null)
          ((Element) button4).StyleId = "btnCreateNewPackage";
        ((INameScope) nameScope).RegisterName("btnPickOrder", (object) button5);
        if (((Element) button5).StyleId == null)
          ((Element) button5).StyleId = "btnPickOrder";
        ((INameScope) nameScope).RegisterName("btnShelfOrderSuccess", (object) button6);
        if (((Element) button6).StyleId == null)
          ((Element) button6).StyleId = "btnShelfOrderSuccess";
        ((INameScope) nameScope).RegisterName("lstShelfDetail", (object) listView2);
        if (((Element) listView2).StyleId == null)
          ((Element) listView2).StyleId = "lstShelfDetail";
        ((INameScope) nameScope).RegisterName("lblListHeader", (object) label2);
        if (((Element) label2).StyleId == null)
          ((Element) label2).StyleId = "lblListHeader";
        ((INameScope) nameScope).RegisterName("loadingScreen", (object) activityIndicator);
        if (((Element) activityIndicator).StyleId == null)
          ((Element) activityIndicator).StyleId = "loadingScreen";
        this.picking = (ContentPage) picking;
        this.stckContent = stackLayout14;
        this.stckShelfOrderList = stackLayout2;
        this.stckEmptyMessage = stackLayout1;
        this.lstShelfOrder = listView1;
        this.stckForm = stackLayout13;
        this.txtShelfOrderNumber = entry1;
        this.pckShelfType = picker1;
        this.stckMainShelf = stackLayout5;
        this.txtMainShelf = softkeyboardDisabledEntry1;
        this.stckShelf = stackLayout6;
        this.txtShelf = entry2;
        this.btnShelfBack = button1;
        this.btnShelfNext = button2;
        this.txtShelfBarcode = softkeyboardDisabledEntry2;
        this.btnShelf = button3;
        this.stckPackageNumber = stackLayout7;
        this.txtPackageNumber = softkeyboardDisabledEntry3;
        this.stckRulot = stackLayout8;
        this.txtRulot = softkeyboardDisabledEntry4;
        this.stckBarcode = stackLayout9;
        this.txtBarcode = entry3;
        this.txtQty = entry4;
        this.pckBarcodeType = picker2;
        this.stckPivotShelf = stackLayout10;
        this.txtPivotShelf = entry5;
        this.btnCreateNewPackage = button4;
        this.btnPickOrder = button5;
        this.btnShelfOrderSuccess = button6;
        this.lstShelfDetail = listView2;
        this.lblListHeader = label2;
        this.loadingScreen = activityIndicator;
        ((BindableObject) stackLayout14).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
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
        objArray1[3] = (object) stackLayout14;
        objArray1[4] = (object) picking;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) Label.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver1.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (Picking).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(13, 128)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter1).ConvertFromInvariantString("Medium", (IServiceProvider) xamlServiceProvider1);
        ((BindableObject) label3).SetValue(fontSizeProperty1, obj2);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) label1);
        VisualDiagnostics.RegisterSourceInfo((object) label1, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 18);
        ((BindableObject) listView1).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        bindingExtension1.Path = ".";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView1).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase1);
        ((BindableObject) listView1).SetValue(ListView.RowHeightProperty, (object) 60);
        ((BindableObject) listView1).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        ((BindableObject) listView1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        DataTemplate dataTemplate3 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Picking.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_19 xamlCdataTemplate19 = new Picking.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_19();
        object[] objArray2 = new object[0 + 5];
        objArray2[0] = (object) dataTemplate1;
        objArray2[1] = (object) listView1;
        objArray2[2] = (object) stackLayout2;
        objArray2[3] = (object) stackLayout14;
        objArray2[4] = (object) picking;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate19.parentValues = objArray2;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate19.root = picking;
        // ISSUE: reference to a compiler-generated method
        Func<object> func1 = new Func<object>(xamlCdataTemplate19.LoadDataTemplate);
        ((IDataTemplate) dataTemplate3).LoadTemplate = func1;
        ((BindableObject) listView1).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) listView1);
        VisualDiagnostics.RegisterSourceInfo((object) listView1, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout14).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 14);
        ((BindableObject) stackLayout13).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0, 2.0, 5.0, 5.0));
        ((BindableObject) stackLayout13).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout3).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf Emri Numarası Giriniz");
        ((BindableObject) entry1).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry1).SetValue(VisualElement.InputTransparentProperty, (object) true);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 51, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout11).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 50, 22);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) picker1).SetValue(Picker.TitleProperty, (object) "Dağıtım Rafı Seçiniz");
        bindingExtension2.Path = ".";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker1).SetBinding(Picker.ItemsSourceProperty, bindingBase2);
        bindingExtension3.Path = "Caption";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        picker1.ItemDisplayBinding = bindingBase3;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase3, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 58, 33);
        ((BindableObject) picker1).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        picker1.SelectedIndexChanged += new EventHandler(picking.pckShelfType_SelectedIndexChanged);
        ((BindableObject) picker1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) picker1);
        VisualDiagnostics.RegisterSourceInfo((object) picker1, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 57, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout11).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 56, 22);
        ((BindableObject) stackLayout5).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout5).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        softkeyboardDisabledEntry1.Completed += new EventHandler(picking.txtMainShelf_Completed);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Ana Raf");
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 64, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout11).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 62, 22);
        ((BindableObject) stackLayout6).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout6).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "");
        ((BindableObject) entry2).SetValue(VisualElement.InputTransparentProperty, (object) true);
        ((BindableObject) entry2).SetValue(VisualElement.WidthRequestProperty, (object) 150.0);
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Start);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 69, 30);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "<");
        referenceExtension1.Name = "picking";
        ReferenceExtension referenceExtension15 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 7];
        objArray3[0] = (object) bindingExtension4;
        objArray3[1] = (object) button1;
        objArray3[2] = (object) stackLayout6;
        objArray3[3] = (object) stackLayout11;
        objArray3[4] = (object) stackLayout13;
        objArray3[5] = (object) stackLayout14;
        objArray3[6] = (object) picking;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray3, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver2.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver2.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (Picking).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(71, 33)));
        object obj4 = ((IMarkupExtension) referenceExtension15).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension4.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 71, 33);
        bindingExtension4.Path = "ButtonColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase4);
        referenceExtension2.Name = "picking";
        ReferenceExtension referenceExtension16 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 7];
        objArray4[0] = (object) bindingExtension5;
        objArray4[1] = (object) button1;
        objArray4[2] = (object) stackLayout6;
        objArray4[3] = (object) stackLayout11;
        objArray4[4] = (object) stackLayout13;
        objArray4[5] = (object) stackLayout14;
        objArray4[6] = (object) picking;
        SimpleValueTargetProvider valueTargetProvider3;
        object obj5 = (object) (valueTargetProvider3 = new SimpleValueTargetProvider(objArray4, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider3.Add(type5, (object) valueTargetProvider3);
        xamlServiceProvider3.Add(typeof (IReferenceProvider), obj5);
        Type type6 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver3 = new XmlNamespaceResolver();
        namespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver3.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver3.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (Picking).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(71, 103)));
        object obj6 = ((IMarkupExtension) referenceExtension16).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension5.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 71, 103);
        bindingExtension5.Path = "TextColor";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(Button.TextColorProperty, bindingBase5);
        button1.Clicked += new EventHandler(picking.btnShelfBack_Clicked);
        ((BindableObject) button1).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.StartAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 70, 30);
        ((BindableObject) button2).SetValue(Button.TextProperty, (object) ">");
        referenceExtension3.Name = "picking";
        ReferenceExtension referenceExtension17 = referenceExtension3;
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray5 = new object[0 + 7];
        objArray5[0] = (object) bindingExtension6;
        objArray5[1] = (object) button2;
        objArray5[2] = (object) stackLayout6;
        objArray5[3] = (object) stackLayout11;
        objArray5[4] = (object) stackLayout13;
        objArray5[5] = (object) stackLayout14;
        objArray5[6] = (object) picking;
        SimpleValueTargetProvider valueTargetProvider4;
        object obj7 = (object) (valueTargetProvider4 = new SimpleValueTargetProvider(objArray5, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider4.Add(type7, (object) valueTargetProvider4);
        xamlServiceProvider4.Add(typeof (IReferenceProvider), obj7);
        Type type8 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver4 = new XmlNamespaceResolver();
        namespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver4.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver4.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (Picking).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(74, 33)));
        object obj8 = ((IMarkupExtension) referenceExtension17).ProvideValue((IServiceProvider) xamlServiceProvider4);
        bindingExtension6.Source = obj8;
        VisualDiagnostics.RegisterSourceInfo(obj8, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 74, 33);
        bindingExtension6.Path = "ButtonColor";
        BindingBase bindingBase6 = ((IMarkupExtension<BindingBase>) bindingExtension6).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase6);
        referenceExtension4.Name = "picking";
        ReferenceExtension referenceExtension18 = referenceExtension4;
        XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
        Type type9 = typeof (IProvideValueTarget);
        object[] objArray6 = new object[0 + 7];
        objArray6[0] = (object) bindingExtension7;
        objArray6[1] = (object) button2;
        objArray6[2] = (object) stackLayout6;
        objArray6[3] = (object) stackLayout11;
        objArray6[4] = (object) stackLayout13;
        objArray6[5] = (object) stackLayout14;
        objArray6[6] = (object) picking;
        SimpleValueTargetProvider valueTargetProvider5;
        object obj9 = (object) (valueTargetProvider5 = new SimpleValueTargetProvider(objArray6, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider5.Add(type9, (object) valueTargetProvider5);
        xamlServiceProvider5.Add(typeof (IReferenceProvider), obj9);
        Type type10 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver5 = new XmlNamespaceResolver();
        namespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver5.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver5.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver5 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver5, typeof (Picking).GetTypeInfo().Assembly);
        xamlServiceProvider5.Add(type10, (object) xamlTypeResolver5);
        xamlServiceProvider5.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(74, 103)));
        object obj10 = ((IMarkupExtension) referenceExtension18).ProvideValue((IServiceProvider) xamlServiceProvider5);
        bindingExtension7.Source = obj10;
        VisualDiagnostics.RegisterSourceInfo(obj10, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 74, 103);
        bindingExtension7.Path = "TextColor";
        BindingBase bindingBase7 = ((IMarkupExtension<BindingBase>) bindingExtension7).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(Button.TextColorProperty, bindingBase7);
        button2.Clicked += new EventHandler(picking.btnShelfNext_Clicked);
        ((BindableObject) button2).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.StartAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) button2);
        VisualDiagnostics.RegisterSourceInfo((object) button2, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 73, 30);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf No Okutunuz");
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(VisualElement.WidthRequestProperty, (object) 150.0);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        softkeyboardDisabledEntry2.Completed += new EventHandler(picking.txtShelfBarcode_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 76, 30);
        ((BindableObject) button3).SetValue(Button.TextProperty, (object) "...");
        button3.Clicked += new EventHandler(picking.btnShelf_Clicked);
        referenceExtension5.Name = "picking";
        ReferenceExtension referenceExtension19 = referenceExtension5;
        XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
        Type type11 = typeof (IProvideValueTarget);
        object[] objArray7 = new object[0 + 7];
        objArray7[0] = (object) bindingExtension8;
        objArray7[1] = (object) button3;
        objArray7[2] = (object) stackLayout6;
        objArray7[3] = (object) stackLayout11;
        objArray7[4] = (object) stackLayout13;
        objArray7[5] = (object) stackLayout14;
        objArray7[6] = (object) picking;
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
        XamlTypeResolver xamlTypeResolver6 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver6, typeof (Picking).GetTypeInfo().Assembly);
        xamlServiceProvider6.Add(type12, (object) xamlTypeResolver6);
        xamlServiceProvider6.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(79, 25)));
        object obj12 = ((IMarkupExtension) referenceExtension19).ProvideValue((IServiceProvider) xamlServiceProvider6);
        bindingExtension8.Source = obj12;
        VisualDiagnostics.RegisterSourceInfo(obj12, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 79, 25);
        bindingExtension8.Path = "ButtonColor";
        BindingBase bindingBase8 = ((IMarkupExtension<BindingBase>) bindingExtension8).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(VisualElement.BackgroundColorProperty, bindingBase8);
        referenceExtension6.Name = "picking";
        ReferenceExtension referenceExtension20 = referenceExtension6;
        XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
        Type type13 = typeof (IProvideValueTarget);
        object[] objArray8 = new object[0 + 7];
        objArray8[0] = (object) bindingExtension9;
        objArray8[1] = (object) button3;
        objArray8[2] = (object) stackLayout6;
        objArray8[3] = (object) stackLayout11;
        objArray8[4] = (object) stackLayout13;
        objArray8[5] = (object) stackLayout14;
        objArray8[6] = (object) picking;
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
        XamlTypeResolver xamlTypeResolver7 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver7, typeof (Picking).GetTypeInfo().Assembly);
        xamlServiceProvider7.Add(type14, (object) xamlTypeResolver7);
        xamlServiceProvider7.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(79, 95)));
        object obj14 = ((IMarkupExtension) referenceExtension20).ProvideValue((IServiceProvider) xamlServiceProvider7);
        bindingExtension9.Source = obj14;
        VisualDiagnostics.RegisterSourceInfo(obj14, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 79, 95);
        bindingExtension9.Path = "TextColor";
        BindingBase bindingBase9 = ((IMarkupExtension<BindingBase>) bindingExtension9).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(Button.TextColorProperty, bindingBase9);
        ((BindableObject) button3).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) button3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.EndAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) button3);
        VisualDiagnostics.RegisterSourceInfo((object) button3, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 78, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout11).Children).Add((View) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 68, 22);
        ((BindableObject) stackLayout7).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout7).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        softkeyboardDisabledEntry3.Completed += new EventHandler(picking.txtPackageNumber_Completed);
        ((BindableObject) softkeyboardDisabledEntry3).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Koliyi Okutunuz");
        ((BindableObject) softkeyboardDisabledEntry3).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) softkeyboardDisabledEntry3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) softkeyboardDisabledEntry3);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry3, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 84, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout11).Children).Add((View) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 82, 22);
        ((BindableObject) stackLayout8).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout8).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) softkeyboardDisabledEntry4).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Rulot Okutunuz");
        softkeyboardDisabledEntry4.Completed += new EventHandler(picking.txtRulot_Completed);
        ((BindableObject) softkeyboardDisabledEntry4).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) softkeyboardDisabledEntry4);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry4, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 89, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout11).Children).Add((View) stackLayout8);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout8, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 88, 22);
        ((BindableObject) stackLayout9).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout9).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) entry3).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod No Giriniz/Okutunuz");
        ((BindableObject) entry3).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("True"));
        entry3.Completed += new EventHandler(picking.txtBarcode_Completed);
        ((BindableObject) entry3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry3).SetValue(KeyboardEffect.EnableKeyboardProperty, (object) false);
        ((ICollection<Effect>) ((Element) entry3).Effects).Add((Effect) keyboardEnableEffect);
        VisualDiagnostics.RegisterSourceInfo((object) keyboardEnableEffect, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 98, 34);
        ((ICollection<View>) ((Layout<View>) stackLayout9).Children).Add((View) entry3);
        VisualDiagnostics.RegisterSourceInfo((object) entry3, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 93, 26);
        ((BindableObject) entry4).SetValue(Xamarin.Forms.Entry.TextProperty, (object) "1");
        ((BindableObject) entry4).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Miktar");
        ((BindableObject) entry4).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry4).SetValue(Xamarin.Forms.Entry.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) entry4).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout9).Children).Add((View) entry4);
        VisualDiagnostics.RegisterSourceInfo((object) entry4, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 101, 26);
        ((BindableObject) picker2).SetValue(Picker.TitleProperty, (object) "Tip");
        bindingExtension10.Path = ".";
        BindingBase bindingBase10 = ((IMarkupExtension<BindingBase>) bindingExtension10).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker2).SetBinding(Picker.ItemsSourceProperty, bindingBase10);
        picker2.SelectedIndexChanged += new EventHandler(picking.pckBarcodeType_SelectedIndexChanged);
        bindingExtension11.Path = "Caption";
        BindingBase bindingBase11 = ((IMarkupExtension<BindingBase>) bindingExtension11).ProvideValue((IServiceProvider) null);
        picker2.ItemDisplayBinding = bindingBase11;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase11, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 104, 33);
        ((BindableObject) picker2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) picker2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout9).Children).Add((View) picker2);
        VisualDiagnostics.RegisterSourceInfo((object) picker2, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 103, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout11).Children).Add((View) stackLayout9);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout9, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 92, 22);
        ((BindableObject) stackLayout10).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout10).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) entry5).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Ürünün Bırakılacağı Raf");
        ((BindableObject) entry5).SetValue(VisualElement.BackgroundColorProperty, (object) Color.White);
        ((BindableObject) entry5).SetValue(Xamarin.Forms.Entry.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) entry5).SetValue(Xamarin.Forms.Entry.TextColorProperty, (object) Color.Red);
        Xamarin.Forms.Entry entry6 = entry5;
        BindableProperty fontSizeProperty2 = Xamarin.Forms.Entry.FontSizeProperty;
        FontSizeConverter fontSizeConverter2 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
        Type type15 = typeof (IProvideValueTarget);
        object[] objArray9 = new object[0 + 6];
        objArray9[0] = (object) entry5;
        objArray9[1] = (object) stackLayout10;
        objArray9[2] = (object) stackLayout11;
        objArray9[3] = (object) stackLayout13;
        objArray9[4] = (object) stackLayout14;
        objArray9[5] = (object) picking;
        SimpleValueTargetProvider valueTargetProvider8;
        object obj15 = (object) (valueTargetProvider8 = new SimpleValueTargetProvider(objArray9, (object) Xamarin.Forms.Entry.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider8.Add(type15, (object) valueTargetProvider8);
        xamlServiceProvider8.Add(typeof (IReferenceProvider), obj15);
        Type type16 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver8 = new XmlNamespaceResolver();
        namespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver8.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver8.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver8 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver8, typeof (Picking).GetTypeInfo().Assembly);
        xamlServiceProvider8.Add(type16, (object) xamlTypeResolver8);
        xamlServiceProvider8.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(109, 48)));
        object obj16 = ((IExtendedTypeConverter) fontSizeConverter2).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider8);
        ((BindableObject) entry6).SetValue(fontSizeProperty2, obj16);
        ((BindableObject) entry5).SetValue(Xamarin.Forms.Entry.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) entry5).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout10).Children).Add((View) entry5);
        VisualDiagnostics.RegisterSourceInfo((object) entry5, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 108, 26);
        ((BindableObject) button4).SetValue(Button.TextProperty, (object) "+");
        ((BindableObject) button4).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        button4.Clicked += new EventHandler(picking.btnCreateNewPackage_Clicked);
        ((BindableObject) button4).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) button4).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        referenceExtension7.Name = "picking";
        ReferenceExtension referenceExtension21 = referenceExtension7;
        XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
        Type type17 = typeof (IProvideValueTarget);
        object[] objArray10 = new object[0 + 7];
        objArray10[0] = (object) bindingExtension12;
        objArray10[1] = (object) button4;
        objArray10[2] = (object) stackLayout10;
        objArray10[3] = (object) stackLayout11;
        objArray10[4] = (object) stackLayout13;
        objArray10[5] = (object) stackLayout14;
        objArray10[6] = (object) picking;
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
        XamlTypeResolver xamlTypeResolver9 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver9, typeof (Picking).GetTypeInfo().Assembly);
        xamlServiceProvider9.Add(type18, (object) xamlTypeResolver9);
        xamlServiceProvider9.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(113, 28)));
        object obj18 = ((IMarkupExtension) referenceExtension21).ProvideValue((IServiceProvider) xamlServiceProvider9);
        bindingExtension12.Source = obj18;
        VisualDiagnostics.RegisterSourceInfo(obj18, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 113, 28);
        bindingExtension12.Path = "ButtonColor";
        BindingBase bindingBase12 = ((IMarkupExtension<BindingBase>) bindingExtension12).ProvideValue((IServiceProvider) null);
        ((BindableObject) button4).SetBinding(VisualElement.BackgroundColorProperty, bindingBase12);
        referenceExtension8.Name = "picking";
        ReferenceExtension referenceExtension22 = referenceExtension8;
        XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
        Type type19 = typeof (IProvideValueTarget);
        object[] objArray11 = new object[0 + 7];
        objArray11[0] = (object) bindingExtension13;
        objArray11[1] = (object) button4;
        objArray11[2] = (object) stackLayout10;
        objArray11[3] = (object) stackLayout11;
        objArray11[4] = (object) stackLayout13;
        objArray11[5] = (object) stackLayout14;
        objArray11[6] = (object) picking;
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
        XamlTypeResolver xamlTypeResolver10 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver10, typeof (Picking).GetTypeInfo().Assembly);
        xamlServiceProvider10.Add(type20, (object) xamlTypeResolver10);
        xamlServiceProvider10.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(114, 28)));
        object obj20 = ((IMarkupExtension) referenceExtension22).ProvideValue((IServiceProvider) xamlServiceProvider10);
        bindingExtension13.Source = obj20;
        VisualDiagnostics.RegisterSourceInfo(obj20, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 114, 28);
        bindingExtension13.Path = "TextColor";
        BindingBase bindingBase13 = ((IMarkupExtension<BindingBase>) bindingExtension13).ProvideValue((IServiceProvider) null);
        ((BindableObject) button4).SetBinding(Button.TextColorProperty, bindingBase13);
        ((ICollection<View>) ((Layout<View>) stackLayout10).Children).Add((View) button4);
        VisualDiagnostics.RegisterSourceInfo((object) button4, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 111, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout11).Children).Add((View) stackLayout10);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout10, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 107, 22);
        ((BindableObject) button5).SetValue(Button.TextProperty, (object) "Ekle/Okut");
        referenceExtension9.Name = "picking";
        ReferenceExtension referenceExtension23 = referenceExtension9;
        XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
        Type type21 = typeof (IProvideValueTarget);
        object[] objArray12 = new object[0 + 6];
        objArray12[0] = (object) bindingExtension14;
        objArray12[1] = (object) button5;
        objArray12[2] = (object) stackLayout11;
        objArray12[3] = (object) stackLayout13;
        objArray12[4] = (object) stackLayout14;
        objArray12[5] = (object) picking;
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
        XamlTypeResolver xamlTypeResolver11 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver11, typeof (Picking).GetTypeInfo().Assembly);
        xamlServiceProvider11.Add(type22, (object) xamlTypeResolver11);
        xamlServiceProvider11.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(117, 21)));
        object obj22 = ((IMarkupExtension) referenceExtension23).ProvideValue((IServiceProvider) xamlServiceProvider11);
        bindingExtension14.Source = obj22;
        VisualDiagnostics.RegisterSourceInfo(obj22, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 117, 21);
        bindingExtension14.Path = "ButtonColor";
        BindingBase bindingBase14 = ((IMarkupExtension<BindingBase>) bindingExtension14).ProvideValue((IServiceProvider) null);
        ((BindableObject) button5).SetBinding(VisualElement.BackgroundColorProperty, bindingBase14);
        referenceExtension10.Name = "picking";
        ReferenceExtension referenceExtension24 = referenceExtension10;
        XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
        Type type23 = typeof (IProvideValueTarget);
        object[] objArray13 = new object[0 + 6];
        objArray13[0] = (object) bindingExtension15;
        objArray13[1] = (object) button5;
        objArray13[2] = (object) stackLayout11;
        objArray13[3] = (object) stackLayout13;
        objArray13[4] = (object) stackLayout14;
        objArray13[5] = (object) picking;
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
        XamlTypeResolver xamlTypeResolver12 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver12, typeof (Picking).GetTypeInfo().Assembly);
        xamlServiceProvider12.Add(type24, (object) xamlTypeResolver12);
        xamlServiceProvider12.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(117, 91)));
        object obj24 = ((IMarkupExtension) referenceExtension24).ProvideValue((IServiceProvider) xamlServiceProvider12);
        bindingExtension15.Source = obj24;
        VisualDiagnostics.RegisterSourceInfo(obj24, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 117, 91);
        bindingExtension15.Path = "TextColor";
        BindingBase bindingBase15 = ((IMarkupExtension<BindingBase>) bindingExtension15).ProvideValue((IServiceProvider) null);
        ((BindableObject) button5).SetBinding(Button.TextColorProperty, bindingBase15);
        ((BindableObject) button5).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((ICollection<View>) ((Layout<View>) stackLayout11).Children).Add((View) button5);
        VisualDiagnostics.RegisterSourceInfo((object) button5, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 116, 22);
        ((BindableObject) button6).SetValue(Button.TextProperty, (object) "Tamamla");
        button6.Clicked += new EventHandler(picking.btnShelfOrderSuccess_Clicked);
        referenceExtension11.Name = "picking";
        ReferenceExtension referenceExtension25 = referenceExtension11;
        XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
        Type type25 = typeof (IProvideValueTarget);
        object[] objArray14 = new object[0 + 6];
        objArray14[0] = (object) bindingExtension16;
        objArray14[1] = (object) button6;
        objArray14[2] = (object) stackLayout11;
        objArray14[3] = (object) stackLayout13;
        objArray14[4] = (object) stackLayout14;
        objArray14[5] = (object) picking;
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
        XamlTypeResolver xamlTypeResolver13 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver13, typeof (Picking).GetTypeInfo().Assembly);
        xamlServiceProvider13.Add(type26, (object) xamlTypeResolver13);
        xamlServiceProvider13.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(120, 21)));
        object obj26 = ((IMarkupExtension) referenceExtension25).ProvideValue((IServiceProvider) xamlServiceProvider13);
        bindingExtension16.Source = obj26;
        VisualDiagnostics.RegisterSourceInfo(obj26, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 120, 21);
        bindingExtension16.Path = "ButtonColor";
        BindingBase bindingBase16 = ((IMarkupExtension<BindingBase>) bindingExtension16).ProvideValue((IServiceProvider) null);
        ((BindableObject) button6).SetBinding(VisualElement.BackgroundColorProperty, bindingBase16);
        referenceExtension12.Name = "picking";
        ReferenceExtension referenceExtension26 = referenceExtension12;
        XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
        Type type27 = typeof (IProvideValueTarget);
        object[] objArray15 = new object[0 + 6];
        objArray15[0] = (object) bindingExtension17;
        objArray15[1] = (object) button6;
        objArray15[2] = (object) stackLayout11;
        objArray15[3] = (object) stackLayout13;
        objArray15[4] = (object) stackLayout14;
        objArray15[5] = (object) picking;
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
        XamlTypeResolver xamlTypeResolver14 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver14, typeof (Picking).GetTypeInfo().Assembly);
        xamlServiceProvider14.Add(type28, (object) xamlTypeResolver14);
        xamlServiceProvider14.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(120, 91)));
        object obj28 = ((IMarkupExtension) referenceExtension26).ProvideValue((IServiceProvider) xamlServiceProvider14);
        bindingExtension17.Source = obj28;
        VisualDiagnostics.RegisterSourceInfo(obj28, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 120, 91);
        bindingExtension17.Path = "TextColor";
        BindingBase bindingBase17 = ((IMarkupExtension<BindingBase>) bindingExtension17).ProvideValue((IServiceProvider) null);
        ((BindableObject) button6).SetBinding(Button.TextColorProperty, bindingBase17);
        ((BindableObject) button6).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((ICollection<View>) ((Layout<View>) stackLayout11).Children).Add((View) button6);
        VisualDiagnostics.RegisterSourceInfo((object) button6, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 119, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout13).Children).Add((View) stackLayout11);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout11, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 49, 18);
        bindingExtension18.Path = ".";
        BindingBase bindingBase18 = ((IMarkupExtension<BindingBase>) bindingExtension18).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView2).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase18);
        ((BindableObject) listView2).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) listView2).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        ((BindableObject) listView2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) listView2).SetValue(VisualElement.HeightRequestProperty, (object) 5000.0);
        ((BindableObject) listView2).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        ((BindableObject) label2).SetValue(Label.TextProperty, (object) "Raf Detayları");
        ((BindableObject) label2).SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        referenceExtension13.Name = "picking";
        ReferenceExtension referenceExtension27 = referenceExtension13;
        XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
        Type type29 = typeof (IProvideValueTarget);
        object[] objArray16 = new object[0 + 7];
        objArray16[0] = (object) bindingExtension19;
        objArray16[1] = (object) label2;
        objArray16[2] = (object) stackLayout12;
        objArray16[3] = (object) listView2;
        objArray16[4] = (object) stackLayout13;
        objArray16[5] = (object) stackLayout14;
        objArray16[6] = (object) picking;
        SimpleValueTargetProvider valueTargetProvider15;
        object obj29 = (object) (valueTargetProvider15 = new SimpleValueTargetProvider(objArray16, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider15.Add(type29, (object) valueTargetProvider15);
        xamlServiceProvider15.Add(typeof (IReferenceProvider), obj29);
        Type type30 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver15 = new XmlNamespaceResolver();
        namespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver15.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver15.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver15 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver15, typeof (Picking).GetTypeInfo().Assembly);
        xamlServiceProvider15.Add(type30, (object) xamlTypeResolver15);
        xamlServiceProvider15.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(130, 36)));
        object obj30 = ((IMarkupExtension) referenceExtension27).ProvideValue((IServiceProvider) xamlServiceProvider15);
        bindingExtension19.Source = obj30;
        VisualDiagnostics.RegisterSourceInfo(obj30, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 130, 36);
        bindingExtension19.Path = "ButtonColor";
        BindingBase bindingBase19 = ((IMarkupExtension<BindingBase>) bindingExtension19).ProvideValue((IServiceProvider) null);
        ((BindableObject) label2).SetBinding(Label.TextColorProperty, bindingBase19);
        referenceExtension14.Name = "picking";
        ReferenceExtension referenceExtension28 = referenceExtension14;
        XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
        Type type31 = typeof (IProvideValueTarget);
        object[] objArray17 = new object[0 + 7];
        objArray17[0] = (object) bindingExtension20;
        objArray17[1] = (object) label2;
        objArray17[2] = (object) stackLayout12;
        objArray17[3] = (object) listView2;
        objArray17[4] = (object) stackLayout13;
        objArray17[5] = (object) stackLayout14;
        objArray17[6] = (object) picking;
        SimpleValueTargetProvider valueTargetProvider16;
        object obj31 = (object) (valueTargetProvider16 = new SimpleValueTargetProvider(objArray17, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider16.Add(type31, (object) valueTargetProvider16);
        xamlServiceProvider16.Add(typeof (IReferenceProvider), obj31);
        Type type32 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver16 = new XmlNamespaceResolver();
        namespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver16.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        namespaceResolver16.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver16 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver16, typeof (Picking).GetTypeInfo().Assembly);
        xamlServiceProvider16.Add(type32, (object) xamlTypeResolver16);
        xamlServiceProvider16.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(131, 36)));
        object obj32 = ((IMarkupExtension) referenceExtension28).ProvideValue((IServiceProvider) xamlServiceProvider16);
        bindingExtension20.Source = obj32;
        VisualDiagnostics.RegisterSourceInfo(obj32, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 131, 36);
        bindingExtension20.Path = "TextColor";
        BindingBase bindingBase20 = ((IMarkupExtension<BindingBase>) bindingExtension20).ProvideValue((IServiceProvider) null);
        ((BindableObject) label2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase20);
        ((BindableObject) label2).SetValue(Label.FontProperty, new FontTypeConverter().ConvertFromInvariantString("Bold,20"));
        ((ICollection<View>) ((Layout<View>) stackLayout12).Children).Add((View) label2);
        VisualDiagnostics.RegisterSourceInfo((object) label2, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 128, 30);
        ((BindableObject) listView2).SetValue(ListView.HeaderProperty, (object) stackLayout12);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout12, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), (int) sbyte.MaxValue, 26);
        DataTemplate dataTemplate4 = dataTemplate2;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Picking.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_20 xamlCdataTemplate20 = new Picking.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_20();
        object[] objArray18 = new object[0 + 5];
        objArray18[0] = (object) dataTemplate2;
        objArray18[1] = (object) listView2;
        objArray18[2] = (object) stackLayout13;
        objArray18[3] = (object) stackLayout14;
        objArray18[4] = (object) picking;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate20.parentValues = objArray18;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate20.root = picking;
        // ISSUE: reference to a compiler-generated method
        Func<object> func2 = new Func<object>(xamlCdataTemplate20.LoadDataTemplate);
        ((IDataTemplate) dataTemplate4).LoadTemplate = func2;
        ((BindableObject) listView2).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate2);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate2, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 136, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout13).Children).Add((View) listView2);
        VisualDiagnostics.RegisterSourceInfo((object) listView2, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 123, 18);
        ((BindableObject) absoluteLayout).SetValue(AbsoluteLayout.LayoutBoundsProperty, (object) new Rectangle(0.5, 0.5, 1.0, 1.0));
        ((BindableObject) absoluteLayout).SetValue(AbsoluteLayout.LayoutFlagsProperty, (object) (AbsoluteLayoutFlags) -1);
        ((BindableObject) activityIndicator).SetValue(ActivityIndicator.IsRunningProperty, (object) false);
        ((BindableObject) activityIndicator).SetValue(VisualElement.IsEnabledProperty, (object) false);
        ((BindableObject) activityIndicator).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) activityIndicator).SetValue(AbsoluteLayout.LayoutBoundsProperty, (object) new Rectangle(0.5, 0.5, 0.1, 0.1));
        ((BindableObject) activityIndicator).SetValue(AbsoluteLayout.LayoutFlagsProperty, (object) (AbsoluteLayoutFlags) -1);
        ((BindableObject) activityIndicator).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) activityIndicator).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) activityIndicator).SetValue(ActivityIndicator.ColorProperty, (object) Color.Black);
        ((ICollection<View>) absoluteLayout.Children).Add((View) activityIndicator);
        VisualDiagnostics.RegisterSourceInfo((object) activityIndicator, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 166, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout13).Children).Add((View) absoluteLayout);
        VisualDiagnostics.RegisterSourceInfo((object) absoluteLayout, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 165, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout14).Children).Add((View) stackLayout13);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout13, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 48, 14);
        ((BindableObject) picking).SetValue(ContentPage.ContentProperty, (object) stackLayout14);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout14, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 9, 10);
        VisualDiagnostics.RegisterSourceInfo((object) picking, new Uri("Views\\Picking.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<Picking>(this, typeof (Picking));
      this.picking = NameScopeExtensions.FindByName<ContentPage>((Element) this, "picking");
      this.stckContent = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckContent");
      this.stckShelfOrderList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfOrderList");
      this.stckEmptyMessage = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckEmptyMessage");
      this.lstShelfOrder = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfOrder");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtShelfOrderNumber = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtShelfOrderNumber");
      this.pckShelfType = NameScopeExtensions.FindByName<Picker>((Element) this, "pckShelfType");
      this.stckMainShelf = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckMainShelf");
      this.txtMainShelf = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtMainShelf");
      this.stckShelf = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelf");
      this.txtShelf = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtShelf");
      this.btnShelfBack = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelfBack");
      this.btnShelfNext = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelfNext");
      this.txtShelfBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtShelfBarcode");
      this.btnShelf = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelf");
      this.stckPackageNumber = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckPackageNumber");
      this.txtPackageNumber = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtPackageNumber");
      this.stckRulot = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckRulot");
      this.txtRulot = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtRulot");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.txtBarcode = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtBarcode");
      this.txtQty = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtQty");
      this.pckBarcodeType = NameScopeExtensions.FindByName<Picker>((Element) this, "pckBarcodeType");
      this.stckPivotShelf = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckPivotShelf");
      this.txtPivotShelf = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtPivotShelf");
      this.btnCreateNewPackage = NameScopeExtensions.FindByName<Button>((Element) this, "btnCreateNewPackage");
      this.btnPickOrder = NameScopeExtensions.FindByName<Button>((Element) this, "btnPickOrder");
      this.btnShelfOrderSuccess = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelfOrderSuccess");
      this.lstShelfDetail = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfDetail");
      this.lblListHeader = NameScopeExtensions.FindByName<Label>((Element) this, "lblListHeader");
      this.loadingScreen = NameScopeExtensions.FindByName<ActivityIndicator>((Element) this, "loadingScreen");
    }
  }
}
