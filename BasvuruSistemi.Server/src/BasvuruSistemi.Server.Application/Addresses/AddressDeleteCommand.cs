using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Addresses;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Addresses;
public sealed record AddressDeleteCommand(
    Guid Id
    ) : IRequest<Result<string>>;

internal sealed class AddressDeleteCommandHandler(
    IAddressRepository addressRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<AddressDeleteCommand, Result<string>>{
    public async Task<Result<string>> Handle(AddressDeleteCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if(!userId.HasValue)
            return Result<string>.Failure(404,"User not found");

        var address = await addressRepository.FirstOrDefaultAsync(p => p.UserId == userId.Value && p.Id == request.Id && !p.IsDeleted);

        if (address is null)
            return Result<string>.Failure(404,"Address not found");

        address.IsDeleted = true;

        addressRepository.Update(address);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Address deleted successfully");
    }
}
