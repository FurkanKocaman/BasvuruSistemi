using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Addresses;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Users;
public sealed record UserAddressUpdateCommand(
    string? Street, 
    string? District,
    string? City,
    string? Country,
    string? PostalCode, 
    string? FullAdress, 
    Guid UserId
    ) : IRequest<Result<string>>;


internal sealed class UserAddressUpdateCommandHandler(
    IAddressRepository addressRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UserAddressUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserAddressUpdateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<string>.Failure(404, "User not found");

        var address = await addressRepository.Where(p => p.UserId == request.UserId && !p.IsDeleted && p.IsActive).FirstOrDefaultAsync();

        if(address is null)
        {
            address = new Address(request.Street,request.District,request.City, request.Country, request.PostalCode, request.FullAdress, request.UserId);
            addressRepository.Add(address);

        }
        else
        {
            address.Update(request.Street, request.District, request.City, request.Country, request.PostalCode, request.FullAdress);
            addressRepository.Update(address);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Address updated successfully.");
    }
}
