using EsnafPlatformu.DataAccess;
using EsnafPlatformu.DTOs;
using EsnafPlatformu.Entities;

namespace EsnafPlatformu.Business;

public enum StoreCreateResult
{
    Created,
    BusinessNotFound,
    InvalidName,
    InvalidCategory
}

public class StoreService(StoreRepository repository)
{
    public Task<List<StoreListItemDto>> ListAsync(
        string ownerUserId, CancellationToken cancellationToken)
    {
        return repository.ListAsync(ownerUserId, cancellationToken);
    }


    public Task<StoreListItemDto?> GetAsync(
    int storeId, string ownerUserId, CancellationToken cancellationToken)
{
    return repository.GetAsync(storeId, ownerUserId, cancellationToken);
}

    public async Task<StoreCreateResult> AddAsync(
        string ownerUserId,
        string? name,
        int? category,
        CancellationToken cancellationToken)
    {
        var trimmedName = name?.Trim();
        if (string.IsNullOrWhiteSpace(trimmedName) || trimmedName.Length > 100)
        {
            return StoreCreateResult.InvalidName;
        }

        if (category is not (1 or 2))
        {
            return StoreCreateResult.InvalidCategory;
        }

        var created = await repository.AddAsync(
            ownerUserId, trimmedName, (StoreCategory)category.Value,
            cancellationToken);

        return created
            ? StoreCreateResult.Created
            : StoreCreateResult.BusinessNotFound;
    }
}