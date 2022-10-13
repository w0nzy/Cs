// Decompiled with JetBrains decompiler
// Type: PCLStorage.Requires
// Assembly: PCLStorage, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: FB422C97-CC83-4BAE-A6A4-C42C2408B899
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\PCLStorage.dll

using System;
using System.Diagnostics;
using System.Globalization;

namespace PCLStorage
{
  internal static class Requires
  {
    private const string Argument_EmptyString = "'{0}' cannot be an empty string (\"\") or start with the null character.";

    [DebuggerStepThrough]
    public static T NotNull<T>(T value, string parameterName) where T : class => (object) value != null ? value : throw new ArgumentNullException(parameterName);

    [DebuggerStepThrough]
    public static void NotNullOrEmpty(string value, string parameterName)
    {
      switch (value)
      {
        case "":
          throw new ArgumentException(Requires.Format("'{0}' cannot be an empty string (\"\") or start with the null character.", (object) parameterName), parameterName);
        case null:
          throw new ArgumentNullException(parameterName);
        default:
          if (value[0] != char.MinValue)
            break;
          goto case "";
      }
    }

    private static string Format(string format, params object[] arguments) => string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, arguments);
  }
}
