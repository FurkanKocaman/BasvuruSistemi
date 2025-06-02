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
                .ThenInclude(p => p.Fields)
            .FirstOrDefaultAsync(cancellationToken);

        if(evaluationStage is null)
            return Result<EvaluationStageDto>.Failure(404, "EvaluationStage not found");

        var response = new EvaluationStageDto(
            evaluationStage.Id,
            evaluationStage.Name,
            evaluationStage.Description,
            evaluationStage.Order,
            evaluationStage.DefaultEvaluationFormId,
            evaluationStage.EvaluationForms?.Where(p => !p.IsDeleted).Select(f => new EvaluationFormDto(
                f.Id,
                f.Name,
                f.EvaluationStageId,
                f.CreatedAt,
                f.Fields.Select(f => new EvaluationFormFieldDto(
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
                )).ToList(),
                f.Description
            )).ToList(),
            evaluationStage.CreatedAt
            );

        return Result<EvaluationStageDto>.Succeed(response);
    }
}
