using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using BasvuruSistemi.Server.Domain.Units;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Units;
public sealed record UnitCreateCommand(
    Guid? parentId,
    string name,
    string? code
    ) : IRequest<Result<string>>;

internal sealed class UnitCreateCommandHandler(
    IAuthorizationService authorizationService,
    IUnitRepository unitRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UnitCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UnitCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<string>.Failure(404,"user not found");

        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return Result<string>.Failure(404,"Tenant not found");

        var isAuthorized = await authorizationService.IsTenantManagerAsync(userId.Value, tenantId.Value, cancellationToken);
        if(!isAuthorized)
            return Result<string>.Failure(403, "You do not have permission to create unit");

        var unitExist = await unitRepository.AnyAsync(p => p.TenantId == tenantId.Value && p.Name == request.name && !p.IsDeleted);

        if(unitExist)
            return Result<string>.Failure("Unit already exists");

        var unit = new Domain.Units.Unit(tenantId.Value,request.name,request.code,request.parentId);

        await unitRepository.AddAsync(unit, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Unit created successfully");
    }
}


