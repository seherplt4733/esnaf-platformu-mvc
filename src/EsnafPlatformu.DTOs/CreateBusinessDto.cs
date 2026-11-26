namespace EsnafPlatformu.DTOs;

public record CreateBusinessDto(
    string Name,
    string Slug,
    string? City,
    string? District);