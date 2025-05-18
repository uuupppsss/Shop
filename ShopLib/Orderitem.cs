using System;
using System.Collections.Generic;

namespace ShopLib;

public partial class Orderitem
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int Count { get; set; }

    public string Size { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
