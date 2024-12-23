namespace blazor_client.Models;

public partial class Driver
{
    public int Driverid { get; set; }

    public string Firstname { get; set; } = string.Empty;

    public string Lastname { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string Licensenumber { get; set; } = null!;

    public DateOnly Licenseexpiry { get; set; }

    public string? Status { get; set; } = "Available";
}