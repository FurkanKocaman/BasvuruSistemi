using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Addresses;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Users;
public sealed record UserAddressUpdateCommand(
    Guid Id,
    string Name,
    string? Street, 
    string? District,
    string? City,
    string? Country,
    string? PostalCode, 
    string? FullAddress
    ) : IRequest<Result<AddressDto>>;


internal sealed class UserAddressUpdateCommandHandler(
    IAddressRepository addressRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UserAddressUpdateCommand, Result<AddressDto>>
{
    public async Task<Result<AddressDto>> Handle(UserAddressUpdateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<AddressDto>.Failure(404, "User not found");

        var address = await addressRepository.Where(p => p.UserId == userId.Value && p.Id == request.Id && !p.IsDeleted && p.IsActive).FirstOrDefaultAsync();

        if(address is null)
        {
            address = new Address(request.Name, request.Street,request.District,request.City, request.Country, request.PostalCode, request.FullAddress, userId.Value);
            addressRepository.Add(address);

        }
        else
        {
            address.Update(request.Name, request.Street, request.District, request.City, request.Country, request.PostalCode, request.FullAddress);
            addressRepository.Update(address);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<AddressDto>.Succeed(new (address.Id,address.Name,address.Country,address.City,address.District,address.Street,address.FullAddress,address.PostalCode));
    }
}
