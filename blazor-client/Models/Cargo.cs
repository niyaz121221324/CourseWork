namespace blazor_client.Models;

public class Cargo
{
    public int CargoId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = "Standart";

    public decimal Weight { get; set; }

    public decimal? Volume { get; set; }

    public string? SpecialRequirements { get; set; }
}