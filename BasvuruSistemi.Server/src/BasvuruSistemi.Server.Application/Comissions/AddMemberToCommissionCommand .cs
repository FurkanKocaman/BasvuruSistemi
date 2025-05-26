using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Comissions;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Comissions;
public sealed record AddMemberToCommissionCommand(
    Guid CommissionId,
    Guid UserId,
    string Role
    ) : IRequest<Result<string>>;

internal sealed class AddMemberToCommissionCommandHandler(
    ICommissionMemberRepository commissionMemberRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService
    ) : IRequestHandler<AddMemberToCommissionCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AddMemberToCommissionCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
        {
            return Result<string>.Failure(401, "user ID is not set.");
        }

        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
        {
            return Result<string>.Failure(401,"Tenant ID is not set.");
        }

        var commissionMember = new CommissionMember(
            request.CommissionId,
            request.UserId,
            request.Role,
            tenantId.Value,
            userId.Value
        );
        commissionMemberRepository.Add(commissionMember);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Member added to commission successfully.");
    }
}
