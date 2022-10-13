// Decompiled with JetBrains decompiler
// Type: Shelf.Models.MenuModelItem
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 375995AA-8D0C-4500-B93C-F0EB5887EB9B
// Assembly location: C:\Users\pc\Downloads\Shelf.dll

using Xamarin.Forms;

namespace Shelf.Models
{
  public class MenuModelItem
  {
    public string ImageName { get; set; }

    public string Title { get; set; }

    public string ColorCode { get; set; }

    public int MenuId { get; set; }

    public Color Color => Color.FromHex(this.ColorCode);
  }
}
