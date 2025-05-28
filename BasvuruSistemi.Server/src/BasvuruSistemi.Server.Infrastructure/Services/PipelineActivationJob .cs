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

    public PipelineActivationJob(IJobPostingEvaluationPipelineStageRepository pipelineRepo, IUnitOfWork unitOfWork)
    {
        _pipelineRepo = pipelineRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task ActivatePipelineStage(Guid pipelineStageId)
    {
        var pipeline = await _pipelineRepo.WhereWithTracking(p => p.Id == pipelineStageId).FirstOrDefaultAsync();
        if (pipeline != null)
        {
            pipeline.SetActive(true);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task DeactivatePipelineStage(Guid pipelineStageId)
    {
        var pipeline = await _pipelineRepo.WhereWithTracking(p => p.Id == pipelineStageId).FirstOrDefaultAsync();
        if (pipeline != null)
        {
            pipeline.SetActive(false);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
