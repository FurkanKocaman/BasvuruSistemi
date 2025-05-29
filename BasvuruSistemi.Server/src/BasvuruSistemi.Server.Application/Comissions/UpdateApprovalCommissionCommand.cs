using BasvuruSistemi.Server.Domain.Comissions;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Comissions;
public sealed record UpdateApprovalCommissionCommand(
    Guid Id,
    string Name,
    string? Description
    ) : IRequest<Result<string>>;

internal sealed class UpdateApprovalCommissionCommandHandler(
    IApprovalCommissionRepository approvalCommissionRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UpdateApprovalCommissionCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateApprovalCommissionCommand request, CancellationToken cancellationToken)
    {
        var commission = await approvalCommissionRepository.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, cancellationToken);
        if (commission is null)
            return Result<string>.Failure(404, "Commission not found.");

        commission.Update(request.Name,request.Description);

        approvalCommissionRepository.Update(commission);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Commission updated successfully.");
    }
}

