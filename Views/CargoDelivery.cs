// Decompiled with JetBrains decompiler
// Type: Shelf.Views.CargoDelivery
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
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;
using Xamarin.KeyboardHelper;
using XFNoSoftKeyboadEntryControl;

namespace Shelf.Views
{
  [XamlCompilation]
  [XamlFilePath("Views\\CargoDelivery.xaml")]
  public class CargoDelivery : ContentPage
  {
    public ztIOShelfCargoDelivery selectedCargoDelivery;
    public List<CargoDeliveryModel> deliveryList;
    private List<PickerItem> cargoCompanyList;
    private ToolbarItem tItem;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckContent;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckForm;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private SoftkeyboardDisabledEntry txtCargoDeliveryNumber;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private StackLayout stckCargoCompanies;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Picker pckCargoCompanies;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private Entry txtBarcode;
    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private ListView lstShelfDetail;

    public CargoDelivery()
    {
      this.InitializeComponent();
      ((Page) this).Title = "Kargo Teslim";
      this.LoadCargoDelivery();
      this.deliveryList = new List<CargoDeliveryModel>();
      ((ICollection<Effect>) ((Element) this.txtCargoDeliveryNumber).Effects).Add((Effect) new LongPressedEffect());
      LongPressedEffect.SetCommand((BindableObject) this.txtCargoDeliveryNumber, this.LongPressCargoDeliveryNumber);
      ToolbarItem toolbarItem = new ToolbarItem();
      ((MenuItem) toolbarItem).Text = "";
      this.tItem = toolbarItem;
      ((ICollection<ToolbarItem>) ((Page) this).ToolbarItems).Add(this.tItem);
    }

    private ICommand LongPressCargoDeliveryNumber => (ICommand) new Command((Action) (async () =>
    {
      CargoDelivery page = this;
      ReturnModel returnModel = GlobalMob.PostJson("GetCargoDeliveries", (ContentPage) page);
      if (!returnModel.Success)
        return;
      List<ztIOShelfCargoDelivery> shelfCargoDeliveryList = GlobalMob.JsonDeserialize<List<ztIOShelfCargoDelivery>>(returnModel.Result);
      ListView shelfListview = GlobalMob.GetShelfListview("CargoDeliveryNumber,DeliveryCompanyCode");
      ((ItemsView<Cell>) shelfListview).ItemsSource = (IEnumerable) shelfCargoDeliveryList;
      shelfListview.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(page.Select_SelectedItem);
      SelectItem selectItem = new SelectItem(shelfListview, "Kargo Teslim No Seçiniz");
      await ((NavigableElement) page).Navigation.PushAsync((Page) selectItem);
    }));

    private void Select_SelectedItem(object sender, SelectedItemChangedEventArgs e)
    {
      ztIOShelfCargoDelivery selectedItem = (ztIOShelfCargoDelivery) e.SelectedItem;
      ((InputView) this.txtCargoDeliveryNumber).Text = selectedItem.CargoDeliveryNumber;
      this.selectedCargoDelivery = selectedItem;
      ReturnModel returnModel = GlobalMob.PostJson("GetCargoDeliveryDetail?cargoDeliveryID=" + selectedItem.CargoDeliveryID.ToString(), (ContentPage) this);
      if (returnModel.Success)
      {
        string deliveryCompanyCode = "";
        List<pIOCargoDeliveryDetailReturnModel> detailReturnModelList = GlobalMob.JsonDeserialize<List<pIOCargoDeliveryDetailReturnModel>>(returnModel.Result);
        this.deliveryList = new List<CargoDeliveryModel>();
        foreach (pIOCargoDeliveryDetailReturnModel detailReturnModel in detailReturnModelList)
        {
          deliveryCompanyCode = detailReturnModel.DeliveryCompanyCode;
          this.deliveryList.Add(new CargoDeliveryModel()
          {
            CargoDeliveryDate = detailReturnModel.CreatedDate,
            CargoDeliveryDetailID = detailReturnModel.CargoDeliveryDetailID,
            CargoDeliveryID = detailReturnModel.CargoDeliveryID,
            CargoDeliveryNumber = detailReturnModel.CargoDeliveryNumber,
            DeliveryCompanyCode = detailReturnModel.DeliveryCompanyCode,
            DocumentNumber = detailReturnModel.DocumentNumber,
            UserName = detailReturnModel.CreatedUserName,
            PackageCount = Convert.ToInt32((object) detailReturnModel.PackageCount)
          });
        }
        this.pckCargoCompanies.SelectedItem = (object) this.cargoCompanyList.Where<PickerItem>((Func<PickerItem, bool>) (x => x.Description == deliveryCompanyCode.Trim())).FirstOrDefault<PickerItem>();
        this.RefreshData();
        this.BarcodeFocus();
      }
      ((NavigableElement) this).Navigation.PopAsync();
    }

