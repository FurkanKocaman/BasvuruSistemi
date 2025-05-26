using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Evaluations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.EvaluationForms;
public sealed record GetEvaluationFormWithFieldsQuery(
    Guid Id
    ) : IRequest<Result<EvaluationFormDto>>;

internal sealed class GetEvaluationFormWithFieldsQueryhandler(
    IEvaluationFormRepository evaluationFormRepository
    ) : IRequestHandler<GetEvaluationFormWithFieldsQuery, Result<EvaluationFormDto>>
{
    public async Task<Result<EvaluationFormDto>> Handle(GetEvaluationFormWithFieldsQuery request, CancellationToken cancellationToken)
    {
        var evaluationForm = await evaluationFormRepository.Where(p => p.Id == request.Id).Include(p => p.Fields).FirstOrDefaultAsync(cancellationToken);

        if (evaluationForm is null)
            return Result<EvaluationFormDto>.Failure(404, "EvaluationForm not found");

        var response = new EvaluationFormDto(
            evaluationForm.Id,
            evaluationForm.Name,
            evaluationForm.EvaluationStageId,
            evaluationForm.CreatedAt,
            evaluationForm.Fields
            .Select(f => new EvaluationFormFieldDto(
                f.Id,
                f.EvaluationFormId,
                f.FieldType,
                f.Label,
                f.Options,
                f.IsRequired,
                f.Order,
                f.Placeholder,
                f.HelpText,
                f.ValidationRules
                )).ToList(),
            evaluationForm.Description
        );

        return Result<EvaluationFormDto>.Succeed(response);
    }
}