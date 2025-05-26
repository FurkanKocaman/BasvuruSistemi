using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Evaluations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Evaluations;
public sealed record ListEvaluationStagesQuery(
    ) : IRequest<Result<List<EvaluationStageDto>>>;

internal sealed class ListEvaluationStagesQueryHandler(
    IEvaluationStageRepository evaluationStageRepository,
    ICurrentUserService currentUserService
    ) : IRequestHandler<ListEvaluationStagesQuery, Result<List<EvaluationStageDto>>>
{
    public async Task<Result<List<EvaluationStageDto>>> Handle(ListEvaluationStagesQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if(!tenantId.HasValue)
            return Result<List<EvaluationStageDto>>.Failure(401, "Tenant ID is not set.");

        var evaluationStages = await evaluationStageRepository.Where(p => p.TenantId == tenantId.Value && !p.IsDeleted)
            .Include(p => p.EvaluationForms)
            .ToListAsync(cancellationToken);

        var response = evaluationStages.Select(evaluationStage => new EvaluationStageDto(
            evaluationStage.Id,
            evaluationStage.Name,
            evaluationStage.Description,
            evaluationStage.Order,
            evaluationStage.DefaultEvaluationFormId,
            evaluationStage.EvaluationForms?.Select(f => new EvaluationFormDto(
                f.Id,
                f.Name,
                f.EvaluationStageId,
                f.CreatedAt,
                f.Fields.Select(field => new EvaluationFormFieldDto(
                    field.Id,
                    field.EvaluationFormId,
                    field.FieldType,
                    field.Label,
                    field.Options,
                    field.IsRequired,
                    field.Order,
                    field.Placeholder,
                    field.HelpText,
                    field.ValidationRules
                )).ToList(),
                f.Description
            )).ToList(),
            evaluationStage.CreatedAt
        )).ToList();

        return Result<List<EvaluationStageDto>>.Succeed(response);
    }
}