    private void BarcodeFocus()
    {
      ((InputView) this.txtBarcode).Text = "";
      ((VisualElement) this.txtBarcode).Focus();
    }

    private void LoadCargoDelivery()
    {
      this.cargoCompanyList = new List<PickerItem>();
      ReturnModel returnModel = GlobalMob.PostJson("GetCargoCompanies", (ContentPage) this);
      if (!returnModel.Success)
        return;
      foreach (vwIOGetDeliveryCompany getDeliveryCompany in GlobalMob.JsonDeserialize<List<vwIOGetDeliveryCompany>>(returnModel.Result))
        this.cargoCompanyList.Add(new PickerItem()
        {
          Caption = getDeliveryCompany.DeliveryCompanyDescription,
          Description = getDeliveryCompany.DeliveryCompanyCode
        });
      this.pckCargoCompanies.ItemsSource = (IList) this.cargoCompanyList;
    }

    private async void txtBarcode_Completed(object sender, EventArgs e)
    {
      CargoDelivery page = this;
      PickerItem selectedCargo = (PickerItem) page.pckCargoCompanies.SelectedItem;
      if (selectedCargo == null)
      {
        GlobalMob.PlayError();
        int num = await ((Page) page).DisplayAlert("Hata", "Lütfen kargo firması seçiniz", "", "Tamam") ? 1 : 0;
        page.BarcodeFocus();
        selectedCargo = (PickerItem) null;
      }
      else
      {
        if (page.selectedCargoDelivery == null)
        {
          if (!await ((Page) page).DisplayAlert("Onay?", "Yeni Kargo Teslim No oluşturmak istiyor musunuz", "Evet", "Hayır"))
          {
            page.BarcodeFocus();
            selectedCargo = (PickerItem) null;
            return;
          }
          page.selectedCargoDelivery = new ztIOShelfCargoDelivery();
        }
        CargoDeliveryModel cargoDeliveryModel1 = new CargoDeliveryModel()
        {
          CargoDeliveryDate = DateTime.Now,
          CargoDeliveryID = page.selectedCargoDelivery.CargoDeliveryID,
          DeliveryCompanyCode = selectedCargo.Description,
          DocumentNumber = ((InputView) page.txtBarcode).Text,
          UserName = GlobalMob.User.UserName
        };
        Dictionary<string, string> paramList = new Dictionary<string, string>();
        string str = JsonConvert.SerializeObject((object) cargoDeliveryModel1);
        paramList.Add("json", str);
        ReturnModel result = GlobalMob.PostJson("SaveCargoDelivery", paramList, (ContentPage) page).Result;
        if (!result.Success)
        {
          selectedCargo = (PickerItem) null;
        }
        else
        {
          ReturnModel returnModel = JsonConvert.DeserializeObject<ReturnModel>(result.Result);
          if (!returnModel.Success)
          {
            GlobalMob.PlayError();
            int num = await ((Page) page).DisplayAlert("Hata", returnModel.ErrorMessage, "", "Tamam") ? 1 : 0;
            page.BarcodeFocus();
            selectedCargo = (PickerItem) null;
          }
          else
          {
            CargoDeliveryModel cargoDeliveryModel2 = JsonConvert.DeserializeObject<CargoDeliveryModel>(returnModel.Result);
            if (cargoDeliveryModel2 == null)
            {
              GlobalMob.PlayError();
              int num = await ((Page) page).DisplayAlert("Hata", "Döküman Numarası Bulunamadı", "", "Tamam") ? 1 : 0;
              page.BarcodeFocus();
              selectedCargo = (PickerItem) null;
            }
            else
            {
              ((InputView) page.txtCargoDeliveryNumber).Text = cargoDeliveryModel2.CargoDeliveryNumber;
              page.selectedCargoDelivery = new ztIOShelfCargoDelivery()
              {
                CargoDeliveryID = cargoDeliveryModel2.CargoDeliveryID,
                CargoDeliveryNumber = cargoDeliveryModel2.CargoDeliveryNumber
              };
              cargoDeliveryModel1.PackageCount = cargoDeliveryModel2.PackageCount;
              page.deliveryList.Add(cargoDeliveryModel1);
              GlobalMob.PlaySave();
              page.RefreshData();
              page.BarcodeFocus();
              selectedCargo = (PickerItem) null;
            }
          }
        }
      }
    }

