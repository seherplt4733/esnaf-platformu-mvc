namespace EsnafPlatformu.Entities;

public enum StoreCategory
{
    Cafe = 1,
    Furniture = 2
}

public class Store
{
    public int Id { get; set; }

    public int BusinessId { get; set; }
    public Business Business { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public StoreCategory Category { get; set; }
}