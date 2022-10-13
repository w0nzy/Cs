// Decompiled with JetBrains decompiler
// Type: Shelf.Views.Login
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using Rg.Plugins.Popup.Extensions;
using Shelf.Helpers;
using Shelf.Manager;
using Shelf.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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
  [XamlFilePath("Views\\Login.xaml")]
  public class Login : ContentPage
  {
    private string currentVersion;
    private string deviceNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtUserName;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtPassword;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Xamarin.Forms.Entry txtServer;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Switch chkRememberMe;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnLogin;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckVersion;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Label lblVersion;

    public Login()
    {
      this.InitializeComponent();
      ((VisualElement) this.btnLogin).BackgroundColor = Color.FromRgb(218, 18, 95);
      this.btnLogin.TextColor = GlobalMob.TextColor;
      ((InputView) this.txtServer).Text = "";
      INativeHelper nativeHelper = DependencyService.Get<INativeHelper>((DependencyFetchTarget) 0);
      this.currentVersion = nativeHelper.GetVersion();
      this.lblVersion.Text = "Versiyon :" + this.currentVersion;
      this.deviceNumber = nativeHelper.GetDeviceNumber();
      Label lblVersion = this.lblVersion;
      lblVersion.Text = lblVersion.Text + "\n" + this.deviceNumber;
      TapGestureRecognizer gestureRecognizer = new TapGestureRecognizer();
      gestureRecognizer.Tapped += new EventHandler(this.DeleteLicencetap_Tapped);
      ((ICollection<IGestureRecognizer>) ((View) this.stckVersion).GestureRecognizers).Add((IGestureRecognizer) gestureRecognizer);
      string userName = Settings.UserName;
      string password = Settings.Password;
      string server = Settings.Server;
      this.chkRememberMe.IsToggled = true;
      ((InputView) this.txtUserName).Text = !string.IsNullOrEmpty(userName) ? userName : "";
      ((InputView) this.txtPassword).Text = !string.IsNullOrEmpty(password) ? password : "";
      ((InputView) this.txtServer).Text = !string.IsNullOrEmpty(server) ? server : "";
    }

    private async void DeleteLicencetap_Tapped(object sender, EventArgs e)
    {
      Login page = this;
      if (!await ((Page) page).DisplayAlert("Devam?", "Bu terminal için lisansı silmek istiyor musunuz?", "Evet", "Hayır"))
        return;
      if (string.IsNullOrEmpty(((InputView) page.txtServer).Text))
      {
        int num1 = await ((Page) page).DisplayAlert("Bilgi", "Lütfen öncelikle server adresi giriniz", "", "Tamam") ? 1 : 0;
      }
      else
      {
        GlobalMob.ServerName = ((InputView) page.txtServer).Text;
        ReturnModel returnModel = GlobalMob.PostJson("DeleteLicenseUserMobile?serial=" + page.deviceNumber, (ContentPage) page);
        if (!returnModel.Success)
          return;
        string str = GlobalMob.JsonDeserialize<bool>(returnModel.Result) ? "Lisans silindi" : "Bir hata oluştu";
        int num2 = await ((Page) page).DisplayAlert("Bilgi", str, "", "Tamam") ? 1 : 0;
      }
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((Page) this).Title = "Raf Takip";
    }

    private async void btnLogin_Clicked(object sender, EventArgs e)
    {
      Login page = this;
      if (string.IsNullOrEmpty(((InputView) page.txtUserName).Text))
        return;
      if (string.IsNullOrEmpty(page.deviceNumber))
      {
        int num1 = await ((Page) page).DisplayAlert("Hata", "Aygıt numarası bulunamadı.", "", "Tamam") ? 1 : 0;
      }
      else
      {
        GlobalMob.User = new LoginModel()
        {
          RequestTimeout = 0
        };
        string str1 = ((InputView) page.txtUserName).Text.Replace("debug", "");
        string text = ((InputView) page.txtPassword).Text;
        if (((InputView) page.txtUserName).Text.Contains("debug"))
          GlobalMob.IsDebug = true;
        GlobalMob.ServerName = ((InputView) page.txtServer).Text;
        int num = 0;
        object obj;
        try
        {
          ReturnModel returnModel = GlobalMob.PostJson(string.Format("Login?userName={0}&password={1}&serial={2}", (object) str1, (object) text, (object) page.deviceNumber), (ContentPage) page);
          if (returnModel.Success)
          {
            LoginModel loginModel = GlobalMob.JsonDeserialize<LoginModel>(returnModel.Result);
            if (loginModel != null)
            {
              if (loginModel.UserID != -1)
              {
                Settings.UserName = page.chkRememberMe.IsToggled ? str1 : "";
                Settings.Password = page.chkRememberMe.IsToggled ? text : "";
                Settings.Server = page.chkRememberMe.IsToggled ? ((InputView) page.txtServer).Text : "";
                GlobalMob.User = loginModel;
                Settings.CurrentVersion = page.currentVersion;
                Settings.NextVersion = GlobalMob.User.NextVersion;
                if (!string.IsNullOrEmpty(GlobalMob.User.NextVersion) && Settings.CurrentVersion != GlobalMob.User.NextVersion && page.IsOldVersion())
                {
                  string str2 = "Uygulamanın yeni versiyonu mevcuttur.\n" + "Yeni versiyon otomatik olarak indirilecektir.Lütfen kurulumu tamamlayınız.\n" + "Eski Versiyon : " + Settings.CurrentVersion + "\n" + "Yeni Versiyon : " + GlobalMob.User.NextVersion;
                  int num2 = await ((Page) page).DisplayAlert("Yeni Versiyon", str2, "", "Tamam") ? 1 : 0;
                  await NavigationExtension.PushPopupAsync(((NavigableElement) page).Navigation, GlobalMob.ShowLoading(), true);
                  GlobalMob.DownloadAndOpen((Page) page);
                  GlobalMob.CloseLoading();
                  return;
                }
                Application.Current.MainPage = (Page) new NavigationPage((Page) new MainPage(page.GetMenuList()))
                {
                  BarBackgroundColor = GlobalMob.ButtonColor
                };
              }
              else
              {
                int num3 = await ((Page) page).DisplayAlert("Lisans Hatası", "Kullanıcı sayısı lisans hakkını aşmıştır.", "", "Tamam") ? 1 : 0;
              }
            }
            else
            {
              int num4 = await ((Page) page).DisplayAlert("Hata", "Kullanıcı adı veya şifre yanlış", "", "Tamam") ? 1 : 0;
            }
          }
          else
          {
            if (!(returnModel.ErrorMessage == "Serverla bağlantı kurulamadı"))
            {
              if (!string.IsNullOrEmpty(returnModel.ErrorMessage))
                goto label_22;
            }
            int num5 = await ((Page) page).DisplayAlert("Hata", "Servera ulaşılamadı", "", "Tamam") ? 1 : 0;
          }
        }
        catch (Exception ex)
        {
          obj = (object) ex;
          num = 1;
        }
label_22:
        if (num == 1)
        {
          int num6 = await ((Page) page).DisplayAlert("Hata", "Servera ulaşılamadı", "", "Tamam") ? 1 : 0;
        }
        obj = (object) null;
      }
    }

    private bool IsOldVersion()
    {
      try
      {
        return Convert.ToInt32(GlobalMob.User.NextVersion) > Convert.ToInt32(Settings.CurrentVersion);
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    private List<MenuModelItem> GetMenuList() => new List<MenuModelItem>()
    {
      new MenuModelItem()
      {
        ImageName = "product.png",
        Title = "ÜRÜN TOPLA",
        ColorCode = "#030A35",
        MenuId = 1,
        pageName = "Shelf.Views.Picking"
      },
      new MenuModelItem()
      {
        ImageName = "basket.png",
        Title = "SEPET OKUT",
        ColorCode = "#152897",
        MenuId = 2,
        pageName = "Shelf.Views.Basket"
      },
      new MenuModelItem()
      {
        ImageName = "basket.png",
        Title = "RAF GİRİŞİ",
        ColorCode = "#34CBC9",
        MenuId = 3
      },
      new MenuModelItem()
      {
        ImageName = "shelf.png",
        Title = "RAF GİRİŞİ (İRSALİYELİ)",
        ColorCode = "#34CBC9",
        MenuId = 4,
        HeaderMenuID = 3,
        pageName = "Shelf.Views.ShelfEntry3"
      },
      new MenuModelItem()
      {
        ImageName = "shelf.png",
        Title = "RAF GİRİŞİ (SERBEST)",
        ColorCode = "#34CBC9",
        MenuId = 5,
        HeaderMenuID = 3,
        pageName = "Shelf.Views.ShelfEntry"
      },
      new MenuModelItem()
      {
        ImageName = "shelf.png",
        Title = "SET ÇÖZ",
        ColorCode = "#34CBC9",
        MenuId = 21,
        HeaderMenuID = 3,
        pageName = "Shelf.Views.SetSolve"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "RULOTTAN RAFA TRANSFER",
        ColorCode = "#34CBC9",
        MenuId = 6,
        HeaderMenuID = 3,
        pageName = "Shelf.Views.RulotToShelf"
      },
      new MenuModelItem()
      {
        ImageName = "basket.png",
        Title = "RAF TAŞI",
        ColorCode = "#34CBC9",
        MenuId = 7,
        HeaderMenuID = 3,
        pageName = "Shelf.Views.ShelfToShelf"
      },
      new MenuModelItem()
      {
        ImageName = "shelfcounting.png",
        Title = "RAF SAYIM",
        ColorCode = "#152897",
        MenuId = 8,
        pageName = "Shelf.Views.ShelfCounting"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "RAF ÇIKIŞ",
        ColorCode = "#8E5198",
        MenuId = 9,
        pageName = "Shelf.Views.ShelfOutput"
      },
      new MenuModelItem()
      {
        ImageName = "search.png",
        Title = "RAF BUL",
        ColorCode = "#5D30F3",
        MenuId = 13
      },
      new MenuModelItem()
      {
        ImageName = "search.png",
        Title = "RAF BUL",
        ColorCode = "#5D30F3",
        MenuId = 10,
        HeaderMenuID = 13,
        pageName = "Shelf.Views.ShelfSearch2"
      },
      new MenuModelItem()
      {
        ImageName = "returnp.png",
        Title = "İADE",
        ColorCode = "#5D30F3",
        MenuId = 11,
        pageName = "Shelf.Views.Return"
      },
      new MenuModelItem()
      {
        ImageName = "pivot.png",
        Title = "PİVOTLA",
        ColorCode = "#000000",
        MenuId = 12,
        HeaderMenuID = 27,
        pageName = "Shelf.Views.ShelfSorting"
      },
      new MenuModelItem()
      {
        ImageName = "shelf.png",
        Title = "RAFTAKİ ÜRÜNLERİ BUL",
        ColorCode = "#DD8282",
        MenuId = 14,
        HeaderMenuID = 13,
        pageName = "Shelf.Views.ShelfItems"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "TRENDYOL İPTAL",
        ColorCode = "#8E5198",
        MenuId = 15,
        pageName = "Shelf.Views.TrendyolCancel"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "MAL KABUL",
        ColorCode = "#8E5198",
        MenuId = 16
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "MAL KABUL (İRSALİYE BAZLI)",
        ColorCode = "#8E5198",
        MenuId = 17,
        HeaderMenuID = 16,
        pageName = "Shelf.Views.AGWayBill"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "MAL KABUL (SİPARİŞ BAZLI)",
        ColorCode = "#8E5198",
        MenuId = 18,
        HeaderMenuID = 16,
        pageName = "Shelf.Views.AGAlcOrder"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "KOLİ ONAY",
        ColorCode = "#8E5198",
        MenuId = 19,
        HeaderMenuID = 16,
        pageName = "Shelf.Views.AGAlcOrderApprove"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "ÇOKLU SİPARİŞ ONAY",
        ColorCode = "#8E5198",
        MenuId = 20,
        HeaderMenuID = 16,
        pageName = "Shelf.Views.MultiOrderApprove"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "KARGO TESLİM",
        ColorCode = "#8E5198",
        MenuId = 21,
        HeaderMenuID = 16,
        pageName = "Shelf.Views.CargoDelivery"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "MAL KABUL(ÜRETİM)",
        ColorCode = "#8E5198",
        MenuId = 22,
        HeaderMenuID = 16,
        pageName = "Shelf.Views.ProductionAcceptance"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "ADRESE KOLİ TRANSFERİ",
        ColorCode = "#8E5198",
        MenuId = 23,
        HeaderMenuID = 16,
        pageName = "Shelf.Views.PackageToShelf"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "MAĞAZA TOPLAMA",
        ColorCode = "#8E5198",
        MenuId = 24,
        HeaderMenuID = 16,
        pageName = "Shelf.Views.StorePicking"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "ARAÇ YÜKLEME",
        ColorCode = "#8E5198",
        MenuId = 25,
        HeaderMenuID = 16,
        pageName = "Shelf.Views.Vehicle"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "KOLİ TAŞI",
        ColorCode = "#34CBC9",
        MenuId = 26,
        HeaderMenuID = 3,
        pageName = "Shelf.Views.MainShelfChange"
      },
      new MenuModelItem()
      {
        ImageName = "pivot.png",
        Title = "PİVOTLA",
        ColorCode = "#000000",
        MenuId = 27
      },
      new MenuModelItem()
      {
        ImageName = "pivot.png",
        Title = "PİVOTLA(MAĞAZALAR)",
        ColorCode = "#000000",
        MenuId = 28,
        HeaderMenuID = 27,
        pageName = "Shelf.Views.StoreSorting"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "KOLİ KAPAMA",
        ColorCode = "#8E5198",
        MenuId = 29,
        HeaderMenuID = 16,
        pageName = "Shelf.Views.Package"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "ADRESTEKİ KOLİLERİ TAŞI",
        ColorCode = "#34CBC9",
        MenuId = 30,
        HeaderMenuID = 3,
        pageName = "Shelf.Views.MainShelfChangeAdress"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "ATAMA BEKLEYEN EMİRLER",
        ColorCode = "#34CBC9",
        MenuId = 31,
        pageName = "Shelf.Views.PendingShelfOrder"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "BASKI MERKEZİ",
        ColorCode = "#34CBC9",
        MenuId = 32,
        pageName = "Shelf.Views.PrintCenter"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "PALET",
        ColorCode = "#8E5198",
        MenuId = 33,
        HeaderMenuID = 16,
        pageName = "Shelf.Views.Palette"
      },
      new MenuModelItem()
      {
        ImageName = "shelfoutput.png",
        Title = "HAVUZDAN İADE",
        ColorCode = "#34CBC9",
        MenuId = 34,
        HeaderMenuID = 3,
        pageName = "Shelf.Views.ReturnPool"
      }
    };

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (Login).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/Login.xaml",
        Instance = (object) this
      }))
        this.__InitComponentRuntime();
      else if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(((object) this).GetType()) != null)
      {
        this.__InitComponentRuntime();
      }
      else
      {
        Image image1 = new Image();
        Label label1 = new Label();
        StackLayout stackLayout1 = new StackLayout();
        Image image2 = new Image();
        Xamarin.Forms.Entry entry1 = new Xamarin.Forms.Entry();
        StackLayout stackLayout2 = new StackLayout();
        Image image3 = new Image();
        Xamarin.Forms.Entry entry2 = new Xamarin.Forms.Entry();
        StackLayout stackLayout3 = new StackLayout();
        Image image4 = new Image();
        Xamarin.Forms.Entry entry3 = new Xamarin.Forms.Entry();
        StackLayout stackLayout4 = new StackLayout();
        Label label2 = new Label();
        Switch @switch = new Switch();
        StackLayout stackLayout5 = new StackLayout();
        StackLayout stackLayout6 = new StackLayout();
        Button button1 = new Button();
        StackLayout stackLayout7 = new StackLayout();
        StackLayout stackLayout8 = new StackLayout();
        Label label3 = new Label();
        StackLayout stackLayout9 = new StackLayout();
        StackLayout stackLayout10 = new StackLayout();
        Login login;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (login = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) login, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("txtUserName", (object) entry1);
        if (((Element) entry1).StyleId == null)
          ((Element) entry1).StyleId = "txtUserName";
        ((INameScope) nameScope).RegisterName("txtPassword", (object) entry2);
        if (((Element) entry2).StyleId == null)
          ((Element) entry2).StyleId = "txtPassword";
        ((INameScope) nameScope).RegisterName("txtServer", (object) entry3);
        if (((Element) entry3).StyleId == null)
          ((Element) entry3).StyleId = "txtServer";
        ((INameScope) nameScope).RegisterName("chkRememberMe", (object) @switch);
        if (((Element) @switch).StyleId == null)
          ((Element) @switch).StyleId = "chkRememberMe";
        ((INameScope) nameScope).RegisterName("btnLogin", (object) button1);
        if (((Element) button1).StyleId == null)
          ((Element) button1).StyleId = "btnLogin";
        ((INameScope) nameScope).RegisterName("stckVersion", (object) stackLayout9);
        if (((Element) stackLayout9).StyleId == null)
          ((Element) stackLayout9).StyleId = "stckVersion";
        ((INameScope) nameScope).RegisterName("lblVersion", (object) label3);
        if (((Element) label3).StyleId == null)
          ((Element) label3).StyleId = "lblVersion";
        this.txtUserName = entry1;
        this.txtPassword = entry2;
        this.txtServer = entry3;
        this.chkRememberMe = @switch;
        this.btnLogin = button1;
        this.stckVersion = stackLayout9;
        this.lblVersion = label3;
        ((BindableObject) login).SetValue(Page.TitleProperty, (object) "Raf Takip");
        ((BindableObject) stackLayout10).SetValue(VisualElement.BackgroundColorProperty, (object) Color.White);
        ((BindableObject) stackLayout10).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) stackLayout1).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.Start);
        ((BindableObject) image1).SetValue(Image.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("lock.png"));
        ((BindableObject) image1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Center);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) image1);
        VisualDiagnostics.RegisterSourceInfo((object) image1, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 18);
        Label label4 = label1;
        BindableProperty fontSizeProperty1 = Label.FontSizeProperty;
        FontSizeConverter fontSizeConverter1 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 4];
        objArray1[0] = (object) label1;
        objArray1[1] = (object) stackLayout1;
        objArray1[2] = (object) stackLayout10;
        objArray1[3] = (object) login;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) Label.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (Login).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(11, 24)));
        object obj2 = ((IExtendedTypeConverter) fontSizeConverter1).ConvertFromInvariantString("Large", (IServiceProvider) xamlServiceProvider1);
        ((BindableObject) label4).SetValue(fontSizeProperty1, obj2);
        ((BindableObject) label1).SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) label1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) label1).SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((BindableObject) label1).SetValue(Label.TextProperty, (object) "Kullanıcı Girişi");
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) label1);
        VisualDiagnostics.RegisterSourceInfo((object) label1, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout10).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 9, 14);
        ((BindableObject) stackLayout8).SetValue(View.MarginProperty, (object) new Thickness(10.0, 0.0, 10.0, 0.0));
        ((BindableObject) stackLayout8).SetValue(Layout.PaddingProperty, (object) new Thickness(10.0, 0.0, 10.0, 0.0));
        ((BindableObject) stackLayout8).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Fill);
        ((BindableObject) stackLayout8).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) stackLayout6).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) image2).SetValue(Image.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("user.png"));
        ((BindableObject) image2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Start);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) image2);
        VisualDiagnostics.RegisterSourceInfo((object) image2, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 26);
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) entry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Kullanıcı Adı Giriniz");
        ((BindableObject) entry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) entry1);
        VisualDiagnostics.RegisterSourceInfo((object) entry1, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 21, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 19, 22);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) image3).SetValue(Image.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("password.png"));
        ((BindableObject) image3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Start);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) image3);
        VisualDiagnostics.RegisterSourceInfo((object) image3, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 24, 26);
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Şifre Giriniz");
        ((BindableObject) entry2).SetValue(Xamarin.Forms.Entry.IsPasswordProperty, (object) true);
        ((BindableObject) entry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) entry2);
        VisualDiagnostics.RegisterSourceInfo((object) entry2, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 25, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 22);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) image4).SetValue(Image.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("server.png"));
        ((BindableObject) image4).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Start);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) image4);
        VisualDiagnostics.RegisterSourceInfo((object) image4, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 26);
        ((BindableObject) entry3).SetValue(Xamarin.Forms.Entry.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) entry3).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Server Adresi Giriniz...");
        ((BindableObject) entry3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) entry3);
        VisualDiagnostics.RegisterSourceInfo((object) entry3, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 29, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 27, 22);
        ((BindableObject) stackLayout5).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout5).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) stackLayout5).SetValue(View.MarginProperty, (object) new Thickness(5.0));
        ((BindableObject) label2).SetValue(Label.TextProperty, (object) "Beni Hatırla");
        Label label5 = label2;
        BindableProperty fontSizeProperty2 = Label.FontSizeProperty;
        FontSizeConverter fontSizeConverter2 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 6];
        objArray2[0] = (object) label2;
        objArray2[1] = (object) stackLayout5;
        objArray2[2] = (object) stackLayout6;
        objArray2[3] = (object) stackLayout8;
        objArray2[4] = (object) stackLayout10;
        objArray2[5] = (object) login;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray2, (object) Label.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (Login).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(32, 52)));
        object obj4 = ((IExtendedTypeConverter) fontSizeConverter2).ConvertFromInvariantString("Medium", (IServiceProvider) xamlServiceProvider2);
        ((BindableObject) label5).SetValue(fontSizeProperty2, obj4);
        ((BindableObject) label2).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) label2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Start);
        ((BindableObject) label2).SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) label2);
        VisualDiagnostics.RegisterSourceInfo((object) label2, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 32, 26);
        ((BindableObject) @switch).SetValue(VisualElement.ScaleProperty, (object) 1.0);
        ((BindableObject) @switch).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.Start);
        ((BindableObject) @switch).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) @switch);
        VisualDiagnostics.RegisterSourceInfo((object) @switch, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 33, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 31, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 18, 18);
        ((BindableObject) stackLayout7).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) button1).SetValue(Button.TextProperty, (object) "Giriş");
        ((BindableObject) button1).SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        button1.Clicked += new EventHandler(login.btnLogin_Clicked);
        Button button2 = button1;
        BindableProperty fontSizeProperty3 = Button.FontSizeProperty;
        FontSizeConverter fontSizeConverter3 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
        Type type5 = typeof (IProvideValueTarget);
        object[] objArray3 = new object[0 + 5];
        objArray3[0] = (object) button1;
        objArray3[1] = (object) stackLayout7;
        objArray3[2] = (object) stackLayout8;
        objArray3[3] = (object) stackLayout10;
        objArray3[4] = (object) login;
        SimpleValueTargetProvider valueTargetProvider3;
        object obj5 = (object) (valueTargetProvider3 = new SimpleValueTargetProvider(objArray3, (object) Button.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider3.Add(type5, (object) valueTargetProvider3);
        xamlServiceProvider3.Add(typeof (IReferenceProvider), obj5);
        Type type6 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver3 = new XmlNamespaceResolver();
        namespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        XamlTypeResolver xamlTypeResolver3 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver3, typeof (Login).GetTypeInfo().Assembly);
        xamlServiceProvider3.Add(type6, (object) xamlTypeResolver3);
        xamlServiceProvider3.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(38, 29)));
        object obj6 = ((IExtendedTypeConverter) fontSizeConverter3).ConvertFromInvariantString("Medium", (IServiceProvider) xamlServiceProvider3);
        ((BindableObject) button2).SetValue(fontSizeProperty3, obj6);
        ((BindableObject) button1).SetValue(VisualElement.BackgroundColorProperty, (object) new Color(0.85490196943283081, 0.070588238537311554, 0.37254902720451355, 1.0));
        ((BindableObject) button1).SetValue(Button.TextColorProperty, (object) Color.White);
        ((BindableObject) button1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) button1);
        VisualDiagnostics.RegisterSourceInfo((object) button1, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 37, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 36, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout10).Children).Add((View) stackLayout8);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout8, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 14);
        ((BindableObject) stackLayout9).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.End);
        Label label6 = label3;
        BindableProperty fontSizeProperty4 = Label.FontSizeProperty;
        FontSizeConverter fontSizeConverter4 = new FontSizeConverter();
        XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
        Type type7 = typeof (IProvideValueTarget);
        object[] objArray4 = new object[0 + 4];
        objArray4[0] = (object) label3;
        objArray4[1] = (object) stackLayout9;
        objArray4[2] = (object) stackLayout10;
        objArray4[3] = (object) login;
        SimpleValueTargetProvider valueTargetProvider4;
        object obj7 = (object) (valueTargetProvider4 = new SimpleValueTargetProvider(objArray4, (object) Label.FontSizeProperty, (INameScope) nameScope));
        xamlServiceProvider4.Add(type7, (object) valueTargetProvider4);
        xamlServiceProvider4.Add(typeof (IReferenceProvider), obj7);
        Type type8 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver4 = new XmlNamespaceResolver();
        namespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        XamlTypeResolver xamlTypeResolver4 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver4, typeof (Login).GetTypeInfo().Assembly);
        xamlServiceProvider4.Add(type8, (object) xamlTypeResolver4);
        xamlServiceProvider4.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(42, 24)));
        object obj8 = ((IExtendedTypeConverter) fontSizeConverter4).ConvertFromInvariantString("Small", (IServiceProvider) xamlServiceProvider4);
        ((BindableObject) label6).SetValue(fontSizeProperty4, obj8);
        ((BindableObject) label3).SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
        ((BindableObject) label3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.Center);
        ((BindableObject) label3).SetValue(View.MarginProperty, (object) new Thickness(50.0));
        ((BindableObject) label3).SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
        ((ICollection<View>) ((Layout<View>) stackLayout9).Children).Add((View) label3);
        VisualDiagnostics.RegisterSourceInfo((object) label3, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 42, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout10).Children).Add((View) stackLayout9);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout9, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 41, 14);
        ((BindableObject) login).SetValue(ContentPage.ContentProperty, (object) stackLayout10);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout10, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 8, 10);
        VisualDiagnostics.RegisterSourceInfo((object) login, new Uri("Views\\Login.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<Login>(this, typeof (Login));
      this.txtUserName = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtUserName");
      this.txtPassword = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtPassword");
      this.txtServer = NameScopeExtensions.FindByName<Xamarin.Forms.Entry>((Element) this, "txtServer");
      this.chkRememberMe = NameScopeExtensions.FindByName<Switch>((Element) this, "chkRememberMe");
      this.btnLogin = NameScopeExtensions.FindByName<Button>((Element) this, "btnLogin");
      this.stckVersion = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckVersion");
      this.lblVersion = NameScopeExtensions.FindByName<Label>((Element) this, "lblVersion");
    }
  }
}
