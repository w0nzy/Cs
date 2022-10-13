// Decompiled with JetBrains decompiler
// Type: Shelf.Views.Main
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using Newtonsoft.Json;
using Shelf.Manager;
using Shelf.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace Shelf.Views
{
  [XamlCompilation]
  [XamlFilePath("Views\\Main.xaml")]
  public class Main : ContentPage
  {
    private List<MenuModelItem> menuList;
    private List<Label> CounterLabelList;
    private bool _canClose = true;
    private bool isClick;

    public Color ButtonColor { get; set; }

    public Color TextColor => Color.White;

    public Main(List<MenuModelItem> fmenuList)
    {
      this.InitializeComponent();
      this.ButtonColor = Color.FromRgb(218, 18, 95);
      this.menuList = fmenuList;
      if (fmenuList.Where<MenuModelItem>((Func<MenuModelItem, bool>) (x => x.HeaderMenuID > 0)).Count<MenuModelItem>() != fmenuList.Count<MenuModelItem>())
        fmenuList = fmenuList.Where<MenuModelItem>((Func<MenuModelItem, bool>) (x => x.HeaderMenuID <= 0)).ToList<MenuModelItem>();
      ((VisualElement) this).BackgroundColor = Color.GhostWhite;
      ((Page) this).Title = "Raf Takip";
      if (fmenuList.Where<MenuModelItem>((Func<MenuModelItem, bool>) (x => x.HeaderMenuID <= 0)).Count<MenuModelItem>() > 0)
      {
        ToolbarItem toolbarItem = new ToolbarItem();
        ((MenuItem) toolbarItem).Text = GlobalMob.User.UserName;
        ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(toolbarItem);
      }
      this.CounterLabelList = new List<Label>();
      if (GlobalMob.User != null)
      {
        if (string.IsNullOrEmpty(GlobalMob.User.MenuIds))
          GlobalMob.User.MenuIds = ",1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,";
        string menuIds = GlobalMob.User.MenuIds;
        fmenuList = fmenuList.Where<MenuModelItem>((Func<MenuModelItem, bool>) (x => menuIds.Contains("," + x.MenuId.ToString() + ","))).ToList<MenuModelItem>();
      }
      int num1 = 2;
      int num2 = fmenuList.Count<MenuModelItem>() / num1;
      Grid grid = new Grid();
      ((VisualElement) grid).BackgroundColor = Color.Black;
      ((View) grid).Margin = Thickness.op_Implicit(10.0);
      for (int index = 0; index < num1; ++index)
        ((DefinitionCollection<ColumnDefinition>) grid.ColumnDefinitions).Add(new ColumnDefinition()
        {
          Width = new GridLength(1.0, (GridUnitType) 1)
        });
      for (int index = 0; index < num2; ++index)
        ((DefinitionCollection<RowDefinition>) grid.RowDefinitions).Add(new RowDefinition()
        {
          Height = new GridLength(1.0, (GridUnitType) 1)
        });
      int num3 = 0;
      int num4 = 0;
      int num5 = 0;
      foreach (MenuModelItem fmenu in fmenuList)
      {
        ++num5;
        StackLayout stackLayout = this.AddMenuItem(fmenu.ImageName, fmenu.Title, fmenu.ColorCode, fmenu.MenuId);
        grid.Children.Add((View) stackLayout, num3, num4);
        if (num5 == fmenuList.Count<MenuModelItem>() && num3 < num1)
          Grid.SetColumnSpan((BindableObject) stackLayout, num1 - num3);
        if (num3 == num1 - 1)
        {
          ++num4;
          num3 = 0;
        }
        else
          ++num3;
      }
      this.Content = (View) grid;
      this.RefreshCounters();
      Device.StartTimer(TimeSpan.FromSeconds(15.0), (Func<bool>) (() =>
      {
        this.RefreshCounters();
        return true;
      }));
    }

    private void RefreshCounters()
    {
      try
      {
        ReturnModel returnModel = GlobalMob.PostJson(string.Format("GetPendingJobs?userID={0}", (object) GlobalMob.User.UserID), (ContentPage) null);
        if (!returnModel.Success)
          return;
        List<pIOOrderPendingJobsReturnModel> pendingJobsReturnModelList = JsonConvert.DeserializeObject<List<pIOOrderPendingJobsReturnModel>>(returnModel.Result);
        if (pendingJobsReturnModelList.Count <= 0)
          return;
        Label counterLabel1 = this.CounterLabelList[0];
        int? orderCount = pendingJobsReturnModelList[0].OrderCount;
        int num1 = 0;
        string str1 = orderCount.GetValueOrDefault() > num1 & orderCount.HasValue ? Convert.ToString((object) pendingJobsReturnModelList[0].OrderCount) : "";
        counterLabel1.Text = str1;
        Label counterLabel2 = this.CounterLabelList[1];
        int? basketCount = pendingJobsReturnModelList[0].BasketCount;
        int num2 = 0;
        string str2 = basketCount.GetValueOrDefault() > num2 & basketCount.HasValue ? Convert.ToString((object) pendingJobsReturnModelList[0].BasketCount) : "";
        counterLabel2.Text = str2;
        Label counterLabel3 = this.CounterLabelList[2];
        int? purchaseCount = pendingJobsReturnModelList[0].PurchaseCount;
        int num3 = 0;
        string str3 = purchaseCount.GetValueOrDefault() > num3 & purchaseCount.HasValue ? Convert.ToString((object) pendingJobsReturnModelList[0].PurchaseCount) : "";
        counterLabel3.Text = str3;
      }
      catch (Exception ex)
      {
      }
    }

    private StackLayout AddMenuItem(
      string image,
      string title,
      string colorCode,
      int menuId)
    {
      StackLayout stackLayout1 = new StackLayout();
      ((VisualElement) stackLayout1).BackgroundColor = Color.White;
      ((View) stackLayout1).VerticalOptions = LayoutOptions.FillAndExpand;
      StackLayout stackLayout2 = new StackLayout();
      ((View) stackLayout2).VerticalOptions = LayoutOptions.CenterAndExpand;
      ((View) stackLayout2).HorizontalOptions = LayoutOptions.CenterAndExpand;
      Grid grid = new Grid();
      ((Element) grid).ClassId = "frame" + menuId.ToString();
      ((View) grid).VerticalOptions = LayoutOptions.Center;
      ((DefinitionCollection<ColumnDefinition>) grid.ColumnDefinitions).Add(new ColumnDefinition()
      {
        Width = new GridLength(1.0, (GridUnitType) 1)
      });
      ((DefinitionCollection<RowDefinition>) grid.RowDefinitions).Add(new RowDefinition()
      {
        Height = new GridLength(1.0, (GridUnitType) 1)
      });
      Frame frame = new Frame();
      ((VisualElement) frame).BackgroundColor = Color.FromHex(colorCode);
      frame.HasShadow = false;
      frame.CornerRadius = 50f;
      ((VisualElement) frame).HeightRequest = 50.0;
      ((VisualElement) frame).WidthRequest = 50.0;
      ((View) frame).HorizontalOptions = LayoutOptions.CenterAndExpand;
      ((View) frame).VerticalOptions = LayoutOptions.Center;
      StackLayout stackLayout3 = new StackLayout();
      ((View) stackLayout3).VerticalOptions = LayoutOptions.Center;
      Image image1 = new Image();
      image1.Source = ImageSource.op_Implicit(image);
      ((View) image1).HorizontalOptions = LayoutOptions.Center;
      ((View) image1).VerticalOptions = LayoutOptions.Start;
      ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) image1);
      ((ContentView) frame).Content = (View) stackLayout3;
      Label label1 = new Label();
      label1.Text = "";
      ((Element) label1).ClassId = "counter" + Convert.ToString(menuId);
      ((View) label1).Margin = new Thickness(100.0, 0.0, 0.0, 0.0);
      label1.TextColor = Color.FromHex(colorCode);
      ((View) label1).VerticalOptions = LayoutOptions.Start;
      ((View) label1).HorizontalOptions = LayoutOptions.Center;
      label1.FontAttributes = (FontAttributes) 1;
      label1.HorizontalTextAlignment = (TextAlignment) 1;
      label1.VerticalTextAlignment = (TextAlignment) 2;
      label1.FontSize = 20.0;
      if (menuId <= 3)
        this.CounterLabelList.Add(label1);
      grid.Children.Add((View) frame, 0, 0);
      grid.Children.Add((View) label1, 0, 0);
      Label label2 = new Label();
      label2.Text = title;
      ((View) label2).Margin = Thickness.op_Implicit(0.0);
      label2.TextColor = Color.Black;
      ((View) label2).VerticalOptions = LayoutOptions.Center;
      ((View) label2).HorizontalOptions = LayoutOptions.Center;
      label2.FontAttributes = (FontAttributes) 1;
      label2.HorizontalTextAlignment = (TextAlignment) 0;
      label2.VerticalTextAlignment = (TextAlignment) 2;
      ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) grid);
      ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) label2);
      ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) stackLayout2);
      TapGestureRecognizer gestureRecognizer = new TapGestureRecognizer();
      gestureRecognizer.CommandParameter = (object) menuId;
      gestureRecognizer.Tapped += (EventHandler) ((s, e) => this.OnTapped(s, e));
      ((ICollection<IGestureRecognizer>) ((View) stackLayout1).GestureRecognizers).Add((IGestureRecognizer) gestureRecognizer);
      return stackLayout1;
    }

    protected virtual bool OnBackButtonPressed()
    {
      if (this._canClose)
        this.ShowExitDialog();
      return this._canClose;
    }

    private async void ShowExitDialog()
    {
      Main main = this;
      if (!await ((Page) main).DisplayAlert("Çıkış?", "Programdan çıkmak istiyor musunuz?", "Evet", "Hayır"))
        return;
      GlobalMob.Exit();
      main._canClose = false;
      ((Page) main).OnBackButtonPressed();
    }

    public async void BackButtonPressed(bool isExit)
    {
      isExit = await ((Page) this).DisplayAlert("Çıkış?", "Programdan çıkmak istiyor musunuz?", "Evet", "Hayır");
      if (!isExit)
        return;
      GlobalMob.Exit();
    }

    private async void OnTapped(object sender, EventArgs e) => await ((NavigableElement) this).Navigation.PushModalAsync((Page) new ShelfOutput());

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (Main).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/Main.xaml",
        Instance = (object) this
      }))
        this.__InitComponentRuntime();
      else if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(((object) this).GetType()) != null)
      {
        this.__InitComponentRuntime();
      }
      else
      {
        Label label = new Label();
        StackLayout stackLayout = new StackLayout();
        Main main;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (main = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) main, (INameScope) nameScope);
        ((BindableObject) label).SetValue(Label.TextProperty, (object) "Welcome to Xamarin.Forms!");
        ((BindableObject) label).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.CenterAndExpand);
        ((BindableObject) label).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.CenterAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout).Children).Add((View) label);
        VisualDiagnostics.RegisterSourceInfo((object) label, new Uri("Views\\Main.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 10, 14);
        ((BindableObject) main).SetValue(ContentPage.ContentProperty, (object) stackLayout);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout, new Uri("Views\\Main.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 9, 10);
        VisualDiagnostics.RegisterSourceInfo((object) main, new Uri("Views\\Main.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime() => Extensions.LoadFromXaml<Main>(this, typeof (Main));
  }
}
