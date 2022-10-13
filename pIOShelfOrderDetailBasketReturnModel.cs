// Decompiled with JetBrains decompiler
// Type: Shelf.Models.pIOShelfOrderDetailBasketReturnModel
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 375995AA-8D0C-4500-B93C-F0EB5887EB9B
// Assembly location: C:\Users\pc\Downloads\Shelf.dll

using System;

namespace Shelf.Models
{
  public class pIOShelfOrderDetailBasketReturnModel
  {
    public string ItemDescription { get; set; }

    public string ItemCode { get; set; }

    public string ColorCode { get; set; }

    public string ItemDim1Code { get; set; }

    public string ItemDim2Code { get; set; }

    public double OrderQty { get; set; }

    public string ItemCodeLong => this.ItemCode + "-" + this.ColorCode + "-" + this.ItemDim1Code + (!string.IsNullOrEmpty(this.ItemDim2Code) ? "-" + this.ItemDim2Code : "");

    public string RowColorCode
    {
      get
      {
        if (this.IsFirst)
          return "DeepSkyBlue";
        return this.ApproveQty != this.PickingQty ? "White" : "Gray";
      }
    }

    public bool IsFirst { get; set; }

    public string ItemDim1CodeStr => !string.IsNullOrEmpty(this.ItemDim1Code) ? "Beden : " + this.ItemDim1Code : "";

    public string PickingQtyStr => this.PickingQty > 0.0 ? "Miktar : " + Convert.ToString(this.PickingQty) : "";

    public string ApproveQtyStr => this.ApproveQty > 0.0 ? "Top. Mik. : " + Convert.ToString(this.ApproveQty) : "";

    public double PickingQty { get; set; }

    public double ApproveQty { get; set; }

    public int ShelfOrderDetailID { get; set; }

    public int ShelfOrderID { get; set; }

    public string Barcode { get; set; }

    public string ShelfCode { get; set; }

    public string ShelfName { get; set; }
  }
}
