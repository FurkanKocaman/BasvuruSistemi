using BasvuruSistemi.Server.Domain.JobPostingEvaluationPipelineStages;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.JobPostingEvaluationPipelineStages;
public sealed record CreateJobPostingEvaluationPipelineStageCommand(
    Guid JobPostingId,
    Guid EvaluationStageId,
    int OrderInPipeline,
    bool IsMandatory,
    Guid? EvaluationFormId,
    DateTimeOffset? StartDate = null,
    DateTimeOffset? EndDate = null
    ) : IRequest<Result<string>>;

internal sealed class CreateJobPostingEvaluationPipelineStageCommandHandler(
    IJobPostingEvaluationPipelineStageRepository jobPostingEvaluationPipelineStageRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CreateJobPostingEvaluationPipelineStageCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateJobPostingEvaluationPipelineStageCommand request, CancellationToken cancellationToken)
    {
        var jobPostingEvaluationPipelineStage = new JobPostingEvaluationPipelineStage(
            request.JobPostingId,
            request.EvaluationStageId,
            request.OrderInPipeline,
            request.IsMandatory,
            request.EvaluationFormId,
            request.StartDate,
            request.EndDate
        );

        jobPostingEvaluationPipelineStageRepository.Add(jobPostingEvaluationPipelineStage);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("İş ilanı değerlendirme aşaması başarıyla oluşturuldu.");
    }
}

