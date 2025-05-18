﻿using System;
using System.Collections.Generic;

namespace ShopLib;

public partial class Userrole
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
