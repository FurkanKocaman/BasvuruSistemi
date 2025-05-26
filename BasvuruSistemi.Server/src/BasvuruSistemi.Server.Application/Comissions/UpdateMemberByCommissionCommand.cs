using BasvuruSistemi.Server.Domain.Comissions;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Comissions;
public sealed record UpdateCommissionMemberCommand(
    Guid Id,
    string RoleInCommission
    ) : IRequest<Result<string>>;

internal sealed class UpdateMemberByCommissionCommandHandler(
    ICommissionMemberRepository commissionMemberRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UpdateCommissionMemberCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateCommissionMemberCommand request, CancellationToken cancellationToken)
    {
        var commissionMember = await commissionMemberRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if(commissionMember is null)
            return Result<string>.Failure(404, "Commission member not found.");

        commissionMember.UpdateRole(request.RoleInCommission);

        commissionMemberRepository.Update(commissionMember);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Commission member updated successfully.");
    }
}
