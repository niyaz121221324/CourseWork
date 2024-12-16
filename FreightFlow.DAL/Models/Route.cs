using System;
using System.Collections.Generic;

namespace FreightFlow.DAL.Models;

public partial class Route
{
    public int Routeid { get; set; }

    public string Startpoint { get; set; } = null!;

    public string Endpoint { get; set; } = null!;

    public string? Intermediatepoints { get; set; }

    public decimal Distance { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
