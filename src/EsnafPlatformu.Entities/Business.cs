namespace EsnafPlatformu.Entities;

public class Business
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Slug { get; set; } = "";
    public string? City { get; set; }
    public string? District { get; set; }

    public string? OwnerUserId { get; set; }

    public List<MenuItem> MenuItems { get; set; } = new();



    public string? ShortDescription { get; set; }
public string? About { get; set; }
public string? Phone { get; set; }
public string? ContactEmail { get; set; }
}