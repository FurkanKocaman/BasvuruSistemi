using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Evaluations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.EvaluationForms;
public sealed record GetEvaluationFormByIdQuery(
    Guid Id
    ) : IRequest<Result<EvaluationFormDto>>;

internal sealed class GetEvaluationFormByIdQueryHandler(
    IEvaluationFormRepository evaluationFormRepository
    ) : IRequestHandler<GetEvaluationFormByIdQuery, Result<EvaluationFormDto>>
{
    public async Task<Result<EvaluationFormDto>> Handle(GetEvaluationFormByIdQuery request, CancellationToken cancellationToken)
    {
        var evaluationForm = await evaluationFormRepository.Where(p => p.Id == request.Id && !p.IsDeleted).Include(p => p.Fields).FirstOrDefaultAsync(cancellationToken);

        if (evaluationForm is null)
            return Result<EvaluationFormDto>.Failure(404,"EvaluationForm not found");

        var response = new EvaluationFormDto(
            evaluationForm.Id,
            evaluationForm.Name,
            evaluationForm.EvaluationStageId,
            evaluationForm.CreatedAt,
            evaluationForm.Fields
            .Select(f => new EvaluationFormFieldDto(
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
            evaluationForm.Description
        );

        return Result<EvaluationFormDto>.Succeed(response);
    }
}
