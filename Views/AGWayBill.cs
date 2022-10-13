// Decompiled with JetBrains decompiler
// Type: Shelf.Views.AGWayBill
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
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;
using XFNoSoftKeyboadEntryControl;

namespace Shelf.Views
{
  [XamlCompilation]
  [XamlFilePath("Views\\AGWayBill.xaml")]
  public class AGWayBill : ContentPage
  {
    private pIOGetErpGoodsInReturnModel selectedGoodsin;
    private List<pIOGetErpGoodsInReturnModel> goodsinList;
    private List<pIOGetErpGoodsInDetailReturnModel> goodsinDetailList;
    private ListView lstGoodsIn;
    private List<pIOGetRemainingDispOrderByItemReturnModel> remainingDispOrderList;
    private ListView lstRemaining;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private DatePicker dtWayBill;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Entry txtWayBillNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstGoodsInDetails;

    public AGWayBill()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Mal Kabul(İrsaliye)";
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      this.RefreshData();
    }

    private void RefreshData()
    {
      if (this.goodsinDetailList == null)
        return;
      IEnumerable<pIOGetErpGoodsInDetailReturnModel> detailReturnModels = this.goodsinDetailList.GroupBy(c => new
      {
        Barcode = c.Barcode,
        ColorCode = c.ColorCode,
        ItemCode = c.ItemCode,
        ItemDim1Code = c.ItemDim1Code,
        ItemDim2Code = c.ItemDim2Code,
        ItemDim3Code = c.ItemDim3Code,
        ItemTypeCode = c.ItemTypeCode,
        Qty = c.Qty,
        ShipmentLineID = c.ShipmentLineID
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType3<string, string, string, string, string, string, byte, double, Guid>, pIOGetErpGoodsInDetailReturnModel>, pIOGetErpGoodsInDetailReturnModel>(gcs => new pIOGetErpGoodsInDetailReturnModel()
      {
        Barcode = gcs.Key.Barcode,
        ColorCode = gcs.Key.ColorCode,
        ItemCode = gcs.Key.ItemCode,
        ItemDim1Code = gcs.Key.ItemDim1Code,
        ItemDim2Code = gcs.Key.ItemDim2Code,
        ItemDim3Code = gcs.Key.ItemDim3Code,
        ItemTypeCode = gcs.Key.ItemTypeCode,
        Qty = gcs.Key.Qty,
        AlcQty = gcs.Sum<pIOGetErpGoodsInDetailReturnModel>((Func<pIOGetErpGoodsInDetailReturnModel, int?>) (x => x.AlcQty))
      });
      ((ItemsView<Cell>) this.lstGoodsInDetails).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstGoodsInDetails).ItemsSource = (IEnumerable) detailReturnModels;
    }

    private async void txtWayBillNumber_Focused(object sender, FocusEventArgs e)
    {
      AGWayBill page = this;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetErpGoodsIn?date={0}", (object) page.dtWayBill.Date.ToString("dd.MM.yyyy")), (ContentPage) page);
      if (!returnModel.Success)
        return;
      StackLayout stck = new StackLayout();
      stck.Orientation = (StackOrientation) 0;
      page.goodsinList = GlobalMob.JsonDeserialize<List<pIOGetErpGoodsInReturnModel>>(returnModel.Result);
      page.lstGoodsIn = GlobalMob.GetListview("CurrAccDesc,CurrAccCode,ShippingNumber,Qty", 2, 2);
      SearchBar searchBar1 = new SearchBar();
      ((InputView) searchBar1).Placeholder = "Ara";
      ((InputView) searchBar1).PlaceholderColor = Color.Orange;
      ((InputView) searchBar1).TextColor = Color.Orange;
      searchBar1.HorizontalTextAlignment = (TextAlignment) 1;
      searchBar1.FontSize = Device.GetNamedSize((NamedSize) 3, typeof (SearchBar));
      searchBar1.FontAttributes = (FontAttributes) 2;
      SearchBar searchBar2 = searchBar1;
      ((InputView) searchBar2).TextChanged += new EventHandler<TextChangedEventArgs>(page.SearchBar_TextChanged);
      page.lstGoodsIn.RowHeight = 120;
      ((ItemsView<Cell>) page.lstGoodsIn).ItemsSource = (IEnumerable) page.goodsinList;
      page.lstGoodsIn.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(page.LstGoodsIn_ItemSelected);
      ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) searchBar2);
      ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) page.lstGoodsIn);
      SelectItem selectItem = new SelectItem(stck, "İrsaliye Seçiniz");
      await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
      SearchBar searchBar = (SearchBar) sender;
      string text = ((InputView) searchBar).Text.ToLower();
      if (string.IsNullOrEmpty(((InputView) searchBar).Text))
        ((ItemsView<Cell>) this.lstGoodsIn).ItemsSource = (IEnumerable) this.goodsinList;
      else
        ((ItemsView<Cell>) this.lstGoodsIn).ItemsSource = (IEnumerable) this.goodsinList.Where<pIOGetErpGoodsInReturnModel>((Func<pIOGetErpGoodsInReturnModel, bool>) (x => x.CurrAccCode.ToLower().Contains(text) || x.CurrAccDesc.ToLower().Contains(text) || x.ShippingNumber.ToLower().Contains(text))).ToList<pIOGetErpGoodsInReturnModel>();
    }

    private void LoadData()
    {
    }

    private void LstGoodsIn_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      this.selectedGoodsin = (pIOGetErpGoodsInReturnModel) e.SelectedItem;
      ((InputView) this.txtWayBillNumber).Text = this.selectedGoodsin.ShippingNumber;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetErpGoodsInDetail?shipmentHeaderID={0}", (object) this.selectedGoodsin.ShipmentHeaderID), (ContentPage) this);
      if (!returnModel.Success)
        return;
      this.goodsinDetailList = GlobalMob.JsonDeserialize<List<pIOGetErpGoodsInDetailReturnModel>>(returnModel.Result);
      this.RefreshData();
      ((NavigableElement) this).Navigation.PopAsync();
      this.BarcodeFocus();
    }

    private async void txtBarcode_Completed(object sender, EventArgs e)
    {
      AGWayBill page = this;
      string barcode = ((InputView) page.txtBarcode).Text;
      if (string.IsNullOrEmpty(barcode))
        return;
      pIOGetErpGoodsInDetailReturnModel detailReturnModel = page.goodsinDetailList.Where<pIOGetErpGoodsInDetailReturnModel>((Func<pIOGetErpGoodsInDetailReturnModel, bool>) (x => x.Barcode.Trim() == barcode.Trim())).FirstOrDefault<pIOGetErpGoodsInDetailReturnModel>();
      if (detailReturnModel != null)
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetRemainingDispOrderByItem?json={0}", (object) JsonConvert.SerializeObject((object) new ItemProps()
        {
          ColorCode = detailReturnModel.ColorCode,
          ItemCode = detailReturnModel.ItemCode,
          ItemDim1Code = detailReturnModel.ItemDim1Code,
          ItemDim2Code = detailReturnModel.ItemDim2Code,
          ItemDim3Code = detailReturnModel.ItemDim3Code,
          ItemTypeCode = (int) detailReturnModel.ItemTypeCode
        })), (ContentPage) page);
        if (!returnModel.Success)
          return;
        page.remainingDispOrderList = GlobalMob.JsonDeserialize<List<pIOGetRemainingDispOrderByItemReturnModel>>(returnModel.Result);
        AGSelectionList agSelectionList = new AGSelectionList(page.remainingDispOrderList, page.goodsinDetailList, page.selectedGoodsin);
        await ((NavigableElement) page).Navigation.PushAsync((Page) agSelectionList);
        ((InputView) page.txtBarcode).Text = "";
      }
      else
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Ürün bulunamadı", "", "Tamam") ? 1 : 0;
        page.BarcodeFocus();
      }
    }

    private async void MItem_Clicked(object sender, EventArgs e)
    {
      AGWayBill agWayBill = this;
      ScrollView scrView = new ScrollView();
      pIOGetRemainingDispOrderByItemReturnModel sItem = (pIOGetRemainingDispOrderByItemReturnModel) (sender as MenuItem).CommandParameter;
      if (sItem == null)
        return;
      agWayBill.CreatedDynamicGrid(agWayBill.remainingDispOrderList.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x => x.SelectionCode == sItem.SelectionCode)).ToList<pIOGetRemainingDispOrderByItemReturnModel>(), scrView);
      ((View) scrView).HorizontalOptions = LayoutOptions.FillAndExpand;
      ((View) scrView).VerticalOptions = LayoutOptions.FillAndExpand;
      StackLayout stck = new StackLayout();
      ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) scrView);
      SelectItem selectItem = new SelectItem(stck, "Miktarlar");
      await ((NavigableElement) agWayBill).Navigation.PushAsync((Page) selectItem);
    }

    private void CreatedDynamicGrid(
      List<pIOGetRemainingDispOrderByItemReturnModel> products,
      ScrollView scrView)
    {
      new ScrollView().Orientation = (ScrollOrientation) 1;
      Grid grid = new Grid();
      ((View) grid).HorizontalOptions = LayoutOptions.FillAndExpand;
      ((VisualElement) grid).BackgroundColor = Color.Black;
      grid.ColumnSpacing = 1.0;
      grid.RowSpacing = 1.0;
      IEnumerable<IGrouping<string, pIOGetRemainingDispOrderByItemReturnModel>> groupings = products.GroupBy<pIOGetRemainingDispOrderByItemReturnModel, string>((Func<pIOGetRemainingDispOrderByItemReturnModel, string>) (x => x.ItemDim1Code));
      foreach (IGrouping<string, pIOGetRemainingDispOrderByItemReturnModel> grouping in groupings)
      {
        IGrouping<string, pIOGetRemainingDispOrderByItemReturnModel> column = grouping;
        products.Add(new pIOGetRemainingDispOrderByItemReturnModel()
        {
          CustomerCode = "TOPLAM",
          CustomerDesc = "",
          ItemDim1Code = column.Key,
          ColorCode = "",
          Qty = Convert.ToDouble(products.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x => x.ItemDim1Code == column.Key)).Sum<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, int>) (x => Convert.ToInt32(x.Qty))))
        });
      }
      for (int index = 0; index < groupings.Count<IGrouping<string, pIOGetRemainingDispOrderByItemReturnModel>>() + 2; ++index)
        ((DefinitionCollection<ColumnDefinition>) grid.ColumnDefinitions).Add(new ColumnDefinition()
        {
          Width = GridLength.Star
        });
      List<pIOGetRemainingDispOrderByItemReturnModel> list1 = products.GroupBy(c => new
      {
        CustomerCode = c.CustomerCode,
        CustomerDesc = c.CustomerDesc,
        ColorCode = c.ColorCode,
        Asorti = c.Asorti,
        ColorDescription = c.ColorDescription
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType4<string, string, string, string, string>, pIOGetRemainingDispOrderByItemReturnModel>, pIOGetRemainingDispOrderByItemReturnModel>(gcs => new pIOGetRemainingDispOrderByItemReturnModel()
      {
        CustomerCode = gcs.Key.CustomerCode,
        CustomerDesc = gcs.Key.CustomerDesc,
        ColorCode = gcs.Key.ColorCode,
        Asorti = gcs.Key.Asorti,
        ColorDescription = gcs.Key.ColorDescription
      }).ToList<pIOGetRemainingDispOrderByItemReturnModel>();
      foreach (pIOGetRemainingDispOrderByItemReturnModel byItemReturnModel in list1)
        ((DefinitionCollection<RowDefinition>) grid.RowDefinitions).Add(new RowDefinition()
        {
          Height = GridLength.Star
        });
      this.AddHeaderColumn(grid, groupings);
      int row = 1;
      foreach (IGrouping<string, pIOGetRemainingDispOrderByItemReturnModel> grouping1 in list1.GroupBy<pIOGetRemainingDispOrderByItemReturnModel, string>((Func<pIOGetRemainingDispOrderByItemReturnModel, string>) (x => x.CustomerCode)))
      {
        IGrouping<string, pIOGetRemainingDispOrderByItemReturnModel> item = grouping1;
        List<pIOGetRemainingDispOrderByItemReturnModel> list2 = list1.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x => x.CustomerCode == item.Key)).ToList<pIOGetRemainingDispOrderByItemReturnModel>();
        string text1 = item.Key;
        if (!string.IsNullOrEmpty(list2[0].CustomerDesc))
          text1 = text1 + "-" + list2[0].CustomerDesc;
        int col1 = 0;
        int rowSpan = list2.Count<pIOGetRemainingDispOrderByItemReturnModel>();
        this.AddLabel(grid, text1, col1, row, rowSpan: rowSpan);
        int col2 = 1;
        foreach (pIOGetRemainingDispOrderByItemReturnModel byItemReturnModel in list1.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x => x.CustomerCode == item.Key)))
        {
          pIOGetRemainingDispOrderByItemReturnModel storeColor = byItemReturnModel;
          this.AddLabel(grid, storeColor.Asorti, col2, row);
          int col3 = col2 + 1;
          this.AddLabel(grid, storeColor.ColorDescription + "-" + storeColor.ColorCode, col3, row);
          int col4 = col3 + 1;
          foreach (IGrouping<string, pIOGetRemainingDispOrderByItemReturnModel> grouping2 in groupings)
          {
            IGrouping<string, pIOGetRemainingDispOrderByItemReturnModel> c = grouping2;
            string text2 = Convert.ToString(products.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x => x.CustomerCode == item.Key && x.ColorCode == storeColor.ColorCode && x.ItemDim1Code == c.Key)).Sum<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, int>) (x => Convert.ToInt32(x.Qty))));
            this.AddLabel(grid, text2, col4, row);
            ++col4;
          }
          ++row;
          col2 = 1;
        }
      }
      scrView.Content = (View) grid;
    }

    private void AddHeaderColumn(
      Grid grid,
      IEnumerable<IGrouping<string, pIOGetRemainingDispOrderByItemReturnModel>> columnList)
    {
      int col1 = 0;
      this.AddLabel(grid, "Mağaza", col1, 0, true);
      int col2 = col1 + 1;
      this.AddLabel(grid, "Asorti", col2, 0, true);
      int col3 = col2 + 1;
      this.AddLabel(grid, "Renk", col3, 0, true);
      int col4 = col3 + 1;
      foreach (IGrouping<string, pIOGetRemainingDispOrderByItemReturnModel> column in columnList)
      {
        this.AddLabel(grid, column.Key, col4, 0, true);
        ++col4;
      }
    }

    private void AddLabel(Grid grid, string text, int col, int row, bool bold = false, int rowSpan = 0)
    {
      Label label = new Label();
      ((VisualElement) label).BackgroundColor = Color.White;
      label.HorizontalTextAlignment = (TextAlignment) 1;
      label.VerticalTextAlignment = (TextAlignment) 1;
      label.Text = text;
      label.TextColor = Color.Black;
      if (bold)
        label.FontAttributes = (FontAttributes) 1;
      grid.Children.Add((View) label, col, row);
      if (rowSpan <= 1)
        return;
      Grid.SetRowSpan((BindableObject) label, rowSpan);
    }

    private void BarcodeFocus()
    {
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode).Focus();
    }

    private async void LstRemaining_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      AGWayBill agWayBill = this;
      pIOGetRemainingDispOrderByItemReturnModel item = (pIOGetRemainingDispOrderByItemReturnModel) e.SelectedItem;
      AGWayBillAlc alc = new AGWayBillAlc(agWayBill.remainingDispOrderList.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x => x.SelectionCode == item.SelectionCode)).ToList<pIOGetRemainingDispOrderByItemReturnModel>(), agWayBill.goodsinDetailList, agWayBill.selectedGoodsin);
      Page page = await ((NavigableElement) agWayBill).Navigation.PopAsync();
      await ((NavigableElement) agWayBill).Navigation.PushAsync((Page) alc);
      alc = (AGWayBillAlc) null;
    }

    private void SearchBarRemaining_TextChanged(object sender, TextChangedEventArgs e)
    {
      SearchBar searchBar = (SearchBar) sender;
      string text = ((InputView) searchBar).Text.ToLower();
      if (string.IsNullOrEmpty(((InputView) searchBar).Text))
        ((ItemsView<Cell>) this.lstRemaining).ItemsSource = (IEnumerable) this.remainingDispOrderList;
      else
        ((ItemsView<Cell>) this.lstGoodsIn).ItemsSource = (IEnumerable) this.remainingDispOrderList.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x => x.CustomerCode.ToLower().Contains(text) || x.CustomerDesc.ToLower().Contains(text) || x.DispOrderNumber.ToLower().Contains(text))).ToList<pIOGetRemainingDispOrderByItemReturnModel>();
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (AGWayBill).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/AGWayBill.xaml",
        Instance = (object) this
      }))
        this.__InitComponentRuntime();
      else if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(((object) this).GetType()) != null)
      {
        this.__InitComponentRuntime();
      }
      else
      {
        DatePicker datePicker = new DatePicker();
        StackLayout stackLayout1 = new StackLayout();
        Entry entry = new Entry();
        StackLayout stackLayout2 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry = new SoftkeyboardDisabledEntry();
        StackLayout stackLayout3 = new StackLayout();
        BindingExtension bindingExtension = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout4 = new StackLayout();
        AGWayBill agWayBill;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (agWayBill = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) agWayBill, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("dtWayBill", (object) datePicker);
        if (((Element) datePicker).StyleId == null)
          ((Element) datePicker).StyleId = "dtWayBill";
        ((INameScope) nameScope).RegisterName("txtWayBillNumber", (object) entry);
        if (((Element) entry).StyleId == null)
          ((Element) entry).StyleId = "txtWayBillNumber";
        ((INameScope) nameScope).RegisterName("stckBarcode", (object) stackLayout3);
        if (((Element) stackLayout3).StyleId == null)
          ((Element) stackLayout3).StyleId = "stckBarcode";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry);
        if (((Element) softkeyboardDisabledEntry).StyleId == null)
          ((Element) softkeyboardDisabledEntry).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("lstGoodsInDetails", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstGoodsInDetails";
        this.stckForm = stackLayout4;
        this.dtWayBill = datePicker;
        this.txtWayBillNumber = entry;
        this.stckBarcode = stackLayout3;
        this.txtBarcode = softkeyboardDisabledEntry;
        this.lstGoodsInDetails = listView;
        ((BindableObject) stackLayout4).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) datePicker).SetValue(DatePicker.FormatProperty, (object) "dd.MM.yyyy");
        ((BindableObject) datePicker).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) datePicker);
        VisualDiagnostics.RegisterSourceInfo((object) datePicker, new Uri("Views\\AGWayBill.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\AGWayBill.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 14);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry).SetValue(Entry.PlaceholderProperty, (object) "İrsaliye Seçiniz");
        ((VisualElement) entry).Focused += new EventHandler<FocusEventArgs>(agWayBill.txtWayBillNumber_Focused);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) entry);
        VisualDiagnostics.RegisterSourceInfo((object) entry, new Uri("Views\\AGWayBill.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\AGWayBill.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 14);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(Entry.PlaceholderProperty, (object) "Dağıtım İçin Barkod No Okutunuz");
        softkeyboardDisabledEntry.Completed += new EventHandler(agWayBill.txtBarcode_Completed);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) softkeyboardDisabledEntry);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry, new Uri("Views\\AGWayBill.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 18, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\AGWayBill.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 14);
        bindingExtension.Path = ".";
        BindingBase bindingBase = ((IMarkupExtension<BindingBase>) bindingExtension).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
        ((BindableObject) listView).SetValue(ListView.RowHeightProperty, (object) 90);
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 5000.0);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AGWayBill.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_5 xamlCdataTemplate5 = new AGWayBill.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_5();
        object[] objArray = new object[0 + 4];
        objArray[0] = (object) dataTemplate1;
        objArray[1] = (object) listView;
        objArray[2] = (object) stackLayout4;
        objArray[3] = (object) agWayBill;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate5.parentValues = objArray;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate5.root = agWayBill;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate5.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\AGWayBill.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 22, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\AGWayBill.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 14);
        ((BindableObject) agWayBill).SetValue(ContentPage.ContentProperty, (object) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\AGWayBill.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 10);
        VisualDiagnostics.RegisterSourceInfo((object) agWayBill, new Uri("Views\\AGWayBill.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Extensions.LoadFromXaml<AGWayBill>(this, typeof (AGWayBill));
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.dtWayBill = NameScopeExtensions.FindByName<DatePicker>((Element) this, "dtWayBill");
      this.txtWayBillNumber = NameScopeExtensions.FindByName<Entry>((Element) this, "txtWayBillNumber");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.lstGoodsInDetails = NameScopeExtensions.FindByName<ListView>((Element) this, "lstGoodsInDetails");
    }
  }
}
