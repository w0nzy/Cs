// Decompiled with JetBrains decompiler
// Type: Shelf.Models.pIOShelfPurchaseOrderDetailReturnModel
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 375995AA-8D0C-4500-B93C-F0EB5887EB9B
// Assembly location: C:\Users\pc\Downloads\Shelf.dll

namespace Shelf.Models
{
  public class pIOShelfPurchaseOrderDetailReturnModel
  {
    public string ItemDescription { get; set; }

    public string ItemCode { get; set; }

    public string ColorCode { get; set; }

    public string ItemDim1Code { get; set; }

    public string ItemDim2Code { get; set; }

    public double OrderQty { get; set; }

    public double AllocatingQty { get; set; }

    public int PurchaseOrderDetailID { get; set; }

    public int PurchaseOrderID { get; set; }

    public string Barcode { get; set; }

    public string ShelfCode { get; set; }

    public string ShelfName { get; set; }

    public int? SortOrder { get; set; }

    public bool LastReadBarcode { get; set; }

    public string ItemDescription2 => this.ItemDescription.Length > 25 ? this.ItemDescription.Substring(0, 25) + ".." : this.ItemDescription;

    public string RowColorCode => this.LastReadBarcode ? "DeepSkyBlue" : "White";

    public string ItemCodeLong => this.ItemCode + "-" + this.ColorCode + "-" + this.ItemDim1Code + (!string.IsNullOrEmpty(this.ItemDim2Code) ? "-" + this.ItemDim2Code : "");

    public string AllocatingQtyStr => this.AllocatingQty > 0.0 ? "Onay Miktar : " + (object) this.AllocatingQty : "";

    public string OrderQtyStr => this.OrderQty > 0.0 ? "Sip Miktarı : " + (object) this.OrderQty : "";
  }
}
