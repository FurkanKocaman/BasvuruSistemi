using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using BasvuruSistemi.Server.Domain.Units;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Units;
public sealed record UnitDeleteCommand(
    Guid id
    ) : IRequest<Result<string>>;

internal sealed class UnitDeleteCommandHandler(
    IAuthorizationService authorizationService,
    ICurrentUserService currentUserService,
    IUnitRepository unitRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UnitDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UnitDeleteCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<string>.Failure(404, "user not found");

        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return Result<string>.Failure(404, "Tenant not found");

        var isAuthorized = await authorizationService.IsTenantManagerAsync(userId.Value, tenantId.Value, cancellationToken);
        if (!isAuthorized)
            return Result<string>.Failure(403, "You do not have permission to delete unit");

        var unit = await unitRepository.FirstOrDefaultAsync(p => p.Id == request.id);

        if (unit is null)
            return Result<string>.Failure("unit not found");

        unit.IsDeleted = true;

        unitRepository.Update(unit);

        await unitOfWork.SaveChangesAsync();

        return Result<string>.Succeed("Unit deleted successfully");
    }
}
