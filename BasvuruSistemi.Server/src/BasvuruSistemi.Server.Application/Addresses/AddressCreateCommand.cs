using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Addresses;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Addresses;
public sealed record AddressCreateCommand(
     string Name,
     string? Country,
     string? City,
     string? District,
     string? Street,
     string? FullAddress,
     string? PostalCode
    ) : IRequest<Result<AddressDto>>;

internal sealed class AddressCreateCommandHandler(
    ICurrentUserService currentUserService,
    IAddressRepository addressRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<AddressCreateCommand, Result<AddressDto>>
{
    public async Task<Result<AddressDto>> Handle(AddressCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if(!userId.HasValue)
            return Result<AddressDto>.Failure("User not found");

        var address = new Address(request.Name, request.Street,request.District,request.City, request.Country, request.PostalCode, request.FullAddress, userId.Value);

        addressRepository.Add(address);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AddressDto>.Succeed(new(address.Id,address.Name,address.Country,address.City,address.District,address.Street,address.FullAddress,address.PostalCode));
    }
}
