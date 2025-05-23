using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.PostingGroups;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.PostingGroups;

public sealed record GetAllPostingGroupSummariesByTenantQuery(
) : IRequest<Result<List<GetAllPostingGroupSummariesByTenantQueryResponse>>>;

public sealed class GetAllPostingGroupSummariesByTenantQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}

internal sealed class GetAllPostingGroupSummariesByTenantQueryHandler(
    ICurrentUserService currentUserService,
    IPostingGroupRepository postingGroupRepository
) : IRequestHandler<GetAllPostingGroupSummariesByTenantQuery, Result<List<GetAllPostingGroupSummariesByTenantQueryResponse>>>
{
    public async Task<Result<List<GetAllPostingGroupSummariesByTenantQueryResponse>>> Handle(GetAllPostingGroupSummariesByTenantQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<List<GetAllPostingGroupSummariesByTenantQueryResponse>>.Failure("Tenant bilgisi bulunamadı.");

        var postingGroups = await postingGroupRepository
            .Where(p => !p.IsDeleted && p.TenantId == tenantId.Value)
            .Select(p => new GetAllPostingGroupSummariesByTenantQueryResponse
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToListAsync(cancellationToken);

        return Result<List<GetAllPostingGroupSummariesByTenantQueryResponse>>.Succeed(postingGroups);
    }
}
