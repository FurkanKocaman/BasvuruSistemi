using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using BasvuruSistemi.Server.Domain.Units;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Units;
public sealed record UnitUpdateCommand(
    Guid id,
    string? parentId,
    string name,
    string? code
    ) : IRequest<Result<string>>;

internal sealed class UnitUpdateCommandhandler(
    ICurrentUserService currentUserService,
    IAuthorizationService authorizationService,
    IUnitRepository unitRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UnitUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UnitUpdateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<string>.Failure(404, "user not found");

        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return Result<string>.Failure(404, "Tenant not found");

        var isAuthorized = await authorizationService.IsTenantManagerAsync(userId.Value, tenantId.Value, cancellationToken);
        if (!isAuthorized)
            return Result<string>.Failure(403, "You do not have permission to edit unit");

        var unit = await unitRepository.FirstOrDefaultAsync(p => p.Id == request.id);

        if (unit is null)
            return Result<string>.Failure("unit not found");

        unit.Update(request.name, request.code, string.IsNullOrWhiteSpace(request.parentId) ? null : Guid.Parse(request.parentId) );

        unitRepository.Update(unit);


        await unitOfWork.SaveChangesAsync();

        return Result<string>.Succeed("Unit updated successfully");
    }
}
