using System;
using System.Collections.Generic;

namespace FreightFlow.DAL.Models;

public partial class User
{
    public int Userid { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
