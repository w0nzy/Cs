// Decompiled with JetBrains decompiler
// Type: PCLStorage.PortablePath
// Assembly: PCLStorage, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: FB422C97-CC83-4BAE-A6A4-C42C2408B899
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\PCLStorage.dll

using System.IO;

namespace PCLStorage
{
  public static class PortablePath
  {
    public static char DirectorySeparatorChar => Path.DirectorySeparatorChar;

    public static string Combine(params string[] paths) => Path.Combine(paths);
  }
}
