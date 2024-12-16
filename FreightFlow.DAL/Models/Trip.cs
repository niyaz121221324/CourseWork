using System;
using System.Collections.Generic;

namespace FreightFlow.DAL.Models;

public partial class Trip
{
    public int Tripid { get; set; }

    public int Orderid { get; set; }

    public int Vehicleid { get; set; }

    public int Driverid { get; set; }

    public DateOnly Startdate { get; set; }

    public DateOnly? Enddate { get; set; }

    public string? Status { get; set; }

    public virtual User Driver { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
