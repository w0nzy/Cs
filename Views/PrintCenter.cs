// Decompiled with JetBrains decompiler
// Type: Shelf.Views.PrintCenter
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Shelf.Helpers;
using Shelf.Manager;
using Shelf.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace Shelf.Views
{
  [XamlCompilation]
  [XamlFilePath("Views\\PrintCenter.xaml")]
  public class PrintCenter : ContentPage
  {
    private List<pIOBLReportParametersReturnModel> paramList;
    private pIOBluetoothReportTemplateReturnModel selectTemplate;
    private pIOBLReportParametersReturnModel selectedParam;
    private List<NameValue> entryValueList;
    private ListView lstEntryValueList;
    private string networkPrinterName = "";
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckReport;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckEmptyMessage;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstReports;

    public Color ButtonColor => Color.FromRgb(3, 10, 53);

    public Color TextColor => Color.White;

    public PrintCenter()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Baskı Merkezi";
      ToolbarItem toolbarItem1 = new ToolbarItem();
      ((MenuItem) toolbarItem1).Text = "Cihazlar";
      ToolbarItem toolbarItem2 = toolbarItem1;
      ((MenuItem) toolbarItem2).Clicked += new EventHandler(this.TItem_Clicked);
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(toolbarItem2);
      this.LoadData();
    }

    private void LoadData()
    {
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetBluetoothReportTemplate"), (ContentPage) this);
      if (!returnModel.Success)
        return;
      ((ItemsView<Cell>) this.lstReports).ItemsSource = (IEnumerable) GlobalMob.JsonDeserialize<List<pIOBluetoothReportTemplateReturnModel>>(returnModel.Result);
    }

    private async void TItem_Clicked(object sender, EventArgs e)
    {
      PrintCenter printCenter = this;
      ListView cnt = new ListView();
      ((ItemsView<Cell>) cnt).ItemsSource = (IEnumerable) GlobalMob.DeviceList();
      cnt.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(printCenter.LstDevice_ItemSelected);
      SelectItem selectItem = new SelectItem(cnt, "Bluetooth Yazıcılar");
      await ((NavigableElement) printCenter).Navigation.PushAsync((Page) selectItem);
    }

    private void LstDevice_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      GlobalMob.MobilePrinter = Convert.ToString(e.SelectedItem);
      ((NavigableElement) this).Navigation.PopAsync();
    }

    private async void btnPrint_Clicked(object sender, EventArgs e)
    {
      PrintCenter page = this;
      if (string.IsNullOrEmpty(GlobalMob.MobilePrinter))
      {
        int num1 = await ((Page) page).DisplayAlert("Hata", "Lütfen öncelikle yazıcı seçin", "", "Tamam") ? 1 : 0;
      }
      else
      {
        ReturnModel returnModel1 = GlobalMob.PostJson(string.Format("GetMobilePrinterText"), (ContentPage) page);
        if (!returnModel1.Success)
          return;
        pIOGetMobilePrinterTextReturnModel printerTextReturnModel = GlobalMob.JsonDeserialize<pIOGetMobilePrinterTextReturnModel>(returnModel1.Result);
        ReturnModel returnModel2 = DependencyService.Get<INativeHelper>((DependencyFetchTarget) 0).Print(new MobilePrinterProp()
        {
          BrandID = Convert.ToInt32((object) printerTextReturnModel.PrinterBrandID),
          PrintText = printerTextReturnModel.PrintText,
          PrinterName = GlobalMob.MobilePrinter
        });
        if (returnModel2.Success)
        {
          int num2 = await ((Page) page).DisplayAlert("Yazdırıldı", returnModel2.ErrorMessage, "", "Tamam") ? 1 : 0;
        }
        else
        {
          int num3 = await ((Page) page).DisplayAlert("Hata", returnModel2.ErrorMessage, "", "Tamam") ? 1 : 0;
        }
      }
    }

    private async void lstReports_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      PrintCenter page = this;
      if (e.SelectedItem == null)
        return;
      await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
      page.selectTemplate = (pIOBluetoothReportTemplateReturnModel) e.SelectedItem;
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetBLReportParameters?reportID={0}", (object) page.selectTemplate.BluetoothReportTemplateID), (ContentPage) page);
      if (returnModel.Success)
      {
        page.paramList = GlobalMob.JsonDeserialize<List<pIOBLReportParametersReturnModel>>(returnModel.Result);
        StackLayout stck = new StackLayout();
        Frame reportParam = page.GetReportParam(page.paramList);
        if (reportParam != null)
          ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) reportParam);
        page.lstReports.SelectedItem = (object) null;
        SelectItem selectItem = new SelectItem(stck, page.selectTemplate.ReportName);
        await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
      }
      GlobalMob.CloseLoading();
    }

    private Frame GetReportParam(List<pIOBLReportParametersReturnModel> paramList)
    {
      if (paramList == null || paramList.Count <= 0)
        return (Frame) null;
      Frame reportParam = new Frame();
      ((Layout) reportParam).Padding = Thickness.op_Implicit(10.0);
      ((View) reportParam).Margin = new Thickness(3.0, 3.0, 3.0, 3.0);
      ((View) reportParam).VerticalOptions = LayoutOptions.StartAndExpand;
      ((View) reportParam).HorizontalOptions = LayoutOptions.FillAndExpand;
      reportParam.BorderColor = Color.FromHex("#817A7C");
      reportParam.HasShadow = true;
      reportParam.CornerRadius = 3f;
      Grid grid = new Grid();
      ((DefinitionCollection<ColumnDefinition>) grid.ColumnDefinitions).Add(new ColumnDefinition()
      {
        Width = new GridLength(1.0, (GridUnitType) 1)
      });
      ((DefinitionCollection<ColumnDefinition>) grid.ColumnDefinitions).Add(new ColumnDefinition()
      {
        Width = new GridLength(1.0, (GridUnitType) 1)
      });
      for (int index = 0; index < paramList.Count; ++index)
        ((DefinitionCollection<RowDefinition>) grid.RowDefinitions).Add(new RowDefinition()
        {
          Height = new GridLength(1.0, (GridUnitType) 1)
        });
      int num = 0;
      foreach (pIOBLReportParametersReturnModel parametersReturnModel in (IEnumerable<pIOBLReportParametersReturnModel>) paramList.OrderBy<pIOBLReportParametersReturnModel, int?>((Func<pIOBLReportParametersReturnModel, int?>) (x => x.ParameterOrder)))
      {
        pIOBLReportParametersReturnModel item = parametersReturnModel;
        View view = (View) null;
        switch (item.ParameterType)
        {
          case 10:
            DatePicker datePicker = new DatePicker();
            datePicker.Format = "dd.MM.yyyy";
            try
            {
              datePicker.Date = DateTime.Now;
              if (!string.IsNullOrEmpty(item.ParamValue))
                datePicker.Date = Convert.ToDateTime(item.ParamValue);
            }
            catch (Exception ex)
            {
            }
            view = (View) datePicker;
            break;
          case 20:
            Xamarin.Forms.Entry entry1 = new Xamarin.Forms.Entry();
            ((InputView) entry1).Text = "";
            entry1.ReturnCommandParameter = (object) item;
            ((ICollection<Effect>) ((Element) entry1).Effects).Add((Effect) new LongPressedEffect());
            LongPressedEffect.SetCommand((BindableObject) entry1, this.LongPress(entry1));
            view = (View) entry1;
            break;
          case 30:
            Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
            ((InputView) entry2).Keyboard = Keyboard.Numeric;
            ((InputView) entry2).Text = item.ParamValue;
            view = (View) entry2;
            break;
          case 40:
            List<NameValue> source = JsonConvert.DeserializeObject<List<NameValue>>(item.ParamJson);
            Picker picker = new Picker();
            picker.Title = "Seçiniz.";
            picker.ItemDisplayBinding = (BindingBase) new Binding("Name", (BindingMode) 0, (IValueConverter) null, (object) null, (string) null, (object) null);
            picker.ItemsSource = (IList) source;
            if (source != null && source.Count > 0)
              picker.SelectedItem = (object) source.Where<NameValue>((Func<NameValue, bool>) (x => x.Value == item.ParamValue)).FirstOrDefault<NameValue>();
            view = (View) picker;
            break;
        }
        Label label1 = new Label();
        ((View) label1).HorizontalOptions = LayoutOptions.StartAndExpand;
        ((View) label1).VerticalOptions = LayoutOptions.Center;
        label1.LineBreakMode = (LineBreakMode) 4;
        label1.FontAttributes = (FontAttributes) 1;
        ((View) label1).Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
        label1.FontSize = 16.0;
        Label label2 = label1;
        label2.Text = item.ParameterLabel;
        if (Convert.ToBoolean((object) item.ParameterRequired))
          label2.TextColor = Color.Red;
        ((ICollection<View>) grid.Children).Add((View) label2);
        ((ICollection<View>) grid.Children).Add(view);
        Grid.SetColumn((BindableObject) label2, 0);
        Grid.SetRow((BindableObject) label2, num);
        Grid.SetColumn((BindableObject) view, 1);
        Grid.SetRow((BindableObject) view, num);
        ++num;
        item.cnt = view;
      }
      if (paramList.Count > 0)
      {
        Button button = new Button();
        button.Text = "Yazdır";
        button.TextColor = this.TextColor;
        ((VisualElement) button).BackgroundColor = this.ButtonColor;
        button.Clicked += new EventHandler(this.Btn_Clicked);
        ((ICollection<View>) grid.Children).Add((View) button);
        Grid.SetColumnSpan((BindableObject) button, 2);
        Grid.SetColumn((BindableObject) button, 0);
        Grid.SetRow((BindableObject) button, num);
      }
      ((ContentView) reportParam).Content = (View) grid;
      return reportParam;
    }

    private ICommand LongPress(Xamarin.Forms.Entry ent) => (ICommand) new Command((Action) (async () =>
    {
      this.selectedParam = (pIOBLReportParametersReturnModel) ent.ReturnCommandParameter;
      if (string.IsNullOrEmpty(this.selectedParam.ParamJson))
        return;
      this.entryValueList = JsonConvert.DeserializeObject<List<NameValue>>(this.selectedParam.ParamJson);
      StackLayout stck = new StackLayout();
      stck.Orientation = (StackOrientation) 0;
      this.lstEntryValueList = GlobalMob.GetListview("Name,Value", 2, 1, hasUnEvenRows: true);
      SearchBar searchBar1 = new SearchBar();
      ((InputView) searchBar1).Placeholder = "Ara";
      ((InputView) searchBar1).PlaceholderColor = Color.Orange;
      ((InputView) searchBar1).TextColor = Color.Orange;
      searchBar1.HorizontalTextAlignment = (TextAlignment) 1;
      searchBar1.FontSize = Device.GetNamedSize((NamedSize) 3, typeof (SearchBar));
      searchBar1.FontAttributes = (FontAttributes) 2;
      SearchBar searchBar2 = searchBar1;
      searchBar2.SearchButtonPressed += new EventHandler(this.SearchBar_SearchButtonPressed);
      ((InputView) searchBar2).TextChanged += new EventHandler<TextChangedEventArgs>(this.SearchBar_TextChanged);
      ((ItemsView<Cell>) this.lstEntryValueList).ItemsSource = (IEnumerable) this.entryValueList;
      this.lstEntryValueList.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(this.Select_SelectedItem);
      ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) searchBar2);
      ((ICollection<View>) ((Layout<View>) stck).Children).Add((View) this.lstEntryValueList);
      await ((NavigableElement) this).Navigation.PushAsync((Page) new SelectItem(stck, this.selectedParam.ParameterLabel + " Seçiniz"));
    }));

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (!string.IsNullOrEmpty(((InputView) sender).Text))
        return;
      this.ParameterListLoad("");
    }

    private void SearchBar_SearchButtonPressed(object sender, EventArgs e) => this.ParameterListLoad(((InputView) sender).Text);

    private void ParameterListLoad(string searchText)
    {
      ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetBLReportParameterResult?parameterID={0}&searchText={1}", (object) this.selectedParam.BLReportParameterID, (object) searchText), (ContentPage) this);
      if (!returnModel.Success)
        return;
      List<NameValue> nameValueList = JsonConvert.DeserializeObject<List<NameValue>>(returnModel.Result);
      ((ItemsView<Cell>) this.lstEntryValueList).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstEntryValueList).ItemsSource = (IEnumerable) nameValueList;
    }

    private void Select_SelectedItem(object sender, SelectedItemChangedEventArgs e)
    {
      ((InputView) this.selectedParam.cnt).Text = ((NameValue) e.SelectedItem).Name;
      ((NavigableElement) this).Navigation.PopAsync();
    }

    private async void Btn_Clicked(object sender, EventArgs e)
    {
      PrintCenter page = this;
      page.networkPrinterName = "";
      if (page.selectTemplate.NetworkPrinter)
      {
        ReturnModel returnModel = GlobalMob.PostJson("GetPrinters", (ContentPage) page);
        if (!returnModel.Success)
          return;
        List<string> stringList = JsonConvert.DeserializeObject<List<string>>(returnModel.Result);
        ListView shelfListview = GlobalMob.GetShelfListview();
        ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) stringList;
        shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(page.LstPrinter_ItemSelected);
        SelectItem selectItem = new SelectItem(shelfListview, "Yazıcı seçiniz");
        await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
      }
      else
        page.PrintOperation();
    }

    private void LstPrinter_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      this.networkPrinterName = Convert.ToString(e.SelectedItem);
      ((NavigableElement) this).Navigation.PopAsync();
      this.PrintOperation();
    }

    private async void PrintOperation()
    {
      PrintCenter page = this;
      page.SetParamVal();
      List<BLReport> blReportList = new List<BLReport>();
      BLReport blReport = new BLReport()
      {
        ReportID = page.selectTemplate.BluetoothReportTemplateID,
        ReportTypeID = page.selectTemplate.BluetoothReportTypeID,
        UserID = GlobalMob.User.UserID,
        PrinterName = page.networkPrinterName,
        AndroidPrintType = page.selectTemplate.AndroidPrintType
      };
      blReport.ParamList = new List<BLReportParam>();
      foreach (pIOBLReportParametersReturnModel parametersReturnModel in page.paramList)
        blReport.ParamList.Add(new BLReportParam()
        {
          ParamName = parametersReturnModel.ParameterName,
          ParamValue = parametersReturnModel.ParamValue,
          ParamType = parametersReturnModel.ParameterType
        });
      blReportList.Add(blReport);
      Dictionary<string, string> pList = new Dictionary<string, string>();
      pList.Add("json", JsonConvert.SerializeObject((object) blReportList));
      await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
      if (page.selectTemplate.FileName.Contains(".repx"))
      {
        if (page.selectTemplate.NetworkPrinter)
        {
          ReturnModel result = GlobalMob.PostJson("GetMobileReportItemExport", pList, (ContentPage) page).Result;
          if (result.Success && !GlobalMob.JsonDeserialize<ReturnModel>(result.Result).Success)
          {
            int num = await ((Page) page).DisplayAlert("Hata", result.ErrorMessage, "", "Tamam") ? 1 : 0;
          }
        }
        else
        {
          byte[] buffer = GlobalMob.PostJsonFile("GetMobileReportItemExport", pList);
          if (buffer != null)
          {
            if (page.selectTemplate.AndroidPrintType > 0)
            {
              Stream inputStream = (Stream) new MemoryStream(buffer);
              INativeHelper nativeHelper = DependencyService.Get<INativeHelper>((DependencyFetchTarget) 0);
              ReturnModel returnModel1 = new ReturnModel();
              ReturnModel returnModel2 = page.selectTemplate.AndroidPrintType != 1 ? nativeHelper.PrintImage(inputStream, "print.png") : nativeHelper.PrintPdf(inputStream, "print.pdf");
              if (!returnModel2.Success)
              {
                int num = await ((Page) page).DisplayAlert("Hata", returnModel2.ErrorMessage, "", "Tamam") ? 1 : 0;
              }
            }
            else
            {
              ReturnModel returnModel = DependencyService.Get<INativeHelper>((DependencyFetchTarget) 0).Print(new MobilePrinterProp()
              {
                BrandID = page.selectTemplate.PrinterBrandID,
                PrinterName = Settings.MobilePrinter,
                PrintText = "Image",
                IsImagePrint = true,
                ImageFile = buffer
              });
              if (!returnModel.Success)
              {
                int num = await ((Page) page).DisplayAlert("Hata", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
              }
            }
          }
        }
      }
      else
      {
        ReturnModel result = GlobalMob.PostJson("MobileTemplateText", pList, (ContentPage) page).Result;
        if (result.Success)
        {
          ztIOBluetoothReportTemplate tmp = JsonConvert.DeserializeObject<ztIOBluetoothReportTemplate>(result.Result);
          tmp.Description = GlobalMob.MobileTemplateReplace((object) null, tmp);
          ReturnModel returnModel = DependencyService.Get<INativeHelper>((DependencyFetchTarget) 0).Print(new MobilePrinterProp()
          {
            BrandID = tmp.PrinterBrandID,
            PrinterName = Settings.MobilePrinter,
            PrintText = tmp.Description
          });
          if (!returnModel.Success)
          {
            int num = await ((Page) page).DisplayAlert("Hata", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
          }
        }
      }
      GlobalMob.CloseLoading();
      pList = (Dictionary<string, string>) null;
    }

    private void SetParamVal()
    {
      foreach (pIOBLReportParametersReturnModel parametersReturnModel in this.paramList)
      {
        switch (parametersReturnModel.ParameterType)
        {
          case 10:
            DatePicker cnt1 = (DatePicker) parametersReturnModel.cnt;
            parametersReturnModel.ParamValue = cnt1.Date.ToString("dd.MM.yyyy");
            continue;
          case 20:
            Xamarin.Forms.Entry cnt2 = (Xamarin.Forms.Entry) parametersReturnModel.cnt;
            parametersReturnModel.ParamValue = ((InputView) cnt2).Text;
            continue;
          case 30:
            Xamarin.Forms.Entry cnt3 = (Xamarin.Forms.Entry) parametersReturnModel.cnt;
            parametersReturnModel.ParamValue = ((InputView) cnt3).Text;
            continue;
          case 40:
            NameValue selectedItem = (NameValue) ((Picker) parametersReturnModel.cnt).SelectedItem;
            if (selectedItem != null)
            {
              parametersReturnModel.ParamValue = selectedItem.Value;
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (PrintCenter).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/PrintCenter.xaml",
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
        BindingExtension bindingExtension = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout2 = new StackLayout();
        StackLayout stackLayout3 = new StackLayout();
        PrintCenter printCenter;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (printCenter = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) printCenter, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("stckReport", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckReport";
        ((INameScope) nameScope).RegisterName("stckEmptyMessage", (object) stackLayout1);
        if (((Element) stackLayout1).StyleId == null)
          ((Element) stackLayout1).StyleId = "stckEmptyMessage";
        ((INameScope) nameScope).RegisterName("lstReports", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstReports";
        this.stckReport = stackLayout2;
        this.stckEmptyMessage = stackLayout1;
        this.lstReports = listView;
        ((BindableObject) stackLayout3).SetValue(View.MarginProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout2).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout1).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) stackLayout1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) stackLayout1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) label1).SetValue(Label.TextProperty, (object) "Bekleyen Raf Emri Bulunmamaktadır.");
        ((BindableObject) label1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.CenterAndExpand);
        ((BindableObject) label1).SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        Label label2 = label1;
        BindableProperty fontSizeProperty = Label.FontSizeProperty;
        FontSizeConverter fontSizeConverter = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 5];
        objArray1[0] = (object) label1;
        objArray1[1] = (object) stackLayout1;
        objArray1[2] = (object) stackLayout2;
        objArray1[3] = (object) stackLayout3;
        objArray1[4] = (object) printCenter;
        SimpleValueTargetProvider valueTargetProvider;
        object obj1 = (object) (valueTargetProvider = new SimpleValueTargetProvider(objArray1, (object) Label.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider.Add(type1, (object) valueTargetProvider);
        xamlServiceProvider.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver = new XmlNamespaceResolver();
        namespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        XamlTypeResolver xamlTypeResolver = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver, typeof (PrintCenter).GetTypeInfo().Assembly);
        xamlServiceProvider.Add(type2, (object) xamlTypeResolver);
        xamlServiceProvider.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(12, 128)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter).ConvertFromInvariantString("Medium", (IServiceProvider) xamlServiceProvider);
        ((BindableObject) label2).SetValue(fontSizeProperty, obj2);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) label1);
        VisualDiagnostics.RegisterSourceInfo((object) label1, new Uri("Views\\PrintCenter.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\PrintCenter.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 18);
        ((BindableObject) listView).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        listView.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(printCenter.lstReports_ItemSelected);
        bindingExtension.Path = ".";
        BindingBase bindingBase = ((IMarkupExtension<BindingBase>) bindingExtension).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
        ((BindableObject) listView).SetValue(ListView.RowHeightProperty, (object) 60);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        ((BindableObject) listView).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        PrintCenter.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_21 xamlCdataTemplate21 = new PrintCenter.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_21();
        object[] objArray2 = new object[0 + 5];
        objArray2[0] = (object) dataTemplate1;
        objArray2[1] = (object) listView;
        objArray2[2] = (object) stackLayout2;
        objArray2[3] = (object) stackLayout3;
        objArray2[4] = (object) printCenter;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate21.parentValues = objArray2;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate21.root = printCenter;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate21.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\PrintCenter.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\PrintCenter.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\PrintCenter.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 14);
        ((BindableObject) printCenter).SetValue(ContentPage.ContentProperty, (object) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\PrintCenter.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 9, 10);
        VisualDiagnostics.RegisterSourceInfo((object) printCenter, new Uri("Views\\PrintCenter.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<PrintCenter>(this, typeof (PrintCenter));
      this.stckReport = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckReport");
      this.stckEmptyMessage = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckEmptyMessage");
      this.lstReports = NameScopeExtensions.FindByName<ListView>((Element) this, "lstReports");
    }
  }
}
