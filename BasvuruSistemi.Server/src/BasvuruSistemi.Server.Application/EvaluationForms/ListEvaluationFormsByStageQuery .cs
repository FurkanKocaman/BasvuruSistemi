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
            .Where(p => p.Id == request.EvaluationStageId && !p.IsDeleted)
            .Include(p => p.EvaluationForms)
            .FirstOrDefaultAsync(cancellationToken);

        if(evaluationStage is null)
            return Result<List<EvaluationFormDto>>.Failure(404, "EvaluationStage not found");

        var response = evaluationStage.EvaluationForms.Where(p => !p.IsDeleted).Select(EvaluationForms => new EvaluationFormDto(
            EvaluationForms.Id,
            EvaluationForms.Name,
            EvaluationForms.EvaluationStageId,
            EvaluationForms.CreatedAt,
            EvaluationForms.Fields.Select(f => new EvaluationFormFieldDto(
                f.Id,
                f.EvaluationFormId,
                f.Label,
                f.Description,
                f.Type,
                f.IsRequired,
                f.Order,
                f.Placeholder,
                f.OptionsJson,
                f.IsReadOnly,
                f.DefaultValue,
                f.AllowedFileTypes,
                f.MaxFileSizeMB
            )).OrderBy(p => p.Order).ToList(),
            EvaluationForms.Description
        )).ToList();

        return Result<List<EvaluationFormDto>>.Succeed(response);
    }
}
