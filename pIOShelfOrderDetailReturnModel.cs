// Decompiled with JetBrains decompiler
// Type: Shelf.Models.pIOShelfOrderDetailReturnModel
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 375995AA-8D0C-4500-B93C-F0EB5887EB9B
// Assembly location: C:\Users\pc\Downloads\Shelf.dll

namespace Shelf.Models
{
  public class pIOShelfOrderDetailReturnModel
  {
    public string ItemDescription { get; set; }

    public string ItemCode { get; set; }

    public string ItemDim1Code { get; set; }

    public string ColorCode { get; set; }

    public int? SortOrder { get; set; }

    public string ItemCodeLong => this.ItemCode + "-" + this.ColorCode + "-" + this.ItemDim1Code + (!string.IsNullOrEmpty(this.ItemDim2Code) ? "-" + this.ItemDim2Code : "");

    public string RowColorCode => this.LastReadBarcode ? "DeepSkyBlue" : "White";

    public bool LastReadBarcode { get; set; }

    public string ItemDim1CodeStr => !string.IsNullOrEmpty(this.ItemDim1Code) ? "Beden : " + this.ItemDim1Code : "";

    public string ItemDim2Code { get; set; }

    public string ItemDim2CodeStr => !string.IsNullOrEmpty(this.ItemDim2Code) ? "Kavala :" + this.ItemDim2Code : "";

    public double OrderQty { get; set; }

    public string OrderQtyStr => this.OrderQty > 0.0 ? "Sip Miktarı : " + (object) this.OrderQty : "";

    public double PickingQty { get; set; }

    public string PickingQtyStr => this.PickingQty > 0.0 ? "Onay Miktar : " + (object) this.PickingQty : "";

    public int ShelfOrderDetailID { get; set; }

    public int ShelfOrderID { get; set; }

    public string Barcode { get; set; }

    public string ShelfCode { get; set; }

    public string ShelfName { get; set; }
  }
}
