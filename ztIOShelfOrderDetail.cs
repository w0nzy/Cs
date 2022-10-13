// Decompiled with JetBrains decompiler
// Type: Shelf.Models.ztIOShelfOrderDetail
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 375995AA-8D0C-4500-B93C-F0EB5887EB9B
// Assembly location: C:\Users\pc\Downloads\Shelf.dll

using System;

namespace Shelf.Models
{
  public class ztIOShelfOrderDetail
  {
    public int ShelfOrderDetailID { get; set; }

    public int ShelfOrderID { get; set; }

    public int ShelfID { get; set; }

    public int ItemTypeCode { get; set; }

    public string ItemCode { get; set; }

    public string ColorCode { get; set; }

    public string ItemDim1Code { get; set; }

    public string ItemDim2Code { get; set; }

    public string ItemDim3Code { get; set; }

    public double OrderQty { get; set; }

    public double PickingQty { get; set; }

    public double ApproveQty { get; set; }

    public bool IsApproved { get; set; }

    public int CurrAccTypeCode { get; set; }

    public string CurrAccCode { get; set; }

    public Guid? SubCurrAccID { get; set; }

    public string DispOrderNumber { get; set; }

    public Guid DispOrderLineID { get; set; }

    public string ShippingNumber { get; set; }

    public Guid? ShipmentLineID { get; set; }

    public bool IsPostToV3 { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string CreatedUserName { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string UpdatedUserName { get; set; }
  }
}
