using System;
using System.Collections.Generic;

namespace ShopLib;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? RecieveDate { get; set; }

    public decimal Cost { get; set; }

    public int? StatusId { get; set; }

    public virtual Orderstatus? Status { get; set; }

    public virtual User User { get; set; } = null!;
}
