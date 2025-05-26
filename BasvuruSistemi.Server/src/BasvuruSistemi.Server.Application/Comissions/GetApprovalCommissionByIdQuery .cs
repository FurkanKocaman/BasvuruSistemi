using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.Comissions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Comissions;
public sealed record GetApprovalCommissionByIdQuery(
    Guid Id
    ) : IRequest<Result<GetApprovalCommissionByIdQueryResponse>>;

public sealed class GetApprovalCommissionByIdQueryResponse : EntityDto
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public List<GetCommissionMembersQueryResponse> CommissionMembers { get; set; } = new List<GetCommissionMembersQueryResponse>();
}

internal sealed class GetApprovalCommissionByIdQueryHandler(
    IApprovalCommissionRepository approvalCommissionRepository
    ) : IRequestHandler<GetApprovalCommissionByIdQuery, Result<GetApprovalCommissionByIdQueryResponse>>
{
    public async Task<Result<GetApprovalCommissionByIdQueryResponse>> Handle(GetApprovalCommissionByIdQuery request, CancellationToken cancellationToken)
    {
        var commission = await approvalCommissionRepository.Where(p => p.Id ==  request.Id && !p.IsDeleted)
            .Include(p => p.CommissionMembers)
                .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(cancellationToken);
        if (commission == null)
            return Result<GetApprovalCommissionByIdQueryResponse>.Failure(404,"Commission not found.");

        var response = new GetApprovalCommissionByIdQueryResponse
        {
            Id = commission.Id,
            Name = commission.Name,
            Description = commission.Description,
            CommissionMembers = commission.CommissionMembers.Select(m => new GetCommissionMembersQueryResponse
            {
                UserId = m.UserId,
                FullName = m.User.FullName,
                Email = m.User.Email,
                RoleInCommission = m.RoleInCommission,
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
        };
        return Result<GetApprovalCommissionByIdQueryResponse>.Succeed(response);
    }
}