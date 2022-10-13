// Decompiled with JetBrains decompiler
// Type: Shelf.Models.pIOUserShelfPurchaseOrdersReturnModel
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 375995AA-8D0C-4500-B93C-F0EB5887EB9B
// Assembly location: C:\Users\pc\Downloads\Shelf.dll

using System;

namespace Shelf.Models
{
  public class pIOUserShelfPurchaseOrdersReturnModel
  {
    public int PurchaseOrderID { get; set; }

    public string PurchaseOrderNumber { get; set; }

    public string WarehouseCode { get; set; }

    public DateTime? PurchaseOrderDate { get; set; }

    public int AssignedUserID { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string CreatedUserName { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string UpdatedUserName { get; set; }
  }
}
