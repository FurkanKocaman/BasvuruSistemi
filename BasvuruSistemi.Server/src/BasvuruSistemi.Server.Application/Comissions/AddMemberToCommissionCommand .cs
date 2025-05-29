using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Comissions;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using BasvuruSistemi.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Comissions;
public sealed record AddMemberToCommissionCommand(
    Guid CommissionId,
    string Email,
    string Role,
    bool IsManager
    ) : IRequest<Result<string>>;

internal sealed class AddMemberToCommissionCommandHandler(
    ICommissionMemberRepository commissionMemberRepository,
    UserManager<AppUser> userManager,
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

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Result<string>.Failure(404, "User not found");

        var isMemberExists = await commissionMemberRepository.AnyAsync(
            p => p.CommissionId == request.CommissionId && p.UserId == user.Id && !p.IsDeleted,
            cancellationToken
        );
        if (isMemberExists)
            return Result<string>.Failure(409, "User is already a member of this commission.");

        var commissionMember = new CommissionMember(
            request.CommissionId,
            user.Id,
            request.Role,
            request.IsManager,
            tenantId.Value,
            userId.Value
        );
        commissionMemberRepository.Add(commissionMember);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Member added to commission successfully.");
    }
}
