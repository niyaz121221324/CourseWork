using System;
using System.Collections.Generic;

namespace FreightFlow.DAL.Models;

public partial class Order
{
    public int Orderid { get; set; }

    public int Clientid { get; set; }

    public int Cargoid { get; set; }

    public int Routeid { get; set; }

    public int? Vehicleid { get; set; }

    public int? Driverid { get; set; }

    public DateOnly Orderdate { get; set; }

    public DateOnly? Deliverydate { get; set; }

    public string? Status { get; set; }

    public virtual Cargo Cargo { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual User? Driver { get; set; }

    public virtual Route Route { get; set; } = null!;

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();

    public virtual Vehicle? Vehicle { get; set; }
}
