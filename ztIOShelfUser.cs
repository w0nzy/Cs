// Decompiled with JetBrains decompiler
// Type: Shelf.Models.ztIOShelfUser
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 375995AA-8D0C-4500-B93C-F0EB5887EB9B
// Assembly location: C:\Users\pc\Downloads\Shelf.dll

using System;

namespace Shelf.Models
{
  public class ztIOShelfUser
  {
    public int ShelfUserID { get; set; }

    public string UserName { get; set; }

    public string FirstLastName { get; set; }

    public string Password { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsPickingUser { get; set; }

    public bool IsReceivingUser { get; set; }

    public bool IsBlocked { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedUserName { get; set; }

    public DateTime UpdatedDate { get; set; }

    public string UpdatedUserName { get; set; }

    public string MenuIds { get; set; }
  }
}
