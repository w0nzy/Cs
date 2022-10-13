// Decompiled with JetBrains decompiler
// Type: Shelf.Models.pIOGetShelfCountingDetailReturnModel
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 375995AA-8D0C-4500-B93C-F0EB5887EB9B
// Assembly location: C:\Users\pc\Downloads\Shelf.dll

using System;
using System.Collections.Generic;

namespace Shelf.Models
{
  public class pIOGetShelfCountingDetailReturnModel
  {
    public int CountingDetailID { get; set; }

    public int CountingID { get; set; }

    public string ShelfCode { get; set; }

    public string UsedBarcode { get; set; }

    public double Qty { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedUserName { get; set; }

    public DateTime UpdatedDate { get; set; }

    public string UpdatedUserName { get; set; }

    public List<pIOGetShelfCountingDetailReturnModel> childList { get; set; }

    public string RowColorCode => !this.LastReadBarcode ? "White" : "DeepSkyBlue";

    public bool LastReadBarcode { get; set; }
  }
}
