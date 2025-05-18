using System;
using System.Collections.Generic;

namespace ShopLib;

public partial class Productsize
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string Size { get; set; } = null!;

    public string Count { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
