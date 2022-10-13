// Decompiled with JetBrains decompiler
// Type: Shelf.Views.Vehicle
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using Newtonsoft.Json;
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
  [XamlFilePath("Views\\Vehicle.xaml")]
  public class Vehicle : ContentPage
  {
    private ToolbarItem tItem;
    private AllVehicleDefinitions definition;
    private ztIOShelfShippingPlanHeader selectedHeader;
    private List<ztIOShelfShippingPlanDetail> details;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ContentPage vehicle;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtPlanNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckInfo;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckShipmentType;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckCargoCompany;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckDriver;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckVehicle;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckRoundsman;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Button btnSave;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstPackage;

    public Color ButtonColor => Color.FromRgb(142, 81, 152);

    public Color TextColor => Color.White;

    public Vehicle()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Araç Yükleme";
      this.details = new List<ztIOShelfShippingPlanDetail>();
      ToolbarItem toolbarItem = new ToolbarItem();
      ((MenuItem) toolbarItem).Text = "";
      ((MenuItem) toolbarItem).Icon = FileImageSource.op_Implicit("history.png");
      this.tItem = toolbarItem;
      ((MenuItem) this.tItem).Clicked += new EventHandler(this.TItem_Clicked);
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(this.tItem);
      ((ICollection<Effect>) ((Element) this.txtPlanNumber).Effects).Add((Effect) new LongPressedEffect());
      LongPressedEffect.SetCommand((BindableObject) this.txtPlanNumber, this.LongPress);
      this.LoadComboFill();
    }

    protected virtual void OnAppearing()
    {
      ((Page) this).OnAppearing();
      ((NavigationPage) Application.Current.MainPage).BarBackgroundColor = this.ButtonColor;
    }

    private ICommand LongPress => (ICommand) new Command((Action) (async () =>
    {
      Vehicle page = this;
      ReturnModel returnModel = GlobalMob.PostJson("GetShippingPlanHeader?userID=" + GlobalMob.User.UserID.ToString(), (ContentPage) page);
      if (!returnModel.Success)
        return;
      List<pIOGetShippingPlanHeaderReturnModel> headerReturnModelList = GlobalMob.JsonDeserialize<List<pIOGetShippingPlanHeaderReturnModel>>(returnModel.Result);
      ListView shelfListview = GlobalMob.GetShelfListview("PlanNumber,Description");
      ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) headerReturnModelList;
      shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(page.Select_SelectedItem);
      SelectItem selectItem = new SelectItem(shelfListview, "Plan Seçiniz");
      await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
    }));

    private async void Select_SelectedItem(object sender, SelectedItemChangedEventArgs e)
    {
      Vehicle vehicle = this;
      pIOGetShippingPlanHeaderReturnModel item = (pIOGetShippingPlanHeaderReturnModel) e.SelectedItem;
      vehicle.selectedHeader = new ztIOShelfShippingPlanHeader()
      {
        PlanID = item.PlanID,
        PlanNumber = item.PlanNumber
      };
      ((InputView) vehicle.txtPlanNumber).Text = vehicle.selectedHeader.PlanNumber;
      ((VisualElement) vehicle.stckBarcode).IsVisible = true;
      PickerItem pickerItem1 = ((IEnumerable<PickerItem>) vehicle.pckDriver.ItemsSource).Where<PickerItem>((Func<PickerItem, bool>) (x => x.Description == item.DriverCode)).FirstOrDefault<PickerItem>();
      if (pickerItem1 != null)
        vehicle.pckDriver.SelectedItem = (object) pickerItem1;
      PickerItem pickerItem2 = ((IEnumerable<PickerItem>) vehicle.pckCargoCompany.ItemsSource).Where<PickerItem>((Func<PickerItem, bool>) (x => x.Description == item.DeliveryCompanyCode)).FirstOrDefault<PickerItem>();
      if (pickerItem2 != null)
        vehicle.pckCargoCompany.SelectedItem = (object) pickerItem2;
      PickerItem pickerItem3 = ((IEnumerable<PickerItem>) vehicle.pckRoundsman.ItemsSource).Where<PickerItem>((Func<PickerItem, bool>) (x => x.Description == item.RoundsmanCode)).FirstOrDefault<PickerItem>();
      if (pickerItem3 != null)
        vehicle.pckRoundsman.SelectedItem = (object) pickerItem3;
      PickerItem pickerItem4 = ((IEnumerable<PickerItem>) vehicle.pckShipmentType.ItemsSource).Where<PickerItem>((Func<PickerItem, bool>) (x => x.Description == item.ShipmentMethodCode)).FirstOrDefault<PickerItem>();
      if (pickerItem4 != null)
        vehicle.pckShipmentType.SelectedItem = (object) pickerItem4;
      PickerItem pickerItem5 = ((IEnumerable<PickerItem>) vehicle.pckVehicle.ItemsSource).Where<PickerItem>((Func<PickerItem, bool>) (x => x.Description == item.VehicleCode)).FirstOrDefault<PickerItem>();
      if (pickerItem5 != null)
        vehicle.pckVehicle.SelectedItem = (object) pickerItem5;
      ((VisualElement) vehicle.stckInfo).IsEnabled = false;
      ((VisualElement) vehicle.stckInfo).IsVisible = false;
      await NavigationExtension.PushPopupAsync(((NavigableElement) vehicle).Navigation, GlobalMob.ShowLoading(), true);
      vehicle.LoadDetails();
      GlobalMob.CloseLoading();
      Page page = await ((NavigableElement) vehicle).Navigation.PopAsync();
    }

    private void LoadDetails()
    {
      ReturnModel returnModel = GlobalMob.PostJson("GetShippingPlanDetail?planID=" + this.selectedHeader.PlanID.ToString(), (ContentPage) this);
      if (!returnModel.Success)
        return;
      foreach (pIOShelfPlanningDetailReturnModel detailReturnModel in GlobalMob.JsonDeserialize<List<pIOShelfPlanningDetailReturnModel>>(returnModel.Result))
        this.details.Add(new ztIOShelfShippingPlanDetail()
        {
          CreatedDate = detailReturnModel.CreatedDate,
          CreatedUserName = detailReturnModel.CreatedUserName,
          IsPostToV3 = detailReturnModel.IsPostToV3,
          PlanID = detailReturnModel.PlanID,
          PackageCode = detailReturnModel.PackageCode,
          PackageHeaderID = detailReturnModel.PackageHeaderID,
          PlanDetailID = detailReturnModel.PlanDetailID,
          ShippingNumber = detailReturnModel.ShippingNumber,
          UpdatedDate = detailReturnModel.UpdatedDate,
          UpdatedUserName = detailReturnModel.UpdatedUserName
        });
      this.RefreshDataSource();
      Device.BeginInvokeOnMainThread((Action) (async () =>
      {
        await Task.Delay(250);
        this.BarcodeFocus();
      }));
    }

    private void LoadComboFill()
    {
      ReturnModel returnModel = GlobalMob.PostJson("GetAllVehicleDefinitions", (ContentPage) this);
      if (!returnModel.Success)
        return;
      this.definition = JsonConvert.DeserializeObject<AllVehicleDefinitions>(returnModel.Result);
      this.CargoCompaniesFill();
      this.ShipmentMethodsFill();
      this.DriversFill();
      this.VehicleFill();
      this.RoundsMenFill();
    }

    private void VehicleFill()
    {
      if (this.definition.vehicles == null)
        return;
      List<PickerItem> pickerItemList = new List<PickerItem>();
      int num = 0;
      foreach (vwIOShelfVehicle vehicle in this.definition.vehicles)
      {
        pickerItemList.Add(new PickerItem()
        {
          Caption = vehicle.LicensePlate,
          Code = num,
          Description = vehicle.VehicleCode
        });
        ++num;
      }
      this.pckVehicle.ItemsSource = (IList) pickerItemList;
    }

    private void CargoCompaniesFill()
    {
      if (this.definition.deliveryCompanies == null)
        return;
      List<PickerItem> pickerItemList = new List<PickerItem>();
      int num = 0;
      foreach (vwIOShelfDeliverCompany deliveryCompany in this.definition.deliveryCompanies)
      {
        pickerItemList.Add(new PickerItem()
        {
          Caption = deliveryCompany.DeliveryCompanyDescription,
          Code = num,
          Description = deliveryCompany.DeliveryCompanyCode
        });
        ++num;
      }
      this.pckCargoCompany.ItemsSource = (IList) pickerItemList;
    }

    private void ShipmentMethodsFill()
    {
      if (this.definition.shipmentMethods == null)
        return;
      List<PickerItem> pickerItemList = new List<PickerItem>();
      int num = 0;
      foreach (vwIOShelfShipmentMethod shipmentMethod in this.definition.shipmentMethods)
      {
        pickerItemList.Add(new PickerItem()
        {
          Caption = shipmentMethod.ShipmentMethodDescription,
          Code = num,
          Description = shipmentMethod.ShipmentMethodCode
        });
        ++num;
      }
      this.pckShipmentType.ItemsSource = (IList) pickerItemList;
    }

    private void DriversFill()
    {
      if (this.definition.drivers == null)
        return;
      List<PickerItem> pickerItemList = new List<PickerItem>();
      int num = 0;
      foreach (vwIOShelfDriver driver in this.definition.drivers)
      {
        pickerItemList.Add(new PickerItem()
        {
          Caption = driver.FirstLastName,
          Code = num,
          Description = driver.DriverCode
        });
        ++num;
      }
      this.pckDriver.ItemsSource = (IList) pickerItemList;
    }

    private void RoundsMenFill()
    {
      if (this.definition.roundsMen == null)
        return;
      List<PickerItem> pickerItemList = new List<PickerItem>();
      int num = 0;
      foreach (vwIOGetRoundsMan roundsMan in this.definition.roundsMen)
      {
        pickerItemList.Add(new PickerItem()
        {
          Caption = roundsMan.FirstLastName,
          Code = num,
          Description = roundsMan.RoundsmanCode
        });
        ++num;
      }
      this.pckRoundsman.ItemsSource = (IList) pickerItemList;
    }

    private void TItem_Clicked(object sender, EventArgs e)
    {
      ((VisualElement) this.stckInfo).IsVisible = !((VisualElement) this.stckInfo).IsVisible;
      this.BarcodeFocus();
    }

    private async void txtBarcode_Completed(object sender, EventArgs e)
    {
      Vehicle page = this;
      string barcode = ((InputView) page.txtBarcode).Text;
      if (string.IsNullOrEmpty(barcode))
        return;
      if (page.selectedHeader == null)
      {
        GlobalMob.PlayError();
        int num = await ((Page) page).DisplayAlert("Hata", "Lütfen öncelikle plan seçiniz yada oluşturunuz", "", "Tamam") ? 1 : 0;
        page.BarcodeFocus();
      }
      else if (page.details.Where<ztIOShelfShippingPlanDetail>((Func<ztIOShelfShippingPlanDetail, bool>) (x => x.PackageCode == barcode)).Count<ztIOShelfShippingPlanDetail>() > 0)
      {
        GlobalMob.PlayError();
        int num = await ((Page) page).DisplayAlert("Hata", "Bu koli zaten eklenmiş", "", "Tamam") ? 1 : 0;
        page.BarcodeFocus();
      }
      else
      {
        ztIOShelfShippingPlanDetail shippingPlanDetail1 = new ztIOShelfShippingPlanDetail();
        shippingPlanDetail1.CreatedDate = DateTime.Now;
        shippingPlanDetail1.CreatedUserName = GlobalMob.User.UserName;
        shippingPlanDetail1.PackageCode = barcode;
        shippingPlanDetail1.PackageHeaderID = page.selectedHeader.PlanID;
        shippingPlanDetail1.PlanID = page.selectedHeader.PlanID;
        shippingPlanDetail1.UpdatedDate = DateTime.Now;
        shippingPlanDetail1.UpdatedUserName = GlobalMob.User.UserName;
        Dictionary<string, string> paramList = new Dictionary<string, string>();
        string str = JsonConvert.SerializeObject((object) shippingPlanDetail1);
        paramList.Add("json", str);
        ReturnModel result = GlobalMob.PostJson("SaveShippingPlanDetail", paramList, (ContentPage) page).Result;
        if (!result.Success)
          return;
        ztIOShelfShippingPlanDetail shippingPlanDetail2 = JsonConvert.DeserializeObject<ztIOShelfShippingPlanDetail>(result.Result);
        if (shippingPlanDetail2 != null)
        {
          page.details.Add(shippingPlanDetail2);
          page.RefreshDataSource();
          page.BarcodeFocus();
        }
        else
        {
          GlobalMob.PlayError();
          int num = await ((Page) page).DisplayAlert("Hata", "Koli bulunamadı", "", "Tamam") ? 1 : 0;
          page.BarcodeFocus();
        }
      }
    }

    private void RefreshDataSource()
    {
      ((ItemsView<Cell>) this.lstPackage).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstPackage).ItemsSource = (IEnumerable) this.details.OrderByDescending<ztIOShelfShippingPlanDetail, DateTime>((Func<ztIOShelfShippingPlanDetail, DateTime>) (x => x.CreatedDate));
    }

    private async void btnSave_Clicked(object sender, EventArgs e)
    {
      Vehicle page = this;
      PickerItem selectedItem1 = (PickerItem) page.pckShipmentType.SelectedItem;
      string str1 = "";
      if (selectedItem1 != null)
        str1 = Convert.ToString(selectedItem1.Description);
      PickerItem selectedItem2 = (PickerItem) page.pckCargoCompany.SelectedItem;
      string str2 = "";
      if (selectedItem2 != null)
        str2 = Convert.ToString(selectedItem2.Description);
      PickerItem selectedItem3 = (PickerItem) page.pckRoundsman.SelectedItem;
      string str3 = "";
      if (selectedItem3 != null)
        str3 = Convert.ToString(selectedItem3.Description);
      PickerItem selectedItem4 = (PickerItem) page.pckDriver.SelectedItem;
      string str4 = "";
      if (selectedItem4 != null)
        str4 = Convert.ToString(selectedItem4.Description);
      PickerItem selectedItem5 = (PickerItem) page.pckVehicle.SelectedItem;
      string str5 = "";
      if (selectedItem5 != null)
        str5 = Convert.ToString(selectedItem5.Description);
      if (str1 == "2" && string.IsNullOrEmpty(str2))
      {
        GlobalMob.PlayError();
        int num = await ((Page) page).DisplayAlert("Hata", "Lütfen Kargo Şirketi Seçiniz!", "", "Tamam") ? 1 : 0;
      }
      else
      {
        if (str1 == "1")
        {
          if (string.IsNullOrEmpty(str4))
          {
            GlobalMob.PlayError();
            int num = await ((Page) page).DisplayAlert("Hata", "Lütfen Sürücü Bilgilerini Seçiniz!", "", "Tamam") ? 1 : 0;
            return;
          }
          if (string.IsNullOrEmpty(str5))
          {
            GlobalMob.PlayError();
            int num = await ((Page) page).DisplayAlert("Hata", "Lütfen Araç Bilgilerini Seçiniz!", "", "Tamam") ? 1 : 0;
            return;
          }
        }
        ztIOShelfShippingPlanHeader shippingPlanHeader = new ztIOShelfShippingPlanHeader();
        shippingPlanHeader.ShipmentMethodCode = str1;
        shippingPlanHeader.DeliveryCompanyCode = str2;
        shippingPlanHeader.RoundsmanCode = str3;
        shippingPlanHeader.VehicleCode = str5;
        shippingPlanHeader.DriverCode = str4;
        shippingPlanHeader.IsShipmentForEachPackage = false;
        shippingPlanHeader.CreatedDate = DateTime.Now;
        shippingPlanHeader.CreatedUserName = GlobalMob.User.UserName;
        Dictionary<string, string> paramList = new Dictionary<string, string>();
        string str6 = JsonConvert.SerializeObject((object) shippingPlanHeader);
        paramList.Add("json", str6);
        ReturnModel result = GlobalMob.PostJson("SaveShippingPlanHeader", paramList, (ContentPage) page).Result;
        if (!result.Success)
          return;
        page.selectedHeader = JsonConvert.DeserializeObject<ztIOShelfShippingPlanHeader>(result.Result);
        if (page.selectedHeader == null)
          return;
        ((InputView) page.txtPlanNumber).Text = page.selectedHeader.PlanNumber;
        ((VisualElement) page.stckBarcode).IsVisible = true;
        ((VisualElement) page.stckInfo).IsEnabled = false;
        ((VisualElement) page.stckInfo).IsVisible = false;
        // ISSUE: reference to a compiler-generated method
        Device.BeginInvokeOnMainThread(new Action(page.\u003CbtnSave_Clicked\u003Eb__23_0));
      }
    }

    private void BarcodeFocus()
    {
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode).Focus();
    }

    private void pckShipmentType_SelectedIndexChanged(object sender, EventArgs e) => this.SetShipmentType();

    private void SetShipmentType()
    {
      PickerItem selectedItem = (PickerItem) this.pckShipmentType.SelectedItem;
      string str = "";
      if (selectedItem != null)
        str = Convert.ToString(selectedItem.Code);
      if (str == "2")
      {
        this.pckDriver.SelectedIndex = -1;
        this.pckVehicle.SelectedIndex = -1;
        this.pckRoundsman.SelectedIndex = -1;
        ((VisualElement) this.pckCargoCompany).IsVisible = false;
        ((VisualElement) this.pckDriver).IsVisible = true;
        ((VisualElement) this.pckVehicle).IsVisible = true;
        ((VisualElement) this.pckRoundsman).IsVisible = true;
      }
      else if (str == "1")
      {
        this.pckCargoCompany.SelectedIndex = -1;
        ((VisualElement) this.pckDriver).IsVisible = false;
        ((VisualElement) this.pckVehicle).IsVisible = false;
        ((VisualElement) this.pckRoundsman).IsVisible = false;
        ((VisualElement) this.pckCargoCompany).IsVisible = true;
      }
      else
      {
        this.pckCargoCompany.SelectedIndex = -1;
        ((VisualElement) this.pckDriver).IsVisible = true;
        ((VisualElement) this.pckVehicle).IsVisible = true;
        ((VisualElement) this.pckRoundsman).IsVisible = true;
        ((VisualElement) this.pckCargoCompany).IsVisible = false;
      }
    }

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (Vehicle).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/Vehicle.xaml",
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
        StackLayout stackLayout1 = new StackLayout();
        BindingExtension bindingExtension1 = new BindingExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        Picker picker1 = new Picker();
        StackLayout stackLayout2 = new StackLayout();
        BindingExtension bindingExtension3 = new BindingExtension();
        BindingExtension bindingExtension4 = new BindingExtension();
        Picker picker2 = new Picker();
        StackLayout stackLayout3 = new StackLayout();
        BindingExtension bindingExtension5 = new BindingExtension();
        BindingExtension bindingExtension6 = new BindingExtension();
        Picker picker3 = new Picker();
        StackLayout stackLayout4 = new StackLayout();
        BindingExtension bindingExtension7 = new BindingExtension();
        BindingExtension bindingExtension8 = new BindingExtension();
        Picker picker4 = new Picker();
        StackLayout stackLayout5 = new StackLayout();
        BindingExtension bindingExtension9 = new BindingExtension();
        BindingExtension bindingExtension10 = new BindingExtension();
        Picker picker5 = new Picker();
        StackLayout stackLayout6 = new StackLayout();
        ReferenceExtension referenceExtension1 = new ReferenceExtension();
        BindingExtension bindingExtension11 = new BindingExtension();
        ReferenceExtension referenceExtension2 = new ReferenceExtension();
        BindingExtension bindingExtension12 = new BindingExtension();
        Button button = new Button();
        StackLayout stackLayout7 = new StackLayout();
        StackLayout stackLayout8 = new StackLayout();
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry2 = new SoftkeyboardDisabledEntry();
        StackLayout stackLayout9 = new StackLayout();
        BindingExtension bindingExtension13 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout10 = new StackLayout();
        StackLayout stackLayout11 = new StackLayout();
        StackLayout stackLayout12 = new StackLayout();
        Vehicle vehicle;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (vehicle = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) vehicle, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("vehicle", (object) vehicle);
        if (((Element) vehicle).StyleId == null)
          ((Element) vehicle).StyleId = "vehicle";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout11);
        if (((Element) stackLayout11).StyleId == null)
          ((Element) stackLayout11).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtPlanNumber", (object) softkeyboardDisabledEntry1);
        if (((Element) softkeyboardDisabledEntry1).StyleId == null)
          ((Element) softkeyboardDisabledEntry1).StyleId = "txtPlanNumber";
        ((INameScope) nameScope).RegisterName("stckInfo", (object) stackLayout8);
        if (((Element) stackLayout8).StyleId == null)
          ((Element) stackLayout8).StyleId = "stckInfo";
        ((INameScope) nameScope).RegisterName("pckShipmentType", (object) picker1);
        if (((Element) picker1).StyleId == null)
          ((Element) picker1).StyleId = "pckShipmentType";
        ((INameScope) nameScope).RegisterName("pckCargoCompany", (object) picker2);
        if (((Element) picker2).StyleId == null)
          ((Element) picker2).StyleId = "pckCargoCompany";
        ((INameScope) nameScope).RegisterName("pckDriver", (object) picker3);
        if (((Element) picker3).StyleId == null)
          ((Element) picker3).StyleId = "pckDriver";
        ((INameScope) nameScope).RegisterName("pckVehicle", (object) picker4);
        if (((Element) picker4).StyleId == null)
          ((Element) picker4).StyleId = "pckVehicle";
        ((INameScope) nameScope).RegisterName("pckRoundsman", (object) picker5);
        if (((Element) picker5).StyleId == null)
          ((Element) picker5).StyleId = "pckRoundsman";
        ((INameScope) nameScope).RegisterName("btnSave", (object) button);
        if (((Element) button).StyleId == null)
          ((Element) button).StyleId = "btnSave";
        ((INameScope) nameScope).RegisterName("stckBarcode", (object) stackLayout10);
        if (((Element) stackLayout10).StyleId == null)
          ((Element) stackLayout10).StyleId = "stckBarcode";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) softkeyboardDisabledEntry2);
        if (((Element) softkeyboardDisabledEntry2).StyleId == null)
          ((Element) softkeyboardDisabledEntry2).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("lstPackage", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstPackage";
        this.vehicle = (ContentPage) vehicle;
        this.stckForm = stackLayout11;
        this.txtPlanNumber = softkeyboardDisabledEntry1;
        this.stckInfo = stackLayout8;
        this.pckShipmentType = picker1;
        this.pckCargoCompany = picker2;
        this.pckDriver = picker3;
        this.pckVehicle = picker4;
        this.pckRoundsman = picker5;
        this.btnSave = button;
        this.stckBarcode = stackLayout10;
        this.txtBarcode = softkeyboardDisabledEntry2;
        this.lstPackage = listView;
        ((BindableObject) stackLayout11).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 0);
        ((BindableObject) stackLayout11).SetValue(View.MarginProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) softkeyboardDisabledEntry1).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Plan Numarası Seçiniz");
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) softkeyboardDisabledEntry1);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry1, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout11).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((BindableObject) stackLayout2).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) picker1).SetValue(Picker.TitleProperty, (object) "Sevkiyat Tipi Seçiniz");
        bindingExtension1.Path = ".";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker1).SetBinding(Picker.ItemsSourceProperty, bindingBase1);
        picker1.SelectedIndexChanged += new EventHandler(vehicle.pckShipmentType_SelectedIndexChanged);
        bindingExtension2.Path = "Caption";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        picker1.ItemDisplayBinding = bindingBase2;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase2, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 37);
        ((BindableObject) picker1).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) picker1);
        VisualDiagnostics.RegisterSourceInfo((object) picker1, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 19, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 18, 22);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) picker2).SetValue(Picker.TitleProperty, (object) "Kargo Firması");
        bindingExtension3.Path = ".";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker2).SetBinding(Picker.ItemsSourceProperty, bindingBase3);
        bindingExtension4.Path = "Caption";
        BindingBase bindingBase4 = ((IMarkupExtension<BindingBase>) bindingExtension4).ProvideValue((IServiceProvider) null);
        picker2.ItemDisplayBinding = bindingBase4;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase4, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 24, 37);
        ((BindableObject) picker2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) picker2);
        VisualDiagnostics.RegisterSourceInfo((object) picker2, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 22, 22);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) picker3).SetValue(Picker.TitleProperty, (object) "Sürücü");
        bindingExtension5.Path = ".";
        BindingBase bindingBase5 = ((IMarkupExtension<BindingBase>) bindingExtension5).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker3).SetBinding(Picker.ItemsSourceProperty, bindingBase5);
        bindingExtension6.Path = "Caption";
        BindingBase bindingBase6 = ((IMarkupExtension<BindingBase>) bindingExtension6).ProvideValue((IServiceProvider) null);
        picker3.ItemDisplayBinding = bindingBase6;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase6, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 37);
        ((BindableObject) picker3).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) picker3);
        VisualDiagnostics.RegisterSourceInfo((object) picker3, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 27, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 26, 22);
        ((BindableObject) stackLayout5).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) picker4).SetValue(Picker.TitleProperty, (object) "Araç Bilgileri");
        bindingExtension7.Path = ".";
        BindingBase bindingBase7 = ((IMarkupExtension<BindingBase>) bindingExtension7).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker4).SetBinding(Picker.ItemsSourceProperty, bindingBase7);
        bindingExtension8.Path = "Caption";
        BindingBase bindingBase8 = ((IMarkupExtension<BindingBase>) bindingExtension8).ProvideValue((IServiceProvider) null);
        picker4.ItemDisplayBinding = bindingBase8;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase8, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 32, 37);
        ((BindableObject) picker4).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) picker4);
        VisualDiagnostics.RegisterSourceInfo((object) picker4, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 31, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 30, 22);
        ((BindableObject) stackLayout6).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) picker5).SetValue(Picker.TitleProperty, (object) "Teslimatçı");
        bindingExtension9.Path = ".";
        BindingBase bindingBase9 = ((IMarkupExtension<BindingBase>) bindingExtension9).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker5).SetBinding(Picker.ItemsSourceProperty, bindingBase9);
        bindingExtension10.Path = "Caption";
        BindingBase bindingBase10 = ((IMarkupExtension<BindingBase>) bindingExtension10).ProvideValue((IServiceProvider) null);
        picker5.ItemDisplayBinding = bindingBase10;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase10, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 36, 37);
        ((BindableObject) picker5).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) picker5);
        VisualDiagnostics.RegisterSourceInfo((object) picker5, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 35, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 34, 22);
        ((BindableObject) stackLayout7).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) button).SetValue(Button.TextProperty, (object) "Kaydet");
        referenceExtension1.Name = "vehicle";
        ReferenceExtension referenceExtension3 = referenceExtension1;
        XamlServiceProvider xamlServiceProvider1 = new XamlServiceProvider();
        Type type1 = typeof (IProvideValueTarget);
        object[] objArray1 = new object[0 + 7];
        objArray1[0] = (object) bindingExtension11;
        objArray1[1] = (object) button;
        objArray1[2] = (object) stackLayout7;
        objArray1[3] = (object) stackLayout8;
        objArray1[4] = (object) stackLayout11;
        objArray1[5] = (object) stackLayout12;
        objArray1[6] = (object) vehicle;
        SimpleValueTargetProvider valueTargetProvider1;
        object obj1 = (object) (valueTargetProvider1 = new SimpleValueTargetProvider(objArray1, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider1.Add(type1, (object) valueTargetProvider1);
        xamlServiceProvider1.Add(typeof (IReferenceProvider), obj1);
        Type type2 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver1 = new XmlNamespaceResolver();
        namespaceResolver1.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver1.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver1.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver1.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver1.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver1 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver1, typeof (Vehicle).GetTypeInfo().Assembly);
        xamlServiceProvider1.Add(type2, (object) xamlTypeResolver1);
        xamlServiceProvider1.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(39, 65)));
        object obj2 = ((IMarkupExtension) referenceExtension3).ProvideValue((IServiceProvider) xamlServiceProvider1);
        bindingExtension11.Source = obj2;
        VisualDiagnostics.RegisterSourceInfo(obj2, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 39, 65);
        bindingExtension11.Path = "ButtonColor";
        BindingBase bindingBase11 = ((IMarkupExtension<BindingBase>) bindingExtension11).ProvideValue((IServiceProvider) null);
        ((BindableObject) button).SetBinding(VisualElement.BackgroundColorProperty, bindingBase11);
        referenceExtension2.Name = "vehicle";
        ReferenceExtension referenceExtension4 = referenceExtension2;
        XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
        Type type3 = typeof (IProvideValueTarget);
        object[] objArray2 = new object[0 + 7];
        objArray2[0] = (object) bindingExtension12;
        objArray2[1] = (object) button;
        objArray2[2] = (object) stackLayout7;
        objArray2[3] = (object) stackLayout8;
        objArray2[4] = (object) stackLayout11;
        objArray2[5] = (object) stackLayout12;
        objArray2[6] = (object) vehicle;
        SimpleValueTargetProvider valueTargetProvider2;
        object obj3 = (object) (valueTargetProvider2 = new SimpleValueTargetProvider(objArray2, (object) typeof (BindingExtension).GetRuntimeProperty("Source"), (INameScope) nameScope));
        xamlServiceProvider2.Add(type3, (object) valueTargetProvider2);
        xamlServiceProvider2.Add(typeof (IReferenceProvider), obj3);
        Type type4 = typeof (IXamlTypeResolver);
        XmlNamespaceResolver namespaceResolver2 = new XmlNamespaceResolver();
        namespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
        namespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
        namespaceResolver2.Add("d", "http://xamarin.com/schemas/2014/forms/design");
        namespaceResolver2.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        namespaceResolver2.Add("local", "clr-namespace:XFNoSoftKeyboadEntryControl");
        XamlTypeResolver xamlTypeResolver2 = new XamlTypeResolver((IXmlNamespaceResolver) namespaceResolver2, typeof (Vehicle).GetTypeInfo().Assembly);
        xamlServiceProvider2.Add(type4, (object) xamlTypeResolver2);
        xamlServiceProvider2.Add(typeof (IXmlLineInfoProvider), (object) new XmlLineInfoProvider((IXmlLineInfo) new XmlLineInfo(39, 135)));
        object obj4 = ((IMarkupExtension) referenceExtension4).ProvideValue((IServiceProvider) xamlServiceProvider2);
        bindingExtension12.Source = obj4;
        VisualDiagnostics.RegisterSourceInfo(obj4, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 39, 135);
        bindingExtension12.Path = "TextColor";
        BindingBase bindingBase12 = ((IMarkupExtension<BindingBase>) bindingExtension12).ProvideValue((IServiceProvider) null);
        ((BindableObject) button).SetBinding(Button.TextColorProperty, bindingBase12);
        button.Clicked += new EventHandler(vehicle.btnSave_Clicked);
        ((BindableObject) button).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) button);
        VisualDiagnostics.RegisterSourceInfo((object) button, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 39, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout8).Children).Add((View) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 38, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout11).Children).Add((View) stackLayout8);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout8, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 17, 18);
        ((BindableObject) stackLayout10).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) stackLayout9).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) softkeyboardDisabledEntry2).SetValue(Xamarin.Forms.Entry.PlaceholderProperty, (object) "Koli Barkod Okutunuz");
        softkeyboardDisabledEntry2.Completed += new EventHandler(vehicle.txtBarcode_Completed);
        ((ICollection<View>) ((Layout<View>) stackLayout9).Children).Add((View) softkeyboardDisabledEntry2);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry2, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 45, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout10).Children).Add((View) stackLayout9);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout9, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 44, 22);
        ((BindableObject) listView).SetValue(ListView.RowHeightProperty, (object) 50);
        bindingExtension13.Path = ".";
        BindingBase bindingBase13 = ((IMarkupExtension<BindingBase>) bindingExtension13).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase13);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Vehicle.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_42 xamlCdataTemplate42 = new Vehicle.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_42();
        object[] objArray3 = new object[0 + 6];
        objArray3[0] = (object) dataTemplate1;
        objArray3[1] = (object) listView;
        objArray3[2] = (object) stackLayout10;
        objArray3[3] = (object) stackLayout11;
        objArray3[4] = (object) stackLayout12;
        objArray3[5] = (object) vehicle;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate42.parentValues = objArray3;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate42.root = vehicle;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate42.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 49, 30);
        ((ICollection<View>) ((Layout<View>) stackLayout10).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 47, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout11).Children).Add((View) stackLayout10);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout10, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 43, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout12).Children).Add((View) stackLayout11);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout11, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 14);
        ((BindableObject) vehicle).SetValue(ContentPage.ContentProperty, (object) stackLayout12);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout12, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 10);
        VisualDiagnostics.RegisterSourceInfo((object) vehicle, new Uri("Views\\Vehicle.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Xamarin.Forms.Xaml.Extensions.LoadFromXaml<Vehicle>(this, typeof (Vehicle));
      this.vehicle = NameScopeExtensions.FindByName<ContentPage>((Element) this, "vehicle");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtPlanNumber = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtPlanNumber");
      this.stckInfo = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckInfo");
      this.pckShipmentType = NameScopeExtensions.FindByName<Picker>((Element) this, "pckShipmentType");
      this.pckCargoCompany = NameScopeExtensions.FindByName<Picker>((Element) this, "pckCargoCompany");
      this.pckDriver = NameScopeExtensions.FindByName<Picker>((Element) this, "pckDriver");
      this.pckVehicle = NameScopeExtensions.FindByName<Picker>((Element) this, "pckVehicle");
      this.pckRoundsman = NameScopeExtensions.FindByName<Picker>((Element) this, "pckRoundsman");
      this.btnSave = NameScopeExtensions.FindByName<Button>((Element) this, "btnSave");
      this.stckBarcode = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckBarcode");
      this.txtBarcode = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtBarcode");
      this.lstPackage = NameScopeExtensions.FindByName<ListView>((Element) this, "lstPackage");
    }
  }
}
