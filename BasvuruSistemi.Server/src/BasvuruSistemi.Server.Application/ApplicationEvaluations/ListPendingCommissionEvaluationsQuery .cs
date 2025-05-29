using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.ApplicationEvaluations;
using BasvuruSistemi.Server.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.ApplicationEvaluations;
public sealed record ListPendingCommissionEvaluationsQuery(
    Guid? JobPostinId,
    Guid? EvaluationStageId
    ) : IRequest<Result<List<ListPendingCommissionEvaluationsQueryResponse>>>;

public sealed class ListPendingCommissionEvaluationsQueryResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public string UserFullName { get; set; } = default!;
    public string? TCKN { get; set; }

    public Guid JobPostingId { get; set; }
    public string JobPosting { get; set; } = default!;
    public DateTimeOffset AppliedDate { get; set; }
    public EvaluationStatus Status { get; set; } = default!;
    public DateTimeOffset? ReviewDate { get; set; }
    public string? ReviewDescription { get; set; }

    public Guid? StageId { get; set; }
    public string? StageName { get; set; }

    public Guid? EvaluationFormId { get; set; }
    public string? EvaluationFormName { get; set; }

    public Guid? EvaluationPipelineStageId { get; set; }
}

internal sealed class ListPendingCommissionEvaluationsQueryHandler(
    ICurrentUserService currentUserService,
    IApplicationEvaluationRepository applicationEvaluationRepository
    ) : IRequestHandler<ListPendingCommissionEvaluationsQuery, Result<List<ListPendingCommissionEvaluationsQueryResponse>>>
{
    public async Task<Result<List<ListPendingCommissionEvaluationsQueryResponse>>> Handle(ListPendingCommissionEvaluationsQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<List<ListPendingCommissionEvaluationsQueryResponse>>.Failure(401,"Unauthorized");

        var applicationEvaluations = await applicationEvaluationRepository
            .Where(p => p.EvaluatorId == userId.Value && !p.IsDeleted && p.IsActive)
            .Include(p => p.Application)
                .ThenInclude(p => p.User)
            .Include(p => p.Application)
                .ThenInclude(p => p.JobPosting)
            .Include(p => p.JobPostingEvaluationPipelineStage)
                .ThenInclude(p => p.EvaluationStage)
            .Include(p => p.JobPostingEvaluationPipelineStage)
                .ThenInclude(p => p.EvaluationForm)
            .ToListAsync(cancellationToken);

        if(request.JobPostinId != null)
        {
            applicationEvaluations = applicationEvaluations.Where(p => p.JobPostingEvaluationPipelineStage.JobPostingId == request.JobPostinId).ToList();
        }

        if (request.EvaluationStageId != null)
        {
            applicationEvaluations = applicationEvaluations.Where(p => p.JobPostingEvaluationPipelineStage.EvaluationStageId == request.EvaluationStageId).ToList();
        }

        var response = applicationEvaluations.Select(applicationEvaluation => new ListPendingCommissionEvaluationsQueryResponse
        {
            Id = applicationEvaluation.Id,

            UserId = applicationEvaluation.Application.UserId,
            UserFullName = applicationEvaluation.Application.User.FullName,
            TCKN = applicationEvaluation.Application.User.TCKN,

            JobPostingId = applicationEvaluation.Application.JobPostingId,
            JobPosting = applicationEvaluation.Application.JobPosting.Title,
            AppliedDate = applicationEvaluation.Application.AppliedDate,
            Status = applicationEvaluation != null ? applicationEvaluation.Status : EvaluationStatus.Pending,
            ReviewDate = applicationEvaluation != null ? applicationEvaluation.EvaluationDate : null,
            ReviewDescription = applicationEvaluation != null ? applicationEvaluation.OverallComment : null,

            StageId = applicationEvaluation?.JobPostingEvaluationPipelineStage.EvaluationStageId,
            StageName = applicationEvaluation?.JobPostingEvaluationPipelineStage.EvaluationStage.Name,

            EvaluationFormId = applicationEvaluation?.JobPostingEvaluationPipelineStage.EvaluationFormId,
            EvaluationFormName = applicationEvaluation?.JobPostingEvaluationPipelineStage.EvaluationForm.Name,

            EvaluationPipelineStageId = applicationEvaluation?.JobPostingEvaluationPipelineStage.Id,
        }).ToList();

        //var commissions = await commissionMemberRepository.Where(p => p.UserId == userId.Value).Select(p => p.CommissionId).ToListAsync(cancellationToken);

        //var jobPostinEvaluationPipelines = await jobPostingEvaluationPipelineStageRepository
        //    .Where(p => commissions.Contains(p.ResponsibleCommissionId) && !p.IsDeleted && p.IsActive)
        //    .Include(p => p.EvaluationStage)
        //    .Include(p => p.EvaluationForm)
        //    .Include(p => p.JobPosting)
        //        .ThenInclude(p => p.Applications)
        //        .ThenInclude(p => p.User)
        //    .ToListAsync(cancellationToken);

        //var applicationEvaluations = await applicationEvaluationRepository.Where(p => p.EvaluatorId == userId.Value).ToListAsync(cancellationToken);

        //var response = jobPostinEvaluationPipelines
        //    .SelectMany(evaluation => evaluation.JobPosting.Applications.Where(application => application.Status == ApplicationStatus.InReview).Select(application =>
        //    {
        //        var applicationEvaluation = applicationEvaluations.FirstOrDefault(p => p.ApplicationId == application.Id);
        //        return new ListPendingCommissionEvaluationsQueryResponse
        //        {
        //            Id = application.Id,

        //            UserId = application.UserId,
        //            UserFullName = application.User.FullName,
        //            TCKN = application.User.TCKN,

        //            JobPosting = application.JobPosting.Title,
        //            AppliedDate = application.AppliedDate,
        //            Status = applicationEvaluation != null ? applicationEvaluation.Status : EvaluationStatus.Pending,
        //            ReviewDate = applicationEvaluation != null ? applicationEvaluation.EvaluationDate : null,
        //            ReviewDescription = applicationEvaluation != null ? applicationEvaluation.OverallComment : null,

        //            StageId = evaluation.EvaluationStageId,
        //            StageName = evaluation.EvaluationStage.Name,

        //            EvaluationFormId = evaluation.EvaluationFormId,
        //            EvaluationFormName = evaluation.EvaluationForm.Name,

        //            EvaluationPipelineStageId = evaluation.Id,
        //        };
        //    })).ToList();

        return Result<List<ListPendingCommissionEvaluationsQueryResponse>>.Succeed(response);
    }
}
