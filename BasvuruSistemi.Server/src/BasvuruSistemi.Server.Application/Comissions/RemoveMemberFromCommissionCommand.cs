using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Comissions;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using System.Diagnostics.CodeAnalysis;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Comissions;
public sealed record RemoveMemberFromCommissionCommand(
    Guid UserId,
    Guid CommissionId
    ) : IRequest<Result<string>>;


internal sealed class RemoveMemberFromCommissionCommandHandler(
    ICurrentUserService currentUserService,
    ICommissionMemberRepository commissionMemberRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<RemoveMemberFromCommissionCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RemoveMemberFromCommissionCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<string>.Failure(401, "Unauthorized");

        var userCommissionMember = await commissionMemberRepository.FirstOrDefaultAsync(p => p.UserId == userId.Value);
        if (userCommissionMember is null || !userCommissionMember.IsManager)
            return Result<string>.Failure(403, "Access failed");

        var commissionMember = await commissionMemberRepository.FirstOrDefaultAsync(p => p.UserId == request.UserId && p.CommissionId == request.CommissionId);

        if (commissionMember is null)
            return Result<string>.Failure(404, "Not found");

        commissionMember.IsDeleted = true;

        commissionMemberRepository.Update( commissionMember );

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Removed Successfully");
    }
}
