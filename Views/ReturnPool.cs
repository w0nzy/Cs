// Decompiled with JetBrains decompiler
// Type: Shelf.Views.ReturnPool
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
  [XamlFilePath("Views\\ReturnPool.xaml")]
  public class ReturnPool : ContentPage
  {
    public ztIOShelf recommendedShelf;
    public pIOReturnPoolRecommendedShelvesReturnModel selectedItem;
    public List<pIOReturnPoolRecommendedShelvesReturnModel> recommendedShelfList;
    public List<ShelfTransaction> transList;
    private ToolbarItem tItem;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage ret;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnClear;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckProcess;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Label lblDescription;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtRecommendedShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtShelf;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtQty;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckDetails;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfDetail;

    public string selectBarcode { get; set; }

    public Color ButtonColor => Color.FromRgb(52, 203, 201);

    public Color TextColor => Color.White;

    public ReturnPool()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Havuzdan İade";
      this.transList = new List<ShelfTransaction>();
      ToolbarItem toolbarItem = new ToolbarItem();
      ((MenuItem) toolbarItem).Text = "";
      this.tItem = toolbarItem;
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(this.tItem);
      GlobalMob.AddBarcodeLongPress(this.txtBarcode);
      GlobalMob.AddShelfBarcodeLongPress((Xamarin.Forms.Entry) this.txtShelf);
      ((ICollection<Effect>) ((Element) this.txtRecommendedShelf).Effects).Add((Effect) new LongPressedEffect());
      LongPressedEffect.SetCommand((BindableObject) this.txtRecommendedShelf, this.LongPress);
      this.BarcodeFocus();
    }

    private ICommand LongPress => (ICommand) new Command((Action) (async () =>
    {
      ((InputView) this.txtBarcode).Text = this.selectBarcode;
      this.txtBarcode_Completed_1((object) this.txtBarcode, (EventArgs) null);
    }));

    private void Lst_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      ((NavigableElement) this).Navigation.PopAsync();
      this.selectedItem = (pIOReturnPoolRecommendedShelvesReturnModel) e.SelectedItem;
      ((InputView) this.txtRecommendedShelf).Text = this.selectedItem.ShelfCode;
      ((VisualElement) this.stckProcess).IsVisible = true;
      this.SetInfo();
      this.txtRecommendedShelf_Completed((object) this.txtRecommendedShelf, (EventArgs) null);
    }

    private void ShelfCodeFocus() => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(150);
      ((InputView) this.txtShelf).Text = "";
      ((VisualElement) this.txtShelf).Focus();
    }));

    private void BarcodeFocus() => Device.BeginInvokeOnMainThread((Action) (async () =>
    {
      await Task.Delay(150);
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode).Focus();
    }));

    private void RefreshData()
    {
      this.transList = this.transList.OrderByDescending<ShelfTransaction, bool>((Func<ShelfTransaction, bool>) (x => x.LastReadBarcode)).ToList<ShelfTransaction>();
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) this.transList;
      int num = this.transList.Sum<ShelfTransaction>((Func<ShelfTransaction, int>) (x => x.Qty));
      ((MenuItem) this.tItem).Text = num > 0 ? Convert.ToString(num) : "";
    }

    private void btnClear_Clicked(object sender, EventArgs e)
    {
      this.selectBarcode = "";
      ((InputView) this.txtBarcode).Text = "";
      this.recommendedShelf = (ztIOShelf) null;
      ((MenuItem) this.tItem).Text = "";
      ((InputView) this.txtQty).Text = "1";
      this.recommendedShelfList = new List<pIOReturnPoolRecommendedShelvesReturnModel>();
      this.RefreshData();
      ((VisualElement) this.stckProcess).IsVisible = false;
      this.BarcodeFocus();
    }

    private async void txtBarcode_Completed_1(object sender, EventArgs e)
    {
      ReturnPool page = this;
      string text = ((InputView) page.txtBarcode).Text;
      page.selectBarcode = text;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetReturnPoolRecommendedShelves?barcode={0}", (object) text), (ContentPage) page);
      if (!returnModel.Success)
        return;
      page.recommendedShelfList = GlobalMob.JsonDeserialize<List<pIOReturnPoolRecommendedShelvesReturnModel>>(returnModel.Result);
      if (page.recommendedShelfList == null || page.recommendedShelfList.Count <= 0)
        return;
      if (page.recommendedShelfList.Count == 1)
      {
        page.selectedItem = page.recommendedShelfList[0];
        page.SetInfo();
        ((VisualElement) page.stckProcess).IsVisible = true;
        page.txtRecommendedShelf_Completed((object) page.txtRecommendedShelf, (EventArgs) null);
      }
      else
      {
        ListView shelfListview = GlobalMob.GetShelfListview("ShelfCode,MainShelfCode,DisplayDescription");
        ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) page.recommendedShelfList;
        shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(page.Lst_ItemSelected);
        SelectItem selectItem = new SelectItem(shelfListview, "Raf Seçiniz");
        await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
      }
    }

    private void SetInfo()
    {
      ((InputView) this.txtRecommendedShelf).Text = this.selectedItem.ShelfCode;
      this.lblDescription.Text = this.selectedItem.ItemDescription;
      Label lblDescription1 = this.lblDescription;
      lblDescription1.Text = lblDescription1.Text + "\n" + this.selectedItem.ColorDescription;
      Label lblDescription2 = this.lblDescription;
      lblDescription2.Text = lblDescription2.Text + "\n" + this.selectedItem.MainShelfCode;
    }

    private async void txtShelf_Completed(object sender, EventArgs e)
    {
      ReturnPool page = this;
      int qty = page.GetQty();
      string text = ((InputView) page.txtShelf).Text;
      if (page.recommendedShelf != null && page.recommendedShelf.Code != text && page.recommendedShelf.Description != text && page.recommendedShelf.MainShelfCode != text && page.recommendedShelf.MainShelfDescription != text)
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Lütfen önerilen rafı okutunuz.\nÖnerilen Raf:" + page.recommendedShelf.Code, "", "Tamam") ? 1 : 0;
        page.ShelfCodeFocus();
      }
      else
      {
        ShelfTransaction shelfTransaction1 = new ShelfTransaction();
        shelfTransaction1.Barcode = page.selectBarcode;
        shelfTransaction1.ColorCode = page.selectedItem.ColorCode;
        shelfTransaction1.ItemCode = page.selectedItem.ItemCode;
        shelfTransaction1.ItemDim1Code = page.selectedItem.ItemDim1Code;
        shelfTransaction1.ItemDim2Code = page.selectedItem.ItemDim2Code;
        shelfTransaction1.ItemDim3Code = page.selectedItem.ItemDim3Code;
        shelfTransaction1.ItemTypeCode = page.selectedItem.ItemTypeCode;
        shelfTransaction1.ProcessTypeID = 1;
        shelfTransaction1.ShelfID = page.recommendedShelf.ShelfID;
        shelfTransaction1.Qty = qty;
        shelfTransaction1.TransTypeID = 21;
        shelfTransaction1.UserName = GlobalMob.User.UserName;
        shelfTransaction1.WareHouseCode = page.recommendedShelf.WarehouseCode;
        Dictionary<string, string> paramList = new Dictionary<string, string>();
        string str = JsonConvert.SerializeObject((object) shelfTransaction1);
        paramList.Add("json", str);
        ReturnModel result = GlobalMob.PostJson("ShelfInsert", paramList, (ContentPage) page).Result;
        if (!result.Success)
          return;
        ShelfTransaction transResult = JsonConvert.DeserializeObject<ShelfTransaction>(result.Result);
        if (transResult == null)
          return;
        page.transList.Select<ShelfTransaction, ShelfTransaction>((Func<ShelfTransaction, ShelfTransaction>) (c =>
        {
          c.LastReadBarcode = false;
          return c;
        })).ToList<ShelfTransaction>();
        transResult.LastReadBarcode = true;
        transResult.Barcode = page.selectBarcode;
        string code = page.recommendedShelf.MainShelfCode + "-" + page.recommendedShelf.Code;
        transResult.ShelfCode = code;
        ShelfTransaction shelfTransaction2 = page.transList.Where<ShelfTransaction>((Func<ShelfTransaction, bool>) (x => x.Barcode == transResult.Barcode && x.ShelfCode == code)).FirstOrDefault<ShelfTransaction>();
        if (shelfTransaction2 != null)
          shelfTransaction2.Qty += qty;
        else
          page.transList.Add(transResult);
        page.RefreshData();
        page.btnClear_Clicked((object) null, (EventArgs) null);
      }
    }

    private int GetQty()
    {
      try
      {
        return Convert.ToInt32(((InputView) this.txtQty).Text);
      }
      catch (Exception ex)
      {
        return 1;
      }
    }

    private async void txtRecommendedShelf_Completed(object sender, EventArgs e)
    {
      ReturnPool page = this;
      ReturnModel shelf = GlobalMob.GetShelf(((InputView) page.txtRecommendedShelf).Text, (ContentPage) page);
      if (!shelf.Success)
        return;
      page.recommendedShelf = JsonConvert.DeserializeObject<ztIOShelf>(shelf.Result);
      if (page.recommendedShelf != null)
      {
        ((InputView) page.txtRecommendedShelf).Text = page.recommendedShelf.Code;
        ((VisualElement) page.stckShelf).IsVisible = true;
        page.ShelfCodeFocus();
      }
      else
      {
        int num = await ((Page) page).DisplayAlert("Bilgi", "Raf bulunamadı", "", "Tamam") ? 1 : 0;
      }
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (ReturnPool).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/ReturnPool.xaml",
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
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension1 = new BindingExtension();
        Button button1 = new Button();
        StackLayout stackLayout1 = new StackLayout();
        Label label1 = new Label();
        StackLayout stackLayout2 = new StackLayout();
        Xamarin.Forms.Entry entry1 = new Xamarin.Forms.Entry();
        StackLayout stackLayout3 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry2 = new SoftkeyboardDisabledEntry();
        Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
        StackLayout stackLayout4 = new StackLayout();
        StackLayout stackLayout5 = new StackLayout();
        BindingExtension bindingExtension2 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout6 = new StackLayout();
        StackLayout stackLayout7 = new StackLayout();
        ReturnPool returnPool;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (returnPool = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) returnPool, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("ret", (object) returnPool);
        if (((Element) returnPool).StyleId == null)
          ((Element) returnPool).StyleId = "ret";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout7);
        if (((Element) stackLayout7).StyleId == null)
          ((Element) stackLayout7).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("btnClear", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnClear";
        ((INameScope) nameScope).RegisterName("stckProcess", (object) stackLayout5);
        if (((Element) stackLayout5).StyleId == null)
          ((Element) stackLayout5).StyleId = "stckProcess";
        ((INameScope) nameScope).RegisterName("lblDescription", (object) label1);
        if (((Element) label1).StyleId == null)
          ((Element) label1).StyleId = "lblDescription";
        ((INameScope) nameScope).RegisterName("txtRecommendedShelf", (object) entry1);
        if (((Element) entry1).StyleId == null)
          ((Element) entry1).StyleId = "txtRecommendedShelf";
        ((INameScope) nameScope).RegisterName("stckShelf", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckShelf";
        ((INameScope) nameScope).RegisterName("txtShelf", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtShelf";
        ((INameScope) nameScope).RegisterName("txtQty", (object) entry2);
        if (((Element) entry2).StyleId == null)
          ((Element) entry2).StyleId = "txtQty";
        ((INameScope) nameScope).RegisterName("stckDetails", (object) stackLayout6);
        if (((Element) stackLayout6).StyleId == null)
          ((Element) stackLayout6).StyleId = "stckDetails";
        ((INameScope) nameScope).RegisterName("lstShelfDetail", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstShelfDetail";
        this.ret = (ContentPage) returnPool;
        this.stckForm = stackLayout7;
        this.txtBarcode = softkeyboardDisabledEntry1;
        this.btnClear = button1;
        this.stckProcess = stackLayout5;
        this.lblDescription = label1;
        this.txtRecommendedShelf = entry1;
        this.stckShelf = stackLayout4;
        this.txtShelf = softkeyboardDisabledEntry2;
        this.txtQty = entry2;
        this.stckDetails = stackLayout6;
        this.lstShelfDetail = listView;
        ((BindableObject) stackLayout7).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Ürün Barkodu Okutunuz");
        softkeyboardDisabledEntry1.Completed += new EventHandler(returnPool.txtBarcode_Completed_1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 18);
        button1.Clicked += new EventHandler(returnPool.btnClear_Clicked);
        referenceExtension1.Name = "ret";
        ReferenceExtension referenceExtension2 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 5];
        objArray1[0] = (object) bindingExtension1;
        objArray1[1] = (object) button1;
        objArray1[2] = (object) stackLayout1;
        objArray1[3] = (object) stackLayout7;
        objArray1[4] = (object) returnPool;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (ReturnPool).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(12, 71)));
        object obj2 = ((IMarkupExtension) referenceExtension2).ProvideValue((IServiceProvider) xamlServiceProvider1);
        bindingExtension1.Source = obj2;
        VisualDiagnostics.RegisterSourceInfo(obj2, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 71);
        bindingExtension1.Path = "ButtonColor";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) button1).SetBinding(VisualElement.BackgroundColorProperty, bindingBase1);
        ((BindableObject) button1).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "X");
        ((BindableObject) button1).SetValue(VisualElement.HeightRequestProperty, (object) 40.0);
        Button button2 = button1;
        BindableProperty fontSizeProperty1 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter1 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 4];
        objArray2[0] = (object) button1;
        objArray2[1] = (object) stackLayout1;
        objArray2[2] = (object) stackLayout7;
        objArray2[3] = (object) returnPool;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray2, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver2.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (ReturnPool).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(13, 75)));
        object obj4 = ((IExtendedTypeConverter) fontSizeConverter1).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider2);
        ((BindableObject) button2).SetValue(fontSizeProperty1, obj4);
        ((BindableObject) button1).SetValue(VisualElement.WidthRequestProperty, (object) 40.0);
        ((BindableObject) button1).SetValue(Button.TextColorProperty, (object) Color.White);
        ((BindableObject) button1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 9, 14);
        ((BindableObject) stackLayout5).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) label1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        Label label2 = label1;
        BindableProperty fontSizeProperty2 = Label.FontSizeProperty;
        FontSizeConverter fontSizeConverter2 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 5];
        objArray3[0] = (object) label1;
        objArray3[1] = (object) stackLayout2;
        objArray3[2] = (object) stackLayout5;
        objArray3[3] = (object) stackLayout7;
        objArray3[4] = (object) returnPool;
        SimpleValueTargetProvider valueTargetProvider3;
        object obj5 = (object) (valueTargetProvider3 = new SimpleValueTargetProvider(objArray3, (object) Label.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider3.Add(type5, (object) valueTargetProvider3);
        xamlServiceProvider3.Add(typeof (IReferenceProvider), obj5);
        Type type6 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver3 = new XmlNamespaceResolver();
        namespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver3.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (ReturnPool).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(17, 84)));
        object obj6 = ((IExtendedTypeConverter) fontSizeConverter2).ConvertFromInvariantString("Medium", (IServiceProvider) xamlServiceProvider3);
        ((BindableObject) label2).SetValue(fontSizeProperty2, obj6);
        ((BindableObject) label1).SetValue(VisualElement.HeightRequestProperty, (object) 100.0);
        ((BindableObject) label1).SetValue(VisualElement.WidthRequestProperty, (object) 200.0);
        ((BindableObject) label1).SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) label1).SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) label1).SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) label1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) label1);
        VisualDiagnostics.RegisterSourceInfo((object) label1, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 18);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Önerilen Raf");
        entry1.Completed += new EventHandler(returnPool.txtRecommendedShelf_Completed);
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 22, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 21, 18);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Raf okutunuz.");
        softkeyboardDisabledEntry2.Completed += new EventHandler(returnPool.txtShelf_Completed);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 26, 22);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.TextProperty, (object) "1");
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Miktar");
        ((BindableObject) entry2).SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 25, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 14);
        bindingExtension2.Path = ".";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase2);
        ((BindableObject) listView).SetValue(ListView.RowHeightProperty, (object) 100);
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 5000.0);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ReturnPool.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_24 xamlCdataTemplate24 = new ReturnPool.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_24();
        object[] objArray4 = new object[0 + 5];
        objArray4[0] = (object) dataTemplate1;
        objArray4[1] = (object) listView;
        objArray4[2] = (object) stackLayout6;
        objArray4[3] = (object) stackLayout7;
        objArray4[4] = (object) returnPool;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate24.parentValues = objArray4;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate24.root = returnPool;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate24.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 36, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 33, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 32, 14);
        ((BindableObject) returnPool).SetValue(ContentPage.ContentProperty, (object) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 8, 10);
        VisualDiagnostics.RegisterSourceInfo((object) returnPool, new Uri("Views\\ReturnPool.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Extensions.LoadFromXaml<ReturnPool>(this, typeof (ReturnPool));
      this.ret = NameScopeExtensions.FindByName<ContentPage>((Element) this, "ret");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.btnClear = NameScopeExtensions.FindByName<Button>((Element) this, "btnClear");
      this.stckProcess = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckProcess");
      this.lblDescription = NameScopeExtensions.FindByName<Label>((Element) this, "lblDescription");
      this.txtRecommendedShelf = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtRecommendedShelf");
      this.stckShelf = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckShelf");
      this.txtShelf = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtShelf");
      this.txtQty = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtQty");
      this.stckDetails = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckDetails");
      this.lstShelfDetail = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfDetail");
    }
  }
}
