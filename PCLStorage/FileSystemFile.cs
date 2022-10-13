// Decompiled with JetBrains decompiler
// Type: PCLStorage.FileSystemFile
// Assembly: PCLStorage, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: FB422C97-CC83-4BAE-A6A4-C42C2408B899
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\PCLStorage.dll

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PCLStorage
{
  [DebuggerDisplay("Name = {_name}")]
  public class FileSystemFile : IFile
  {
    private string _name;
    private string _path;

    public FileSystemFile(string path)
    {
      this._name = System.IO.Path.GetFileName(path);
      this._path = path;
    }

    public string Name => this._name;

    public string Path => this._path;

    public async Task<Stream> OpenAsync(
      FileAccess fileAccess,
      CancellationToken cancellationToken)
    {
      await AwaitExtensions.SwitchOffMainThreadAsync(cancellationToken);
      if (fileAccess == null)
        return (Stream) File.OpenRead(this.Path);
      if (fileAccess == 1)
        return (Stream) File.Open(this.Path, FileMode.Open, FileAccess.ReadWrite);
      throw new ArgumentException("Unrecognized FileAccess value: " + (object) fileAccess);
    }

    public async Task DeleteAsync(CancellationToken cancellationToken)
    {
      await AwaitExtensions.SwitchOffMainThreadAsync(cancellationToken);
      if (!File.Exists(this.Path))
        throw new PCLStorage.Exceptions.FileNotFoundException("File does not exist: " + this.Path);
      File.Delete(this.Path);
    }

    public async Task RenameAsync(
      string newName,
      NameCollisionOption collisionOption,
      CancellationToken cancellationToken)
    {
      Requires.NotNullOrEmpty(newName, nameof (newName));
      await this.MoveAsync(PortablePath.Combine(System.IO.Path.GetDirectoryName(this._path), newName), collisionOption, cancellationToken);
    }

    public async Task MoveAsync(
      string newPath,
      NameCollisionOption collisionOption,
      CancellationToken cancellationToken)
    {
      Requires.NotNullOrEmpty(newPath, nameof (newPath));
      await AwaitExtensions.SwitchOffMainThreadAsync(cancellationToken);
      string newDirectory = System.IO.Path.GetDirectoryName(newPath);
      string newName = System.IO.Path.GetFileName(newPath);
      int num = 1;
      string str1;
      string str2;
      while (true)
      {
        cancellationToken.ThrowIfCancellationRequested();
        str1 = newName;
        if (num > 1)
          str1 = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} ({1}){2}", new object[3]
          {
            (object) System.IO.Path.GetFileNameWithoutExtension(newName),
            (object) num,
            (object) System.IO.Path.GetExtension(newName)
          });
        str2 = PortablePath.Combine(newDirectory, str1);
        if (File.Exists(str2))
        {
          switch ((int) collisionOption)
          {
            case 0:
              ++num;
              continue;
            case 1:
              goto label_7;
            case 2:
              goto label_6;
            default:
              goto label_8;
          }
        }
        else
          goto label_8;
      }
label_6:
      throw new IOException("File already exists.");
label_7:
      File.Delete(str2);
label_8:
      File.Move(this._path, str2);
      this._path = str2;
      this._name = str1;
    }
  }
}
