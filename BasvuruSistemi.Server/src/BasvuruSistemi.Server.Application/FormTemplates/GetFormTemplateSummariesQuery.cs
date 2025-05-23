using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.FormTemplates;
public sealed record GetFormTemplateSummariesQuery(
    ) : IRequest<Result<List<GetFormTemplateSummariesQueryResponse>>>;

public sealed class GetFormTemplateSummariesQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}

internal sealed class GetFormTemplateSummariesQueryHandler(
    ICurrentUserService currentUserService,
    IFormTemplateRepository formTemplateRepository
    ) : IRequestHandler<GetFormTemplateSummariesQuery, Result<List<GetFormTemplateSummariesQueryResponse>>>
{
    public async Task<Result<List<GetFormTemplateSummariesQueryResponse>>> Handle(GetFormTemplateSummariesQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return Result<List<GetFormTemplateSummariesQueryResponse>>.Failure(401, "Unauthorized");

        var formTemplates = await formTemplateRepository
            .Where(p => p.TenantId == tenantId.Value && !p.IsDeleted && p.IsSaved)
            .ToListAsync(cancellationToken);

        var response = formTemplates
            .Select(formTemplate => new GetFormTemplateSummariesQueryResponse
            {
                Id = formTemplate.Id,
                Name = formTemplate.Name,
            })
            .ToList();

        return Result<List<GetFormTemplateSummariesQueryResponse>>.Succeed(response);
    }
}
