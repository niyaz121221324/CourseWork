namespace FreightFlow.DAL.Models;

public partial class Driver
{
    public int Driverid { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Phone { get; set; }

    public string Licensenumber { get; set; } = null!;

    public DateOnly Licenseexpiry { get; set; }

    public string? Status { get; set; }
}
