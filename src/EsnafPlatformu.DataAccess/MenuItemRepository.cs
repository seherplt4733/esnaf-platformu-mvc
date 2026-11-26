using EsnafPlatformu.DTOs;
using EsnafPlatformu.Entities;
using Microsoft.EntityFrameworkCore;

namespace EsnafPlatformu.DataAccess;

public class MenuItemRepository(ApplicationDbContext db)
{
    public Task<List<MenuItemDto>> ListAsync(
        string ownerUserId, CancellationToken cancellationToken)
    {
        return db.MenuItems
            .AsNoTracking()
            .Where(item => item.Business.OwnerUserId == ownerUserId)
            .OrderBy(item => item.SortOrder)
            .ThenBy(item => item.Id)
            .Select(item => new MenuItemDto(
            item.Name, item.Price, item.SortOrder, item.IsVisible)
              {
            Id = item.Id
             })
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> AddAsync(
        string ownerUserId, MenuItemDto input, CancellationToken cancellationToken)
    {
        var business = await db.Businesses
            .AsNoTracking()
            .SingleOrDefaultAsync(b => b.OwnerUserId == ownerUserId, cancellationToken);

        if (business is null)
        {
            return false;
        }

        db.MenuItems.Add(new MenuItem
        {
            BusinessId = business.Id,
            Name = input.Name,
            Price = input.Price,
            SortOrder = input.SortOrder,
            IsVisible = input.IsVisible
        });

        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
    private IQueryable<MenuItem> FindOwnedItem(int id, string ownerUserId)
{
    return db.MenuItems.Where(item =>
        item.Id == id && item.Business.OwnerUserId == ownerUserId);
}

public Task<MenuItemDto?> GetAsync(
    int id, string ownerUserId, CancellationToken cancellationToken)
{
    return FindOwnedItem(id, ownerUserId)
        .AsNoTracking()
        .Select(item => new MenuItemDto(
            item.Name, item.Price, item.SortOrder, item.IsVisible)
        {
            Id = item.Id
        })
        .SingleOrDefaultAsync(cancellationToken);
}

public async Task<bool> UpdateAsync(
    int id,
    string ownerUserId,
    MenuItemDto input,
    CancellationToken cancellationToken)
{
    var affectedRows = await FindOwnedItem(id, ownerUserId)
        .ExecuteUpdateAsync(setters => setters
            .SetProperty(item => item.Name, input.Name)
            .SetProperty(item => item.Price, input.Price)
            .SetProperty(item => item.SortOrder, input.SortOrder)
            .SetProperty(item => item.IsVisible, input.IsVisible),
            cancellationToken);

    return affectedRows == 1;
}

public async Task<bool> DeleteAsync(
    int id, string ownerUserId, CancellationToken cancellationToken)
{
    var affectedRows = await FindOwnedItem(id, ownerUserId)
        .ExecuteDeleteAsync(cancellationToken);

    return affectedRows == 1;
}






}