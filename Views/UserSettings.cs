// Decompiled with JetBrains decompiler
// Type: Shelf.Views.UserSettings
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using Newtonsoft.Json;
using Shelf.Helpers;
using Shelf.Manager;
using Shelf.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections;
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
  [XamlFilePath("Views\\UserSettings.xaml")]
  public class UserSettings : ContentPage
  {
    private bool isFormLoad;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage usersettings;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckWarehouse;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckBluetooth;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnSelectDevice;

    public Color ButtonColor => Color.FromRgb(3, 10, 53);

    public Color TextColor => Color.White;

    public UserSettings()
    {
      this.InitializeComponent();
      this.isFormLoad = true;
      ((Page) this).Title = "Ayarlar";
      if (GlobalMob.User.IsBluetoothPrinter)
      {
        ((VisualElement) this.stckBluetooth).IsVisible = true;
        this.SetBluetoothButtonText();
      }
      this.FillWarehouse();
      this.isFormLoad = false;
    }

    private void SetBluetoothButtonText()
    {
      this.btnSelectDevice.Text = "Bluetooth Yazıcı Seç";
      if (string.IsNullOrEmpty(Settings.MobilePrinter))
        return;
      this.btnSelectDevice.Text = "Bluetooth Yazıcı Seç(" + Settings.MobilePrinter + ")";
    }

    private void FillWarehouse()
    {
      List<PickerItem> pickerItemList = new List<PickerItem>();
      PickerItem pickerItem1 = (PickerItem) null;
      ReturnModel returnModel = GlobalMob.PostJson("GetUserWarehouse?userName=" + GlobalMob.User.UserName, (ContentPage) this);
      if (returnModel.Success)
      {
        foreach (pIOGetUserWarehouseReturnModel warehouseReturnModel in JsonConvert.DeserializeObject<List<pIOGetUserWarehouseReturnModel>>(returnModel.Result))
        {
          PickerItem pickerItem2 = new PickerItem()
          {
            Caption = warehouseReturnModel.WarehouseCode,
            Description = warehouseReturnModel.WarehouseCode
          };
          if (GlobalMob.User.WarehouseCode == warehouseReturnModel.WarehouseCode)
            pickerItem1 = pickerItem2;
          pickerItemList.Add(pickerItem2);
        }
      }
      this.pckWarehouse.ItemsSource = (IList) pickerItemList;
      this.pckWarehouse.SelectedItem = (object) pickerItem1;
    }

    private async void btnSelectDevice_Clicked(object sender, EventArgs e)
    {
      UserSettings userSettings = this;
      ListView cnt = new ListView();
      ((ItemsView<Cell>) cnt).ItemsSource = (IEnumerable) GlobalMob.DeviceList();
      cnt.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(userSettings.LstDevice_ItemSelected);
      SelectItem selectItem = new SelectItem(cnt, "Bluetooth Yazıcılar");
      await ((NavigableElement) userSettings).Navigation.PushAsync((Page) selectItem);
    }

    private void LstDevice_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      Settings.MobilePrinter = Convert.ToString(e.SelectedItem);
      this.SetBluetoothButtonText();
      ((NavigableElement) this).Navigation.PopAsync();
    }

    private void pckWarehouse_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.isFormLoad)
        return;
      PickerItem selectedItem = (PickerItem) this.pckWarehouse.SelectedItem;
      ReturnModel returnModel = GlobalMob.PostJson("UpdateUserWarehouse?userID=" + GlobalMob.User.UserID.ToString() + "&warehouseCode=" + selectedItem.Caption, (ContentPage) this);
      if (!returnModel.Success || !JsonConvert.DeserializeObject<bool>(returnModel.Result))
        return;
      GlobalMob.User.WarehouseCode = selectedItem.Caption;
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (UserSettings).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/UserSettings.xaml",
        Instance = (object) this
      }))
        this.__InitComponentRuntime();
      else if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(((object) this).GetType()) != null)
      {
        this.__InitComponentRuntime();
      }
      else
      {
        BindingExtension bindingExtension1 = new BindingExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        Picker picker = new Picker();
        StackLayout stackLayout1 = new StackLayout();
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension3 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension4 = new BindingExtension();
        Button button = new Button();
        StackLayout stackLayout2 = new StackLayout();
        StackLayout stackLayout3 = new StackLayout();
        UserSettings userSettings;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (userSettings = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) userSettings, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("usersettings", (object) userSettings);
        if (((Element) userSettings).StyleId == null)
          ((Element) userSettings).StyleId = "usersettings";
        ((INameScope) nameScope).RegisterName("pckWarehouse", (object) picker);
        if (((Element) picker).StyleId == null)
          ((Element) picker).StyleId = "pckWarehouse";
        ((INameScope) nameScope).RegisterName("stckBluetooth", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckBluetooth";
        ((INameScope) nameScope).RegisterName("btnSelectDevice", (object) button);
        if (((Element) button).StyleId == null)
          ((Element) button).StyleId = "btnSelectDevice";
        this.usersettings = (ContentPage) userSettings;
        this.pckWarehouse = picker;
        this.stckBluetooth = stackLayout2;
        this.btnSelectDevice = button;
        ((BindableObject) stackLayout3).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        bindingExtension1.Path = "Caption";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        picker.ItemDisplayBinding = bindingBase1;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase1, new Uri("Views\\UserSettings.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 9, 48);
        ((BindableObject) picker).SetValue(Picker.TitleProperty, (object) "Depo Kodu Seçiniz");
        picker.SelectedIndexChanged += new EventHandler(userSettings.pckWarehouse_SelectedIndexChanged);
        bindingExtension2.Path = ".";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker).SetBinding(Picker.ItemsSourceProperty, bindingBase2);
        ((BindableObject) picker).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) picker);
        VisualDiagnostics.RegisterSourceInfo((object) picker, new Uri("Views\\UserSettings.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 9, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\UserSettings.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 8, 14);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) stackLayout2).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
        ((BindableObject) button).SetValue(Button.TextProperty, (object) "Bluetooth Yazıcı Seç");
        button.Clicked += new EventHandler(userSettings.btnSelectDevice_Clicked);
        referenceExtension1.Name = "usersettings";
        ReferenceExtension referenceExtension3 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 5];
        objArray1[0] = (object) bindingExtension3;
        objArray1[1] = (object) button;
        objArray1[2] = (object) stackLayout2;
        objArray1[3] = (object) stackLayout3;
        objArray1[4] = (object) userSettings;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (UserSettings).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(15, 21)));
        object obj2 = ((IMarkupExtension) referenceExtension3).ProvideValue((IServiceProvider) xamlServiceProvider1);
        bindingExtension3.Source = obj2;
        VisualDiagnostics.RegisterSourceInfo(obj2, new Uri("Views\\UserSettings.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 21);
        bindingExtension3.Path = "ButtonColor";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) button).SetBinding(VisualElement.BackgroundColorProperty, bindingBase3);
        ((BindableObject) button).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        referenceExtension2.Name = "usersettings";
        ReferenceExtension referenceExtension4 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 5];
        objArray2[0] = (object) bindingExtension4;
        objArray2[1] = (object) button;
        objArray2[2] = (object) stackLayout2;
        objArray2[3] = (object) stackLayout3;
        objArray2[4] = (object) userSettings;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray2, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (UserSettings).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(16, 25)));
        object obj4 = ((IMarkupExtension) referenceExtension4).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension4.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\UserSettings.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 16, 25);
        bindingExtension4.Path = "TextColor";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        ((BindableObject) button).SetBinding(Button.TextColorProperty, bindingBase4);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) button);
        VisualDiagnostics.RegisterSourceInfo((object) button, new Uri("Views\\UserSettings.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\UserSettings.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 14);
        ((BindableObject) userSettings).SetValue(ContentPage.ContentProperty, (object) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\UserSettings.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 7, 10);
        VisualDiagnostics.RegisterSourceInfo((object) userSettings, new Uri("Views\\UserSettings.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Extensions.LoadFromXaml<UserSettings>(this, typeof (UserSettings));
      this.usersettings = NameScopeExtensions.FindByName<ContentPage>((Element) this, "usersettings");
      this.pckWarehouse = NameScopeExtensions.FindByName<Picker>((Element) this, "pckWarehouse");
      this.stckBluetooth = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBluetooth");
      this.btnSelectDevice = NameScopeExtensions.FindByName<Button>((Element) this, "btnSelectDevice");
    }
  }
}
