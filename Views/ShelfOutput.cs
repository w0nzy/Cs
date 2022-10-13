// Decompiled with JetBrains decompiler
// Type: Shelf.Views.ShelfOutput
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
  [XamlFilePath("Views\\ShelfOutput.xaml")]
  public class ShelfOutput : ContentPage
  {
    private List<ztIOShelfTransactionDetail> list;
    private List<string> documentList;
    private List<pIOGetShelfTransDetailByDocumentNumberReturnModel> allDetails;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage shelfoutput;
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
    private Label lblShelfCountingDetailHeader;

    public Color ButtonColor => Color.FromRgb(142, 81, 152);

    public Color TextColor => Color.White;

    public ztIOShelf selectShelf { get; set; }

    public int trID { get; set; }

    public ShelfOutput()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Raf Çıkışı";
      this.documentList = new List<string>();
      this.LoadDocuments();
      this.allDetails = new List<pIOGetShelfTransDetailByDocumentNumberReturnModel>();
      this.list = new List<ztIOShelfTransactionDetail>();
      ((ItemsView<Cell>) this.lstProducts).ItemsSource = (IEnumerable) this.list;
      ((ICollection<Effect>) ((Element) this.txtDocumentNumber).Effects).Add((Effect) new LongPressedEffect());
      LongPressedEffect.SetCommand((BindableObject) this.txtDocumentNumber, this.LongPress);
      ToolbarItem toolbarItem1 = new ToolbarItem();
      ((MenuItem) toolbarItem1).Text = "";
      ToolbarItem toolbarItem2 = toolbarItem1;
      ((MenuItem) toolbarItem2).Clicked += new EventHandler(this.TItem_Clicked);
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(toolbarItem2);
      GlobalMob.AddBarcodeLongPress(this.txtBarcode);
      ((VisualElement) this.txtQty).IsVisible = !GlobalMob.User.HideQty;
      GlobalMob.FillBarcodeType(this.pckBarcodeType);
      GlobalMob.AddShelfBarcodeLongPress((Xamarin.Forms.Entry) this.txtShelf);
    }

    private void btnCreateDocumentNumber_Clicked(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(((InputView) this.txtDocumentNumber).Text))
        ((InputView) this.txtDocumentNumber).Text = ((InputView) this.txtDocumentNumber).Text + "_";
      Xamarin.Forms.Entry txtDocumentNumber = this.txtDocumentNumber;
      ((InputView) txtDocumentNumber).Text = ((InputView) txtDocumentNumber).Text + this.GetLastDocumentNumber();
      ((VisualElement) this.txtShelf).Focus();
    }

    private async void TItem_Clicked(object sender, EventArgs e)
    {
      ShelfOutput shelfOutput = this;
      if (shelfOutput.selectShelf == null)
        return;
      double? nullable1 = shelfOutput.allDetails.Sum<pIOGetShelfTransDetailByDocumentNumberReturnModel>((Func<pIOGetShelfTransDetailByDocumentNumberReturnModel, double?>) (x => x.Qty));
      // ISSUE: reference to a compiler-generated method
      double? nullable2 = shelfOutput.allDetails.Where<pIOGetShelfTransDetailByDocumentNumberReturnModel>(new Func<pIOGetShelfTransDetailByDocumentNumberReturnModel, bool>(shelfOutput.\u003CTItem_Clicked\u003Eb__17_1)).Sum<pIOGetShelfTransDetailByDocumentNumberReturnModel>((Func<pIOGetShelfTransDetailByDocumentNumberReturnModel, double?>) (x => x.Qty));
      string str1 = nullable2.ToString();
      nullable2 = nullable1;
      string str2 = nullable2.ToString();
      string str3 = "Raf Miktarı  : " + str1 + "\nToplam Miktar : " + str2;
      int num = await ((Page) shelfOutput).DisplayAlert(shelfOutput.selectShelf.Code, str3, "", "Tamam") ? 1 : 0;
    }

    private void SetInfo()
    {
      double? nullable = this.allDetails.Sum<pIOGetShelfTransDetailByDocumentNumberReturnModel>((Func<pIOGetShelfTransDetailByDocumentNumberReturnModel, double?>) (x => x.Qty));
      ((MenuItem) ((Page) this).ToolbarItems[0]).Text = this.allDetails.Count > 0 ? "Miktar : " + nullable.ToString() : "";
      ((VisualElement) this.lstProducts).IsVisible = this.list.Count > 0;
      ((ItemsView<Cell>) this.lstProducts).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstProducts).ItemsSource = (IEnumerable) this.list;
    }

    private string GetLastDocumentNumber()
    {
      int num = 0;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetLastDocumentNumber?transTypeID={0}", (object) 4), (ContentPage) this);
      if (returnModel.Success && !string.IsNullOrEmpty(returnModel.Result))
        num = JsonConvert.DeserializeObject<int>(returnModel.Result) + 1;
      return "C" + Convert.ToString(num);
    }

    private void LoadDocuments()
    {
      // ISSUE: variable of a boxed type
      __Boxed<int> local = (ValueType) 4;
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.Date;
      string str = dateTime.ToString("dd.MM.yyyy");
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfTransDocumentNumber?transTypeID={0}&date={1}", (object) local, (object) str), (ContentPage) this);
      if (!returnModel.Success || string.IsNullOrEmpty(returnModel.Result))
        return;
      this.documentList = GlobalMob.JsonDeserialize<List<pIOGetShelfTransDocumentNumberReturnModel>>(returnModel.Result).Select<pIOGetShelfTransDocumentNumberReturnModel, string>((Func<pIOGetShelfTransDocumentNumberReturnModel, string>) (x => x.DocumentNumber)).ToList<string>();
    }

    private ICommand LongPressShelf => (ICommand) new Command((Action) (async () =>
    {
      ShelfOutput shelfOutput = this;
      ListView cnt = new ListView();
      cnt.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(shelfOutput.Lst_ItemSelected);
      ((ItemsView<Cell>) cnt).ItemTemplate = new DataTemplate((Func<object>) (() =>
      {
        ViewCell longPressShelf = new ViewCell();
        ((Cell) longPressShelf).Height = 20.0;
        Label label3 = new Label();
        ((View) label3).HorizontalOptions = LayoutOptions.Start;
        ((View) label3).VerticalOptions = LayoutOptions.Center;
        label3.LineBreakMode = (LineBreakMode) 4;
        label3.FontAttributes = (FontAttributes) 1;
        ((View) label3).Margin = new Thickness(10.0, 0.0, 0.0, 0.0);
        label3.FontSize = 16.0;
        Label label4 = label3;
        ((BindableObject) label4).SetBinding(Label.TextProperty, (BindingBase) new Binding(".", (BindingMode) 0, (IValueConverter) null, (object) null, (string) null, (object) null));
        longPressShelf.View = (View) label4;
        return (object) longPressShelf;
      }));
      ((ItemsView<Cell>) cnt).ItemsSource = (IEnumerable) shelfOutput.documentList;
      SelectItem selectItem = new SelectItem(cnt, "Döküman Numarası Seçiniz");
      await ((NavigableElement) shelfOutput).Navigation.PushAsync((Page) selectItem);
    }));

    private ICommand LongPress => (ICommand) new Command((Action) (async () =>
    {
      ShelfOutput shelfOutput = this;
      ListView cnt = new ListView();
      cnt.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(shelfOutput.Lst_ItemSelected);
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
      ((ItemsView<Cell>) cnt).ItemsSource = (IEnumerable) shelfOutput.documentList;
      SelectItem selectItem = new SelectItem(cnt, "Döküman Numarası Seçiniz");
      await ((NavigableElement) shelfOutput).Navigation.PushAsync((Page) selectItem);
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

    private async void txtShelf_Completed(object sender, EventArgs e)
    {
      ShelfOutput page = this;
      page.selectShelf = (ztIOShelf) null;
      page.trID = 0;
      if (string.IsNullOrEmpty(((InputView) page.txtShelf).Text))
        return;
      ReturnModel shelf = GlobalMob.GetShelf(((InputView) page.txtShelf).Text, (ContentPage) page);
      if (!shelf.Success)
        return;
      if (!string.IsNullOrEmpty(shelf.Result))
      {
        page.selectShelf = JsonConvert.DeserializeObject<ztIOShelf>(shelf.Result);
        page.LoadDetails();
        ((VisualElement) page.stckBarcode).IsVisible = true;
        ((VisualElement) page.stckSuccess).IsVisible = true;
        // ISSUE: reference to a compiler-generated method
        Device.BeginInvokeOnMainThread(new Action(page.\u003CtxtShelf_Completed\u003Eb__27_0));
      }
      else
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Hatalı Raf", "", "Tamam") ? 1 : 0;
        ((InputView) page.txtShelf).Text = "";
        ((VisualElement) page.txtShelf).Focus();
      }
    }

    private async void txtBarvode_Completed(object sender, EventArgs e)
    {
      ShelfOutput page = this;
      if (string.IsNullOrEmpty(((InputView) page.txtShelf).Text))
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Lütfen raf okutunuz", "", "Tamam") ? 1 : 0;
        ((InputView) page.txtBarcode).Text = "";
        ((VisualElement) page.txtBarcode).Focus();
      }
      else
      {
        if (!string.IsNullOrEmpty(((InputView) page.txtBarcode).Text))
        {
          PickerItem selectedItem = (PickerItem) page.pckBarcodeType.SelectedItem;
          ShelfTransaction shelfTransaction1 = new ShelfTransaction();
          shelfTransaction1.ShelfID = page.selectShelf.ShelfID;
          shelfTransaction1.ProcessTypeID = 2;
          shelfTransaction1.WareHouseCode = page.selectShelf.WarehouseCode;
          shelfTransaction1.Barcode = ((InputView) page.txtBarcode).Text;
          shelfTransaction1.UserName = GlobalMob.User.UserName;
          shelfTransaction1.ShelfTransID = page.trID;
          shelfTransaction1.Qty = Convert.ToInt32(((InputView) page.txtQty).Text);
          shelfTransaction1.TransTypeID = 4;
          shelfTransaction1.DocumentNumber = ((InputView) page.txtDocumentNumber).Text;
          if (selectedItem != null && selectedItem.Code == 2 && ((VisualElement) page.pckBarcodeType).IsVisible)
            shelfTransaction1.isLot = true;
          if (selectedItem != null && selectedItem.Code == 3 && ((VisualElement) page.pckBarcodeType).IsVisible)
            shelfTransaction1.isUnique = true;
          ReturnModel result = GlobalMob.PostJson("ShelfOutputIns", new Dictionary<string, string>()
          {
            {
              "json",
              JsonConvert.SerializeObject((object) shelfTransaction1)
            }
          }, (ContentPage) page).Result;
          if (result.Success)
          {
            if (shelfTransaction1.isLot)
            {
              List<ShelfTransaction> shelfTransactionList = JsonConvert.DeserializeObject<List<ShelfTransaction>>(result.Result);
              if (shelfTransactionList != null)
              {
                if (shelfTransactionList.Count == 1 && shelfTransactionList[0].ShelfTransID == -1)
                {
                  GlobalMob.PlayError();
                  string str = "Raftaki Lot Miktarı Yetersiz";
                  int num = await ((Page) page).DisplayAlert("Bilgi", str, "", "Tamam") ? 1 : 0;
                }
                else
                {
                  foreach (ShelfTransaction shelfTransaction2 in shelfTransactionList)
                  {
                    ShelfTransaction trans = shelfTransaction2;
                    ((VisualElement) page.lstProducts).IsVisible = true;
                    ztIOShelfTransactionDetail transactionDetail1 = page.list.Where<ztIOShelfTransactionDetail>((Func<ztIOShelfTransactionDetail, bool>) (x =>
                    {
                      if (x.ItemCode == trans.ItemCode && x.ItemDim1Code == trans.ItemDim1Code && x.ItemDim2Code == trans.ItemDim2Code && x.ItemDim3Code == trans.ItemDim3Code)
                      {
                        int? itemTypeCode1 = x.ItemTypeCode;
                        int itemTypeCode2 = trans.ItemTypeCode;
                        if (itemTypeCode1.GetValueOrDefault() == itemTypeCode2 & itemTypeCode1.HasValue)
                          return x.ColorCode == trans.ColorCode;
                      }
                      return false;
                    })).FirstOrDefault<ztIOShelfTransactionDetail>();
                    if (transactionDetail1 == null)
                    {
                      ztIOShelfTransactionDetail transactionDetail2 = new ztIOShelfTransactionDetail()
                      {
                        ColorCode = trans.ColorCode,
                        ColorDescription = trans.ColorDescription,
                        CreatedDate = new DateTime?(DateTime.Now),
                        CreatedUserName = GlobalMob.User.UserName,
                        ItemCode = trans.ItemCode,
                        ItemDim1Code = trans.ItemDim1Code,
                        ItemDim2Code = trans.ItemDim2Code,
                        ItemDim3Code = trans.ItemDim3Code,
                        ItemTypeCode = new int?(trans.ItemTypeCode),
                        Qty = new double?((double) trans.Qty)
                      };
                      page.list.Add(transactionDetail2);
                    }
                    else
                    {
                      ztIOShelfTransactionDetail transactionDetail3 = transactionDetail1;
                      double? qty1 = transactionDetail3.Qty;
                      double qty2 = (double) trans.Qty;
                      transactionDetail3.Qty = qty1.HasValue ? new double?(qty1.GetValueOrDefault() + qty2) : new double?();
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
              }
              else
              {
                GlobalMob.PlayError();
                int num = await ((Page) page).DisplayAlert("Bilgi", "Ürün Bulunamadı", "", "Tamam") ? 1 : 0;
              }
            }
            else
            {
              ztIOShelfTransactionDetail trans = JsonConvert.DeserializeObject<ztIOShelfTransactionDetail>(result.Result);
              if (trans != null)
              {
                if (trans.TransactionDetailID == -1)
                {
                  GlobalMob.PlayError();
                  Convert.ToInt32((object) trans.ShelfOrderDetailID);
                  Convert.ToInt32((object) trans.Qty);
                  string str = "Kullanılabilir Miktar Yetersiz" + "\nKullanılabilir Ürün Miktarı :" + trans.Qty.ToString() + "\nÇıkış Yapılmak İstenen Miktar:" + shelfTransaction1.Qty.ToString() + "\nRaftaki Miktar:" + trans.ShelfOrderDetailID.ToString();
                  int num = await ((Page) page).DisplayAlert("Bilgi", str, "", "Tamam") ? 1 : 0;
                }
                else
                {
                  trans.ColorDescription = trans.CreatedUserName;
                  ((VisualElement) page.lstProducts).IsVisible = true;
                  page.trID = trans.TransactionID;
                  ztIOShelfTransactionDetail transactionDetail4 = page.list.Where<ztIOShelfTransactionDetail>((Func<ztIOShelfTransactionDetail, bool>) (x =>
                  {
                    if (x.ItemCode == trans.ItemCode && x.ItemDim1Code == trans.ItemDim1Code && x.ItemDim2Code == trans.ItemDim2Code && x.ItemDim3Code == trans.ItemDim3Code)
                    {
                      int? itemTypeCode3 = x.ItemTypeCode;
                      int? itemTypeCode4 = trans.ItemTypeCode;
                      if (itemTypeCode3.GetValueOrDefault() == itemTypeCode4.GetValueOrDefault() & itemTypeCode3.HasValue == itemTypeCode4.HasValue)
                        return x.ColorCode == trans.ColorCode;
                    }
                    return false;
                  })).FirstOrDefault<ztIOShelfTransactionDetail>();
                  if (transactionDetail4 == null)
                  {
                    page.list.Add(trans);
                  }
                  else
                  {
                    ztIOShelfTransactionDetail transactionDetail5 = trans;
                    double? qty3 = transactionDetail5.Qty;
                    double? qty4 = transactionDetail4.Qty;
                    transactionDetail5.Qty = qty3.HasValue & qty4.HasValue ? new double?(qty3.GetValueOrDefault() + qty4.GetValueOrDefault()) : new double?();
                    page.list.Remove(transactionDetail4);
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
              }
              else
              {
                GlobalMob.PlayError();
                int num = await ((Page) page).DisplayAlert("Bilgi", "Ürün Bulunamadı", "", "Tamam") ? 1 : 0;
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated method
        Device.BeginInvokeOnMainThread(new Action(page.\u003CtxtBarvode_Completed\u003Eb__28_0));
      }
    }

    private void LoadDetails()
    {
      int shelfID = 0;
      if (this.selectShelf != null)
        shelfID = this.selectShelf.ShelfID;
      object[] objArray = new object[4]
      {
        (object) 4,
        null,
        null,
        null
      };
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.Date;
      objArray[1] = (object) dateTime.ToString("dd.MM.yyyy");
      objArray[2] = (object) ((InputView) this.txtDocumentNumber).Text;
      objArray[3] = (object) shelfID;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfTransDetailByDocumentNumber?transTypeID={0}&date={1}&documentNumber={2}&shelfID={3}", objArray), (ContentPage) this);
      if (!returnModel.Success || string.IsNullOrEmpty(returnModel.Result))
        return;
      this.allDetails = GlobalMob.JsonDeserialize<List<pIOGetShelfTransDetailByDocumentNumberReturnModel>>(returnModel.Result);
      this.list = new List<ztIOShelfTransactionDetail>();
      foreach (pIOGetShelfTransDetailByDocumentNumberReturnModel numberReturnModel in this.allDetails.Where<pIOGetShelfTransDetailByDocumentNumberReturnModel>((Func<pIOGetShelfTransDetailByDocumentNumberReturnModel, bool>) (x => x.ShelfID == shelfID || shelfID == 0)).ToList<pIOGetShelfTransDetailByDocumentNumberReturnModel>())
      {
        pIOGetShelfTransDetailByDocumentNumberReturnModel item = numberReturnModel;
        ztIOShelfTransactionDetail transactionDetail1 = this.list.Where<ztIOShelfTransactionDetail>((Func<ztIOShelfTransactionDetail, bool>) (x =>
        {
          if (!(x.ItemCode == item.ItemCode) || !(x.ColorCode == item.ColorCode) || !(x.ItemDim1Code == item.ItemDim1Code) || !(x.ItemDim2Code == item.ItemDim2Code) || !(x.ItemDim3Code == item.ItemDim3Code))
            return false;
          int? itemTypeCode1 = x.ItemTypeCode;
          int? itemTypeCode2 = item.ItemTypeCode;
          return itemTypeCode1.GetValueOrDefault() == itemTypeCode2.GetValueOrDefault() & itemTypeCode1.HasValue == itemTypeCode2.HasValue;
        })).FirstOrDefault<ztIOShelfTransactionDetail>();
        if (transactionDetail1 == null)
        {
          this.list.Add(new ztIOShelfTransactionDetail()
          {
            ColorCode = item.ColorCode,
            CreatedDate = item.CreatedDate,
            CreatedUserName = item.CreatedUserName,
            ItemCode = item.ItemCode,
            ItemDim1Code = item.ItemDim1Code,
            ItemDim2Code = item.ItemDim2Code,
            ItemDim3Code = item.ItemDim3Code,
            ItemTypeCode = item.ItemTypeCode,
            Qty = item.Qty,
            ShelfOrderDetailID = item.ShelfOrderDetailID,
            TransactionDetailID = item.TransactionDetailID,
            TransactionID = item.TransactionID,
            UpdatedDate = item.UpdatedDate,
            UpdatedUserName = item.UpdatedUserName,
            UsedBarcode = item.UsedBarcode,
            ColorDescription = item.ColorDescription
          });
        }
        else
        {
          ztIOShelfTransactionDetail transactionDetail2 = transactionDetail1;
          double? qty1 = transactionDetail2.Qty;
          double? qty2 = item.Qty;
          transactionDetail2.Qty = qty1.HasValue & qty2.HasValue ? new double?(qty1.GetValueOrDefault() + qty2.GetValueOrDefault()) : new double?();
          this.list.Remove(transactionDetail1);
          this.list.Add(transactionDetail1);
        }
      }
      this.SetInfo();
    }

    private void btnSuccess_Clicked(object sender, EventArgs e)
    {
      ((VisualElement) this.stckBarcode).IsVisible = false;
      this.selectShelf = (ztIOShelf) null;
      this.trID = 0;
      ((VisualElement) this.stckSuccess).IsVisible = false;
      this.list = new List<ztIOShelfTransactionDetail>();
      this.allDetails = new List<pIOGetShelfTransDetailByDocumentNumberReturnModel>();
      ((ItemsView<Cell>) this.lstProducts).ItemsSource = (IEnumerable) this.list;
      this.SetInfo();
      ((InputView) this.txtShelf).Text = "";
      ((VisualElement) this.txtShelf).Focus();
    }

    private void btnClearDocumentNumber_Clicked(object sender, EventArgs e) => ((InputView) this.txtDocumentNumber).Text = "";

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (ShelfOutput).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/ShelfOutput.xaml",
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
        Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
        BindingExtension bindingExtension5 = new BindingExtension();
        BindingExtension bindingExtension6 = new BindingExtension();
        Picker picker = new Picker();
        StackLayout stackLayout3 = new StackLayout();
        ReferenceExtension referenceExtension5 = new ReferenceExtension();
        BindingExtension bindingExtension7 = new BindingExtension();
        ReferenceExtension referenceExtension6 = new ReferenceExtension();
        BindingExtension bindingExtension8 = new BindingExtension();
        Button button3 = new Button();
        StackLayout stackLayout4 = new StackLayout();
        ReferenceExtension referenceExtension7 = new ReferenceExtension();
        BindingExtension bindingExtension9 = new BindingExtension();
        ReferenceExtension referenceExtension8 = new ReferenceExtension();
        BindingExtension bindingExtension10 = new BindingExtension();
        Label label = new Label();
        StackLayout stackLayout5 = new StackLayout();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout6 = new StackLayout();
        StackLayout stackLayout7 = new StackLayout();
        ShelfOutput shelfOutput;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (shelfOutput = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) shelfOutput, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("shelfoutput", (object) shelfOutput);
        if (((Element) shelfOutput).StyleId == null)
          ((Element) shelfOutput).StyleId = "shelfoutput";
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
        ((INameScope) nameScope).RegisterName("btnSuccess", (object) button3);
        if (((Element) button3).StyleId == null)
          ((Element) button3).StyleId = "btnSuccess";
        ((INameScope) nameScope).RegisterName("lstProducts", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstProducts";
        ((INameScope) nameScope).RegisterName("lblShelfCountingDetailHeader", (object) label);
        if (((Element) label).StyleId == null)
          ((Element) label).StyleId = "lblShelfCountingDetailHeader";
        this.shelfoutput = (ContentPage) shelfOutput;
        this.stckDocumentNumber = stackLayout1;
        this.txtDocumentNumber = entry1;
        this.btnCreateDocumentNumber = button1;
        this.btnClearDocumentNumber = button2;
        this.txtShelf = softkeyboardDisabledEntry1;
        this.stckBarcode = stackLayout3;
        this.txtBarcode = softkeyboardDisabledEntry2;
        this.txtQty = entry2;
        this.pckBarcodeType = picker;
        this.stckSuccess = stackLayout4;
        this.btnSuccess = button3;
        this.lstProducts = listView;
        this.lblShelfCountingDetailHeader = label;
        ((BindableObject) stackLayout7).SetValue(Layout.PaddingProperty, (object) new Thickness(3.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Döküman Numarası Giriniz");
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 18);
        ((BindableObject) button1).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "+");
        ((BindableObject) button1).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Button button4 = button1;
        BindableProperty fontSizeProperty1 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter1 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 4];
        objArray1[0] = (object) button1;
        objArray1[1] = (object) stackLayout1;
        objArray1[2] = (object) stackLayout7;
        objArray1[3] = (object) shelfOutput;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (ShelfOutput).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(11, 126)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter1).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider1);
        ((BindableObject) button4).SetValue(fontSizeProperty1, obj2);
        button1.Clicked += new EventHandler(shelfOutput.btnCreateDocumentNumber_Clicked);
        referenceExtension1.Name = "shelfoutput";
        ReferenceExtension referenceExtension9 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 5];
        objArray2[0] = (object) bindingExtension1;
        objArray2[1] = (object) button1;
        objArray2[2] = (object) stackLayout1;
        objArray2[3] = (object) stackLayout7;
        objArray2[4] = (object) shelfOutput;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray2, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver2.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (ShelfOutput).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(12, 25)));
        object obj4 = ((IMarkupExtension) referenceExtension9).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension1.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 25);
        bindingExtension1.Path = "ButtonColor";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase1);
        referenceExtension2.Name = "shelfoutput";
        ReferenceExtension referenceExtension10 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 5];
        objArray3[0] = (object) bindingExtension2;
        objArray3[1] = (object) button1;
        objArray3[2] = (object) stackLayout1;
        objArray3[3] = (object) stackLayout7;
        objArray3[4] = (object) shelfOutput;
        SimpleValueTargetProvider valueTargetProvider3;
        object obj5 = (object) (valueTargetProvider3 = new SimpleValueTargetProvider(objArray3, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider3.Add(type5, (object) valueTargetProvider3);
        xamlServiceProvider3.Add(typeof (IReferenceProvider), obj5);
        Type type6 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver3 = new XmlNamespaceResolver();
        namespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver3.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (ShelfOutput).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(12, 99)));
        object obj6 = ((IMarkupExtension) referenceExtension10).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension2.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 99);
        bindingExtension2.Path = "TextColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(Button.TextColorProperty, bindingBase2);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 18);
        ((BindableObject) button2).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button2).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        ((BindableObject) button2).SetValue(Button.TextProperty, (object) "x");
        ((BindableObject) button2).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Button button5 = button2;
        BindableProperty fontSizeProperty2 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter2 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 4];
        objArray4[0] = (object) button2;
        objArray4[1] = (object) stackLayout1;
        objArray4[2] = (object) stackLayout7;
        objArray4[3] = (object) shelfOutput;
        SimpleValueTargetProvider valueTargetProvider4;
        object obj7 = (object) (valueTargetProvider4 = new SimpleValueTargetProvider(objArray4, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider4.Add(type7, (object) valueTargetProvider4);
        xamlServiceProvider4.Add(typeof (IReferenceProvider), obj7);
        Type type8 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver4 = new XmlNamespaceResolver();
        namespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver4.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (ShelfOutput).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(13, 125)));
        object obj8 = ((IExtendedTypeConverter) fontSizeConverter2).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider4);
        ((BindableObject) button5).SetValue(fontSizeProperty2, obj8);
        button2.Clicked += new EventHandler(shelfOutput.btnClearDocumentNumber_Clicked);
        referenceExtension3.Name = "shelfoutput";
        ReferenceExtension referenceExtension11 = referenceExtension3;
        XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
        Type type9 = typeof (IProvideValueTarget);
        object[] objArray5 = new object[0 + 5];
        objArray5[0] = (object) bindingExtension3;
        objArray5[1] = (object) button2;
        objArray5[2] = (object) stackLayout1;
        objArray5[3] = (object) stackLayout7;
        objArray5[4] = (object) shelfOutput;
        SimpleValueTargetProvider valueTargetProvider5;
        object obj9 = (object) (valueTargetProvider5 = new SimpleValueTargetProvider(objArray5, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider5.Add(type9, (object) valueTargetProvider5);
        xamlServiceProvider5.Add(typeof (IReferenceProvider), obj9);
        Type type10 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver5 = new XmlNamespaceResolver();
        namespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver5.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver5 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver5, typeof (ShelfOutput).GetTypeInfo().Assembly);
        xamlServiceProvider5.Add(type10, (object) xamlTypeResolver5);
        xamlServiceProvider5.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(14, 25)));
        object obj10 = ((IMarkupExtension) referenceExtension11).ProvideValue((IServiceProvider) xamlServiceProvider5);
        bindingExtension3.Source = obj10;
        VisualDiagnostics.RegisterSourceInfo(obj10, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 25);
        bindingExtension3.Path = "ButtonColor";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase3);
        referenceExtension4.Name = "shelfoutput";
        ReferenceExtension referenceExtension12 = referenceExtension4;
        XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
        Type type11 = typeof (IProvideValueTarget);
        object[] objArray6 = new object[0 + 5];
        objArray6[0] = (object) bindingExtension4;
        objArray6[1] = (object) button2;
        objArray6[2] = (object) stackLayout1;
        objArray6[3] = (object) stackLayout7;
        objArray6[4] = (object) shelfOutput;
        SimpleValueTargetProvider valueTargetProvider6;
        object obj11 = (object) (valueTargetProvider6 = new SimpleValueTargetProvider(objArray6, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider6.Add(type11, (object) valueTargetProvider6);
        xamlServiceProvider6.Add(typeof (IReferenceProvider), obj11);
        Type type12 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver6 = new XmlNamespaceResolver();
        namespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver6.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver6 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver6, typeof (ShelfOutput).GetTypeInfo().Assembly);
        xamlServiceProvider6.Add(type12, (object) xamlTypeResolver6);
        xamlServiceProvider6.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(14, 99)));
        object obj12 = ((IMarkupExtension) referenceExtension12).ProvideValue((IServiceProvider) xamlServiceProvider6);
        bindingExtension4.Source = obj12;
        VisualDiagnostics.RegisterSourceInfo(obj12, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 99);
        bindingExtension4.Path = "TextColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(Button.TextColorProperty, bindingBase4);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button2);
        VisualDiagnostics.RegisterSourceInfo((object) button2, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 9, 14);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf No Giriniz/Okutunuz");
        softkeyboardDisabledEntry1.Completed += new EventHandler(shelfOutput.txtShelf_Completed);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 14);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout3).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod Okutunuz");
        softkeyboardDisabledEntry2.Completed += new EventHandler(shelfOutput.txtBarvode_Completed);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 18);
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.TextProperty, (object) "1");
        ((BindableObject) entry2).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Miktar");
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 21, 18);
        ((BindableObject) picker).SetValue(Picker.TitleProperty, (object) "Tip");
        bindingExtension5.Path = ".";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker).SetBinding(Picker.ItemsSourceProperty, bindingBase5);
        bindingExtension6.Path = "Caption";
        BindingBase bindingBase6 = ((IMarkupExtension<BindingBase>) bindingExtension6).ProvideValue((IServiceProvider) null);
        picker.ItemDisplayBinding = bindingBase6;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase6, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 33);
        ((BindableObject) picker).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) picker).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) picker);
        VisualDiagnostics.RegisterSourceInfo((object) picker, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 22, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 19, 14);
        ((BindableObject) stackLayout4).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) button3).SetValue(Button.TextProperty, (object) "Tamamla");
        ((BindableObject) button3).SetValue(VisualElement.HeightRequestProperty, (object) 80.0);
        button3.Clicked += new EventHandler(shelfOutput.btnSuccess_Clicked);
        referenceExtension5.Name = "shelfoutput";
        ReferenceExtension referenceExtension13 = referenceExtension5;
        XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
        Type type13 = typeof (IProvideValueTarget);
        object[] objArray7 = new object[0 + 5];
        objArray7[0] = (object) bindingExtension7;
        objArray7[1] = (object) button3;
        objArray7[2] = (object) stackLayout4;
        objArray7[3] = (object) stackLayout7;
        objArray7[4] = (object) shelfOutput;
        SimpleValueTargetProvider valueTargetProvider7;
        object obj13 = (object) (valueTargetProvider7 = new SimpleValueTargetProvider(objArray7, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider7.Add(type13, (object) valueTargetProvider7);
        xamlServiceProvider7.Add(typeof (IReferenceProvider), obj13);
        Type type14 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver7 = new XmlNamespaceResolver();
        namespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver7.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver7 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver7, typeof (ShelfOutput).GetTypeInfo().Assembly);
        xamlServiceProvider7.Add(type14, (object) xamlTypeResolver7);
        xamlServiceProvider7.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(28, 25)));
        object obj14 = ((IMarkupExtension) referenceExtension13).ProvideValue((IServiceProvider) xamlServiceProvider7);
        bindingExtension7.Source = obj14;
        VisualDiagnostics.RegisterSourceInfo(obj14, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 25);
        bindingExtension7.Path = "ButtonColor";
        BindingBase bindingBase7 = ((IMarkupExtension<BindingBase>) bindingExtension7).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(VisualElement.BackgroundColorProperty, bindingBase7);
        referenceExtension6.Name = "shelfoutput";
        ReferenceExtension referenceExtension14 = referenceExtension6;
        XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
        Type type15 = typeof (IProvideValueTarget);
        object[] objArray8 = new object[0 + 5];
        objArray8[0] = (object) bindingExtension8;
        objArray8[1] = (object) button3;
        objArray8[2] = (object) stackLayout4;
        objArray8[3] = (object) stackLayout7;
        objArray8[4] = (object) shelfOutput;
        SimpleValueTargetProvider valueTargetProvider8;
        object obj15 = (object) (valueTargetProvider8 = new SimpleValueTargetProvider(objArray8, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider8.Add(type15, (object) valueTargetProvider8);
        xamlServiceProvider8.Add(typeof (IReferenceProvider), obj15);
        Type type16 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver8 = new XmlNamespaceResolver();
        namespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver8.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver8 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver8, typeof (ShelfOutput).GetTypeInfo().Assembly);
        xamlServiceProvider8.Add(type16, (object) xamlTypeResolver8);
        xamlServiceProvider8.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(28, 99)));
        object obj16 = ((IMarkupExtension) referenceExtension14).ProvideValue((IServiceProvider) xamlServiceProvider8);
        bindingExtension8.Source = obj16;
        VisualDiagnostics.RegisterSourceInfo(obj16, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 99);
        bindingExtension8.Path = "TextColor";
        BindingBase bindingBase8 = ((IMarkupExtension<BindingBase>) bindingExtension8).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(Button.TextColorProperty, bindingBase8);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) button3);
        VisualDiagnostics.RegisterSourceInfo((object) button3, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 27, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 26, 14);
        ((BindableObject) listView).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 500.0);
        ((BindableObject) listView).SetValue(ListView.RowHeightProperty, (object) 80);
        ((BindableObject) label).SetValue(Label.TextProperty, (object) "Eklenen Ürünler");
        ((BindableObject) label).SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        referenceExtension7.Name = "shelfoutput";
        ReferenceExtension referenceExtension15 = referenceExtension7;
        XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
        Type type17 = typeof (IProvideValueTarget);
        object[] objArray9 = new object[0 + 7];
        objArray9[0] = (object) bindingExtension9;
        objArray9[1] = (object) label;
        objArray9[2] = (object) stackLayout5;
        objArray9[3] = (object) listView;
        objArray9[4] = (object) stackLayout6;
        objArray9[5] = (object) stackLayout7;
        objArray9[6] = (object) shelfOutput;
        SimpleValueTargetProvider valueTargetProvider9;
        object obj17 = (object) (valueTargetProvider9 = new SimpleValueTargetProvider(objArray9, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider9.Add(type17, (object) valueTargetProvider9);
        xamlServiceProvider9.Add(typeof (IReferenceProvider), obj17);
        Type type18 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver9 = new XmlNamespaceResolver();
        namespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver9.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver9 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver9, typeof (ShelfOutput).GetTypeInfo().Assembly);
        xamlServiceProvider9.Add(type18, (object) xamlTypeResolver9);
        xamlServiceProvider9.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(36, 36)));
        object obj18 = ((IMarkupExtension) referenceExtension15).ProvideValue((IServiceProvider) xamlServiceProvider9);
        bindingExtension9.Source = obj18;
        VisualDiagnostics.RegisterSourceInfo(obj18, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 36, 36);
        bindingExtension9.Path = "ButtonColor";
        BindingBase bindingBase9 = ((IMarkupExtension<BindingBase>) bindingExtension9).ProvideValue((IServiceProvider) null);
        ((BindableObject) label).SetBinding(Label.TextColorProperty, bindingBase9);
        referenceExtension8.Name = "shelfoutput";
        ReferenceExtension referenceExtension16 = referenceExtension8;
        XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
        Type type19 = typeof (IProvideValueTarget);
        object[] objArray10 = new object[0 + 7];
        objArray10[0] = (object) bindingExtension10;
        objArray10[1] = (object) label;
        objArray10[2] = (object) stackLayout5;
        objArray10[3] = (object) listView;
        objArray10[4] = (object) stackLayout6;
        objArray10[5] = (object) stackLayout7;
        objArray10[6] = (object) shelfOutput;
        SimpleValueTargetProvider valueTargetProvider10;
        object obj19 = (object) (valueTargetProvider10 = new SimpleValueTargetProvider(objArray10, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider10.Add(type19, (object) valueTargetProvider10);
        xamlServiceProvider10.Add(typeof (IReferenceProvider), obj19);
        Type type20 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver10 = new XmlNamespaceResolver();
        namespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver10.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver10 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver10, typeof (ShelfOutput).GetTypeInfo().Assembly);
        xamlServiceProvider10.Add(type20, (object) xamlTypeResolver10);
        xamlServiceProvider10.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(37, 36)));
        object obj20 = ((IMarkupExtension) referenceExtension16).ProvideValue((IServiceProvider) xamlServiceProvider10);
        bindingExtension10.Source = obj20;
        VisualDiagnostics.RegisterSourceInfo(obj20, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 37, 36);
        bindingExtension10.Path = "TextColor";
        BindingBase bindingBase10 = ((IMarkupExtension<BindingBase>) bindingExtension10).ProvideValue((IServiceProvider) null);
        ((BindableObject) label).SetBinding(VisualElement.BackgroundColorProperty, bindingBase10);
        ((BindableObject) label).SetValue(Label.FontProperty, new FontTypeConverter().ConvertFromInvariantString("Bold,20"));
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) label);
        VisualDiagnostics.RegisterSourceInfo((object) label, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 34, 30);
        ((BindableObject) listView).SetValue(ListView.HeaderProperty, (object) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 33, 26);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ShelfOutput.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_34 xamlCdataTemplate34 = new ShelfOutput.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_34();
        object[] objArray11 = new object[0 + 5];
        objArray11[0] = (object) dataTemplate1;
        objArray11[1] = (object) listView;
        objArray11[2] = (object) stackLayout6;
        objArray11[3] = (object) stackLayout7;
        objArray11[4] = (object) shelfOutput;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate34.parentValues = objArray11;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate34.root = shelfOutput;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate34.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 42, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 31, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 30, 14);
        ((BindableObject) shelfOutput).SetValue(ContentPage.ContentProperty, (object) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 8, 10);
        VisualDiagnostics.RegisterSourceInfo((object) shelfOutput, new Uri("Views\\ShelfOutput.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Extensions.LoadFromXaml<ShelfOutput>(this, typeof (ShelfOutput));
      this.shelfoutput = NameScopeExtensions.FindByName<ContentPage>((Element) this, "shelfoutput");
      this.stckDocumentNumber = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckDocumentNumber");
      this.txtDocumentNumber = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtDocumentNumber");
      this.btnCreateDocumentNumber = NameScopeExtensions.FindByName<Button>((Element) this, "btnCreateDocumentNumber");
      this.btnClearDocumentNumber = NameScopeExtensions.FindByName<Button>((Element) this, "btnClearDocumentNumber");
      this.txtShelf = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtShelf");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.txtQty = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtQty");
      this.pckBarcodeType = NameScopeExtensions.FindByName<Picker>((Element) this, "pckBarcodeType");
      this.stckSuccess = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckSuccess");
      this.btnSuccess = NameScopeExtensions.FindByName<Button>((Element) this, "btnSuccess");
      this.lstProducts = NameScopeExtensions.FindByName<ListView>((Element) this, "lstProducts");
      this.lblShelfCountingDetailHeader = NameScopeExtensions.FindByName<Label>((Element) this, "lblShelfCountingDetailHeader");
    }
  }
}
