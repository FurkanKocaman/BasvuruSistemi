using BasvuruSistemi.Server.Domain.ApplicationEvaluations;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostingEvaluationPipelineStages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.ApplicationEvaluations;
public sealed record GetApplicationEvaluationProcessQuery(
    Guid ApplicationId
    ) : IRequest<Result<List<GetApplicationEvaluationProcessQueryResponse>>>;

public sealed class GetApplicationEvaluationProcessQueryResponse
{
    public Guid StageId { get; set; }
    public string StageName { get; set; } = default!;

    public Guid? EvaluationFormId { get; set; }
    public string? EvaluationFormName { get; set; }

    public Guid? CommissionId { get; set; }
    public string? CommissionName { get; set; }
    
    public List<ApplicationEvaluationSummary> CommissionEvaluationSummaries { get; set; } = new List<ApplicationEvaluationSummary>();
}

public sealed class ApplicationEvaluationSummary
{
    public Guid? UserId { get; set; }
    public string? UserName { get; set; } = default!;

    public EvaluationStatus Status { get; set; }

    public string? StatusDescription { get; set; }
}

internal sealed class GetApplicationEvaluationProcessQueryResponseHandler(
    IApplicationRepository applicationRepository,
    IApplicationEvaluationRepository applicationEvaluationRepository,
    IJobPostingEvaluationPipelineStageRepository jobPostingEvaluationPipelineStageRepository
    ) : IRequestHandler<GetApplicationEvaluationProcessQuery, Result<List<GetApplicationEvaluationProcessQueryResponse>>>
{
    public async Task<Result<List<GetApplicationEvaluationProcessQueryResponse>>> Handle(GetApplicationEvaluationProcessQuery request, CancellationToken cancellationToken)
    {
        var application = await applicationRepository.FirstOrDefaultAsync(p => p.Id == request.ApplicationId,cancellationToken);
        if (application is null)
            Result< List < GetApplicationEvaluationProcessQueryResponse >>.Failure(404, "Application not found");

        var evaluationPipelineStages = await jobPostingEvaluationPipelineStageRepository
                    .Where(p => p.JobPostingId == application!.JobPostingId)
                    .Include(p => p.EvaluationStage)
                    .Include(p => p.EvaluationForm)
                    .Include(p => p.ResponsibleCommission)
                    .ToListAsync(cancellationToken);

        var applicationEvaluations = await applicationEvaluationRepository.Where(p => p.ApplicationId == application!.Id).ToListAsync();

        var response = evaluationPipelineStages.Select(pipelineStage => new GetApplicationEvaluationProcessQueryResponse
        {
            StageId = pipelineStage.EvaluationStageId,
            StageName = pipelineStage.EvaluationStage.Name,

            EvaluationFormId = pipelineStage.EvaluationFormId,
            EvaluationFormName = pipelineStage.EvaluationForm.Name,

            CommissionId = pipelineStage.ResponsibleCommissionId,
            CommissionName = pipelineStage.ResponsibleCommission.Name,

            CommissionEvaluationSummaries = applicationEvaluations.Where(p => p.JobPostingEvaluationPipelineStageId == pipelineStage.Id ).Select(applicationEvaluation => new ApplicationEvaluationSummary
            {
                UserId = applicationEvaluation.EvaluatorId,
                UserName = applicationEvaluation.Evaluator != null ? applicationEvaluation.Evaluator.FullName : null,

                Status = applicationEvaluation.Status,
                StatusDescription = applicationEvaluation.OverallComment,
            }).ToList(),

        } ).ToList();

        return Result<List<GetApplicationEvaluationProcessQueryResponse>>.Succeed(response);
    }
}
