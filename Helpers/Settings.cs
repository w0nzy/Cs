// Decompiled with JetBrains decompiler
// Type: Shelf.Helpers.Settings
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Shelf.Helpers
{
  public static class Settings
  {
    private const string SettingsKey = "settings_key";
    private static readonly string SettingsDefault = string.Empty;

    private static ISettings AppSettings => CrossSettings.Current;

    public static string GeneralSettings
    {
      get => Shelf.Helpers.Settings.AppSettings.GetValueOrDefault("settings_key", Shelf.Helpers.Settings.SettingsDefault, (string) null);
      set => Shelf.Helpers.Settings.AppSettings.AddOrUpdateValue("settings_key", value, (string) null);
    }

    public static string UserName
    {
      get => Shelf.Helpers.Settings.AppSettings.GetValueOrDefault("username", "", (string) null);
      set => Shelf.Helpers.Settings.AppSettings.AddOrUpdateValue("username", value, (string) null);
    }

    public static string Password
    {
      get => Shelf.Helpers.Settings.AppSettings.GetValueOrDefault("password", "", (string) null);
      set => Shelf.Helpers.Settings.AppSettings.AddOrUpdateValue("password", value, (string) null);
    }

    public static string Server
    {
      get => Shelf.Helpers.Settings.AppSettings.GetValueOrDefault("server", "", (string) null);
      set => Shelf.Helpers.Settings.AppSettings.AddOrUpdateValue("server", value, (string) null);
    }

    public static string NextVersion
    {
      get => Shelf.Helpers.Settings.AppSettings.GetValueOrDefault("nextversion", "", (string) null);
      set => Shelf.Helpers.Settings.AppSettings.AddOrUpdateValue("nextversion", value, (string) null);
    }

    public static string CurrentVersion
    {
      get => Shelf.Helpers.Settings.AppSettings.GetValueOrDefault("currentversion", "", (string) null);
      set => Shelf.Helpers.Settings.AppSettings.AddOrUpdateValue("currentversion", value, (string) null);
    }

    public static string MobilePrinter
    {
      get => Shelf.Helpers.Settings.AppSettings.GetValueOrDefault("mobileprinter", "", (string) null);
      set => Shelf.Helpers.Settings.AppSettings.AddOrUpdateValue("mobileprinter", value, (string) null);
    }

    public static string ProductSearchType
    {
      get => Shelf.Helpers.Settings.AppSettings.GetValueOrDefault("productsearchtype", "", (string) null);
      set => Shelf.Helpers.Settings.AppSettings.AddOrUpdateValue("productsearchtype", value, (string) null);
    }

    public static string PickingBarcodeType
    {
      get => Shelf.Helpers.Settings.AppSettings.GetValueOrDefault("pickingbarcodeype", "", (string) null);
      set => Shelf.Helpers.Settings.AppSettings.AddOrUpdateValue("pickingbarcodeype", value, (string) null);
    }
  }
}
