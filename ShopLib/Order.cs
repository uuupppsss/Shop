using System;
using System.Collections.Generic;

namespace ShopLib;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime CreateDate { get; set; }

    public decimal Cost { get; set; }

    public int StatusId { get; set; }

    public string Adress { get; set; } = null!;

    public string Index { get; set; } = null!;

    public string ContactPhone { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Trak { get; set; } = " ";

    public virtual Orderstatus? Status { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();
}
