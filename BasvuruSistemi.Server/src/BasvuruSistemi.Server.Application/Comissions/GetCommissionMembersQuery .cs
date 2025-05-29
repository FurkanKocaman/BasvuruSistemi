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
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = default!;
    public string? Email { get; set; }
    public string RoleInCommission { get; set; } = default!;
    public bool IsManager { get; set; }
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

        var response = commissionMembers.Where(p => !p.IsDeleted).Select(commissionMember => new GetCommissionMembersQueryResponse
        {
            Id = commissionMember.Id,
            UserId = commissionMember.UserId,
            FullName = commissionMember.User.FullName,
            Email = commissionMember.User.Email,
            RoleInCommission = commissionMember.RoleInCommission,
            IsManager = commissionMember.IsManager,
            CreatedAt = commissionMember.CreatedAt
        }).ToList();

        return Result<List<GetCommissionMembersQueryResponse>>.Succeed(response);
    }
}