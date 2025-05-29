namespace BasvuruSistemi.Server.Application.Services;
public interface IJobScheduler
{
    /// <summary>
    /// Schedules the job that will publish the ad on the specified date.
    /// </summary>
    Task<string> SchedulePublishAsync(Guid adId, DateTimeOffset publishAt);

    Task<string> ScheduleCloseAsync(Guid adId, DateTimeOffset publishAt);

    /// <summary> 
    /// Cancels the existing job and schedules a new one.
    /// </summary>
    Task<string> ReschedulePublishAsync(Guid adId, DateTimeOffset newPublishAt, string existingJobId);

    Task<string> RescheduleCloseAsync(Guid adId, DateTimeOffset newPublishAt, string existingJobId);

    /// <summary>
    ///Deletes the existing job from Hangfire.
    /// </summary>
    Task<bool> CancelScheduleAsync(string jobId);
    Task<bool> ScheduleInReviewApplications(Guid jobPostingId);
}
