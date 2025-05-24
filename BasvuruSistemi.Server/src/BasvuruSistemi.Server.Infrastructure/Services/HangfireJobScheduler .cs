using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.JobPostings;
using Hangfire;

namespace BasvuruSistemi.Server.Infrastructure.Services;
internal sealed class HangfireJobScheduler(
    IBackgroundJobClient jobs,
    IJobPostingRepository jobPostingRepository) : IJobScheduler
{
    public async Task<string> SchedulePublishAsync(Guid adId, DateTimeOffset publishAt)
    {
        throw new NotImplementedException();

    }
    public Task<string> ReschedulePublishAsync(Guid adId, DateTimeOffset newPublishAt, string existingJobId)
    {
        throw new NotImplementedException();
    }
    public Task<bool> CancelScheduledPublishAsync(string jobId)
    {
        throw new NotImplementedException();
    }
}
