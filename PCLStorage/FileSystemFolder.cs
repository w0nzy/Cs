// Decompiled with JetBrains decompiler
// Type: PCLStorage.FileSystemFolder
// Assembly: PCLStorage, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: FB422C97-CC83-4BAE-A6A4-C42C2408B899
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\PCLStorage.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PCLStorage
{
  [DebuggerDisplay("Name = {_name}")]
  public class FileSystemFolder : IFolder
  {
    private readonly string _name;
    private readonly string _path;
    private readonly bool _canDelete;

    public FileSystemFolder(string path, bool canDelete)
    {
      this._name = System.IO.Path.GetFileName(path);
      this._path = path;
      this._canDelete = canDelete;
    }

    public FileSystemFolder(string path)
      : this(path, false)
    {
    }

    public string Name => this._name;

    public string Path => this._path;

    public async Task<IFile> CreateFileAsync(
      string desiredName,
      CreationCollisionOption option,
      CancellationToken cancellationToken)
    {
      Requires.NotNullOrEmpty(desiredName, nameof (desiredName));
      await AwaitExtensions.SwitchOffMainThreadAsync(cancellationToken);
      this.EnsureExists();
      string nameToUse = desiredName;
      string newPath = System.IO.Path.Combine(this.Path, nameToUse);
      if (File.Exists(newPath))
      {
        if (option == null)
        {
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(desiredName);
          string extension = System.IO.Path.GetExtension(desiredName);
          int num = 2;
          while (File.Exists(newPath))
          {
            cancellationToken.ThrowIfCancellationRequested();
            nameToUse = withoutExtension + " (" + (object) num + ")" + extension;
            newPath = System.IO.Path.Combine(this.Path, nameToUse);
            ++num;
          }
          this.InternalCreateFile(newPath);
        }
        else if (option == 1)
        {
          File.Delete(newPath);
          this.InternalCreateFile(newPath);
        }
        else
        {
          if (option == 2)
            throw new IOException("File already exists: " + newPath);
          if (option != 3)
            throw new ArgumentException("Unrecognized CreationCollisionOption: " + (object) option);
        }
      }
      else
        this.InternalCreateFile(newPath);
      FileSystemFile ret = new FileSystemFile(newPath);
      return (IFile) ret;
    }

    private void InternalCreateFile(string path)
    {
      using (File.Create(path))
        ;
    }

    public async Task<IFile> GetFileAsync(string name, CancellationToken cancellationToken)
    {
      await AwaitExtensions.SwitchOffMainThreadAsync(cancellationToken);
      string path = System.IO.Path.Combine(this.Path, name);
      FileSystemFile ret = File.Exists(path) ? new FileSystemFile(path) : throw new PCLStorage.Exceptions.FileNotFoundException("File does not exist: " + path);
      return (IFile) ret;
    }

    public async Task<IList<IFile>> GetFilesAsync(CancellationToken cancellationToken)
    {
      await AwaitExtensions.SwitchOffMainThreadAsync(cancellationToken);
      this.EnsureExists();
      IList<IFile> ret = (IList<IFile>) ((IEnumerable<IFile>) ((IEnumerable<string>) Directory.GetFiles(this.Path)).Select<string, FileSystemFile>((Func<string, FileSystemFile>) (f => new FileSystemFile(f)))).ToList<IFile>().AsReadOnly();
      return ret;
    }

    public async Task<IFolder> CreateFolderAsync(
      string desiredName,
      CreationCollisionOption option,
      CancellationToken cancellationToken)
    {
      Requires.NotNullOrEmpty(desiredName, nameof (desiredName));
      await AwaitExtensions.SwitchOffMainThreadAsync(cancellationToken);
      this.EnsureExists();
      string nameToUse = desiredName;
      string newPath = System.IO.Path.Combine(this.Path, nameToUse);
      if (Directory.Exists(newPath))
      {
        if (option == null)
        {
          int num = 2;
          while (Directory.Exists(newPath))
          {
            cancellationToken.ThrowIfCancellationRequested();
            nameToUse = desiredName + " (" + (object) num + ")";
            newPath = System.IO.Path.Combine(this.Path, nameToUse);
            ++num;
          }
          Directory.CreateDirectory(newPath);
        }
        else if (option == 1)
        {
          Directory.Delete(newPath, true);
          Directory.CreateDirectory(newPath);
        }
        else
        {
          if (option == 2)
            throw new IOException("Directory already exists: " + newPath);
          if (option != 3)
            throw new ArgumentException("Unrecognized CreationCollisionOption: " + (object) option);
        }
      }
      else
        Directory.CreateDirectory(newPath);
      FileSystemFolder ret = new FileSystemFolder(newPath, true);
      return (IFolder) ret;
    }

    public async Task<IFolder> GetFolderAsync(
      string name,
      CancellationToken cancellationToken)
    {
      Requires.NotNullOrEmpty(name, nameof (name));
      await AwaitExtensions.SwitchOffMainThreadAsync(cancellationToken);
      string path = System.IO.Path.Combine(this.Path, name);
      FileSystemFolder ret = Directory.Exists(path) ? new FileSystemFolder(path, true) : throw new PCLStorage.Exceptions.DirectoryNotFoundException("Directory does not exist: " + path);
      return (IFolder) ret;
    }

    public async Task<IList<IFolder>> GetFoldersAsync(
      CancellationToken cancellationToken)
    {
      await AwaitExtensions.SwitchOffMainThreadAsync(cancellationToken);
      this.EnsureExists();
      IList<IFolder> ret = (IList<IFolder>) ((IEnumerable<IFolder>) ((IEnumerable<string>) Directory.GetDirectories(this.Path)).Select<string, FileSystemFolder>((Func<string, FileSystemFolder>) (d => new FileSystemFolder(d, true)))).ToList<IFolder>().AsReadOnly();
      return ret;
    }

    public async Task<ExistenceCheckResult> CheckExistsAsync(
      string name,
      CancellationToken cancellationToken)
    {
      Requires.NotNullOrEmpty(name, nameof (name));
      await AwaitExtensions.SwitchOffMainThreadAsync(cancellationToken);
      string checkPath = PortablePath.Combine(this.Path, name);
      return !File.Exists(checkPath) ? (!Directory.Exists(checkPath) ? (ExistenceCheckResult) 0 : (ExistenceCheckResult) 2) : (ExistenceCheckResult) 1;
    }

    public async Task DeleteAsync(CancellationToken cancellationToken)
    {
      if (!this._canDelete)
        throw new IOException("Cannot delete root storage folder.");
      await AwaitExtensions.SwitchOffMainThreadAsync(cancellationToken);
      this.EnsureExists();
      Directory.Delete(this.Path, true);
    }

    private void EnsureExists()
    {
      if (!Directory.Exists(this.Path))
        throw new PCLStorage.Exceptions.DirectoryNotFoundException("Directory does not exist: " + this.Path);
    }
  }
}
