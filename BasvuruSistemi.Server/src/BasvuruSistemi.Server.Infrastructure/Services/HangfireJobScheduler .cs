using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.JobPostingEvaluationPipelineStages;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Infrastructure.Services;
internal sealed class HangfireJobScheduler(
    IBackgroundJobClient jobs,
    IJobPostingRepository jobPostingRepository,
    IJobPostingEvaluationPipelineStageRepository jobPostingEvaluationPipelineStageRepository,
    IUnitOfWork unitOfWork) : IJobScheduler
{
    public async Task<string> SchedulePublishAsync(Guid jobPostingId, DateTimeOffset publishAt)
    {
        string jobId = jobs.Schedule<IPublishJob>(
            job => job.Publish(jobPostingId),
            publishAt);

        var jobPosting = await jobPostingRepository.FirstOrDefaultAsync(p => p.Id == jobPostingId);
        if(jobPosting is null)
        {
            throw new InvalidOperationException($"Job posting with ID {jobPostingId} not found.");
        }
        jobPosting.HangfirePublishJobId = jobId;
        jobPostingRepository.Update(jobPosting);

        await unitOfWork.SaveChangesAsync();

        return jobId;

    }
    public async Task<string> ScheduleCloseAsync(Guid jobPostingId, DateTimeOffset publishAt)
    {
        string jobId = jobs.Schedule<IPublishJob>(
           job => job.Close(jobPostingId),
           publishAt);

        var jobPosting = await jobPostingRepository.FirstOrDefaultAsync(p => p.Id == jobPostingId);
        if (jobPosting is null)
        {
            throw new InvalidOperationException($"Job posting with ID {jobPostingId} not found.");
        }
        jobPosting.HangfireCloseJobId = jobId;
        jobPostingRepository.Update(jobPosting);

        await unitOfWork.SaveChangesAsync();

        return jobId;

    }
    public async Task<string> ReschedulePublishAsync(Guid jobPostingId, DateTimeOffset newPublishAt, string existingJobId)
    {
        jobs.Delete(existingJobId);

        return await SchedulePublishAsync(jobPostingId, newPublishAt);
    }

    public async Task<string> RescheduleCloseAsync(Guid jobPostingId, DateTimeOffset newPublishAt, string existingJobId)
    {
        jobs.Delete(existingJobId);

        return await ScheduleCloseAsync(jobPostingId, newPublishAt);
    }

    public Task<bool> CancelScheduleAsync(string jobId)
    {
        return Task.FromResult(jobs.Delete(jobId));
    }

    public async Task<bool> ScheduleInReviewApplications(Guid jobPostingId)
    {
        var jobPosting = await jobPostingRepository.WhereWithTracking(p => p.Id == jobPostingId).Include(p => p.EvaluationPipelineStages).FirstOrDefaultAsync();

        if (jobPosting is null)
            return false;

        Console.WriteLine(jobPosting.Title);

        var pipelineEvaluationStages = await jobPostingEvaluationPipelineStageRepository.WhereWithTracking(p => p.JobPostingId == jobPostingId && !p.IsDeleted).OrderBy(p => p.OrderInPipeline).ToListAsync();
            

        if (pipelineEvaluationStages[0] is not null && pipelineEvaluationStages[0].StartDate is not null)
        {
            jobs.Schedule<IInReviewApplications>(job => job.Execute(jobPostingId), pipelineEvaluationStages[0].StartDate!.Value);
        }

        foreach (var pipeline in pipelineEvaluationStages)
        {
            if(pipeline.StartDate is not null)
            {
                var jobId = jobs.Schedule<IPipelineActivationJob>(job => job.ActivatePipelineStage(pipeline.Id), pipeline.StartDate.Value);
                pipeline.HangfireStartJobId = jobId;

                jobPostingEvaluationPipelineStageRepository.Update(pipeline);
                await unitOfWork.SaveChangesAsync();
            }

            if (pipeline.EndDate is not null)
            {
                var jobId = jobs.Schedule<IPipelineActivationJob>(job => job.DeactivatePipelineStage(pipeline.Id), pipeline.EndDate.Value);
                pipeline.HangfireEndJobId = jobId;

                jobPostingEvaluationPipelineStageRepository.Update(pipeline);
                await unitOfWork.SaveChangesAsync();
            }
        }

        return true;
    }
}