    private void RefreshData()
    {
      ((VisualElement) this.lstShelfDetail).IsVisible = true;
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) null;
      ((ItemsView<Cell>) this.lstShelfDetail).ItemsSource = (IEnumerable) this.deliveryList.OrderBy<CargoDeliveryModel, int>((Func<CargoDeliveryModel, int>) (x => x.CargoDeliveryDetailID)).ToList<CargoDeliveryModel>();
      this.SetSumQty();
    }

    private void SetSumQty() => ((MenuItem) this.tItem).Text = Convert.ToString(this.deliveryList.Sum<CargoDeliveryModel>((Func<CargoDeliveryModel, int>) (x => x.PackageCount)));

    [GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
    private void InitializeComponent()
    {
      if (ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery()
      {
        AssemblyName = typeof (CargoDelivery).GetTypeInfo().Assembly.GetName(),
        ResourcePath = "Views/CargoDelivery.xaml",
        Instance = (object) this
      }))
        this.__InitComponentRuntime();
      else if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(((object) this).GetType()) != null)
      {
        this.__InitComponentRuntime();
      }
      else
      {
        SoftkeyboardDisabledEntry softkeyboardDisabledEntry = new SoftkeyboardDisabledEntry();
        StackLayout stackLayout1 = new StackLayout();
        BindingExtension bindingExtension1 = new BindingExtension();
        BindingExtension bindingExtension2 = new BindingExtension();
        Picker picker = new Picker();
        StackLayout stackLayout2 = new StackLayout();
        KeyboardEnableEffect keyboardEnableEffect = new KeyboardEnableEffect();
        Entry entry = new Entry();
        StackLayout stackLayout3 = new StackLayout();
        BindingExtension bindingExtension3 = new BindingExtension();
        DataTemplate dataTemplate1 = new DataTemplate();
        ListView listView = new ListView();
        StackLayout stackLayout4 = new StackLayout();
        StackLayout stackLayout5 = new StackLayout();
        StackLayout stackLayout6 = new StackLayout();
        StackLayout stackLayout7 = new StackLayout();
        CargoDelivery cargoDelivery;
        NameScope nameScope = (NameScope) (NameScope.GetNameScope((BindableObject) (cargoDelivery = this)) ?? (INameScope) new NameScope());
        NameScope.SetNameScope((BindableObject) cargoDelivery, (INameScope) nameScope);
        ((INameScope) nameScope).RegisterName("stckContent", (object) stackLayout7);
        if (((Element) stackLayout7).StyleId == null)
          ((Element) stackLayout7).StyleId = "stckContent";
        ((INameScope) nameScope).RegisterName("stckForm", (object) stackLayout6);
        if (((Element) stackLayout6).StyleId == null)
          ((Element) stackLayout6).StyleId = "stckForm";
        ((INameScope) nameScope).RegisterName("txtCargoDeliveryNumber", (object) softkeyboardDisabledEntry);
        if (((Element) softkeyboardDisabledEntry).StyleId == null)
          ((Element) softkeyboardDisabledEntry).StyleId = "txtCargoDeliveryNumber";
        ((INameScope) nameScope).RegisterName("stckCargoCompanies", (object) stackLayout2);
        if (((Element) stackLayout2).StyleId == null)
          ((Element) stackLayout2).StyleId = "stckCargoCompanies";
        ((INameScope) nameScope).RegisterName("pckCargoCompanies", (object) picker);
        if (((Element) picker).StyleId == null)
          ((Element) picker).StyleId = "pckCargoCompanies";
        ((INameScope) nameScope).RegisterName("txtBarcode", (object) entry);
        if (((Element) entry).StyleId == null)
          ((Element) entry).StyleId = "txtBarcode";
        ((INameScope) nameScope).RegisterName("lstShelfDetail", (object) listView);
        if (((Element) listView).StyleId == null)
          ((Element) listView).StyleId = "lstShelfDetail";
        this.stckContent = stackLayout7;
        this.stckForm = stackLayout6;
        this.txtCargoDeliveryNumber = softkeyboardDisabledEntry;
        this.stckCargoCompanies = stackLayout2;
        this.pckCargoCompanies = picker;
        this.txtBarcode = entry;
        this.lstShelfDetail = listView;
        ((BindableObject) stackLayout6).SetValue(Layout.PaddingProperty, (object) new Thickness(5.0));
        ((BindableObject) stackLayout1).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) softkeyboardDisabledEntry).SetValue(Entry.PlaceholderProperty, (object) "Kargo Mutabakat No");
        ((BindableObject) softkeyboardDisabledEntry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout1).Children).Add((View) softkeyboardDisabledEntry);
        VisualDiagnostics.RegisterSourceInfo((object) softkeyboardDisabledEntry, new Uri("Views\\CargoDelivery.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 15, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout1);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout1, new Uri("Views\\CargoDelivery.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 14, 22);
        ((BindableObject) picker).SetValue(Picker.TitleProperty, (object) "Kargo Firması Seçiniz");
        bindingExtension1.Path = "Caption";
        BindingBase bindingBase1 = ((IMarkupExtension<BindingBase>) bindingExtension1).ProvideValue((IServiceProvider) null);
        picker.ItemDisplayBinding = bindingBase1;
        VisualDiagnostics.RegisterSourceInfo((object) bindingBase1, new Uri("Views\\CargoDelivery.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 20, 33);
        bindingExtension2.Path = ".";
        BindingBase bindingBase2 = ((IMarkupExtension<BindingBase>) bindingExtension2).ProvideValue((IServiceProvider) null);
        ((BindableObject) picker).SetBinding(Picker.ItemsSourceProperty, bindingBase2);
        ((BindableObject) picker).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((ICollection<View>) ((Layout<View>) stackLayout2).Children).Add((View) picker);
        VisualDiagnostics.RegisterSourceInfo((object) picker, new Uri("Views\\CargoDelivery.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 19, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout2);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout2, new Uri("Views\\CargoDelivery.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 18, 22);
        ((BindableObject) stackLayout3).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        ((BindableObject) entry).SetValue(Entry.PlaceholderProperty, (object) "Barkod No Giriniz/Okutunuz");
        ((BindableObject) entry).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("True"));
        entry.Completed += new EventHandler(cargoDelivery.txtBarcode_Completed);
        ((BindableObject) entry).SetValue(View.HorizontalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) entry).SetValue(KeyboardEffect.EnableKeyboardProperty, (object) false);
        ((ICollection<Effect>) ((Element) entry).Effects).Add((Effect) keyboardEnableEffect);
        VisualDiagnostics.RegisterSourceInfo((object) keyboardEnableEffect, new Uri("Views\\CargoDelivery.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 28, 34);
        ((ICollection<View>) ((Layout<View>) stackLayout3).Children).Add((View) entry);
        VisualDiagnostics.RegisterSourceInfo((object) entry, new Uri("Views\\CargoDelivery.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 24, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout3);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout3, new Uri("Views\\CargoDelivery.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 23, 22);
        ((BindableObject) stackLayout4).SetValue(StackLayout.OrientationProperty, (object) (StackOrientation) 1);
        bindingExtension3.Path = ".";
        BindingBase bindingBase3 = ((IMarkupExtension<BindingBase>) bindingExtension3).ProvideValue((IServiceProvider) null);
        ((BindableObject) listView).SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase3);
        ((BindableObject) listView).SetValue(View.VerticalOptionsProperty, (object) LayoutOptions.FillAndExpand);
        ((BindableObject) listView).SetValue(ListView.HasUnevenRowsProperty, (object) true);
        ((BindableObject) listView).SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
        ((BindableObject) listView).SetValue(VisualElement.HeightRequestProperty, (object) 5000.0);
        ((BindableObject) listView).SetValue(ListView.SeparatorVisibilityProperty, (object) (SeparatorVisibility) 1);
        DataTemplate dataTemplate2 = dataTemplate1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CargoDelivery.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_9 xamlCdataTemplate9 = new CargoDelivery.\u003CInitializeComponent\u003E_anonXamlCDataTemplate_9();
        object[] objArray = new object[0 + 7];
        objArray[0] = (object) dataTemplate1;
        objArray[1] = (object) listView;
        objArray[2] = (object) stackLayout4;
        objArray[3] = (object) stackLayout5;
        objArray[4] = (object) stackLayout6;
        objArray[5] = (object) stackLayout7;
        objArray[6] = (object) cargoDelivery;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate9.parentValues = objArray;
        // ISSUE: reference to a compiler-generated field
        xamlCdataTemplate9.root = cargoDelivery;
        // ISSUE: reference to a compiler-generated method
        Func<object> func = new Func<object>(xamlCdataTemplate9.LoadDataTemplate);
        ((IDataTemplate) dataTemplate2).LoadTemplate = func;
        ((BindableObject) listView).SetValue(ItemsView<Cell>.ItemTemplateProperty, (object) dataTemplate1);
        VisualDiagnostics.RegisterSourceInfo((object) dataTemplate1, new Uri("Views\\CargoDelivery.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 37, 34);
        ((ICollection<View>) ((Layout<View>) stackLayout4).Children).Add((View) listView);
        VisualDiagnostics.RegisterSourceInfo((object) listView, new Uri("Views\\CargoDelivery.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 33, 26);
        ((ICollection<View>) ((Layout<View>) stackLayout5).Children).Add((View) stackLayout4);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout4, new Uri("Views\\CargoDelivery.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 32, 22);
        ((ICollection<View>) ((Layout<View>) stackLayout6).Children).Add((View) stackLayout5);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout5, new Uri("Views\\CargoDelivery.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 13, 18);
        ((ICollection<View>) ((Layout<View>) stackLayout7).Children).Add((View) stackLayout6);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout6, new Uri("Views\\CargoDelivery.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 12, 14);
        ((BindableObject) cargoDelivery).SetValue(ContentPage.ContentProperty, (object) stackLayout7);
        VisualDiagnostics.RegisterSourceInfo((object) stackLayout7, new Uri("Views\\CargoDelivery.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 11, 10);
        VisualDiagnostics.RegisterSourceInfo((object) cargoDelivery, new Uri("Views\\CargoDelivery.xaml" + ";assembly=" + "Shelf", UriKind.RelativeOrAbsolute), 2, 2);
      }
    }

    private void __InitComponentRuntime()
    {
      Extensions.LoadFromXaml<CargoDelivery>(this, typeof (CargoDelivery));
      this.stckContent = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckContent");
      this.stckForm = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckForm");
      this.txtCargoDeliveryNumber = NameScopeExtensions.FindByName<SoftkeyboardDisabledEntry>((Element) this, "txtCargoDeliveryNumber");
      this.stckCargoCompanies = NameScopeExtensions.FindByName<StackLayout>((Element) this, "stckCargoCompanies");
      this.pckCargoCompanies = NameScopeExtensions.FindByName<Picker>((Element) this, "pckCargoCompanies");
      this.txtBarcode = NameScopeExtensions.FindByName<Entry>((Element) this, "txtBarcode");
      this.lstShelfDetail = NameScopeExtensions.FindByName<ListView>((Element) this, "lstShelfDetail");
    }
  }
}
