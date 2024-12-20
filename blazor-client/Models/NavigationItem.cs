namespace blazor_client.Models;

public class NavigationItem
{
    public string Title { get; }
    public string Icon { get; }
    public string Path { get; }

    public NavigationItem(string title, string icon, string path)
    {
        Title = title;
        Icon = icon;
        Path = path;
    }
}