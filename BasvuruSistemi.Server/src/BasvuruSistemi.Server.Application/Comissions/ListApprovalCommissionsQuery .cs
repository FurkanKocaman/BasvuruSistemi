using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Comissions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Comissions;
public sealed record ListApprovalCommissionsQuery(
    ) : IRequest<Result<List<GetApprovalCommissionByIdQueryResponse>>>;

internal sealed class ListApprovalCommissionsQueryHandler(
    ICurrentUserService currentUserService,
    IApprovalCommissionRepository approvalCommissionRepository
    ) : IRequestHandler<ListApprovalCommissionsQuery, Result<List<GetApprovalCommissionByIdQueryResponse>>>
{
    public async Task<Result<List<GetApprovalCommissionByIdQueryResponse>>> Handle(ListApprovalCommissionsQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if(!tenantId.HasValue)
            return Result<List<GetApprovalCommissionByIdQueryResponse>>.Failure(403, "Tenant not found.");

        var commissions = await approvalCommissionRepository.Where(p => p.TenantId == tenantId.Value && !p.IsDeleted)
            .Include(p => p.CommissionMembers)
                .ThenInclude(m => m.User)
            .ToListAsync(cancellationToken);

        var response = commissions.Select(commission => new GetApprovalCommissionByIdQueryResponse
        {
            Id = commission.Id,
            Name = commission.Name,
            Description = commission.Description,
            CommissionMembers = commission.CommissionMembers.Where(p => !p.IsDeleted).Select(m => new GetCommissionMembersQueryResponse
            {
                Id = m.Id,
                UserId = m.UserId,
                FullName = m.User.FullName,
                Email = m.User.Email,
                RoleInCommission = m.RoleInCommission,
                IsManager = m.IsManager,
                CreatedAt = m.CreatedAt
            }).ToList(),
            CreatedAt = commission.CreatedAt,
            CreateUserId = commission.CreateUserId,
            CreateUserName = "",
            UpdateAt = commission.UpdateAt,
            UpdateUserId = commission.UpdateUserId,
            UpdateUserName = "",
            DeleteAt = commission.DeleteAt,
            IsDeleted = commission.IsDeleted,
            IsActive = commission.IsActive,

        }).ToList();
        return Result<List<GetApprovalCommissionByIdQueryResponse>>.Succeed(response);
    }
}
