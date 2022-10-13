using Android.App;
using Android.Runtime;
using AndroidAppNamespace.Effects;
using System.Reflection;
using System.Runtime.InteropServices;
using Xam.Plugin.AutoUpdate.Droid;
using Xamarin.Forms;
using XFNoSoftKeyboadEntryControl;
using XFNoSoftKeyboadEntryControl.Droid;

[assembly: ResolutionGroupName("MyApp")]
[assembly: ExportEffect(typeof (AndroidLongPressedEffect), "LongPressedEffect")]
[assembly: Dependency(typeof (FileOpener))]
[assembly: Dependency(typeof (NativeHelper))]
[assembly: ResourceDesigner("Shelf.Droid.Resource", IsApplication = true)]
[assembly: AssemblyTitle("Shelf.Android")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Shelf.Android")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyTrademark("")]
[assembly: ComVisible(false)]
[assembly: AssemblyFileVersion("2021.03.19")]
[assembly: UsesPermission("android.permission.INTERNET")]
[assembly: UsesPermission("android.permission.WRITE_EXTERNAL_STORAGE")]
[assembly: ExportRenderer(typeof (SoftkeyboardDisabledEntry), typeof (SoftkeyboardDisabledEntryRenderer))]
[assembly: AssemblyVersion("2021.3.19.0")]
