using BasvuruSistemi.Server.Domain.Evaluations;
using BasvuruSistemi.Server.Domain.JobPostingEvaluationPipelineStages;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.EvaluationForms;
public sealed record DeleteEvaluationFormCommand(
    Guid Id
    ) : IRequest<Result<string>>;

internal sealed class DeleteEvaluationFormCommandHandler(
    IEvaluationFormRepository evaluationFormRepository,
    IEvaluationFormFieldDefinitionRepository evaluationFormFieldDefinitionRepository,
    IJobPostingEvaluationPipelineStageRepository jobPostingEvaluationPipelineStageRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteEvaluationFormCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteEvaluationFormCommand request, CancellationToken cancellationToken)
    {
        var evaluationForm = await evaluationFormRepository.Where(p => p.Id == request.Id).Include(p => p.Fields).FirstOrDefaultAsync(cancellationToken);

        if (evaluationForm is null)
            return Result<string>.Failure(404, "Form not found");

        var isPipelineExist = await jobPostingEvaluationPipelineStageRepository.AnyAsync(p => p.EvaluationFormId == request.Id && !p.IsDeleted);

        if (isPipelineExist)
            return Result<string>.Failure(400, "This form is using an evaluation for delete this form you need to delete jobPosting before");

        evaluationForm.IsDeleted = true;
        evaluationFormRepository.Update(evaluationForm);

        var fields = evaluationForm.Fields;

        foreach ( var field in fields)
        {
            evaluationFormFieldDefinitionRepository.Delete(field);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Form delete successfully");
    }
}
