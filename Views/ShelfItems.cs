// Decompiled with JetBrains decompiler
// Type: Shelf.Views.ShelfItems
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Shelf.Controls;
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
  [XamlFilePath("Views\\ShelfItems.xaml")]
  public class ShelfItems : ContentPage
  {
    public pIOGetShelfWithMainReturnModel shelf;
    private List<pIOGetInventoryFromShelfIDReturnModel> shelfCountingList;
    private List<pIOGetInventoryFromShelfIDReturnModel> shelfCountingOriginalList;
    private ToolbarItem tItem;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage shelfSearch;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckMainShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtMainShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckBarcodeCount;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcodeCount;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckBarcodeType;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnShelfSync;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnShelfClear;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelfList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfList;

    public Color ButtonColor => Color.FromRgb(221, 130, 130);

    public Color TextColor => Color.White;

    public ShelfItems()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Raftaki Ürünler";
      ToolbarItem toolbarItem = new ToolbarItem();
      ((MenuItem) toolbarItem).Text = "";
      this.tItem = toolbarItem;
      ((MenuItem) this.tItem).Clicked += new EventHandler(this.TItem_Clicked);
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(this.tItem);
      GlobalMob.AddShelfBarcodeLongPress((Xamarin.Forms.Entry) this.txtBarcode);
      GlobalMob.FillBarcodeType(this.pckBarcodeType);
    }

    private void TItem_Clicked(object sender, EventArgs e)
    {
      string str = "";
      foreach (pIOGetInventoryFromShelfIDReturnModel shelfIdReturnModel in this.shelfCountingList.Where<pIOGetInventoryFromShelfIDReturnModel>((Func<pIOGetInventoryFromShelfIDReturnModel, bool>) (x =>
      {
        double readQty = x.ReadQty;
        double? inventoryQty = x.InventoryQty;
        double valueOrDefault = inventoryQty.GetValueOrDefault();
        return !(readQty == valueOrDefault & inventoryQty.HasValue);
      })).ToList<pIOGetInventoryFromShelfIDReturnModel>())
      {
        str = str + shelfIdReturnModel.ItemCode + "-" + shelfIdReturnModel.ColorCode + "-" + shelfIdReturnModel.ItemDim1Code + ":Miktar:" + shelfIdReturnModel.ReadQty.ToString() + "/" + shelfIdReturnModel.InventoryQty.ToString();
        str += "\n";
      }
      if (string.IsNullOrEmpty(str))
        ((Page) this).DisplayAlert("Hatalı Raflar", "Hatalı Raf Yok", "", "Tamam");
      else
        ((Page) this).DisplayAlert("Hatalı Raflar", str, "", "Tamam");
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
      this.BarcodeFocus(200);
    }

    private async void TxtBarcode_Completed(object sender, EventArgs e)
    {
      ShelfItems page = this;
      string barcode = ((InputView) page.txtBarcode).Text;
      if (string.IsNullOrEmpty(barcode))
      {
        barcode = (string) null;
      }
      else
      {
        await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
        page.shelf = new pIOGetShelfWithMainReturnModel();
        ReturnModel returnModel1 = GlobalMob.PostJson(string.Format("GetShelfWithMain?shelfCode={0}", (object) barcode), (ContentPage) page);
        if (returnModel1.Success && !string.IsNullOrEmpty(returnModel1.Result))
          page.shelf = GlobalMob.JsonDeserialize<pIOGetShelfWithMainReturnModel>(returnModel1.Result);
        if (page.shelf == null || page.shelf.ShelfID <= 0)
        {
          GlobalMob.PlayError();
          page.BarcodeFocus(200);
          int num = await ((Page) page).DisplayAlert("Hata", "Raf Bulunamadı", "", "Tamam") ? 1 : 0;
          GlobalMob.CloseLoading();
          barcode = (string) null;
        }
        else
        {
          ((VisualElement) page.stckMainShelf).IsVisible = GlobalMob.User.IsMainShelf;
          if (!string.IsNullOrEmpty(barcode))
          {
            ReturnModel returnModel2 = GlobalMob.PostJson(string.Format("GetInventoryFromShelfID?shelfID={0}", (object) page.shelf.ShelfID), (ContentPage) page);
            if (returnModel2.Success)
            {
              page.shelfCountingList = GlobalMob.JsonDeserialize<List<pIOGetInventoryFromShelfIDReturnModel>>(returnModel2.Result);
              page.shelfCountingOriginalList = JsonConvert.DeserializeObject<List<pIOGetInventoryFromShelfIDReturnModel>>(returnModel2.Result);
              ((ItemsView<Cell>) page.lstShelfList).ItemsSource = (IEnumerable) page.shelfCountingList;
              ((VisualElement) page.stckShelfList).IsVisible = true;
              ((VisualElement) page.stckBarcodeCount).IsVisible = true;
              ((InputView) page.txtMainShelf).Text = page.shelf.MainShelfCode;
              ((InputView) page.txtBarcode).Text = page.shelf.Code;
              page.SetInfo();
              if (GlobalMob.User.ShelfSyncVisible)
              {
                ((VisualElement) page.btnShelfSync).IsVisible = true;
                ((VisualElement) page.btnShelfClear).IsVisible = true;
              }
              if (page.shelfCountingList == null || page.shelfCountingList.Count == 0)
              {
                ((VisualElement) page.stckShelfList).IsVisible = false;
                ((VisualElement) page.stckBarcodeCount).IsVisible = false;
                GlobalMob.PlayError();
                int num = await ((Page) page).DisplayAlert("Hata", "Envanter Bulunamadı", "", "Tamam") ? 1 : 0;
              }
            }
            page.BarcodeCountFocus();
          }
          GlobalMob.CloseLoading();
          barcode = (string) null;
        }
      }
    }

    private void BarcodeFocus(int time) => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(time);
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode)?.Focus();
    }));

    private void BarcodeCountFocus() => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(200);
      ((InputView) this.txtBarcodeCount).Text = "";
      ((VisualElement) this.txtBarcodeCount)?.Focus();
    }));

    private void txtBarcodeCount_Completed(object sender, EventArgs e)
    {
      string barcode = ((InputView) this.txtBarcodeCount).Text;
      if (string.IsNullOrEmpty(barcode))
        return;
      bool flag = false;
      PickerItem selectedItem = (PickerItem) this.pckBarcodeType.SelectedItem;
      if (selectedItem != null && selectedItem.Code == 2 && ((VisualElement) this.pckBarcodeType).IsVisible)
        flag = true;
      if (flag)
      {
        ReturnModel returnModel = GlobalMob.PostJson("GetLotDetail?barcode=" + barcode, (ContentPage) this);
        if (!returnModel.Success)
          return;
        List<pIOGetLotDetailReturnModel> detailReturnModelList = GlobalMob.JsonDeserialize<List<pIOGetLotDetailReturnModel>>(returnModel.Result);
        if (detailReturnModelList == null || detailReturnModelList.Count <= 0)
        {
          ((Page) this).DisplayAlert("Bilgi", "Ürün bulunamadı", "", "Tamam");
          this.BarcodeCountFocus();
          GlobalMob.PlayError();
        }
        else
        {
          foreach (pIOGetLotDetailReturnModel detailReturnModel in detailReturnModelList)
          {
            pIOGetLotDetailReturnModel lotItem = detailReturnModel;
            pIOGetInventoryFromShelfIDReturnModel shelfIdReturnModel1 = this.shelfCountingList.Where<pIOGetInventoryFromShelfIDReturnModel>((Func<pIOGetInventoryFromShelfIDReturnModel, bool>) (x => x.Barcode == lotItem.Barcode)).FirstOrDefault<pIOGetInventoryFromShelfIDReturnModel>();
            if (shelfIdReturnModel1 != null)
            {
              shelfIdReturnModel1.ReadQty += lotItem.Qty;
              shelfIdReturnModel1.LastReadBarcode = true;
            }
            else
            {
              List<pIOGetInventoryFromShelfIDReturnModel> shelfCountingList = this.shelfCountingList;
              pIOGetInventoryFromShelfIDReturnModel shelfIdReturnModel2 = new pIOGetInventoryFromShelfIDReturnModel();
              shelfIdReturnModel2.AvailableInventoryQty = new double?(0.0);
              shelfIdReturnModel2.Barcode = lotItem.Barcode;
              shelfIdReturnModel2.ColorCode = lotItem.ColorCode;
              shelfIdReturnModel2.ColorDescription = lotItem.ColorDescription;
              shelfIdReturnModel2.InventoryQty = new double?(0.0);
              shelfIdReturnModel2.ItemCode = lotItem.ItemCode;
              shelfIdReturnModel2.ItemDescription = lotItem.ItemDescription;
              shelfIdReturnModel2.ItemDim1Code = lotItem.ItemDim1Code;
              shelfIdReturnModel2.ItemDim2Code = lotItem.ItemDim2Code;
              shelfIdReturnModel2.ItemDim3Code = lotItem.ItemDim3Code;
              byte? itemTypeCode = lotItem.ItemTypeCode;
              shelfIdReturnModel2.ItemTypeCode = itemTypeCode.HasValue ? new int?((int) itemTypeCode.GetValueOrDefault()) : new int?();
              shelfIdReturnModel2.LastReadBarcode = true;
              shelfIdReturnModel2.ReadQty = lotItem.Qty;
              shelfIdReturnModel2.RemainingOrderQty = new double?(0.0);
              shelfIdReturnModel2.ShelfCode = this.shelf.Code;
              shelfIdReturnModel2.ShelfID = this.shelf.ShelfID;
              shelfIdReturnModel2.WareHouseCode = this.shelf.WarehouseCode;
              shelfIdReturnModel2.UsedBarcode = lotItem.Barcode;
              shelfCountingList.Add(shelfIdReturnModel2);
            }
          }
          GlobalMob.PlaySave();
          this.SetInfo();
          this.RefreshData();
          this.BarcodeCountFocus();
        }
      }
      else
      {
        pIOGetInventoryFromShelfIDReturnModel shelfIdReturnModel = this.shelfCountingList.Where<pIOGetInventoryFromShelfIDReturnModel>((Func<pIOGetInventoryFromShelfIDReturnModel, bool>) (x => x.Barcode == barcode)).FirstOrDefault<pIOGetInventoryFromShelfIDReturnModel>();
        if (shelfIdReturnModel != null)
        {
          GlobalMob.PlaySave();
          ++shelfIdReturnModel.ReadQty;
          shelfIdReturnModel.LastReadBarcode = true;
          this.SetInfo();
          this.RefreshData();
          this.BarcodeCountFocus();
        }
        else
        {
          ReturnModel returnModel = GlobalMob.PostJson("GetItemFromBarcode?barcode=" + barcode, (ContentPage) this);
          if (!returnModel.Success)
            return;
          pIOValidateItemBarcodeReturnModel barcodeReturnModel = GlobalMob.JsonDeserialize<pIOValidateItemBarcodeReturnModel>(returnModel.Result);
          if (barcodeReturnModel != null)
          {
            this.shelfCountingList.Add(new pIOGetInventoryFromShelfIDReturnModel()
            {
              AvailableInventoryQty = new double?(0.0),
              Barcode = barcode,
              ColorCode = barcodeReturnModel.ColorCode,
              ColorDescription = barcodeReturnModel.ColorDescription,
              InventoryQty = new double?(0.0),
              ItemCode = barcodeReturnModel.ItemCode,
              ItemDescription = barcodeReturnModel.ItemDescription,
              ItemDim1Code = barcodeReturnModel.ItemDim1Code,
              ItemDim2Code = barcodeReturnModel.ItemDim2Code,
              ItemDim3Code = barcodeReturnModel.ItemDim3Code,
              ItemTypeCode = new int?((int) barcodeReturnModel.ItemTypeCode),
              LastReadBarcode = true,
              ReadQty = 1.0,
              RemainingOrderQty = new double?(0.0),
              ShelfCode = this.shelf.Code,
              ShelfID = this.shelf.ShelfID,
              WareHouseCode = this.shelf.WarehouseCode,
              UsedBarcode = barcode
            });
            GlobalMob.PlaySave();
            this.SetInfo();
            this.RefreshData();
            this.BarcodeCountFocus();
          }
          else
          {
            ((Page) this).DisplayAlert("Bilgi", "Ürün bulunamadı", "", "Tamam");
            this.BarcodeCountFocus();
            GlobalMob.PlayError();
          }
        }
      }
    }

    private void RefreshData()
    {
      this.shelfCountingList = this.shelfCountingList.OrderByDescending<pIOGetInventoryFromShelfIDReturnModel, bool>((Func<pIOGetInventoryFromShelfIDReturnModel, bool>) (x => x.LastReadBarcode)).ToList<pIOGetInventoryFromShelfIDReturnModel>();
      ((ItemsView<Cell>) this.lstShelfList).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstShelfList).ItemsSource = (IEnumerable) this.shelfCountingList;
    }

    private void SetInfo()
    {
      double? nullable = this.shelfCountingList.Sum<pIOGetInventoryFromShelfIDReturnModel>((Func<pIOGetInventoryFromShelfIDReturnModel, double?>) (x => x.InventoryQty));
      ((MenuItem) ((Page) this).ToolbarItems[0]).Text = this.shelfCountingList.Sum<pIOGetInventoryFromShelfIDReturnModel>((Func<pIOGetInventoryFromShelfIDReturnModel, double>) (x => x.ReadQty)).ToString() + "/" + nullable.ToString();
    }

    private async void btnShelfSync_Clicked(object sender, EventArgs e)
    {
      ShelfItems page = this;
      if (page.shelfCountingList.Where<pIOGetInventoryFromShelfIDReturnModel>((Func<pIOGetInventoryFromShelfIDReturnModel, bool>) (x => x.ReadQty > 0.0)).Count<pIOGetInventoryFromShelfIDReturnModel>() <= 0)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Hiç ürün okutulmadı", "", "Tamam") ? 1 : 0;
        GlobalMob.PlayError();
      }
      else
      {
        if (!await ((Page) page).DisplayAlert("Devam?", "Raftaki envanter okuttuğunuz ürünlerle eşitlenecek.Emin misiniz?", "Evet", "Hayır"))
          return;
        await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
        List<ShelfTransaction> shelfTransactionList = new List<ShelfTransaction>();
        foreach (pIOGetInventoryFromShelfIDReturnModel shelfIdReturnModel in page.shelfCountingList.Where<pIOGetInventoryFromShelfIDReturnModel>((Func<pIOGetInventoryFromShelfIDReturnModel, bool>) (x =>
        {
          double readQty = x.ReadQty;
          double? inventoryQty = x.InventoryQty;
          double valueOrDefault = inventoryQty.GetValueOrDefault();
          return !(readQty == valueOrDefault & inventoryQty.HasValue);
        })))
        {
          ShelfTransaction shelfTransaction1 = new ShelfTransaction();
          shelfTransaction1.ShelfID = page.shelf.ShelfID;
          shelfTransaction1.ProcessTypeID = 2;
          ShelfTransaction shelfTransaction2 = shelfTransaction1;
          double? inventoryQty = shelfIdReturnModel.InventoryQty;
          double readQty1 = shelfIdReturnModel.ReadQty;
          int num = Math.Abs(Convert.ToInt32((object) (inventoryQty.HasValue ? new double?(inventoryQty.GetValueOrDefault() - readQty1) : new double?())));
          shelfTransaction2.Qty = num;
          double readQty2 = shelfIdReturnModel.ReadQty;
          inventoryQty = shelfIdReturnModel.InventoryQty;
          double valueOrDefault = inventoryQty.GetValueOrDefault();
          if (readQty2 > valueOrDefault & inventoryQty.HasValue)
            shelfTransaction1.ProcessTypeID = 1;
          shelfTransaction1.WareHouseCode = page.shelf.WarehouseCode;
          shelfTransaction1.Barcode = shelfIdReturnModel.Barcode;
          shelfTransaction1.UserName = GlobalMob.User.UserName;
          shelfTransaction1.TransTypeID = 20;
          shelfTransaction1.DocumentNumber = "";
          shelfTransaction1.ItemCode = shelfIdReturnModel.ItemCode;
          shelfTransaction1.ColorCode = shelfIdReturnModel.ColorCode;
          shelfTransaction1.ItemDim1Code = shelfIdReturnModel.ItemDim1Code;
          shelfTransaction1.ItemDim2Code = shelfIdReturnModel.ItemDim2Code;
          shelfTransaction1.ItemDim3Code = shelfIdReturnModel.ItemDim3Code;
          shelfTransactionList.Add(shelfTransaction1);
        }
        if (shelfTransactionList.Count > 0)
        {
          ReturnModel result = GlobalMob.PostJson("TrInsertList", new Dictionary<string, string>()
          {
            {
              "json",
              JsonConvert.SerializeObject((object) shelfTransactionList)
            }
          }, (ContentPage) page).Result;
          if (!result.Success)
            return;
          ReturnModel returnModel = JsonConvert.DeserializeObject<ReturnModel>(result.Result);
          if (returnModel.Success)
          {
            ((IEntryController) page.txtBarcode).SendCompleted();
            GlobalMob.CloseLoading();
            int num = await ((Page) page).DisplayAlert("Bilgi", "Raf Eşitlendi", "", "Tamam") ? 1 : 0;
          }
          else
          {
            GlobalMob.CloseLoading();
            int num = await ((Page) page).DisplayAlert("Bilgi", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
          }
        }
        else
        {
          GlobalMob.CloseLoading();
          int num = await ((Page) page).DisplayAlert("Bilgi", "Eşitlenecek Ürün bulunamadı", "", "Tamam") ? 1 : 0;
          GlobalMob.PlayError();
        }
      }
    }

    private async void btnShelfClear_Clicked(object sender, EventArgs e)
    {
      ShelfItems page = this;
      if (page.shelfCountingOriginalList.Where<pIOGetInventoryFromShelfIDReturnModel>((Func<pIOGetInventoryFromShelfIDReturnModel, bool>) (x =>
      {
        double? remainingOrderQty = x.RemainingOrderQty;
        double num = 0.0;
        return remainingOrderQty.GetValueOrDefault() > num & remainingOrderQty.HasValue;
      })).Count<pIOGetInventoryFromShelfIDReturnModel>() > 0)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Bu rafa atanmış ve henüz toplanmamış emir bulunmaktadır.İşlem yapamazsınız", "", "Tamam") ? 1 : 0;
        GlobalMob.PlayError();
      }
      else
      {
        if (!await ((Page) page).DisplayAlert("Devam?", "Raftaki envanter sıfırlanacak.Emin misiniz?", "Evet", "Hayır"))
          return;
        await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
        List<ShelfTransaction> shelfTransactionList = new List<ShelfTransaction>();
        foreach (pIOGetInventoryFromShelfIDReturnModel countingOriginal in page.shelfCountingOriginalList)
        {
          ShelfTransaction shelfTransaction = new ShelfTransaction();
          shelfTransaction.ShelfID = page.shelf.ShelfID;
          shelfTransaction.ProcessTypeID = 2;
          shelfTransaction.Qty = Convert.ToInt32((object) countingOriginal.InventoryQty);
          if (shelfTransaction.Qty != 0)
          {
            if (shelfTransaction.Qty < 0)
            {
              shelfTransaction.ProcessTypeID = 1;
              shelfTransaction.Qty = -1 * shelfTransaction.Qty;
            }
            shelfTransaction.WareHouseCode = page.shelf.WarehouseCode;
            shelfTransaction.Barcode = countingOriginal.Barcode;
            shelfTransaction.UserName = GlobalMob.User.UserName;
            shelfTransaction.TransTypeID = 20;
            shelfTransaction.DocumentNumber = "";
            shelfTransaction.ItemCode = countingOriginal.ItemCode;
            shelfTransaction.ColorCode = countingOriginal.ColorCode;
            shelfTransaction.ItemDim1Code = countingOriginal.ItemDim1Code;
            shelfTransaction.ItemDim2Code = countingOriginal.ItemDim2Code;
            shelfTransaction.ItemDim3Code = countingOriginal.ItemDim3Code;
            shelfTransactionList.Add(shelfTransaction);
          }
        }
        if (shelfTransactionList.Count > 0)
        {
          ReturnModel result = GlobalMob.PostJson("TrInsertList", new Dictionary<string, string>()
          {
            {
              "json",
              JsonConvert.SerializeObject((object) shelfTransactionList)
            }
          }, (ContentPage) page).Result;
          if (!result.Success)
            return;
          ReturnModel returnModel = JsonConvert.DeserializeObject<ReturnModel>(result.Result);
          if (returnModel.Success)
          {
            page.shelfCountingList = new List<pIOGetInventoryFromShelfIDReturnModel>();
            page.shelfCountingOriginalList = new List<pIOGetInventoryFromShelfIDReturnModel>();
            page.RefreshData();
            page.BarcodeFocus(250);
            GlobalMob.CloseLoading();
            int num = await ((Page) page).DisplayAlert("Bilgi", "Raf Sıfırlandı", "", "Tamam") ? 1 : 0;
          }
          else
          {
            GlobalMob.CloseLoading();
            int num = await ((Page) page).DisplayAlert("Bilgi", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
          }
        }
        else
        {
          GlobalMob.CloseLoading();
          int num = await ((Page) page).DisplayAlert("Bilgi", "Sıfırlanacak Ürün bulunamadı", "", "Tamam") ? 1 : 0;
          GlobalMob.PlayError();
        }
      }
    }

    private void cmImage_Clicked(object sender, EventArgs e)
    {
      pIOGetInventoryFromShelfIDReturnModel commandParameter = (pIOGetInventoryFromShelfIDReturnModel) (sender as MenuItem).CommandParameter;
      ((NavigableElement) this).Navigation.PushAsync((Page) new ImagePage(commandParameter.Url, commandParameter.ItemDescription));
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (ShelfItems).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/ShelfItems.xaml",
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
        StackLayout stackLayout2 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry3 = new SoftkeyboardDisabledEntry();
        BindingExtension bindingExtension1 = new BindingExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        Picker picker = new Picker();
        StackLayout stackLayout3 = new StackLayout();
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension3 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension4 = new BindingExtension();
        Button button1 = new Button();
        ReferenceExtension referenceExtension3 = new ReferenceExtension();
        BindingExtension bindingExtension5 = new BindingExtension();
        ReferenceExtension referenceExtension4 = new ReferenceExtension();
        BindingExtension bindingExtension6 = new BindingExtension();
        Button button2 = new Button();
        StackLayout stackLayout4 = new StackLayout();
        BindingExtension bindingExtension7 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout5 = new StackLayout();
        StackLayout stackLayout6 = new StackLayout();
        ShelfItems shelfItems;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (shelfItems = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) shelfItems, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("shelfSearch", (object) shelfItems);
        if (((Element) shelfItems).StyleId == null)
          ((Element) shelfItems).StyleId = "shelfSearch";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("stckMainShelf", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckMainShelf";
        ((INameScope) nameScope).RegisterName("txtMainShelf", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtMainShelf";
        ((INameScope) nameScope).RegisterName("stckBarcodeCount", (object) stackLayout3);
        if (((Element) stackLayout3).StyleId == null)
          ((Element) stackLayout3).StyleId = "stckBarcodeCount";
        ((INameScope) nameScope).RegisterName("txtBarcodeCount", (object) softkeyboardDisabledEntry3);
        if (((Element) softkeyboardDisabledEntry3).StyleId == null)
          ((Element) softkeyboardDisabledEntry3).StyleId = "txtBarcodeCount";
        ((INameScope) nameScope).RegisterName("pckBarcodeType", (object) picker);
        if (((Element) picker).StyleId == null)
          ((Element) picker).StyleId = "pckBarcodeType";
        ((INameScope) nameScope).RegisterName("btnShelfSync", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnShelfSync";
        ((INameScope) nameScope).RegisterName("btnShelfClear", (object) button2);
        if (((Element) button2).StyleId == null)
          ((Element) button2).StyleId = "btnShelfClear";
        ((INameScope) nameScope).RegisterName("stckShelfList", (object) stackLayout5);
        if (((Element) stackLayout5).StyleId == null)
          ((Element) stackLayout5).StyleId = "stckShelfList";
        ((INameScope) nameScope).RegisterName("lstShelfList", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstShelfList";
        this.shelfSearch = (ContentPage) shelfItems;
        this.stckForm = stackLayout4;
        this.txtBarcode = softkeyboardDisabledEntry1;
        this.stckMainShelf = stackLayout2;
        this.txtMainShelf = softkeyboardDisabledEntry2;
        this.stckBarcodeCount = stackLayout3;
        this.txtBarcodeCount = softkeyboardDisabledEntry3;
        this.pckBarcodeType = picker;
        this.btnShelfSync = button1;
        this.btnShelfClear = button2;
        this.stckShelfList = stackLayout5;
        this.lstShelfList = listView;
        ((BindableObject) stackLayout6).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout6).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout4).SetValue(StackLayout.SpacingProperty, (object) 20.0);
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf Okutunuz");
        softkeyboardDisabledEntry1.Completed += new EventHandler(shelfItems.TxtBarcode_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 18);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Ana Raf");
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout3).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
        ((BindableObject) softkeyboardDisabledEntry3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) softkeyboardDisabledEntry3).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Ürün Okutunuz (Sayım İçin)");
        softkeyboardDisabledEntry3.Completed += new EventHandler(shelfItems.txtBarcodeCount_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) softkeyboardDisabledEntry3);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry3, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 22);
        ((BindableObject) picker).SetValue(Picker.TitleProperty, (object) "Tip");
        bindingExtension1.Path = ".";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker).SetBinding(Picker.ItemsSourceProperty, bindingBase1);
        bindingExtension2.Path = "Caption";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        picker.ItemDisplayBinding = bindingBase2;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase2, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 33);
        ((BindableObject) picker).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) picker).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) picker);
        VisualDiagnostics.RegisterSourceInfo((object) picker, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 19, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 18);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "Rafı Eşitle");
        button1.Clicked += new EventHandler(shelfItems.btnShelfSync_Clicked);
        ((BindableObject) button1).SetValue(View.MarginProperty, (object) new Thickness(5.0));
        referenceExtension1.Name = "shelfSearch";
        ReferenceExtension referenceExtension5 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 5];
        objArray1[0] = (object) bindingExtension3;
        objArray1[1] = (object) button1;
        objArray1[2] = (object) stackLayout4;
        objArray1[3] = (object) stackLayout6;
        objArray1[4] = (object) shelfItems;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (ShelfItems).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(25, 21)));
        object obj2 = ((IMarkupExtension) referenceExtension5).ProvideValue((IServiceProvider) xamlServiceProvider1);
        bindingExtension3.Source = obj2;
        VisualDiagnostics.RegisterSourceInfo(obj2, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 25, 21);
        bindingExtension3.Path = "ButtonColor";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase3);
        referenceExtension2.Name = "shelfSearch";
        ReferenceExtension referenceExtension6 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 5];
        objArray2[0] = (object) bindingExtension4;
        objArray2[1] = (object) button1;
        objArray2[2] = (object) stackLayout4;
        objArray2[3] = (object) stackLayout6;
        objArray2[4] = (object) shelfItems;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray2, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver2.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (ShelfItems).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(26, 21)));
        object obj4 = ((IMarkupExtension) referenceExtension6).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension4.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 26, 21);
        bindingExtension4.Path = "TextColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(Button.TextColorProperty, bindingBase4);
        ((BindableObject) button1).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 18);
        ((BindableObject) button2).SetValue(Button.TextProperty, (object) "Rafı Sıfırla");
        button2.Clicked += new EventHandler(shelfItems.btnShelfClear_Clicked);
        ((BindableObject) button2).SetValue(View.MarginProperty, (object) new Thickness(5.0));
        referenceExtension3.Name = "shelfSearch";
        ReferenceExtension referenceExtension7 = referenceExtension3;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 5];
        objArray3[0] = (object) bindingExtension5;
        objArray3[1] = (object) button2;
        objArray3[2] = (object) stackLayout4;
        objArray3[3] = (object) stackLayout6;
        objArray3[4] = (object) shelfItems;
        SimpleValueTargetProvider valueTargetProvider3;
        object obj5 = (object) (valueTargetProvider3 = new SimpleValueTargetProvider(objArray3, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider3.Add(type5, (object) valueTargetProvider3);
        xamlServiceProvider3.Add(typeof (IReferenceProvider), obj5);
        Type type6 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver3 = new XmlNamespaceResolver();
        namespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver3.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (ShelfItems).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(30, 21)));
        object obj6 = ((IMarkupExtension) referenceExtension7).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension5.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 30, 21);
        bindingExtension5.Path = "ButtonColor";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase5);
        referenceExtension4.Name = "shelfSearch";
        ReferenceExtension referenceExtension8 = referenceExtension4;
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 5];
        objArray4[0] = (object) bindingExtension6;
        objArray4[1] = (object) button2;
        objArray4[2] = (object) stackLayout4;
        objArray4[3] = (object) stackLayout6;
        objArray4[4] = (object) shelfItems;
        SimpleValueTargetProvider valueTargetProvider4;
        object obj7 = (object) (valueTargetProvider4 = new SimpleValueTargetProvider(objArray4, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider4.Add(type7, (object) valueTargetProvider4);
        xamlServiceProvider4.Add(typeof (IReferenceProvider), obj7);
        Type type8 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver4 = new XmlNamespaceResolver();
        namespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver4.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (ShelfItems).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(31, 21)));
        object obj8 = ((IMarkupExtension) referenceExtension8).ProvideValue((IServiceProvider) xamlServiceProvider4);
        bindingExtension6.Source = obj8;
        VisualDiagnostics.RegisterSourceInfo(obj8, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 31, 21);
        bindingExtension6.Path = "TextColor";
        BindingBase bindingBase6 = ((IMarkupExtension<BindingBase>) bindingExtension6).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(Button.TextColorProperty, bindingBase6);
        ((BindableObject) button2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) button2);
        VisualDiagnostics.RegisterSourceInfo((object) button2, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 9, 14);
        ((BindableObject) stackLayout5).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout5).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) listView).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 800.0);
        bindingExtension7.Path = ".";
        BindingBase bindingBase7 = ((IMarkupExtension<BindingBase>) bindingExtension7).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase7);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        ((BindableObject) listView).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ShelfItems.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_33 xamlCdataTemplate33 = new ShelfItems.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_33();
        object[] objArray5 = new object[0 + 5];
        objArray5[0] = (object) dataTemplate1;
        objArray5[1] = (object) listView;
        objArray5[2] = (object) stackLayout5;
        objArray5[3] = (object) stackLayout6;
        objArray5[4] = (object) shelfItems;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate33.parentValues = objArray5;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate33.root = shelfItems;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate33.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 39, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 35, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 34, 14);
        ((BindableObject) shelfItems).SetValue(ContentPage.ContentProperty, (object) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 8, 10);
        VisualDiagnostics.RegisterSourceInfo((object) shelfItems, new Uri("Views\\ShelfItems.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<ShelfItems>(this, typeof (ShelfItems));
      this.shelfSearch = NameScopeExtensions.FindByName<ContentPage>((Element) this, "shelfSearch");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.stckMainShelf = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckMainShelf");
      this.txtMainShelf = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtMainShelf");
      this.stckBarcodeCount = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcodeCount");
      this.txtBarcodeCount = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcodeCount");
      this.pckBarcodeType = NameScopeExtensions.FindByName<Picker>((Element) this, "pckBarcodeType");
      this.btnShelfSync = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelfSync");
      this.btnShelfClear = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelfClear");
      this.stckShelfList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfList");
      this.lstShelfList = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfList");
    }
  }
}
