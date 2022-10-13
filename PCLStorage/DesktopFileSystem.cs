// Decompiled with JetBrains decompiler
// Type: PCLStorage.DesktopFileSystem
// Assembly: PCLStorage, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: FB422C97-CC83-4BAE-A6A4-C42C2408B899
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\PCLStorage.dll

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PCLStorage
{
  public class DesktopFileSystem : IFileSystem
  {
    public IFolder LocalStorage => (IFolder) new FileSystemFolder(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

    public IFolder RoamingStorage => (IFolder) null;

    public async Task<IFile> GetFileFromPathAsync(
      string path,
      CancellationToken cancellationToken)
    {
      Requires.NotNullOrEmpty(path, nameof (path));
      await AwaitExtensions.SwitchOffMainThreadAsync(cancellationToken);
      return !File.Exists(path) ? (IFile) null : (IFile) new FileSystemFile(path);
    }

    public async Task<IFolder> GetFolderFromPathAsync(
      string path,
      CancellationToken cancellationToken)
    {
      Requires.NotNullOrEmpty(path, nameof (path));
      await AwaitExtensions.SwitchOffMainThreadAsync(cancellationToken);
      return !Directory.Exists(path) ? (IFolder) null : (IFolder) new FileSystemFolder(path, true);
    }
  }
}
