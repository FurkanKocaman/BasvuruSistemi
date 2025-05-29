using BasvuruSistemi.Server.Domain.ApplicationEvaluations;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostingEvaluationPipelineStages;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Infrastructure.Services;
public interface IPipelineActivationJob
{
    Task ActivatePipelineStage(Guid pipelineStageId);
    Task DeactivatePipelineStage(Guid pipelineStageId);
}

internal sealed class PipelineActivationJob : IPipelineActivationJob
{
    private readonly IJobPostingEvaluationPipelineStageRepository _pipelineRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationEvaluationRepository _applicationEvaluationRepository;

    public PipelineActivationJob(IJobPostingEvaluationPipelineStageRepository pipelineRepo, IUnitOfWork unitOfWork, IApplicationEvaluationRepository applicationEvaluationRepository)
    {
        _pipelineRepo = pipelineRepo;
        _unitOfWork = unitOfWork;
        _applicationEvaluationRepository = applicationEvaluationRepository;
    }

    public async Task ActivatePipelineStage(Guid pipelineStageId)
    {
        var pipeline = await _pipelineRepo
            .WhereWithTracking(p => p.Id == pipelineStageId)
            .Include(p => p.JobPosting)
                .ThenInclude(p => p.Applications)
            .Include(p => p.ResponsibleCommission)
                .ThenInclude(p => p.CommissionMembers)
            .FirstOrDefaultAsync();
        if (pipeline != null)
        {
            if (pipeline is null)
                return;

            var commission = pipeline.ResponsibleCommission;

            if (commission != null)
            {
                foreach (var application in pipeline.JobPosting.Applications)
                {
                    var members = commission.CommissionMembers;
                    foreach (var member in members)
                    {
                        var applicationEvaluationExist = await _applicationEvaluationRepository.AnyAsync(p => p.ApplicationId == application.Id && p.EvaluatorId == member.Id && p.JobPostingEvaluationPipelineStageId == pipeline.Id);
                        if (!applicationEvaluationExist)
                        {
                            var applicationEvaluation = new ApplicationEvaluation(application.Id, pipeline.Id, member.UserId, EvaluationStatus.Pending, null);
                            _applicationEvaluationRepository.Add(applicationEvaluation);
                        }
                      
                    }
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task DeactivatePipelineStage(Guid pipelineStageId)
    {
        var pipeline = await _pipelineRepo
            .WhereWithTracking(p => p.Id == pipelineStageId)
            .Include(p => p.JobPosting)
                .ThenInclude(p => p.Applications)
            .Include(p => p.ResponsibleCommission)
                .ThenInclude(p => p.CommissionMembers)
            .FirstOrDefaultAsync();
        if (pipeline != null)
        {
            var commission = pipeline.ResponsibleCommission;

            if (commission != null)
            {
                foreach (var application in pipeline.JobPosting.Applications)
                {
                    var members = commission.CommissionMembers;
                    foreach (var member in members)
                    {
                        var applicationEvaluations = await _applicationEvaluationRepository.WhereWithTracking(p => p.ApplicationId == application.Id && p.JobPostingEvaluationPipelineStageId == pipeline.Id && p.EvaluatorId == member.UserId).ToListAsync();
                        foreach (var applicationEvaluation in applicationEvaluations)
                        {
                            applicationEvaluation.SetActive(false);
                            _applicationEvaluationRepository.Update(applicationEvaluation);
                        }
                    }
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }


}