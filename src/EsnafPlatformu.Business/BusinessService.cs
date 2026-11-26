using EsnafPlatformu.DataAccess;
using EsnafPlatformu.DTOs;
using BusinessEntity = EsnafPlatformu.Entities.Business;

namespace EsnafPlatformu.Business;

public class BusinessService(BusinessRepository repository)
{
    public Task<BusinessCreateResult> TryCreateAsync(
        CreateBusinessDto input,
        string ownerUserId,
        CancellationToken cancellationToken)
    {
        var business = new BusinessEntity
        {
            Name = input.Name,
            Slug = input.Slug,
            City = input.City,
            District = input.District,
            OwnerUserId = ownerUserId
        };

        return repository.TryAddAsync(business, cancellationToken);
    }

    public Task<BusinessProfileDetailsDto?> GetProfileAsync(
    string ownerUserId,
    CancellationToken cancellationToken)
{
    return repository.GetProfileAsync(ownerUserId, cancellationToken);
}

public Task<bool> UpdateProfileAsync(
    string ownerUserId,
    BusinessProfileDto input,
    CancellationToken cancellationToken)
{
    return repository.UpdateProfileAsync(ownerUserId, input, cancellationToken);
}


}