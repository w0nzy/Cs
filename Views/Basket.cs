// Decompiled with JetBrains decompiler
// Type: Shelf.Views.Basket
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
  [XamlFilePath("Views\\Basket.xaml")]
  public class Basket : ContentPage
  {
    private List<pIOShelfOrderDetailBasketReturnModel> shelfOrderDetail;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage basket;
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
    private StackLayout stckBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtQty;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckBarcodeType;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnShelfOrderApproved;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstBasket;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Label lblListHeader;

    public Color ButtonColor => Color.FromRgb(21, 40, 151);

    public Color TextColor => Color.White;

    public Basket()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Raf Emirleri";
      GlobalMob.AddBarcodeLongPress(this.txtBarcode);
      ToolbarItem toolbarItem1 = new ToolbarItem();
      ((MenuItem) toolbarItem1).Text = "";
      ToolbarItem toolbarItem2 = toolbarItem1;
      ((MenuItem) toolbarItem2).Clicked += new EventHandler(this.TItem_Clicked);
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(toolbarItem2);
      ((VisualElement) this.txtQty).IsVisible = !GlobalMob.User.HideQty;
      GlobalMob.FillBarcodeType(this.pckBarcodeType);
    }

    private void TItem_Clicked(object sender, EventArgs e)
    {
      double num1 = this.shelfOrderDetail.Sum<pIOShelfOrderDetailBasketReturnModel>((Func<pIOShelfOrderDetailBasketReturnModel, double>) (x => x.ApproveQty));
      double num2 = this.shelfOrderDetail.Sum<pIOShelfOrderDetailBasketReturnModel>((Func<pIOShelfOrderDetailBasketReturnModel, double>) (x => x.PickingQty));
      ((Page) this).DisplayAlert("Miktar", "Toplanan Miktar : " + num1.ToString() + "\n" + "Toplam Miktar : " + num2.ToString(), "", "Tamam");
    }

    private void SetInfo()
    {
      double num1 = this.shelfOrderDetail.Sum<pIOShelfOrderDetailBasketReturnModel>((Func<pIOShelfOrderDetailBasketReturnModel, double>) (x => x.ApproveQty));
      double num2 = this.shelfOrderDetail.Sum<pIOShelfOrderDetailBasketReturnModel>((Func<pIOShelfOrderDetailBasketReturnModel, double>) (x => x.PickingQty));
      ((MenuItem) ((Page) this).ToolbarItems[0]).Text = num1.ToString() + "/" + num2.ToString();
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetUserShelfOrdersBasket?userID={0}", (object) GlobalMob.User.UserID), (ContentPage) this);
      if (returnModel.Success)
      {
        List<pIOUserShelfOrdersBasketReturnModel> basketReturnModelList1 = GlobalMob.JsonDeserialize<List<pIOUserShelfOrdersBasketReturnModel>>(returnModel.Result);
        List<pIOUserShelfOrdersBasketReturnModel> basketReturnModelList2 = basketReturnModelList1 == null ? new List<pIOUserShelfOrdersBasketReturnModel>() : basketReturnModelList1;
        ((VisualElement) this.lstShelfOrder).IsVisible = basketReturnModelList2.Count > 0;
        ((VisualElement) this.stckEmptyMessage).IsVisible = basketReturnModelList2.Count == 0;
        if (((VisualElement) this.stckEmptyMessage).IsVisible)
          ((View) this.stckContent).VerticalOptions = LayoutOptions.Center;
        ((BindableObject) this.lstShelfOrder).BindingContext = (object) basketReturnModelList2;
      }
      this.lstShelfOrder.ItemSelected += (EventHandler<SelectedItemChangedEventArgs>) ((sender, e) =>
      {
        object selectedItem = ((ListView) sender).SelectedItem;
        if (selectedItem == null)
          return;
        ((Page) this).Title = "Sepet Okut";
        pIOUserShelfOrdersBasketReturnModel basketReturnModel = (pIOUserShelfOrdersBasketReturnModel) selectedItem;
        ((InputView) this.txtShelfOrderNumber).Text = "";
        ((InputView) this.txtShelfOrderNumber).Text = basketReturnModel.ShelfOrderNumber.Replace("S", "");
        ((VisualElement) this.stckShelfOrderList).IsVisible = false;
        ((VisualElement) this.stckForm).IsVisible = true;
        this.FillListView();
        ((VisualElement) this.txtBarcode).Focus();
      });
    }

    private void FillListView(bool isBarcode = false, List<PickAndSort> returnList = null)
    {
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetShelfOrderDetailBasket?shelfOrderNumber=S{0}", (object) ((InputView) this.txtShelfOrderNumber).Text), (ContentPage) this);
      if (returnModel.Success)
      {
        this.shelfOrderDetail = GlobalMob.JsonDeserialize<List<pIOShelfOrderDetailBasketReturnModel>>(returnModel.Result);
        ((ItemsView<Cell>) this.lstBasket).ItemsSource = (IEnumerable) null;
        List<pIOShelfOrderDetailBasketReturnModel> source = this.GroupList();
        ((VisualElement) this.pckBarcodeType).IsVisible = this.shelfOrderDetail[0].ShelfOrderType == 1;
        if (isBarcode)
        {
          if (returnList != null)
          {
            foreach (PickAndSort pickAndSort in returnList)
            {
              PickAndSort returnItem = pickAndSort;
              pIOShelfOrderDetailBasketReturnModel basketReturnModel = source.Where<pIOShelfOrderDetailBasketReturnModel>((Func<pIOShelfOrderDetailBasketReturnModel, bool>) (x => x.Barcode.Contains(returnItem.barcode))).FirstOrDefault<pIOShelfOrderDetailBasketReturnModel>();
              if (basketReturnModel != null)
                basketReturnModel.LastReadBarcode = true;
            }
          }
          else
          {
            pIOShelfOrderDetailBasketReturnModel basketReturnModel = source.Where<pIOShelfOrderDetailBasketReturnModel>((Func<pIOShelfOrderDetailBasketReturnModel, bool>) (x => x.Barcode.Contains(((InputView) this.txtBarcode).Text))).FirstOrDefault<pIOShelfOrderDetailBasketReturnModel>();
            if (basketReturnModel != null)
              basketReturnModel.LastReadBarcode = true;
          }
        }
        ((ItemsView<Cell>) this.lstBasket).ItemsSource = (IEnumerable) source.OrderByDescending<pIOShelfOrderDetailBasketReturnModel, bool>((Func<pIOShelfOrderDetailBasketReturnModel, bool>) (x => x.LastReadBarcode)).ToList<pIOShelfOrderDetailBasketReturnModel>();
      }
      ((VisualElement) this.stckBarcode).IsVisible = true;
      ((VisualElement) this.lstBasket).IsVisible = true;
      ((VisualElement) this.btnShelfOrderApproved).IsVisible = true;
      this.SetInfo();
    }

    private List<pIOShelfOrderDetailBasketReturnModel> GroupList() => this.shelfOrderDetail.GroupBy(c => new
    {
      ItemCode = c.ItemCode,
      ItemDim1Code = c.ItemDim1Code,
      ItemDim2Code = c.ItemDim2Code,
      Barcode = c.Barcode,
      ColorCode = c.ColorCode,
      ItemDescription = c.ItemDescription
    }).Select<IGrouping<\u003C\u003Ef__AnonymousType7<string, string, string, string, string, string>, pIOShelfOrderDetailBasketReturnModel>, pIOShelfOrderDetailBasketReturnModel>(gcs => new pIOShelfOrderDetailBasketReturnModel()
    {
      ItemCode = gcs.Key.ItemCode,
      ItemDim1Code = gcs.Key.ItemDim1Code,
      ItemDim2Code = gcs.Key.ItemDim2Code,
      ColorCode = gcs.Key.ColorCode,
      Barcode = gcs.Key.Barcode,
      ItemDescription = gcs.Key.ItemDescription,
      ApproveQty = gcs.Sum<pIOShelfOrderDetailBasketReturnModel>((Func<pIOShelfOrderDetailBasketReturnModel, double>) (c => c.ApproveQty)),
      PickingQty = gcs.Sum<pIOShelfOrderDetailBasketReturnModel>((Func<pIOShelfOrderDetailBasketReturnModel, double>) (c => c.PickingQty)),
      LastReadBarcode = gcs.Max<pIOShelfOrderDetailBasketReturnModel, bool>((Func<pIOShelfOrderDetailBasketReturnModel, bool>) (c => c.LastReadBarcode))
    }).OrderByDescending<pIOShelfOrderDetailBasketReturnModel, bool>((Func<pIOShelfOrderDetailBasketReturnModel, bool>) (x => x.LastReadBarcode)).ToList<pIOShelfOrderDetailBasketReturnModel>();

    private bool IsUniqueControl(string barcode)
    {
      pIOShelfOrderDetailBasketReturnModel basketReturnModel = this.shelfOrderDetail.Where<pIOShelfOrderDetailBasketReturnModel>((Func<pIOShelfOrderDetailBasketReturnModel, bool>) (x => x.UsedBarcode.Contains(barcode) || x.Barcode.Contains(barcode))).FirstOrDefault<pIOShelfOrderDetailBasketReturnModel>();
      return basketReturnModel != null && basketReturnModel.UseSerialNumber;
    }

    private async void txtBarcode_Completed(object sender, EventArgs e)
    {
      Basket page = this;
      if (string.IsNullOrEmpty(((InputView) page.txtBarcode).Text))
        return;
      string text = ((InputView) page.txtBarcode).Text;
      if (text.Length < GlobalMob.User.MinimumBarcodeLength)
      {
        GlobalMob.PlayError();
        ((InputView) page.txtBarcode).Text = "";
        ((VisualElement) page.txtBarcode).Focus();
      }
      else
      {
        bool flag1 = false;
        bool flag2 = page.IsUniqueControl(text);
        PickerItem selectedItem = (PickerItem) page.pckBarcodeType.SelectedItem;
        if (selectedItem != null && selectedItem.Code == 2 && ((VisualElement) page.pckBarcodeType).IsVisible)
          flag1 = true;
        // ISSUE: reference to a compiler-generated method
        List<pIOShelfOrderDetailBasketReturnModel> list = page.shelfOrderDetail.Where<pIOShelfOrderDetailBasketReturnModel>(new Func<pIOShelfOrderDetailBasketReturnModel, bool>(page.\u003CtxtBarcode_Completed\u003Eb__12_0)).ToList<pIOShelfOrderDetailBasketReturnModel>();
        if (flag2)
        {
          // ISSUE: reference to a compiler-generated method
          list = page.shelfOrderDetail.Where<pIOShelfOrderDetailBasketReturnModel>(new Func<pIOShelfOrderDetailBasketReturnModel, bool>(page.\u003CtxtBarcode_Completed\u003Eb__12_1)).ToList<pIOShelfOrderDetailBasketReturnModel>();
        }
        if (list.GroupBy<pIOShelfOrderDetailBasketReturnModel, string>((Func<pIOShelfOrderDetailBasketReturnModel, string>) (x => x.Barcode)).Count<IGrouping<string, pIOShelfOrderDetailBasketReturnModel>>() > 1 && !flag2)
          list = page.shelfOrderDetail.Where<pIOShelfOrderDetailBasketReturnModel>((Func<pIOShelfOrderDetailBasketReturnModel, bool>) (x => x.Barcode.Contains(list[0].Barcode) && x.PickingQty != x.ApproveQty)).ToList<pIOShelfOrderDetailBasketReturnModel>();
        if (list.Count<pIOShelfOrderDetailBasketReturnModel>() == 0 && !flag1)
        {
          GlobalMob.PlayError();
          int num = await ((Page) page).DisplayAlert("Bilgi", "Miktar Yetersiz", "", "Tamam") ? 1 : 0;
          ((InputView) page.txtBarcode).Text = "";
          ((VisualElement) page.txtBarcode).Focus();
        }
        else
        {
          if (flag1)
          {
            list = page.shelfOrderDetail.Where<pIOShelfOrderDetailBasketReturnModel>((Func<pIOShelfOrderDetailBasketReturnModel, bool>) (x => x.PickingQty != x.ApproveQty)).ToList<pIOShelfOrderDetailBasketReturnModel>();
            PickAndSort pickAndSort1 = new PickAndSort()
            {
              barcode = ((InputView) page.txtBarcode).Text,
              PickQty = Convert.ToInt32(((InputView) page.txtQty).Text),
              userName = GlobalMob.User.UserName,
              Detail = new List<PickAndSortDetail>()
            };
            foreach (pIOShelfOrderDetailBasketReturnModel basketReturnModel in list)
              pickAndSort1.Detail.Add(new PickAndSortDetail()
              {
                Barcode = basketReturnModel.Barcode,
                ColorCode = basketReturnModel.ColorCode,
                ItemCode = basketReturnModel.ItemCode,
                ItemDim1Code = basketReturnModel.ItemDim1Code,
                ItemDim2Code = basketReturnModel.ItemDim2Code,
                PickingQty = basketReturnModel.PickingQty,
                OrderQty = basketReturnModel.OrderQty,
                ApproveQty = basketReturnModel.ApproveQty,
                ShelfOrderDetailID = basketReturnModel.ShelfOrderDetailID,
                ShelfOrderID = basketReturnModel.ShelfOrderID
              });
            Dictionary<string, string> paramList = new Dictionary<string, string>();
            string str = JsonConvert.SerializeObject((object) pickAndSort1);
            paramList.Add("json", str);
            ReturnModel result = GlobalMob.PostJson("BasketAndSortLot", paramList, (ContentPage) page).Result;
            if (result.Success)
            {
              List<PickAndSort> returnList = JsonConvert.DeserializeObject<List<PickAndSort>>(result.Result);
              if (returnList == null)
              {
                GlobalMob.PlayError();
                int num = await ((Page) page).DisplayAlert("Bilgi", "Hata Oluştu", "", "Tamam") ? 1 : 0;
                ((InputView) page.txtBarcode).Text = "";
                ((VisualElement) page.txtBarcode).Focus();
                return;
              }
              if (returnList.Count == 0)
              {
                GlobalMob.PlayError();
                int num = await ((Page) page).DisplayAlert("Bilgi", "Lot Miktarı Yetersiz", "", "Tamam") ? 1 : 0;
                ((InputView) page.txtBarcode).Text = "";
                ((VisualElement) page.txtBarcode).Focus();
                return;
              }
              foreach (PickAndSort pickAndSort2 in returnList)
              {
                PickAndSort returnItem = pickAndSort2;
                pIOShelfOrderDetailBasketReturnModel basketReturnModel = page.shelfOrderDetail.Where<pIOShelfOrderDetailBasketReturnModel>((Func<pIOShelfOrderDetailBasketReturnModel, bool>) (x => x.ShelfOrderDetailID == returnItem.ShelfOrderDetailID)).FirstOrDefault<pIOShelfOrderDetailBasketReturnModel>();
                basketReturnModel.ApproveQty = (double) returnItem.ApproveQty;
                if (basketReturnModel.PickingQty == basketReturnModel.ApproveQty)
                  page.shelfOrderDetail.Remove(basketReturnModel);
              }
              page.FillListView(true, returnList);
            }
          }
          else
          {
            ReturnModel returnModel1 = new ReturnModel();
            ReturnModel returnModel2 = !flag2 ? GlobalMob.PostJson(string.Format("UpdateApproveShelfOrderDetail?shelfOrderDetailIDs={0}&pickQty={1}&barcode={2}", (object) string.Join<int>(",", (IEnumerable<int>) list.Select<pIOShelfOrderDetailBasketReturnModel, int>((Func<pIOShelfOrderDetailBasketReturnModel, int>) (x => x.ShelfOrderDetailID)).ToArray<int>()), (object) ((InputView) page.txtQty).Text, (object) list[0].Barcode), (ContentPage) page) : GlobalMob.PostJson(string.Format("UpdateApproveShelfOrderDetailUnique?shelfOrderDetailID={0}&pickQty={1}&barcode={2}", (object) list[0].ShelfOrderDetailID, (object) ((InputView) page.txtQty).Text, (object) list[0].UsedBarcode), (ContentPage) page);
            if (returnModel2.Success)
            {
              if (flag2)
              {
                ztIOShelfOrderDetail returnItem = JsonConvert.DeserializeObject<ztIOShelfOrderDetail>(returnModel2.Result);
                if (returnItem == null)
                {
                  GlobalMob.PlayError();
                  int num = await ((Page) page).DisplayAlert("Bilgi", "Hata Oluştu", "", "Tamam") ? 1 : 0;
                  ((InputView) page.txtBarcode).Text = "";
                  ((VisualElement) page.txtBarcode).Focus();
                  return;
                }
                pIOShelfOrderDetailBasketReturnModel basketReturnModel = page.shelfOrderDetail.Where<pIOShelfOrderDetailBasketReturnModel>((Func<pIOShelfOrderDetailBasketReturnModel, bool>) (x => x.ShelfOrderDetailID == returnItem.ShelfOrderDetailID)).FirstOrDefault<pIOShelfOrderDetailBasketReturnModel>();
                basketReturnModel.ApproveQty = returnItem.ApproveQty;
                if (basketReturnModel.PickingQty == basketReturnModel.ApproveQty)
                  page.shelfOrderDetail.Remove(basketReturnModel);
                page.FillListView(true);
              }
              else
              {
                List<ztIOShelfOrderDetail> shelfOrderDetailList = JsonConvert.DeserializeObject<List<ztIOShelfOrderDetail>>(returnModel2.Result);
                if (shelfOrderDetailList == null)
                {
                  GlobalMob.PlayError();
                  int num = await ((Page) page).DisplayAlert("Bilgi", "Hata Oluştu", "", "Tamam") ? 1 : 0;
                  ((InputView) page.txtBarcode).Text = "";
                  ((VisualElement) page.txtBarcode).Focus();
                  return;
                }
                if (shelfOrderDetailList.Count == 0)
                {
                  GlobalMob.PlayError();
                  int num = await ((Page) page).DisplayAlert("Bilgi", "Miktar Yetersiz", "", "Tamam") ? 1 : 0;
                  ((InputView) page.txtBarcode).Text = "";
                  ((VisualElement) page.txtBarcode).Focus();
                  return;
                }
                foreach (ztIOShelfOrderDetail shelfOrderDetail in shelfOrderDetailList)
                {
                  ztIOShelfOrderDetail returnItem = shelfOrderDetail;
                  pIOShelfOrderDetailBasketReturnModel basketReturnModel = page.shelfOrderDetail.Where<pIOShelfOrderDetailBasketReturnModel>((Func<pIOShelfOrderDetailBasketReturnModel, bool>) (x => x.ShelfOrderDetailID == returnItem.ShelfOrderDetailID)).FirstOrDefault<pIOShelfOrderDetailBasketReturnModel>();
                  basketReturnModel.ApproveQty = returnItem.ApproveQty;
                  if (basketReturnModel.PickingQty == basketReturnModel.ApproveQty)
                    page.shelfOrderDetail.Remove(basketReturnModel);
                }
                page.FillListView(true);
              }
            }
          }
          ((InputView) page.txtBarcode).Text = "";
          ((InputView) page.txtQty).Text = "1";
          // ISSUE: reference to a compiler-generated method
          Device.BeginInvokeOnMainThread(new Action(page.\u003CtxtBarcode_Completed\u003Eb__12_4));
        }
      }
    }

    private async void btnShelfOrderApproved_Clicked(object sender, EventArgs e)
    {
      Basket page1 = this;
      if (!string.IsNullOrEmpty(((InputView) page1.txtShelfOrderNumber).Text))
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("ShelfOrderApproveCompleted?shelfOrderNumber={0}&isCompleted=false&userName={1}", (object) ((InputView) page1.txtShelfOrderNumber).Text, (object) GlobalMob.User.UserName), (ContentPage) page1);
        if (!returnModel.Success)
          return;
        if (!string.IsNullOrEmpty(returnModel.Result.Replace("\"", "")))
        {
          if (await ((Page) page1).DisplayAlert("Devam?", "Ürünler tamamlanmadı.Devam etmek istiyor musunuz?", "Evet", "Hayır"))
          {
            if (!GlobalMob.PostJson(string.Format("ShelfOrderApproveCompleted?shelfOrderNumber={0}&isCompleted=true&userName={1}", (object) ((InputView) page1.txtShelfOrderNumber).Text, (object) GlobalMob.User.UserName), (ContentPage) page1).Success)
              return;
            int num = await ((Page) page1).DisplayAlert("Bilgi", "Sepet Sayımı Tamamlandı", "", "Tamam") ? 1 : 0;
            Page page2 = await ((NavigableElement) page1).Navigation.PopAsync();
          }
          else
            page1.FillListView();
        }
        else
        {
          if (!GlobalMob.PostJson(string.Format("ShelfOrderApproveCompleted?shelfOrderNumber={0}&isCompleted=true&userName", (object) ((InputView) page1.txtShelfOrderNumber).Text, (object) GlobalMob.User.UserName), (ContentPage) page1).Success)
            return;
          int num = await ((Page) page1).DisplayAlert("Bilgi", "Sepet Sayımı Tamamlandı", "", "Tamam") ? 1 : 0;
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
        AssemblyName = typeof (Basket).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/Basket.xaml",
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
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry = new SoftkeyboardDisabledEntry();
        Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
        BindingExtension bindingExtension2 = new BindingExtension();
        BindingExtension bindingExtension3 = new BindingExtension();
        Picker picker = new Picker();
        StackLayout stackLayout4 = new StackLayout();
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension4 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension5 = new BindingExtension();
        Button button = new Button();
        StackLayout stackLayout5 = new StackLayout();
        ReferenceExtension referenceExtension3 = new ReferenceExtension();
        BindingExtension bindingExtension6 = new BindingExtension();
        ReferenceExtension referenceExtension4 = new ReferenceExtension();
        BindingExtension bindingExtension7 = new BindingExtension();
        Label label2 = new Label();
        StackLayout stackLayout6 = new StackLayout();
        DataTemplate dataTemplate2 = new DataTemplate();
        ListView listView2 = new ListView();
        StackLayout stackLayout7 = new StackLayout();
        StackLayout stackLayout8 = new StackLayout();
        StackLayout stackLayout9 = new StackLayout();
        Basket basket;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (basket = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) basket, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("basket", (object) basket);
        if (((Element) basket).StyleId == null)
          ((Element) basket).StyleId = "basket";
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
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout8);
        if (((Element) stackLayout8).StyleId == null)
          ((Element) stackLayout8).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtShelfOrderNumber", (object) entry1);
        if (((Element) entry1).StyleId == null)
          ((Element) entry1).StyleId = "txtShelfOrderNumber";
        ((INameScope) nameScope).RegisterName("stckBarcode", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckBarcode";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry);
        if (((Element) softkeyboardDisabledEntry).StyleId == null)
          ((Element) softkeyboardDisabledEntry).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("txtQty", (object) entry2);
        if (((Element) entry2).StyleId == null)
          ((Element) entry2).StyleId = "txtQty";
        ((INameScope) nameScope).RegisterName("pckBarcodeType", (object) picker);
        if (((Element) picker).StyleId == null)
          ((Element) picker).StyleId = "pckBarcodeType";
        ((INameScope) nameScope).RegisterName("btnShelfOrderApproved", (object) button);
        if (((Element) button).StyleId == null)
          ((Element) button).StyleId = "btnShelfOrderApproved";
        ((INameScope) nameScope).RegisterName("lstBasket", (object) listView2);
        if (((Element) listView2).StyleId == null)
          ((Element) listView2).StyleId = "lstBasket";
        ((INameScope) nameScope).RegisterName("lblListHeader", (object) label2);
        if (((Element) label2).StyleId == null)
          ((Element) label2).StyleId = "lblListHeader";
        this.basket = (ContentPage) basket;
        this.stckContent = stackLayout9;
        this.stckShelfOrderList = stackLayout2;
        this.stckEmptyMessage = stackLayout1;
        this.lstShelfOrder = listView1;
        this.stckForm = stackLayout8;
        this.txtShelfOrderNumber = entry1;
        this.stckBarcode = stackLayout4;
        this.txtBarcode = softkeyboardDisabledEntry;
        this.txtQty = entry2;
        this.pckBarcodeType = picker;
        this.btnShelfOrderApproved = button;
        this.lstBasket = listView2;
        this.lblListHeader = label2;
        ((BindableObject) stackLayout9).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
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
        objArray1[3] = (object) stackLayout9;
        objArray1[4] = (object) basket;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) Label.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (Basket).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(12, 128)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter).ConvertFromInvariantString("Medium", (IServiceProvider) xamlServiceProvider1);
        ((BindableObject) label3).SetValue(fontSizeProperty, obj2);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) label1);
        VisualDiagnostics.RegisterSourceInfo((object) label1, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 18);
        bindingExtension1.Path = ".";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView1).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase1);
        DataTemplate dataTemplate3 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Basket.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_7 xamlCdataTemplate7 = new Basket.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_7();
        object[] objArray2 = new object[0 + 5];
        objArray2[0] = (object) dataTemplate1;
        objArray2[1] = (object) listView1;
        objArray2[2] = (object) stackLayout2;
        objArray2[3] = (object) stackLayout9;
        objArray2[4] = (object) basket;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate7.parentValues = objArray2;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate7.root = basket;
        // ISSUE: reference to a compiler-generated method
        Func<object> func1 = new Func<object>(xamlCdataTemplate7.LoadDataTemplate);
        ((IDataTemplate) dataTemplate3).LoadTemplate = func1;
        ((BindableObject) listView1).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) listView1);
        VisualDiagnostics.RegisterSourceInfo((object) listView1, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout9).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 14);
        ((BindableObject) stackLayout8).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout8).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout8).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf Emri Numarası Giriniz");
        ((BindableObject) entry1).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 42, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 41, 18);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout4).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) softkeyboardDisabledEntry).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod No Giriniz/Okutunuz");
        softkeyboardDisabledEntry.Completed += new EventHandler(basket.txtBarcode_Completed);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) softkeyboardDisabledEntry);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 45, 22);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.TextProperty, (object) "1");
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Miktar");
        ((BindableObject) entry2).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 46, 22);
        ((BindableObject) picker).SetValue(Picker.TitleProperty, (object) "Tip");
        bindingExtension2.Path = ".";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker).SetBinding(Picker.ItemsSourceProperty, bindingBase2);
        bindingExtension3.Path = "Caption";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        picker.ItemDisplayBinding = bindingBase3;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase3, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 48, 33);
        ((BindableObject) picker).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) picker).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) picker);
        VisualDiagnostics.RegisterSourceInfo((object) picker, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 47, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 44, 18);
        ((BindableObject) button).SetValue(Button.TextProperty, (object) "Tamamla");
        referenceExtension1.Name = "basket";
        ReferenceExtension referenceExtension5 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 6];
        objArray3[0] = (object) bindingExtension4;
        objArray3[1] = (object) button;
        objArray3[2] = (object) stackLayout5;
        objArray3[3] = (object) stackLayout8;
        objArray3[4] = (object) stackLayout9;
        objArray3[5] = (object) basket;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray3, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver2.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (Basket).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(53, 25)));
        object obj4 = ((IMarkupExtension) referenceExtension5).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension4.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 53, 25);
        bindingExtension4.Path = "ButtonColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button).SetBinding(VisualElement.BackgroundColorProperty, bindingBase4);
        referenceExtension2.Name = "basket";
        ReferenceExtension referenceExtension6 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 6];
        objArray4[0] = (object) bindingExtension5;
        objArray4[1] = (object) button;
        objArray4[2] = (object) stackLayout5;
        objArray4[3] = (object) stackLayout8;
        objArray4[4] = (object) stackLayout9;
        objArray4[5] = (object) basket;
        SimpleValueTargetProvider valueTargetProvider3;
        object obj5 = (object) (valueTargetProvider3 = new SimpleValueTargetProvider(objArray4, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider3.Add(type5, (object) valueTargetProvider3);
        xamlServiceProvider3.Add(typeof (IReferenceProvider), obj5);
        Type type6 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver3 = new XmlNamespaceResolver();
        namespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver3.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (Basket).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(53, 94)));
        object obj6 = ((IMarkupExtension) referenceExtension6).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension5.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 53, 94);
        bindingExtension5.Path = "TextColor";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) button).SetBinding(Button.TextColorProperty, bindingBase5);
        button.Clicked += new EventHandler(basket.btnShelfOrderApproved_Clicked);
        ((BindableObject) button).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) button);
        VisualDiagnostics.RegisterSourceInfo((object) button, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 52, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 51, 18);
        ((BindableObject) listView2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) listView2).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        ((BindableObject) listView2).SetValue(VisualElement.HeightRequestProperty, (object) 500.0);
        ((BindableObject) label2).SetValue(Label.TextProperty, (object) "Sepetteki Ürünler");
        ((BindableObject) label2).SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        referenceExtension3.Name = "basket";
        ReferenceExtension referenceExtension7 = referenceExtension3;
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray5 = new object[0 + 8];
        objArray5[0] = (object) bindingExtension6;
        objArray5[1] = (object) label2;
        objArray5[2] = (object) stackLayout6;
        objArray5[3] = (object) listView2;
        objArray5[4] = (object) stackLayout7;
        objArray5[5] = (object) stackLayout8;
        objArray5[6] = (object) stackLayout9;
        objArray5[7] = (object) basket;
        SimpleValueTargetProvider valueTargetProvider4;
        object obj7 = (object) (valueTargetProvider4 = new SimpleValueTargetProvider(objArray5, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider4.Add(type7, (object) valueTargetProvider4);
        xamlServiceProvider4.Add(typeof (IReferenceProvider), obj7);
        Type type8 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver4 = new XmlNamespaceResolver();
        namespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver4.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (Basket).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(62, 36)));
        object obj8 = ((IMarkupExtension) referenceExtension7).ProvideValue((IServiceProvider) xamlServiceProvider4);
        bindingExtension6.Source = obj8;
        VisualDiagnostics.RegisterSourceInfo(obj8, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 62, 36);
        bindingExtension6.Path = "ButtonColor";
        BindingBase bindingBase6 = ((IMarkupExtension<BindingBase>) bindingExtension6).ProvideValue((IServiceProvider) null);
        ((BindableObject) label2).SetBinding(Label.TextColorProperty, bindingBase6);
        referenceExtension4.Name = "basket";
        ReferenceExtension referenceExtension8 = referenceExtension4;
        XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
        Type type9 = typeof (IProvideValueTarget);
        object[] objArray6 = new object[0 + 8];
        objArray6[0] = (object) bindingExtension7;
        objArray6[1] = (object) label2;
        objArray6[2] = (object) stackLayout6;
        objArray6[3] = (object) listView2;
        objArray6[4] = (object) stackLayout7;
        objArray6[5] = (object) stackLayout8;
        objArray6[6] = (object) stackLayout9;
        objArray6[7] = (object) basket;
        SimpleValueTargetProvider valueTargetProvider5;
        object obj9 = (object) (valueTargetProvider5 = new SimpleValueTargetProvider(objArray6, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider5.Add(type9, (object) valueTargetProvider5);
        xamlServiceProvider5.Add(typeof (IReferenceProvider), obj9);
        Type type10 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver5 = new XmlNamespaceResolver();
        namespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver5.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver5 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver5, typeof (Basket).GetTypeInfo().Assembly);
        xamlServiceProvider5.Add(type10, (object) xamlTypeResolver5);
        xamlServiceProvider5.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(63, 36)));
        object obj10 = ((IMarkupExtension) referenceExtension8).ProvideValue((IServiceProvider) xamlServiceProvider5);
        bindingExtension7.Source = obj10;
        VisualDiagnostics.RegisterSourceInfo(obj10, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 63, 36);
        bindingExtension7.Path = "TextColor";
        BindingBase bindingBase7 = ((IMarkupExtension<BindingBase>) bindingExtension7).ProvideValue((IServiceProvider) null);
        ((BindableObject) label2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase7);
        ((BindableObject) label2).SetValue(Label.FontProperty, new FontTypeConverter().ConvertFromInvariantString("Bold,20"));
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) label2);
        VisualDiagnostics.RegisterSourceInfo((object) label2, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 60, 34);
        ((BindableObject) listView2).SetValue(ListView.HeaderProperty, (object) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 59, 30);
        DataTemplate dataTemplate4 = dataTemplate2;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Basket.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_8 xamlCdataTemplate8 = new Basket.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_8();
        object[] objArray7 = new object[0 + 6];
        objArray7[0] = (object) dataTemplate2;
        objArray7[1] = (object) listView2;
        objArray7[2] = (object) stackLayout7;
        objArray7[3] = (object) stackLayout8;
        objArray7[4] = (object) stackLayout9;
        objArray7[5] = (object) basket;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate8.parentValues = objArray7;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate8.root = basket;
        // ISSUE: reference to a compiler-generated method
        Func<object> func2 = new Func<object>(xamlCdataTemplate8.LoadDataTemplate);
        ((IDataTemplate) dataTemplate4).LoadTemplate = func2;
        ((BindableObject) listView2).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate2);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate2, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 68, 30);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) listView2);
        VisualDiagnostics.RegisterSourceInfo((object) listView2, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 57, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 56, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout9).Children).Add((View) stackLayout8);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout8, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 40, 14);
        ((BindableObject) basket).SetValue(ContentPage.ContentProperty, (object) stackLayout9);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout9, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 8, 10);
        VisualDiagnostics.RegisterSourceInfo((object) basket, new Uri("Views\\Basket.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Extensions.LoadFromXaml<Basket>(this, typeof (Basket));
      this.basket = NameScopeExtensions.FindByName<ContentPage>((Element) this, "basket");
      this.stckContent = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckContent");
      this.stckShelfOrderList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelfOrderList");
      this.stckEmptyMessage = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckEmptyMessage");
      this.lstShelfOrder = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfOrder");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtShelfOrderNumber = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtShelfOrderNumber");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.txtQty = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtQty");
      this.pckBarcodeType = NameScopeExtensions.FindByName<Picker>((Element) this, "pckBarcodeType");
      this.btnShelfOrderApproved = NameScopeExtensions.FindByName<Button>((Element) this, "btnShelfOrderApproved");
      this.lstBasket = NameScopeExtensions.FindByName<ListView>((Element) this, "lstBasket");
      this.lblListHeader = NameScopeExtensions.FindByName<Label>((Element) this, "lblListHeader");
    }
  }
}
