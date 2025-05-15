using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using MediatR;

namespace BasvuruSistemi.Server.Application.FormTemplates;
public sealed record GetFormTemplateSummariesQuery(
    ) : IRequest<List<GetFormTemplateSummariesQueryResponse>>;

public sealed class GetFormTemplateSummariesQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}

internal sealed class GetFormTemplateSummariesQueryHandler(
    ICurrentUserService currentUserService,
    IFormTemplateRepository formTemplateRepository
    ) : IRequestHandler<GetFormTemplateSummariesQuery, List<GetFormTemplateSummariesQueryResponse>>
{
    public Task<List<GetFormTemplateSummariesQueryResponse>> Handle(GetFormTemplateSummariesQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if(!tenantId.HasValue)
            return Task.FromResult(new List<GetFormTemplateSummariesQueryResponse>());

        var formTemplates = formTemplateRepository.Where(p => p.TenantId == tenantId && !p.IsDeleted && p.IsSaved).ToList();

        var response = formTemplates.Select(formTemplate => new GetFormTemplateSummariesQueryResponse
        {
            Id = formTemplate.Id,
            Name = formTemplate.Name,
        }).ToList();

        return Task.FromResult(response);
    }
}
