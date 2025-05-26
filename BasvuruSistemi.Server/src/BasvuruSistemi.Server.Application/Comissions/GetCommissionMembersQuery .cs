using BasvuruSistemi.Server.Domain.Comissions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Comissions;
public sealed record GetCommissionMembersQuery(
    Guid CommissionId
    ) : IRequest<Result<List<GetCommissionMembersQueryResponse>>>;

public sealed class GetCommissionMembersQueryResponse
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = default!;
    public string RoleInCommission { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
}

internal sealed class GetCommissionMembersQueryHandler(
    ICommissionMemberRepository commissionMemberRepository
    ) : IRequestHandler<GetCommissionMembersQuery, Result<List<GetCommissionMembersQueryResponse>>>
{
    public async Task<Result<List<GetCommissionMembersQueryResponse>>> Handle(GetCommissionMembersQuery request, CancellationToken cancellationToken)
    {
        var commissionMembers = await commissionMemberRepository.Where(p => p.CommissionId == request.CommissionId && !p.IsDeleted)
            .Include(p => p.User)
            .ToListAsync(cancellationToken);

        if (commissionMembers is null || !commissionMembers.Any())
            return Result<List<GetCommissionMembersQueryResponse>>.Failure(404, "Commission not found.");

        var response = commissionMembers.Select(commissionMember => new GetCommissionMembersQueryResponse
        {
            UserId = commissionMember.UserId,
            FullName = commissionMember.User.FullName,
            RoleInCommission = commissionMember.RoleInCommission,
            CreatedAt = commissionMember.CreatedAt
        }).ToList();

        return Result<List<GetCommissionMembersQueryResponse>>.Succeed(response);
    }
}