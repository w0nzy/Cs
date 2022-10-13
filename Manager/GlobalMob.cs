// Decompiled with JetBrains decompiler
// Type: Shelf.Manager.GlobalMob
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using Newtonsoft.Json;
using Plugin.SimpleAudioPlayer;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Shelf.Controls;
using Shelf.Helpers;
using Shelf.Models;
using Shelf.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xam.Plugin.AutoUpdate;
using Xam.Plugin.AutoUpdate.Services;
using Xamarin.Forms;
using XFNoSoftKeyboadEntryControl;

namespace Shelf.Manager
{
  public static class GlobalMob
  {
    private static ContentPage blPrintPage;
    private static List<BLReport> blReportList;
    private static ShelfList pageShelf;
    private static Entry shelfBarcodeEntry;
    private static ProductList pageProduct;
    private static Entry barcodeEntry;

    public static IList<string> DeviceList() => DependencyService.Get<INativeHelper>((DependencyFetchTarget) 0).GetDeviceList();

    public static ContentPage currentPage { get; set; }

    public static async void DownloadAndOpen(Page page)
    {
      string url = "http://" + GlobalMob.ServerName + "/apk/com.companyname.Shelf.apk";
      if (GlobalMob.ServerName.Contains("http"))
        url = GlobalMob.ServerName + "/apk/com.companyname.Shelf.apk";
      byte[] data = (byte[]) null;
      try
      {
        if (url.Contains("https"))
        {
          ServicePointManager.Expect100Continue = true;
          ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        data = await new HttpClient().GetAsync(url).Result.Content.ReadAsByteArrayAsync();
      }
      catch (Exception ex)
      {
        await page.DisplayAlert("Hata", ex.ToString(), "OK");
        throw ex;
      }
      string name = url.Substring(url.LastIndexOf("/") + 1);
      string str = DependencyService.Get<IFileOpener>((DependencyFetchTarget) 0).OpenFile(data, name);
      if (string.IsNullOrEmpty(str))
      {
        url = (string) null;
        data = (byte[]) null;
      }
      else
      {
        await page.DisplayAlert("Hata", str, "OK");
        url = (string) null;
        data = (byte[]) null;
      }
    }

    internal static void FillBarcodeType(Picker pckBarcodeType)
    {
      if (!GlobalMob.User.IsBarcodeType)
        return;
      List<PickerItem> pickerItemList = new List<PickerItem>();
      pickerItemList.Add(new PickerItem()
      {
        Caption = "Tek",
        Code = 1,
        Description = "Tek"
      });
      pickerItemList.Add(new PickerItem()
      {
        Caption = "Lot",
        Code = 2,
        Description = "Lot"
      });
      if (GlobalMob.User.IsUniqueBarcode)
        pickerItemList.Add(new PickerItem()
        {
          Caption = "Uni",
          Code = 3,
          Description = "Uni"
        });
      pckBarcodeType.ItemsSource = (IList) pickerItemList;
      pckBarcodeType.SelectedItem = (object) pickerItemList[0];
      ((VisualElement) pckBarcodeType).IsVisible = true;
    }

    public static T JsonDeserialize<T>(string json)
    {
      if (GlobalMob.IsDebug)
        DependencyService.Get<INativeHelper>((DependencyFetchTarget) 0).ShowMessage(Convert.ToString((object) typeof (T)).Replace("System.Collections.Generic.List`1", "").Replace("[", "").Replace("]", "").Replace("Shelf.Models", "").Replace("ReturnModel", "").Replace(".", ""));
      return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings()
      {
        NullValueHandling = (NullValueHandling) 1,
        DefaultValueHandling = (DefaultValueHandling) 1
      });
    }

    internal static void FillProcessType(Picker pckProcessType)
    {
      List<PickerItem> pickerItemList = new List<PickerItem>();
      pickerItemList.Add(new PickerItem()
      {
        Caption = "Çıkış",
        Code = 2,
        Description = "Çıkış"
      });
      pickerItemList.Add(new PickerItem()
      {
        Caption = "Giriş",
        Code = 1,
        Description = "Giriş"
      });
      pckProcessType.ItemsSource = (IList) pickerItemList;
      pckProcessType.SelectedItem = (object) pickerItemList[0];
    }

    internal static void InstallNewVersion()
    {
      string downloadUrl = string.Empty;
      if (Device.RuntimePlatform == "Android")
        downloadUrl = "http://164.68.118.254/apk/com.companyname.Shelf.apk";
      else if (Device.RuntimePlatform == "UWP")
        downloadUrl = "https://github.com/angelinn/TramlineFive.Xamarin/releases/download/2.8/TramlineFive.UWP_2.8.0.0_arm.appxbundle";
      UpdateManager.Initialize(new UpdateManagerParameters()
      {
        Title = "Yeni Versiyon",
        Message = "Uygulamanın yeni versiyonu mevcuttur.Lütfen güncelleyin",
        Confirm = "Güncelle",
        Cancel = "İptal",
        CheckForUpdatesFunction = (Func<Task<UpdatesCheckResponse>>) (() =>
        {
          try
          {
            return new UpdatesCheckResponse(true, downloadUrl);
          }
          catch (Exception ex)
          {
            return (UpdatesCheckResponse) null;
          }
        })
      }, UpdateMode.AutoInstall);
    }

