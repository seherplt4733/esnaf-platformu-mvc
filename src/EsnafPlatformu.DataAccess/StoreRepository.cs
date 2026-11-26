using EsnafPlatformu.DTOs;
using EsnafPlatformu.Entities;
using Microsoft.EntityFrameworkCore;

namespace EsnafPlatformu.DataAccess;

public class StoreRepository(ApplicationDbContext db)
{
    public Task<List<StoreListItemDto>> ListAsync(
        string ownerUserId,
        CancellationToken cancellationToken)
    {
        return OwnedStores(ownerUserId)
            .OrderBy(store => store.Name)
            .ThenBy(store => store.Id)
            .Select(store => new StoreListItemDto(
                store.Id, store.Name, (int)store.Category))
            .ToListAsync(cancellationToken);
    }

    public Task<StoreListItemDto?> GetAsync(
        int storeId,
        string ownerUserId,
        CancellationToken cancellationToken)
    {
        return OwnedStores(ownerUserId)
            .Where(store => store.Id == storeId)
            .Select(store => new StoreListItemDto(
                store.Id, store.Name, (int)store.Category))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> AddAsync(
        string ownerUserId,
        string name,
        StoreCategory category,
        CancellationToken cancellationToken)
    {
        var business = await db.Businesses
            .AsNoTracking()
            .SingleOrDefaultAsync(
                business => business.OwnerUserId == ownerUserId,
                cancellationToken);

        if (business is null)
        {
            return false;
        }

        db.Stores.Add(new Store
        {
            BusinessId = business.Id,
            Name = name,
            Category = category
        });

        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    private IQueryable<Store> OwnedStores(string ownerUserId)
    {
        return db.Stores
            .AsNoTracking()
            .Where(store => store.Business.OwnerUserId == ownerUserId);
    }
}