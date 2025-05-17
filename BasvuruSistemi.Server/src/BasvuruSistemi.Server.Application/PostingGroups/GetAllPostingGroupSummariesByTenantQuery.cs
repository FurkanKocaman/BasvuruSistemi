using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.PostingGroups;
using MediatR;

namespace BasvuruSistemi.Server.Application.PostingGroups;
public sealed record GetAllPostingGroupSummariesByTenantQuery(
    ) : IRequest<List<GetAllPostingGroupSummariesByTenantQueryResponse>>;

public sealed class GetAllPostingGroupSummariesByTenantQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}

internal sealed class GetAllPostingGroupSummariesByTenantQueryHandler(
    ICurrentUserService currentUserService,
    IPostingGroupRepository postingGroupRepository
    ) : IRequestHandler<GetAllPostingGroupSummariesByTenantQuery, List<GetAllPostingGroupSummariesByTenantQueryResponse>>
{
    public Task<List<GetAllPostingGroupSummariesByTenantQueryResponse>> Handle(GetAllPostingGroupSummariesByTenantQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if(!tenantId.HasValue)
            return Task.FromResult(new List<GetAllPostingGroupSummariesByTenantQueryResponse>());

        var postingGroups = postingGroupRepository
            .Where(p => !p.IsDeleted & p.TenantId == tenantId.Value)
            .Select(p => new GetAllPostingGroupSummariesByTenantQueryResponse
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToList();

        return Task.FromResult(postingGroups);
    }
}