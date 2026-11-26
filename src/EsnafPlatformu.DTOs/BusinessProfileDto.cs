namespace EsnafPlatformu.DTOs;

public record BusinessProfileDto(
    string? ShortDescription,
    string? About,
    string? Phone,
    string? ContactEmail);

public record BusinessProfileDetailsDto(
    string Slug,
    BusinessProfileDto Profile);