using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.UnitOfWork;

namespace BasvuruSistemi.Server.Infrastructure.Services;
internal sealed class PublishJob(
    IJobPostingRepository jobPostingRepository,
    IUnitOfWork unitOfWork) : IPublishJob
{
    public async Task Execute(Guid jobPostingId)
    {
        var jobPosting = await jobPostingRepository.FirstOrDefaultAsync(p => p.Id == jobPostingId);
        if (jobPosting is null || jobPosting.Status == Domain.Enums.JobPostingStatus.Published)
            return;

        jobPosting.Publish();
        jobPostingRepository.Update(jobPosting);
        await unitOfWork.SaveChangesAsync();
    }
}
