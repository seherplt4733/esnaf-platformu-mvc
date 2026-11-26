namespace EsnafPlatformu.DTOs;

public record MenuItemDto(
    string Name,
    decimal Price,
    int SortOrder,
    bool IsVisible)
{
    public int Id { get; init; }
}