namespace EsnafPlatformu.Web.Models;
public class MenuItem
{
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int SortOrder { get; set; }
    public bool IsVisible { get; set; }
}