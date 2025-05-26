using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Evaluations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.EvaluationForms;
public sealed record ListEvaluationFormsByStageQuery(
    Guid EvaluationStageId
    ) : IRequest<Result<List<EvaluationFormDto>>>;

internal sealed class ListEvaluationFormsByStageQueryHandler(
    IEvaluationStageRepository evaluationStageRepository
    ) : IRequestHandler<ListEvaluationFormsByStageQuery, Result<List<EvaluationFormDto>>>
{
    public async Task<Result<List<EvaluationFormDto>>> Handle(ListEvaluationFormsByStageQuery request, CancellationToken cancellationToken)
    {
        var evaluationStage = await evaluationStageRepository
            .Where(p => p.Id == request.EvaluationStageId)
            .Include(p => p.EvaluationForms)
            .FirstOrDefaultAsync(cancellationToken);

        if(evaluationStage is null)
            return Result<List<EvaluationFormDto>>.Failure(404, "EvaluationStage not found");

        var response = evaluationStage.EvaluationForms.Select(EvaluationForms => new EvaluationFormDto(
            EvaluationForms.Id,
            EvaluationForms.Name,
            EvaluationForms.EvaluationStageId,
            EvaluationForms.CreatedAt,
            EvaluationForms.Fields.Select(field => new EvaluationFormFieldDto(
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
            EvaluationForms.Description
        )).ToList();

        return Result<List<EvaluationFormDto>>.Succeed(response);
    }
}
