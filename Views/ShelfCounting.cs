// Decompiled with JetBrains decompiler
// Type: Shelf.Views.ShelfCounting
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
  [XamlFilePath("Views\\ShelfCounting.xaml")]
  public class ShelfCounting : ContentPage
  {
    public string CountingHeaderText = "asdas";
    private pIOGetShelfCountingReturnModel selectedCounting;
    private List<pIOGetShelfCountingDetailReturnModel> shelfCountingDetailList;
    private ztIOShelf selectShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage shelfCounting;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelfCounting;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfCounting;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckWarehouse;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private DatePicker dtShelfCounting;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnClear;
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
    private StackLayout stckShelfClear;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnShelfClear;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelfCountingDetailList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfCountingDetail;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Label lblShelfCountingDetailHeader;

    public Color ButtonColor => Color.FromRgb(21, 40, 151);

    public Color TextColor => Color.White;

    public ShelfCounting()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Raf Sayım";
      GlobalMob.AddBarcodeLongPress(this.txtBarcode);
      GlobalMob.AddShelfBarcodeLongPress((Xamarin.Forms.Entry) this.txtShelf);
      this.LoadWarehouse();
      ((VisualElement) this.txtQty).IsVisible = !GlobalMob.User.HideQty;
      GlobalMob.FillBarcodeType(this.pckBarcodeType);
      ((VisualElement) this.btnShelfClear).IsVisible = GlobalMob.User.ShelfCountClearVisible;
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
      ((VisualElement) this.txtShelf).Focus();
    }

    private void txtShelf_Completed(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(((InputView) this.txtShelf).Text))
        return;
      string str = "";
      if (!this.ShelfCodeControl())
        return;
      if (this.pckWarehouse.SelectedItem != null)
        str = ((ztIOShelfWarehouse) this.pckWarehouse.SelectedItem).WarehouseCode;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfCounting?date={0}&userName={1}&warehouseCode={2}", (object) this.dtShelfCounting.Date.ToString("dd/MM/yyyy"), (object) GlobalMob.User.UserName, (object) str), (ContentPage) this);
      if (returnModel.Success)
      {
        List<pIOGetShelfCountingReturnModel> countingReturnModelList = GlobalMob.JsonDeserialize<List<pIOGetShelfCountingReturnModel>>(returnModel.Result);
        if (countingReturnModelList.Count == 1)
        {
          this.selectedCounting = countingReturnModelList[0];
          ((VisualElement) this.stckShelfCounting).IsVisible = false;
          ((VisualElement) this.stckForm).IsVisible = true;
          ((VisualElement) this.stckShelfCountingDetailList).IsVisible = true;
          this.GetDetailShelfList();
        }
        else if (countingReturnModelList.Count > 1)
        {
          ((VisualElement) this.stckShelfCounting).IsVisible = true;
          ((VisualElement) this.stckForm).IsVisible = false;
          ((Page) this).Title = "Raf Sayım Emirleri";
          ((ItemsView<Cell>) this.lstShelfCounting).ItemsSource = (IEnumerable) countingReturnModelList;
          this.lstShelfCounting.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(this.LstShelfCounting_ItemSelected);
        }
        else
        {
          GlobalMob.PlayError();
          ((Page) this).DisplayAlert("Hata", "Raf Bulunamadı", "", "Tamam");
          ((InputView) this.txtShelf).Text = "";
          ((VisualElement) this.txtShelf).Focus();
          return;
        }
      }
      ((VisualElement) this.stckBarcode).IsVisible = true;
      ((VisualElement) this.stckSuccess).IsVisible = true;
      this.BarcodeFocus(500);
    }

    private void GetDetailShelfList()
    {
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfCountingDetail?countingId={0}&shelfCode={1}", (object) this.selectedCounting.CountingID, (object) ((InputView) this.txtShelf).Text), (ContentPage) this);
      if (!returnModel.Success)
        return;
      this.shelfCountingDetailList = GlobalMob.JsonDeserialize<List<pIOGetShelfCountingDetailReturnModel>>(returnModel.Result);
      this.FillListview();
    }

    private bool ShelfCodeControl()
    {
      bool flag = false;
      if (!string.IsNullOrEmpty(((InputView) this.txtShelf).Text))
      {
        ReturnModel shelf = GlobalMob.GetShelf(((InputView) this.txtShelf).Text, (ContentPage) this);
        if (shelf.Success)
        {
          if (!string.IsNullOrEmpty(shelf.Result))
          {
            this.selectShelf = JsonConvert.DeserializeObject<ztIOShelf>(shelf.Result);
            ((InputView) this.txtShelf).Text = this.selectShelf.Description;
            flag = true;
          }
          else
          {
            flag = false;
            this.selectShelf = (ztIOShelf) null;
            ((Page) this).DisplayAlert("Bilgi", "Hatalı Raf", "", "Tamam");
            ((InputView) this.txtShelf).Text = "";
            ((VisualElement) this.txtShelf).Focus();
          }
        }
      }
      return flag;
    }

    private void LstShelfCounting_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      object selectedItem = ((ListView) sender).SelectedItem;
      if (selectedItem == null)
        return;
      ((Page) this).Title = "Raf Sayım";
      this.selectedCounting = (pIOGetShelfCountingReturnModel) selectedItem;
      ((VisualElement) this.stckShelfCounting).IsVisible = false;
      ((VisualElement) this.stckForm).IsVisible = true;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfCountingDetail?countingId={0}&shelfCode={1}", (object) this.selectedCounting.CountingID, (object) ((InputView) this.txtShelf).Text), (ContentPage) this);
      if (!returnModel.Success)
        return;
      this.shelfCountingDetailList = GlobalMob.JsonDeserialize<List<pIOGetShelfCountingDetailReturnModel>>(returnModel.Result);
      this.FillListview();
    }

    private void txtBarcode_Completed(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(((InputView) this.txtBarcode).Text))
      {
        if (this.selectShelf == null)
        {
          ((Page) this).DisplayAlert("Bilgi", "Lütfen Raf Okutunuz", "", "Tamam");
          GlobalMob.PlayError();
          ((InputView) this.txtShelf).Text = "";
          ((VisualElement) this.txtShelf).Focus();
          return;
        }
        PickerItem selectedItem = (PickerItem) this.pckBarcodeType.SelectedItem;
        ztIOShelfCountingDetail shelfCountingDetail = new ztIOShelfCountingDetail()
        {
          CountingID = this.selectedCounting.CountingID,
          CreatedUserName = GlobalMob.User.UserName
        };
        shelfCountingDetail.UpdatedUserName = shelfCountingDetail.CreatedUserName;
        shelfCountingDetail.Qty = (double) Convert.ToInt32(((InputView) this.txtQty).Text);
        shelfCountingDetail.ShelfCode = this.selectShelf.Code;
        shelfCountingDetail.UsedBarcode = ((InputView) this.txtBarcode).Text;
        bool flag = false;
        if (selectedItem != null && selectedItem.Code == 2 && ((VisualElement) this.pckBarcodeType).IsVisible)
        {
          flag = true;
          shelfCountingDetail.CountingDetailID = 1;
        }
        if (selectedItem != null && selectedItem.Code == 3 && ((VisualElement) this.pckBarcodeType).IsVisible)
          shelfCountingDetail.CountingDetailID = 2;
        ReturnModel result = GlobalMob.PostJsonWithParam("SaveShelfCounting", new Dictionary<string, string>()
        {
          {
            "detail",
            JsonConvert.SerializeObject((object) shelfCountingDetail)
          },
          {
            "date",
            this.dtShelfCounting.Date.ToString("dd.MM.yyyy")
          }
        }).Result;
        if (result.Success)
        {
          List<ShelfCountingResult> source = new List<ShelfCountingResult>();
          if (flag)
          {
            source = JsonConvert.DeserializeObject<List<ShelfCountingResult>>(result.Result);
          }
          else
          {
            if (!string.IsNullOrEmpty(result.Result))
            {
              if (!result.Result.Contains("["))
              {
                try
                {
                  ShelfCountingResult shelfCountingResult = JsonConvert.DeserializeObject<ShelfCountingResult>(result.Result);
                  source.Add(shelfCountingResult);
                  goto label_15;
                }
                catch (Exception ex)
                {
                  source = JsonConvert.DeserializeObject<List<ShelfCountingResult>>(result.Result);
                  goto label_15;
                }
              }
            }
            source = JsonConvert.DeserializeObject<List<ShelfCountingResult>>(result.Result);
          }
label_15:
          if (source == null)
          {
            ((Page) this).DisplayAlert("Bilgi", "Hata Oluştu", "", "Tamam");
            GlobalMob.PlayError();
            this.BarcodeFocus(100);
            return;
          }
          if (source.Count<ShelfCountingResult>() <= 0)
          {
            ((Page) this).DisplayAlert("Bilgi", "Ürün Bulunamadı", "", "Tamam");
            GlobalMob.PlayError();
            this.BarcodeFocus(100);
            return;
          }
          if (this.selectedCounting.CountingID <= 0)
            this.selectedCounting = new pIOGetShelfCountingReturnModel()
            {
              CountingID = source[0].CountingID
            };
          this.shelfCountingDetailList = this.shelfCountingDetailList.Select<pIOGetShelfCountingDetailReturnModel, pIOGetShelfCountingDetailReturnModel>((Func<pIOGetShelfCountingDetailReturnModel, pIOGetShelfCountingDetailReturnModel>) (c =>
          {
            c.LastReadBarcode = false;
            return c;
          })).ToList<pIOGetShelfCountingDetailReturnModel>();
          foreach (ShelfCountingResult shelfCountingResult in source)
            this.shelfCountingDetailList.Add(new pIOGetShelfCountingDetailReturnModel()
            {
              CountingDetailID = shelfCountingResult.CountingDetailID,
              CountingID = shelfCountingResult.CountingID,
              Qty = shelfCountingResult.Qty,
              ShelfCode = ((InputView) this.txtShelf).Text,
              UsedBarcode = !string.IsNullOrEmpty(shelfCountingResult.UsedBarcode) ? shelfCountingResult.UsedBarcode : ((InputView) this.txtBarcode).Text,
              ItemCode = shelfCountingResult.ItemCode,
              ColorCode = shelfCountingResult.ColorCode,
              ItemDim1Code = shelfCountingResult.ItemDim1Code,
              ItemDim2Code = shelfCountingResult.ItemDim2Code,
              ItemDim3Code = shelfCountingResult.ItemDim3Code,
              LastReadBarcode = true
            });
          GlobalMob.PlaySave();
          this.FillListview();
          ((InputView) this.txtQty).Text = "1";
        }
      }
      this.BarcodeFocus(100);
    }

    private void FillListview()
    {
      IEnumerable<pIOGetShelfCountingDetailReturnModel> source = this.shelfCountingDetailList.GroupBy(c => new
      {
        UsedBarcode = c.UsedBarcode,
        ItemCode = c.ItemCode,
        ColorCode = c.ColorCode,
        ItemDim1Code = c.ItemDim1Code
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType19<string, string, string, string>, pIOGetShelfCountingDetailReturnModel>, pIOGetShelfCountingDetailReturnModel>(gcs => new pIOGetShelfCountingDetailReturnModel()
      {
        UsedBarcode = gcs.Key.UsedBarcode,
        ItemCode = gcs.Key.ItemCode,
        ColorCode = gcs.Key.ColorCode,
        ItemDim1Code = gcs.Key.ItemDim1Code,
        LastReadBarcode = gcs.Max<pIOGetShelfCountingDetailReturnModel, bool>((Func<pIOGetShelfCountingDetailReturnModel, bool>) (x => x.LastReadBarcode)),
        Qty = gcs.Sum<pIOGetShelfCountingDetailReturnModel>((Func<pIOGetShelfCountingDetailReturnModel, double>) (x => x.Qty)),
        childList = gcs.ToList<pIOGetShelfCountingDetailReturnModel>()
      });
      ((ItemsView<Cell>) this.lstShelfCountingDetail).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstShelfCountingDetail).ItemsSource = (IEnumerable) source.OrderByDescending<pIOGetShelfCountingDetailReturnModel, bool>((Func<pIOGetShelfCountingDetailReturnModel, bool>) (x => x.LastReadBarcode)).ToList<pIOGetShelfCountingDetailReturnModel>();
      this.CountingHeaderText = source.Count<pIOGetShelfCountingDetailReturnModel>() > 0 ? "Toplam Miktar : " + Convert.ToInt32(this.shelfCountingDetailList.Sum<pIOGetShelfCountingDetailReturnModel>((Func<pIOGetShelfCountingDetailReturnModel, double>) (x => x.Qty))).ToString() : "";
      this.lblShelfCountingDetailHeader.Text = this.CountingHeaderText;
      ((VisualElement) this.stckShelfClear).IsVisible = GlobalMob.User.ShelfSyncVisible;
    }

    private async void BarcodeFocus(int time) => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(time);
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode)?.Focus();
    }));

    private void btnSuccess_Clicked(object sender, EventArgs e)
    {
      ((VisualElement) this.stckShelfCountingDetailList).IsVisible = false;
      ((VisualElement) this.stckShelfCounting).IsVisible = false;
      ((VisualElement) this.stckForm).IsVisible = true;
      this.shelfCountingDetailList = new List<pIOGetShelfCountingDetailReturnModel>();
      this.FillListview();
      ((VisualElement) this.stckBarcode).IsVisible = false;
      ((VisualElement) this.stckSuccess).IsVisible = false;
      ((InputView) this.txtShelf).Text = "";
      ((VisualElement) this.txtShelf).Focus();
    }

    private void CmDelete_Clicked(object sender, EventArgs e)
    {
      string barcode = Convert.ToString(((MenuItem) sender).CommandParameter);
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("DeleteShelfCountingDetails?ids={0}", (object) string.Join<int>(",", (IEnumerable<int>) this.shelfCountingDetailList.Where<pIOGetShelfCountingDetailReturnModel>((Func<pIOGetShelfCountingDetailReturnModel, bool>) (x => x.UsedBarcode == barcode)).ToList<pIOGetShelfCountingDetailReturnModel>().Select<pIOGetShelfCountingDetailReturnModel, int>((Func<pIOGetShelfCountingDetailReturnModel, int>) (x => x.CountingDetailID)).ToArray<int>())), (ContentPage) this);
      if (returnModel.Success)
      {
        bool boolean = Convert.ToBoolean(returnModel.Result);
        ((Page) this).DisplayAlert("Bilgi", boolean ? "Ürün Silindi" : "Hata Oluştu", "", "Tamam");
        if (boolean)
          this.shelfCountingDetailList.RemoveAll((Predicate<pIOGetShelfCountingDetailReturnModel>) (x => x.UsedBarcode == barcode));
        this.FillListview();
      }
      this.BarcodeFocus(250);
    }

    private void LoadWarehouse()
    {
      ReturnModel returnModel = GlobalMob.PostJson("GetWarehouseList", (ContentPage) this);
      if (!returnModel.Success)
        return;
      List<ztIOShelfWarehouse> ioShelfWarehouseList = JsonConvert.DeserializeObject<List<ztIOShelfWarehouse>>(returnModel.Result);
      this.pckWarehouse.ItemsSource = (IList) ioShelfWarehouseList;
      if (ioShelfWarehouseList.Count <= 0)
        return;
      this.pckWarehouse.SelectedIndex = 0;
    }

    private async void btnShelfClear_Clicked(object sender, EventArgs e)
    {
      ShelfCounting page = this;
      if (page.selectShelf == null)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Lütfen Raf Okutunuz", "", "Tamam") ? 1 : 0;
        GlobalMob.PlayError();
        ((InputView) page.txtBarcode).Text = "";
        page.ShelfFocus();
      }
      else
      {
        if (!await ((Page) page).DisplayAlert("Devam?", "Raftaki tüm envanter sıfırlanacak.Emin misiniz?", "Evet", "Hayır"))
          return;
        string str = "";
        int num1 = 0;
        if (page.selectedCounting != null)
        {
          str = page.selectedCounting.CountingNumber;
          num1 = page.selectedCounting.CountingID;
        }
        ShelfCountingShelfClear clear = new ShelfCountingShelfClear()
        {
          ShelfID = page.selectShelf.ShelfID,
          UserName = GlobalMob.User.UserName,
          DocumentNumber = str,
          ShelfCountingID = num1
        };
        await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
        ReturnModel result = GlobalMob.PostJson("ClearShelfInventory", new Dictionary<string, string>()
        {
          {
            "json",
            JsonConvert.SerializeObject((object) clear)
          }
        }, (ContentPage) page).Result;
        if (result.Success)
        {
          ReturnModel returnModel = JsonConvert.DeserializeObject<ReturnModel>(result.Result);
          if (returnModel.Success)
          {
            GlobalMob.PlaySave();
            int num2 = await ((Page) page).DisplayAlert("Bilgi", "Raf Sıfırlandı", "", "Tamam") ? 1 : 0;
            page.BarcodeFocus(250);
          }
          else
          {
            GlobalMob.PlayError();
            int num3 = await ((Page) page).DisplayAlert("Hata", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
            page.BarcodeFocus(250);
          }
        }
        GlobalMob.CloseLoading();
        clear = (ShelfCountingShelfClear) null;
      }
    }

    private void btnClear_Clicked(object sender, EventArgs e)
    {
      this.selectShelf = (ztIOShelf) null;
      this.ShelfFocus();
    }

    private void ShelfFocus()
    {
      ((InputView) this.txtShelf).Text = "";
      ((VisualElement) this.txtShelf).Focus();
    }

    private async void cmAllDelete_Clicked(object sender, EventArgs e)
    {
      ShelfCounting page = this;
      if (!await ((Page) page).DisplayAlert("Devam?", "Raftaki tüm sayım sıfırlanacak.Emin misiniz?", "Evet", "Hayır"))
        return;
      await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("DeleteShelfCountingDetails?ids={0}", (object) string.Join<int>(",", (IEnumerable<int>) page.shelfCountingDetailList.Select<pIOGetShelfCountingDetailReturnModel, int>((Func<pIOGetShelfCountingDetailReturnModel, int>) (x => x.CountingDetailID)).ToArray<int>())), (ContentPage) page);
      if (returnModel.Success)
      {
        bool boolean = Convert.ToBoolean(returnModel.Result);
        int num = await ((Page) page).DisplayAlert("Bilgi", boolean ? "Ürünler Sayımdan Silindi" : "Hata Oluştu", "", "Tamam") ? 1 : 0;
        page.GetDetailShelfList();
      }
      GlobalMob.CloseLoading();
      page.BarcodeFocus(250);
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (ShelfCounting).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/ShelfCounting.xaml",
        Instance = (object) this
      }))
        this.__InitComponentRuntime();
      else if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(((object) this).GetType()) != null)
      {
        this.__InitComponentRuntime();
      }
      else
      {
        BindingExtension bindingExtension1 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView1 = new ListView();
        StackLayout stackLayout1 = new StackLayout();
        BindingExtension bindingExtension2 = new BindingExtension();
        BindingExtension bindingExtension3 = new BindingExtension();
        Picker picker1 = new Picker();
        StackLayout stackLayout2 = new StackLayout();
        DatePicker datePicker = new DatePicker();
        StackLayout stackLayout3 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry1 = new SoftkeyboardDisabledEntry();
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension4 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension5 = new BindingExtension();
        Button button1 = new Button();
        StackLayout stackLayout4 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry2 = new SoftkeyboardDisabledEntry();
        Xamarin.Forms.Entry entry = new Xamarin.Forms.Entry();
        BindingExtension bindingExtension6 = new BindingExtension();
        BindingExtension bindingExtension7 = new BindingExtension();
        Picker picker2 = new Picker();
        StackLayout stackLayout5 = new StackLayout();
        ReferenceExtension referenceExtension3 = new ReferenceExtension();
        BindingExtension bindingExtension8 = new BindingExtension();
        ReferenceExtension referenceExtension4 = new ReferenceExtension();
        BindingExtension bindingExtension9 = new BindingExtension();
        Button button2 = new Button();
        StackLayout stackLayout6 = new StackLayout();
        ReferenceExtension referenceExtension5 = new ReferenceExtension();
        BindingExtension bindingExtension10 = new BindingExtension();
        ReferenceExtension referenceExtension6 = new ReferenceExtension();
        BindingExtension bindingExtension11 = new BindingExtension();
        Button button3 = new Button();
        StackLayout stackLayout7 = new StackLayout();
        BindingExtension bindingExtension12 = new BindingExtension();
        BindingExtension bindingExtension13 = new BindingExtension();
        ReferenceExtension referenceExtension7 = new ReferenceExtension();
        BindingExtension bindingExtension14 = new BindingExtension();
        ReferenceExtension referenceExtension8 = new ReferenceExtension();
        BindingExtension bindingExtension15 = new BindingExtension();
        Label label = new Label();
        StackLayout stackLayout8 = new StackLayout();
        DataTemplate dataTemplate2 = new DataTemplate();
        ListView listView2 = new ListView();
        StackLayout stackLayout9 = new StackLayout();
        StackLayout stackLayout10 = new StackLayout();
        StackLayout stackLayout11 = new StackLayout();
        ShelfCounting shelfCounting;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (shelfCounting = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) shelfCounting, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("shelfCounting", (object) shelfCounting);
        if (((Element) shelfCounting).StyleId == null)
          ((Element) shelfCounting).StyleId = "shelfCounting";
        ((INameScope) nameScope).RegisterName("stckShelfCounting", (object) stackLayout1);
        if (((Element) stackLayout1).StyleId == null)
          ((Element) stackLayout1).StyleId = "stckShelfCounting";
        ((INameScope) nameScope).RegisterName("lstShelfCounting", (object) listView1);
        if (((Element) listView1).StyleId == null)
          ((Element) listView1).StyleId = "lstShelfCounting";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout10);
        if (((Element) stackLayout10).StyleId == null)
          ((Element) stackLayout10).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("pckWarehouse", (object) picker1);
        if (((Element) picker1).StyleId == null)
          ((Element) picker1).StyleId = "pckWarehouse";
        ((INameScope) nameScope).RegisterName("dtShelfCounting", (object) datePicker);
        if (((Element) datePicker).StyleId == null)
          ((Element) datePicker).StyleId = "dtShelfCounting";
        ((INameScope) nameScope).RegisterName("txtShelf", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtShelf";
        ((INameScope) nameScope).RegisterName("btnClear", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnClear";
        ((INameScope) nameScope).RegisterName("stckBarcode", (object) stackLayout5);
        if (((Element) stackLayout5).StyleId == null)
          ((Element) stackLayout5).StyleId = "stckBarcode";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("txtQty", (object) entry);
        if (((Element) entry).StyleId == null)
          ((Element) entry).StyleId = "txtQty";
        ((INameScope) nameScope).RegisterName("pckBarcodeType", (object) picker2);
        if (((Element) picker2).StyleId == null)
          ((Element) picker2).StyleId = "pckBarcodeType";
        ((INameScope) nameScope).RegisterName("stckSuccess", (object) stackLayout6);
        if (((Element) stackLayout6).StyleId == null)
          ((Element) stackLayout6).StyleId = "stckSuccess";
        ((INameScope) nameScope).RegisterName("btnSuccess", (object) button2);
        if (((Element) button2).StyleId == null)
          ((Element) button2).StyleId = "btnSuccess";
        ((INameScope) nameScope).RegisterName("stckShelfClear", (object) stackLayout7);
        if (((Element) stackLayout7).StyleId == null)
          ((Element) stackLayout7).StyleId = "stckShelfClear";
        ((INameScope) nameScope).RegisterName("btnShelfClear", (object) button3);
        if (((Element) button3).StyleId == null)
          ((Element) button3).StyleId = "btnShelfClear";
        ((INameScope) nameScope).RegisterName("stckShelfCountingDetailList", (object) stackLayout9);
        if (((Element) stackLayout9).StyleId == null)
          ((Element) stackLayout9).StyleId = "stckShelfCountingDetailList";
        ((INameScope) nameScope).RegisterName("lstShelfCountingDetail", (object) listView2);
        if (((Element) listView2).StyleId == null)
          ((Element) listView2).StyleId = "lstShelfCountingDetail";
        ((INameScope) nameScope).RegisterName("lblShelfCountingDetailHeader", (object) label);
        if (((Element) label).StyleId == null)
          ((Element) label).StyleId = "lblShelfCountingDetailHeader";
        this.shelfCounting = (ContentPage) shelfCounting;
        this.stckShelfCounting = stackLayout1;
        this.lstShelfCounting = listView1;
        this.stckForm = stackLayout10;
        this.pckWarehouse = picker1;
        this.dtShelfCounting = datePicker;
        this.txtShelf = softkeyboardDisabledEntry1;
        this.btnClear = button1;
        this.stckBarcode = stackLayout5;
        this.txtBarcode = softkeyboardDisabledEntry2;
        this.txtQty = entry;
        this.pckBarcodeType = picker2;
        this.stckSuccess = stackLayout6;
        this.btnSuccess = button2;
        this.stckShelfClear = stackLayout7;
        this.btnShelfClear = button3;
        this.stckShelfCountingDetailList = stackLayout9;
        this.lstShelfCountingDetail = listView2;
        this.lblShelfCountingDetailHeader = label;
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) stackLayout1).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        bindingExtension1.Path = ".";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView1).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase1);
        DataTemplate dataTemplate3 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ShelfCounting.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_26 xamlCdataTemplate26 = new ShelfCounting.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_26();
        object[] objArray1 = new object[0 + 5];
        objArray1[0] = (object) dataTemplate1;
        objArray1[1] = (object) listView1;
        objArray1[2] = (object) stackLayout1;
        objArray1[3] = (object) stackLayout11;
        objArray1[4] = (object) shelfCounting;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate26.parentValues = objArray1;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate26.root = shelfCounting;
        // ISSUE: reference to a compiler-generated method
        Func<object> func1 = new Func<object>(xamlCdataTemplate26.LoadDataTemplate);
        ((IDataTemplate) dataTemplate3).LoadTemplate = func1;
        ((BindableObject) listView1).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) listView1);
        VisualDiagnostics.RegisterSourceInfo((object) listView1, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout11).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 14);
        ((BindableObject) stackLayout10).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout10).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        bindingExtension2.Path = ".";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker1).SetBinding(Picker.ItemsSourceProperty, bindingBase2);
        bindingExtension3.Path = "WarehouseCode";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        picker1.ItemDisplayBinding = bindingBase3;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase3, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 29, 29);
        ((BindableObject) picker1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) picker1);
        VisualDiagnostics.RegisterSourceInfo((object) picker1, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout10).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 27, 18);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) datePicker).SetValue(DatePicker.FormatProperty, (object) "dd.MM.yyyy");
        ((BindableObject) datePicker).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) datePicker);
        VisualDiagnostics.RegisterSourceInfo((object) datePicker, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 33, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout10).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 32, 18);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf No Okutunuz");
        softkeyboardDisabledEntry1.Completed += new EventHandler(shelfCounting.txtShelf_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 36, 22);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "x");
        referenceExtension1.Name = "shelfCounting";
        ReferenceExtension referenceExtension9 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 6];
        objArray2[0] = (object) bindingExtension4;
        objArray2[1] = (object) button1;
        objArray2[2] = (object) stackLayout4;
        objArray2[3] = (object) stackLayout10;
        objArray2[4] = (object) stackLayout11;
        objArray2[5] = (object) shelfCounting;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray2, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (ShelfCounting).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(38, 25)));
        object obj2 = ((IMarkupExtension) referenceExtension9).ProvideValue((IServiceProvider) xamlServiceProvider1);
        bindingExtension4.Source = obj2;
        VisualDiagnostics.RegisterSourceInfo(obj2, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 38, 25);
        bindingExtension4.Path = "ButtonColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase4);
        referenceExtension2.Name = "shelfCounting";
        ReferenceExtension referenceExtension10 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 6];
        objArray3[0] = (object) bindingExtension5;
        objArray3[1] = (object) button1;
        objArray3[2] = (object) stackLayout4;
        objArray3[3] = (object) stackLayout10;
        objArray3[4] = (object) stackLayout11;
        objArray3[5] = (object) shelfCounting;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray3, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver2.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (ShelfCounting).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(39, 25)));
        object obj4 = ((IMarkupExtension) referenceExtension10).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension5.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 39, 25);
        bindingExtension5.Path = "TextColor";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(Button.TextColorProperty, bindingBase5);
        button1.Clicked += new EventHandler(shelfCounting.btnClear_Clicked);
        ((BindableObject) button1).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 37, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout10).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 35, 18);
        ((BindableObject) stackLayout5).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout5).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod No Okutunuz");
        softkeyboardDisabledEntry2.Completed += new EventHandler(shelfCounting.txtBarcode_Completed);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 43, 22);
        ((BindableObject) entry).SetValue(Xamarin.Forms.Entry.TextProperty, (object) "1");
        ((BindableObject) entry).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Miktar");
        ((BindableObject) entry).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((BindableObject) entry).SetValue(Xamarin.Forms.Entry.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) entry);
        VisualDiagnostics.RegisterSourceInfo((object) entry, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 44, 22);
        ((BindableObject) picker2).SetValue(Picker.TitleProperty, (object) "Tip");
        bindingExtension6.Path = ".";
        BindingBase bindingBase6 = ((IMarkupExtension<BindingBase>) bindingExtension6).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker2).SetBinding(Picker.ItemsSourceProperty, bindingBase6);
        bindingExtension7.Path = "Caption";
        BindingBase bindingBase7 = ((IMarkupExtension<BindingBase>) bindingExtension7).ProvideValue((IServiceProvider) null);
        picker2.ItemDisplayBinding = bindingBase7;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase7, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 46, 33);
        ((BindableObject) picker2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) picker2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) picker2);
        VisualDiagnostics.RegisterSourceInfo((object) picker2, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 45, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout10).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 42, 18);
        ((BindableObject) stackLayout6).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout6).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) button2).SetValue(Button.TextProperty, (object) "Tamamla");
        referenceExtension3.Name = "shelfCounting";
        ReferenceExtension referenceExtension11 = referenceExtension3;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 6];
        objArray4[0] = (object) bindingExtension8;
        objArray4[1] = (object) button2;
        objArray4[2] = (object) stackLayout6;
        objArray4[3] = (object) stackLayout10;
        objArray4[4] = (object) stackLayout11;
        objArray4[5] = (object) shelfCounting;
        SimpleValueTargetProvider valueTargetProvider3;
        object obj5 = (object) (valueTargetProvider3 = new SimpleValueTargetProvider(objArray4, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider3.Add(type5, (object) valueTargetProvider3);
        xamlServiceProvider3.Add(typeof (IReferenceProvider), obj5);
        Type type6 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver3 = new XmlNamespaceResolver();
        namespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver3.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (ShelfCounting).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(50, 64)));
        object obj6 = ((IMarkupExtension) referenceExtension11).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension8.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 50, 64);
        bindingExtension8.Path = "ButtonColor";
        BindingBase bindingBase8 = ((IMarkupExtension<BindingBase>) bindingExtension8).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase8);
        referenceExtension4.Name = "shelfCounting";
        ReferenceExtension referenceExtension12 = referenceExtension4;
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray5 = new object[0 + 6];
        objArray5[0] = (object) bindingExtension9;
        objArray5[1] = (object) button2;
        objArray5[2] = (object) stackLayout6;
        objArray5[3] = (object) stackLayout10;
        objArray5[4] = (object) stackLayout11;
        objArray5[5] = (object) shelfCounting;
        SimpleValueTargetProvider valueTargetProvider4;
        object obj7 = (object) (valueTargetProvider4 = new SimpleValueTargetProvider(objArray5, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider4.Add(type7, (object) valueTargetProvider4);
        xamlServiceProvider4.Add(typeof (IReferenceProvider), obj7);
        Type type8 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver4 = new XmlNamespaceResolver();
        namespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver4.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (ShelfCounting).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(50, 140)));
        object obj8 = ((IMarkupExtension) referenceExtension12).ProvideValue((IServiceProvider) xamlServiceProvider4);
        bindingExtension9.Source = obj8;
        VisualDiagnostics.RegisterSourceInfo(obj8, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 50, 140);
        bindingExtension9.Path = "TextColor";
        BindingBase bindingBase9 = ((IMarkupExtension<BindingBase>) bindingExtension9).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(Button.TextColorProperty, bindingBase9);
        ((BindableObject) button2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        button2.Clicked += new EventHandler(shelfCounting.btnSuccess_Clicked);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) button2);
        VisualDiagnostics.RegisterSourceInfo((object) button2, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 50, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout10).Children).Add((View) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 49, 18);
        ((BindableObject) stackLayout7).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout7).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) button3).SetValue(Button.TextProperty, (object) "Rafı Sıfırla");
        referenceExtension5.Name = "shelfCounting";
        ReferenceExtension referenceExtension13 = referenceExtension5;
        XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
        Type type9 = typeof (IProvideValueTarget);
        object[] objArray6 = new object[0 + 6];
        objArray6[0] = (object) bindingExtension10;
        objArray6[1] = (object) button3;
        objArray6[2] = (object) stackLayout7;
        objArray6[3] = (object) stackLayout10;
        objArray6[4] = (object) stackLayout11;
        objArray6[5] = (object) shelfCounting;
        SimpleValueTargetProvider valueTargetProvider5;
        object obj9 = (object) (valueTargetProvider5 = new SimpleValueTargetProvider(objArray6, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider5.Add(type9, (object) valueTargetProvider5);
        xamlServiceProvider5.Add(typeof (IReferenceProvider), obj9);
        Type type10 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver5 = new XmlNamespaceResolver();
        namespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver5.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver5 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver5, typeof (ShelfCounting).GetTypeInfo().Assembly);
        xamlServiceProvider5.Add(type10, (object) xamlTypeResolver5);
        xamlServiceProvider5.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(55, 72)));
        object obj10 = ((IMarkupExtension) referenceExtension13).ProvideValue((IServiceProvider) xamlServiceProvider5);
        bindingExtension10.Source = obj10;
        VisualDiagnostics.RegisterSourceInfo(obj10, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 55, 72);
        bindingExtension10.Path = "ButtonColor";
        BindingBase bindingBase10 = ((IMarkupExtension<BindingBase>) bindingExtension10).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(VisualElement.BackgroundColorProperty, bindingBase10);
        referenceExtension6.Name = "shelfCounting";
        ReferenceExtension referenceExtension14 = referenceExtension6;
        XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
        Type type11 = typeof (IProvideValueTarget);
        object[] objArray7 = new object[0 + 6];
        objArray7[0] = (object) bindingExtension11;
        objArray7[1] = (object) button3;
        objArray7[2] = (object) stackLayout7;
        objArray7[3] = (object) stackLayout10;
        objArray7[4] = (object) stackLayout11;
        objArray7[5] = (object) shelfCounting;
        SimpleValueTargetProvider valueTargetProvider6;
        object obj11 = (object) (valueTargetProvider6 = new SimpleValueTargetProvider(objArray7, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider6.Add(type11, (object) valueTargetProvider6);
        xamlServiceProvider6.Add(typeof (IReferenceProvider), obj11);
        Type type12 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver6 = new XmlNamespaceResolver();
        namespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver6.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver6 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver6, typeof (ShelfCounting).GetTypeInfo().Assembly);
        xamlServiceProvider6.Add(type12, (object) xamlTypeResolver6);
        xamlServiceProvider6.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(55, 148)));
        object obj12 = ((IMarkupExtension) referenceExtension14).ProvideValue((IServiceProvider) xamlServiceProvider6);
        bindingExtension11.Source = obj12;
        VisualDiagnostics.RegisterSourceInfo(obj12, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 55, 148);
        bindingExtension11.Path = "TextColor";
        BindingBase bindingBase11 = ((IMarkupExtension<BindingBase>) bindingExtension11).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(Button.TextColorProperty, bindingBase11);
        ((BindableObject) button3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        button3.Clicked += new EventHandler(shelfCounting.btnShelfClear_Clicked);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) button3);
        VisualDiagnostics.RegisterSourceInfo((object) button3, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 55, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout10).Children).Add((View) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 54, 18);
        ((BindableObject) stackLayout9).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        bindingExtension12.Path = ".";
        BindingBase bindingBase12 = ((IMarkupExtension<BindingBase>) bindingExtension12).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView2).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase12);
        ((BindableObject) listView2).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        bindingExtension13.Path = ".";
        BindingBase bindingBase13 = ((IMarkupExtension<BindingBase>) bindingExtension13).ProvideValue((IServiceProvider) null);
        ((BindableObject) label).SetBinding(Label.TextProperty, bindingBase13);
        ((BindableObject) label).SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        referenceExtension7.Name = "shelfCounting";
        ReferenceExtension referenceExtension15 = referenceExtension7;
        XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
        Type type13 = typeof (IProvideValueTarget);
        object[] objArray8 = new object[0 + 8];
        objArray8[0] = (object) bindingExtension14;
        objArray8[1] = (object) label;
        objArray8[2] = (object) stackLayout8;
        objArray8[3] = (object) listView2;
        objArray8[4] = (object) stackLayout9;
        objArray8[5] = (object) stackLayout10;
        objArray8[6] = (object) stackLayout11;
        objArray8[7] = (object) shelfCounting;
        SimpleValueTargetProvider valueTargetProvider7;
        object obj13 = (object) (valueTargetProvider7 = new SimpleValueTargetProvider(objArray8, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider7.Add(type13, (object) valueTargetProvider7);
        xamlServiceProvider7.Add(typeof (IReferenceProvider), obj13);
        Type type14 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver7 = new XmlNamespaceResolver();
        namespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver7.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver7 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver7, typeof (ShelfCounting).GetTypeInfo().Assembly);
        xamlServiceProvider7.Add(type14, (object) xamlTypeResolver7);
        xamlServiceProvider7.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(65, 36)));
        object obj14 = ((IMarkupExtension) referenceExtension15).ProvideValue((IServiceProvider) xamlServiceProvider7);
        bindingExtension14.Source = obj14;
        VisualDiagnostics.RegisterSourceInfo(obj14, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 65, 36);
        bindingExtension14.Path = "ButtonColor";
        BindingBase bindingBase14 = ((IMarkupExtension<BindingBase>) bindingExtension14).ProvideValue((IServiceProvider) null);
        ((BindableObject) label).SetBinding(Label.TextColorProperty, bindingBase14);
        referenceExtension8.Name = "shelfCounting";
        ReferenceExtension referenceExtension16 = referenceExtension8;
        XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
        Type type15 = typeof (IProvideValueTarget);
        object[] objArray9 = new object[0 + 8];
        objArray9[0] = (object) bindingExtension15;
        objArray9[1] = (object) label;
        objArray9[2] = (object) stackLayout8;
        objArray9[3] = (object) listView2;
        objArray9[4] = (object) stackLayout9;
        objArray9[5] = (object) stackLayout10;
        objArray9[6] = (object) stackLayout11;
        objArray9[7] = (object) shelfCounting;
        SimpleValueTargetProvider valueTargetProvider8;
        object obj15 = (object) (valueTargetProvider8 = new SimpleValueTargetProvider(objArray9, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider8.Add(type15, (object) valueTargetProvider8);
        xamlServiceProvider8.Add(typeof (IReferenceProvider), obj15);
        Type type16 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver8 = new XmlNamespaceResolver();
        namespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver8.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver8 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver8, typeof (ShelfCounting).GetTypeInfo().Assembly);
        xamlServiceProvider8.Add(type16, (object) xamlTypeResolver8);
        xamlServiceProvider8.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(66, 36)));
        object obj16 = ((IMarkupExtension) referenceExtension16).ProvideValue((IServiceProvider) xamlServiceProvider8);
        bindingExtension15.Source = obj16;
        VisualDiagnostics.RegisterSourceInfo(obj16, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 66, 36);
        bindingExtension15.Path = "TextColor";
        BindingBase bindingBase15 = ((IMarkupExtension<BindingBase>) bindingExtension15).ProvideValue((IServiceProvider) null);
        ((BindableObject) label).SetBinding(VisualElement.BackgroundColorProperty, bindingBase15);
        ((BindableObject) label).SetValue(Label.FontProperty, new FontTypeConverter().ConvertFromInvariantString("Bold,20"));
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) label);
        VisualDiagnostics.RegisterSourceInfo((object) label, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 63, 34);
        ((BindableObject) listView2).SetValue(ListView.HeaderProperty, (object) stackLayout8);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout8, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 62, 30);
        DataTemplate dataTemplate4 = dataTemplate2;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ShelfCounting.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_27 xamlCdataTemplate27 = new ShelfCounting.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_27();
        object[] objArray10 = new object[0 + 6];
        objArray10[0] = (object) dataTemplate2;
        objArray10[1] = (object) listView2;
        objArray10[2] = (object) stackLayout9;
        objArray10[3] = (object) stackLayout10;
        objArray10[4] = (object) stackLayout11;
        objArray10[5] = (object) shelfCounting;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate27.parentValues = objArray10;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate27.root = shelfCounting;
        // ISSUE: reference to a compiler-generated method
        Func<object> func2 = new Func<object>(xamlCdataTemplate27.LoadDataTemplate);
        ((IDataTemplate) dataTemplate4).LoadTemplate = func2;
        ((BindableObject) listView2).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate2);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate2, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 71, 30);
        ((ICollection<View>) ((Layout<View>) stackLayout9).Children).Add((View) listView2);
        VisualDiagnostics.RegisterSourceInfo((object) listView2, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 60, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout10).Children).Add((View) stackLayout9);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout9, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 59, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout11).Children).Add((View) stackLayout10);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout10, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 26, 14);
        ((BindableObject) shelfCounting).SetValue(ContentPage.ContentProperty, (object) stackLayout11);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout11, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 8, 10);
        VisualDiagnostics.RegisterSourceInfo((object) shelfCounting, new Uri("Views\\ShelfCounting.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<ShelfCounting>(this, typeof (ShelfCounting));
      this.shelfCounting = NameScopeExtensions.FindByName<ContentPage>((Element) this, "shelfCounting");
      this.stckShelfCounting = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfCounting");
      this.lstShelfCounting = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfCounting");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.pckWarehouse = NameScopeExtensions.FindByName<Picker>((Element) this, "pckWarehouse");
      this.dtShelfCounting = NameScopeExtensions.FindByName<DatePicker>((Element) this, "dtShelfCounting");
      this.txtShelf = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtShelf");
      this.btnClear = NameScopeExtensions.FindByName<Button>((Element) this, "btnClear");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.txtQty = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtQty");
      this.pckBarcodeType = NameScopeExtensions.FindByName<Picker>((Element) this, "pckBarcodeType");
      this.stckSuccess = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckSuccess");
      this.btnSuccess = NameScopeExtensions.FindByName<Button>((Element) this, "btnSuccess");
      this.stckShelfClear = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfClear");
      this.btnShelfClear = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelfClear");
      this.stckShelfCountingDetailList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfCountingDetailList");
      this.lstShelfCountingDetail = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfCountingDetail");
      this.lblShelfCountingDetailHeader = NameScopeExtensions.FindByName<Label>((Element) this, "lblShelfCountingDetailHeader");
    }
  }
}
