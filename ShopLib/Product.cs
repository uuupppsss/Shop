using System;
using System.Collections.Generic;

namespace ShopLib;

public partial class Product
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public int TypeId { get; set; }

    public int BrandId { get; set; }

    public int TimeBought { get; set; }

    public int Count { get; set; } 

    public virtual ICollection<Basketitem> Basketitems { get; set; } = new List<Basketitem>();

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    public virtual ICollection<Productsize> Productsizes { get; set; } = new List<Productsize>();

    public virtual Producttype Type { get; set; } = null!;
}
