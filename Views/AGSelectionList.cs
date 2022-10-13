// Decompiled with JetBrains decompiler
// Type: Shelf.Views.AGSelectionList
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

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

namespace Shelf.Views
{
  [XamlCompilation]
  [XamlFilePath("Views\\AGSelectionList.xaml")]
  public class AGSelectionList : ContentPage
  {
    private List<pIOGetRemainingDispOrderByItemReturnModel> remainingDispOrderList;
    private List<pIOGetRemainingDispOrderByItemReturnModel> remainingDispOrderListGroup;
    private List<pIOGetErpGoodsInDetailReturnModel> goodsInDetailList;
    private pIOGetErpGoodsInReturnModel selectGoodsIn;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstRemaining;

    public AGSelectionList(
      List<pIOGetRemainingDispOrderByItemReturnModel> list,
      List<pIOGetErpGoodsInDetailReturnModel> fGoodsInDetailList,
      pIOGetErpGoodsInReturnModel fSelectGoodsIn)
    {
      this.InitializeComponent();
      this.remainingDispOrderList = list;
      this.goodsInDetailList = fGoodsInDetailList;
      this.selectGoodsIn = fSelectGoodsIn;
      this.LoadData();
      ((Page) this).Title = "Sipariş Seçiniz";
    }

    private void LoadData(string text = "")
    {
      this.remainingDispOrderListGroup = this.remainingDispOrderList.GroupBy(c => new
      {
        SelectionCode = c.SelectionCode,
        SelectionDesc = c.SelectionDesc
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType0<string, string>, pIOGetRemainingDispOrderByItemReturnModel>, pIOGetRemainingDispOrderByItemReturnModel>(gcs => new pIOGetRemainingDispOrderByItemReturnModel()
      {
        SelectionCode = gcs.Key.SelectionCode,
        SelectionDesc = gcs.Key.SelectionDesc,
        Qty = Convert.ToDouble((object) gcs.Sum<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, double?>) (x =>
        {
          double qty = x.Qty;
          int? alcQty = x.AlcQty;
          double? nullable = alcQty.HasValue ? new double?((double) alcQty.GetValueOrDefault()) : new double?();
          return !nullable.HasValue ? new double?() : new double?(qty - nullable.GetValueOrDefault());
        })))
      }).ToList<pIOGetRemainingDispOrderByItemReturnModel>();
      if (!string.IsNullOrEmpty(text))
        this.remainingDispOrderListGroup = this.remainingDispOrderListGroup.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x => x.SelectionCode.ToLower().Contains(text) || x.SelectionDesc.ToLower().Contains(text))).ToList<pIOGetRemainingDispOrderByItemReturnModel>();
      ((ItemsView<Cell>) this.lstRemaining).ItemsSource = (IEnumerable) this.remainingDispOrderListGroup;
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e) => this.LoadData(((InputView) sender).Text.ToLower());

    private async void lstRemaining_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      AGSelectionList agSelectionList = this;
      pIOGetRemainingDispOrderByItemReturnModel item = (pIOGetRemainingDispOrderByItemReturnModel) e.SelectedItem;
      if (item == null)
        return;
      List<pIOGetRemainingDispOrderByItemReturnModel> list = agSelectionList.remainingDispOrderList.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x => x.SelectionCode == item.SelectionCode)).ToList<pIOGetRemainingDispOrderByItemReturnModel>();
      agSelectionList.lstRemaining.SelectedItem = (object) null;
      List<pIOGetErpGoodsInDetailReturnModel> goodsInDetailList = agSelectionList.goodsInDetailList;
      pIOGetErpGoodsInReturnModel selectGoodsIn = agSelectionList.selectGoodsIn;
      AGWayBillAlc agWayBillAlc = new AGWayBillAlc(list, goodsInDetailList, selectGoodsIn);
      await ((NavigableElement) agSelectionList).Navigation.PushAsync((Page) agWayBillAlc);
    }

    private async void cmDetail_Clicked(object sender, SelectedItemChangedEventArgs e)
    {
      AGSelectionList agSelectionList = this;
      ScrollView scrView = new ScrollView();
      pIOGetRemainingDispOrderByItemReturnModel sItem = (pIOGetRemainingDispOrderByItemReturnModel) (sender as MenuItem).CommandParameter;
      if (sItem == null)
        return;
      agSelectionList.CreatedDynamicGrid(agSelectionList.remainingDispOrderList.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x => x.SelectionCode == sItem.SelectionCode)).ToList<pIOGetRemainingDispOrderByItemReturnModel>(), scrView);
      ((View) scrView).HorizontalOptions = LayoutOptions.FillAndExpand;
      ((View) scrView).VerticalOptions = LayoutOptions.FillAndExpand;
      StackLayout stck = new StackLayout();
      ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) scrView);
      SelectItem selectItem = new SelectItem(stck, "Miktarlar");
      await ((NavigableElement) agSelectionList).Navigation.PushAsync((Page) selectItem);
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
        SubCurrAccID = c.SubCurrAccID,
        ColorDescription = c.ColorDescription
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType1<string, string, string, string, Guid?, string>, pIOGetRemainingDispOrderByItemReturnModel>, pIOGetRemainingDispOrderByItemReturnModel>(gcs => new pIOGetRemainingDispOrderByItemReturnModel()
      {
        CustomerCode = gcs.Key.CustomerCode,
        CustomerDesc = gcs.Key.CustomerDesc,
        ColorCode = gcs.Key.ColorCode,
        Asorti = gcs.Key.Asorti,
        SubCurrAccID = gcs.Key.SubCurrAccID,
        ColorDescription = gcs.Key.ColorDescription
      }).ToList<pIOGetRemainingDispOrderByItemReturnModel>();
      foreach (pIOGetRemainingDispOrderByItemReturnModel byItemReturnModel in list1)
        ((DefinitionCollection<RowDefinition>) grid.RowDefinitions).Add(new RowDefinition()
        {
          Height = GridLength.Star
        });
      this.AddHeaderColumn(grid, groupings);
      int row = 1;
      foreach (pIOGetRemainingDispOrderByItemReturnModel byItemReturnModel1 in products.GroupBy(c => new
      {
        CustomerCode = c.CustomerCode,
        CustomerDesc = c.CustomerDesc,
        SubCurrAccID = c.SubCurrAccID
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType2<string, string, Guid?>, pIOGetRemainingDispOrderByItemReturnModel>, pIOGetRemainingDispOrderByItemReturnModel>(gcs => new pIOGetRemainingDispOrderByItemReturnModel()
      {
        CustomerCode = gcs.Key.CustomerCode,
        SubCurrAccID = gcs.Key.SubCurrAccID,
        CustomerDesc = gcs.Key.CustomerDesc
      }).ToList<pIOGetRemainingDispOrderByItemReturnModel>())
      {
        pIOGetRemainingDispOrderByItemReturnModel item = byItemReturnModel1;
        string key = item.CustomerCode;
        List<pIOGetRemainingDispOrderByItemReturnModel> list2 = list1.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x =>
        {
          if (!(x.CustomerCode == key))
            return false;
          Guid? subCurrAccId1 = x.SubCurrAccID;
          Guid? subCurrAccId2 = item.SubCurrAccID;
          if (subCurrAccId1.HasValue != subCurrAccId2.HasValue)
            return false;
          return !subCurrAccId1.HasValue || subCurrAccId1.GetValueOrDefault() == subCurrAccId2.GetValueOrDefault();
        })).ToList<pIOGetRemainingDispOrderByItemReturnModel>();
        string text1 = key;
        if (!string.IsNullOrEmpty(list2[0].CustomerDesc))
          text1 = text1 + "-" + list2[0].CustomerDesc;
        int col1 = 0;
        int rowSpan = list2.Count<pIOGetRemainingDispOrderByItemReturnModel>();
        this.AddLabel(grid, text1, col1, row, rowSpan: rowSpan);
        int col2 = 1;
        foreach (pIOGetRemainingDispOrderByItemReturnModel byItemReturnModel2 in list2)
        {
          pIOGetRemainingDispOrderByItemReturnModel storeColor = byItemReturnModel2;
          this.AddLabel(grid, storeColor.Asorti, col2, row);
          int col3 = col2 + 1;
          this.AddLabel(grid, storeColor.ColorDescription + "-" + storeColor.ColorCode, col3, row);
          int col4 = col3 + 1;
          foreach (IGrouping<string, pIOGetRemainingDispOrderByItemReturnModel> grouping in groupings)
          {
            IGrouping<string, pIOGetRemainingDispOrderByItemReturnModel> c = grouping;
            string text2 = Convert.ToString(products.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x =>
            {
              if (x.CustomerCode == key)
              {
                Guid? subCurrAccId3 = x.SubCurrAccID;
                Guid? subCurrAccId4 = item.SubCurrAccID;
                if ((subCurrAccId3.HasValue == subCurrAccId4.HasValue ? (subCurrAccId3.HasValue ? (subCurrAccId3.GetValueOrDefault() == subCurrAccId4.GetValueOrDefault() ? 1 : 0) : 1) : 0) != 0 && x.ColorCode == storeColor.ColorCode)
                  return x.ItemDim1Code == c.Key;
              }
              return false;
            })).Sum<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, int>) (x => Convert.ToInt32(x.Qty))));
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

    private async void cmDetail_Clicked_1(object sender, EventArgs e)
    {
      AGSelectionList agSelectionList = this;
      ScrollView scrView = new ScrollView();
      pIOGetRemainingDispOrderByItemReturnModel sItem = (pIOGetRemainingDispOrderByItemReturnModel) (sender as MenuItem).CommandParameter;
      if (sItem == null)
        return;
      agSelectionList.CreatedDynamicGrid(agSelectionList.remainingDispOrderList.Where<pIOGetRemainingDispOrderByItemReturnModel>((Func<pIOGetRemainingDispOrderByItemReturnModel, bool>) (x => x.SelectionCode == sItem.SelectionCode)).ToList<pIOGetRemainingDispOrderByItemReturnModel>(), scrView);
      ((View) scrView).HorizontalOptions = LayoutOptions.FillAndExpand;
      ((View) scrView).VerticalOptions = LayoutOptions.FillAndExpand;
      StackLayout stck = new StackLayout();
      ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) scrView);
      SelectItem selectItem = new SelectItem(stck, "Miktarlar");
      await ((NavigableElement) agSelectionList).Navigation.PushAsync((Page) selectItem);
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (AGSelectionList).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/AGSelectionList.xaml",
        Instance = (object) this
      }))
        this.__InitComponentRuntime();
      else if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(((object) this).GetType()) != null)
      {
        this.__InitComponentRuntime();
      }
      else
      {
        SearchBar searchBar1 = new SearchBar();
        BindingExtension bindingExtension = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout = new StackLayout();
        AGSelectionList agSelectionList;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (agSelectionList = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) agSelectionList, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("lstRemaining", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstRemaining";
        this.lstRemaining = listView;
        ((BindableObject) searchBar1).SetValue(SearchBar.PlaceholderProperty, (object) "Ara");
        ((BindableObject) searchBar1).SetValue(SearchBar.PlaceholderColorProperty, (object) Color.Orange);
        ((BindableObject) searchBar1).SetValue(SearchBar.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        SearchBar searchBar2 = searchBar1;
        BindableProperty fontSizeProperty = SearchBar.FontSizeProperty;
        FontSizeConverter fontSizeConverter = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 3];
        objArray1[0] = (object) searchBar1;
        objArray1[1] = (object) stackLayout;
        objArray1[2] = (object) agSelectionList;
        SimpleValueTargetProvider valueTargetProvider;
        object obj1 = (object) (valueTargetProvider = new SimpleValueTargetProvider(objArray1, (object) SearchBar.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider.Add(type1, (object) valueTargetProvider);
        xamlServiceProvider.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver = new XmlNamespaceResolver();
        namespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        XamlTypeResolver xamlTypeResolver = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver, typeof (AGSelectionList).GetTypeInfo().Assembly);
        xamlServiceProvider.Add(type2, (object) xamlTypeResolver);
        xamlServiceProvider.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(11, 24)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter).ConvertFromInvariantString("Medium", (IServiceProvider) xamlServiceProvider);
        ((BindableObject) searchBar2).SetValue(fontSizeProperty, obj2);
        ((BindableObject) searchBar1).SetValue(SearchBar.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
        ((InputView) searchBar1).TextChanged += new EventHandler<TextChangedEventArgs>(agSelectionList.SearchBar_TextChanged);
        ((ICollection<View>) ((Layout<View>) stackLayout).Children).Add((View) searchBar1);
        VisualDiagnostics.RegisterSourceInfo((object) searchBar1, new Uri("Views\\AGSelectionList.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 14);
        bindingExtension.Path = ".";
        BindingBase bindingBase = ((IMarkupExtension<BindingBase>) bindingExtension).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
        listView.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(agSelectionList.lstRemaining_ItemSelected);
        ((BindableObject) listView).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AGSelectionList.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_4 xamlCdataTemplate4 = new AGSelectionList.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_4();
        object[] objArray2 = new object[0 + 4];
        objArray2[0] = (object) dataTemplate1;
        objArray2[1] = (object) listView;
        objArray2[2] = (object) stackLayout;
        objArray2[3] = (object) agSelectionList;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate4.parentValues = objArray2;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate4.root = agSelectionList;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate4.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\AGSelectionList.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\AGSelectionList.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 14);
        ((BindableObject) agSelectionList).SetValue(ContentPage.ContentProperty, (object) stackLayout);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout, new Uri("Views\\AGSelectionList.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 9, 10);
        VisualDiagnostics.RegisterSourceInfo((object) agSelectionList, new Uri("Views\\AGSelectionList.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Extensions.LoadFromXaml<AGSelectionList>(this, typeof (AGSelectionList));
      this.lstRemaining = NameScopeExtensions.FindByName<ListView>((Element) this, "lstRemaining");
    }
  }
}
