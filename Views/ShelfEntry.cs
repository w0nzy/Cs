// Decompiled with JetBrains decompiler
// Type: Shelf.Views.ShelfEntry
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
using System.Collections.Specialized;
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
  [XamlFilePath("Views\\ShelfEntry.xaml")]
  public class ShelfEntry : ContentPage
  {
    public ztIOShelf mkShelf;
    private List<ShelfTransaction> list;
    private List<string> documentList;
    private List<pIOGetShelfTransDetailByDocumentNumberReturnModel> allDetails;
    private ShelfList shelf;
    private pIOGetShelfFromBarcodeForShelfEntryReturnModel recommendedShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage Product;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckDocumentNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtDocumentNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnCreateDocumentNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnClearDocumentNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnCreateShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnClearShelfBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtQty;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckBarcodeType;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckSuccess;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnSuccess;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstProducts;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Label lblListHeader;

    public Color ButtonColor => Color.FromRgb(52, 203, 201);

    public Color TextColor => Color.White;

    public ztIOShelf selectShelf { get; set; }

    public int trID { get; set; }

    public ShelfEntry()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Raf Girişi(Serbest)";
      this.documentList = new List<string>();
      this.LoadDocuments();
      this.GetMKShelf();
      ((VisualElement) this.btnCreateShelf).IsVisible = GlobalMob.User.IsMainShelf;
      this.list = new List<ShelfTransaction>();
      this.allDetails = new List<pIOGetShelfTransDetailByDocumentNumberReturnModel>();
      ((ItemsView<Cell>) this.lstProducts).ItemsSource = (IEnumerable) this.list;
      GlobalMob.AddBarcodeLongPress(this.txtBarcode);
      ((ICollection<Effect>) ((Element) this.txtDocumentNumber).Effects).Add((Effect) new LongPressedEffect());
      LongPressedEffect.SetCommand((BindableObject) this.txtDocumentNumber, this.LongPress);
      ToolbarItem toolbarItem1 = new ToolbarItem();
      ((MenuItem) toolbarItem1).Text = "";
      ToolbarItem toolbarItem2 = toolbarItem1;
      ((MenuItem) toolbarItem2).Clicked += new EventHandler(this.TItem_Clicked);
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(toolbarItem2);
      ((VisualElement) this.txtQty).IsVisible = !GlobalMob.User.HideQty;
      GlobalMob.AddShelfBarcodeLongPress((Xamarin.Forms.Entry) this.txtShelf);
      GlobalMob.FillBarcodeType(this.pckBarcodeType);
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

    private void Shelf_ShelfSelectedItem(object sender, EventArgs e)
    {
      pIOGetShelfFromTextReturnModel selectedShelf = this.shelf.selectedShelf;
      if (selectedShelf == null)
        return;
      ((InputView) this.txtShelf).Text = selectedShelf.Code;
      this.txtShelf_Completed((object) null, (EventArgs) null);
    }

    private async void TItem_Clicked(object sender, EventArgs e)
    {
      ShelfEntry shelfEntry = this;
      if (shelfEntry.selectShelf == null)
        return;
      double? nullable1 = shelfEntry.allDetails.Sum<pIOGetShelfTransDetailByDocumentNumberReturnModel>((Func<pIOGetShelfTransDetailByDocumentNumberReturnModel, double?>) (x => x.Qty));
      // ISSUE: reference to a compiler-generated method
      double? nullable2 = shelfEntry.allDetails.Where<pIOGetShelfTransDetailByDocumentNumberReturnModel>(new Func<pIOGetShelfTransDetailByDocumentNumberReturnModel, bool>(shelfEntry.\u003CTItem_Clicked\u003Eb__21_1)).Sum<pIOGetShelfTransDetailByDocumentNumberReturnModel>((Func<pIOGetShelfTransDetailByDocumentNumberReturnModel, double?>) (x => x.Qty));
      string str1 = nullable2.ToString();
      nullable2 = nullable1;
      string str2 = nullable2.ToString();
      string str3 = "Raf Miktarı  : " + str1 + "\nToplam Miktar : " + str2;
      int num = await ((Page) shelfEntry).DisplayAlert(shelfEntry.selectShelf.Code, str3, "", "Tamam") ? 1 : 0;
    }

    private string GetLastDocumentNumber()
    {
      int num = 0;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetLastDocumentNumber?transTypeID={0}", (object) 3), (ContentPage) this);
      if (returnModel.Success && !string.IsNullOrEmpty(returnModel.Result))
        num = JsonConvert.DeserializeObject<int>(returnModel.Result) + 1;
      return "G" + Convert.ToString(num);
    }

    private void LoadDocuments()
    {
      // ISSUE: variable of a boxed type
      __Boxed<int> local = (ValueType) 3;
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.Date;
      string str = dateTime.ToString("dd.MM.yyyy");
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfTransDocumentNumber?transTypeID={0}&date={1}", (object) local, (object) str), (ContentPage) this);
      if (!returnModel.Success || string.IsNullOrEmpty(returnModel.Result))
        return;
      this.documentList = GlobalMob.JsonDeserialize<List<pIOGetShelfTransDocumentNumberReturnModel>>(returnModel.Result).Select<pIOGetShelfTransDocumentNumberReturnModel, string>((Func<pIOGetShelfTransDocumentNumberReturnModel, string>) (x => x.DocumentNumber)).ToList<string>();
    }

    private void LoadDetails()
    {
      int shelfID = 0;
      string str = "";
      if (this.selectShelf != null)
      {
        shelfID = this.selectShelf.ShelfID;
        str = this.selectShelf.Code;
      }
      object[] objArray = new object[4]
      {
        (object) 3,
        null,
        null,
        null
      };
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.Date;
      objArray[1] = (object) dateTime.ToString("dd.MM.yyyy");
      objArray[2] = (object) ((InputView) this.txtDocumentNumber).Text;
      objArray[3] = (object) 0;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfTransDetailByDocumentNumber?transTypeID={0}&date={1}&documentNumber={2}&shelfID={3}", objArray), (ContentPage) this);
      if (!returnModel.Success || string.IsNullOrEmpty(returnModel.Result))
        return;
      this.allDetails = GlobalMob.JsonDeserialize<List<pIOGetShelfTransDetailByDocumentNumberReturnModel>>(returnModel.Result);
      this.list = new List<ShelfTransaction>();
      foreach (pIOGetShelfTransDetailByDocumentNumberReturnModel numberReturnModel in this.allDetails.Where<pIOGetShelfTransDetailByDocumentNumberReturnModel>((Func<pIOGetShelfTransDetailByDocumentNumberReturnModel, bool>) (x => x.ShelfID == shelfID || shelfID == 0)).ToList<pIOGetShelfTransDetailByDocumentNumberReturnModel>())
      {
        pIOGetShelfTransDetailByDocumentNumberReturnModel item = numberReturnModel;
        ShelfTransaction shelfTransaction = this.list.Where<ShelfTransaction>((Func<ShelfTransaction, bool>) (x =>
        {
          if (!(x.ItemCode == item.ItemCode) || !(x.ItemDim1Code == item.ItemDim1Code) || !(x.ItemDim2Code == item.ItemDim2Code) || !(x.ItemDim3Code == item.ItemDim3Code) || !(x.ColorCode == item.ColorCode))
            return false;
          int itemTypeCode1 = x.ItemTypeCode;
          int? itemTypeCode2 = item.ItemTypeCode;
          int valueOrDefault = itemTypeCode2.GetValueOrDefault();
          return itemTypeCode1 == valueOrDefault & itemTypeCode2.HasValue;
        })).FirstOrDefault<ShelfTransaction>();
        if (shelfTransaction == null)
        {
          this.list.Add(new ShelfTransaction()
          {
            ColorCode = item.ColorCode,
            UserName = item.CreatedUserName,
            ItemCode = item.ItemCode,
            ItemDim1Code = item.ItemDim1Code,
            ItemDim2Code = item.ItemDim2Code,
            ItemDim3Code = item.ItemDim3Code,
            ItemTypeCode = Convert.ToInt32((object) item.ItemTypeCode),
            Qty = Convert.ToInt32((object) item.Qty),
            ShelfOrderDetailID = Convert.ToInt32((object) item.ShelfOrderDetailID),
            ShelfTransID = item.TransactionID,
            Barcode = item.UsedBarcode,
            ColorDescription = item.ColorDescription,
            ShelfCode = str
          });
        }
        else
        {
          shelfTransaction.Qty += Convert.ToInt32((object) item.Qty);
          this.list.Remove(shelfTransaction);
          this.list.Add(shelfTransaction);
        }
      }
      this.SetInfo();
    }

    private void SetInfo()
    {
      double? nullable = this.allDetails.Sum<pIOGetShelfTransDetailByDocumentNumberReturnModel>((Func<pIOGetShelfTransDetailByDocumentNumberReturnModel, double?>) (x => x.Qty));
      ((MenuItem) ((Page) this).ToolbarItems[0]).Text = this.allDetails.Count > 0 ? "Miktar : " + nullable.ToString() : "";
      this.list = this.list.OrderByDescending<ShelfTransaction, bool>((Func<ShelfTransaction, bool>) (x => x.LastReadBarcode)).ToList<ShelfTransaction>();
      ((VisualElement) this.lstProducts).IsVisible = this.list.Count > 0;
      ((ItemsView<Cell>) this.lstProducts).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstProducts).ItemsSource = (IEnumerable) this.list;
    }

    private ICommand LongPress => (ICommand) new Command((Action) (async () =>
    {
      ShelfEntry shelfEntry = this;
      ListView cnt = new ListView();
      cnt.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(shelfEntry.Lst_ItemSelected);
      ((ItemsView<Cell>) cnt).ItemTemplate = new DataTemplate((Func<object>) (() =>
      {
        ViewCell longPress = new ViewCell();
        ((Cell) longPress).Height = 20.0;
        Label label3 = new Label();
        ((View) label3).HorizontalOptions = LayoutOptions.Start;
        ((View) label3).VerticalOptions = LayoutOptions.Center;
        label3.LineBreakMode = (LineBreakMode) 4;
        label3.FontAttributes = (FontAttributes) 1;
        ((View) label3).Margin = new Thickness(10.0, 0.0, 0.0, 0.0);
        label3.FontSize = 16.0;
        Label label4 = label3;
        ((BindableObject) label4).SetBinding(Label.TextProperty, (BindingBase) new Binding(".", (BindingMode) 0, (IValueConverter) null, (object) null, (string) null, (object) null));
        longPress.View = (View) label4;
        return (object) longPress;
      }));
      ((ItemsView<Cell>) cnt).ItemsSource = (IEnumerable) shelfEntry.documentList;
      SelectItem selectItem = new SelectItem(cnt, "Döküman Numarası Seçiniz");
      await ((NavigableElement) shelfEntry).Navigation.PushAsync((Page) selectItem);
    }));

    private void Lst_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      object selectedItem = ((ListView) sender).SelectedItem;
      if (selectedItem == null)
        return;
      ((InputView) this.txtDocumentNumber).Text = Convert.ToString(selectedItem);
      this.LoadDetails();
      ((NavigableElement) this).Navigation.PopAsync();
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
      if (!string.IsNullOrEmpty(((InputView) this.txtShelf).Text))
        ((VisualElement) this.txtBarcode).Focus();
      else
        ((VisualElement) this.txtShelf).Focus();
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
    }

    private async void txtBarcode_Completed(object sender, EventArgs e)
    {
      ShelfEntry page = this;
      string text = ((InputView) page.txtBarcode).Text;
      if (string.IsNullOrEmpty(text))
        return;
      if (page.selectShelf == null)
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfFromBarcodeForShelfEntry?barcode={0}", (object) text), (ContentPage) page);
        if (!returnModel.Success)
          return;
        List<pIOGetShelfFromBarcodeForShelfEntryReturnModel> source = GlobalMob.JsonDeserialize<List<pIOGetShelfFromBarcodeForShelfEntryReturnModel>>(returnModel.Result);
        if (source.Count <= 0)
          return;
        IEnumerable<pIOGetShelfFromBarcodeForShelfEntryReturnModel> entryReturnModels = source.GroupBy(c => new
        {
          ShelfCode = c.ShelfCode,
          Description = c.Description,
          MainShelfID = c.MainShelfID
        }).Select<IGrouping<\u003C\u003Ef__AnonymousType20<string, string, int>, pIOGetShelfFromBarcodeForShelfEntryReturnModel>, pIOGetShelfFromBarcodeForShelfEntryReturnModel>(gcs => new pIOGetShelfFromBarcodeForShelfEntryReturnModel()
        {
          ShelfCode = gcs.Key.ShelfCode,
          Description = gcs.Key.Description,
          Code = gcs.Key.ShelfCode,
          MainShelfID = gcs.Key.MainShelfID
        });
        ListView listview = GlobalMob.GetListview("ShelfCode,Description", 2, 1, hasUnEvenRows: true);
        ((View) listview).Margin = Thickness.op_Implicit(3.0);
        ((ItemsView<Cell>) listview).ItemsSource = (IEnumerable) entryReturnModels;
        listview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(page.Lst_ItemSelected1);
        SelectItem selectItem = new SelectItem(listview, "Raf Seçiniz");
        await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
      }
      else if (text.Length < GlobalMob.User.MinimumBarcodeLength)
      {
        GlobalMob.PlayError();
        ((InputView) page.txtBarcode).Text = "";
        ((VisualElement) page.txtBarcode).Focus();
      }
      else
      {
        ShelfTransaction shelfTransaction1 = new ShelfTransaction();
        PickerItem selectedItem = (PickerItem) page.pckBarcodeType.SelectedItem;
        if (selectedItem != null && selectedItem.Code == 2 && ((VisualElement) page.pckBarcodeType).IsVisible)
          shelfTransaction1.isLot = true;
        if (selectedItem != null && selectedItem.Code == 3 && ((VisualElement) page.pckBarcodeType).IsVisible)
          shelfTransaction1.isUnique = true;
        shelfTransaction1.ShelfID = page.selectShelf.ShelfID;
        shelfTransaction1.ProcessTypeID = 1;
        shelfTransaction1.WareHouseCode = page.selectShelf.WarehouseCode;
        shelfTransaction1.Barcode = ((InputView) page.txtBarcode).Text;
        shelfTransaction1.UserName = GlobalMob.User.UserName;
        shelfTransaction1.ShelfTransID = page.trID;
        shelfTransaction1.Qty = Convert.ToInt32(((InputView) page.txtQty).Text);
        shelfTransaction1.TransTypeID = 3;
        shelfTransaction1.DocumentNumber = ((InputView) page.txtDocumentNumber).Text;
        ReturnModel result = GlobalMob.PostJson("ShelfInsert", new Dictionary<string, string>()
        {
          {
            "json",
            JsonConvert.SerializeObject((object) shelfTransaction1)
          }
        }, (ContentPage) page).Result;
        if (!result.Success)
          return;
        if (shelfTransaction1.isLot)
        {
          List<ShelfTransaction> shelfTransactionList = JsonConvert.DeserializeObject<List<ShelfTransaction>>(result.Result);
          if (shelfTransactionList == null)
          {
            GlobalMob.PlayError();
            int num = await ((Page) page).DisplayAlert("Bilgi", "Lot Bulunamadı", "", "Tamam") ? 1 : 0;
            page.BarcodeFocus();
            return;
          }
          foreach (ShelfTransaction shelfTransaction2 in shelfTransactionList)
          {
            ShelfTransaction trans = shelfTransaction2;
            trans.ShelfCode = page.selectShelf.Code;
            page.list.Select<ShelfTransaction, ShelfTransaction>((Func<ShelfTransaction, ShelfTransaction>) (c =>
            {
              c.LastReadBarcode = false;
              return c;
            })).ToList<ShelfTransaction>();
            trans.LastReadBarcode = true;
            ShelfTransaction shelfTransaction3 = page.list.Where<ShelfTransaction>((Func<ShelfTransaction, bool>) (x => x.ItemCode == trans.ItemCode && x.ItemDim1Code == trans.ItemDim1Code && x.ItemDim2Code == trans.ItemDim2Code && x.ItemDim3Code == trans.ItemDim3Code && x.ItemTypeCode == trans.ItemTypeCode && x.ColorCode == trans.ColorCode)).FirstOrDefault<ShelfTransaction>();
            if (shelfTransaction3 == null)
            {
              page.list.Add(trans);
            }
            else
            {
              trans.Qty += shelfTransaction3.Qty;
              page.list.Remove(shelfTransaction3);
              page.list.Add(trans);
            }
            page.allDetails.Add(new pIOGetShelfTransDetailByDocumentNumberReturnModel()
            {
              Qty = new double?((double) Convert.ToInt32(((InputView) page.txtQty).Text)),
              ShelfID = page.selectShelf.ShelfID
            });
          }
          page.SetInfo();
          ((InputView) page.txtQty).Text = "1";
          GlobalMob.PlaySave();
        }
        else
        {
          ShelfTransaction trans = JsonConvert.DeserializeObject<ShelfTransaction>(result.Result);
          if (trans != null)
          {
            trans.ColorDescription = trans.ColorDescription;
            trans.ShelfCode = page.selectShelf.Code;
            trans.Description = page.selectShelf.Code;
            if (!string.IsNullOrEmpty(page.selectShelf.MainShelfCode))
              trans.Description = page.selectShelf.MainShelfCode + "-" + page.selectShelf.Code;
            ((VisualElement) page.lstProducts).IsVisible = true;
            page.trID = trans.ShelfTransID;
            ShelfTransaction shelfTransaction4 = page.list.Where<ShelfTransaction>((Func<ShelfTransaction, bool>) (x => x.ItemCode == trans.ItemCode && x.ItemDim1Code == trans.ItemDim1Code && x.ItemDim2Code == trans.ItemDim2Code && x.ItemDim3Code == trans.ItemDim3Code && x.ItemTypeCode == trans.ItemTypeCode && x.ColorCode == trans.ColorCode)).FirstOrDefault<ShelfTransaction>();
            page.list.Select<ShelfTransaction, ShelfTransaction>((Func<ShelfTransaction, ShelfTransaction>) (c =>
            {
              c.LastReadBarcode = false;
              return c;
            })).ToList<ShelfTransaction>();
            trans.LastReadBarcode = true;
            if (shelfTransaction4 == null)
            {
              page.list.Add(trans);
            }
            else
            {
              trans.Qty += shelfTransaction4.Qty;
              page.list.Remove(shelfTransaction4);
              page.list.Add(trans);
            }
            page.allDetails.Add(new pIOGetShelfTransDetailByDocumentNumberReturnModel()
            {
              Qty = new double?((double) Convert.ToInt32(((InputView) page.txtQty).Text)),
              ShelfID = page.selectShelf.ShelfID
            });
            page.SetInfo();
            ((InputView) page.txtQty).Text = "1";
            GlobalMob.PlaySave();
          }
          else
          {
            GlobalMob.PlayError();
            int num = await ((Page) page).DisplayAlert("Bilgi", "Ürün Bulunamadı", "", "Tamam") ? 1 : 0;
          }
        }
        page.BarcodeFocus();
      }
    }

    private void Lst_ItemSelected1(object sender, SelectedItemChangedEventArgs e)
    {
      pIOGetShelfFromBarcodeForShelfEntryReturnModel selectedItem = (pIOGetShelfFromBarcodeForShelfEntryReturnModel) e.SelectedItem;
      if (selectedItem == null)
        return;
      ((NavigableElement) this).Navigation.PopAsync();
      ((InputView) this.txtShelf).Placeholder = selectedItem.ShelfCode;
      this.recommendedShelf = selectedItem;
    }

    private void BarcodeFocus() => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(150);
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode).Focus();
    }));

    private async void txtShelf_Completed(object sender, EventArgs e)
    {
      ShelfEntry page = this;
      page.selectShelf = (ztIOShelf) null;
      page.trID = 0;
      if (string.IsNullOrEmpty(((InputView) page.txtShelf).Text))
        return;
      ReturnModel shelf = GlobalMob.GetShelf(((InputView) page.txtShelf).Text, (ContentPage) page);
      if (!shelf.Success)
        return;
      if (!string.IsNullOrEmpty(shelf.Result))
      {
        ztIOShelf ztIoShelf = JsonConvert.DeserializeObject<ztIOShelf>(shelf.Result);
        if (page.recommendedShelf != null && page.recommendedShelf.ShelfCode != ztIoShelf.Code && ztIoShelf.ShelfID != page.recommendedShelf.ShelfID)
        {
          int num = await ((Page) page).DisplayAlert("Bilgi", "Lütfen önerilen rafı okutunuz.\nÖnerilen Raf:" + page.recommendedShelf.Code, "", "Tamam") ? 1 : 0;
          ((InputView) page.txtBarcode).Text = "";
          ((InputView) page.txtShelf).Text = "";
          ((VisualElement) page.txtShelf).Focus();
        }
        else
        {
          page.selectShelf = ztIoShelf;
          ((InputView) page.txtShelf).Text = page.selectShelf.Description;
          if (page.recommendedShelf != null && !string.IsNullOrEmpty(((InputView) page.txtBarcode).Text))
          {
            page.txtBarcode_Completed((object) page.txtBarcode, (EventArgs) null);
            page.recommendedShelf = (pIOGetShelfFromBarcodeForShelfEntryReturnModel) null;
            ((InputView) page.txtShelf).Placeholder = "Raf No Giriniz/Okutunuz";
            ((InputView) page.txtShelf).Text = "";
            page.selectShelf = (ztIOShelf) null;
            ((InputView) page.txtBarcode).Text = "";
            ((VisualElement) page.txtBarcode).Focus();
          }
          else
            page.LoadDetails();
          ((VisualElement) page.stckSuccess).IsVisible = true;
          // ISSUE: reference to a compiler-generated method
          Device.BeginInvokeOnMainThread(new Action(page.\u003CtxtShelf_Completed\u003Eb__34_0));
        }
      }
      else
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Hatalı Raf", "", "Tamam") ? 1 : 0;
        ((InputView) page.txtShelf).Text = "";
        ((VisualElement) page.txtShelf).Focus();
      }
    }

    private void btnSuccess_Clicked(object sender, EventArgs e)
    {
      this.selectShelf = (ztIOShelf) null;
      this.trID = 0;
      ((VisualElement) this.stckSuccess).IsVisible = false;
      this.list = new List<ShelfTransaction>();
      this.allDetails = new List<pIOGetShelfTransDetailByDocumentNumberReturnModel>();
      ((ItemsView<Cell>) this.lstProducts).ItemsSource = (IEnumerable) this.list;
      this.SetInfo();
      ((InputView) this.txtShelf).Text = "";
      ((MenuItem) ((Page) this).ToolbarItems[0]).Text = "";
      ((VisualElement) this.txtShelf).Focus();
    }

    private void btnCreateDocumentNumber_Clicked(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(((InputView) this.txtDocumentNumber).Text))
        ((InputView) this.txtDocumentNumber).Text = ((InputView) this.txtDocumentNumber).Text + "_";
      Xamarin.Forms.Entry txtDocumentNumber = this.txtDocumentNumber;
      ((InputView) txtDocumentNumber).Text = ((InputView) txtDocumentNumber).Text + this.GetLastDocumentNumber();
      ((VisualElement) this.txtShelf).Focus();
    }

    private void btnClearDocumentNumber_Clicked(object sender, EventArgs e) => ((InputView) this.txtDocumentNumber).Text = "";

    private async void btnCreateShelf_Clicked(object sender, EventArgs e)
    {
      ShelfEntry page = this;
      ztIOShelf mainShelf;
      string pCode;
      ztIOShelf newShelf;
      if (page.mkShelf == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "MK Rafı parametre tablosunda tanımlı değil", "", "Tamam") ? 1 : 0;
        mainShelf = (ztIOShelf) null;
        pCode = (string) null;
        newShelf = (ztIOShelf) null;
      }
      else
      {
        mainShelf = page.mkShelf;
        pCode = await GlobalMob.AskShelfCode((Page) page);
        if (pCode == "-1")
        {
          mainShelf = (ztIOShelf) null;
          pCode = (string) null;
          newShelf = (ztIOShelf) null;
        }
        else
        {
          newShelf = new ztIOShelf()
          {
            CreatedUserName = GlobalMob.User.UserName,
            CreatedDate = new DateTime?(DateTime.Now),
            MainShelfID = new int?(mainShelf.ShelfID),
            IsBlocked = new bool?(false),
            ShelfType = (byte) 0,
            UpdatedDate = new DateTime?(DateTime.Now),
            UpdatedUserName = GlobalMob.User.UserName,
            WarehouseCode = mainShelf.WarehouseCode,
            HallID = mainShelf.HallID,
            ZoneID = mainShelf.ZoneID,
            SortOrder = new int?(0),
            Description = mainShelf.Code,
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
              ((InputView) page.txtShelf).Text = page.selectShelf.Code;
              page.txtShelf_Completed((object) null, (EventArgs) null);
            }
            else
            {
              ((InputView) page.txtShelf).Text = "";
              int num = await ((Page) page).DisplayAlert("Hata", "Bu kod zaten tanımlı:" + pCode, "", "Tamam") ? 1 : 0;
            }
          }
          GlobalMob.CloseLoading();
          mainShelf = (ztIOShelf) null;
          pCode = (string) null;
          newShelf = (ztIOShelf) null;
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
      this.selectShelf = (ztIOShelf) null;
      ((InputView) this.txtShelf).Text = "";
      ((InputView) this.txtBarcode).Text = "";
      this.recommendedShelf = (pIOGetShelfFromBarcodeForShelfEntryReturnModel) null;
      ((InputView) this.txtShelf).Placeholder = "Raf No Giriniz/Okutunuz";
      ((VisualElement) this.stckSuccess).IsVisible = false;
      Device.BeginInvokeOnMainThread((Action) (async () =>
      {
        await Task.Delay(200);
        ((VisualElement) this.txtShelf)?.Focus();
      }));
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (ShelfEntry).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/ShelfEntry.xaml",
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
        ReferenceExtension referenceExtension5 = new ReferenceExtension();
        BindingExtension bindingExtension5 = new BindingExtension();
        ReferenceExtension referenceExtension6 = new ReferenceExtension();
        BindingExtension bindingExtension6 = new BindingExtension();
        Button button3 = new Button();
        ReferenceExtension referenceExtension7 = new ReferenceExtension();
        BindingExtension bindingExtension7 = new BindingExtension();
        ReferenceExtension referenceExtension8 = new ReferenceExtension();
        BindingExtension bindingExtension8 = new BindingExtension();
        Button button4 = new Button();
        StackLayout stackLayout2 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry2 = new SoftkeyboardDisabledEntry();
        Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
        BindingExtension bindingExtension9 = new BindingExtension();
        BindingExtension bindingExtension10 = new BindingExtension();
        Picker picker = new Picker();
        StackLayout stackLayout3 = new StackLayout();
        ReferenceExtension referenceExtension9 = new ReferenceExtension();
        BindingExtension bindingExtension11 = new BindingExtension();
        ReferenceExtension referenceExtension10 = new ReferenceExtension();
        BindingExtension bindingExtension12 = new BindingExtension();
        Button button5 = new Button();
        StackLayout stackLayout4 = new StackLayout();
        ReferenceExtension referenceExtension11 = new ReferenceExtension();
        BindingExtension bindingExtension13 = new BindingExtension();
        ReferenceExtension referenceExtension12 = new ReferenceExtension();
        BindingExtension bindingExtension14 = new BindingExtension();
        Label label = new Label();
        StackLayout stackLayout5 = new StackLayout();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout6 = new StackLayout();
        StackLayout stackLayout7 = new StackLayout();
        ShelfEntry shelfEntry;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (shelfEntry = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) shelfEntry, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("Product", (object) shelfEntry);
        if (((Element) shelfEntry).StyleId == null)
          ((Element) shelfEntry).StyleId = "Product";
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
        ((INameScope) nameScope).RegisterName("stckShelf", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckShelf";
        ((INameScope) nameScope).RegisterName("txtShelf", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtShelf";
        ((INameScope) nameScope).RegisterName("btnCreateShelf", (object) button3);
        if (((Element) button3).StyleId == null)
          ((Element) button3).StyleId = "btnCreateShelf";
        ((INameScope) nameScope).RegisterName("btnClearShelfBarcode", (object) button4);
        if (((Element) button4).StyleId == null)
          ((Element) button4).StyleId = "btnClearShelfBarcode";
        ((INameScope) nameScope).RegisterName("stckBarcode", (object) stackLayout3);
        if (((Element) stackLayout3).StyleId == null)
          ((Element) stackLayout3).StyleId = "stckBarcode";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("txtQty", (object) entry2);
        if (((Element) entry2).StyleId == null)
          ((Element) entry2).StyleId = "txtQty";
        ((INameScope) nameScope).RegisterName("pckBarcodeType", (object) picker);
        if (((Element) picker).StyleId == null)
          ((Element) picker).StyleId = "pckBarcodeType";
        ((INameScope) nameScope).RegisterName("stckSuccess", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckSuccess";
        ((INameScope) nameScope).RegisterName("btnSuccess", (object) button5);
        if (((Element) button5).StyleId == null)
          ((Element) button5).StyleId = "btnSuccess";
        ((INameScope) nameScope).RegisterName("lstProducts", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstProducts";
        ((INameScope) nameScope).RegisterName("lblListHeader", (object) label);
        if (((Element) label).StyleId == null)
          ((Element) label).StyleId = "lblListHeader";
        this.Product = (ContentPage) shelfEntry;
        this.stckDocumentNumber = stackLayout1;
        this.txtDocumentNumber = entry1;
        this.btnCreateDocumentNumber = button1;
        this.btnClearDocumentNumber = button2;
        this.stckShelf = stackLayout2;
        this.txtShelf = softkeyboardDisabledEntry1;
        this.btnCreateShelf = button3;
        this.btnClearShelfBarcode = button4;
        this.stckBarcode = stackLayout3;
        this.txtBarcode = softkeyboardDisabledEntry2;
        this.txtQty = entry2;
        this.pckBarcodeType = picker;
        this.stckSuccess = stackLayout4;
        this.btnSuccess = button5;
        this.lstProducts = listView;
        this.lblListHeader = label;
        ((BindableObject) stackLayout7).SetValue(Layout.PaddingProperty, (object) new Thickness(3.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Döküman Numarası Giriniz");
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 18);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "+");
        ((BindableObject) button1).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Button button6 = button1;
        BindableProperty fontSizeProperty1 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter1 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 4];
        objArray1[0] = (object) button1;
        objArray1[1] = (object) stackLayout1;
        objArray1[2] = (object) stackLayout7;
        objArray1[3] = (object) shelfEntry;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (ShelfEntry).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(11, 126)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter1).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider1);
        ((BindableObject) button6).SetValue(fontSizeProperty1, obj2);
        button1.Clicked += new EventHandler(shelfEntry.btnCreateDocumentNumber_Clicked);
        referenceExtension1.Name = "Product";
        ReferenceExtension referenceExtension13 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 5];
        objArray2[0] = (object) bindingExtension1;
        objArray2[1] = (object) button1;
        objArray2[2] = (object) stackLayout1;
        objArray2[3] = (object) stackLayout7;
        objArray2[4] = (object) shelfEntry;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray2, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver2.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (ShelfEntry).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(12, 25)));
        object obj4 = ((IMarkupExtension) referenceExtension13).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension1.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 25);
        bindingExtension1.Path = "ButtonColor";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase1);
        referenceExtension2.Name = "Product";
        ReferenceExtension referenceExtension14 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 5];
        objArray3[0] = (object) bindingExtension2;
        objArray3[1] = (object) button1;
        objArray3[2] = (object) stackLayout1;
        objArray3[3] = (object) stackLayout7;
        objArray3[4] = (object) shelfEntry;
        SimpleValueTargetProvider valueTargetProvider3;
        object obj5 = (object) (valueTargetProvider3 = new SimpleValueTargetProvider(objArray3, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider3.Add(type5, (object) valueTargetProvider3);
        xamlServiceProvider3.Add(typeof (IReferenceProvider), obj5);
        Type type6 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver3 = new XmlNamespaceResolver();
        namespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver3.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (ShelfEntry).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(12, 95)));
        object obj6 = ((IMarkupExtension) referenceExtension14).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension2.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 95);
        bindingExtension2.Path = "TextColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(Button.TextColorProperty, bindingBase2);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 18);
        ((BindableObject) button2).SetValue(Button.TextProperty, (object) "x");
        ((BindableObject) button2).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button2).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        ((BindableObject) button2).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Button button7 = button2;
        BindableProperty fontSizeProperty2 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter2 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 4];
        objArray4[0] = (object) button2;
        objArray4[1] = (object) stackLayout1;
        objArray4[2] = (object) stackLayout7;
        objArray4[3] = (object) shelfEntry;
        SimpleValueTargetProvider valueTargetProvider4;
        object obj7 = (object) (valueTargetProvider4 = new SimpleValueTargetProvider(objArray4, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider4.Add(type7, (object) valueTargetProvider4);
        xamlServiceProvider4.Add(typeof (IReferenceProvider), obj7);
        Type type8 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver4 = new XmlNamespaceResolver();
        namespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver4.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (ShelfEntry).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(13, 125)));
        object obj8 = ((IExtendedTypeConverter) fontSizeConverter2).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider4);
        ((BindableObject) button7).SetValue(fontSizeProperty2, obj8);
        button2.Clicked += new EventHandler(shelfEntry.btnClearDocumentNumber_Clicked);
        referenceExtension3.Name = "Product";
        ReferenceExtension referenceExtension15 = referenceExtension3;
        XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
        Type type9 = typeof (IProvideValueTarget);
        object[] objArray5 = new object[0 + 5];
        objArray5[0] = (object) bindingExtension3;
        objArray5[1] = (object) button2;
        objArray5[2] = (object) stackLayout1;
        objArray5[3] = (object) stackLayout7;
        objArray5[4] = (object) shelfEntry;
        SimpleValueTargetProvider valueTargetProvider5;
        object obj9 = (object) (valueTargetProvider5 = new SimpleValueTargetProvider(objArray5, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider5.Add(type9, (object) valueTargetProvider5);
        xamlServiceProvider5.Add(typeof (IReferenceProvider), obj9);
        Type type10 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver5 = new XmlNamespaceResolver();
        namespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver5.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver5 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver5, typeof (ShelfEntry).GetTypeInfo().Assembly);
        xamlServiceProvider5.Add(type10, (object) xamlTypeResolver5);
        xamlServiceProvider5.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(14, 25)));
        object obj10 = ((IMarkupExtension) referenceExtension15).ProvideValue((IServiceProvider) xamlServiceProvider5);
        bindingExtension3.Source = obj10;
        VisualDiagnostics.RegisterSourceInfo(obj10, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 25);
        bindingExtension3.Path = "ButtonColor";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase3);
        referenceExtension4.Name = "Product";
        ReferenceExtension referenceExtension16 = referenceExtension4;
        XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
        Type type11 = typeof (IProvideValueTarget);
        object[] objArray6 = new object[0 + 5];
        objArray6[0] = (object) bindingExtension4;
        objArray6[1] = (object) button2;
        objArray6[2] = (object) stackLayout1;
        objArray6[3] = (object) stackLayout7;
        objArray6[4] = (object) shelfEntry;
        SimpleValueTargetProvider valueTargetProvider6;
        object obj11 = (object) (valueTargetProvider6 = new SimpleValueTargetProvider(objArray6, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider6.Add(type11, (object) valueTargetProvider6);
        xamlServiceProvider6.Add(typeof (IReferenceProvider), obj11);
        Type type12 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver6 = new XmlNamespaceResolver();
        namespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver6.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver6 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver6, typeof (ShelfEntry).GetTypeInfo().Assembly);
        xamlServiceProvider6.Add(type12, (object) xamlTypeResolver6);
        xamlServiceProvider6.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(14, 95)));
        object obj12 = ((IMarkupExtension) referenceExtension16).ProvideValue((IServiceProvider) xamlServiceProvider6);
        bindingExtension4.Source = obj12;
        VisualDiagnostics.RegisterSourceInfo(obj12, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 95);
        bindingExtension4.Path = "TextColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(Button.TextColorProperty, bindingBase4);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button2);
        VisualDiagnostics.RegisterSourceInfo((object) button2, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 9, 14);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf No Giriniz/Okutunuz");
        softkeyboardDisabledEntry1.Completed += new EventHandler(shelfEntry.txtShelf_Completed);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 18, 18);
        ((BindableObject) button3).SetValue(Button.TextProperty, (object) "+");
        ((BindableObject) button3).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button3).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        ((BindableObject) button3).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Button button8 = button3;
        BindableProperty fontSizeProperty3 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter3 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
        Type type13 = typeof (IProvideValueTarget);
        object[] objArray7 = new object[0 + 4];
        objArray7[0] = (object) button3;
        objArray7[1] = (object) stackLayout2;
        objArray7[2] = (object) stackLayout7;
        objArray7[3] = (object) shelfEntry;
        SimpleValueTargetProvider valueTargetProvider7;
        object obj13 = (object) (valueTargetProvider7 = new SimpleValueTargetProvider(objArray7, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider7.Add(type13, (object) valueTargetProvider7);
        xamlServiceProvider7.Add(typeof (IReferenceProvider), obj13);
        Type type14 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver7 = new XmlNamespaceResolver();
        namespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver7.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver7 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver7, typeof (ShelfEntry).GetTypeInfo().Assembly);
        xamlServiceProvider7.Add(type14, (object) xamlTypeResolver7);
        xamlServiceProvider7.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(21, 117)));
        object obj14 = ((IExtendedTypeConverter) fontSizeConverter3).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider7);
        ((BindableObject) button8).SetValue(fontSizeProperty3, obj14);
        button3.Clicked += new EventHandler(shelfEntry.btnCreateShelf_Clicked);
        referenceExtension5.Name = "Product";
        ReferenceExtension referenceExtension17 = referenceExtension5;
        XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
        Type type15 = typeof (IProvideValueTarget);
        object[] objArray8 = new object[0 + 5];
        objArray8[0] = (object) bindingExtension5;
        objArray8[1] = (object) button3;
        objArray8[2] = (object) stackLayout2;
        objArray8[3] = (object) stackLayout7;
        objArray8[4] = (object) shelfEntry;
        SimpleValueTargetProvider valueTargetProvider8;
        object obj15 = (object) (valueTargetProvider8 = new SimpleValueTargetProvider(objArray8, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider8.Add(type15, (object) valueTargetProvider8);
        xamlServiceProvider8.Add(typeof (IReferenceProvider), obj15);
        Type type16 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver8 = new XmlNamespaceResolver();
        namespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver8.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver8 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver8, typeof (ShelfEntry).GetTypeInfo().Assembly);
        xamlServiceProvider8.Add(type16, (object) xamlTypeResolver8);
        xamlServiceProvider8.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(22, 25)));
        object obj16 = ((IMarkupExtension) referenceExtension17).ProvideValue((IServiceProvider) xamlServiceProvider8);
        bindingExtension5.Source = obj16;
        VisualDiagnostics.RegisterSourceInfo(obj16, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 22, 25);
        bindingExtension5.Path = "ButtonColor";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(VisualElement.BackgroundColorProperty, bindingBase5);
        referenceExtension6.Name = "Product";
        ReferenceExtension referenceExtension18 = referenceExtension6;
        XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
        Type type17 = typeof (IProvideValueTarget);
        object[] objArray9 = new object[0 + 5];
        objArray9[0] = (object) bindingExtension6;
        objArray9[1] = (object) button3;
        objArray9[2] = (object) stackLayout2;
        objArray9[3] = (object) stackLayout7;
        objArray9[4] = (object) shelfEntry;
        SimpleValueTargetProvider valueTargetProvider9;
        object obj17 = (object) (valueTargetProvider9 = new SimpleValueTargetProvider(objArray9, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider9.Add(type17, (object) valueTargetProvider9);
        xamlServiceProvider9.Add(typeof (IReferenceProvider), obj17);
        Type type18 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver9 = new XmlNamespaceResolver();
        namespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver9.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver9 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver9, typeof (ShelfEntry).GetTypeInfo().Assembly);
        xamlServiceProvider9.Add(type18, (object) xamlTypeResolver9);
        xamlServiceProvider9.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(22, 95)));
        object obj18 = ((IMarkupExtension) referenceExtension18).ProvideValue((IServiceProvider) xamlServiceProvider9);
        bindingExtension6.Source = obj18;
        VisualDiagnostics.RegisterSourceInfo(obj18, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 22, 95);
        bindingExtension6.Path = "TextColor";
        BindingBase bindingBase6 = ((IMarkupExtension<BindingBase>) bindingExtension6).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(Button.TextColorProperty, bindingBase6);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) button3);
        VisualDiagnostics.RegisterSourceInfo((object) button3, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 21, 18);
        ((BindableObject) button4).SetValue(Button.TextProperty, (object) "x");
        ((BindableObject) button4).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button4).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        ((BindableObject) button4).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Button button9 = button4;
        BindableProperty fontSizeProperty4 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter4 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
        Type type19 = typeof (IProvideValueTarget);
        object[] objArray10 = new object[0 + 4];
        objArray10[0] = (object) button4;
        objArray10[1] = (object) stackLayout2;
        objArray10[2] = (object) stackLayout7;
        objArray10[3] = (object) shelfEntry;
        SimpleValueTargetProvider valueTargetProvider10;
        object obj19 = (object) (valueTargetProvider10 = new SimpleValueTargetProvider(objArray10, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider10.Add(type19, (object) valueTargetProvider10);
        xamlServiceProvider10.Add(typeof (IReferenceProvider), obj19);
        Type type20 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver10 = new XmlNamespaceResolver();
        namespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver10.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver10 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver10, typeof (ShelfEntry).GetTypeInfo().Assembly);
        xamlServiceProvider10.Add(type20, (object) xamlTypeResolver10);
        xamlServiceProvider10.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(23, 123)));
        object obj20 = ((IExtendedTypeConverter) fontSizeConverter4).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider10);
        ((BindableObject) button9).SetValue(fontSizeProperty4, obj20);
        button4.Clicked += new EventHandler(shelfEntry.btnClearShelfBarcode_Clicked);
        referenceExtension7.Name = "Product";
        ReferenceExtension referenceExtension19 = referenceExtension7;
        XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
        Type type21 = typeof (IProvideValueTarget);
        object[] objArray11 = new object[0 + 5];
        objArray11[0] = (object) bindingExtension7;
        objArray11[1] = (object) button4;
        objArray11[2] = (object) stackLayout2;
        objArray11[3] = (object) stackLayout7;
        objArray11[4] = (object) shelfEntry;
        SimpleValueTargetProvider valueTargetProvider11;
        object obj21 = (object) (valueTargetProvider11 = new SimpleValueTargetProvider(objArray11, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider11.Add(type21, (object) valueTargetProvider11);
        xamlServiceProvider11.Add(typeof (IReferenceProvider), obj21);
        Type type22 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver11 = new XmlNamespaceResolver();
        namespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver11.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver11 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver11, typeof (ShelfEntry).GetTypeInfo().Assembly);
        xamlServiceProvider11.Add(type22, (object) xamlTypeResolver11);
        xamlServiceProvider11.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(24, 25)));
        object obj22 = ((IMarkupExtension) referenceExtension19).ProvideValue((IServiceProvider) xamlServiceProvider11);
        bindingExtension7.Source = obj22;
        VisualDiagnostics.RegisterSourceInfo(obj22, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 24, 25);
        bindingExtension7.Path = "ButtonColor";
        BindingBase bindingBase7 = ((IMarkupExtension<BindingBase>) bindingExtension7).ProvideValue((IServiceProvider) null);
        ((BindableObject) button4).SetBinding(VisualElement.BackgroundColorProperty, bindingBase7);
        referenceExtension8.Name = "Product";
        ReferenceExtension referenceExtension20 = referenceExtension8;
        XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
        Type type23 = typeof (IProvideValueTarget);
        object[] objArray12 = new object[0 + 5];
        objArray12[0] = (object) bindingExtension8;
        objArray12[1] = (object) button4;
        objArray12[2] = (object) stackLayout2;
        objArray12[3] = (object) stackLayout7;
        objArray12[4] = (object) shelfEntry;
        SimpleValueTargetProvider valueTargetProvider12;
        object obj23 = (object) (valueTargetProvider12 = new SimpleValueTargetProvider(objArray12, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider12.Add(type23, (object) valueTargetProvider12);
        xamlServiceProvider12.Add(typeof (IReferenceProvider), obj23);
        Type type24 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver12 = new XmlNamespaceResolver();
        namespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver12.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver12 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver12, typeof (ShelfEntry).GetTypeInfo().Assembly);
        xamlServiceProvider12.Add(type24, (object) xamlTypeResolver12);
        xamlServiceProvider12.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(24, 95)));
        object obj24 = ((IMarkupExtension) referenceExtension20).ProvideValue((IServiceProvider) xamlServiceProvider12);
        bindingExtension8.Source = obj24;
        VisualDiagnostics.RegisterSourceInfo(obj24, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 24, 95);
        bindingExtension8.Path = "TextColor";
        BindingBase bindingBase8 = ((IMarkupExtension<BindingBase>) bindingExtension8).ProvideValue((IServiceProvider) null);
        ((BindableObject) button4).SetBinding(Button.TextColorProperty, bindingBase8);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) button4);
        VisualDiagnostics.RegisterSourceInfo((object) button4, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 14);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod No Giriniz/Okutunuz");
        softkeyboardDisabledEntry2.Completed += new EventHandler(shelfEntry.txtBarcode_Completed);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 27, 18);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.TextProperty, (object) "1");
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Miktar");
        ((BindableObject) entry2).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 18);
        ((BindableObject) picker).SetValue(Picker.TitleProperty, (object) "Tip");
        bindingExtension9.Path = ".";
        BindingBase bindingBase9 = ((IMarkupExtension<BindingBase>) bindingExtension9).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker).SetBinding(Picker.ItemsSourceProperty, bindingBase9);
        bindingExtension10.Path = "Caption";
        BindingBase bindingBase10 = ((IMarkupExtension<BindingBase>) bindingExtension10).ProvideValue((IServiceProvider) null);
        picker.ItemDisplayBinding = bindingBase10;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase10, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 30, 33);
        ((BindableObject) picker).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) picker).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) picker);
        VisualDiagnostics.RegisterSourceInfo((object) picker, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 29, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 26, 14);
        ((BindableObject) stackLayout4).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) button5).SetValue(Button.TextProperty, (object) "Temizle");
        ((BindableObject) button5).SetValue(VisualElement.HeightRequestProperty, (object) 80.0);
        referenceExtension9.Name = "Product";
        ReferenceExtension referenceExtension21 = referenceExtension9;
        XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
        Type type25 = typeof (IProvideValueTarget);
        object[] objArray13 = new object[0 + 5];
        objArray13[0] = (object) bindingExtension11;
        objArray13[1] = (object) button5;
        objArray13[2] = (object) stackLayout4;
        objArray13[3] = (object) stackLayout7;
        objArray13[4] = (object) shelfEntry;
        SimpleValueTargetProvider valueTargetProvider13;
        object obj25 = (object) (valueTargetProvider13 = new SimpleValueTargetProvider(objArray13, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider13.Add(type25, (object) valueTargetProvider13);
        xamlServiceProvider13.Add(typeof (IReferenceProvider), obj25);
        Type type26 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver13 = new XmlNamespaceResolver();
        namespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver13.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver13 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver13, typeof (ShelfEntry).GetTypeInfo().Assembly);
        xamlServiceProvider13.Add(type26, (object) xamlTypeResolver13);
        xamlServiceProvider13.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(35, 25)));
        object obj26 = ((IMarkupExtension) referenceExtension21).ProvideValue((IServiceProvider) xamlServiceProvider13);
        bindingExtension11.Source = obj26;
        VisualDiagnostics.RegisterSourceInfo(obj26, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 35, 25);
        bindingExtension11.Path = "ButtonColor";
        BindingBase bindingBase11 = ((IMarkupExtension<BindingBase>) bindingExtension11).ProvideValue((IServiceProvider) null);
        ((BindableObject) button5).SetBinding(VisualElement.BackgroundColorProperty, bindingBase11);
        referenceExtension10.Name = "Product";
        ReferenceExtension referenceExtension22 = referenceExtension10;
        XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
        Type type27 = typeof (IProvideValueTarget);
        object[] objArray14 = new object[0 + 5];
        objArray14[0] = (object) bindingExtension12;
        objArray14[1] = (object) button5;
        objArray14[2] = (object) stackLayout4;
        objArray14[3] = (object) stackLayout7;
        objArray14[4] = (object) shelfEntry;
        SimpleValueTargetProvider valueTargetProvider14;
        object obj27 = (object) (valueTargetProvider14 = new SimpleValueTargetProvider(objArray14, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider14.Add(type27, (object) valueTargetProvider14);
        xamlServiceProvider14.Add(typeof (IReferenceProvider), obj27);
        Type type28 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver14 = new XmlNamespaceResolver();
        namespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver14.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver14 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver14, typeof (ShelfEntry).GetTypeInfo().Assembly);
        xamlServiceProvider14.Add(type28, (object) xamlTypeResolver14);
        xamlServiceProvider14.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(35, 95)));
        object obj28 = ((IMarkupExtension) referenceExtension22).ProvideValue((IServiceProvider) xamlServiceProvider14);
        bindingExtension12.Source = obj28;
        VisualDiagnostics.RegisterSourceInfo(obj28, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 35, 95);
        bindingExtension12.Path = "TextColor";
        BindingBase bindingBase12 = ((IMarkupExtension<BindingBase>) bindingExtension12).ProvideValue((IServiceProvider) null);
        ((BindableObject) button5).SetBinding(Button.TextColorProperty, bindingBase12);
        button5.Clicked += new EventHandler(shelfEntry.btnSuccess_Clicked);
        ((BindableObject) button5).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) button5);
        VisualDiagnostics.RegisterSourceInfo((object) button5, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 34, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 33, 14);
        ((BindableObject) listView).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 500.0);
        ((BindableObject) listView).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        ((BindableObject) label).SetValue(Label.TextProperty, (object) "Eklenen Ürünler");
        ((BindableObject) label).SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        referenceExtension11.Name = "Product";
        ReferenceExtension referenceExtension23 = referenceExtension11;
        XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
        Type type29 = typeof (IProvideValueTarget);
        object[] objArray15 = new object[0 + 7];
        objArray15[0] = (object) bindingExtension13;
        objArray15[1] = (object) label;
        objArray15[2] = (object) stackLayout5;
        objArray15[3] = (object) listView;
        objArray15[4] = (object) stackLayout6;
        objArray15[5] = (object) stackLayout7;
        objArray15[6] = (object) shelfEntry;
        SimpleValueTargetProvider valueTargetProvider15;
        object obj29 = (object) (valueTargetProvider15 = new SimpleValueTargetProvider(objArray15, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider15.Add(type29, (object) valueTargetProvider15);
        xamlServiceProvider15.Add(typeof (IReferenceProvider), obj29);
        Type type30 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver15 = new XmlNamespaceResolver();
        namespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver15.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver15 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver15, typeof (ShelfEntry).GetTypeInfo().Assembly);
        xamlServiceProvider15.Add(type30, (object) xamlTypeResolver15);
        xamlServiceProvider15.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(44, 36)));
        object obj30 = ((IMarkupExtension) referenceExtension23).ProvideValue((IServiceProvider) xamlServiceProvider15);
        bindingExtension13.Source = obj30;
        VisualDiagnostics.RegisterSourceInfo(obj30, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 44, 36);
        bindingExtension13.Path = "ButtonColor";
        BindingBase bindingBase13 = ((IMarkupExtension<BindingBase>) bindingExtension13).ProvideValue((IServiceProvider) null);
        ((BindableObject) label).SetBinding(Label.TextColorProperty, bindingBase13);
        referenceExtension12.Name = "Product";
        ReferenceExtension referenceExtension24 = referenceExtension12;
        XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
        Type type31 = typeof (IProvideValueTarget);
        object[] objArray16 = new object[0 + 7];
        objArray16[0] = (object) bindingExtension14;
        objArray16[1] = (object) label;
        objArray16[2] = (object) stackLayout5;
        objArray16[3] = (object) listView;
        objArray16[4] = (object) stackLayout6;
        objArray16[5] = (object) stackLayout7;
        objArray16[6] = (object) shelfEntry;
        SimpleValueTargetProvider valueTargetProvider16;
        object obj31 = (object) (valueTargetProvider16 = new SimpleValueTargetProvider(objArray16, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider16.Add(type31, (object) valueTargetProvider16);
        xamlServiceProvider16.Add(typeof (IReferenceProvider), obj31);
        Type type32 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver16 = new XmlNamespaceResolver();
        namespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver16.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver16 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver16, typeof (ShelfEntry).GetTypeInfo().Assembly);
        xamlServiceProvider16.Add(type32, (object) xamlTypeResolver16);
        xamlServiceProvider16.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(45, 36)));
        object obj32 = ((IMarkupExtension) referenceExtension24).ProvideValue((IServiceProvider) xamlServiceProvider16);
        bindingExtension14.Source = obj32;
        VisualDiagnostics.RegisterSourceInfo(obj32, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 45, 36);
        bindingExtension14.Path = "TextColor";
        BindingBase bindingBase14 = ((IMarkupExtension<BindingBase>) bindingExtension14).ProvideValue((IServiceProvider) null);
        ((BindableObject) label).SetBinding(VisualElement.BackgroundColorProperty, bindingBase14);
        ((BindableObject) label).SetValue(Label.FontProperty, new FontTypeConverter().ConvertFromInvariantString("Bold,20"));
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) label);
        VisualDiagnostics.RegisterSourceInfo((object) label, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 42, 30);
        ((BindableObject) listView).SetValue(ListView.HeaderProperty, (object) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 41, 26);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ShelfEntry.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_28 xamlCdataTemplate28 = new ShelfEntry.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_28();
        object[] objArray17 = new object[0 + 5];
        objArray17[0] = (object) dataTemplate1;
        objArray17[1] = (object) listView;
        objArray17[2] = (object) stackLayout6;
        objArray17[3] = (object) stackLayout7;
        objArray17[4] = (object) shelfEntry;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate28.parentValues = objArray17;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate28.root = shelfEntry;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate28.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 50, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 39, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 38, 14);
        ((BindableObject) shelfEntry).SetValue(ContentPage.ContentProperty, (object) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 8, 10);
        VisualDiagnostics.RegisterSourceInfo((object) shelfEntry, new Uri("Views\\ShelfEntry.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<ShelfEntry>(this, typeof (ShelfEntry));
      this.Product = NameScopeExtensions.FindByName<ContentPage>((Element) this, "Product");
      this.stckDocumentNumber = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckDocumentNumber");
      this.txtDocumentNumber = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtDocumentNumber");
      this.btnCreateDocumentNumber = NameScopeExtensions.FindByName<Button>((Element) this, "btnCreateDocumentNumber");
      this.btnClearDocumentNumber = NameScopeExtensions.FindByName<Button>((Element) this, "btnClearDocumentNumber");
      this.stckShelf = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelf");
      this.txtShelf = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtShelf");
      this.btnCreateShelf = NameScopeExtensions.FindByName<Button>((Element) this, "btnCreateShelf");
      this.btnClearShelfBarcode = NameScopeExtensions.FindByName<Button>((Element) this, "btnClearShelfBarcode");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.txtQty = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtQty");
      this.pckBarcodeType = NameScopeExtensions.FindByName<Picker>((Element) this, "pckBarcodeType");
      this.stckSuccess = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckSuccess");
      this.btnSuccess = NameScopeExtensions.FindByName<Button>((Element) this, "btnSuccess");
      this.lstProducts = NameScopeExtensions.FindByName<ListView>((Element) this, "lstProducts");
      this.lblListHeader = NameScopeExtensions.FindByName<Label>((Element) this, "lblListHeader");
    }
  }
}
