using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Addresses;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Addresses;
public sealed record AddressCreateCommand(
     string? Country,
     string? City,
     string? District,
     string? Street,
     string? FullAddress,
     string? PostalCode
    ) : IRequest<Result<string>>;

internal sealed class AddressCreateCommandHandler(
    ICurrentUserService currentUserService,
    IAddressRepository addressRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<AddressCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AddressCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if(!userId.HasValue)
            return Result<string>.Failure("User not found");

        var addressExist = await addressRepository.Where(p => p.UserId == userId).FirstOrDefaultAsync();

        if(addressExist is not null)
        {
            addressExist.IsDeleted = true;
            addressExist.IsActive = false;
            addressRepository.Update(addressExist);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        var address = new Address(request.Street,request.District,request.City, request.Country, request.PostalCode, request.FullAddress, userId.Value);

        addressRepository.Add(address);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Address created successfully.");
    }
}
