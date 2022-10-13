// Decompiled with JetBrains decompiler
// Type: PCLStorage.Exceptions.DirectoryNotFoundException
// Assembly: PCLStorage, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: FB422C97-CC83-4BAE-A6A4-C42C2408B899
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\PCLStorage.dll

using System;

namespace PCLStorage.Exceptions
{
  public class DirectoryNotFoundException : System.IO.DirectoryNotFoundException
  {
    public DirectoryNotFoundException(string message)
      : base(message)
    {
    }

    public DirectoryNotFoundException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
