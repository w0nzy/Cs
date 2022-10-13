// Decompiled with JetBrains decompiler
// Type: Shelf.Models.ztIOShelfTransactionDetail
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 375995AA-8D0C-4500-B93C-F0EB5887EB9B
// Assembly location: C:\Users\pc\Downloads\Shelf.dll

using System;

namespace Shelf.Models
{
  public class ztIOShelfTransactionDetail
  {
    public int TransactionDetailID { get; set; }

    public int TransactionID { get; set; }

    public int? ShelfOrderDetailID { get; set; }

    public int? ItemTypeCode { get; set; }

    public string ItemCode { get; set; }

    public string ColorCode { get; set; }

    public string ItemDim1Code { get; set; }

    public string ItemDim2Code { get; set; }

    public string ItemDim3Code { get; set; }

    public string UsedBarcode { get; set; }

    public double? Qty { get; set; }

    public string QtyStr
    {
      get
      {
        double? qty = this.Qty;
        double num = 0.0;
        return qty.GetValueOrDefault() > num & qty.HasValue ? "Miktar : " + (object) this.Qty : "";
      }
    }

    public DateTime? CreatedDate { get; set; }

    public string CreatedUserName { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string UpdatedUserName { get; set; }
  }
}
