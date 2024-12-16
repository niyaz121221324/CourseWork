namespace FreightFlow.DAL.Models;

public partial class Vehicle
{
    public int Vehicleid { get; set; }

    public string Vehiclenumber { get; set; } = null!;

    public string Vehicletype { get; set; } = null!;

    public decimal Loadcapacity { get; set; }

    public decimal? Volume { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
