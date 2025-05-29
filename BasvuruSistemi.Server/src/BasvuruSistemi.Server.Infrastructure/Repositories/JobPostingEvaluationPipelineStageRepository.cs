using BasvuruSistemi.Server.Domain.JobPostingEvaluationPipelineStages;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class JobPostingEvaluationPipelineStageRepository : Repository<JobPostingEvaluationPipelineStage, ApplicationDbContext>, IJobPostingEvaluationPipelineStageRepository
{
    public JobPostingEvaluationPipelineStageRepository(ApplicationDbContext context) : base(context)
    {
    }
}
