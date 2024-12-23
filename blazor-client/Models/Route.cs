namespace blazor_client.Models;

public class Route
{
    public int RouteId { get; set; }

    public string StartPoint { get; set; } = string.Empty;

    public string EndPoint { get; set; } = string.Empty;

    public string? IntermediatePoints { get; set; }

    public decimal Distance { get; set; }

    public string? Description { get; set; }
}