    public static string MobilePrinter { get; set; }

    public static Color MenuColor { get; set; }

    public static MenuModelItem CurrentMenuItem { get; set; }

    public static Color ButtonColor { get; set; }

    public static Color TextColor { get; set; }

    public static bool isLogin { get; set; }

    public static bool IsDebug { get; set; }

    public static LoginModel User { get; set; }

    public static string ServerName { get; set; }

    public static void Exit() => DependencyService.Get<INativeHelper>((DependencyFetchTarget) 0)?.CloseApp();

    public static Task<string> InputBarcode(
      INavigation navigation,
      string title,
      string message,
      Keyboard keyboard,
      bool allTimeFocus = false)
    {
      TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
      Label label1 = new Label();
      label1.Text = title;
      ((View) label1).HorizontalOptions = LayoutOptions.Center;
      label1.FontAttributes = (FontAttributes) 1;
      Label label2 = label1;
      Label label3 = new Label() { Text = message };
      SoftkeyboardDisabledEntry softkeyboardDisabledEntry = new SoftkeyboardDisabledEntry();
      ((InputView) softkeyboardDisabledEntry).Keyboard = keyboard;
      SoftkeyboardDisabledEntry txtInput = softkeyboardDisabledEntry;
      txtInput.Completed += (EventHandler) (async (s, e) =>
      {
        string result;
        if (string.IsNullOrEmpty(((InputView) txtInput).Text))
        {
          result = (string) null;
        }
        else
        {
          result = ((InputView) txtInput).Text;
          Page page = await navigation.PopModalAsync();
          tcs.SetResult(result);
          result = (string) null;
        }
      });
      Button button1 = new Button();
      button1.Text = "Tamam";
      ((VisualElement) button1).WidthRequest = 100.0;
      ((VisualElement) button1).BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8);
      Button button2 = button1;
      button2.Clicked += (EventHandler) (async (s, e) =>
      {
        string result = ((InputView) txtInput).Text;
        Page page = await navigation.PopModalAsync();
        tcs.SetResult(result);
        result = (string) null;
      });
      Button button3 = new Button();
      button3.Text = "İptal";
      ((VisualElement) button3).WidthRequest = 100.0;
      ((VisualElement) button3).BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8);
      Button button4 = button3;
      button4.Clicked += (EventHandler) (async (s, e) =>
      {
        Page page = await navigation.PopModalAsync();
        tcs.SetResult((string) null);
      });
      StackLayout stackLayout1 = new StackLayout();
      stackLayout1.Orientation = (StackOrientation) 1;
      ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button2);
      ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button4);
      StackLayout stackLayout2 = stackLayout1;
      StackLayout stackLayout3 = new StackLayout();
      ((Layout) stackLayout3).Padding = new Thickness(0.0, 40.0, 0.0, 0.0);
      ((View) stackLayout3).VerticalOptions = LayoutOptions.StartAndExpand;
      ((View) stackLayout3).HorizontalOptions = LayoutOptions.CenterAndExpand;
      stackLayout3.Orientation = (StackOrientation) 0;
      ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) label2);
      ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) label3);
      ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) txtInput);
      ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout2);
      StackLayout stackLayout4 = stackLayout3;
      ContentPage contentPage = new ContentPage();
      ((Page) contentPage).Padding = Thickness.op_Implicit(10.0);
      contentPage.Content = (View) stackLayout4;
      ((VisualElement) contentPage).HeightRequest = 200.0;
      navigation.PushModalAsync((Page) contentPage);
      if (allTimeFocus)
        ((VisualElement) txtInput).Unfocused += (EventHandler<FocusEventArgs>) ((s, e) => ((VisualElement) txtInput).Focus());
      ((VisualElement) txtInput).Focus();
      Device.BeginInvokeOnMainThread((Action) (async () =>
      {
        await Task.Delay(40);
        ((VisualElement) txtInput).Focus();
      }));
      return tcs.Task;
    }

    internal static async void BLPrint(
      List<BLReport> repList,
      object replaceItem,
      ContentPage page)
    {
      BLReport rep = repList[0];
      GlobalMob.blPrintPage = page;
      GlobalMob.blReportList = repList;
      if (string.IsNullOrEmpty(Settings.MobilePrinter) && !rep.NetworkPrinter)
      {
        int num1 = await ((Page) page).DisplayAlert("Uyarı", "Lütfen öncelikle bluetooth yazıcı seçiniz", "", "Tamam") ? 1 : 0;
      }
      else
      {
        Dictionary<string, string> paramList = new Dictionary<string, string>();
        string str = JsonConvert.SerializeObject((object) repList);
        paramList.Add("json", str);
        if (rep.FileType == 2)
        {
          if (rep.NetworkPrinter)
          {
            ReturnModel returnModel = GlobalMob.PostJson("GetPrinters", page);
            if (!returnModel.Success)
              return;
            List<string> stringList = JsonConvert.DeserializeObject<List<string>>(returnModel.Result);
            ListView shelfListview = GlobalMob.GetShelfListview();
            ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) stringList;
            shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(GlobalMob.LstPrinter_ItemSelected);
            await ((NavigableElement) page).Navigation.PushAsync((Page) new SelectItem(shelfListview, "Yazıcı seçiniz"));
          }
          else
          {
            byte[] numArray = GlobalMob.PostJsonFile("GetMobileReportItemExport", paramList);
            if (numArray == null)
              return;
            ReturnModel returnModel = DependencyService.Get<INativeHelper>((DependencyFetchTarget) 0).Print(new MobilePrinterProp()
            {
              BrandID = rep.PrinterBrandID,
              PrinterName = Settings.MobilePrinter,
              PrintText = "Image",
              IsImagePrint = true,
              ImageFile = numArray
            });
            if (returnModel.Success)
              return;
            int num2 = await ((Page) page).DisplayAlert("Hata", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
          }
        }
        else
        {
          ReturnModel result = GlobalMob.PostJson("MobileTemplateText", paramList, page).Result;
          if (!result.Success)
            return;
          ztIOBluetoothReportTemplate tmp = JsonConvert.DeserializeObject<ztIOBluetoothReportTemplate>(result.Result);
          tmp.Description = GlobalMob.MobileTemplateReplace(replaceItem, tmp);
          ReturnModel returnModel = DependencyService.Get<INativeHelper>((DependencyFetchTarget) 0).Print(new MobilePrinterProp()
          {
            BrandID = tmp.PrinterBrandID,
            PrinterName = Settings.MobilePrinter,
            PrintText = tmp.Description
          });
          if (returnModel.Success)
            return;
          int num3 = await ((Page) page).DisplayAlert("Hata", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
        }
      }
    }

    private static async void LstPrinter_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      Page page = await ((NavigableElement) GlobalMob.blPrintPage).Navigation.PopAsync();
      string str1 = Convert.ToString(e.SelectedItem);
      foreach (BLReport blReport in GlobalMob.blReportList)
        blReport.PrinterName = str1;
      Dictionary<string, string> paramList = new Dictionary<string, string>();
      string str2 = JsonConvert.SerializeObject((object) GlobalMob.blReportList);
      paramList.Add("json", str2);
      ReturnModel result = GlobalMob.PostJson("GetMobileReportItemExport", paramList, GlobalMob.blPrintPage).Result;
      if (result.Success)
        return;
      int num = await ((Page) GlobalMob.blPrintPage).DisplayAlert("Hata", result.ErrorMessage, "", "Tamam") ? 1 : 0;
    }

    public static bool IsNumeric(string qty)
    {
      double result = 0.0;
      return double.TryParse(qty, out result);
    }

    public static Task<string> InputBox(
      INavigation navigation,
      string title,
      string message,
      Keyboard keyboard)
    {
      TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
      Label label1 = new Label();
      label1.Text = title;
      ((View) label1).HorizontalOptions = LayoutOptions.Center;
      label1.FontAttributes = (FontAttributes) 1;
      Label label2 = label1;
      Label label3 = new Label() { Text = message };
      Entry entry = new Entry();
      ((InputView) entry).Keyboard = keyboard;
      Entry txtInput = entry;
      txtInput.Completed += (EventHandler) (async (s, e) =>
      {
        string result;
        if (string.IsNullOrEmpty(((InputView) txtInput).Text))
        {
          result = (string) null;
        }
        else
        {
          result = ((InputView) txtInput).Text;
          Page page = await navigation.PopModalAsync();
          tcs.SetResult(result);
          result = (string) null;
        }
      });
      Button button1 = new Button();
      button1.Text = "Tamam";
      ((VisualElement) button1).WidthRequest = 100.0;
      ((VisualElement) button1).BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8);
      Button button2 = button1;
      button2.Clicked += (EventHandler) (async (s, e) =>
      {
        string result = ((InputView) txtInput).Text;
        Page page = await navigation.PopModalAsync();
        tcs.SetResult(result);
        result = (string) null;
      });
      Button button3 = new Button();
      button3.Text = "İptal";
      ((VisualElement) button3).WidthRequest = 100.0;
      ((VisualElement) button3).BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8);
      Button button4 = button3;
      button4.Clicked += (EventHandler) (async (s, e) =>
      {
        Page page = await navigation.PopModalAsync();
        tcs.SetResult((string) null);
      });
      StackLayout stackLayout1 = new StackLayout();
      stackLayout1.Orientation = (StackOrientation) 1;
      ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button2);
      ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) button4);
      StackLayout stackLayout2 = stackLayout1;
      StackLayout stackLayout3 = new StackLayout();
      ((Layout) stackLayout3).Padding = new Thickness(0.0, 40.0, 0.0, 0.0);
      ((View) stackLayout3).VerticalOptions = LayoutOptions.StartAndExpand;
      ((View) stackLayout3).HorizontalOptions = LayoutOptions.CenterAndExpand;
      stackLayout3.Orientation = (StackOrientation) 0;
      ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) label2);
      ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) label3);
      ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) txtInput);
      ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout2);
      StackLayout stackLayout4 = stackLayout3;
      ContentPage contentPage = new ContentPage();
      contentPage.Content = (View) stackLayout4;
      ((VisualElement) contentPage).HeightRequest = 200.0;
      navigation.PushModalAsync((Page) contentPage);
      ((VisualElement) txtInput).Focus();
      return tcs.Task;
    }

    internal static PopupPage ShowLoading()
    {
      StackLayout stackLayout1 = new StackLayout();
      ((View) stackLayout1).HorizontalOptions = LayoutOptions.Center;
      ((View) stackLayout1).VerticalOptions = LayoutOptions.Center;
      ((VisualElement) stackLayout1).BackgroundColor = Color.White;
      ((Layout) stackLayout1).Padding = new Thickness(10.0);
      ((View) stackLayout1).Margin = new Thickness(10.0);
      StackLayout stackLayout2 = stackLayout1;
      Label label = new Label()
      {
        Text = "Lütfen Bekleyiniz"
      };
      label.FontAttributes = (FontAttributes) 1;
      ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) label);
      ActivityIndicator activityIndicator = new ActivityIndicator()
      {
        IsRunning = true
      };
      ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) activityIndicator);
      PopupPage popupPage = new PopupPage();
      ((Page) popupPage).Title = "Lütfen Bekleyin";
      ((ContentPage) popupPage).Content = (View) stackLayout2;
      return popupPage;
    }

    internal static void CloseLoading() => PopupNavigation.Instance.PopAsync(true);

    public static ListView GetListview(
      string bindingCode,
      int colCount,
      int rowCount,
      List<CustomMenuItemParameter> mContextList = null,
      bool hasUnEvenRows = false,
      string rowColorCode = "RowColorCode")
    {
      ListView listview1 = new ListView();
      if (hasUnEvenRows)
        listview1.HasUnevenRows = hasUnEvenRows;
      ((ItemsView<Cell>) listview1).ItemTemplate = new DataTemplate((Func<object>) (() =>
      {
        ViewCell listview2 = new ViewCell();
        if (!hasUnEvenRows)
          ((Cell) listview2).Height = 20.0;
        Frame frame = new Frame();
        ((Layout) frame).Padding = Thickness.op_Implicit(10.0);
        ((View) frame).Margin = new Thickness(3.0, 0.0, 3.0, 3.0);
        ((View) frame).VerticalOptions = LayoutOptions.FillAndExpand;
        ((View) frame).HorizontalOptions = LayoutOptions.FillAndExpand;
        frame.OutlineColor = Color.FromHex("#817A7C");
        ((BindableObject) frame).SetBinding(VisualElement.BackgroundColorProperty, (BindingBase) new Binding(rowColorCode, (BindingMode) 0, (IValueConverter) null, (object) null, (string) null, (object) null));
        frame.HasShadow = true;
        frame.CornerRadius = 3f;
        Grid grid = new Grid();
        for (int index = 0; index < colCount; ++index)
          ((DefinitionCollection<ColumnDefinition>) grid.ColumnDefinitions).Add(new ColumnDefinition()
          {
            Width = new GridLength(1.0, (GridUnitType) 1)
          });
        for (int index = 0; index < rowCount; ++index)
          ((DefinitionCollection<RowDefinition>) grid.RowDefinitions).Add(new RowDefinition()
          {
            Height = new GridLength(1.0, (GridUnitType) 1)
          });
        ((View) grid).Margin = Thickness.op_Implicit(0.0);
        int num1 = 0;
        int num2 = 0;
        string str1 = bindingCode;
        char[] chArray = new char[1]{ ',' };
        foreach (string str2 in str1.Split(chArray))
        {
          Label label1 = new Label();
          ((View) label1).HorizontalOptions = LayoutOptions.StartAndExpand;
          ((View) label1).VerticalOptions = LayoutOptions.Center;
          label1.FontAttributes = (FontAttributes) 1;
          ((View) label1).Margin = new Thickness(10.0, 0.0, 0.0, 0.0);
          label1.FontSize = 16.0;
          Label label2 = label1;
          if (!hasUnEvenRows)
            label2.LineBreakMode = (LineBreakMode) 4;
          ((BindableObject) label2).SetBinding(Label.TextProperty, (BindingBase) new Binding(str2, (BindingMode) 0, (IValueConverter) null, (object) null, (string) null, (object) null));
          ((ICollection<View>) grid.Children).Add((View) label2);
          Grid.SetColumn((BindableObject) label2, num2);
          Grid.SetRow((BindableObject) label2, num1);
          if (num2 == colCount - 1)
          {
            if (((ICollection<View>) grid.Children).Count > 0)
              ((View) label2).HorizontalOptions = LayoutOptions.End;
            num2 = 0;
            ++num1;
          }
          else
            ++num2;
        }
        ((ContentView) frame).Content = (View) grid;
        if (mContextList != null)
        {
          foreach (CustomMenuItemParameter mContext in mContextList)
          {
            MenuItem menuItem = new MenuItem();
            menuItem.Text = mContext.Text;
            ((BindableObject) menuItem).SetBinding(MenuItem.CommandParameterProperty, (BindingBase) new Binding(".", (BindingMode) 0, (IValueConverter) null, (object) null, (string) null, (object) null));
            menuItem.Clicked += mContext.ClickedEvent;
            ((ICollection<MenuItem>) ((Cell) listview2).ContextActions).Add(menuItem);
          }
        }
        listview2.View = (View) frame;
        return (object) listview2;
      }));
      return listview1;
    }

    public static ListView GetShelfListviewWithGrid(string bindingCode = ".")
    {
      ListView listviewWithGrid1 = new ListView();
      listviewWithGrid1.HasUnevenRows = true;
      listviewWithGrid1.RowHeight = -1;
      listviewWithGrid1.SeparatorVisibility = (SeparatorVisibility) 1;
      ((View) listviewWithGrid1).VerticalOptions = LayoutOptions.FillAndExpand;
      ((ItemsView<Cell>) listviewWithGrid1).ItemTemplate = new DataTemplate((Func<object>) (() =>
      {
        ViewCell listviewWithGrid2 = new ViewCell();
        StackLayout stackLayout = new StackLayout();
        stackLayout.Orientation = (StackOrientation) 1;
        ((View) stackLayout).HorizontalOptions = LayoutOptions.FillAndExpand;
        ((View) stackLayout).VerticalOptions = LayoutOptions.FillAndExpand;
        ((View) stackLayout).Margin = Thickness.op_Implicit(10.0);
        ((BindableObject) stackLayout).SetBinding(VisualElement.BackgroundColorProperty, (BindingBase) new Binding("GroupRowColorCode", (BindingMode) 0, (IValueConverter) null, (object) null, (string) null, (object) null));
        string[] strArray = bindingCode.Split(',');
        Grid grid = new Grid();
        ((View) grid).HorizontalOptions = LayoutOptions.FillAndExpand;
        ((DefinitionCollection<ColumnDefinition>) grid.ColumnDefinitions).Add(new ColumnDefinition()
        {
          Width = GridLength.Star
        });
        ((DefinitionCollection<ColumnDefinition>) grid.ColumnDefinitions).Add(new ColumnDefinition()
        {
          Width = GridLength.Star
        });
        int int32 = Convert.ToInt32(strArray.Length / 2);
        for (int index = 0; index < int32; ++index)
          ((DefinitionCollection<RowDefinition>) grid.RowDefinitions).Add(new RowDefinition()
          {
            Height = GridLength.Star
          });
        int num1 = 0;
        int num2 = 0;
        string str1 = bindingCode;
        char[] chArray = new char[1]{ ',' };
        foreach (string str2 in str1.Split(chArray))
        {
          Label label1 = new Label();
          ((View) label1).HorizontalOptions = LayoutOptions.StartAndExpand;
          ((View) label1).VerticalOptions = LayoutOptions.Center;
          label1.LineBreakMode = (LineBreakMode) 4;
          label1.FontAttributes = (FontAttributes) 1;
          ((View) label1).Margin = new Thickness(10.0, 0.0, 0.0, 0.0);
          label1.FontSize = 16.0;
          Label label2 = label1;
          if (num1 == 1)
          {
            ((View) label2).HorizontalOptions = LayoutOptions.End;
            label2.HorizontalTextAlignment = (TextAlignment) 2;
          }
          ((BindableObject) label2).SetBinding(Label.TextProperty, (BindingBase) new Binding(str2, (BindingMode) 0, (IValueConverter) null, (object) null, (string) null, (object) null));
          Grid.SetColumn((BindableObject) label2, num1);
          Grid.SetRow((BindableObject) label2, num2);
          ++num1;
          if (num1 > 1)
          {
            num1 = 0;
            ++num2;
          }
          ((ICollection<View>) grid.Children).Add((View) label2);
        }
        ((ICollection<View>) ((Layout<View>) stackLayout).Children).Add((View) grid);
        listviewWithGrid2.View = (View) stackLayout;
        return (object) listviewWithGrid2;
      }));
      return listviewWithGrid1;
    }

    public static ReturnModel GetShelf(string shelfCode, ContentPage page)
    {
      ReturnModel ret = GlobalMob.PostJson(string.Format("GetShelf?shelfCode={0}", (object) shelfCode), page);
      if (!GlobalMob.ShelfWarehouseControl(ret))
        ret.Result = "";
      return ret;
    }

    internal static async Task<string> AskShelfCode(Page page)
    {
      string str = "";
      if (GlobalMob.User.IsAskShelfCode)
      {
        string text = await GlobalMob.InputBox(((NavigableElement) page).Navigation, "Yeni boş bir koli oluşturulacak emin misiniz?", "Lütfen Koli Kodunu Giriniz.", Keyboard.Chat);
        str = text;
        if (string.IsNullOrEmpty(text))
          str = "-1";
        if (!GlobalMob.PrefixControl(text, GlobalMob.User.ShelfPrefix))
        {
          GlobalMob.PlayError();
          int num = await page.DisplayAlert("Hata", "Hatalı kod : " + text + "\nKod önekleri:" + GlobalMob.User.ShelfPrefix, "", "Tamam") ? 1 : 0;
          str = "-1";
        }
      }
      return str;
    }

    public static ListView GetShelfListview(
      string bindingCode = ".",
      List<CustomMenuItemParameter> contextList = null)
    {
      ListView shelfListview1 = new ListView();
      ((ItemsView<Cell>) shelfListview1).ItemTemplate = new DataTemplate((Func<object>) (() =>
      {
        ViewCell shelfListview2 = new ViewCell();
        if (contextList != null)
        {
          foreach (CustomMenuItemParameter context in contextList)
          {
            MenuItem menuItem = new MenuItem();
            menuItem.Text = context.Text;
            ((BindableObject) menuItem).SetBinding(MenuItem.CommandParameterProperty, (BindingBase) new Binding(".", (BindingMode) 0, (IValueConverter) null, (object) null, (string) null, (object) null));
            menuItem.Clicked += context.ClickedEvent;
            ((ICollection<MenuItem>) ((Cell) shelfListview2).ContextActions).Add(menuItem);
          }
        }
        ((Cell) shelfListview2).Height = 20.0;
        StackLayout stackLayout = new StackLayout();
        stackLayout.Orientation = (StackOrientation) 1;
        ((View) stackLayout).Margin = Thickness.op_Implicit(10.0);
        ((BindableObject) stackLayout).SetBinding(VisualElement.BackgroundColorProperty, (BindingBase) new Binding("GroupRowColorCode", (BindingMode) 0, (IValueConverter) null, (object) null, (string) null, (object) null));
        string str1 = bindingCode;
        char[] chArray = new char[1]{ ',' };
        foreach (string str2 in str1.Split(chArray))
        {
          Label label1 = new Label();
          ((View) label1).HorizontalOptions = LayoutOptions.StartAndExpand;
          ((View) label1).VerticalOptions = LayoutOptions.Center;
          label1.LineBreakMode = (LineBreakMode) 4;
          label1.FontAttributes = (FontAttributes) 1;
          ((View) label1).Margin = new Thickness(10.0, 0.0, 0.0, 0.0);
          label1.FontSize = 16.0;
          Label label2 = label1;
          if (((ICollection<View>) ((Layout<View>) stackLayout).Children).Count > 1)
            ((View) label2).HorizontalOptions = LayoutOptions.End;
          ((BindableObject) label2).SetBinding(Label.TextProperty, (BindingBase) new Binding(str2, (BindingMode) 0, (IValueConverter) null, (object) null, (string) null, (object) null));
          ((ICollection<View>) ((Layout<View>) stackLayout).Children).Add((View) label2);
        }
        shelfListview2.View = (View) stackLayout;
        return (object) shelfListview2;
      }));
      return shelfListview1;
    }

    public static bool PrefixControl(string text, string prefixList)
    {
      bool flag = false;
      if (string.IsNullOrEmpty(prefixList))
        return true;
      string str1 = prefixList;
      char[] chArray = new char[1]{ ',' };
      foreach (string str2 in str1.Split(chArray))
      {
        if (!string.IsNullOrEmpty(str2) && text.StartsWith(str2))
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private static void Mn_Clicked(object sender, EventArgs e)
    {
    }

    public static byte[] PostJsonFile(string url, Dictionary<string, string> paramList)
    {
      ReturnModel returnModel = new ReturnModel();
      string serverName = GlobalMob.ServerName;
      url = !string.IsNullOrEmpty(serverName) ? "http://" + serverName + "/ShelfWebApi/" + url : "http://" + "164.68.118.254" + "/ShelfWebApi/" + url;
      using (HttpClient httpClient = new HttpClient())
      {
        byte[] bytes = Encoding.UTF8.GetBytes("a:a");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));
        FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>) paramList);
        try
        {
          HttpResponseMessage result = httpClient.PostAsync(url, (HttpContent) content).Result;
          returnModel.Success = true;
          return result.Content.ReadAsByteArrayAsync().Result;
        }
        catch (Exception ex)
        {
          returnModel.Success = false;
          returnModel.ErrorMessage = ex.ToString();
        }
      }
      return (byte[]) null;
    }

    public static ReturnModel PostJson(string url, ContentPage page)
    {
      ReturnModel returnModel = new ReturnModel();
      string serverName = GlobalMob.ServerName;
      url = !string.IsNullOrEmpty(serverName) ? (!GlobalMob.ServerName.Contains("http") ? "http://" + serverName + "/ShelfWebApi/" + url : serverName + "/ShelfWebApi/" + url) : "http://" + "10.0.0.250:5050" + "/ShelfWebApi/" + url;
      using (HttpClient client = new HttpClient())
      {
        if (url.Contains("https"))
        {
          ServicePointManager.Expect100Continue = true;
          ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        byte[] bytes = Encoding.UTF8.GetBytes("a:a");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));
        try
        {
          int num = 0;
          if (GlobalMob.User != null)
            num = GlobalMob.User.RequestTimeout;
          HttpResponseMessage httpResponseMessage;
          if (num > 0)
          {
            httpResponseMessage = GlobalMob.GetResponseMessage(client, url);
            if (httpResponseMessage == null)
            {
              returnModel.Success = false;
              returnModel.ErrorMessage = "Serverla bağlantı kurulamadı";
              return returnModel;
            }
          }
          else
            httpResponseMessage = client.GetAsync(url).Result;
          if (httpResponseMessage.IsSuccessStatusCode)
          {
            HttpContent content = httpResponseMessage.Content;
            returnModel.Result = content.ReadAsStringAsync().Result;
            returnModel.Success = true;
          }
        }
        catch (Exception ex)
        {
          returnModel.Success = false;
          returnModel.ErrorMessage = ex.ToString();
          ((Page) page)?.DisplayAlert("Hata", "Serverla bağlantı kurulamadı", "", "Tamam");
        }
      }
      return returnModel;
    }

    private static bool ShelfWarehouseControl(ReturnModel ret)
    {
      if (GlobalMob.User != null && !string.IsNullOrEmpty(GlobalMob.User.WarehouseCode) && !string.IsNullOrEmpty(ret.Result))
      {
        ztIOShelf ztIoShelf = JsonConvert.DeserializeObject<ztIOShelf>(ret.Result);
        if (ztIoShelf.WarehouseCode != GlobalMob.User.WarehouseCode)
        {
          DependencyService.Get<INativeHelper>((DependencyFetchTarget) 0).ShowMessage("Raf başka depoya ait : " + ztIoShelf.WarehouseCode);
          return false;
        }
      }
      return true;
    }

    public static async Task<ReturnModel> PostJson(
      string url,
      Dictionary<string, string> paramList,
      ContentPage page)
    {
      ReturnModel ret = new ReturnModel();
      string serverName = GlobalMob.ServerName;
      url = !string.IsNullOrEmpty(serverName) ? (!GlobalMob.ServerName.Contains("http") ? "http://" + serverName + "/ShelfWebApi/" + url : serverName + "/ShelfWebApi/" + url) : "http://" + "78.188.108.66:93" + "/ShelfWebApi/" + url;
      using (HttpClient client = new HttpClient())
      {
        if (url.Contains("https"))
        {
          ServicePointManager.Expect100Continue = true;
          ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes("a:a")));
        FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>) paramList);
        try
        {
          HttpResponseMessage httpResponseMessage;
          if (GlobalMob.User.RequestTimeout > 0)
          {
            httpResponseMessage = GlobalMob.PostResponseMessage(client, url, (HttpContent) content);
            if (httpResponseMessage == null)
            {
              ret.Success = false;
              ret.ErrorMessage = "Serverla bağlantı kurulamadı";
              return ret;
            }
          }
          else
            httpResponseMessage = await client.PostAsync(url, (HttpContent) content).ConfigureAwait(false);
          if (httpResponseMessage.IsSuccessStatusCode)
          {
            ret.Success = true;
            ret.Result = httpResponseMessage.Content.ReadAsStringAsync().Result;
          }
        }
        catch (Exception ex)
        {
          ret.Success = false;
          ret.ErrorMessage = ex.ToString();
          if (page != null)
          {
            int num = await ((Page) page).DisplayAlert("Hata", "Serverla bağlantı kurulamadı", "", "Tamam") ? 1 : 0;
          }
        }
      }
      return ret;
    }

    private static HttpResponseMessage GetResponseMessage(
      HttpClient client,
      string url)
    {
      return Task.Run<HttpResponseMessage>((Func<HttpResponseMessage>) (() =>
      {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        Task<HttpResponseMessage> async = client.GetAsync(url);
        if (Task.WaitAny(new Task[1]{ (Task) async }, GlobalMob.User.RequestTimeout) >= 0)
          return async.GetAwaiter().GetResult();
        cancellationTokenSource.Cancel();
        return (HttpResponseMessage) null;
      })).Result;
    }

    private static HttpResponseMessage PostResponseMessage(
      HttpClient client,
      string url,
      HttpContent content)
    {
      return Task.Run<HttpResponseMessage>((Func<HttpResponseMessage>) (() =>
      {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        Task<HttpResponseMessage> task = client.PostAsync(url, content);
        if (Task.WaitAny(new Task[1]{ (Task) task }, GlobalMob.User.RequestTimeout) >= 0)
          return task.GetAwaiter().GetResult();
        cancellationTokenSource.Cancel();
        return (HttpResponseMessage) null;
      })).Result;
    }

    internal static void AddShelfBarcodeLongPress(Entry txtBarcode)
    {
      GlobalMob.shelfBarcodeEntry = txtBarcode;
      ((ICollection<Effect>) ((Element) GlobalMob.shelfBarcodeEntry).Effects).Add((Effect) new LongPressedEffect());
      LongPressedEffect.SetCommand((BindableObject) GlobalMob.shelfBarcodeEntry, GlobalMob.LongPressShelf);
    }

    private static ICommand LongPressShelf => (ICommand) new Command((Action) (async () =>
    {
      GlobalMob.pageShelf = new ShelfList(((InputView) GlobalMob.shelfBarcodeEntry).Text);
      GlobalMob.pageShelf.ShelfSelectedItem += new EventHandler(GlobalMob.PageShelf_ShelfSelectedItem);
      await ((NavigableElement) GlobalMob.currentPage).Navigation.PushAsync((Page) GlobalMob.pageShelf);
    }));

    private static void PageShelf_ShelfSelectedItem(object sender, EventArgs e)
    {
      pIOGetShelfFromTextReturnModel selectedShelf = GlobalMob.pageShelf.selectedShelf;
      if (!string.IsNullOrEmpty(selectedShelf.Code))
      {
        ((InputView) GlobalMob.shelfBarcodeEntry).Text = selectedShelf.Code;
        ((IEntryController) GlobalMob.shelfBarcodeEntry).SendCompleted();
      }
      else
        ((Page) GlobalMob.currentPage).DisplayAlert("Bilgi", "Raf tanımlı değil", "", "Tamam");
    }

    internal static void AddBarcodeLongPress(Entry txtBarcode)
    {
      GlobalMob.barcodeEntry = txtBarcode;
      ((ICollection<Effect>) ((Element) GlobalMob.barcodeEntry).Effects).Add((Effect) new LongPressedEffect());
      LongPressedEffect.SetCommand((BindableObject) GlobalMob.barcodeEntry, GlobalMob.LongPress);
    }

    internal static void AddBarcodeLongPress(SoftkeyboardDisabledEntry txtBarcode)
    {
      GlobalMob.barcodeEntry = (Entry) txtBarcode;
      ((ICollection<Effect>) ((Element) GlobalMob.barcodeEntry).Effects).Add((Effect) new LongPressedEffect());
      LongPressedEffect.SetCommand((BindableObject) GlobalMob.barcodeEntry, GlobalMob.LongPress);
    }

    private static ICommand LongPress => (ICommand) new Command((Action) (async () =>
    {
      GlobalMob.pageProduct = new ProductList();
      GlobalMob.pageProduct.IsClose = true;
      GlobalMob.pageProduct.ProductSelectedItem += new EventHandler(GlobalMob.PageProduct_ProductSelectedItem);
      await ((NavigableElement) GlobalMob.currentPage).Navigation.PushAsync((Page) GlobalMob.pageProduct);
      GlobalMob.pageProduct.FocusSearch();
    }));

    private static void PageProduct_ProductSelectedItem(object sender, EventArgs e)
    {
      pIOGetShelfItemsReturnModel selectedItem = GlobalMob.pageProduct.selectedItem;
      if (!string.IsNullOrEmpty(selectedItem.Barcode))
      {
        if (selectedItem.IgnoreEntryComplete)
        {
          ((Page) GlobalMob.currentPage).DisplayAlert("Bilgi", "Ürünü arayarak okutma yetkiniz yok", "", "Tamam");
        }
        else
        {
          ((InputView) GlobalMob.barcodeEntry).Text = selectedItem.Barcode;
          ((IEntryController) GlobalMob.barcodeEntry).SendCompleted();
        }
      }
      else
        ((Page) GlobalMob.currentPage).DisplayAlert("Bilgi", "Barkod tanımlı değil", "", "Tamam");
    }

    public static async Task<ReturnModel> PostJsonWithParam(
      string url,
      Dictionary<string, string> paramList)
    {
      ReturnModel ret = new ReturnModel();
      string serverName = GlobalMob.ServerName;
      url = !string.IsNullOrEmpty(serverName) ? (!GlobalMob.ServerName.Contains("http") ? "http://" + serverName + "/ShelfWebApi/" + url : serverName + "/ShelfWebApi/" + url) : "http://" + "iontegration.com" + "/ShelfWebApi/" + url;
      using (HttpClient client = new HttpClient())
      {
        if (url.Contains("https"))
        {
          ServicePointManager.Expect100Continue = true;
          ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes("a:a")));
        FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>) paramList);
        try
        {
          HttpResponseMessage httpResponseMessage = await client.PostAsync(url, (HttpContent) content).ConfigureAwait(false);
          if (httpResponseMessage.IsSuccessStatusCode)
          {
            ret.Success = true;
            ret.Result = httpResponseMessage.Content.ReadAsStringAsync().Result;
          }
        }
        catch (Exception ex)
        {
          ret.Success = false;
          ret.ErrorMessage = ex.ToString();
        }
      }
      ReturnModel returnModel = ret;
      ret = (ReturnModel) null;
      return returnModel;
    }

    public static void PlaySave()
    {
      ISimpleAudioPlayer current = CrossSimpleAudioPlayer.Current;
      current.Load("Save.wav");
      current.Play();
    }

    public static void PlayError()
    {
      ISimpleAudioPlayer current = CrossSimpleAudioPlayer.Current;
      current.Load("Error.wav");
      current.Play();
    }

    public static string ReplaceTurkishCharachter(string text)
    {
      text = text.Replace("İ", "I");
      text = text.Replace("Ş", "S");
      text = text.Replace("Ö", "O");
      text = text.Replace("Ü", "U");
      text = text.Replace("Ç", "C");
      text = text.Replace("Ğ", "G");
      text = text.Replace("ş", "s");
      text = text.Replace("ö", "o");
      text = text.Replace("ü", "u");
      text = text.Replace("ç", "c");
      text = text.Replace("ğ", "g");
      return text;
    }

    public static string MobileTemplateReplace(object item, ztIOBluetoothReportTemplate tmp)
    {
      string str1 = tmp.Description.Replace("<Today>", DateTime.Now.ToString("dd.MM.yy HH:mm")).Replace("<UserName>", GlobalMob.User.UserName);
      if (item != null)
      {
        foreach (PropertyInfo property in item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
        {
          try
          {
            string name = property.Name;
            string str2 = Convert.ToString(item.GetType().GetProperty(property.Name).GetValue(item, (object[]) null));
            if (tmp.TurkishCharacterReplace)
              str2 = GlobalMob.ReplaceTurkishCharachter(str2);
            str1 = str1.Replace("<" + name + ">", str2);
          }
          catch (Exception ex)
          {
          }
        }
      }
      return str1;
    }
  }
}
