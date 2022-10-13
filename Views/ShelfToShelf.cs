// Decompiled with JetBrains decompiler
// Type: Shelf.Views.ShelfToShelf
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
  [XamlFilePath("Views\\ShelfToShelf.xaml")]
  public class ShelfToShelf : ContentPage
  {
    private ztIOShelf oldShelf;
    private ztIOShelf newShelf;
    private List<pIOGetInventoryFromShelfIDReturnModel> list;
    public ShelfList pageShelf;
    public ztIOShelf mkShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage shelftToShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckContent;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckDocumentNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtDocumentNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnCreateDocumentNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnClearDocumentNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtToShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnCreateShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtQty;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckBarcodeType;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnShelfOrderSuccess;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnShelfChangeSelect;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelfList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfList;

    public Color ButtonColor => Color.FromRgb(52, 203, 201);

    public Color TextColor => Color.White;

    public ztIOShelf selectShelf { get; set; }

    public ShelfToShelf()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Raf Taşı";
      ((ICollection<Effect>) ((Element) this.txtToShelf).Effects).Add((Effect) new LongPressedEffect());
      LongPressedEffect.SetCommand((BindableObject) this.txtToShelf, this.LongPressToShelf);
      GlobalMob.AddShelfBarcodeLongPress((Xamarin.Forms.Entry) this.txtShelf);
      this.list = new List<pIOGetInventoryFromShelfIDReturnModel>();
      ((VisualElement) this.stckBarcode).IsVisible = GlobalMob.User.PieceTransfer;
      ((VisualElement) this.btnCreateShelf).IsVisible = GlobalMob.User.IsMainShelf;
      GlobalMob.FillBarcodeType(this.pckBarcodeType);
      this.ShelfFocus();
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

    private ICommand LongPressToShelf => (ICommand) new Command((Action) (async () =>
    {
      ShelfToShelf shelfToShelf = this;
      shelfToShelf.pageShelf = new ShelfList();
      shelfToShelf.pageShelf.ShelfSelectedItem += new EventHandler(shelfToShelf.PageShelf_ShelfSelectedItem);
      await ((NavigableElement) shelfToShelf).Navigation.PushAsync((Page) shelfToShelf.pageShelf);
    }));

    private void PageShelf_ShelfSelectedItem(object sender, EventArgs e)
    {
      pIOGetShelfFromTextReturnModel selectedShelf = this.pageShelf.selectedShelf;
      if (!string.IsNullOrEmpty(selectedShelf.Code))
      {
        ((InputView) this.txtToShelf).Text = selectedShelf.Code;
        ((IEntryController) this.txtToShelf).SendCompleted();
      }
      else
        ((Page) GlobalMob.currentPage).DisplayAlert("Bilgi", "Raf tanımlı değil", "", "Tamam");
    }

    private async void ShelfFocus()
    {
      await Task.Delay(250);
      ((VisualElement) this.txtShelf)?.Focus();
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
    }

    private void BarcodeFocus() => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(250);
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode)?.Focus();
    }));

    private void ShelfBarcodeFocus() => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(250);
      ((InputView) this.txtShelf).Text = "";
      ((VisualElement) this.txtShelf)?.Focus();
    }));

    private void ToShelfBarcodeFocus() => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(250);
      ((InputView) this.txtToShelf).Text = "";
      ((VisualElement) this.txtToShelf)?.Focus();
    }));

    private async void btnShelfOrderSuccess_Clicked(object sender, EventArgs e)
    {
      ShelfToShelf page = this;
      if (page.oldShelf == null)
      {
        int num1 = await ((Page) page).DisplayAlert("Bilgi", "Kaynak Raf Okutunuz", "", "Tamam") ? 1 : 0;
      }
      else if (page.newShelf == null)
      {
        int num2 = await ((Page) page).DisplayAlert("Bilgi", "Hedef Raf Okutunuz", "", "Tamam") ? 1 : 0;
      }
      else
      {
        if (!await ((Page) page).DisplayAlert("Devam?", "Tüm ürünleri taşımak istediğinize emin misiniz?", "Evet", "Hayır"))
          return;
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("ChangeShelfInventory?oldShelfID={0}&newShelfID={1}&userName={2}&documentNumber={3}", (object) page.oldShelf.ShelfID, (object) page.newShelf.ShelfID, (object) GlobalMob.User.UserName, (object) ((InputView) page.txtDocumentNumber).Text), (ContentPage) page);
        if (!returnModel.Success)
          return;
        int num3 = JsonConvert.DeserializeObject<bool>(returnModel.Result) ? 1 : 0;
        string str = "Bir hata oluştu";
        if (num3 != 0)
          str = "Rafdaki envanter (" + page.oldShelf.Code + ") yeni rafa (" + page.newShelf.Code + ") taşındı";
        int num4 = await ((Page) page).DisplayAlert("Bilgi", str, "", "Tamam") ? 1 : 0;
        ((InputView) page.txtShelf).Text = "";
        ((InputView) page.txtToShelf).Text = "";
        ((ItemsView<Cell>) page.lstShelfList).ItemsSource = (IEnumerable) null;
        page.oldShelf = (ztIOShelf) null;
        page.newShelf = (ztIOShelf) null;
        ((VisualElement) page.txtShelf).Focus();
      }
    }

    private async void txtShelf_Completed(object sender, EventArgs e)
    {
      ShelfToShelf page = this;
      if (string.IsNullOrEmpty(((InputView) page.txtShelf).Text))
        return;
      ReturnModel shelf = GlobalMob.GetShelf(((InputView) page.txtShelf).Text, (ContentPage) page);
      if (!shelf.Success)
        return;
      page.oldShelf = JsonConvert.DeserializeObject<ztIOShelf>(shelf.Result);
      if (page.oldShelf != null)
      {
        if (page.newShelf != null && page.newShelf.ShelfID == page.oldShelf.ShelfID)
        {
          int num = await ((Page) page).DisplayAlert("Bilgi", "Kaynak Raf Hedef Rafla Aynı Olamaz", "", "Tamam") ? 1 : 0;
          page.oldShelf = (ztIOShelf) null;
          ((InputView) page.txtShelf).Text = "";
          page.ShelfFocus();
        }
        else
        {
          ((InputView) page.txtShelf).Text = page.oldShelf.Code;
          ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetInventoryFromShelfID?shelfID={0}&isUnique={1}", (object) page.oldShelf.ShelfID, (object) GlobalMob.User.IsUniqueBarcode), (ContentPage) page);
          if (returnModel.Success)
          {
            page.list = GlobalMob.JsonDeserialize<List<pIOGetInventoryFromShelfIDReturnModel>>(returnModel.Result);
            ((ItemsView<Cell>) page.lstShelfList).ItemsSource = (IEnumerable) page.list;
          }
          page.ToShelfFocus();
        }
      }
      else
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Raf Bulunamadı", "", "Tamam") ? 1 : 0;
        ((InputView) page.txtShelf).Text = "";
        page.ShelfFocus();
      }
    }

    private async void txtToShelf_Completed(object sender, EventArgs e)
    {
      ShelfToShelf page = this;
      if (string.IsNullOrEmpty(((InputView) page.txtToShelf).Text))
        return;
      ReturnModel shelf = GlobalMob.GetShelf(((InputView) page.txtToShelf).Text, (ContentPage) page);
      if (!shelf.Success)
        return;
      page.newShelf = JsonConvert.DeserializeObject<ztIOShelf>(shelf.Result);
      if (page.newShelf != null)
      {
        if (page.oldShelf != null && page.newShelf.ShelfID == page.oldShelf.ShelfID)
        {
          int num = await ((Page) page).DisplayAlert("Bilgi", "Hedef Rafla Kaynak Raf Aynı Olamaz", "", "Tamam") ? 1 : 0;
          page.newShelf = (ztIOShelf) null;
          page.ToShelfFocus();
        }
        else
        {
          ((InputView) page.txtToShelf).Text = page.newShelf.Code;
          ((VisualElement) page.txtBarcode).Focus();
        }
      }
      else
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Raf Bulunamadı", "", "Tamam") ? 1 : 0;
        page.ToShelfFocus();
      }
    }

    private async void ToShelfFocus()
    {
      await Task.Delay(250);
      ((InputView) this.txtToShelf).Text = "";
      ((VisualElement) this.txtToShelf).Focus();
    }

    private void cmImage_Clicked(object sender, EventArgs e)
    {
      pIOGetInventoryFromShelfIDReturnModel commandParameter = (pIOGetInventoryFromShelfIDReturnModel) (sender as MenuItem).CommandParameter;
      ((NavigableElement) this).Navigation.PushAsync((Page) new ImagePage(commandParameter.Url, commandParameter.ItemDescription));
    }

    private async void btnShelfChangeSelect_Clicked(object sender, EventArgs e)
    {
      ShelfToShelf page = this;
      if (page.oldShelf == null)
      {
        int num1 = await ((Page) page).DisplayAlert("Bilgi", "Kaynak Raf Okutunuz", "", "Tamam") ? 1 : 0;
      }
      else if (page.newShelf == null)
      {
        int num2 = await ((Page) page).DisplayAlert("Bilgi", "Hedef Raf Okutunuz", "", "Tamam") ? 1 : 0;
      }
      else if (page.list.Where<pIOGetInventoryFromShelfIDReturnModel>((Func<pIOGetInventoryFromShelfIDReturnModel, bool>) (x => x.IsSelected)).Count<pIOGetInventoryFromShelfIDReturnModel>() <= 0)
      {
        int num3 = await ((Page) page).DisplayAlert("Bilgi", "Lütfen öncelikle taşınacak ürünleri seçiniz", "", "Tamam") ? 1 : 0;
      }
      else
      {
        if (!await ((Page) page).DisplayAlert("Devam?", "Seçili ürünleri taşımak istediğinize emin misiniz?", "Evet", "Hayır"))
          return;
        await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
        List<ChangeShelf> changeShelfList = new List<ChangeShelf>();
        foreach (pIOGetInventoryFromShelfIDReturnModel shelfIdReturnModel in page.list.Where<pIOGetInventoryFromShelfIDReturnModel>((Func<pIOGetInventoryFromShelfIDReturnModel, bool>) (x => x.IsSelected)))
          changeShelfList.Add(new ChangeShelf()
          {
            DocumentNumber = ((InputView) page.txtDocumentNumber).Text,
            Barcode = shelfIdReturnModel.Barcode,
            ColorCode = shelfIdReturnModel.ColorCode,
            ItemCode = shelfIdReturnModel.ItemCode,
            ItemDim1Code = shelfIdReturnModel.ItemDim1Code,
            ItemDim2Code = shelfIdReturnModel.ItemDim2Code,
            ItemDim3Code = shelfIdReturnModel.ItemDim3Code,
            NewShelfID = page.newShelf.ShelfID,
            OldShelfID = page.oldShelf.ShelfID,
            Username = GlobalMob.User.UserName
          });
        ReturnModel result = GlobalMob.PostJson("ChangeShelfInventorySelected", new Dictionary<string, string>()
        {
          {
            "json",
            JsonConvert.SerializeObject((object) changeShelfList)
          }
        }, (ContentPage) page).Result;
        if (result.Success)
        {
          if (JsonConvert.DeserializeObject<bool>(result.Result))
          {
            int num4 = await ((Page) page).DisplayAlert("Tamam", "Seçili ürünler taşındı.", "", "Tamam") ? 1 : 0;
            ((InputView) page.txtShelf).Text = "";
            ((InputView) page.txtToShelf).Text = "";
            ((ItemsView<Cell>) page.lstShelfList).ItemsSource = (IEnumerable) null;
            page.oldShelf = (ztIOShelf) null;
            page.newShelf = (ztIOShelf) null;
            ((VisualElement) page.txtShelf).Focus();
          }
          else
          {
            int num5 = await ((Page) page).DisplayAlert("Hata", "Hata", "", "Tamam") ? 1 : 0;
          }
        }
        GlobalMob.CloseLoading();
      }
    }

    private bool IsUniqueControl(string barcode)
    {
      pIOGetInventoryFromShelfIDReturnModel shelfIdReturnModel = this.list.Where<pIOGetInventoryFromShelfIDReturnModel>((Func<pIOGetInventoryFromShelfIDReturnModel, bool>) (x => x.UsedBarcode.Contains(barcode) || x.Barcode.Contains(barcode))).FirstOrDefault<pIOGetInventoryFromShelfIDReturnModel>();
      return shelfIdReturnModel != null && shelfIdReturnModel.UseSerialNumber;
    }

    private async void txtBarcode_Completed(object sender, EventArgs e)
    {
      ShelfToShelf page = this;
      try
      {
        string barcode = ((InputView) page.txtBarcode).Text;
        if (string.IsNullOrEmpty(barcode))
        {
          page.BarcodeFocus();
          return;
        }
        if (barcode.Length < GlobalMob.User.MinimumBarcodeLength)
        {
          page.BarcodeFocus();
          return;
        }
        if (page.oldShelf == null)
        {
          int num = await ((Page) page).DisplayAlert("Bilgi", "Kaynak Raf Okutunuz", "", "Tamam") ? 1 : 0;
          page.ShelfBarcodeFocus();
          return;
        }
        if (page.newShelf == null)
        {
          int num = await ((Page) page).DisplayAlert("Bilgi", "Hedef Raf Okutunuz", "", "Tamam") ? 1 : 0;
          page.ToShelfBarcodeFocus();
          return;
        }
        if (barcode.Length < GlobalMob.User.MinimumBarcodeLength)
        {
          page.BarcodeFocus();
          return;
        }
        if (page.GetQty() <= 0)
        {
          int num = await ((Page) page).DisplayAlert("Hata", "Lütfen geçerli bir miktar giriniz", "", "Tamam") ? 1 : 0;
          ((InputView) page.txtBarcode).Text = "";
          ((VisualElement) page.txtQty).Focus();
          return;
        }
        PickerItem selectedItem = (PickerItem) page.pckBarcodeType.SelectedItem;
        bool isLot = false;
        if (selectedItem != null && selectedItem.Code == 2 && ((VisualElement) page.pckBarcodeType).IsVisible)
          isLot = true;
        // ISSUE: reference to a compiler-generated method
        pIOGetInventoryFromShelfIDReturnModel findItem = page.list.Where<pIOGetInventoryFromShelfIDReturnModel>(new Func<pIOGetInventoryFromShelfIDReturnModel, bool>(page.\u003CtxtBarcode_Completed\u003Eb__30_0)).FirstOrDefault<pIOGetInventoryFromShelfIDReturnModel>();
        if (findItem == null && !isLot)
        {
          int num = await ((Page) page).DisplayAlert("Hata", "Ürün bulunamadı", "", "Tamam") ? 1 : 0;
          page.BarcodeFocus();
          return;
        }
        bool isUnique = findItem.UseSerialNumber;
        if (isLot)
        {
          barcode = ((InputView) page.txtBarcode).Text;
          findItem = page.list[0];
        }
        else
          barcode = findItem.Barcode;
        await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
        ChangeShelf shelfItem = new ChangeShelf()
        {
          Barcode = barcode,
          DocumentNumber = ((InputView) page.txtDocumentNumber).Text,
          ColorCode = findItem.ColorCode,
          ItemCode = findItem.ItemCode,
          ItemDim1Code = findItem.ItemDim1Code,
          ItemDim2Code = findItem.ItemDim2Code,
          ItemDim3Code = findItem.ItemDim3Code,
          NewShelfID = page.newShelf.ShelfID,
          OldShelfID = page.oldShelf.ShelfID,
          Username = GlobalMob.User.UserName,
          MKShelfID = GlobalMob.User.MKShelfID,
          Qty = Convert.ToInt32(((InputView) page.txtQty).Text)
        };
        string str1 = JsonConvert.SerializeObject((object) shelfItem);
        Dictionary<string, string> paramList = new Dictionary<string, string>();
        paramList.Add("json", str1);
        string url = isLot ? "ChangeShelfInventoryLotBarcode" : "ChangeShelfInventoryBarcode";
        if (isUnique)
          url = "ChangeShelfInventoryUniqueBarcode";
        ReturnModel result = GlobalMob.PostJson(url, paramList, (ContentPage) page).Result;
        if (result.Success)
        {
          ReturnModel returnModel = JsonConvert.DeserializeObject<ReturnModel>(result.Result);
          if (returnModel != null)
          {
            if (!returnModel.Success && returnModel.ErrorMessage == "-1")
            {
              GlobalMob.CloseLoading();
              bool action = await ((Page) page).DisplayAlert("Onay?", "Bu rafa atanmış raf emirleri mevcuttur.Onları da taşımak istiyor musunuz?", "Evet", "Hayır");
              await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
              shelfItem.ShelfChangeControl = true;
              shelfItem.NotIsShelfChange = !action;
              string str2 = JsonConvert.SerializeObject((object) shelfItem);
              returnModel = JsonConvert.DeserializeObject<ReturnModel>(GlobalMob.PostJson(url, new Dictionary<string, string>()
              {
                {
                  "json",
                  str2
                }
              }, (ContentPage) page).Result.Result);
            }
            if (returnModel.Success)
            {
              GlobalMob.PlaySave();
              if (isLot)
              {
                foreach (pIOGetLotDetailReturnModel detailReturnModel in JsonConvert.DeserializeObject<List<pIOGetLotDetailReturnModel>>(returnModel.Result))
                {
                  pIOGetLotDetailReturnModel item = detailReturnModel;
                  pIOGetInventoryFromShelfIDReturnModel shelfIdReturnModel1 = page.list.Where<pIOGetInventoryFromShelfIDReturnModel>((Func<pIOGetInventoryFromShelfIDReturnModel, bool>) (x => x.Barcode.Contains(item.Barcode))).FirstOrDefault<pIOGetInventoryFromShelfIDReturnModel>();
                  if (shelfIdReturnModel1 != null)
                  {
                    pIOGetInventoryFromShelfIDReturnModel shelfIdReturnModel2 = shelfIdReturnModel1;
                    double? inventoryQty = shelfIdReturnModel2.InventoryQty;
                    double qty = item.Qty;
                    shelfIdReturnModel2.InventoryQty = inventoryQty.HasValue ? new double?(inventoryQty.GetValueOrDefault() - qty) : new double?();
                  }
                }
              }
              else
              {
                pIOGetInventoryFromShelfIDReturnModel shelfIdReturnModel = findItem;
                double? inventoryQty = shelfIdReturnModel.InventoryQty;
                double qty = (double) shelfItem.Qty;
                shelfIdReturnModel.InventoryQty = inventoryQty.HasValue ? new double?(inventoryQty.GetValueOrDefault() - qty) : new double?();
              }
              ((ItemsView<Cell>) page.lstShelfList).ItemsSource = (IEnumerable) null;
              ((ItemsView<Cell>) page.lstShelfList).ItemsSource = (IEnumerable) page.list.Where<pIOGetInventoryFromShelfIDReturnModel>((Func<pIOGetInventoryFromShelfIDReturnModel, bool>) (x =>
              {
                double? inventoryQty = x.InventoryQty;
                double num = 0.0;
                return inventoryQty.GetValueOrDefault() > num & inventoryQty.HasValue;
              })).ToList<pIOGetInventoryFromShelfIDReturnModel>();
              ((InputView) page.txtQty).Text = "1";
            }
            else
            {
              int num = await ((Page) page).DisplayAlert("Uyarı", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
              GlobalMob.PlayError();
            }
            page.BarcodeFocus();
          }
          else
          {
            int num = await ((Page) page).DisplayAlert("Hata", "Hata", "", "Tamam") ? 1 : 0;
            page.BarcodeFocus();
          }
        }
        GlobalMob.CloseLoading();
        barcode = (string) null;
        findItem = (pIOGetInventoryFromShelfIDReturnModel) null;
        shelfItem = (ChangeShelf) null;
        url = (string) null;
      }
      catch (Exception ex)
      {
        int num = await ((Page) page).DisplayAlert("Hata", ex.ToString(), "", "Tamam") ? 1 : 0;
      }
    }

    private int GetQty()
    {
      int qty;
      try
      {
        qty = Convert.ToInt32(((InputView) this.txtQty).Text);
        if (qty <= 0)
          return 0;
      }
      catch (Exception ex)
      {
        qty = 0;
      }
      return qty;
    }

    private void btnCreateDocumentNumber_Clicked(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(((InputView) this.txtDocumentNumber).Text))
        ((InputView) this.txtDocumentNumber).Text = ((InputView) this.txtDocumentNumber).Text + "_";
      Xamarin.Forms.Entry txtDocumentNumber = this.txtDocumentNumber;
      ((InputView) txtDocumentNumber).Text = ((InputView) txtDocumentNumber).Text + this.GetLastDocumentNumber();
      ((VisualElement) this.txtShelf).Focus();
    }

    private string GetLastDocumentNumber()
    {
      int num = 0;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetLastDocumentNumber?transTypeID={0}", (object) 9), (ContentPage) this);
      if (returnModel.Success && !string.IsNullOrEmpty(returnModel.Result))
        num = JsonConvert.DeserializeObject<int>(returnModel.Result) + 1;
      return "D" + Convert.ToString(num);
    }

    private void btnClearDocumentNumber_Clicked(object sender, EventArgs e) => ((InputView) this.txtDocumentNumber).Text = "";

    private async void btnCreateShelf_Clicked(object sender, EventArgs e)
    {
      ShelfToShelf page = this;
      if (page.mkShelf == null)
        page.GetMKShelf();
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
            ZoneID = page.mkShelf.ZoneID,
            SortOrder = new int?(0),
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
            page.selectShelf = JsonConvert.DeserializeObject<ztIOShelf>(result.Result);
            if (page.selectShelf != null)
            {
              ((InputView) page.txtToShelf).Text = page.selectShelf.Code;
              page.txtToShelf_Completed((object) null, (EventArgs) null);
            }
            else
            {
              ((InputView) page.txtShelf).Text = "";
              int num = await ((Page) page).DisplayAlert("Hata", "Bu kod zaten tanımlı:" + pCode, "", "Tamam") ? 1 : 0;
            }
          }
          GlobalMob.CloseLoading();
          pCode = (string) null;
          newShelf = (ztIOShelf) null;
        }
      }
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (ShelfToShelf).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/ShelfToShelf.xaml",
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
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry1 = new SoftkeyboardDisabledEntry();
        StackLayout stackLayout2 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry2 = new SoftkeyboardDisabledEntry();
        ReferenceExtension referenceExtension5 = new ReferenceExtension();
        BindingExtension bindingExtension5 = new BindingExtension();
        ReferenceExtension referenceExtension6 = new ReferenceExtension();
        BindingExtension bindingExtension6 = new BindingExtension();
        Button button3 = new Button();
        StackLayout stackLayout3 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry3 = new SoftkeyboardDisabledEntry();
        Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
        BindingExtension bindingExtension7 = new BindingExtension();
        BindingExtension bindingExtension8 = new BindingExtension();
        Picker picker = new Picker();
        StackLayout stackLayout4 = new StackLayout();
        ReferenceExtension referenceExtension7 = new ReferenceExtension();
        BindingExtension bindingExtension9 = new BindingExtension();
        ReferenceExtension referenceExtension8 = new ReferenceExtension();
        BindingExtension bindingExtension10 = new BindingExtension();
        Button button4 = new Button();
        StackLayout stackLayout5 = new StackLayout();
        ReferenceExtension referenceExtension9 = new ReferenceExtension();
        BindingExtension bindingExtension11 = new BindingExtension();
        ReferenceExtension referenceExtension10 = new ReferenceExtension();
        BindingExtension bindingExtension12 = new BindingExtension();
        Button button5 = new Button();
        StackLayout stackLayout6 = new StackLayout();
        StackLayout stackLayout7 = new StackLayout();
        BindingExtension bindingExtension13 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout8 = new StackLayout();
        StackLayout stackLayout9 = new StackLayout();
        ShelfToShelf shelfToShelf;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (shelfToShelf = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) shelfToShelf, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("shelftToShelf", (object) shelfToShelf);
        if (((Element) shelfToShelf).StyleId == null)
          ((Element) shelfToShelf).StyleId = "shelftToShelf";
        ((INameScope) nameScope).RegisterName("stckContent", (object) stackLayout7);
        if (((Element) stackLayout7).StyleId == null)
          ((Element) stackLayout7).StyleId = "stckContent";
        ((INameScope) nameScope).RegisterName("stckDocumentNumber", (object) stackLayout1);
        if (((Element) stackLayout1).StyleId == null)
          ((Element) stackLayout1).StyleId = "stckDocumentNumber";
        ((INameScope) nameScope).RegisterName("txtDocumentNumber", (object) entry1);
        if (((Element) entry1).StyleId == null)
          ((Element) entry1).StyleId = "txtDocumentNumber";
        ((INameScope) nameScope).RegisterName("btnCreateDocumentNumber", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnCreateDocumentNumber";
        ((INameScope) nameScope).RegisterName("btnClearDocumentNumber", (object) button2);
        if (((Element) button2).StyleId == null)
          ((Element) button2).StyleId = "btnClearDocumentNumber";
        ((INameScope) nameScope).RegisterName("txtShelf", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtShelf";
        ((INameScope) nameScope).RegisterName("txtToShelf", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtToShelf";
        ((INameScope) nameScope).RegisterName("btnCreateShelf", (object) button3);
        if (((Element) button3).StyleId == null)
          ((Element) button3).StyleId = "btnCreateShelf";
        ((INameScope) nameScope).RegisterName("stckBarcode", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckBarcode";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry3);
        if (((Element) softkeyboardDisabledEntry3).StyleId == null)
          ((Element) softkeyboardDisabledEntry3).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("txtQty", (object) entry2);
        if (((Element) entry2).StyleId == null)
          ((Element) entry2).StyleId = "txtQty";
        ((INameScope) nameScope).RegisterName("pckBarcodeType", (object) picker);
        if (((Element) picker).StyleId == null)
          ((Element) picker).StyleId = "pckBarcodeType";
        ((INameScope) nameScope).RegisterName("btnShelfOrderSuccess", (object) button4);
        if (((Element) button4).StyleId == null)
          ((Element) button4).StyleId = "btnShelfOrderSuccess";
        ((INameScope) nameScope).RegisterName("btnShelfChangeSelect", (object) button5);
        if (((Element) button5).StyleId == null)
          ((Element) button5).StyleId = "btnShelfChangeSelect";
        ((INameScope) nameScope).RegisterName("stckShelfList", (object) stackLayout8);
        if (((Element) stackLayout8).StyleId == null)
          ((Element) stackLayout8).StyleId = "stckShelfList";
        ((INameScope) nameScope).RegisterName("lstShelfList", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstShelfList";
        this.shelftToShelf = (ContentPage) shelfToShelf;
        this.stckContent = stackLayout7;
        this.stckDocumentNumber = stackLayout1;
        this.txtDocumentNumber = entry1;
        this.btnCreateDocumentNumber = button1;
        this.btnClearDocumentNumber = button2;
        this.txtShelf = softkeyboardDisabledEntry1;
        this.txtToShelf = softkeyboardDisabledEntry2;
        this.btnCreateShelf = button3;
        this.stckBarcode = stackLayout4;
        this.txtBarcode = softkeyboardDisabledEntry3;
        this.txtQty = entry2;
        this.pckBarcodeType = picker;
        this.btnShelfOrderSuccess = button4;
        this.btnShelfChangeSelect = button5;
        this.stckShelfList = stackLayout8;
        this.lstShelfList = listView;
        ((BindableObject) stackLayout9).SetValue(View.MarginProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Döküman Numarası Giriniz");
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 22);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "+");
        ((BindableObject) button1).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Button button6 = button1;
        BindableProperty fontSizeProperty1 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter1 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 5];
        objArray1[0] = (object) button1;
        objArray1[1] = (object) stackLayout1;
        objArray1[2] = (object) stackLayout7;
        objArray1[3] = (object) stackLayout9;
        objArray1[4] = (object) shelfToShelf;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver1.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (ShelfToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(15, 130)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter1).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider1);
        ((BindableObject) button6).SetValue(fontSizeProperty1, obj2);
        button1.Clicked += new EventHandler(shelfToShelf.btnCreateDocumentNumber_Clicked);
        referenceExtension1.Name = "shelftToShelf";
        ReferenceExtension referenceExtension11 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 6];
        objArray2[0] = (object) bindingExtension1;
        objArray2[1] = (object) button1;
        objArray2[2] = (object) stackLayout1;
        objArray2[3] = (object) stackLayout7;
        objArray2[4] = (object) stackLayout9;
        objArray2[5] = (object) shelfToShelf;
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
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (ShelfToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(16, 25)));
        object obj4 = ((IMarkupExtension) referenceExtension11).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension1.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 25);
        bindingExtension1.Path = "ButtonColor";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase1);
        referenceExtension2.Name = "shelftToShelf";
        ReferenceExtension referenceExtension12 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 6];
        objArray3[0] = (object) bindingExtension2;
        objArray3[1] = (object) button1;
        objArray3[2] = (object) stackLayout1;
        objArray3[3] = (object) stackLayout7;
        objArray3[4] = (object) stackLayout9;
        objArray3[5] = (object) shelfToShelf;
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
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (ShelfToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(16, 101)));
        object obj6 = ((IMarkupExtension) referenceExtension12).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension2.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 101);
        bindingExtension2.Path = "TextColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(Button.TextColorProperty, bindingBase2);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 22);
        ((BindableObject) button2).SetValue(Button.TextProperty, (object) "x");
        ((BindableObject) button2).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button2).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        ((BindableObject) button2).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Button button7 = button2;
        BindableProperty fontSizeProperty2 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter2 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 5];
        objArray4[0] = (object) button2;
        objArray4[1] = (object) stackLayout1;
        objArray4[2] = (object) stackLayout7;
        objArray4[3] = (object) stackLayout9;
        objArray4[4] = (object) shelfToShelf;
        SimpleValueTargetProvider valueTargetProvider4;
        object obj7 = (object) (valueTargetProvider4 = new SimpleValueTargetProvider(objArray4, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider4.Add(type7, (object) valueTargetProvider4);
        xamlServiceProvider4.Add(typeof (IReferenceProvider), obj7);
        Type type8 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver4 = new XmlNamespaceResolver();
        namespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver4.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver4.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver4.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (ShelfToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(17, 129)));
        object obj8 = ((IExtendedTypeConverter) fontSizeConverter2).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider4);
        ((BindableObject) button7).SetValue(fontSizeProperty2, obj8);
        button2.Clicked += new EventHandler(shelfToShelf.btnClearDocumentNumber_Clicked);
        referenceExtension3.Name = "shelftToShelf";
        ReferenceExtension referenceExtension13 = referenceExtension3;
        XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
        Type type9 = typeof (IProvideValueTarget);
        object[] objArray5 = new object[0 + 6];
        objArray5[0] = (object) bindingExtension3;
        objArray5[1] = (object) button2;
        objArray5[2] = (object) stackLayout1;
        objArray5[3] = (object) stackLayout7;
        objArray5[4] = (object) stackLayout9;
        objArray5[5] = (object) shelfToShelf;
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
        XamlTypeResolver xamlTypeResolver5 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver5, typeof (ShelfToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider5.Add(type10, (object) xamlTypeResolver5);
        xamlServiceProvider5.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(18, 25)));
        object obj10 = ((IMarkupExtension) referenceExtension13).ProvideValue((IServiceProvider) xamlServiceProvider5);
        bindingExtension3.Source = obj10;
        VisualDiagnostics.RegisterSourceInfo(obj10, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 18, 25);
        bindingExtension3.Path = "ButtonColor";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase3);
        referenceExtension4.Name = "shelftToShelf";
        ReferenceExtension referenceExtension14 = referenceExtension4;
        XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
        Type type11 = typeof (IProvideValueTarget);
        object[] objArray6 = new object[0 + 6];
        objArray6[0] = (object) bindingExtension4;
        objArray6[1] = (object) button2;
        objArray6[2] = (object) stackLayout1;
        objArray6[3] = (object) stackLayout7;
        objArray6[4] = (object) stackLayout9;
        objArray6[5] = (object) shelfToShelf;
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
        XamlTypeResolver xamlTypeResolver6 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver6, typeof (ShelfToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider6.Add(type12, (object) xamlTypeResolver6);
        xamlServiceProvider6.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(18, 101)));
        object obj12 = ((IMarkupExtension) referenceExtension14).ProvideValue((IServiceProvider) xamlServiceProvider6);
        bindingExtension4.Source = obj12;
        VisualDiagnostics.RegisterSourceInfo(obj12, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 18, 101);
        bindingExtension4.Path = "TextColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(Button.TextColorProperty, bindingBase4);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button2);
        VisualDiagnostics.RegisterSourceInfo((object) button2, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Kaynak Raf Okutunuz.");
        softkeyboardDisabledEntry1.Completed += new EventHandler(shelfToShelf.txtShelf_Completed);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 21, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 18);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Hedef Raf Okutunuz.");
        softkeyboardDisabledEntry2.Completed += new EventHandler(shelfToShelf.txtToShelf_Completed);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 25, 22);
        ((BindableObject) button3).SetValue(Button.TextProperty, (object) "+");
        ((BindableObject) button3).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button3).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        ((BindableObject) button3).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Button button8 = button3;
        BindableProperty fontSizeProperty3 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter3 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
        Type type13 = typeof (IProvideValueTarget);
        object[] objArray7 = new object[0 + 5];
        objArray7[0] = (object) button3;
        objArray7[1] = (object) stackLayout3;
        objArray7[2] = (object) stackLayout7;
        objArray7[3] = (object) stackLayout9;
        objArray7[4] = (object) shelfToShelf;
        SimpleValueTargetProvider valueTargetProvider7;
        object obj13 = (object) (valueTargetProvider7 = new SimpleValueTargetProvider(objArray7, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider7.Add(type13, (object) valueTargetProvider7);
        xamlServiceProvider7.Add(typeof (IReferenceProvider), obj13);
        Type type14 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver7 = new XmlNamespaceResolver();
        namespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver7.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver7.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver7.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver7 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver7, typeof (ShelfToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider7.Add(type14, (object) xamlTypeResolver7);
        xamlServiceProvider7.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(27, 121)));
        object obj14 = ((IExtendedTypeConverter) fontSizeConverter3).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider7);
        ((BindableObject) button8).SetValue(fontSizeProperty3, obj14);
        button3.Clicked += new EventHandler(shelfToShelf.btnCreateShelf_Clicked);
        referenceExtension5.Name = "shelftToShelf";
        ReferenceExtension referenceExtension15 = referenceExtension5;
        XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
        Type type15 = typeof (IProvideValueTarget);
        object[] objArray8 = new object[0 + 6];
        objArray8[0] = (object) bindingExtension5;
        objArray8[1] = (object) button3;
        objArray8[2] = (object) stackLayout3;
        objArray8[3] = (object) stackLayout7;
        objArray8[4] = (object) stackLayout9;
        objArray8[5] = (object) shelfToShelf;
        SimpleValueTargetProvider valueTargetProvider8;
        object obj15 = (object) (valueTargetProvider8 = new SimpleValueTargetProvider(objArray8, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider8.Add(type15, (object) valueTargetProvider8);
        xamlServiceProvider8.Add(typeof (IReferenceProvider), obj15);
        Type type16 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver8 = new XmlNamespaceResolver();
        namespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver8.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver8.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver8.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver8 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver8, typeof (ShelfToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider8.Add(type16, (object) xamlTypeResolver8);
        xamlServiceProvider8.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(28, 25)));
        object obj16 = ((IMarkupExtension) referenceExtension15).ProvideValue((IServiceProvider) xamlServiceProvider8);
        bindingExtension5.Source = obj16;
        VisualDiagnostics.RegisterSourceInfo(obj16, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 25);
        bindingExtension5.Path = "ButtonColor";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(VisualElement.BackgroundColorProperty, bindingBase5);
        referenceExtension6.Name = "shelftToShelf";
        ReferenceExtension referenceExtension16 = referenceExtension6;
        XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
        Type type17 = typeof (IProvideValueTarget);
        object[] objArray9 = new object[0 + 6];
        objArray9[0] = (object) bindingExtension6;
        objArray9[1] = (object) button3;
        objArray9[2] = (object) stackLayout3;
        objArray9[3] = (object) stackLayout7;
        objArray9[4] = (object) stackLayout9;
        objArray9[5] = (object) shelfToShelf;
        SimpleValueTargetProvider valueTargetProvider9;
        object obj17 = (object) (valueTargetProvider9 = new SimpleValueTargetProvider(objArray9, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider9.Add(type17, (object) valueTargetProvider9);
        xamlServiceProvider9.Add(typeof (IReferenceProvider), obj17);
        Type type18 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver9 = new XmlNamespaceResolver();
        namespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver9.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver9.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver9.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver9 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver9, typeof (ShelfToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider9.Add(type18, (object) xamlTypeResolver9);
        xamlServiceProvider9.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(28, 101)));
        object obj18 = ((IMarkupExtension) referenceExtension16).ProvideValue((IServiceProvider) xamlServiceProvider9);
        bindingExtension6.Source = obj18;
        VisualDiagnostics.RegisterSourceInfo(obj18, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 101);
        bindingExtension6.Path = "TextColor";
        BindingBase bindingBase6 = ((IMarkupExtension<BindingBase>) bindingExtension6).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(Button.TextColorProperty, bindingBase6);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) button3);
        VisualDiagnostics.RegisterSourceInfo((object) button3, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 27, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 24, 18);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry3).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Ürün barkodu okutunuz.");
        softkeyboardDisabledEntry3.Completed += new EventHandler(shelfToShelf.txtBarcode_Completed);
        ((BindableObject) softkeyboardDisabledEntry3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) softkeyboardDisabledEntry3);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry3, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 31, 22);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.TextProperty, (object) "1");
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Miktar");
        ((BindableObject) entry2).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 33, 22);
        ((BindableObject) picker).SetValue(Picker.TitleProperty, (object) "Tip");
        bindingExtension7.Path = ".";
        BindingBase bindingBase7 = ((IMarkupExtension<BindingBase>) bindingExtension7).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker).SetBinding(Picker.ItemsSourceProperty, bindingBase7);
        bindingExtension8.Path = "Caption";
        BindingBase bindingBase8 = ((IMarkupExtension<BindingBase>) bindingExtension8).ProvideValue((IServiceProvider) null);
        picker.ItemDisplayBinding = bindingBase8;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase8, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 36, 33);
        ((BindableObject) picker).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) picker).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) picker);
        VisualDiagnostics.RegisterSourceInfo((object) picker, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 35, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 30, 18);
        ((BindableObject) stackLayout5).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) button4).SetValue(Button.TextProperty, (object) "RAFTAKİ TÜM ÜRÜNLERİ TAŞI");
        button4.Clicked += new EventHandler(shelfToShelf.btnShelfOrderSuccess_Clicked);
        ((BindableObject) button4).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        referenceExtension7.Name = "shelftToShelf";
        ReferenceExtension referenceExtension17 = referenceExtension7;
        XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
        Type type19 = typeof (IProvideValueTarget);
        object[] objArray10 = new object[0 + 6];
        objArray10[0] = (object) bindingExtension9;
        objArray10[1] = (object) button4;
        objArray10[2] = (object) stackLayout5;
        objArray10[3] = (object) stackLayout7;
        objArray10[4] = (object) stackLayout9;
        objArray10[5] = (object) shelfToShelf;
        SimpleValueTargetProvider valueTargetProvider10;
        object obj19 = (object) (valueTargetProvider10 = new SimpleValueTargetProvider(objArray10, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider10.Add(type19, (object) valueTargetProvider10);
        xamlServiceProvider10.Add(typeof (IReferenceProvider), obj19);
        Type type20 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver10 = new XmlNamespaceResolver();
        namespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver10.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver10.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver10.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver10 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver10, typeof (ShelfToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider10.Add(type20, (object) xamlTypeResolver10);
        xamlServiceProvider10.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(41, 21)));
        object obj20 = ((IMarkupExtension) referenceExtension17).ProvideValue((IServiceProvider) xamlServiceProvider10);
        bindingExtension9.Source = obj20;
        VisualDiagnostics.RegisterSourceInfo(obj20, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 41, 21);
        bindingExtension9.Path = "ButtonColor";
        BindingBase bindingBase9 = ((IMarkupExtension<BindingBase>) bindingExtension9).ProvideValue((IServiceProvider) null);
        ((BindableObject) button4).SetBinding(VisualElement.BackgroundColorProperty, bindingBase9);
        referenceExtension8.Name = "shelftToShelf";
        ReferenceExtension referenceExtension18 = referenceExtension8;
        XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
        Type type21 = typeof (IProvideValueTarget);
        object[] objArray11 = new object[0 + 6];
        objArray11[0] = (object) bindingExtension10;
        objArray11[1] = (object) button4;
        objArray11[2] = (object) stackLayout5;
        objArray11[3] = (object) stackLayout7;
        objArray11[4] = (object) stackLayout9;
        objArray11[5] = (object) shelfToShelf;
        SimpleValueTargetProvider valueTargetProvider11;
        object obj21 = (object) (valueTargetProvider11 = new SimpleValueTargetProvider(objArray11, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider11.Add(type21, (object) valueTargetProvider11);
        xamlServiceProvider11.Add(typeof (IReferenceProvider), obj21);
        Type type22 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver11 = new XmlNamespaceResolver();
        namespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver11.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver11.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver11.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver11 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver11, typeof (ShelfToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider11.Add(type22, (object) xamlTypeResolver11);
        xamlServiceProvider11.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(41, 97)));
        object obj22 = ((IMarkupExtension) referenceExtension18).ProvideValue((IServiceProvider) xamlServiceProvider11);
        bindingExtension10.Source = obj22;
        VisualDiagnostics.RegisterSourceInfo(obj22, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 41, 97);
        bindingExtension10.Path = "TextColor";
        BindingBase bindingBase10 = ((IMarkupExtension<BindingBase>) bindingExtension10).ProvideValue((IServiceProvider) null);
        ((BindableObject) button4).SetBinding(Button.TextColorProperty, bindingBase10);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) button4);
        VisualDiagnostics.RegisterSourceInfo((object) button4, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 40, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 39, 18);
        ((BindableObject) stackLayout6).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) button5).SetValue(Button.TextProperty, (object) "SEÇİLİ ÜRÜNLERİ TAŞI");
        button5.Clicked += new EventHandler(shelfToShelf.btnShelfChangeSelect_Clicked);
        ((BindableObject) button5).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        referenceExtension9.Name = "shelftToShelf";
        ReferenceExtension referenceExtension19 = referenceExtension9;
        XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
        Type type23 = typeof (IProvideValueTarget);
        object[] objArray12 = new object[0 + 6];
        objArray12[0] = (object) bindingExtension11;
        objArray12[1] = (object) button5;
        objArray12[2] = (object) stackLayout6;
        objArray12[3] = (object) stackLayout7;
        objArray12[4] = (object) stackLayout9;
        objArray12[5] = (object) shelfToShelf;
        SimpleValueTargetProvider valueTargetProvider12;
        object obj23 = (object) (valueTargetProvider12 = new SimpleValueTargetProvider(objArray12, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider12.Add(type23, (object) valueTargetProvider12);
        xamlServiceProvider12.Add(typeof (IReferenceProvider), obj23);
        Type type24 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver12 = new XmlNamespaceResolver();
        namespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver12.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver12.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver12.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver12 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver12, typeof (ShelfToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider12.Add(type24, (object) xamlTypeResolver12);
        xamlServiceProvider12.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(45, 21)));
        object obj24 = ((IMarkupExtension) referenceExtension19).ProvideValue((IServiceProvider) xamlServiceProvider12);
        bindingExtension11.Source = obj24;
        VisualDiagnostics.RegisterSourceInfo(obj24, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 45, 21);
        bindingExtension11.Path = "ButtonColor";
        BindingBase bindingBase11 = ((IMarkupExtension<BindingBase>) bindingExtension11).ProvideValue((IServiceProvider) null);
        ((BindableObject) button5).SetBinding(VisualElement.BackgroundColorProperty, bindingBase11);
        referenceExtension10.Name = "shelftToShelf";
        ReferenceExtension referenceExtension20 = referenceExtension10;
        XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
        Type type25 = typeof (IProvideValueTarget);
        object[] objArray13 = new object[0 + 6];
        objArray13[0] = (object) bindingExtension12;
        objArray13[1] = (object) button5;
        objArray13[2] = (object) stackLayout6;
        objArray13[3] = (object) stackLayout7;
        objArray13[4] = (object) stackLayout9;
        objArray13[5] = (object) shelfToShelf;
        SimpleValueTargetProvider valueTargetProvider13;
        object obj25 = (object) (valueTargetProvider13 = new SimpleValueTargetProvider(objArray13, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider13.Add(type25, (object) valueTargetProvider13);
        xamlServiceProvider13.Add(typeof (IReferenceProvider), obj25);
        Type type26 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver13 = new XmlNamespaceResolver();
        namespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver13.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver13.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver13.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver13 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver13, typeof (ShelfToShelf).GetTypeInfo().Assembly);
        xamlServiceProvider13.Add(type26, (object) xamlTypeResolver13);
        xamlServiceProvider13.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(45, 97)));
        object obj26 = ((IMarkupExtension) referenceExtension20).ProvideValue((IServiceProvider) xamlServiceProvider13);
        bindingExtension12.Source = obj26;
        VisualDiagnostics.RegisterSourceInfo(obj26, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 45, 97);
        bindingExtension12.Path = "TextColor";
        BindingBase bindingBase12 = ((IMarkupExtension<BindingBase>) bindingExtension12).ProvideValue((IServiceProvider) null);
        ((BindableObject) button5).SetBinding(Button.TextColorProperty, bindingBase12);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) button5);
        VisualDiagnostics.RegisterSourceInfo((object) button5, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 44, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 43, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout9).Children).Add((View) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 14);
        ((BindableObject) stackLayout8).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout8).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) listView).SetValue(ListView.RowHeightProperty, (object) 120);
        ((BindableObject) listView).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 800.0);
        bindingExtension13.Path = ".";
        BindingBase bindingBase13 = ((IMarkupExtension<BindingBase>) bindingExtension13).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase13);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        ((BindableObject) listView).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ShelfToShelf.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_38 xamlCdataTemplate38 = new ShelfToShelf.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_38();
        object[] objArray14 = new object[0 + 5];
        objArray14[0] = (object) dataTemplate1;
        objArray14[1] = (object) listView;
        objArray14[2] = (object) stackLayout8;
        objArray14[3] = (object) stackLayout9;
        objArray14[4] = (object) shelfToShelf;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate38.parentValues = objArray14;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate38.root = shelfToShelf;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate38.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 53, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 49, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout9).Children).Add((View) stackLayout8);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout8, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 48, 14);
        ((BindableObject) shelfToShelf).SetValue(ContentPage.ContentProperty, (object) stackLayout9);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout9, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 10);
        VisualDiagnostics.RegisterSourceInfo((object) shelfToShelf, new Uri("Views\\ShelfToShelf.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<ShelfToShelf>(this, typeof (ShelfToShelf));
      this.shelftToShelf = NameScopeExtensions.FindByName<ContentPage>((Element) this, "shelftToShelf");
      this.stckContent = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckContent");
      this.stckDocumentNumber = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckDocumentNumber");
      this.txtDocumentNumber = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtDocumentNumber");
      this.btnCreateDocumentNumber = NameScopeExtensions.FindByName<Button>((Element) this, "btnCreateDocumentNumber");
      this.btnClearDocumentNumber = NameScopeExtensions.FindByName<Button>((Element) this, "btnClearDocumentNumber");
      this.txtShelf = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtShelf");
      this.txtToShelf = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtToShelf");
      this.btnCreateShelf = NameScopeExtensions.FindByName<Button>((Element) this, "btnCreateShelf");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.txtQty = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtQty");
      this.pckBarcodeType = NameScopeExtensions.FindByName<Picker>((Element) this, "pckBarcodeType");
      this.btnShelfOrderSuccess = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelfOrderSuccess");
      this.btnShelfChangeSelect = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelfChangeSelect");
      this.stckShelfList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfList");
      this.lstShelfList = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfList");
    }
  }
}
