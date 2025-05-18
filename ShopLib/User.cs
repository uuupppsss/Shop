using System;
using System.Collections.Generic;

namespace ShopLib;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<Basketitem> Basketitems { get; set; } = new List<Basketitem>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Userrole Role { get; set; } = null!;
}
