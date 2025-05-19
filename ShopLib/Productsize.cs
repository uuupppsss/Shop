using System;
using System.Collections.Generic;

namespace ShopLib;

public partial class Productsize
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string Size { get; set; } = null!;

    public int Count { get; set; } 

    public virtual Product Product { get; set; } = null!;
}
