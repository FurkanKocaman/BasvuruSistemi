using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using Hangfire;

namespace BasvuruSistemi.Server.Infrastructure.Services;
internal sealed class HangfireJobScheduler(
    IBackgroundJobClient jobs,
    IJobPostingRepository jobPostingRepository,
    IUnitOfWork unitOfWork) : IJobScheduler
{
    public async Task<string> SchedulePublishAsync(Guid jobPostingId, DateTimeOffset publishAt)
    {
        string jobId = jobs.Schedule<IPublishJob>(
            job => job.Execute(jobPostingId),
            publishAt);

        var jobPosting = await jobPostingRepository.FirstOrDefaultAsync(p => p.Id == jobPostingId);
        if(jobPosting is null)
        {
            throw new InvalidOperationException($"Job posting with ID {jobPostingId} not found.");
        }
        jobPosting.HangfireJobId = jobId;
        jobPostingRepository.Update(jobPosting);

        await unitOfWork.SaveChangesAsync();

        return jobId;

    }
    public async Task<string> ReschedulePublishAsync(Guid jobPostingId, DateTimeOffset newPublishAt, string existingJobId)
    {
        jobs.Delete(existingJobId);

        return await SchedulePublishAsync(jobPostingId, newPublishAt);
    }
    public Task<bool> CancelScheduledPublishAsync(string jobId)
    {
        return Task.FromResult(jobs.Delete(jobId));
    }
}
