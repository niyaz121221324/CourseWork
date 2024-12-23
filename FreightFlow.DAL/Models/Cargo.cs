namespace FreightFlow.DAL.Models;

public partial class Cargo
{
    public int Cargoid { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public decimal Weight { get; set; }

    public decimal? Volume { get; set; }

    public string? Specialrequirements { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}