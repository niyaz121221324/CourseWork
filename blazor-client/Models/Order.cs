namespace blazor_client.Models;

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
  
    public string? Status { get; set; } = "New";

    public Cargo Cargo { get; set; } = null!;

    public Client Client { get; set; } = null!;

    public User? Driver { get; set; }

    public Route Route { get; set; } = null!;

    public Vehicle? Vehicle { get; set; }
}