// Decompiled with JetBrains decompiler
// Type: Shelf.Views.ShelfEntry2
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
  [XamlFilePath("Views\\ShelfEntry2.xaml")]
  public class ShelfEntry2 : ContentPage
  {
    private List<pIOShelfPurchaseOrderDetailReturnModel> shelfOrderDetail;
    private string selectedShelfCode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage shelfentry;
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
    private StackLayout stckBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtQty;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnPickOrder;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnShelfOrderSuccess;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfDetail;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Label lblListHeader;

    public Color ButtonColor => Color.FromRgb(52, 203, 201);

    public Color TextColor => Color.White;

    public ShelfEntry2()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Raf Girişi";
      GlobalMob.AddBarcodeLongPress(this.txtBarcode);
      ((VisualElement) this.txtQty).IsVisible = !GlobalMob.User.HideQty;
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetUserShelfPurchaseOrders?userID={0}&shelfPurchaseOrderType={1}", (object) GlobalMob.User.UserID, (object) -1), (ContentPage) this);
      if (returnModel.Success)
      {
        List<pIOUserShelfPurchaseOrdersReturnModel> ordersReturnModelList = JsonConvert.DeserializeObject<List<pIOUserShelfPurchaseOrdersReturnModel>>(returnModel.Result);
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
        ((Page) this).Title = "Raf Giriş(İrsaliyeli)";
        pIOUserShelfPurchaseOrdersReturnModel ordersReturnModel = (pIOUserShelfPurchaseOrdersReturnModel) selectedItem;
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

    private void GetShelfDetail()
    {
      this.shelfOrderDetail = new List<pIOShelfPurchaseOrderDetailReturnModel>();
      Device.BeginInvokeOnMainThread((Action) (() =>
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfPurchaseOrderDetail?purchaseOrderNumber=S{0}", (object) ((InputView) this.txtShelfOrderNumber).Text), (ContentPage) this);
        if (!returnModel.Success)
          return;
        this.shelfOrderDetail = JsonConvert.DeserializeObject<List<pIOShelfPurchaseOrderDetailReturnModel>>(returnModel.Result);
        ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
        if (this.shelfOrderDetail.Count > 0)
        {
          this.lblListHeader.Text = "Raf Adı : " + this.shelfOrderDetail[0].ShelfCode;
          ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) this.shelfOrderDetail;
          ((InputView) this.txtShelf).Text = this.shelfOrderDetail[0].ShelfCode;
          this.selectedShelfCode = ((InputView) this.txtShelf).Text;
          ((InputView) this.txtShelfBarcode).Text = "";
          ((VisualElement) this.stckBarcode).IsVisible = true;
          ((VisualElement) this.stckShelf).IsVisible = true;
          ((VisualElement) this.btnShelfOrderSuccess).IsVisible = true;
          this.NextBackButtonEnabled();
          ((VisualElement) this.txtShelfBarcode).Focus();
        }
        else
          ((VisualElement) this.btnShelfOrderSuccess).IsVisible = true;
      }));
    }

    private void FillListView()
    {
      List<pIOShelfPurchaseOrderDetailReturnModel> list = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.ShelfCode == ((InputView) this.txtShelfBarcode).Text || x.ShelfName == ((InputView) this.txtShelfBarcode).Text)).ToList<pIOShelfPurchaseOrderDetailReturnModel>();
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
      this.lblListHeader.Text = "";
      this.selectedShelfCode = ((InputView) this.txtShelfBarcode).Text;
      ((InputView) this.txtShelf).Text = this.selectedShelfCode;
      if (list.Count > 0)
      {
        ((VisualElement) this.lstShelfDetail).IsVisible = true;
        ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) list;
        this.lblListHeader.Text = "Raf Adı : " + ((InputView) this.txtShelf).Text;
      }
      else
        ((InputView) this.txtShelfBarcode).Text = "";
    }

    private async void GetBarcode()
    {
      ShelfEntry2 page = this;
      string barcode = ((InputView) page.txtBarcode).Text;
      if (string.IsNullOrEmpty(barcode))
        return;
      if (barcode.Length < GlobalMob.User.MinimumBarcodeLength)
      {
        GlobalMob.PlayError();
        ((InputView) page.txtBarcode).Text = "";
        ((VisualElement) page.txtBarcode).Focus();
      }
      else
      {
        pIOShelfPurchaseOrderDetailReturnModel detailReturnModel = page.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.Barcode.Contains(barcode) && x.ShelfCode == this.selectedShelfCode && x.OrderQty != x.AllocatingQty)).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>();
        if (detailReturnModel != null)
        {
          int int32_1 = Convert.ToInt32(((InputView) page.txtQty).Text);
          int purchaseOrderDetailId = detailReturnModel.PurchaseOrderDetailID;
          if (int32_1 <= 0)
            return;
          ReturnModel returnModel = GlobalMob.PostJson(string.Format("UpdateAlcShelfPurchaseOrderDetail?shelfPurchaseOrderDetailID={0}&qty={1}&barcode={2}&userName={3}", (object) purchaseOrderDetailId, (object) int32_1, (object) barcode, (object) GlobalMob.User.UserName), (ContentPage) page);
          if (!returnModel.Success)
            return;
          int int32_2 = Convert.ToInt32(returnModel.Result);
          if (int32_2 > 0)
          {
            detailReturnModel.AllocatingQty += (double) int32_2;
            page.shelfOrderDetail.Select<pIOShelfPurchaseOrderDetailReturnModel, pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, pIOShelfPurchaseOrderDetailReturnModel>) (c =>
            {
              c.LastReadBarcode = false;
              return c;
            })).ToList<pIOShelfPurchaseOrderDetailReturnModel>();
            detailReturnModel.LastReadBarcode = true;
            page.shelfOrderDetail = page.shelfOrderDetail.OrderByDescending<pIOShelfPurchaseOrderDetailReturnModel, bool>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.LastReadBarcode)).ToList<pIOShelfPurchaseOrderDetailReturnModel>();
            page.FillListView();
            ((InputView) page.txtQty).Text = "1";
            ((InputView) page.txtBarcode).Text = "";
            Device.BeginInvokeOnMainThread((Action) (async () =>
            {
              GlobalMob.PlaySave();
              await Task.Delay(250);
              ((VisualElement) this.txtBarcode)?.Focus();
            }));
          }
          else if (int32_2 == -1)
          {
            GlobalMob.PlayError();
            int num = await ((Page) page).DisplayAlert("Bilgi", "Hata Oluştu", "", "Tamam") ? 1 : 0;
            ((InputView) page.txtBarcode).Text = "";
            ((VisualElement) page.txtBarcode).Focus();
          }
          else
          {
            GlobalMob.PlayError();
            int num = await ((Page) page).DisplayAlert("Bilgi", "Miktar Yetersiz", "", "Tamam") ? 1 : 0;
            ((InputView) page.txtBarcode).Text = "";
            ((VisualElement) page.txtBarcode).Focus();
          }
        }
        else
        {
          int num1 = page.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.Barcode.Contains(barcode) && x.ShelfCode == this.selectedShelfCode && x.AllocatingQty == x.OrderQty)).Any<pIOShelfPurchaseOrderDetailReturnModel>() ? 1 : 0;
          GlobalMob.PlayError();
          string str = num1 != 0 ? "Sipariş miktarı tamamlandı" : "Ürün bulunamadı";
          int num2 = await ((Page) page).DisplayAlert("Bilgi", str, "", "Tamam") ? 1 : 0;
          ((InputView) page.txtBarcode).Text = "";
          ((VisualElement) page.txtBarcode).Focus();
        }
      }
    }

    private void btnPickOrder_Clicked(object sender, EventArgs e) => this.GetBarcode();

    private async void btnShelf_Clicked(object sender, EventArgs e)
    {
      ShelfEntry2 shelfEntry2 = this;
      // ISSUE: reference to a compiler-generated method
      if (shelfEntry2.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>(new Func<pIOShelfPurchaseOrderDetailReturnModel, bool>(shelfEntry2.\u003CbtnShelf_Clicked\u003Eb__12_0)).Count<pIOShelfPurchaseOrderDetailReturnModel>() > 0)
      {
        // ISSUE: reference to a compiler-generated method
        if (shelfEntry2.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>(new Func<pIOShelfPurchaseOrderDetailReturnModel, bool>(shelfEntry2.\u003CbtnShelf_Clicked\u003Eb__12_1)).ToList<pIOShelfPurchaseOrderDetailReturnModel>().Count > 0 && shelfEntry2.selectedShelfCode != ((InputView) shelfEntry2.txtShelfBarcode).Text)
        {
          if (!await ((Page) shelfEntry2).DisplayAlert("Devam?", "Rafdaki ürünler tamamlanmadı.Devam etmek istiyor musunuz?", "Evet", "Hayır"))
            return;
          shelfEntry2.FillListView();
        }
        else
          shelfEntry2.FillListView();
      }
      else
      {
        int num = await ((Page) shelfEntry2).DisplayAlert("Bilgi", "Raf bulunamadı", "", "Tamam") ? 1 : 0;
        ((InputView) shelfEntry2.txtShelfBarcode).Text = "";
        ((VisualElement) shelfEntry2.txtShelfBarcode).Focus();
      }
    }

    private void NextBackButtonEnabled()
    {
      pIOShelfPurchaseOrderDetailReturnModel item = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode)).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>();
      ((VisualElement) this.btnShelfBack).IsEnabled = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x =>
      {
        int? sortOrder1 = x.SortOrder;
        int? sortOrder2 = item.SortOrder;
        return sortOrder1.GetValueOrDefault() < sortOrder2.GetValueOrDefault() & sortOrder1.HasValue & sortOrder2.HasValue;
      })).Any<pIOShelfPurchaseOrderDetailReturnModel>();
      ((VisualElement) this.btnShelfNext).IsEnabled = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x =>
      {
        int? sortOrder3 = x.SortOrder;
        int? sortOrder4 = item.SortOrder;
        return sortOrder3.GetValueOrDefault() > sortOrder4.GetValueOrDefault() & sortOrder3.HasValue & sortOrder4.HasValue;
      })).Any<pIOShelfPurchaseOrderDetailReturnModel>();
    }

    private async void btnShelfNext_Clicked(object sender, EventArgs e)
    {
      ShelfEntry2 shelfEntry2 = this;
      if (shelfEntry2.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode && x.OrderQty - x.AllocatingQty > 0.0)).ToList<pIOShelfPurchaseOrderDetailReturnModel>().Count > 0 && shelfEntry2.selectedShelfCode != ((InputView) shelfEntry2.txtShelfBarcode).Text)
      {
        if (!await ((Page) shelfEntry2).DisplayAlert("Devam?", "Rafdaki ürünler tamamlanmadı.Devam etmek istiyor musunuz?", "Evet", "Hayır"))
          return;
      }
      pIOShelfPurchaseOrderDetailReturnModel item = shelfEntry2.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode)).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>();
      if (item != null)
      {
        pIOShelfPurchaseOrderDetailReturnModel detailReturnModel = shelfEntry2.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x =>
        {
          int? sortOrder1 = x.SortOrder;
          int? sortOrder2 = item.SortOrder;
          return sortOrder1.GetValueOrDefault() > sortOrder2.GetValueOrDefault() & sortOrder1.HasValue & sortOrder2.HasValue;
        })).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>();
        ((InputView) shelfEntry2.txtShelf).Text = detailReturnModel.ShelfCode;
        shelfEntry2.selectedShelfCode = ((InputView) shelfEntry2.txtShelf).Text;
        ((InputView) shelfEntry2.txtShelfBarcode).Text = "";
        ((VisualElement) shelfEntry2.txtShelfBarcode).Focus();
      }
      shelfEntry2.NextBackButtonEnabled();
    }

    private void btnShelfBack_Clicked(object sender, EventArgs e)
    {
      pIOShelfPurchaseOrderDetailReturnModel item = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x => x.ShelfCode == this.selectedShelfCode)).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>();
      if (item != null)
      {
        ((InputView) this.txtShelf).Text = this.shelfOrderDetail.Where<pIOShelfPurchaseOrderDetailReturnModel>((Func<pIOShelfPurchaseOrderDetailReturnModel, bool>) (x =>
        {
          int? sortOrder1 = x.SortOrder;
          int? sortOrder2 = item.SortOrder;
          return sortOrder1.GetValueOrDefault() < sortOrder2.GetValueOrDefault() & sortOrder1.HasValue & sortOrder2.HasValue;
        })).OrderByDescending<pIOShelfPurchaseOrderDetailReturnModel, int?>((Func<pIOShelfPurchaseOrderDetailReturnModel, int?>) (x => x.SortOrder)).FirstOrDefault<pIOShelfPurchaseOrderDetailReturnModel>().ShelfCode;
        this.selectedShelfCode = ((InputView) this.txtShelf).Text;
      }
      this.NextBackButtonEnabled();
    }

    private async void txtShelfBarcode_Completed(object sender, EventArgs e)
    {
      ShelfEntry2 page = this;
      ztIOShelf ztIoShelf = new ztIOShelf();
      ReturnModel shelf = GlobalMob.GetShelf(((InputView) page.txtShelfBarcode).Text, (ContentPage) page);
      if (shelf.Success)
      {
        ztIoShelf = JsonConvert.DeserializeObject<ztIOShelf>(shelf.Result);
        ((InputView) page.txtShelfBarcode).Text = ztIoShelf.Code;
      }
      if (page.selectedShelfCode != ztIoShelf.Code)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Hatalı Raf Kodu", "", "Tamam") ? 1 : 0;
        ((InputView) page.txtShelfBarcode).Text = "";
        ((VisualElement) page.txtShelfBarcode).Focus();
      }
      else
        page.FillListView();
      // ISSUE: reference to a compiler-generated method
      Device.BeginInvokeOnMainThread(new Action(page.\u003CtxtShelfBarcode_Completed\u003Eb__16_0));
    }

    private void txtBarcode_Completed(object sender, EventArgs e) => this.btnPickOrder_Clicked((object) null, (EventArgs) null);

    private async void btnShelfOrderSuccess_Clicked(object sender, EventArgs e)
    {
      ShelfEntry2 page1 = this;
      if (!string.IsNullOrEmpty(((InputView) page1.txtShelfOrderNumber).Text))
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("ShelfOrderPurchaseCompleted?shelfPurchaseOrderNumber={0}&isCompleted=false", (object) ((InputView) page1.txtShelfOrderNumber).Text), (ContentPage) page1);
        if (!returnModel.Success)
          return;
        if (!string.IsNullOrEmpty(JsonConvert.DeserializeObject<string>(returnModel.Result)))
        {
          if (await ((Page) page1).DisplayAlert("Devam?", "Ürünler tamamlanmadı.Devam etmek istiyor musunuz?", "Evet", "Hayır"))
          {
            if (!GlobalMob.PostJson(string.Format("ShelfOrderPurchaseCompleted?shelfOrderNumber={0}&isCompleted=true", (object) ((InputView) page1.txtShelfOrderNumber).Text), (ContentPage) page1).Success)
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

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (ShelfEntry2).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/ShelfEntry2.xaml",
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
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry1 = new SoftkeyboardDisabledEntry();
        ReferenceExtension referenceExtension5 = new ReferenceExtension();
        BindingExtension bindingExtension6 = new BindingExtension();
        ReferenceExtension referenceExtension6 = new ReferenceExtension();
        BindingExtension bindingExtension7 = new BindingExtension();
        Button button3 = new Button();
        StackLayout stackLayout4 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry2 = new SoftkeyboardDisabledEntry();
        Xamarin.Forms.Entry entry3 = new Xamarin.Forms.Entry();
        StackLayout stackLayout5 = new StackLayout();
        ReferenceExtension referenceExtension7 = new ReferenceExtension();
        BindingExtension bindingExtension8 = new BindingExtension();
        ReferenceExtension referenceExtension8 = new ReferenceExtension();
        BindingExtension bindingExtension9 = new BindingExtension();
        Button button4 = new Button();
        ReferenceExtension referenceExtension9 = new ReferenceExtension();
        BindingExtension bindingExtension10 = new BindingExtension();
        ReferenceExtension referenceExtension10 = new ReferenceExtension();
        BindingExtension bindingExtension11 = new BindingExtension();
        Button button5 = new Button();
        BindingExtension bindingExtension12 = new BindingExtension();
        ReferenceExtension referenceExtension11 = new ReferenceExtension();
        BindingExtension bindingExtension13 = new BindingExtension();
        ReferenceExtension referenceExtension12 = new ReferenceExtension();
        BindingExtension bindingExtension14 = new BindingExtension();
        Label label2 = new Label();
        StackLayout stackLayout6 = new StackLayout();
        DataTemplate dataTemplate2 = new DataTemplate();
        ListView listView2 = new ListView();
        StackLayout stackLayout7 = new StackLayout();
        StackLayout stackLayout8 = new StackLayout();
        ShelfEntry2 shelfEntry2;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (shelfEntry2 = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) shelfEntry2, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("shelfentry", (object) shelfEntry2);
        if (((Element) shelfEntry2).StyleId == null)
          ((Element) shelfEntry2).StyleId = "shelfentry";
        ((INameScope) nameScope).RegisterName("stckContent", (object) stackLayout8);
        if (((Element) stackLayout8).StyleId == null)
          ((Element) stackLayout8).StyleId = "stckContent";
        ((INameScope) nameScope).RegisterName("stckShelfOrderList", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckShelfOrderList";
        ((INameScope) nameScope).RegisterName("stckEmptyMessage", (object) stackLayout1);
        if (((Element) stackLayout1).StyleId == null)
          ((Element) stackLayout1).StyleId = "stckEmptyMessage";
        ((INameScope) nameScope).RegisterName("lstShelfOrder", (object) listView1);
        if (((Element) listView1).StyleId == null)
          ((Element) listView1).StyleId = "lstShelfOrder";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout7);
        if (((Element) stackLayout7).StyleId == null)
          ((Element) stackLayout7).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtShelfOrderNumber", (object) entry1);
        if (((Element) entry1).StyleId == null)
          ((Element) entry1).StyleId = "txtShelfOrderNumber";
        ((INameScope) nameScope).RegisterName("stckShelf", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckShelf";
        ((INameScope) nameScope).RegisterName("txtShelf", (object) entry2);
        if (((Element) entry2).StyleId == null)
          ((Element) entry2).StyleId = "txtShelf";
        ((INameScope) nameScope).RegisterName("btnShelfBack", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnShelfBack";
        ((INameScope) nameScope).RegisterName("btnShelfNext", (object) button2);
        if (((Element) button2).StyleId == null)
          ((Element) button2).StyleId = "btnShelfNext";
        ((INameScope) nameScope).RegisterName("txtShelfBarcode", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtShelfBarcode";
        ((INameScope) nameScope).RegisterName("btnShelf", (object) button3);
        if (((Element) button3).StyleId == null)
          ((Element) button3).StyleId = "btnShelf";
        ((INameScope) nameScope).RegisterName("stckBarcode", (object) stackLayout5);
        if (((Element) stackLayout5).StyleId == null)
          ((Element) stackLayout5).StyleId = "stckBarcode";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("txtQty", (object) entry3);
        if (((Element) entry3).StyleId == null)
          ((Element) entry3).StyleId = "txtQty";
        ((INameScope) nameScope).RegisterName("btnPickOrder", (object) button4);
        if (((Element) button4).StyleId == null)
          ((Element) button4).StyleId = "btnPickOrder";
        ((INameScope) nameScope).RegisterName("btnShelfOrderSuccess", (object) button5);
        if (((Element) button5).StyleId == null)
          ((Element) button5).StyleId = "btnShelfOrderSuccess";
        ((INameScope) nameScope).RegisterName("lstShelfDetail", (object) listView2);
        if (((Element) listView2).StyleId == null)
          ((Element) listView2).StyleId = "lstShelfDetail";
        ((INameScope) nameScope).RegisterName("lblListHeader", (object) label2);
        if (((Element) label2).StyleId == null)
          ((Element) label2).StyleId = "lblListHeader";
        this.shelfentry = (ContentPage) shelfEntry2;
        this.stckContent = stackLayout8;
        this.stckShelfOrderList = stackLayout2;
        this.stckEmptyMessage = stackLayout1;
        this.lstShelfOrder = listView1;
        this.stckForm = stackLayout7;
        this.txtShelfOrderNumber = entry1;
        this.stckShelf = stackLayout4;
        this.txtShelf = entry2;
        this.btnShelfBack = button1;
        this.btnShelfNext = button2;
        this.txtShelfBarcode = softkeyboardDisabledEntry1;
        this.btnShelf = button3;
        this.stckBarcode = stackLayout5;
        this.txtBarcode = softkeyboardDisabledEntry2;
        this.txtQty = entry3;
        this.btnPickOrder = button4;
        this.btnShelfOrderSuccess = button5;
        this.lstShelfDetail = listView2;
        this.lblListHeader = label2;
        ((BindableObject) stackLayout8).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout8).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout2).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout1).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) stackLayout1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) stackLayout1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Center);
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
        objArray1[1] = (object) stackLayout1;
        objArray1[2] = (object) stackLayout2;
        objArray1[3] = (object) stackLayout8;
        objArray1[4] = (object) shelfEntry2;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) Label.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (ShelfEntry2).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(12, 128)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter).ConvertFromInvariantString("Medium", (IServiceProvider) xamlServiceProvider1);
        ((BindableObject) label3).SetValue(fontSizeProperty, obj2);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) label1);
        VisualDiagnostics.RegisterSourceInfo((object) label1, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 18);
        bindingExtension1.Path = ".";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView1).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase1);
        DataTemplate dataTemplate3 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ShelfEntry2.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_29 xamlCdataTemplate29 = new ShelfEntry2.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_29();
        object[] objArray2 = new object[0 + 5];
        objArray2[0] = (object) dataTemplate1;
        objArray2[1] = (object) listView1;
        objArray2[2] = (object) stackLayout2;
        objArray2[3] = (object) stackLayout8;
        objArray2[4] = (object) shelfEntry2;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate29.parentValues = objArray2;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate29.root = shelfEntry2;
        // ISSUE: reference to a compiler-generated method
        Func<object> func1 = new Func<object>(xamlCdataTemplate29.LoadDataTemplate);
        ((IDataTemplate) dataTemplate3).LoadTemplate = func1;
        ((BindableObject) listView1).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) listView1);
        VisualDiagnostics.RegisterSourceInfo((object) listView1, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 14);
        ((BindableObject) stackLayout7).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout7).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout7).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) stackLayout7).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf Emri Numarası Giriniz");
        ((BindableObject) entry1).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry1).SetValue(VisualElement.InputTransparentProperty, (object) true);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 41, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 40, 18);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout4).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "");
        ((BindableObject) entry2).SetValue(VisualElement.InputTransparentProperty, (object) true);
        ((BindableObject) entry2).SetValue(VisualElement.WidthRequestProperty, (object) 100.0);
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Start);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 46, 22);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "<");
        referenceExtension1.Name = "shelfentry";
        ReferenceExtension referenceExtension13 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 6];
        objArray3[0] = (object) bindingExtension2;
        objArray3[1] = (object) button1;
        objArray3[2] = (object) stackLayout4;
        objArray3[3] = (object) stackLayout7;
        objArray3[4] = (object) stackLayout8;
        objArray3[5] = (object) shelfEntry2;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray3, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver2.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (ShelfEntry2).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(48, 25)));
        object obj4 = ((IMarkupExtension) referenceExtension13).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension2.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 48, 25);
        bindingExtension2.Path = "ButtonColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase2);
        referenceExtension2.Name = "shelfentry";
        ReferenceExtension referenceExtension14 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 6];
        objArray4[0] = (object) bindingExtension3;
        objArray4[1] = (object) button1;
        objArray4[2] = (object) stackLayout4;
        objArray4[3] = (object) stackLayout7;
        objArray4[4] = (object) stackLayout8;
        objArray4[5] = (object) shelfEntry2;
        SimpleValueTargetProvider valueTargetProvider3;
        object obj5 = (object) (valueTargetProvider3 = new SimpleValueTargetProvider(objArray4, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider3.Add(type5, (object) valueTargetProvider3);
        xamlServiceProvider3.Add(typeof (IReferenceProvider), obj5);
        Type type6 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver3 = new XmlNamespaceResolver();
        namespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver3.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (ShelfEntry2).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(48, 98)));
        object obj6 = ((IMarkupExtension) referenceExtension14).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension3.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 48, 98);
        bindingExtension3.Path = "TextColor";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(Button.TextColorProperty, bindingBase3);
        button1.Clicked += new EventHandler(shelfEntry2.btnShelfBack_Clicked);
        ((BindableObject) button1).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.StartAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 47, 22);
        ((BindableObject) button2).SetValue(Button.TextProperty, (object) ">");
        referenceExtension3.Name = "shelfentry";
        ReferenceExtension referenceExtension15 = referenceExtension3;
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray5 = new object[0 + 6];
        objArray5[0] = (object) bindingExtension4;
        objArray5[1] = (object) button2;
        objArray5[2] = (object) stackLayout4;
        objArray5[3] = (object) stackLayout7;
        objArray5[4] = (object) stackLayout8;
        objArray5[5] = (object) shelfEntry2;
        SimpleValueTargetProvider valueTargetProvider4;
        object obj7 = (object) (valueTargetProvider4 = new SimpleValueTargetProvider(objArray5, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider4.Add(type7, (object) valueTargetProvider4);
        xamlServiceProvider4.Add(typeof (IReferenceProvider), obj7);
        Type type8 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver4 = new XmlNamespaceResolver();
        namespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver4.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (ShelfEntry2).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(51, 25)));
        object obj8 = ((IMarkupExtension) referenceExtension15).ProvideValue((IServiceProvider) xamlServiceProvider4);
        bindingExtension4.Source = obj8;
        VisualDiagnostics.RegisterSourceInfo(obj8, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 51, 25);
        bindingExtension4.Path = "ButtonColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase4);
        referenceExtension4.Name = "shelfentry";
        ReferenceExtension referenceExtension16 = referenceExtension4;
        XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
        Type type9 = typeof (IProvideValueTarget);
        object[] objArray6 = new object[0 + 6];
        objArray6[0] = (object) bindingExtension5;
        objArray6[1] = (object) button2;
        objArray6[2] = (object) stackLayout4;
        objArray6[3] = (object) stackLayout7;
        objArray6[4] = (object) stackLayout8;
        objArray6[5] = (object) shelfEntry2;
        SimpleValueTargetProvider valueTargetProvider5;
        object obj9 = (object) (valueTargetProvider5 = new SimpleValueTargetProvider(objArray6, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider5.Add(type9, (object) valueTargetProvider5);
        xamlServiceProvider5.Add(typeof (IReferenceProvider), obj9);
        Type type10 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver5 = new XmlNamespaceResolver();
        namespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver5.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver5 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver5, typeof (ShelfEntry2).GetTypeInfo().Assembly);
        xamlServiceProvider5.Add(type10, (object) xamlTypeResolver5);
        xamlServiceProvider5.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(51, 98)));
        object obj10 = ((IMarkupExtension) referenceExtension16).ProvideValue((IServiceProvider) xamlServiceProvider5);
        bindingExtension5.Source = obj10;
        VisualDiagnostics.RegisterSourceInfo(obj10, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 51, 98);
        bindingExtension5.Path = "TextColor";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) button2).SetBinding(Button.TextColorProperty, bindingBase5);
        button2.Clicked += new EventHandler(shelfEntry2.btnShelfNext_Clicked);
        ((BindableObject) button2).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.StartAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) button2);
        VisualDiagnostics.RegisterSourceInfo((object) button2, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 50, 22);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf No Okutunuz");
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(VisualElement.WidthRequestProperty, (object) 200.0);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        softkeyboardDisabledEntry1.Completed += new EventHandler(shelfEntry2.txtShelfBarcode_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 53, 22);
        ((BindableObject) button3).SetValue(Button.TextProperty, (object) "...");
        button3.Clicked += new EventHandler(shelfEntry2.btnShelf_Clicked);
        referenceExtension5.Name = "shelfentry";
        ReferenceExtension referenceExtension17 = referenceExtension5;
        XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
        Type type11 = typeof (IProvideValueTarget);
        object[] objArray7 = new object[0 + 6];
        objArray7[0] = (object) bindingExtension6;
        objArray7[1] = (object) button3;
        objArray7[2] = (object) stackLayout4;
        objArray7[3] = (object) stackLayout7;
        objArray7[4] = (object) stackLayout8;
        objArray7[5] = (object) shelfEntry2;
        SimpleValueTargetProvider valueTargetProvider6;
        object obj11 = (object) (valueTargetProvider6 = new SimpleValueTargetProvider(objArray7, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider6.Add(type11, (object) valueTargetProvider6);
        xamlServiceProvider6.Add(typeof (IReferenceProvider), obj11);
        Type type12 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver6 = new XmlNamespaceResolver();
        namespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver6.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver6 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver6, typeof (ShelfEntry2).GetTypeInfo().Assembly);
        xamlServiceProvider6.Add(type12, (object) xamlTypeResolver6);
        xamlServiceProvider6.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(55, 25)));
        object obj12 = ((IMarkupExtension) referenceExtension17).ProvideValue((IServiceProvider) xamlServiceProvider6);
        bindingExtension6.Source = obj12;
        VisualDiagnostics.RegisterSourceInfo(obj12, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 55, 25);
        bindingExtension6.Path = "ButtonColor";
        BindingBase bindingBase6 = ((IMarkupExtension<BindingBase>) bindingExtension6).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(VisualElement.BackgroundColorProperty, bindingBase6);
        referenceExtension6.Name = "shelfentry";
        ReferenceExtension referenceExtension18 = referenceExtension6;
        XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
        Type type13 = typeof (IProvideValueTarget);
        object[] objArray8 = new object[0 + 6];
        objArray8[0] = (object) bindingExtension7;
        objArray8[1] = (object) button3;
        objArray8[2] = (object) stackLayout4;
        objArray8[3] = (object) stackLayout7;
        objArray8[4] = (object) stackLayout8;
        objArray8[5] = (object) shelfEntry2;
        SimpleValueTargetProvider valueTargetProvider7;
        object obj13 = (object) (valueTargetProvider7 = new SimpleValueTargetProvider(objArray8, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider7.Add(type13, (object) valueTargetProvider7);
        xamlServiceProvider7.Add(typeof (IReferenceProvider), obj13);
        Type type14 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver7 = new XmlNamespaceResolver();
        namespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver7.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver7 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver7, typeof (ShelfEntry2).GetTypeInfo().Assembly);
        xamlServiceProvider7.Add(type14, (object) xamlTypeResolver7);
        xamlServiceProvider7.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(55, 98)));
        object obj14 = ((IMarkupExtension) referenceExtension18).ProvideValue((IServiceProvider) xamlServiceProvider7);
        bindingExtension7.Source = obj14;
        VisualDiagnostics.RegisterSourceInfo(obj14, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 55, 98);
        bindingExtension7.Path = "TextColor";
        BindingBase bindingBase7 = ((IMarkupExtension<BindingBase>) bindingExtension7).ProvideValue((IServiceProvider) null);
        ((BindableObject) button3).SetBinding(Button.TextColorProperty, bindingBase7);
        ((BindableObject) button3).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) button3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.EndAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) button3);
        VisualDiagnostics.RegisterSourceInfo((object) button3, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 54, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 45, 18);
        ((BindableObject) stackLayout5).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout5).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod No Giriniz/Okutunuz");
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("True"));
        softkeyboardDisabledEntry2.Completed += new EventHandler(shelfEntry2.txtBarcode_Completed);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 59, 22);
        ((BindableObject) entry3).SetValue(Xamarin.Forms.Entry.TextProperty, (object) "1");
        ((BindableObject) entry3).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Miktar");
        ((BindableObject) entry3).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.EndAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) entry3);
        VisualDiagnostics.RegisterSourceInfo((object) entry3, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 60, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 58, 18);
        ((BindableObject) button4).SetValue(Button.TextProperty, (object) "Ekle/Okut");
        referenceExtension7.Name = "shelfentry";
        ReferenceExtension referenceExtension19 = referenceExtension7;
        XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
        Type type15 = typeof (IProvideValueTarget);
        object[] objArray9 = new object[0 + 5];
        objArray9[0] = (object) bindingExtension8;
        objArray9[1] = (object) button4;
        objArray9[2] = (object) stackLayout7;
        objArray9[3] = (object) stackLayout8;
        objArray9[4] = (object) shelfEntry2;
        SimpleValueTargetProvider valueTargetProvider8;
        object obj15 = (object) (valueTargetProvider8 = new SimpleValueTargetProvider(objArray9, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider8.Add(type15, (object) valueTargetProvider8);
        xamlServiceProvider8.Add(typeof (IReferenceProvider), obj15);
        Type type16 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver8 = new XmlNamespaceResolver();
        namespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver8.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver8 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver8, typeof (ShelfEntry2).GetTypeInfo().Assembly);
        xamlServiceProvider8.Add(type16, (object) xamlTypeResolver8);
        xamlServiceProvider8.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(63, 21)));
        object obj16 = ((IMarkupExtension) referenceExtension19).ProvideValue((IServiceProvider) xamlServiceProvider8);
        bindingExtension8.Source = obj16;
        VisualDiagnostics.RegisterSourceInfo(obj16, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 63, 21);
        bindingExtension8.Path = "ButtonColor";
        BindingBase bindingBase8 = ((IMarkupExtension<BindingBase>) bindingExtension8).ProvideValue((IServiceProvider) null);
        ((BindableObject) button4).SetBinding(VisualElement.BackgroundColorProperty, bindingBase8);
        referenceExtension8.Name = "shelfentry";
        ReferenceExtension referenceExtension20 = referenceExtension8;
        XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
        Type type17 = typeof (IProvideValueTarget);
        object[] objArray10 = new object[0 + 5];
        objArray10[0] = (object) bindingExtension9;
        objArray10[1] = (object) button4;
        objArray10[2] = (object) stackLayout7;
        objArray10[3] = (object) stackLayout8;
        objArray10[4] = (object) shelfEntry2;
        SimpleValueTargetProvider valueTargetProvider9;
        object obj17 = (object) (valueTargetProvider9 = new SimpleValueTargetProvider(objArray10, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider9.Add(type17, (object) valueTargetProvider9);
        xamlServiceProvider9.Add(typeof (IReferenceProvider), obj17);
        Type type18 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver9 = new XmlNamespaceResolver();
        namespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver9.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver9 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver9, typeof (ShelfEntry2).GetTypeInfo().Assembly);
        xamlServiceProvider9.Add(type18, (object) xamlTypeResolver9);
        xamlServiceProvider9.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(63, 94)));
        object obj18 = ((IMarkupExtension) referenceExtension20).ProvideValue((IServiceProvider) xamlServiceProvider9);
        bindingExtension9.Source = obj18;
        VisualDiagnostics.RegisterSourceInfo(obj18, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 63, 94);
        bindingExtension9.Path = "TextColor";
        BindingBase bindingBase9 = ((IMarkupExtension<BindingBase>) bindingExtension9).ProvideValue((IServiceProvider) null);
        ((BindableObject) button4).SetBinding(Button.TextColorProperty, bindingBase9);
        ((BindableObject) button4).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        button4.Clicked += new EventHandler(shelfEntry2.btnPickOrder_Clicked);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) button4);
        VisualDiagnostics.RegisterSourceInfo((object) button4, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 62, 18);
        ((BindableObject) button5).SetValue(Button.TextProperty, (object) "Tamamla");
        button5.Clicked += new EventHandler(shelfEntry2.btnShelfOrderSuccess_Clicked);
        referenceExtension9.Name = "shelfentry";
        ReferenceExtension referenceExtension21 = referenceExtension9;
        XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
        Type type19 = typeof (IProvideValueTarget);
        object[] objArray11 = new object[0 + 5];
        objArray11[0] = (object) bindingExtension10;
        objArray11[1] = (object) button5;
        objArray11[2] = (object) stackLayout7;
        objArray11[3] = (object) stackLayout8;
        objArray11[4] = (object) shelfEntry2;
        SimpleValueTargetProvider valueTargetProvider10;
        object obj19 = (object) (valueTargetProvider10 = new SimpleValueTargetProvider(objArray11, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider10.Add(type19, (object) valueTargetProvider10);
        xamlServiceProvider10.Add(typeof (IReferenceProvider), obj19);
        Type type20 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver10 = new XmlNamespaceResolver();
        namespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver10.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver10 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver10, typeof (ShelfEntry2).GetTypeInfo().Assembly);
        xamlServiceProvider10.Add(type20, (object) xamlTypeResolver10);
        xamlServiceProvider10.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(66, 21)));
        object obj20 = ((IMarkupExtension) referenceExtension21).ProvideValue((IServiceProvider) xamlServiceProvider10);
        bindingExtension10.Source = obj20;
        VisualDiagnostics.RegisterSourceInfo(obj20, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 66, 21);
        bindingExtension10.Path = "ButtonColor";
        BindingBase bindingBase10 = ((IMarkupExtension<BindingBase>) bindingExtension10).ProvideValue((IServiceProvider) null);
        ((BindableObject) button5).SetBinding(VisualElement.BackgroundColorProperty, bindingBase10);
        referenceExtension10.Name = "shelfentry";
        ReferenceExtension referenceExtension22 = referenceExtension10;
        XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
        Type type21 = typeof (IProvideValueTarget);
        object[] objArray12 = new object[0 + 5];
        objArray12[0] = (object) bindingExtension11;
        objArray12[1] = (object) button5;
        objArray12[2] = (object) stackLayout7;
        objArray12[3] = (object) stackLayout8;
        objArray12[4] = (object) shelfEntry2;
        SimpleValueTargetProvider valueTargetProvider11;
        object obj21 = (object) (valueTargetProvider11 = new SimpleValueTargetProvider(objArray12, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider11.Add(type21, (object) valueTargetProvider11);
        xamlServiceProvider11.Add(typeof (IReferenceProvider), obj21);
        Type type22 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver11 = new XmlNamespaceResolver();
        namespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver11.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver11 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver11, typeof (ShelfEntry2).GetTypeInfo().Assembly);
        xamlServiceProvider11.Add(type22, (object) xamlTypeResolver11);
        xamlServiceProvider11.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(66, 94)));
        object obj22 = ((IMarkupExtension) referenceExtension22).ProvideValue((IServiceProvider) xamlServiceProvider11);
        bindingExtension11.Source = obj22;
        VisualDiagnostics.RegisterSourceInfo(obj22, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 66, 94);
        bindingExtension11.Path = "TextColor";
        BindingBase bindingBase11 = ((IMarkupExtension<BindingBase>) bindingExtension11).ProvideValue((IServiceProvider) null);
        ((BindableObject) button5).SetBinding(Button.TextColorProperty, bindingBase11);
        ((BindableObject) button5).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) button5);
        VisualDiagnostics.RegisterSourceInfo((object) button5, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 65, 18);
        bindingExtension12.Path = ".";
        BindingBase bindingBase12 = ((IMarkupExtension<BindingBase>) bindingExtension12).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView2).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase12);
        ((BindableObject) listView2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) listView2).SetValue(VisualElement.HeightRequestProperty, (object) 500.0);
        ((BindableObject) listView2).SetValue(ListView.RowHeightProperty, (object) 100);
        ((BindableObject) listView2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout6).SetValue(Layout.PaddingProperty, (object) new Thickness(10.0, 5.0, 0.0, 5.0));
        ((BindableObject) stackLayout6).SetValue(VisualElement.BackgroundColorProperty, (object) Color.AliceBlue);
        ((BindableObject) label2).SetValue(Label.TextProperty, (object) "Raf Detayları");
        ((BindableObject) label2).SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        referenceExtension11.Name = "shelfentry";
        ReferenceExtension referenceExtension23 = referenceExtension11;
        XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
        Type type23 = typeof (IProvideValueTarget);
        object[] objArray13 = new object[0 + 7];
        objArray13[0] = (object) bindingExtension13;
        objArray13[1] = (object) label2;
        objArray13[2] = (object) stackLayout6;
        objArray13[3] = (object) listView2;
        objArray13[4] = (object) stackLayout7;
        objArray13[5] = (object) stackLayout8;
        objArray13[6] = (object) shelfEntry2;
        SimpleValueTargetProvider valueTargetProvider12;
        object obj23 = (object) (valueTargetProvider12 = new SimpleValueTargetProvider(objArray13, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider12.Add(type23, (object) valueTargetProvider12);
        xamlServiceProvider12.Add(typeof (IReferenceProvider), obj23);
        Type type24 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver12 = new XmlNamespaceResolver();
        namespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver12.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver12 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver12, typeof (ShelfEntry2).GetTypeInfo().Assembly);
        xamlServiceProvider12.Add(type24, (object) xamlTypeResolver12);
        xamlServiceProvider12.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(74, 36)));
        object obj24 = ((IMarkupExtension) referenceExtension23).ProvideValue((IServiceProvider) xamlServiceProvider12);
        bindingExtension13.Source = obj24;
        VisualDiagnostics.RegisterSourceInfo(obj24, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 74, 36);
        bindingExtension13.Path = "ButtonColor";
        BindingBase bindingBase13 = ((IMarkupExtension<BindingBase>) bindingExtension13).ProvideValue((IServiceProvider) null);
        ((BindableObject) label2).SetBinding(Label.TextColorProperty, bindingBase13);
        referenceExtension12.Name = "shelfentry";
        ReferenceExtension referenceExtension24 = referenceExtension12;
        XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
        Type type25 = typeof (IProvideValueTarget);
        object[] objArray14 = new object[0 + 7];
        objArray14[0] = (object) bindingExtension14;
        objArray14[1] = (object) label2;
        objArray14[2] = (object) stackLayout6;
        objArray14[3] = (object) listView2;
        objArray14[4] = (object) stackLayout7;
        objArray14[5] = (object) stackLayout8;
        objArray14[6] = (object) shelfEntry2;
        SimpleValueTargetProvider valueTargetProvider13;
        object obj25 = (object) (valueTargetProvider13 = new SimpleValueTargetProvider(objArray14, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider13.Add(type25, (object) valueTargetProvider13);
        xamlServiceProvider13.Add(typeof (IReferenceProvider), obj25);
        Type type26 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver13 = new XmlNamespaceResolver();
        namespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver13.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver13 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver13, typeof (ShelfEntry2).GetTypeInfo().Assembly);
        xamlServiceProvider13.Add(type26, (object) xamlTypeResolver13);
        xamlServiceProvider13.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(75, 36)));
        object obj26 = ((IMarkupExtension) referenceExtension24).ProvideValue((IServiceProvider) xamlServiceProvider13);
        bindingExtension14.Source = obj26;
        VisualDiagnostics.RegisterSourceInfo(obj26, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 75, 36);
        bindingExtension14.Path = "TextColor";
        BindingBase bindingBase14 = ((IMarkupExtension<BindingBase>) bindingExtension14).ProvideValue((IServiceProvider) null);
        ((BindableObject) label2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase14);
        ((BindableObject) label2).SetValue(Label.FontProperty, new FontTypeConverter().ConvertFromInvariantString("Bold,20"));
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) label2);
        VisualDiagnostics.RegisterSourceInfo((object) label2, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 72, 30);
        ((BindableObject) listView2).SetValue(ListView.HeaderProperty, (object) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 71, 26);
        DataTemplate dataTemplate4 = dataTemplate2;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ShelfEntry2.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_30 xamlCdataTemplate30 = new ShelfEntry2.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_30();
        object[] objArray15 = new object[0 + 5];
        objArray15[0] = (object) dataTemplate2;
        objArray15[1] = (object) listView2;
        objArray15[2] = (object) stackLayout7;
        objArray15[3] = (object) stackLayout8;
        objArray15[4] = (object) shelfEntry2;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate30.parentValues = objArray15;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate30.root = shelfEntry2;
        // ISSUE: reference to a compiler-generated method
        Func<object> func2 = new Func<object>(xamlCdataTemplate30.LoadDataTemplate);
        ((IDataTemplate) dataTemplate4).LoadTemplate = func2;
        ((BindableObject) listView2).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate2);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate2, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 80, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) listView2);
        VisualDiagnostics.RegisterSourceInfo((object) listView2, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 68, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 39, 14);
        ((BindableObject) shelfEntry2).SetValue(ContentPage.ContentProperty, (object) stackLayout8);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout8, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 8, 10);
        VisualDiagnostics.RegisterSourceInfo((object) shelfEntry2, new Uri("Views\\ShelfEntry2.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Extensions.LoadFromXaml<ShelfEntry2>(this, typeof (ShelfEntry2));
      this.shelfentry = NameScopeExtensions.FindByName<ContentPage>((Element) this, "shelfentry");
      this.stckContent = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckContent");
      this.stckShelfOrderList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfOrderList");
      this.stckEmptyMessage = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckEmptyMessage");
      this.lstShelfOrder = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfOrder");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtShelfOrderNumber = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtShelfOrderNumber");
      this.stckShelf = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelf");
      this.txtShelf = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtShelf");
      this.btnShelfBack = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelfBack");
      this.btnShelfNext = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelfNext");
      this.txtShelfBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtShelfBarcode");
      this.btnShelf = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelf");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.txtQty = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtQty");
      this.btnPickOrder = NameScopeExtensions.FindByName<Button>((Element) this, "btnPickOrder");
      this.btnShelfOrderSuccess = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelfOrderSuccess");
      this.lstShelfDetail = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfDetail");
      this.lblListHeader = NameScopeExtensions.FindByName<Label>((Element) this, "lblListHeader");
    }
  }
}
