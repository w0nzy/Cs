// Decompiled with JetBrains decompiler
// Type: Shelf.Models.ztIOShelf
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 375995AA-8D0C-4500-B93C-F0EB5887EB9B
// Assembly location: C:\Users\pc\Downloads\Shelf.dll

using System;

namespace Shelf.Models
{
  public class ztIOShelf
  {
    public int ShelfID { get; set; }

    public int? HallID { get; set; }

    public string Code { get; set; }

    public string WarehouseCode { get; set; }

    public string Description { get; set; }

    public int? SortOrder { get; set; }

    public bool? IsBlocked { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string CreatedUserName { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string UpdatedUserName { get; set; }
  }
}
