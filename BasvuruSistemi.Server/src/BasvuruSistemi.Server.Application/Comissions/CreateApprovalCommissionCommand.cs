using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Comissions;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Comissions;
public sealed record CreateApprovalCommissionCommand(
    string Name,
    string? Description
    ) : IRequest<Result<string>>;

internal sealed class CreateApprovalCommissionCommandHandler(
    IApprovalCommissionRepository approvalCommissionRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService
    ) : IRequestHandler<CreateApprovalCommissionCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateApprovalCommissionCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
        {
            return Result<string>.Failure(401, "Tenant ID is not set.");
        }

        var commission = new ApprovalCommission(
            request.Name,
            tenantId.Value,
            request.Description
        );
        approvalCommissionRepository.Add(commission);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Commission created successfully.");
    }
}
