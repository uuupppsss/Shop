using System;
using System.Collections.Generic;

namespace ShopLib;

public partial class Orderstatus
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
