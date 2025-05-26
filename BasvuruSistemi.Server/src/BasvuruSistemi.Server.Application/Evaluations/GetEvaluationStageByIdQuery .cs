using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Evaluations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Evaluations;
public sealed record GetEvaluationStageByIdQuery(
    Guid Id
    ) : IRequest<Result<EvaluationStageDto>>;

internal sealed class GetEvaluationStageByIdQueryHandler(
    IEvaluationStageRepository evaluationStageRepository
    ) : IRequestHandler<GetEvaluationStageByIdQuery, Result<EvaluationStageDto>>
{
    public async Task<Result<EvaluationStageDto>> Handle(GetEvaluationStageByIdQuery request, CancellationToken cancellationToken)
    {
        var evaluationStage = await evaluationStageRepository
            .Where(p => p.Id == request.Id && !p.IsDeleted)
            .Include(p => p.EvaluationForms)
            .FirstOrDefaultAsync(cancellationToken);

        if(evaluationStage is null)
            return Result<EvaluationStageDto>.Failure(404, "EvaluationStage not found");

        var response = new EvaluationStageDto(
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
            );

        return Result<EvaluationStageDto>.Succeed(response);
    }
}
