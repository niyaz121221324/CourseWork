using System;
using System.Collections.Generic;

namespace FreightFlow.DAL.Models;

public partial class Client
{
    public int Clientid { get; set; }

    public string? Companyname { get; set; }

    public string? Inn { get; set; }

    public string? Contactperson { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
