namespace blazor_client.Models;

public class User
{
    public int Userid { get; set; }

    public string Firstname { get; set; } = string.Empty;

    public string Lastname { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string Login { get; set; } = string.Empty;

    public string Passwordhash { get; set; } = string.Empty;

    public string? Phone { get; set; }
    
    public string? Email { get; set; }
}