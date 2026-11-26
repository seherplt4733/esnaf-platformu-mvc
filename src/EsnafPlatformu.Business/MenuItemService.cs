using EsnafPlatformu.DataAccess;
using EsnafPlatformu.DTOs;

namespace EsnafPlatformu.Business;

public enum MenuItemWriteResult
{
    Saved,
    NotFound,
    InvalidPrice,
    InvalidSortOrder
}

public class MenuItemService(MenuItemRepository repository)
{
    public Task<List<MenuItemDto>> ListAsync(
        string ownerUserId, CancellationToken cancellationToken)
    {
        return repository.ListAsync(ownerUserId, cancellationToken);
    }

    public Task<MenuItemDto?> GetAsync(
        int id, string ownerUserId, CancellationToken cancellationToken)
    {
        return repository.GetAsync(id, ownerUserId, cancellationToken);
    }

    public async Task<MenuItemWriteResult> AddAsync(
        string ownerUserId, MenuItemDto input, CancellationToken cancellationToken)
    {
        if (ValidateForSave(input) is { } error)
        {
            return error;
        }

        var saved = await repository.AddAsync(
            ownerUserId, input, cancellationToken);

        return saved ? MenuItemWriteResult.Saved : MenuItemWriteResult.NotFound;
    }

    public async Task<MenuItemWriteResult> UpdateAsync(
        int id,
        string ownerUserId,
        MenuItemDto input,
        CancellationToken cancellationToken)
    {
        if (ValidateForSave(input) is { } error)
        {
            return error;
        }

        var saved = await repository.UpdateAsync(
            id, ownerUserId, input, cancellationToken);

        return saved ? MenuItemWriteResult.Saved : MenuItemWriteResult.NotFound;
    }

    public Task<bool> DeleteAsync(
        int id, string ownerUserId, CancellationToken cancellationToken)
    {
        return repository.DeleteAsync(id, ownerUserId, cancellationToken);
    }

    private static MenuItemWriteResult? ValidateForSave(MenuItemDto input)
    {
        if (input.Price <= 0)
        {
            return MenuItemWriteResult.InvalidPrice;
        }

        if (input.SortOrder < 0)
        {
            return MenuItemWriteResult.InvalidSortOrder;
        }

        return null;
    }
}