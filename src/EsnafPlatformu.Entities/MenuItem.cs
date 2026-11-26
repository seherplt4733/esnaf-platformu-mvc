namespace EsnafPlatformu.Entities;
public class MenuItem
{

public int Id { get; set; }
public int BusinessId { get; set; }
public Business Business { get; set; } = null!;
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int SortOrder { get; set; }
    public bool IsVisible { get; set; }
}