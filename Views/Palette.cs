// Decompiled with JetBrains decompiler
// Type: Shelf.Views.Palette
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

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
using Xamarin.KeyboardHelper;

namespace Shelf.Views
{
  [XamlCompilation]
  [XamlFilePath("Views\\Palette.xaml")]
  public class Palette : ContentPage
  {
    private List<pIOPaletteDetailReturnModel> detailList;
    private pIOPaletteHeaderReturnModel selectedPaletteHeader;
    private List<pIOPaletteHeaderReturnModel> headerList;
    private ToolbarItem tNewPalette;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage palette;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckContent;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckPaletteOrderList;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtSearch;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ImageButton imgSearch;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckEmptyMessage;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstPalette;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckPackage;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtPalette;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ImageButton imgPrint;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckSuccess;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnSuccess;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtPackage;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstPackage;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Label lblListHeader;

    public Color ButtonColor => Color.FromRgb(142, 81, 152);

    public Color TextColor => Color.White;

    public Palette()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Palet";
      ToolbarItem toolbarItem = new ToolbarItem();
      ((MenuItem) toolbarItem).Text = "Yeni";
      this.tNewPalette = toolbarItem;
      ((MenuItem) this.tNewPalette).Clicked += new EventHandler(this.TNewPalette_Clicked);
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(this.tNewPalette);
      this.detailList = new List<pIOPaletteDetailReturnModel>();
      this.GetPaletteHeader();
    }

    private void GetPaletteHeader()
    {
      ReturnModel returnModel = GlobalMob.PostJson(string.Format(nameof (GetPaletteHeader)), (ContentPage) this);
      if (!returnModel.Success)
        return;
      this.headerList = GlobalMob.JsonDeserialize<List<pIOPaletteHeaderReturnModel>>(returnModel.Result);
      ((ItemsView<Cell>) this.lstPalette).ItemsSource = (IEnumerable) this.headerList;
      this.lstPalette.ItemSelected += (EventHandler<SelectedItemChangedEventArgs>) ((sender, e) =>
      {
        object selectedItem = ((ListView) sender).SelectedItem;
        if (selectedItem == null)
          return;
        this.selectedPaletteHeader = (pIOPaletteHeaderReturnModel) selectedItem;
        ((Page) this).Title = "Palet-" + this.selectedPaletteHeader.PaletteCode;
        ((InputView) this.txtPalette).Text = this.selectedPaletteHeader.PaletteCode;
        ((VisualElement) this.stckPaletteOrderList).IsVisible = false;
        ((VisualElement) this.stckForm).IsVisible = true;
        this.GetShelfDetail();
        ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Clear();
        Device.BeginInvokeOnMainThread((Action) (async () =>
        {
          await Task.Delay(250);
          this.BarcodeFocus();
        }));
      });
    }

    private void GetShelfDetail()
    {
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetPaletteDetail?headerID={0}", (object) this.selectedPaletteHeader.PaletteHeaderID), (ContentPage) this);
      if (!returnModel.Success)
        return;
      this.detailList = GlobalMob.JsonDeserialize<List<pIOPaletteDetailReturnModel>>(returnModel.Result);
      this.RefreshData();
      this.BarcodeFocus();
    }

