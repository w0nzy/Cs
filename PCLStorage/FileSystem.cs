// Decompiled with JetBrains decompiler
// Type: PCLStorage.FileSystem
// Assembly: PCLStorage, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: FB422C97-CC83-4BAE-A6A4-C42C2408B899
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\PCLStorage.dll

using System;
using System.Threading;

namespace PCLStorage
{
  public static class FileSystem
  {
    private static Lazy<IFileSystem> _fileSystem = new Lazy<IFileSystem>((Func<IFileSystem>) (() => FileSystem.CreateFileSystem()), LazyThreadSafetyMode.PublicationOnly);

    public static IFileSystem Current => FileSystem._fileSystem.Value ?? throw FileSystem.NotImplementedInReferenceAssembly();

    private static IFileSystem CreateFileSystem() => (IFileSystem) new DesktopFileSystem();

    internal static Exception NotImplementedInReferenceAssembly() => (Exception) new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the PCLStorage NuGet package from your main application project in order to reference the platform-specific implementation.");
  }
}
