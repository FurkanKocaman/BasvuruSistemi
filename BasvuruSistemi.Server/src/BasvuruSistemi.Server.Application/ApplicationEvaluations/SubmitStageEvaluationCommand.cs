using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostingEvaluationPipelineStages;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.ApplicationEvaluations;
public sealed record SubmitStageEvaluationCommand(
    Guid EvaluationPipelineStageId,
    int Status,
    string? Comment
    ) : IRequest<Result<string>>;

internal sealed class SubmitStageEvaluationCommandHandler(
    IJobPostingEvaluationPipelineStageRepository jobPostingEvaluationPipelineStageRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService
    ) : IRequestHandler<SubmitStageEvaluationCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SubmitStageEvaluationCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<string>.Failure(401, "Unauthorized");

        var evaluationPipelineStage = await jobPostingEvaluationPipelineStageRepository
                    .Where(p => p.Id == request.EvaluationPipelineStageId)
                    .Include(p => p.ResponsibleCommission)
                        .ThenInclude(p => p.CommissionMembers)
                    .FirstOrDefaultAsync(cancellationToken);

        if (evaluationPipelineStage is null)
            return Result<string>.Failure(404, "Stage not found");

        if (!evaluationPipelineStage.ResponsibleCommission.CommissionMembers.Any(p => p.UserId == userId.Value && p.IsManager && !p.IsDeleted))
            return Result<string>.Failure(400, "You aren't manager of commission");

        if (!System.Enum.IsDefined(typeof(EvaluationStatus), request.Status))
        {
            return Result<string>.Failure($"Invalid JobPostingStatus: {request.Status}.");
        }
        EvaluationStatus status = (EvaluationStatus)request.Status;

        evaluationPipelineStage.SubmitEvaluation(status, request.Comment);

        jobPostingEvaluationPipelineStageRepository.Update(evaluationPipelineStage);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Successfully submitted");
    }
}
