using BasvuruSistemi.Server.Domain.Evaluations;
using BasvuruSistemi.Server.Domain.JobPostingEvaluationPipelineStages;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Evaluations;
public sealed record DeleteEvaluationStageCommand(
    Guid Id
    ) : IRequest<Result<string>>;

internal sealed class DeleteEvaluationStageCommandHandler(
    IEvaluationStageRepository evaluationStageRepository,
    IEvaluationFormRepository evaluationFormRepository,
    IEvaluationFormFieldDefinitionRepository evaluationFormFieldDefinitionRepository,
    IUnitOfWork unitOfWork,
    IJobPostingEvaluationPipelineStageRepository jobPostingEvaluationPipelineStageRepository
    ) : IRequestHandler<DeleteEvaluationStageCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteEvaluationStageCommand request, CancellationToken cancellationToken)
    {
        var isPipelineExist = await jobPostingEvaluationPipelineStageRepository.AnyAsync(p => p.EvaluationStageId == request.Id,cancellationToken);

        if (isPipelineExist)
            return Result<string>.Failure(400, "You neede to delete jobPosting before deleet stage");

        var evaluationStage = await evaluationStageRepository
            .Where(p => p.Id == request.Id)
            .Include(p => p.EvaluationForms)
                .ThenInclude(p => p.Fields)
            .FirstOrDefaultAsync(cancellationToken);

        if (evaluationStage is null)
            return Result<string>.Failure(404, "Evaluation stage not found");

        evaluationStage.IsDeleted = true;
        evaluationStageRepository.Update(evaluationStage);

        foreach(var form in evaluationStage.EvaluationForms)
        {
            if(form is not null)
            {
                foreach(var field in form.Fields)
                {
                    if(field is not null)
                    {
                        evaluationFormFieldDefinitionRepository.Delete(field);
                    }
                }
                form.IsDeleted = true;
                evaluationFormRepository.Update(form);
            }
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Stage deleted successfully");

    }
}