    private async void TNewPalette_Clicked(object sender, EventArgs e)
    {
      Palette page = this;
      if (!await ((Page) page).DisplayAlert("", "Yeni palet oluşturulacak emin misiniz?", "Evet", "Hayır"))
        return;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("SavePalette?description={0}&userName={1}", (object) "", (object) GlobalMob.User.UserName), (ContentPage) page);
      if (!returnModel.Success)
        return;
      ztIOPaletteHeader ztIoPaletteHeader = GlobalMob.JsonDeserialize<ztIOPaletteHeader>(returnModel.Result);
      page.selectedPaletteHeader = new pIOPaletteHeaderReturnModel()
      {
        CreatedDate = ztIoPaletteHeader.CreatedDate,
        CreatedUserName = ztIoPaletteHeader.CreatedUserName,
        Description = ztIoPaletteHeader.Description,
        IsCompleted = ztIoPaletteHeader.IsCompleted,
        PaletteCode = ztIoPaletteHeader.PaletteCode,
        PaletteHeaderID = ztIoPaletteHeader.PaletteHeaderID
      };
      ((VisualElement) page.stckPaletteOrderList).IsVisible = false;
      ((VisualElement) page.stckForm).IsVisible = true;
      ((InputView) page.txtPalette).Text = page.selectedPaletteHeader.PaletteCode;
      // ISSUE: reference to a compiler-generated method
      Device.BeginInvokeOnMainThread(new Action(page.\u003CTNewPalette_Clicked\u003Eb__11_0));
    }

    private async void txtPackage_Completed(object sender, EventArgs e)
    {
      Palette page = this;
      string text = ((InputView) page.txtPackage).Text;
      if (page.selectedPaletteHeader == null || string.IsNullOrEmpty(text))
        return;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("SavePaletteDetail?paletteHeaderID={0}&userName={1}&packageCode={2}", (object) page.selectedPaletteHeader.PaletteHeaderID, (object) GlobalMob.User.UserName, (object) text), (ContentPage) page);
      if (!returnModel.Success)
        return;
      pIOPaletteDetailReturnModel detailReturnModel = GlobalMob.JsonDeserialize<pIOPaletteDetailReturnModel>(returnModel.Result);
      if (detailReturnModel != null)
      {
        if (detailReturnModel.PaletteDetailID == -1)
        {
          GlobalMob.PlayError();
          int num = await ((Page) page).DisplayAlert("Hata", detailReturnModel.Description, "", "Tamam") ? 1 : 0;
          page.BarcodeFocus();
        }
        else
        {
          page.detailList.Select<pIOPaletteDetailReturnModel, pIOPaletteDetailReturnModel>((Func<pIOPaletteDetailReturnModel, pIOPaletteDetailReturnModel>) (c =>
          {
            c.LastReadBarcode = false;
            return c;
          })).ToList<pIOPaletteDetailReturnModel>();
          detailReturnModel.LastReadBarcode = true;
          page.detailList.Add(detailReturnModel);
          page.RefreshData();
          page.BarcodeFocus();
        }
      }
      else
      {
        GlobalMob.PlayError();
        int num = await ((Page) page).DisplayAlert("Hata", "Koli bulunamadı", "", "Tamam") ? 1 : 0;
        page.BarcodeFocus();
      }
    }

    private void RefreshData()
    {
      ((ItemsView<Cell>) this.lstPackage).ItemsSource = (IEnumerable) null;
      ((VisualElement) this.lstPackage).IsVisible = this.detailList.Count<pIOPaletteDetailReturnModel>() > 0;
      this.lblListHeader.Text = "Koli Sayısı : " + this.detailList.Count<pIOPaletteDetailReturnModel>().ToString();
      ((ItemsView<Cell>) this.lstPackage).ItemsSource = (IEnumerable) this.detailList.OrderByDescending<pIOPaletteDetailReturnModel, bool>((Func<pIOPaletteDetailReturnModel, bool>) (x => x.LastReadBarcode));
    }

    private void BarcodeFocus()
    {
      ((InputView) this.txtPackage).Text = "";
      ((VisualElement) this.txtPackage).Focus();
    }

    private async void btnSuccess_Clicked(object sender, EventArgs e)
    {
      Palette page1 = this;
      if (!await ((Page) page1).DisplayAlert("Onay", "Palet kapatılacak.Emin misiniz?", "Evet", "Hayır") || page1.selectedPaletteHeader == null)
        return;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("CompletePalette?headerID={0}", (object) page1.selectedPaletteHeader.PaletteHeaderID), (ContentPage) page1);
      if (!returnModel.Success)
        return;
      if (!GlobalMob.JsonDeserialize<bool>(returnModel.Result))
      {
        int num = await ((Page) page1).DisplayAlert("Hata", "Bir hata oluştu", "", "Tamam") ? 1 : 0;
      }
      else
      {
        Page page2 = await ((NavigableElement) page1).Navigation.PopAsync();
      }
    }

    private void txtSearch_Completed(object sender, EventArgs e) => this.FilterPaletteHeader();

    private void imgSearch_Clicked(object sender, EventArgs e) => this.FilterPaletteHeader();

    private void FilterPaletteHeader()
    {
      string text = ((InputView) this.txtSearch).Text;
      if (string.IsNullOrEmpty(text))
      {
        ((ItemsView<Cell>) this.lstPalette).ItemsSource = (IEnumerable) null;
        ((ItemsView<Cell>) this.lstPalette).ItemsSource = (IEnumerable) this.headerList;
      }
      else
      {
        ((ItemsView<Cell>) this.lstPalette).ItemsSource = (IEnumerable) null;
        ((ItemsView<Cell>) this.lstPalette).ItemsSource = (IEnumerable) this.headerList.Where<pIOPaletteHeaderReturnModel>((Func<pIOPaletteHeaderReturnModel, bool>) (x => x.PaletteCode.Contains(text) || x.Description.Contains(text))).ToList<pIOPaletteHeaderReturnModel>();
      }
    }

    private async void imgPrint_Clicked(object sender, EventArgs e)
    {
      Palette page = this;
      await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
      int num = 2;
      List<BLReport> repList = new List<BLReport>();
      BLReport blReport = new BLReport()
      {
        ReportTypeID = 3,
        UserID = GlobalMob.User.UserID,
        FileType = num,
        PrinterBrandID = Convert.ToInt32((object) page.selectedPaletteHeader.PrinterBrandID),
        NetworkPrinter = Convert.ToBoolean((object) page.selectedPaletteHeader.NetworkPrinter)
      };
      blReport.ParamList = new List<BLReportParam>();
      blReport.ParamList.Add(new BLReportParam()
      {
        ParamName = "PaletteCode",
        ParamValue = page.selectedPaletteHeader.PaletteCode,
        ParamType = 20
      });
      repList.Add(blReport);
      GlobalMob.BLPrint(repList, (object) page.selectedPaletteHeader, (ContentPage) page);
      GlobalMob.CloseLoading();
    }

    private async void cmDelete_Clicked(object sender, EventArgs e)
    {
      Palette page = this;
      pIOPaletteDetailReturnModel selectItem = (pIOPaletteDetailReturnModel) (sender as MenuItem).CommandParameter;
      if (!await ((Page) page).DisplayAlert("", selectItem.PackageCode + " numaralı koli silinecek emin misiniz?", "Evet", "Hayır"))
      {
        selectItem = (pIOPaletteDetailReturnModel) null;
      }
      else
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("DeletePaletteDetail?detailID={0}", (object) selectItem.PaletteDetailID), (ContentPage) page);
        if (!returnModel.Success)
          selectItem = (pIOPaletteDetailReturnModel) null;
        else if (!GlobalMob.JsonDeserialize<bool>(returnModel.Result))
        {
          selectItem = (pIOPaletteDetailReturnModel) null;
        }
        else
        {
          page.detailList.Remove(selectItem);
          page.RefreshData();
          selectItem = (pIOPaletteDetailReturnModel) null;
        }
      }
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (Palette).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/Palette.xaml",
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
        ImageButton imageButton1 = new ImageButton();
        StackLayout stackLayout1 = new StackLayout();
        Label label1 = new Label();
        StackLayout stackLayout2 = new StackLayout();
        BindingExtension bindingExtension1 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView1 = new ListView();
        StackLayout stackLayout3 = new StackLayout();
        KeyboardEnableEffect keyboardEnableEffect1 = new KeyboardEnableEffect();
        Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
        ImageButton imageButton2 = new ImageButton();
        StackLayout stackLayout4 = new StackLayout();
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension3 = new BindingExtension();
        Button button = new Button();
        StackLayout stackLayout5 = new StackLayout();
        KeyboardEnableEffect keyboardEnableEffect2 = new KeyboardEnableEffect();
        Xamarin.Forms.Entry entry3 = new Xamarin.Forms.Entry();
        StackLayout stackLayout6 = new StackLayout();
        BindingExtension bindingExtension4 = new BindingExtension();
        ReferenceExtension referenceExtension3 = new ReferenceExtension();
        BindingExtension bindingExtension5 = new BindingExtension();
        ReferenceExtension referenceExtension4 = new ReferenceExtension();
        BindingExtension bindingExtension6 = new BindingExtension();
        Label label2 = new Label();
        StackLayout stackLayout7 = new StackLayout();
        DataTemplate dataTemplate2 = new DataTemplate();
        ListView listView2 = new ListView();
        StackLayout stackLayout8 = new StackLayout();
        StackLayout stackLayout9 = new StackLayout();
        Palette palette;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (palette = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) palette, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("palette", (object) palette);
        if (((Element) palette).StyleId == null)
          ((Element) palette).StyleId = "palette";
        ((INameScope) nameScope).RegisterName("stckContent", (object) stackLayout9);
        if (((Element) stackLayout9).StyleId == null)
          ((Element) stackLayout9).StyleId = "stckContent";
        ((INameScope) nameScope).RegisterName("stckPaletteOrderList", (object) stackLayout3);
        if (((Element) stackLayout3).StyleId == null)
          ((Element) stackLayout3).StyleId = "stckPaletteOrderList";
        ((INameScope) nameScope).RegisterName("txtSearch", (object) entry1);
        if (((Element) entry1).StyleId == null)
          ((Element) entry1).StyleId = "txtSearch";
        ((INameScope) nameScope).RegisterName("imgSearch", (object) imageButton1);
        if (((Element) imageButton1).StyleId == null)
          ((Element) imageButton1).StyleId = "imgSearch";
        ((INameScope) nameScope).RegisterName("stckEmptyMessage", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckEmptyMessage";
        ((INameScope) nameScope).RegisterName("lstPalette", (object) listView1);
        if (((Element) listView1).StyleId == null)
          ((Element) listView1).StyleId = "lstPalette";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout8);
        if (((Element) stackLayout8).StyleId == null)
          ((Element) stackLayout8).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("stckPackage", (object) stackLayout4);
        if (((Element) stackLayout4).StyleId == null)
          ((Element) stackLayout4).StyleId = "stckPackage";
        ((INameScope) nameScope).RegisterName("txtPalette", (object) entry2);
        if (((Element) entry2).StyleId == null)
          ((Element) entry2).StyleId = "txtPalette";
        ((INameScope) nameScope).RegisterName("imgPrint", (object) imageButton2);
        if (((Element) imageButton2).StyleId == null)
          ((Element) imageButton2).StyleId = "imgPrint";
        ((INameScope) nameScope).RegisterName("stckSuccess", (object) stackLayout5);
        if (((Element) stackLayout5).StyleId == null)
          ((Element) stackLayout5).StyleId = "stckSuccess";
        ((INameScope) nameScope).RegisterName("btnSuccess", (object) button);
        if (((Element) button).StyleId == null)
          ((Element) button).StyleId = "btnSuccess";
        ((INameScope) nameScope).RegisterName("stckBarcode", (object) stackLayout6);
        if (((Element) stackLayout6).StyleId == null)
          ((Element) stackLayout6).StyleId = "stckBarcode";
        ((INameScope) nameScope).RegisterName("txtPackage", (object) entry3);
        if (((Element) entry3).StyleId == null)
          ((Element) entry3).StyleId = "txtPackage";
        ((INameScope) nameScope).RegisterName("lstPackage", (object) listView2);
        if (((Element) listView2).StyleId == null)
          ((Element) listView2).StyleId = "lstPackage";
        ((INameScope) nameScope).RegisterName("lblListHeader", (object) label2);
        if (((Element) label2).StyleId == null)
          ((Element) label2).StyleId = "lblListHeader";
        this.palette = (ContentPage) palette;
        this.stckContent = stackLayout9;
        this.stckPaletteOrderList = stackLayout3;
        this.txtSearch = entry1;
        this.imgSearch = imageButton1;
        this.stckEmptyMessage = stackLayout2;
        this.lstPalette = listView1;
        this.stckForm = stackLayout8;
        this.stckPackage = stackLayout4;
        this.txtPalette = entry2;
        this.imgPrint = imageButton2;
        this.stckSuccess = stackLayout5;
        this.btnSuccess = button;
        this.stckBarcode = stackLayout6;
        this.txtPackage = entry3;
        this.lstPackage = listView2;
        this.lblListHeader = label2;
        ((BindableObject) stackLayout9).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout9).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout9).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout3).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Ara");
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry1).SetValue(View.MarginProperty, (object) new Thickness(5.0));
        entry1.Completed += new EventHandler(palette.txtSearch_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 22);
        ((BindableObject) imageButton1).SetValue(ImageButton.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("search.png"));
        ((BindableObject) imageButton1).SetValue(ImageButton.AspectProperty, (object) (Aspect) 0);
        ((BindableObject) imageButton1).SetValue(VisualElement.BackgroundColorProperty, (object) new Color(0.55686277151107788, 0.31764706969261169, 0.59607845544815063, 1.0));
        ((BindableObject) imageButton1).SetValue(View.MarginProperty, (object) new Thickness(5.0));
        imageButton1.Clicked += new EventHandler(palette.imgSearch_Clicked);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) imageButton1);
        VisualDiagnostics.RegisterSourceInfo((object) imageButton1, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 18);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) stackLayout2).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) stackLayout2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) label1).SetValue(Label.TextProperty, (object) "Bekeyen Palet Bulunmamaktadır.");
        ((BindableObject) label1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.CenterAndExpand);
        ((BindableObject) label1).SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Label label3 = label1;
        BindableProperty fontSizeProperty = Label.FontSizeProperty;
        FontSizeConverter fontSizeConverter = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 5];
        objArray1[0] = (object) label1;
        objArray1[1] = (object) stackLayout2;
        objArray1[2] = (object) stackLayout3;
        objArray1[3] = (object) stackLayout9;
        objArray1[4] = (object) palette;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) Label.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (Palette).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(18, 124)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter).ConvertFromInvariantString("Medium", (IServiceProvider) xamlServiceProvider1);
        ((BindableObject) label3).SetValue(fontSizeProperty, obj2);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) label1);
        VisualDiagnostics.RegisterSourceInfo((object) label1, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 18, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 18);
        ((BindableObject) listView1).SetValue(ListView.RowHeightProperty, (object) 80);
        bindingExtension1.Path = ".";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView1).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase1);
        ((BindableObject) listView1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        DataTemplate dataTemplate3 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Palette.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_16 xamlCdataTemplate16 = new Palette.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_16();
        object[] objArray2 = new object[0 + 5];
        objArray2[0] = (object) dataTemplate1;
        objArray2[1] = (object) listView1;
        objArray2[2] = (object) stackLayout3;
        objArray2[3] = (object) stackLayout9;
        objArray2[4] = (object) palette;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate16.parentValues = objArray2;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate16.root = palette;
        // ISSUE: reference to a compiler-generated method
        Func<object> func1 = new Func<object>(xamlCdataTemplate16.LoadDataTemplate);
        ((IDataTemplate) dataTemplate3).LoadTemplate = func1;
        ((BindableObject) listView1).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 22, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) listView1);
        VisualDiagnostics.RegisterSourceInfo((object) listView1, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout9).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 14);
        ((BindableObject) stackLayout8).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout8).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout8).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) stackLayout8).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Koli Barkodu");
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry2).SetValue(KeyboardEffect.EnableKeyboardProperty, (object) false);
        ((ICollection<Effect>) ((Element) entry2).Effects).Add((Effect) keyboardEnableEffect1);
        VisualDiagnostics.RegisterSourceInfo((object) keyboardEnableEffect1, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 53, 30);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 49, 22);
        ((BindableObject) imageButton2).SetValue(ImageButton.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("print.png"));
        ((BindableObject) imageButton2).SetValue(ImageButton.AspectProperty, (object) (Aspect) 0);
        ((BindableObject) imageButton2).SetValue(VisualElement.BackgroundColorProperty, (object) new Color(0.55686277151107788, 0.31764706969261169, 0.59607845544815063, 1.0));
        ((BindableObject) imageButton2).SetValue(View.MarginProperty, (object) new Thickness(5.0));
        ((BindableObject) imageButton2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.End);
        imageButton2.Clicked += new EventHandler(palette.imgPrint_Clicked);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) imageButton2);
        VisualDiagnostics.RegisterSourceInfo((object) imageButton2, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 56, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 48, 18);
        ((BindableObject) stackLayout5).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) button).SetValue(Button.TextProperty, (object) "Tamamla");
        ((BindableObject) button).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        button.Clicked += new EventHandler(palette.btnSuccess_Clicked);
        ((BindableObject) button).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        referenceExtension1.Name = "palette";
        ReferenceExtension referenceExtension5 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 6];
        objArray3[0] = (object) bindingExtension2;
        objArray3[1] = (object) button;
        objArray3[2] = (object) stackLayout5;
        objArray3[3] = (object) stackLayout8;
        objArray3[4] = (object) stackLayout9;
        objArray3[5] = (object) palette;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray3, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver2.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (Palette).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(63, 28)));
        object obj4 = ((IMarkupExtension) referenceExtension5).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension2.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 63, 28);
        bindingExtension2.Path = "ButtonColor";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) button).SetBinding(VisualElement.BackgroundColorProperty, bindingBase2);
        referenceExtension2.Name = "palette";
        ReferenceExtension referenceExtension6 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 6];
        objArray4[0] = (object) bindingExtension3;
        objArray4[1] = (object) button;
        objArray4[2] = (object) stackLayout5;
        objArray4[3] = (object) stackLayout8;
        objArray4[4] = (object) stackLayout9;
        objArray4[5] = (object) palette;
        SimpleValueTargetProvider valueTargetProvider3;
        object obj5 = (object) (valueTargetProvider3 = new SimpleValueTargetProvider(objArray4, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider3.Add(type5, (object) valueTargetProvider3);
        xamlServiceProvider3.Add(typeof (IReferenceProvider), obj5);
        Type type6 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver3 = new XmlNamespaceResolver();
        namespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver3.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (Palette).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(64, 28)));
        object obj6 = ((IMarkupExtension) referenceExtension6).ProvideValue((IServiceProvider) xamlServiceProvider3);
        bindingExtension3.Source = obj6;
        VisualDiagnostics.RegisterSourceInfo(obj6, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 64, 28);
        bindingExtension3.Path = "TextColor";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) button).SetBinding(Button.TextColorProperty, bindingBase3);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) button);
        VisualDiagnostics.RegisterSourceInfo((object) button, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 61, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 60, 18);
        ((BindableObject) stackLayout6).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry3).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Barkod No Giriniz/Okutunuz");
        ((BindableObject) entry3).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("True"));
        entry3.Completed += new EventHandler(palette.txtPackage_Completed);
        ((BindableObject) entry3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry3).SetValue(KeyboardEffect.EnableKeyboardProperty, (object) false);
        ((ICollection<Effect>) ((Element) entry3).Effects).Add((Effect) keyboardEnableEffect2);
        VisualDiagnostics.RegisterSourceInfo((object) keyboardEnableEffect2, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 71, 30);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) entry3);
        VisualDiagnostics.RegisterSourceInfo((object) entry3, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 67, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 66, 18);
        bindingExtension4.Path = ".";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView2).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase4);
        ((BindableObject) listView2).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) listView2).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        ((BindableObject) listView2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
        ((BindableObject) listView2).SetValue(VisualElement.HeightRequestProperty, (object) 5000.0);
        ((BindableObject) listView2).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        ((BindableObject) label2).SetValue(Label.TextProperty, (object) "Koliler");
        ((BindableObject) label2).SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        referenceExtension3.Name = "palette";
        ReferenceExtension referenceExtension7 = referenceExtension3;
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray5 = new object[0 + 7];
        objArray5[0] = (object) bindingExtension5;
        objArray5[1] = (object) label2;
        objArray5[2] = (object) stackLayout7;
        objArray5[3] = (object) listView2;
        objArray5[4] = (object) stackLayout8;
        objArray5[5] = (object) stackLayout9;
        objArray5[6] = (object) palette;
        SimpleValueTargetProvider valueTargetProvider4;
        object obj7 = (object) (valueTargetProvider4 = new SimpleValueTargetProvider(objArray5, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider4.Add(type7, (object) valueTargetProvider4);
        xamlServiceProvider4.Add(typeof (IReferenceProvider), obj7);
        Type type8 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver4 = new XmlNamespaceResolver();
        namespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver4.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (Palette).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(82, 36)));
        object obj8 = ((IMarkupExtension) referenceExtension7).ProvideValue((IServiceProvider) xamlServiceProvider4);
        bindingExtension5.Source = obj8;
        VisualDiagnostics.RegisterSourceInfo(obj8, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 82, 36);
        bindingExtension5.Path = "ButtonColor";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) label2).SetBinding(Label.TextColorProperty, bindingBase5);
        referenceExtension4.Name = "palette";
        ReferenceExtension referenceExtension8 = referenceExtension4;
        XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
        Type type9 = typeof (IProvideValueTarget);
        object[] objArray6 = new object[0 + 7];
        objArray6[0] = (object) bindingExtension6;
        objArray6[1] = (object) label2;
        objArray6[2] = (object) stackLayout7;
        objArray6[3] = (object) listView2;
        objArray6[4] = (object) stackLayout8;
        objArray6[5] = (object) stackLayout9;
        objArray6[6] = (object) palette;
        SimpleValueTargetProvider valueTargetProvider5;
        object obj9 = (object) (valueTargetProvider5 = new SimpleValueTargetProvider(objArray6, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider5.Add(type9, (object) valueTargetProvider5);
        xamlServiceProvider5.Add(typeof (IReferenceProvider), obj9);
        Type type10 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver5 = new XmlNamespaceResolver();
        namespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver5.Add("effects", "clr-namespace:Xamarin.KeyboardHelper;assembly=Xamarin.KeyboardHelper");
        XamlTypeResolver xamlTypeResolver5 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver5, typeof (Palette).GetTypeInfo().Assembly);
        xamlServiceProvider5.Add(type10, (object) xamlTypeResolver5);
        xamlServiceProvider5.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(83, 36)));
        object obj10 = ((IMarkupExtension) referenceExtension8).ProvideValue((IServiceProvider) xamlServiceProvider5);
        bindingExtension6.Source = obj10;
        VisualDiagnostics.RegisterSourceInfo(obj10, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 83, 36);
        bindingExtension6.Path = "TextColor";
        BindingBase bindingBase6 = ((IMarkupExtension<BindingBase>) bindingExtension6).ProvideValue((IServiceProvider) null);
        ((BindableObject) label2).SetBinding(VisualElement.BackgroundColorProperty, bindingBase6);
        ((BindableObject) label2).SetValue(Label.FontProperty, new FontTypeConverter().ConvertFromInvariantString("Bold,20"));
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) label2);
        VisualDiagnostics.RegisterSourceInfo((object) label2, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 80, 30);
        ((BindableObject) listView2).SetValue(ListView.HeaderProperty, (object) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 79, 26);
        DataTemplate dataTemplate4 = dataTemplate2;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Palette.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_17 xamlCdataTemplate17 = new Palette.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_17();
        object[] objArray7 = new object[0 + 5];
        objArray7[0] = (object) dataTemplate2;
        objArray7[1] = (object) listView2;
        objArray7[2] = (object) stackLayout8;
        objArray7[3] = (object) stackLayout9;
        objArray7[4] = (object) palette;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate17.parentValues = objArray7;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate17.root = palette;
        // ISSUE: reference to a compiler-generated method
        Func<object> func2 = new Func<object>(xamlCdataTemplate17.LoadDataTemplate);
        ((IDataTemplate) dataTemplate4).LoadTemplate = func2;
        ((BindableObject) listView2).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate2);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate2, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 88, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) listView2);
        VisualDiagnostics.RegisterSourceInfo((object) listView2, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 75, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout9).Children).Add((View) stackLayout8);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout8, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 47, 14);
        ((BindableObject) palette).SetValue(ContentPage.ContentProperty, (object) stackLayout9);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout9, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 8, 10);
        VisualDiagnostics.RegisterSourceInfo((object) palette, new Uri("Views\\Palette.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<Palette>(this, typeof (Palette));
      this.palette = NameScopeExtensions.FindByName<ContentPage>((Element) this, "palette");
      this.stckContent = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckContent");
      this.stckPaletteOrderList = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckPaletteOrderList");
      this.txtSearch = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtSearch");
      this.imgSearch = NameScopeExtensions.FindByName<ImageButton>((Element) this, "imgSearch");
      this.stckEmptyMessage = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckEmptyMessage");
      this.lstPalette = NameScopeExtensions.FindByName<ListView>((Element) this, "lstPalette");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.stckPackage = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckPackage");
      this.txtPalette = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtPalette");
      this.imgPrint = NameScopeExtensions.FindByName<ImageButton>((Element) this, "imgPrint");
      this.stckSuccess = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckSuccess");
      this.btnSuccess = NameScopeExtensions.FindByName<Button>((Element) this, "btnSuccess");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.txtPackage = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtPackage");
      this.lstPackage = NameScopeExtensions.FindByName<ListView>((Element) this, "lstPackage");
      this.lblListHeader = NameScopeExtensions.FindByName<Label>((Element) this, "lblListHeader");
    }
  }
}
