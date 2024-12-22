namespace blazor_client.Models;

public class Vehicle
{
    public int VehicleId { get; set; }

    public string VehicleNumber { get; set; } = string.Empty;

    public string VehicleType { get; set; } = string.Empty;

    public decimal LoadCapacity { get; set; }

    public decimal? Volume { get; set; }

    public string? Status { get; set; }
}