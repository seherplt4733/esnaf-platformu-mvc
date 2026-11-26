using EsnafPlatformu.DTOs;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using BusinessEntity = EsnafPlatformu.Entities.Business;

namespace EsnafPlatformu.DataAccess;

public class BusinessRepository(ApplicationDbContext db)
{
    public async Task<BusinessCreateResult> TryAddAsync(
        BusinessEntity business,
        CancellationToken cancellationToken)
    {
        db.Businesses.Add(business);

        try
        {
            await db.SaveChangesAsync(cancellationToken);
            return BusinessCreateResult.Created;
        }
        catch (DbUpdateException ex) when (
            ex.InnerException is PostgresException pg &&
            pg.SqlState == PostgresErrorCodes.UniqueViolation &&
            pg.ConstraintName is "ux_businesses_owner_user_id" or "ux_businesses_slug")
        {
            db.Entry(business).State = EntityState.Detached;

            return pg.ConstraintName == "ux_businesses_owner_user_id"
                ? BusinessCreateResult.OwnerAlreadyHasBusiness
                : BusinessCreateResult.SlugAlreadyUsed;
        }
    }

    public Task<BusinessProfileDetailsDto?> GetProfileAsync(
        string ownerUserId,
        CancellationToken cancellationToken)
    {
        return db.Businesses
            .AsNoTracking()
            .Where(business => business.OwnerUserId == ownerUserId)
            .Select(business => new BusinessProfileDetailsDto(
                business.Slug,
                new BusinessProfileDto(
                    business.ShortDescription,
                    business.About,
                    business.Phone,
                    business.ContactEmail)))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> UpdateProfileAsync(
        string ownerUserId,
        BusinessProfileDto input,
        CancellationToken cancellationToken)
    {
        var business = await db.Businesses.SingleOrDefaultAsync(
            business => business.OwnerUserId == ownerUserId,
            cancellationToken);

        if (business is null)
        {
            return false;
        }

        business.ShortDescription = input.ShortDescription;
        business.About = input.About;
        business.Phone = input.Phone;
        business.ContactEmail = input.ContactEmail;

        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}